using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace MediaPlayer_X_Ark.Engine
{
	/// <summary>
	/// CDトラック情報
	/// </summary>
	public class CdTrackInfo
	{
		public int TrackNumber { get; set; }
		public long StartSector { get; set; }
		public long EndSector { get; set; }
		public long SectorCount => EndSector - StartSector;
		public TimeSpan Duration => TimeSpan.FromSeconds(SectorCount / 75.0);
		public string Title => $"Track {TrackNumber:D2}";
		public string DurationText => Duration.ToString(@"mm\:ss");
	}

	/// <summary>
	/// Win32 APIを使ったCDDA読み取りクラス。
	/// IOCTL操作を完全に隔離する。
	/// </summary>
	public class CdReader : IDisposable
	{
		// ===========================
		// Win32 定数
		// ===========================
		private const uint GENERIC_READ = 0x80000000;
		private const uint FILE_SHARE_READ = 0x00000001;
		private const uint FILE_SHARE_WRITE = 0x00000002;
		private const uint OPEN_EXISTING = 3;

		private const uint IOCTL_CDROM_READ_TOC = 0x00024000;
		private const uint IOCTL_CDROM_RAW_READ = 0x0002403E;
		private const uint IOCTL_STORAGE_EJECT_MEDIA = 0x002D4808;

		private const int CDDA_SECTOR_SIZE = 2352;   // CDDAの1セクタ = 2352バイト
		private const int SECTORS_PER_SECOND = 75;   // 1秒あたり75セクタ
		private const int MSF_OFFSET = 150;          // MSF→LBA変換オフセット
		private const uint TRACK_MODE_CDDA = 2;       // RAW_READ_INFOのTrackMode

		// ===========================
		// Win32 構造体
		// ===========================
		[StructLayout(LayoutKind.Sequential)]
		private struct TRACK_DATA
		{
			public byte Reserved;
			public byte Control;      // bits 0-3: ADR, bits 4-7: Control
			public byte TrackNumber;
			public byte Reserved1;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public byte[] Address;    // MSF形式 [0]=予約, [1]=分, [2]=秒, [3]=フレーム
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct CDROM_TOC
		{
			public ushort Length;
			public byte FirstTrack;
			public byte LastTrack;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
			public TRACK_DATA[] TrackData;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct RAW_READ_INFO
		{
			public long DiskOffset;   // バイトオフセット（LBA * 2048）
			public uint SectorCount;
			public uint TrackMode;    // CDDA = 2
		}

		// ===========================
		// P/Invoke
		// ===========================
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern SafeFileHandle CreateFile(
			string lpFileName,
			uint dwDesiredAccess,
			uint dwShareMode,
			IntPtr lpSecurityAttributes,
			uint dwCreationDisposition,
			uint dwFlagsAndAttributes,
			IntPtr hTemplateFile);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool DeviceIoControl(
			SafeFileHandle hDevice,
			uint dwIoControlCode,
			IntPtr lpInBuffer,
			uint nInBufferSize,
			IntPtr lpOutBuffer,
			uint nOutBufferSize,
			out uint lpBytesReturned,
			IntPtr lpOverlapped);

		// ===========================
		// フィールド
		// ===========================
		private readonly char _driveLetter;
		private SafeFileHandle _handle;
		private List<CdTrackInfo> _tracks;
		private bool _disposed = false;

		// ===========================
		// プロパティ
		// ===========================
		public IReadOnlyList<CdTrackInfo> Tracks => _tracks;
		public int AudioTracks => _tracks?.Count ?? 0;
		public char DriveLetter => _driveLetter;

		// ===========================
		// コンストラクタ
		// ===========================
		public CdReader(char driveLetter)
		{
			_driveLetter = char.ToUpper(driveLetter);
			_tracks = new List<CdTrackInfo>();
			Open();
			ReadToc();
		}

		// ===========================
		// ドライブオープン
		// ===========================
		private void Open()
		{
			string path = $@"\\.\{_driveLetter}:";
			_handle = CreateFile(
				path,
				GENERIC_READ,
				FILE_SHARE_READ | FILE_SHARE_WRITE,
				IntPtr.Zero,
				OPEN_EXISTING,
				0,
				IntPtr.Zero);

			if (_handle.IsInvalid)
				throw new InvalidOperationException(
					$"ドライブ {_driveLetter}: を開けませんでした。(Error: {Marshal.GetLastWin32Error()})");
		}

		// ===========================
		// TOC読み取り（トラック一覧）
		// ===========================
		private void ReadToc()
		{
			_tracks.Clear();

			int tocSize = Marshal.SizeOf(typeof(CDROM_TOC));
			IntPtr tocPtr = Marshal.AllocHGlobal(tocSize);

			try
			{
				uint bytesReturned;
				bool ok = DeviceIoControl(
					_handle,
					IOCTL_CDROM_READ_TOC,
					IntPtr.Zero, 0,
					tocPtr, (uint)tocSize,
					out bytesReturned,
					IntPtr.Zero);

				if (!ok)
					throw new InvalidOperationException(
						$"TOC読み取り失敗 (Error: {Marshal.GetLastWin32Error()})");

				CDROM_TOC toc = (CDROM_TOC)Marshal.PtrToStructure(tocPtr, typeof(CDROM_TOC));

				// leadoutトラックを含む総トラック数
				int totalEntries = toc.LastTrack - toc.FirstTrack + 2;

				for (int i = 0; i < toc.LastTrack - toc.FirstTrack + 1; i++)
				{
					TRACK_DATA current = toc.TrackData[i];
					TRACK_DATA next = toc.TrackData[i + 1];

					// オーディオトラックのみ（Control bit2 = 0 がオーディオ）
					if ((current.Control & 0x04) != 0) continue;

					long startSector = MsfToLba(current.Address);
					long endSector = MsfToLba(next.Address);

					_tracks.Add(new CdTrackInfo
					{
						TrackNumber = current.TrackNumber,
						StartSector = startSector,
						EndSector = endSector,
					});
				}
			}
			finally
			{
				Marshal.FreeHGlobal(tocPtr);
			}
		}

		// ===========================
		// トラックのPCMデータ取得
		// ===========================
		/// <summary>
		/// 指定トラックのPCMデータを byte[] で返す。
		/// CDDA固定：44100Hz / ステレオ / 16bit
		/// </summary>
		public byte[] ReadTrack(int trackIndex)
		{
			if (trackIndex < 0 || trackIndex >= _tracks.Count)
				throw new ArgumentOutOfRangeException(nameof(trackIndex));

			CdTrackInfo track = _tracks[trackIndex];
			long sectorCount = track.SectorCount;
			byte[] buffer = new byte[sectorCount * CDDA_SECTOR_SIZE];

			// セクタを分割して読む（一度に読める上限を設ける）
			const int READ_CHUNK = 20; // 一度に読むセクタ数
			long remaining = sectorCount;
			long currentLba = track.StartSector;
			int bufferPos = 0;

			while (remaining > 0)
			{
				long toRead = Math.Min(remaining, READ_CHUNK);
				byte[] chunk = ReadSectors(currentLba, (uint)toRead);
				Buffer.BlockCopy(chunk, 0, buffer, bufferPos, chunk.Length);

				bufferPos += chunk.Length;
				currentLba += toRead;
				remaining -= toRead;
			}

			return buffer;
		}

		// ===========================
		// セクタ読み取り
		// ===========================
		private byte[] ReadSectors(long startLba, uint sectorCount)
		{
			RAW_READ_INFO readInfo = new RAW_READ_INFO
			{
				DiskOffset = startLba * 2048, // バイトオフセット（2048はISOセクタサイズ）
				SectorCount = sectorCount,
				TrackMode = TRACK_MODE_CDDA,
			};

			int infoSize = Marshal.SizeOf(typeof(RAW_READ_INFO));
			int bufferSize = (int)(sectorCount * CDDA_SECTOR_SIZE);
			byte[] output = new byte[bufferSize];

			IntPtr infoPtr = Marshal.AllocHGlobal(infoSize);
			IntPtr outputPtr = Marshal.AllocHGlobal(bufferSize);

			try
			{
				Marshal.StructureToPtr(readInfo, infoPtr, false);

				uint bytesReturned;
				bool ok = DeviceIoControl(
					_handle,
					IOCTL_CDROM_RAW_READ,
					infoPtr, (uint)infoSize,
					outputPtr, (uint)bufferSize,
					out bytesReturned,
					IntPtr.Zero);

				if (!ok)
					throw new InvalidOperationException(
						$"セクタ読み取り失敗 LBA={startLba} (Error: {Marshal.GetLastWin32Error()})");

				Marshal.Copy(outputPtr, output, 0, bufferSize);
			}
			finally
			{
				Marshal.FreeHGlobal(infoPtr);
				Marshal.FreeHGlobal(outputPtr);
			}

			return output;
		}

		// ===========================
		// イジェクト
		// ===========================
		public void Eject()
		{
			uint bytesReturned;
			DeviceIoControl(
				_handle,
				IOCTL_STORAGE_EJECT_MEDIA,
				IntPtr.Zero, 0,
				IntPtr.Zero, 0,
				out bytesReturned,
				IntPtr.Zero);
		}

		// ===========================
		// ユーティリティ
		// ===========================
		/// <summary>
		/// MSFアドレス → LBA変換
		/// </summary>
		private static long MsfToLba(byte[] msf)
		{
			// msf[0]=予約, [1]=分, [2]=秒, [3]=フレーム
			return ((long)msf[1] * 60 + msf[2]) * SECTORS_PER_SECOND + msf[3] - MSF_OFFSET;
		}

		/// <summary>
		/// CDドライブの一覧を返す
		/// </summary>
		public static List<string> GetCdDrives()
		{
			var drives = new List<string>();
			foreach (System.IO.DriveInfo drive in System.IO.DriveInfo.GetDrives())
			{
				if (drive.DriveType == System.IO.DriveType.CDRom)
					drives.Add(drive.Name.TrimEnd('\\'));
			}
			return drives;
		}

		// ===========================
		// IDisposable
		// ===========================
		public void Dispose()
		{
			if (!_disposed)
			{
				_handle?.Dispose();
				_disposed = true;
			}
		}
	}
}