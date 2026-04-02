using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace MediaPlayer_X_Ark.Engine.Player
{
	internal sealed class BassMidiRenderer : IDisposable
	{
		private const uint BASS_UNICODE = 0x80000000;
		private const uint BASS_STREAM_DECODE = 0x200000;
		private static readonly object SyncRoot = new object();
		private bool _disposed;

		public byte[] Render(string midiPath, string soundFontPath, int sampleRate = 44100)
		{
			lock (SyncRoot)
			{
				uint soundFont = 0;
				uint stream = 0;
				bool bassInitialized = false;

				try
				{
					if (!NativeMethods.BASS_Init(0, sampleRate, 0, IntPtr.Zero, IntPtr.Zero))
						throw CreateBassException("BASS_Init");
					bassInitialized = true;

					soundFont = NativeMethods.BASS_MIDI_FontInit(soundFontPath, BASS_UNICODE);
					if (soundFont == 0)
						throw CreateBassException("BASS_MIDI_FontInit");

					var fonts = new BASS_MIDI_FONT[]
					{
						new BASS_MIDI_FONT
						{
							font = soundFont,
							preset = -1,
							bank = 0,
						},
					};

					stream = NativeMethods.BASS_MIDI_StreamCreateFile(
						0,
						midiPath,
						0,
						0,
						BASS_STREAM_DECODE | BASS_UNICODE,
						(uint)sampleRate);
					if (stream == 0)
						throw CreateBassException("BASS_MIDI_StreamCreateFile");

					if (!NativeMethods.BASS_MIDI_StreamSetFonts(stream, fonts, 1))
						throw CreateBassException("BASS_MIDI_StreamSetFonts");

					Debug.WriteLine(
						$"[BASSMIDI] Render start midi=\"{midiPath}\" sf2=\"{soundFontPath}\" sampleRate={sampleRate}");

					var ms = new MemoryStream();
					var buffer = new byte[16384];

					while (true)
					{
						int read = NativeMethods.BASS_ChannelGetData(stream, buffer, buffer.Length);
						if (read <= 0)
							break;

						ms.Write(buffer, 0, read);
					}

					Debug.WriteLine(
						$"[BASSMIDI] Render completed pcmSize={ms.Length} midi=\"{midiPath}\"");
					return ms.ToArray();
				}
				finally
				{
					if (stream != 0)
						NativeMethods.BASS_StreamFree(stream);
					if (soundFont != 0)
						NativeMethods.BASS_MIDI_FontFree(soundFont);
					if (bassInitialized)
						NativeMethods.BASS_Free();
				}
			}
		}

		private static InvalidOperationException CreateBassException(string apiName)
		{
			int errorCode = NativeMethods.BASS_ErrorGetCode();
			return new InvalidOperationException($"{apiName} failed (BASS error {errorCode}).");
		}

		public void Dispose()
		{
			if (!_disposed)
				_disposed = true;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct BASS_MIDI_FONT
		{
			public uint font;
			public int preset;
			public int bank;
		}

		private static class NativeMethods
		{
			[DllImport("bass.dll", CallingConvention = CallingConvention.StdCall)]
			internal static extern bool BASS_Init(
				int device,
				int freq,
				uint flags,
				IntPtr win,
				IntPtr clsid);

			[DllImport("bass.dll", CallingConvention = CallingConvention.StdCall)]
			internal static extern bool BASS_Free();

			[DllImport("bass.dll", CallingConvention = CallingConvention.StdCall)]
			internal static extern int BASS_ErrorGetCode();

			[DllImport("bass.dll", CallingConvention = CallingConvention.StdCall)]
			internal static extern int BASS_ChannelGetData(
				uint handle,
				[Out] byte[] buffer,
				int length);

			[DllImport("bass.dll", CallingConvention = CallingConvention.StdCall)]
			internal static extern bool BASS_StreamFree(uint handle);

			[DllImport("bassmidi.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
			internal static extern uint BASS_MIDI_FontInit(string file, uint flags);

			[DllImport("bassmidi.dll", CallingConvention = CallingConvention.StdCall)]
			internal static extern bool BASS_MIDI_FontFree(uint handle);

			[DllImport("bassmidi.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
			internal static extern uint BASS_MIDI_StreamCreateFile(
				uint filetype,
				string file,
				long offset,
				long length,
				uint flags,
				uint freq);

			[DllImport("bassmidi.dll", CallingConvention = CallingConvention.StdCall)]
			internal static extern bool BASS_MIDI_StreamSetFonts(
				uint handle,
				[In] BASS_MIDI_FONT[] fonts,
				uint count);
		}
	}
}
