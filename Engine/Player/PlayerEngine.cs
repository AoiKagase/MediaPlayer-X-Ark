using FMOD;
using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using MediaPlayer_X_Ark.Engine.Visualize;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Player
{
    public struct DEVICE_INFO
	{
		public int deviceId;
		public System.Guid guid;
		public string name;
		public int namelen;
		public int systemrate;
		public FMOD.SPEAKERMODE speakermode;
		public int speakerModeChannels;

		public string Name
        {
			get { return name; }
        }
		public string GUID
        {
			get { return guid.ToString(); }
        }
    }

    [Flags]
    public enum LOOP_MODE
    {
        LOOP_NONE		= 0x01, // 0001
        LOOP_RANDOM		= 0x02, // 0010
        LOOP_ONE_REPEAT = 0x04, // 0100
        LOOP_ALL		= 0x08, // 1000
    }

	internal sealed class XArkMidiFmodStream : IDisposable
	{
		private const int BytesPerSample = sizeof(short);
		private readonly XArkMidiEngine.Engine _engine;
		private readonly object _syncRoot = new object();
		private readonly GCHandle _selfHandle;
		private short[] _renderBuffer;
		private byte[] _zeroBuffer;
		private bool _disposed;
		private bool _loopEnabled;

		public int SampleRate { get; }
		public int ChannelCount { get; }
		public uint DecodeBufferFrames { get; } = 2048;
		public uint LengthBytes { get; }
		public uint FmodStreamLengthBytes => uint.MaxValue;
		public uint EstimatedLengthMs { get; }
		public IntPtr UserData => GCHandle.ToIntPtr(_selfHandle);
		public FMOD.SOUND_PCMREAD_CALLBACK PcmReadCallback { get; }
		public FMOD.SOUND_PCMSETPOS_CALLBACK PcmSetPosCallback { get; }
		public Exception LastError { get; private set; }

		public XArkMidiFmodStream(
			string midiPath,
			string soundBankPath,
			XArkMidiEngine.SoundBankKind soundBankKind,
			uint sampleRate,
			uint channelCount,
			XArkMidiEngine.CreateOptions options)
		{
			_engine = new XArkMidiEngine.Engine(
				midiPath,
				soundBankPath,
				soundBankKind,
				sampleRate,
				channelCount,
				options);
			SampleRate = checked((int)sampleRate);
			ChannelCount = checked((int)channelCount);
			LengthBytes = CalculateLengthBytes(_engine.LengthFramesEstimate, channelCount);
			EstimatedLengthMs = CalculateLengthMs(_engine.LengthFramesEstimate, sampleRate);
			_renderBuffer = new short[checked((int)(DecodeBufferFrames * channelCount))];
			_zeroBuffer = new byte[checked((int)(DecodeBufferFrames * channelCount * BytesPerSample))];
			_selfHandle = GCHandle.Alloc(this);
			PcmReadCallback = OnPcmRead;
			PcmSetPosCallback = OnPcmSetPosition;
		}

		public void SetLoop(bool enabled)
		{
			lock (_syncRoot)
			{
				if (_disposed || _loopEnabled == enabled)
					return;

				_engine.SetLoop(enabled, 0);
				_loopEnabled = enabled;
			}
		}

		public bool TryTakeLastError(out Exception error)
		{
			error = LastError;
			LastError = null;
			return error != null;
		}

		public uint CurrentPositionMs
		{
			get
			{
				lock (_syncRoot)
				{
					if (_disposed || SampleRate <= 0)
						return 0;

					ulong positionMs = (ulong)Math.Round(_engine.CurrentFramePosition * 1000.0 / SampleRate);
					return positionMs > uint.MaxValue ? uint.MaxValue : (uint)positionMs;
				}
			}
		}

		public void SeekMilliseconds(uint positionMs)
		{
			lock (_syncRoot)
			{
				if (_disposed)
					return;

				SeekFramesCore((ulong)Math.Round(positionMs * (double)SampleRate / 1000.0));
			}
		}

		private FMOD.RESULT OnPcmRead(IntPtr sound, IntPtr data, uint datalen)
		{
			try
			{
				if (_disposed)
				{
					FillSilence(data, datalen);
					return FMOD.RESULT.OK;
				}

				int requestedBytes = checked((int)datalen);
				int bytesPerFrame = ChannelCount * BytesPerSample;
				int requestedFrames = requestedBytes / bytesPerFrame;
				if (requestedFrames <= 0)
				{
					FillSilence(data, datalen);
					return FMOD.RESULT.OK;
				}

				EnsureRenderBuffer(requestedFrames);
				uint writtenFrames;
				lock (_syncRoot)
				{
					writtenFrames = _engine.Render(_renderBuffer, (uint)requestedFrames);
					if (writtenFrames == 0 && _loopEnabled)
					{
						_engine.Reset();
						writtenFrames = _engine.Render(_renderBuffer, (uint)requestedFrames);
					}
				}

				if (writtenFrames == 0 && !_loopEnabled)
					return FMOD.RESULT.ERR_FILE_EOF;

				int writtenSamples = checked((int)writtenFrames * ChannelCount);
				int writtenBytes = writtenSamples * BytesPerSample;
				if (writtenSamples > 0)
					Marshal.Copy(_renderBuffer, 0, data, writtenSamples);
				if (writtenBytes < requestedBytes)
					FillSilence(IntPtr.Add(data, writtenBytes), (uint)(requestedBytes - writtenBytes));

				return FMOD.RESULT.OK;
			}
			catch (Exception ex)
			{
				LastError = ex;
				FillSilence(data, datalen);
				return FMOD.RESULT.OK;
			}
		}

		private FMOD.RESULT OnPcmSetPosition(
			IntPtr sound,
			int subsound,
			uint position,
			FMOD.TIMEUNIT postype)
		{
			try
			{
				lock (_syncRoot)
				{
					if (_disposed)
						return FMOD.RESULT.OK;

					SeekFramesCore(ConvertToFrames(position, postype));
				}
				return FMOD.RESULT.OK;
			}
			catch (Exception ex)
			{
				LastError = ex;
				return FMOD.RESULT.OK;
			}
		}

		private ulong ConvertToFrames(uint position, FMOD.TIMEUNIT postype)
		{
			if ((postype & FMOD.TIMEUNIT.PCM) != 0)
				return position;
			if ((postype & FMOD.TIMEUNIT.PCMBYTES) != 0 || (postype & FMOD.TIMEUNIT.RAWBYTES) != 0)
				return position / (uint)(ChannelCount * BytesPerSample);
			if ((postype & FMOD.TIMEUNIT.MS) != 0)
				return (ulong)Math.Round(position * (double)SampleRate / 1000.0);

			return position;
		}

		private void SeekFramesCore(ulong framePosition)
		{
			if (framePosition == 0)
				_engine.Reset();
			else
				_engine.SeekFrames(framePosition);
		}

		private static uint CalculateLengthBytes(ulong frameCount, uint channelCount)
		{
			ulong lengthBytes = frameCount * channelCount * BytesPerSample;
			if (lengthBytes == 0)
				return 1;
			return lengthBytes > uint.MaxValue ? uint.MaxValue : (uint)lengthBytes;
		}

		private static uint CalculateLengthMs(ulong frameCount, uint sampleRate)
		{
			if (frameCount == 0 || sampleRate == 0)
				return 0;

			ulong lengthMs = (ulong)Math.Round(frameCount * 1000.0 / sampleRate);
			return lengthMs > uint.MaxValue ? uint.MaxValue : (uint)lengthMs;
		}

		private void EnsureRenderBuffer(int requestedFrames)
		{
			int requiredSamples = checked(requestedFrames * ChannelCount);
			if (_renderBuffer.Length < requiredSamples)
				_renderBuffer = new short[requiredSamples];
		}

		private void FillSilence(IntPtr data, uint datalen)
		{
			int remaining = checked((int)datalen);
			int offset = 0;
			EnsureZeroBuffer(Math.Min(remaining, _zeroBuffer.Length));
			while (remaining > 0)
			{
				int chunk = Math.Min(remaining, _zeroBuffer.Length);
				Marshal.Copy(_zeroBuffer, 0, IntPtr.Add(data, offset), chunk);
				remaining -= chunk;
				offset += chunk;
			}
		}

		private void EnsureZeroBuffer(int requiredBytes)
		{
			if (_zeroBuffer.Length < requiredBytes)
				_zeroBuffer = new byte[requiredBytes];
		}

		public void Dispose()
		{
			if (_disposed)
				return;

			lock (_syncRoot)
			{
				if (_disposed)
					return;

				_disposed = true;
				_engine.Dispose();
				if (_selfHandle.IsAllocated)
					_selfHandle.Free();
			}
		}
	}

	public class PlayerEngine : IPlayerEngine
	{
		private bool _disposed = false;
		private readonly SemaphoreSlim _tagLoadSemaphore = new SemaphoreSlim(3, 3);
		public IReadOnlyList<PluginLoadResult> LoadedPlugins => _loadedPlugins;
		private readonly List<PluginLoadResult> _loadedPlugins = new List<PluginLoadResult>();

		public FmodSpectrum spectrum { get; private set; }
		public FmodWave wave { get; private set; }
		public Effector.Effectors effector { get; private set; }
		private LOOP_MODE _loop = LOOP_MODE.LOOP_NONE;
		public LOOP_MODE loop
		{
			get => _loop;
			set
			{
				_loop = value;
				ApplyCurrentXArkMidiLoopMode();
			}
		}
		public event EventHandler<PlayerErrorEventArgs> ErrorOccurred;
		public event Action<int> TrackAdvanced;
		public int PlayingIndex { get; private set; } = -1;
		protected bool initialized = false;
		private bool _nowPlaying = false;
		public bool NowPlaying => _nowPlaying;
		public BindingList<Engine.Player.PlayList> PlayList { get; set; } = new BindingList<Engine.Player.PlayList>();
		protected FMOD.System FmodSystem;
		protected FMOD.ChannelGroup FmodChannelGroup;
		protected FMOD.Channel FmodChannel;

		protected FMOD.OUTPUTTYPE FmodOutputType;


		protected uint FmodVersion;

		protected List<DEVICE_INFO> FmodDeviceList = new List<DEVICE_INFO>();

		private const int channelCount = 8;
		public int ChannelCount => channelCount;
		private List<int> _shuffleQueue = new List<int>();
		private int _shuffleQueueIndex = 0;
		private readonly Random _rng = new Random();

		// ── クロスフェード用フィールド ────────────────────────────────────────
		private FMOD.Channel FmodChannelFading;      // フェードアウト中の旧チャンネル
		private int _fadingPlayListIndex = -1;       // フェードアウト中の PlayList インデックス
		private int _crossfadeElapsedMs = 0;         // フェード経過時間（ms）
		private bool _isCrossfading = false;
		private bool _isCrossfadeVolumeFixed = false; // NonStopMix 時は音量固定で並走
		private float _masterVolume = 1.0f;           // SetVolume で設定されたマスター音量
		private float _masterPan = 0.0f;
		private FMOD.Channel _preparedChannel;
		private FMOD.Channel _retiringChannel;
		private int _preparedIndex = -1;
		private int _preparedShuffleQueueIndex = -1;
		private ulong _scheduledTransitionClock = 0;
		private bool _pendingGaplessTransition = false;
		private volatile bool _preparedChannelEnded = false;
		private volatile bool _retiringChannelEnded = false;
		private readonly FMOD.CHANNELCONTROL_CALLBACK _channelControlCallback;
		public bool CrossfadeEnabled { get; set; } = false;
		public int CrossfadeDurationMs { get; set; } = 3000;
		public bool CrossfadeTriggered { get; set; } = false;
		public bool NonStopMixEnabled { get; set; } = false;
		private string _soundFontPath = "";
		private MidiRendererBackend _midiRendererBackend = MidiRendererBackend.Auto;
		private readonly object _fmodLock = new object();
		private WaveformAnalyzer _waveformAnalyzer;
		// 曲切り替わり時に前回の波形解析をキャンセルするためのトークン
		private System.Threading.CancellationTokenSource _waveformCts;
		public bool WaveformEnabled { get; set; } = false;
		public event Action<int> WaveformReady;
		public string SoundFontPath
		{
			get => _soundFontPath;
			set => _soundFontPath = value ?? "";
		}

		public MidiRendererBackend MidiRendererBackend
		{
			get => _midiRendererBackend;
			set
			{
				if (_midiRendererBackend == value) return;
				_midiRendererBackend = value;
				InvalidateLoadedMidiSounds();
			}
		}

		/// <summary>
		/// MIDIレンダラー切り替え時に、既ロード済みのMIDIサウンドを破棄する。
		/// 次回再生時に新エンジンで再レンダリングされる。
		/// 現在再生中のトラックは安全のためスキップする。
		/// </summary>
		private void InvalidateLoadedMidiSounds()
		{
			for (int i = 0; i < PlayList.Count; i++)
			{
				if (i == PlayingIndex) continue;
				var entry = PlayList[i];
				if (!entry.IsLoaded) continue;
				var ext = Path.GetExtension(entry.FileName).ToLower();
				if (ext != ".mid" && ext != ".midi") continue;
				ReleasePlaylistSound(entry);
			}
		}

		public List<DEVICE_INFO> DeviceList
		{
			get { return FmodDeviceList; }
		}

		public bool ReplayGainEnabled { get; set; } = false;
		public int ReplayGainMode { get; set; } = 0;
		public float ReplayGainPreamp { get; set; } = 0.0f;

		protected FMOD.RESULT FmodCallFunction(FMOD.RESULT result, [CallerMemberName] string callerMethodName = "")
		{
			if (result != FMOD.RESULT.OK)
			{
				ErrorOccurred?.Invoke(this, new PlayerErrorEventArgs(
					callerMethodName,
					FMOD.Error.String(result),
					(int)result));
			}
			return result;
		}

		public PlayerEngine()
		{
			_channelControlCallback = OnChannelControlCallback;
			CreateSystem();
		}

		~PlayerEngine()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) return;
			if (initialized)
			{
				if (disposing)
				{
					spectrum?.Dispose();
					wave?.Dispose();
					_tagLoadSemaphore?.Dispose();
				}

				_waveformCts?.Cancel();
				_waveformCts?.Dispose();

				// Relase FMOD handles for Channel.
				try
				{
					if (FmodChannel.hasHandle())
						FmodChannel.stop();
				}
				catch { }
				try
				{
					if (FmodChannelFading.hasHandle())
						FmodChannelFading.stop();
				}
				catch { }
				try
				{
					if (FmodChannelGroup.hasHandle())
						FmodChannelGroup.release();
				}
				catch { }

				for (int i = 0; i < PlayList.Count; i++)
				{
					try
					{
						if (PlayList[i].IsLoaded && PlayList[i].Sound.hasHandle())
							ReleasePlaylistSound(PlayList[i]);
					}
					catch { }
				}
				PlayList.Clear();

				try
				{
					if (FmodSystem.hasHandle())
					{
						FmodSystem.close();
						FmodSystem.release();
					}
				}
				catch { }
			}
			_disposed = true;
		}

		protected FMOD.RESULT CreateSystem()
		{
			return FmodCallFunction(FMOD.Factory.System_Create(out FmodSystem));
		}
		/// <summary>
		/// System Initialize.
		/// </summary>
		public void Initialize(CfgBuffer bufferSettings = null)
		{
			var fluidSynthPath = Path.Combine(
				AppDomain.CurrentDomain.BaseDirectory, "Libs", "fluidsynth.dll");
			_fluidSynthAvailable = File.Exists(fluidSynthPath);
			var bassPath = Path.Combine(
				AppDomain.CurrentDomain.BaseDirectory, "Libs", "bass.dll");
			var bassMidiPath = Path.Combine(
				AppDomain.CurrentDomain.BaseDirectory, "Libs", "bassmidi.dll");
			_bassMidiAvailable = File.Exists(bassPath) && File.Exists(bassMidiPath);
			var xarkMidiPath = Path.Combine(
				AppDomain.CurrentDomain.BaseDirectory, "Libs", "XArkMidiEngine.dll");
			_xarkMidiAvailable = File.Exists(xarkMidiPath);

			{
				if (FmodCallFunction(FmodSystem.getVersion(out FmodVersion)) == FMOD.RESULT.OK)
				{
					if (FmodVersion != FMOD.VERSION.number)
					{
						ErrorOccurred?.Invoke(this, new PlayerErrorEventArgs(
							nameof(Initialize),
							$"FMOD version mismatch. Found: {FmodVersion:X}, Required: {FMOD.VERSION.number:X}",
							-1));
						return;
					}
				}

				// バッファ設定は init() より前に適用する必要がある
				if (bufferSettings != null)
				{
					FmodSystem.setStreamBufferSize(
						(uint)(bufferSettings.StreamBufferSizeKB * 1024),
						FMOD.TIMEUNIT.RAWBYTES);
					FmodSystem.setDSPBufferSize(
						(uint)bufferSettings.DspBufferSize,
						bufferSettings.DspBufferCount);
				}

				if (FmodCallFunction(FmodSystem.init(channelCount, FMOD.INITFLAGS.NORMAL, IntPtr.Zero)) == RESULT.OK)
				{
					if (FmodCallFunction(FmodSystem.createChannelGroup("Channel 01", out FmodChannelGroup)) == RESULT.OK)
					{
						spectrum = new FmodSpectrum(ref FmodSystem, 1024, ref this.FmodChannelGroup);
						wave = new FmodWave(ref FmodSystem, ref FmodChannelGroup);

						LoadPlugins();

						PlayList = new BindingList<Engine.Player.PlayList>();

						effector = new Engine.Effector.Effectors(FmodSystem, this);
						_waveformAnalyzer = new WaveformAnalyzer(FmodSystem, _fmodLock);
						GetDeviceList();

						loop = LOOP_MODE.LOOP_NONE;

						initialized = true;
					}
				}
			}
		}

		public void LoadPlugins()
		{
			_loadedPlugins.Clear();

			string pluginDir = Path.Combine(
				AppDomain.CurrentDomain.BaseDirectory, "Plugins");

			if (!Directory.Exists(pluginDir)) return;

			// ★末尾にセパレータを付けてパスを確定させる
			string pluginPath = pluginDir.TrimEnd(
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar)
				+ Path.DirectorySeparatorChar;
			var result = FmodSystem.setPluginPath(pluginPath);

			foreach (string dllPath in Directory.GetFiles(pluginDir, "*.dll"))
			{
				string filename = Path.GetFileName(dllPath);
				// loadPlugin にはファイル名のみ渡す（setPluginPath との相対パス）
				result = FmodSystem.loadPlugin(filename, out uint handle, 10);

				if (result != FMOD.RESULT.OK)
				{
					_loadedPlugins.Add(new PluginLoadResult
					{
						FileName = filename,
						Success = false,
						Type = FMOD.PLUGINTYPE.MAX,
						Version = 0,
					});
					continue;
				}

				FmodSystem.getPluginInfo(
					handle,
					out FMOD.PLUGINTYPE pluginType,
					out string pluginName,
					256,
					out uint version);

				_loadedPlugins.Add(new PluginLoadResult
				{
					FileName = filename,
					PluginName = pluginName,
					Success = true,
					Type = pluginType,
					Version = version,
				});

				if (pluginType == FMOD.PLUGINTYPE.CODEC)
					SupportedFormats.RegisterLoadedCodec(filename);
			}
			return;
		}

		/// <summary>一時停止をトグルする</summary>
		public void SwitchPause()
		{
			if (FmodChannel.hasHandle())
			{
				FmodChannel.isPlaying(out bool isplaying);
				if (isplaying)
				{
					bool paused;
					if (FmodCallFunction(FmodChannel.getPaused(out paused)) == RESULT.OK)
					{
						FmodCallFunction(FmodChannel.setPaused(!paused));
					}
				}
				else
				{
					if (IsValidIndex(PlayingIndex))
					{
						var result = FmodCallFunction(FmodSystem.playSound(PlayList[PlayingIndex].Sound, FmodChannelGroup, false, out FmodChannel));
						if (result != RESULT.OK)
						{
							ApplyOutputSettingsToCurrentChannel(PlayingIndex);
						}
					}
				}
			}
		}

		/// <summary>
		/// 出力タイプを設定する。必ず Initialize() より前に呼ぶこと。
		/// 対応タイプ：AUTODETECT / WASAPI / ASIO / WINSONIC
		/// </summary>
		public void SetOutputTypeBeforeInit(FMOD.OUTPUTTYPE outputtype)
		{
			FmodOutputType = outputtype;
			FmodCallFunction(FmodSystem.setOutput(outputtype));
		}

		/// <summary>
		/// OutputType設定。
		/// ※ init()後に呼んでも反映されない。設定の保存のみに使用すること。
		/// </summary>
		public void SetOutputType(FMOD.OUTPUTTYPE outputtype)
		{
			FmodOutputType = outputtype;
			// init後は反映されないため、FmodSystem.setOutput()は呼ばない
		}

		public FMOD.OUTPUTTYPE GetOutputType()
		{
			FmodCallFunction(FmodSystem.getOutput(out FmodOutputType));
			return FmodOutputType;
		}

		/// <summary>再生中かどうかを返す</summary>
		public bool IsPlaying()
		{
			UpdateScheduledTransition();
			ReportCurrentXArkMidiStreamError(nameof(IsPlaying));
			if (_pendingGaplessTransition)
				return true;

			bool result = false;
			if (FmodChannel.hasHandle())
			{
				// stop() 後は FMOD_ERR_INVALID_HANDLE が返るためエラーチェックしない
				FmodChannel.isPlaying(out result);
				return result;
			}
			return false;
		}

		public void UpdateScheduledTransition()
		{
			if (_retiringChannelEnded)
			{
				_retiringChannel = default;
				_retiringChannelEnded = false;
			}

			if (!_pendingGaplessTransition)
			{
				if (_preparedChannelEnded && !_preparedChannel.hasHandle())
					_preparedChannelEnded = false;
				return;
			}

			if (!_preparedChannel.hasHandle() || _preparedChannelEnded)
			{
				ClearPreparedTransition(false);
				return;
			}

			if (!FmodChannelGroup.hasHandle())
				return;

			FmodChannelGroup.getDSPClock(out _, out ulong parentClock);
			if (parentClock < _scheduledTransitionClock)
				return;

			FmodChannel = _preparedChannel;
			_preparedChannel = default;
			PlayingIndex = _preparedIndex;
			_preparedIndex = -1;
			if (_preparedShuffleQueueIndex >= 0)
				_shuffleQueueIndex = _preparedShuffleQueueIndex;
			_preparedShuffleQueueIndex = -1;
			_pendingGaplessTransition = false;
			_scheduledTransitionClock = 0;
			_preparedChannelEnded = false;
			_nowPlaying = true;
			TrackAdvanced?.Invoke(PlayingIndex);

			if (NonStopMixEnabled && !CrossfadeEnabled)
				PrepareGaplessTransition(PlayingIndex);
		}

		private FMOD.RESULT OnChannelControlCallback(
			IntPtr channelcontrol,
			FMOD.CHANNELCONTROL_TYPE controltype,
			FMOD.CHANNELCONTROL_CALLBACK_TYPE callbacktype,
			IntPtr commanddata1,
			IntPtr commanddata2)
		{
			if (controltype != FMOD.CHANNELCONTROL_TYPE.CHANNEL
				|| callbacktype != FMOD.CHANNELCONTROL_CALLBACK_TYPE.END)
				return FMOD.RESULT.OK;

			if (_preparedChannel.hasHandle() && channelcontrol == _preparedChannel.handle)
				_preparedChannelEnded = true;
			else if (_retiringChannel.hasHandle() && channelcontrol == _retiringChannel.handle)
				_retiringChannelEnded = true;

			return FMOD.RESULT.OK;
		}


		/// <summary>FMOD からデバイス一覧を取得して FmodDeviceList に格納する</summary>
		public void GetDeviceList()
		{
			int numDrivers = 0;
			DEVICE_INFO device;

			FmodDeviceList.Clear();
			if (FmodCallFunction(FmodSystem.getNumDrivers(out numDrivers)) == RESULT.OK)
			{
				for (int i = 0; i < numDrivers; i++)
				{
					device = new DEVICE_INFO();
					device.namelen = 64;
					device.deviceId = i;
					if (FmodCallFunction(FmodSystem.getDriverInfo(i, out device.name, device.namelen, out device.guid, out device.systemrate, out device.speakermode, out device.speakerModeChannels)) == RESULT.OK)
					{
						FmodDeviceList.Add(device);
					} else if (i == 0)
					{
						device.name = "Default Device";
						device.guid = Guid.Empty;
						FmodDeviceList.Add(device);
					}
				}
			}
		}

		/// <summary>現在選択中のデバイス ID を返す</summary>
		public int GetDevice()
		{
			int driver;
			FmodSystem.getDriver(out driver);
			return driver;
		}
		public string GetDeviceGUID()
		{
			int driver;
			FmodSystem.getDriver(out driver);
			for (int i = 0; i < FmodDeviceList.Count(); i++)
			{
				if (FmodDeviceList[i].deviceId == driver)
				{
					return FmodDeviceList[i].GUID.ToString();
				}
			}
			return "";
		}
		/// <summary>デバイスをインデックスで指定する</summary>
		public void SetDevice(int driver)
		{
			FmodSystem.setDriver(driver);
		}

		/// <summary>デバイスをシステム GUID 文字列で指定する。空またはnullはOSデフォルトデバイスを使用する</summary>
		public void SetDevice(string driver)
		{
			if (string.IsNullOrEmpty(driver))
			{
				// テンポラリシステムで現在のOSデフォルトデバイスGUIDを取得する
				// （setDriver(0)を直接呼ぶと2回目以降にOS変更が反映されないため）
				var defaultGuid = GetCurrentDefaultDeviceGuid();
				if (defaultGuid != Guid.Empty)
				{
					int matchedId = FindDeviceIdByGuid(defaultGuid);
					if (matchedId < 0)
					{
						// リストに無ければ再列挙してリトライ
						GetDeviceList();
						matchedId = FindDeviceIdByGuid(defaultGuid);
					}
					if (matchedId >= 0)
					{
						FmodSystem.setDriver(matchedId);
						return;
					}
				}
				FmodSystem.setDriver(0);
				return;
			}
			for (int i = 0; i < FmodDeviceList.Count(); i++)
			{
				if (FmodDeviceList[i].GUID.Equals(driver))
				{
					FmodSystem.setDriver(FmodDeviceList[i].deviceId);
					return;
				}
			}
		}

		/// <summary>テンポラリFMODシステムで現在のOSデフォルトデバイスのGUIDを取得する</summary>
		private Guid GetCurrentDefaultDeviceGuid()
		{
			FMOD.System tempSystem;
			if (FMOD.Factory.System_Create(out tempSystem) != FMOD.RESULT.OK)
				return Guid.Empty;
			try
			{
				if (tempSystem.setOutput(FmodOutputType) != FMOD.RESULT.OK)
					return Guid.Empty;
				if (tempSystem.init(1, FMOD.INITFLAGS.NORMAL, IntPtr.Zero) != FMOD.RESULT.OK)
					return Guid.Empty;
				var device = new DEVICE_INFO { namelen = 256 };
				if (tempSystem.getDriverInfo(0, out device.name, device.namelen,
					out device.guid, out device.systemrate,
					out device.speakermode, out device.speakerModeChannels) == FMOD.RESULT.OK)
					return device.guid;
				return Guid.Empty;
			}
			finally
			{
				tempSystem.close();
				tempSystem.release();
			}
		}

		/// <summary>FmodDeviceListからGUIDが一致するdeviceIdを返す。見つからなければ-1</summary>
		private int FindDeviceIdByGuid(Guid guid)
		{
			for (int i = 0; i < FmodDeviceList.Count; i++)
			{
				if (FmodDeviceList[i].guid == guid)
					return FmodDeviceList[i].deviceId;
			}
			return -1;
		}
		public uint GetPosition()
		{
			UpdateScheduledTransition();
			ReportCurrentXArkMidiStreamError(nameof(GetPosition));
			uint position = 0;
			if (FmodChannel.hasHandle() && (_pendingGaplessTransition || IsPlaying()))
				FmodCallFunction(FmodChannel.getPosition(out position, TIMEUNIT.MS));

			// CUEトラック: 絶対位置→トラック相対位置に変換
			if (IsValidIndex(PlayingIndex))
			{
				var entry = PlayList[PlayingIndex];
				if (entry.XArkMidiStream != null)
					position = entry.XArkMidiStream.CurrentPositionMs;

				if (entry.IsCueTrack && entry.CueStartMs.HasValue
					&& position >= (uint)entry.CueStartMs.Value)
					position -= (uint)entry.CueStartMs.Value;
			}
			return position;
		}
		public void SetPosition(uint position)
		{
			if (!IsValidIndex(PlayingIndex))
				return;

			bool wasPendingTransition = _pendingGaplessTransition;
			bool preservePaused = false;
			if (!_pendingGaplessTransition && FmodChannel.hasHandle())
				FmodChannel.getPaused(out preservePaused);

			ClearPreparedTransition(true);

			var entry = PlayList[PlayingIndex];
			uint maxPosition = entry.IsCueTrack ? entry.LengthMs : GetLength(PlayingIndex);
			if (maxPosition > 0)
				position = Math.Min(position, maxPosition);

			uint absPos = position;
			if (entry.IsCueTrack && entry.CueStartMs.HasValue)
				absPos = (uint)entry.CueStartMs.Value + position;

			if (wasPendingTransition || !FmodChannel.hasHandle())
			{
				RecreateCurrentChannelAtPosition(absPos, preservePaused);
			}
			else if (entry.XArkMidiStream != null)
			{
				entry.XArkMidiStream.SeekMilliseconds(absPos);
			}
			else
			{
				var result = FmodCallFunction(FmodChannel.setPosition(absPos, TIMEUNIT.MS));
				if (result == FMOD.RESULT.ERR_INVALID_HANDLE)
					RecreateCurrentChannelAtPosition(absPos, preservePaused);
			}

			if (NonStopMixEnabled && !CrossfadeEnabled && IsPlaying())
				PrepareGaplessTransition(PlayingIndex);
		}
		/// <summary>
		/// Retrieves the state a sound is in after being opened with the non blocking flag, or the current state of the streaming buffer.
		/// </summary>
		/// <param name="buffered">Filled percentage of a stream's file buffer. [0 - 100]%</param>
		/// <param name="starving">Starving state. true if a stream has decoded more than the stream file buffer has ready.</param>
		/// <param name="diskBusy">Disk is currently being accessed for this sound.</param>
		/// <returns>Open state of a sound. </returns>
		public FMOD.OPENSTATE GetOpenState(int index, out uint buffered, out bool starving, out bool diskBusy)
		{
			FMOD.OPENSTATE state;
			buffered = 0;
			starving = false;
			diskBusy = false;
			PlayList[index].Sound.getOpenState(out state, out buffered, out starving, out diskBusy);
			return state;
		}

		private void ClearPreparedTransition(bool stopChannel)
		{
			if (_preparedChannel.hasHandle() && stopChannel)
				_preparedChannel.stop();
			if (_retiringChannel.hasHandle() && stopChannel)
				_retiringChannel.stop();

			_preparedChannel = default;
			_retiringChannel = default;
			_preparedIndex = -1;
			_preparedShuffleQueueIndex = -1;
			_scheduledTransitionClock = 0;
			_pendingGaplessTransition = false;
			_preparedChannelEnded = false;
			_retiringChannelEnded = false;
		}

		private void RecreateCurrentChannelAtPosition(uint absPos, bool startPaused)
		{
			if (!IsValidIndex(PlayingIndex))
				return;

			try
			{
				if (FmodChannel.hasHandle())
					FmodChannel.stop();
			}
			catch { }

			var result = FmodCallFunction(FmodSystem.playSound(
				PlayList[PlayingIndex].Sound, FmodChannelGroup, true, out FmodChannel));
			if (result != FMOD.RESULT.OK)
				return;

			if (ReplayGainEnabled)
				ApplyReplayGain(PlayingIndex);
			else
				FmodChannel.setVolume(_masterVolume);
			FmodChannel.setPan(_masterPan);

			var entry = PlayList[PlayingIndex];
			if (entry.XArkMidiStream != null)
			{
				entry.XArkMidiStream.SeekMilliseconds(absPos);
				ApplyCurrentXArkMidiLoopMode();
			}
			else
			{
				result = FmodCallFunction(FmodChannel.setPosition(absPos, TIMEUNIT.MS));
				if (result != FMOD.RESULT.OK)
					return;
			}

			FmodCallFunction(FmodChannel.setPaused(startPaused));
			_nowPlaying = true;
		}

		private bool TryResolveNextIndex(int fromIndex, out int nextIndex, bool advanceShuffleQueue)
		{
			nextIndex = -1;
			if (PlayList.Count == 0)
				return false;

			if (fromIndex < 0)
				fromIndex = Math.Min(PlayingIndex, PlayList.Count - 1);

			if ((loop & LOOP_MODE.LOOP_RANDOM) != 0)
			{
				if (_shuffleQueueIndex >= _shuffleQueue.Count)
					BuildShuffleQueue();
				if (_shuffleQueueIndex >= _shuffleQueue.Count)
					return false;

				nextIndex = _shuffleQueue[_shuffleQueueIndex];
				if (advanceShuffleQueue)
					_shuffleQueueIndex++;
				return true;
			}

			switch (loop)
			{
				case LOOP_MODE.LOOP_ONE_REPEAT:
					nextIndex = fromIndex;
					return true;
				case LOOP_MODE.LOOP_ALL:
					nextIndex = (fromIndex < PlayList.Count - 1) ? fromIndex + 1 : 0;
					return true;
				default:
					if (fromIndex >= PlayList.Count - 1)
						return false;
					nextIndex = fromIndex + 1;
					return true;
			}
		}

		private void ApplyReplayGainToChannel(int index, FMOD.Channel channel)
		{
			if (!channel.hasHandle() || !IsValidIndex(index))
				return;

			var entry = PlayList[index];
			float? gainDb = ReplayGainMode == 1
				? (entry.ReplayGainAlbum ?? entry.ReplayGainTrack)
				: (entry.ReplayGainTrack ?? entry.ReplayGainAlbum);

			if (gainDb == null)
			{
				channel.setVolume(_masterVolume);
				return;
			}

			float totalDb = gainDb.Value + ReplayGainPreamp;
			float linearGain = (float)Math.Pow(10.0, totalDb / 20.0);
			float finalVolume = Math.Min(_masterVolume * linearGain, 1.0f);
			channel.setVolume(finalVolume);
		}

		private ulong EstimateRemainingDspClocks(int index)
		{
			if (!IsValidIndex(index) || !FmodChannel.hasHandle())
				return 0;

			FmodSystem.getSoftwareFormat(out int mixerRate, out _, out _);
			if (mixerRate <= 0)
				return 0;

			var entry = PlayList[index];
			if (entry.IsCueTrack)
			{
				uint currentPos = GetPosition();
				uint remainingMs = entry.LengthMs > currentPos
					? entry.LengthMs - currentPos
					: 0;
				return (ulong)Math.Round(remainingMs * (double)mixerRate / 1000.0);
			}

			if (!entry.IsLoaded)
				return 0;

			entry.Sound.getLength(out uint lengthPcm, FMOD.TIMEUNIT.PCM);
			FmodChannel.getPosition(out uint positionPcm, FMOD.TIMEUNIT.PCM);
			entry.Sound.getDefaults(out float soundRate, out _);

			if (lengthPcm == 0 || soundRate <= 0f || positionPcm >= lengthPcm)
				return 0;

			double remainingSeconds = (lengthPcm - positionPcm) / soundRate;
			return (ulong)Math.Round(remainingSeconds * mixerRate);
		}

		private void PrepareGaplessTransition(int fromIndex)
		{
			ClearPreparedTransition(true);

			if (!NonStopMixEnabled || CrossfadeEnabled || !IsValidIndex(fromIndex) || !FmodChannel.hasHandle())
				return;

			if (!TryResolveNextIndex(fromIndex, out int nextIndex, false))
				return;

			var loadResult = LoadSound(nextIndex);
			if (loadResult != FMOD.RESULT.OK)
				return;

			var result = FmodCallFunction(FmodSystem.playSound(
				PlayList[nextIndex].Sound, FmodChannelGroup, true, out FMOD.Channel preparedChannel));
			if (result != FMOD.RESULT.OK)
			{
				ClearPreparedTransition(false);
				return;
			}

			if (PlayList[nextIndex].IsCueTrack && PlayList[nextIndex].CueStartMs.HasValue)
				preparedChannel.setPosition((uint)PlayList[nextIndex].CueStartMs.Value, FMOD.TIMEUNIT.MS);

			if (ReplayGainEnabled)
				ApplyReplayGainToChannel(nextIndex, preparedChannel);
			else
				preparedChannel.setVolume(_masterVolume);
			preparedChannel.setPan(_masterPan);

			FmodChannelGroup.getDSPClock(out _, out ulong parentClock);
			ulong remainingClocks = EstimateRemainingDspClocks(fromIndex);
			if (remainingClocks == 0)
			{
				preparedChannel.stop();
				return;
			}

			result = FmodCallFunction(preparedChannel.setCallback(_channelControlCallback));
			if (result != FMOD.RESULT.OK)
			{
				preparedChannel.stop();
				return;
			}

			result = FmodCallFunction(FmodChannel.setCallback(_channelControlCallback));
			if (result != FMOD.RESULT.OK)
			{
				preparedChannel.stop();
				return;
			}

			ulong startClock = parentClock + remainingClocks;
			result = FmodCallFunction(preparedChannel.setDelay(startClock, 0, false));
			if (result != FMOD.RESULT.OK)
			{
				preparedChannel.stop();
				return;
			}

			result = FmodCallFunction(FmodChannel.setDelay(0, startClock, true));
			if (result != FMOD.RESULT.OK)
			{
				preparedChannel.stop();
				return;
			}

			result = FmodCallFunction(preparedChannel.setPaused(false));
			if (result != FMOD.RESULT.OK)
			{
				preparedChannel.stop();
				return;
			}

			_retiringChannel = FmodChannel;
			_preparedChannel = preparedChannel;
			_preparedIndex = nextIndex;
			_preparedShuffleQueueIndex = ((loop & LOOP_MODE.LOOP_RANDOM) != 0)
				? _shuffleQueueIndex + 1
				: -1;
			_scheduledTransitionClock = startClock;
			_pendingGaplessTransition = true;
			_preparedChannelEnded = false;
			_retiringChannelEnded = false;
			ApplyCurrentXArkMidiLoopMode();
		}

		public FMOD.RESULT PlaySound(int index)
		{
			if (!IsValidIndex(index))
				return FMOD.RESULT.OK;

			ClearPreparedTransition(true);
			ReleaseNonStopFadingIfDone();
			StartCrossfadeOrStop();
			CrossfadeTriggered = false;

			// クロスフェード中の旧チャンネルが使うサウンドは解放しない
			int fadingIndex = _fadingPlayListIndex;

			for (int i = 0; i < PlayList.Count; i++)
			{
				if (i == index) continue;
				if (i == index + 1) continue;
				if (i == fadingIndex) continue;
				if (i == _preparedIndex) continue;
				if (PlayList[i].IsPcm) continue; // PCM トラックは再ロード不可のため常に保護
				if (PlayList[i].IsLoaded)
				{
					ReleasePlaylistSound(PlayList[i]);
				}
			}

			var loadResult = LoadSound(index);
			if (loadResult != FMOD.RESULT.OK) return loadResult;

			PlayingIndex = index;
			var result = FmodCallFunction(FmodSystem.playSound(
				PlayList[index].Sound, FmodChannelGroup, true, out FmodChannel));

			if (result == FMOD.RESULT.OK)
				ApplyOutputSettingsToCurrentChannel(index);
			if (result == FMOD.RESULT.OK)
				ApplyCurrentXArkMidiLoopMode();

			// CUEトラック: 開始位置にシーク
			if (result == FMOD.RESULT.OK && PlayList[index].IsCueTrack)
			{
				uint cueStart = (uint)PlayList[index].CueStartMs.Value;
				if (PlayList[index].XArkMidiStream != null)
					PlayList[index].XArkMidiStream.SeekMilliseconds(cueStart);
				else
					FmodChannel.setPosition(cueStart, FMOD.TIMEUNIT.MS);
			}

			_nowPlaying = true;
			if (result == FMOD.RESULT.OK && NonStopMixEnabled && !CrossfadeEnabled)
				PrepareGaplessTransition(index);

			FmodChannel.setPaused(false);
			return result;
		}

		private void ApplyReplayGain(int index)
		{
			if (!FmodChannel.hasHandle())
				return;

			if (!IsValidIndex(index))
				return;

			var entry = PlayList[index];

			// モードに応じてゲイン値を選択（アルバム優先 or トラック優先）
			float? gainDb = ReplayGainMode == 1
				? (entry.ReplayGainAlbum ?? entry.ReplayGainTrack)
				: (entry.ReplayGainTrack ?? entry.ReplayGainAlbum);

			if (gainDb == null)
				return;

			// dB → 線形変換： volume = 10 ^ ((gainDb + preamp) / 20)
			float totalDb = gainDb.Value + ReplayGainPreamp;
			float linearGain = (float)Math.Pow(10.0, totalDb / 20.0);

			// マスター音量と合算してクリッピング防止
			float finalVolume = Math.Min(_masterVolume * linearGain, 1.0f);

			FmodChannel.setVolume(finalVolume);
		}

		private void ApplyOutputSettingsToCurrentChannel(int index)
		{
			if (!FmodChannel.hasHandle())
				return;

			if (ReplayGainEnabled && IsValidIndex(index))
				ApplyReplayGain(index);
			else
				FmodChannel.setVolume(_masterVolume);

			FmodChannel.setPan(_masterPan);
		}

		public FMOD.RESULT PlaySoundPaused(int index, uint position = 0)
		{
			if (!IsValidIndex(index))
				return FMOD.RESULT.OK;

			ClearPreparedTransition(true);
			if (FmodChannel.hasHandle())
				FmodChannel.stop();

			var loadResult = LoadSound(index);
			if (loadResult != FMOD.RESULT.OK)
				return loadResult;

			PlayingIndex = index;

			// paused=true で再生開始することで位置だけ設定し、音を出さない
			var result = FmodCallFunction(FmodSystem.playSound(
				PlayList[index].Sound, FmodChannelGroup, true, out FmodChannel));

			if (result == FMOD.RESULT.OK)
			{
				uint maxPosition = PlayList[index].IsCueTrack
					? PlayList[index].LengthMs
					: GetLength(index);
				if (maxPosition > 0)
					position = Math.Min(position, maxPosition);

				uint seekPos = position;
				// CUEトラック: 相対位置を絶対位置に変換
				if (PlayList[index].IsCueTrack && PlayList[index].CueStartMs.HasValue)
					seekPos = (uint)PlayList[index].CueStartMs.Value + position;
				if (PlayList[index].XArkMidiStream != null)
					PlayList[index].XArkMidiStream.SeekMilliseconds(seekPos);
				else if (seekPos > 0)
					FmodChannel.setPosition(seekPos, FMOD.TIMEUNIT.MS);

				ApplyOutputSettingsToCurrentChannel(index);
				ApplyCurrentXArkMidiLoopMode();
			}

			_nowPlaying = true;
			return result;
		}
		public uint GetLength(int index)
		{
			uint length = 0;
			if (index >= PlayList.Count || index < 0)
				return 0;

			if (PlayList[index].IsCueTrack)
				return PlayList[index].LengthMs;

			if (PlayList[index].XArkMidiStream != null)
			{
				if (PlayList[index].LengthMs > 0)
					return PlayList[index].LengthMs;
				return PlayList[index].XArkMidiStream.EstimatedLengthMs;
			}

			FmodCallFunction(PlayList[index].Sound.getLength(out length, TIMEUNIT.MS));

			// FMOD で取得できない場合（ストリームなど）は ATL で読み込んだ値を使う
			if (length == 0 || length == 0xFFFFFFFF)
				length = PlayList[index].LengthMs;

			return length;
		}

		/// <summary>
		/// ファイルをプレイリストに追加する。タグ情報はバックグラウンドで取得する。
		/// URL の場合はタグ取得をスキップする。
		/// </summary>
		public RESULT CreateSound(string filename, out int index)
		{
			index = 0;

			// CUEシートは専用メソッドで処理する
			if (Path.GetExtension(filename).Equals(".cue", StringComparison.OrdinalIgnoreCase))
				return CreateCueSounds(filename, out index);

			if (!filename.StartsWith("http://") && !filename.StartsWith("https://"))
			{
				var existing = PlayList.FirstOrDefault(p => p.FileName == filename);

				if (existing != null)
				{
					index = PlayList.IndexOf(existing);
					return FMOD.RESULT.OK;
				}

				var plist = new Engine.Player.PlayList(filename);
				PlayList.Add(plist);
				index = PlayList.Count - 1;

				// ★バックグラウンドでタグ・長さを取得
				int capturedIndex = index;
				_ = LoadTagsOnlyAsync(capturedIndex);
			}

			return FMOD.RESULT.OK;
		}
		/// <summary>
		/// CUEシートを解析してプレイリストにトラックを追加する。
		/// タグはCUEから直接設定し、タグ取得・波形解析は行わない。
		/// </summary>
		private FMOD.RESULT CreateCueSounds(string cuePath, out int firstIndex)
		{
			firstIndex = 0;

			// 既に同じCUEファイルが追加済みなら最初のトラックを返す
			var existing = PlayList.FirstOrDefault(
				p => p.IsCueTrack && string.Equals(
					p.CueSheetRef?.CuePath, cuePath, StringComparison.OrdinalIgnoreCase));
			if (existing != null)
			{
				firstIndex = PlayList.IndexOf(existing);
				return FMOD.RESULT.OK;
			}

			CUE.CueSheet sheet;
			try { sheet = CUE.CueParser.Parse(cuePath); }
			catch { return FMOD.RESULT.ERR_FILE_BAD; }

			if (sheet.Tracks.Count == 0)
				return FMOD.RESULT.ERR_FILE_BAD;

			// 音声ファイルの総再生時間を取得（最終トラックの長さ計算に使用）
			string lastAudioFile = sheet.IsMultiFile
				? (sheet.Tracks[sheet.Tracks.Count - 1].AudioFile ?? sheet.AudioPath)
				: sheet.AudioPath;

			int fileDurationMs = 0;
			if (!string.IsNullOrEmpty(lastAudioFile) && File.Exists(lastAudioFile))
			{
				try { fileDurationMs = (int)new ATL.Track(lastAudioFile).DurationMs; }
				catch { }
			}
			sheet.TotalDurationMs = fileDurationMs;

			bool first = true;
			foreach (var track in sheet.Tracks)
			{
				string audioFile = sheet.IsMultiFile
					? (track.AudioFile ?? sheet.AudioPath)
					: sheet.AudioPath;

				if (string.IsNullOrEmpty(audioFile) || !File.Exists(audioFile))
					continue;

				uint lengthMs;
				int? cueEnd;
				if (track.EndMs >= 0)
				{
					lengthMs = (uint)(track.EndMs - track.StartMs);
					cueEnd = track.EndMs;
				}
				else if (fileDurationMs > 0)
				{
					lengthMs = (uint)(fileDurationMs - track.StartMs);
					cueEnd = null; // ファイル末尾はFMODに任せる
				}
				else
				{
					lengthMs = 0;
					cueEnd = null;
				}

				var plist = new PlayList(audioFile);
				plist.Title = !string.IsNullOrEmpty(track.Title)
					? track.Title
					: $"Track {track.Number:D2}";
				plist.Artist = track.Performer ?? sheet.Performer ?? "";
				plist.Album = sheet.Title ?? "";
				plist.SetLength(lengthMs);
				plist.CueStartMs = track.StartMs;
				plist.CueEndMs = cueEnd;
				plist.CueSheetRef = sheet;

				PlayList.Add(plist);
				if (first)
				{
					firstIndex = PlayList.Count - 1;
					first = false;
				}
			}

			return first ? FMOD.RESULT.ERR_FILE_BAD : FMOD.RESULT.OK;
		}

		private async Task LoadTagsOnlyAsync(int index)
		{
			await _tagLoadSemaphore.WaitAsync();

			if (!IsValidIndex(index))
				return;

			try
			{
				string filename = PlayList[index].FileName;

				await Task.Run(() =>
				{
					try
					{
						var track = new ATL.Track(filename);
						PlayList[index].Title = track.Title;
						PlayList[index].Artist = track.Artist;
						PlayList[index].Album = track.Album;
						PlayList[index].Year = track.Year ?? 0;
						PlayList[index].TrackNumber = track.TrackNumber ?? 0;
						PlayList[index].TrackTotal = track.TrackTotal ?? 0;
						PlayList[index].SetLength((uint)track.DurationMs);

						// ATL の AdditionalFields に "REPLAYGAIN_TRACK_GAIN" などが格納される
						if (track.AdditionalFields.TryGetValue("REPLAYGAIN_TRACK_GAIN", out string tGain))
							PlayList[index].ReplayGainTrack = ParseReplayGainDb(tGain);

						if (track.AdditionalFields.TryGetValue("REPLAYGAIN_ALBUM_GAIN", out string aGain))
							PlayList[index].ReplayGainAlbum = ParseReplayGainDb(aGain);
					}
					catch { }
				});
			}
			catch { }
			finally
			{
				_tagLoadSemaphore.Release();
			}

		}

		public RESULT GetTag(string name, int index, out FMOD.TAG tag)
		{
			tag = new FMOD.TAG();
			if (!IsValidIndex(index))
				return RESULT.ERR_INVALID_PARAM;
			var sound = PlayList[index].Sound;
			if (!sound.hasHandle())
				return RESULT.ERR_INVALID_HANDLE;
			return FmodCallFunction(sound.getTag(name, 0, out tag));
		}
		public void DumpTags(int index)
		{
			if (!IsValidIndex(index))
			{
				System.Diagnostics.Debug.WriteLine($"[TAG] invalid index: {index}");
				return;
			}

			var sound = PlayList[index].Sound;
			if (!sound.hasHandle())
			{
				System.Diagnostics.Debug.WriteLine($"[TAG] no sound handle: {index}");
				return;
			}

			if (sound.getNumTags(out int numTags, out int numUpdated) != FMOD.RESULT.OK)
			{
				System.Diagnostics.Debug.WriteLine($"[TAG] getNumTags failed: {index}");
				return;
			}

			System.Diagnostics.Debug.WriteLine($"[TAG] index={index} numTags={numTags} updated={numUpdated}");

			for (int i = 0; i < numTags; i++)
			{
				var result = sound.getTag(null, i, out FMOD.TAG tag);
				if (result != FMOD.RESULT.OK)
				{
					System.Diagnostics.Debug.WriteLine($"[TAG] getTag failed: i={i} result={result}");
					continue;
				}

				string type = tag.type.ToString();
				string dataType = tag.datatype.ToString();
				string name = (string)tag.name ?? "";
				string valueInfo;

				if (tag.datatype == FMOD.TAGDATATYPE.STRING || tag.datatype == FMOD.TAGDATATYPE.STRING_UTF8)
				{
					string text = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(tag.data) ?? "";
					valueInfo = text;
				}
				else if (tag.datatype == FMOD.TAGDATATYPE.BINARY)
				{
					byte[] bytes = new byte[tag.datalen];
					System.Runtime.InteropServices.Marshal.Copy(tag.data, bytes, 0, (int)tag.datalen);
					int take = Math.Min(bytes.Length, 32);
					valueInfo = BitConverter.ToString(bytes, 0, take);
					if (bytes.Length > 32) valueInfo += "...";
				}
				else
				{
					valueInfo = $"data=0x{tag.data.ToString("X")}, len={tag.datalen}";
				}

				System.Diagnostics.Debug.WriteLine(
						$"[TAG] #{i} type={type} datatype={dataType} name={name} len={tag.datalen} updated={tag.updated} value={valueInfo}");
			}
		}
		/// <summary>"-6.54 dB" 形式の文字列を float に変換する。解析失敗時は null を返す。</summary>
		private static float? ParseReplayGainDb(string value)
		{
			if (string.IsNullOrEmpty(value)) return null;
			string num = value.Replace("dB", "").Trim();
			if (float.TryParse(num,
				System.Globalization.NumberStyles.Float,
				System.Globalization.CultureInfo.InvariantCulture,
				out float result))
				return result;
			return null;
		}
		/// <summary>
		/// メモリ上のPCMデータからSoundを生成してプレイリストへ追加する。
		/// CDDA固定：44100Hz / ステレオ / 16bit
		/// </summary>
		public FMOD.RESULT CreateSoundFromPCM(byte[] pcmData, string title, out int index)
		{
			index = 0;
			FMOD.CREATESOUNDEXINFO info = new FMOD.CREATESOUNDEXINFO();
			info.cbsize = Marshal.SizeOf(info);
			info.length = (uint)pcmData.Length;
			info.numchannels = 2;
			info.defaultfrequency = 44100;
			info.format = FMOD.SOUND_FORMAT.PCM16;

			FMOD.Sound sound;
			var result = FmodCallFunction(FmodSystem.createSound(
				pcmData,
				FMOD.MODE.OPENMEMORY | FMOD.MODE.OPENRAW | FMOD.MODE._2D | FMOD.MODE.CREATESAMPLE,
				ref info,
				out sound));

			if (result == FMOD.RESULT.OK)
			{
				var plist = new Engine.Player.PlayList(title, sound);
				plist.IsPcm = true;
				PlayList.Add(plist);
				index = PlayList.Count - 1;
				_ = StartWaveformAnalysisFromSoundAsync(index);
			}
			return result;
		}
		private bool _fluidSynthAvailable = false;
		public bool FluidSynthAvailable => _fluidSynthAvailable;
		private bool _bassMidiAvailable = false;
		public bool BassMidiAvailable => _bassMidiAvailable;
		private bool _xarkMidiAvailable = false;
		public bool XArkMidiAvailable => _xarkMidiAvailable;

		private static readonly HashSet<string> _trackerExtensions = new HashSet<string>
			{
				".mod", ".xm", ".it", ".s3m"
			};
		private FMOD.RESULT LoadSound(int index)
		{
			if (!IsValidIndex(index))
				return FMOD.RESULT.ERR_INVALID_PARAM;

			// 既にロード済みの場合はスキップ
			if (PlayList[index].IsLoaded)
				return FMOD.RESULT.OK;
			PlayList[index].XArkMidiStream?.Dispose();
			PlayList[index].XArkMidiStream = null;

			string filename = PlayList[index].FileName;
			FMOD.Sound sound;
			FMOD.RESULT result;
			FMOD.CREATESOUNDEXINFO info = new FMOD.CREATESOUNDEXINFO();
			info.cbsize = Marshal.SizeOf(info);
			string ext = Path.GetExtension(filename).ToLower();
			if (ext == ".mid" || ext == ".midi")
			{
				if (!string.IsNullOrEmpty(_soundFontPath) && File.Exists(_soundFontPath))
				{
					var rendererBackend = ResolveMidiRendererBackend();
					if (rendererBackend == MidiRendererBackend.XArkMidi)
					{
						try
						{
							var options = XArkMidiEngine.CreateOptions.Default();
							options.CompatibilityFlags = XArkMidiEngine.CompatibilityFlags.Sf2ZeroLengthLoopRetrigger;
							options.MaxSampleDataBytes = 4096ul * 1024ul * 1024ul; // 大容量 SF2 向けに 4GB まで展開を許可
							var stream = new XArkMidiFmodStream(
								filename,
								_soundFontPath,
								XArkMidiEngine.SoundBankKind.Auto,
								44100,
								2,
								options);
							result = CreateXArkMidiStreamSound(stream, out sound);
							if (result == FMOD.RESULT.OK)
							{
								PlayList[index].XArkMidiStream = stream;
								return StoreXArkMidiStreamSound(index, sound, result);
							}
							stream.Dispose();
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine(
								$"[XArkMidi] Render failed midi=\"{filename}\" sf2=\"{_soundFontPath}\" error=\"{ex}\"");
							ErrorOccurred?.Invoke(this, new PlayerErrorEventArgs(
								nameof(LoadSound),
								$"X-Ark Midi Engine error: {ex.Message}",
								-1));
						}
					}
					else if (rendererBackend == MidiRendererBackend.BassMidi)
					{
						try
						{
							using (var renderer = new BassMidiRenderer())
							{
								var pcm = renderer.Render(filename, _soundFontPath);
								result = CreateMidiPcmSound(pcm, out sound);
								if (result == FMOD.RESULT.OK)
									return StoreMidiPcmSound(index, sound, result);
							}
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine(
								$"[BASSMIDI] Render failed midi=\"{filename}\" sf2=\"{_soundFontPath}\" error=\"{ex}\"");
							ErrorOccurred?.Invoke(this, new PlayerErrorEventArgs(
								nameof(LoadSound),
								$"BASSMIDI error: {ex.Message}",
							-1));
						}
					}
					else if (rendererBackend == MidiRendererBackend.FluidSynth)
					{
						try
						{
							using (var renderer = new FluidSynthMidiRenderer())
							{
								System.Diagnostics.Debug.WriteLine(
									$"[FluidSynth] Rendering MIDI via FluidSynth midi=\"{filename}\" sf2=\"{_soundFontPath}\"");
								var pcm = renderer.Render(filename, _soundFontPath);
								System.Diagnostics.Debug.WriteLine(
									$"[FluidSynth] Render completed profile={renderer.LastProfileUsed} pcmSize={pcm?.Length ?? 0}");
								result = CreateMidiPcmSound(pcm, out sound);
								if (result == FMOD.RESULT.OK)
									return StoreMidiPcmSound(index, sound, result);
							}
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine(
								$"[FluidSynth] Render failed midi=\"{filename}\" sf2=\"{_soundFontPath}\" error=\"{ex}\"");
							ErrorOccurred?.Invoke(this, new PlayerErrorEventArgs(
								nameof(LoadSound),
								$"FluidSynth error: {ex.Message}",
							-1));
						}
					}
				}

				info.suggestedsoundtype = FMOD.SOUND_TYPE.MIDI;
				IntPtr dlsPtr = IntPtr.Zero;
				string dlsPath = null;
				if (string.IsNullOrEmpty(_soundFontPath) || !File.Exists(_soundFontPath))
				{
					// サウンドフォント未設定 → Windows標準の gm.dls をフォールバックとして使用
					var systemGmDls = Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.System),
						"drivers", "gm.dls");
					if (File.Exists(systemGmDls))
						dlsPath = systemGmDls;
				}
				else if (!_fluidSynthAvailable && !_bassMidiAvailable && !_xarkMidiAvailable)
				{
					// サウンドフォント設定済み + .dls形式 + レンダラー全不在 → そのDLSをFMODに渡す
					if (Path.GetExtension(_soundFontPath).ToLower() == ".dls")
						dlsPath = _soundFontPath;
				}
				if (dlsPath != null)
				{
					dlsPtr = Marshal.StringToHGlobalAnsi(dlsPath);
					info.dlsname = dlsPtr;
				}
				try
				{
					result = FmodCallFunction(FmodSystem.createSound(
						filename, FMOD.MODE.DEFAULT, ref info, out sound));
				}
				finally
				{
					if (dlsPtr != IntPtr.Zero)
						Marshal.FreeHGlobal(dlsPtr);
				}
			}
			// .bin は生PCMとして開く
			else if (ext == ".bin")
			{
				info.format = FMOD.SOUND_FORMAT.PCM16;
				info.numchannels = 2;
				info.defaultfrequency = 44100;
				result = FmodCallFunction(FmodSystem.createStream(
					filename,
					FMOD.MODE.OPENRAW | FMOD.MODE.DEFAULT,
					ref info,
					out sound));
			}
			else if (_trackerExtensions.Contains(ext))
			{
				// トラッカー形式はストリームではなく createSound でメモリに展開する
				result = FmodCallFunction(FmodSystem.createSound(
					filename, FMOD.MODE.DEFAULT | FMOD.MODE.ACCURATETIME | FMOD.MODE.LOOP_OFF, ref info, out sound));
			}
			else
			{
				result = FmodCallFunction(FmodSystem.createStream(
					filename, FMOD.MODE.DEFAULT, ref info, out sound));
			}

			if (result == FMOD.RESULT.OK)
			{
				PlayList[index].Sound = sound;
				// CUEトラックは全体ファイルの解析は不要
				if (!PlayList[index].IsCueTrack)
				{
					if (_trackerExtensions.Contains(ext))
						_ = StartWaveformAnalysisFromSoundAsync(index);
					else if (ext != ".mid" && ext != ".midi")
						_ = StartWaveformAnalysisAsync(filename, index);
					// FMODネイティブMIDIはseekData非対応のため波形解析スキップ
				}
			}
			return result;
		}

		private MidiRendererBackend ResolveMidiRendererBackend()
		{
			switch (_midiRendererBackend)
			{
				case MidiRendererBackend.XArkMidi:
					if (_xarkMidiAvailable)   return MidiRendererBackend.XArkMidi;
					if (_bassMidiAvailable)   return MidiRendererBackend.BassMidi;
					if (_fluidSynthAvailable) return MidiRendererBackend.FluidSynth;
					break;
				case MidiRendererBackend.BassMidi:
					if (_bassMidiAvailable)   return MidiRendererBackend.BassMidi;
					if (_fluidSynthAvailable) return MidiRendererBackend.FluidSynth;
					if (_xarkMidiAvailable)   return MidiRendererBackend.XArkMidi;
					break;
				case MidiRendererBackend.FluidSynth:
					if (_fluidSynthAvailable) return MidiRendererBackend.FluidSynth;
					if (_bassMidiAvailable)   return MidiRendererBackend.BassMidi;
					if (_xarkMidiAvailable)   return MidiRendererBackend.XArkMidi;
					break;
				default: // Auto: BASSMIDI > FluidSynth > XArkMidi
					if (_bassMidiAvailable)   return MidiRendererBackend.BassMidi;
					if (_fluidSynthAvailable) return MidiRendererBackend.FluidSynth;
					if (_xarkMidiAvailable)   return MidiRendererBackend.XArkMidi;
					break;
			}

			return MidiRendererBackend.Auto;
		}

		private FMOD.RESULT CreateMidiPcmSound(byte[] pcm, out FMOD.Sound sound)
		{
			sound = default;
			if (pcm == null || pcm.Length == 0)
				return FMOD.RESULT.ERR_FILE_BAD;

			FMOD.CREATESOUNDEXINFO pcmInfo = new FMOD.CREATESOUNDEXINFO();
			pcmInfo.cbsize = Marshal.SizeOf(pcmInfo);
			pcmInfo.length = (uint)pcm.Length;
			pcmInfo.numchannels = 2;
			pcmInfo.defaultfrequency = 44100;
			pcmInfo.format = FMOD.SOUND_FORMAT.PCM16;

			return FmodCallFunction(FmodSystem.createSound(
				pcm,
				FMOD.MODE.OPENMEMORY | FMOD.MODE.OPENRAW |
				FMOD.MODE._2D | FMOD.MODE.CREATESAMPLE,
				ref pcmInfo,
				out sound));
		}

		private FMOD.RESULT CreateXArkMidiStreamSound(XArkMidiFmodStream stream, out FMOD.Sound sound)
		{
			sound = default;
			FMOD.CREATESOUNDEXINFO streamInfo = new FMOD.CREATESOUNDEXINFO();
			streamInfo.cbsize = Marshal.SizeOf(streamInfo);
			streamInfo.length = stream.FmodStreamLengthBytes;
			streamInfo.numchannels = stream.ChannelCount;
			streamInfo.defaultfrequency = stream.SampleRate;
			streamInfo.format = FMOD.SOUND_FORMAT.PCM16;
			streamInfo.decodebuffersize = stream.DecodeBufferFrames;
			streamInfo.pcmreadcallback = stream.PcmReadCallback;
			streamInfo.pcmsetposcallback = stream.PcmSetPosCallback;
			streamInfo.userdata = stream.UserData;

			return FmodCallFunction(FmodSystem.createStream(
				IntPtr.Zero,
				FMOD.MODE.OPENUSER | FMOD.MODE.OPENRAW | FMOD.MODE._2D,
				ref streamInfo,
				out sound));
		}

		private FMOD.RESULT StoreMidiPcmSound(int index, FMOD.Sound sound, FMOD.RESULT result)
		{
			if (result == FMOD.RESULT.OK)
			{
				PlayList[index].Sound = sound;
				_ = StartWaveformAnalysisFromSoundAsync(index);
			}
			return result;
		}

		private FMOD.RESULT StoreXArkMidiStreamSound(int index, FMOD.Sound sound, FMOD.RESULT result)
		{
			if (result == FMOD.RESULT.OK)
			{
				PlayList[index].Sound = sound;
				sound.setUserData(PlayList[index].XArkMidiStream.UserData);
				ApplyXArkMidiLoopMode(index);
			}
			return result;
		}

		private void ReleasePlaylistSound(PlayList entry)
		{
			if (entry == null)
				return;

			try
			{
				if (entry.Sound.hasHandle())
					entry.Sound.release();
			}
			finally
			{
				entry.Sound = default;
				entry.XArkMidiStream?.Dispose();
				entry.XArkMidiStream = null;
			}
		}

		private void ApplyXArkMidiLoopMode(int index)
		{
			if (!IsValidIndex(index))
				return;

			var stream = PlayList[index].XArkMidiStream;
			if (stream == null)
				return;

			try
			{
				stream.SetLoop((_loop & LOOP_MODE.LOOP_ONE_REPEAT) != 0);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(
					$"[XArkMidi] SetLoop failed file=\"{PlayList[index].FileName}\" error=\"{ex}\"");
				ErrorOccurred?.Invoke(this, new PlayerErrorEventArgs(
					nameof(ApplyXArkMidiLoopMode),
					$"X-Ark Midi Engine loop error: {ex.Message}",
					-1));
			}
		}

		private void ApplyCurrentXArkMidiLoopMode()
		{
			ApplyXArkMidiLoopMode(PlayingIndex);
			if (_preparedIndex >= 0)
				ApplyXArkMidiLoopMode(_preparedIndex);
		}

		private void ReportCurrentXArkMidiStreamError(string callerMethodName)
		{
			if (!IsValidIndex(PlayingIndex))
				return;

			var stream = PlayList[PlayingIndex].XArkMidiStream;
			if (stream == null || !stream.TryTakeLastError(out Exception error))
				return;

			System.Diagnostics.Debug.WriteLine(
				$"[XArkMidi] Stream callback error file=\"{PlayList[PlayingIndex].FileName}\" error=\"{error}\"");
			ErrorOccurred?.Invoke(this, new PlayerErrorEventArgs(
				callerMethodName,
				$"X-Ark Midi Engine stream error: {error.Message}",
				-1));
		}

		private async System.Threading.Tasks.Task StartWaveformAnalysisAsync(
			string filename, int index)
		{
			if (!IsValidIndex(index))
				return;

			// 前回の曲の解析をキャンセルしてから新しい解析を開始する
			_waveformCts?.Cancel();
			_waveformCts?.Dispose();
			_waveformCts = new System.Threading.CancellationTokenSource();
			var ct = _waveformCts.Token;

			await _waveformAnalyzer.AnalyzeAsync(
				filename,
				PlayList[index],
				entry =>
				{
					int idx = PlayList.IndexOf(entry);
					if (idx >= 0)
						WaveformReady?.Invoke(idx);
				},
				ct);
		}
		private async System.Threading.Tasks.Task StartWaveformAnalysisFromSoundAsync(int index)
		{
			if (!IsValidIndex(index))
				return;

			var entry = PlayList[index];
			if (!entry.IsLoaded)
				return;

			_waveformCts?.Cancel();
			_waveformCts?.Dispose();
			_waveformCts = new System.Threading.CancellationTokenSource();
			var ct = _waveformCts.Token;

			await _waveformAnalyzer.AnalyzeFromSoundAsync(
				entry.Sound,
				entry,
				e =>
				{
					int idx = PlayList.IndexOf(e);
					if (idx >= 0)
						WaveformReady?.Invoke(idx);
				},
				ct);
		}
		/// <summary>
		/// プレイリストを全消去する。
		/// </summary>
		public void ClearPlayList()
		{
			Stop();
			for (int i = 0; i < PlayList.Count; i++)
			{
				if (PlayList[i].IsLoaded)
					ReleasePlaylistSound(PlayList[i]);
			}
			PlayList.Clear();
		}

		public void PlayNext(int fromIndex, bool manual = false)
		{
			if (PlayList.Count == 0) return;

			if (fromIndex < 0)
				fromIndex = Math.Min(PlayingIndex, PlayList.Count - 1);

			if (manual)
				Stop();

			ClearPreparedTransition(true);
			if (!TryResolveNextIndex(fromIndex, out int next, true))
			{
				_nowPlaying = false;
				return;
			}
			PlaySound(next);
		}
		public void Sort<T>(Func<Engine.Player.PlayList, T> keySelector)
		{
			var playingItem = IsValidIndex(PlayingIndex)
				? PlayList[PlayingIndex]
				: null;

			var sorted = PlayList.OrderBy(keySelector).ToList();
			PlayList.Clear();
			foreach (var item in sorted)
				PlayList.Add(item);

			// private set なので内部で更新可能
			if (playingItem != null)
				PlayingIndex = PlayList.IndexOf(playingItem);
		}

		public void PlayPrevious(int fromIndex = -1, bool manual = false)
		{
			if (PlayList.Count == 0)
				return;
			if (fromIndex < 0)
				fromIndex = Math.Max(0, PlayingIndex);
			if (manual) Stop();
			ClearPreparedTransition(true);
			if ((loop & LOOP_MODE.LOOP_RANDOM) != 0)
			{
				// シャッフルキューを 1 つ戻る（最低 0 まで）
				_shuffleQueueIndex = Math.Max(0, _shuffleQueueIndex - 2);
				if (_shuffleQueueIndex < _shuffleQueue.Count)
					PlaySound(_shuffleQueue[_shuffleQueueIndex++]);
				return;
			}

			int prev;
			switch (loop)
			{
				case LOOP_MODE.LOOP_ALL:
					prev = (fromIndex > 0) ? fromIndex - 1 : PlayList.Count - 1;
					break;
				default:
					prev = Math.Max(0, fromIndex - 1);
					break;
			}
			PlaySound(prev);
		}
		/// <summary>再生を停止する。クロスフェード用の退避チャンネルも解放する。</summary>
		public void Stop()
		{
			_nowPlaying = false;
			//			PlayingIndex = -1;
			if (FmodChannel.hasHandle())
				FmodChannel.stop();
			ClearPreparedTransition(true);

			if (FmodChannelFading.hasHandle())
				FmodChannelFading.stop();

			if (IsValidIndex(_fadingPlayListIndex)
				&& PlayList[_fadingPlayListIndex].IsLoaded
				&& !PlayList[_fadingPlayListIndex].IsPcm)
			{
				ReleasePlaylistSound(PlayList[_fadingPlayListIndex]);
			}

			FmodChannelFading = default;
			_isCrossfading = false;
			_isCrossfadeVolumeFixed = false;
			_crossfadeElapsedMs = 0;
		}

		/// <summary>マスター音量を設定する（0.0〜1.0）</summary>
		public void SetVolume(float vol)
		{
			UpdateScheduledTransition();
			_masterVolume = vol;
			if (FmodChannel.hasHandle())
				FmodChannel.setVolume(vol);
			if (_preparedChannel.hasHandle())
				_preparedChannel.setVolume(vol);
			// フェードアウト中の旧チャンネルの音量には干渉しない
		}

		public int GetVolume()
		{
			UpdateScheduledTransition();
			float volume;
			if (!FmodChannel.hasHandle())
				return (int)(_masterVolume * 100);
			FmodChannel.getVolume(out volume);
			return (int)(volume * 100);
		}
		/// <summary>パンを設定する（-1.0〜1.0）</summary>
		public void SetPan(float pan)
		{
			UpdateScheduledTransition();
			_masterPan = pan;
			if (FmodChannel.hasHandle())
				FmodChannel.setPan(pan);
			if (_preparedChannel.hasHandle())
				_preparedChannel.setPan(pan);
		}

		/// <summary>
		/// 指定した出力タイプのデバイス一覧を取得する。
		/// テンポラリなFMODシステムを使うためメインの再生には影響しない。
		/// </summary>
		public List<DEVICE_INFO> GetDeviceListForOutputType(FMOD.OUTPUTTYPE outputType)
		{
			var list = new List<DEVICE_INFO>();

			FMOD.System tempSystem;
			if (FMOD.Factory.System_Create(out tempSystem) != FMOD.RESULT.OK)
				return list;

			try
			{
				if (tempSystem.setOutput(outputType) != FMOD.RESULT.OK)
					return list;

				// 最小構成（1ch）で初期化してデバイス一覧を取得する
				if (tempSystem.init(1, FMOD.INITFLAGS.NORMAL, IntPtr.Zero) != FMOD.RESULT.OK)
					return list;

				int numDrivers = 0;
				if (tempSystem.getNumDrivers(out numDrivers) != FMOD.RESULT.OK)
					return list;

				for (int i = 0; i < numDrivers; i++)
				{
					var device = new DEVICE_INFO();
					device.namelen = 256;
					device.deviceId = i;
					if (tempSystem.getDriverInfo(i, out device.name, device.namelen,
						out device.guid, out device.systemrate,
						out device.speakermode, out device.speakerModeChannels) == FMOD.RESULT.OK)
					{
						list.Add(device);
					} else if (i == 0)
					{
						device.name = "Default Device";
						list.Add(device);
					}
				} 
			}
			finally
			{
				tempSystem.close();
				tempSystem.release();
			}

			return list;
		}

		/// <summary>
		/// 現在のFMODシステムのデバイスリストを直接取得する（再初期化不要）。
		/// ASIO起動中のASIOデバイス列挙に使用。
		/// </summary>
		public List<DEVICE_INFO> GetCurrentDeviceList()
		{
			var list = new List<DEVICE_INFO>();
			int numDrivers = 0;
			if (FmodSystem.getNumDrivers(out numDrivers) != FMOD.RESULT.OK)
				return list;

			for (int i = 0; i < numDrivers; i++)
			{
				var device = new DEVICE_INFO();
				device.namelen = 256;
				device.deviceId = i;
				if (FmodSystem.getDriverInfo(i, out device.name, device.namelen,
					out device.guid, out device.systemrate,
					out device.speakermode, out device.speakerModeChannels) == FMOD.RESULT.OK)
				{
					list.Add(device);
				}
			}
			return list;
		}

		public void BuildShuffleQueue()
		{
			_shuffleQueue = Enumerable.Range(0, PlayList.Count).ToList();

			// Fisher-Yates シャッフル
			for (int i = _shuffleQueue.Count - 1; i > 0; i--)
			{
				int j = _rng.Next(i + 1);
				(_shuffleQueue[i], _shuffleQueue[j]) = (_shuffleQueue[j], _shuffleQueue[i]);
			}
			_shuffleQueueIndex = 0;
		}

		public void UpdateShuffleQueueOnRemove(int removedIndex)
		{
			for (int i = _shuffleQueue.Count - 1; i >= 0; i--)
			{
				if (_shuffleQueue[i] == removedIndex)
				{
					_shuffleQueue.RemoveAt(i);
					if (i < _shuffleQueueIndex)
						_shuffleQueueIndex--;
				}
				else if (_shuffleQueue[i] > removedIndex)
				{
					_shuffleQueue[i]--;
				}
			}
		}

		public FMOD.RESULT PlayUrl(string url)
		{
			FMOD.Sound sound;
			FMOD.CREATESOUNDEXINFO info = new FMOD.CREATESOUNDEXINFO();
			info.cbsize = Marshal.SizeOf(info);

			var result = FmodCallFunction(FmodSystem.createStream(
				url, FMOD.MODE.DEFAULT | FMOD.MODE.NONBLOCKING, ref info, out sound));

			if (result != FMOD.RESULT.OK) return result;

			// URL ストリームはプレイリストに追加せず直接再生する
			var playResult = FmodCallFunction(FmodSystem.playSound(sound, FmodChannelGroup, true, out FmodChannel));
			if (playResult != FMOD.RESULT.OK)
				return playResult;

			FmodChannel.setVolume(_masterVolume);
			FmodChannel.setPan(_masterPan);
			FmodChannel.setPaused(false);

			_nowPlaying = true;
			return FMOD.RESULT.OK;
		}

		public Bitmap GetCoverArt(int index)
		{
			if (!IsValidIndex(index))
				return null;
			string filename = PlayList[index].FileName;
			if (string.IsNullOrEmpty(filename) || !File.Exists(filename)) return null;

			try
			{
				var track = new ATL.Track(filename);
				if (track.EmbeddedPictures?.Count > 0)
				{
					using (var ms = new MemoryStream(
						track.EmbeddedPictures[0].PictureData))
						return new Bitmap(ms);
				}
			}
			catch { }
			return null;
		}
		private void StartCrossfadeOrStop()
		{
			if (NonStopMixEnabled)
			{
				// NonStopMix：フェードなし即切り替え。前回の退避チャンネルを強制解放する
				if (FmodChannelFading.hasHandle())
				{
					FmodChannelFading.stop();
					if (IsValidIndex(_fadingPlayListIndex)
						&& PlayList[_fadingPlayListIndex].IsLoaded)
					{
						if (!PlayList[_fadingPlayListIndex].IsPcm)
						{
							ReleasePlaylistSound(PlayList[_fadingPlayListIndex]);
						}
					}
					FmodChannelFading = default;
					_fadingPlayListIndex = -1;
				}

				// 現在のチャンネルは stop() せず退避し、自然終了に任せる
				if (FmodChannel.hasHandle() && IsPlaying())
				{
					_fadingPlayListIndex = PlayingIndex;
					FmodChannelFading = FmodChannel;
					FmodChannel = default;
				}
				else
				{
					if (FmodChannel.hasHandle()) FmodChannel.stop();
					_fadingPlayListIndex = -1;
				}
				return;
			}
			if (!CrossfadeEnabled || !FmodChannel.hasHandle() || !IsPlaying())
			{
				if (FmodChannel.hasHandle())
					FmodChannel.stop();
				_fadingPlayListIndex = -1;
				return;
			}

			// 現在のチャンネルをフェードアウト用に退避する
			if (FmodChannelFading.hasHandle())
			{
				// 前回のフェードが残っていたら先に停止してサウンドを解放する
				FmodChannelFading.stop();
				if (IsValidIndex(_fadingPlayListIndex)
					&& PlayList[_fadingPlayListIndex].IsLoaded
					&& !PlayList[_fadingPlayListIndex].IsPcm)
				{
					ReleasePlaylistSound(PlayList[_fadingPlayListIndex]);
				}
			}
			_fadingPlayListIndex = PlayingIndex;
			FmodChannelFading = FmodChannel;
			FmodChannel = default;
			_crossfadeElapsedMs = 0;
			_isCrossfading = true;
		}
		/// <summary>
		/// NonStopMix用：退避チャンネルが自然終了していたら解放する。
		/// PreciseTimerCallback から毎ティック呼ぶ。
		/// </summary>
		public void ReleaseNonStopFadingIfDone()
		{
			if (!NonStopMixEnabled) return;
			if (!FmodChannelFading.hasHandle()) return;

			bool isPlaying = false;
			FmodChannelFading.isPlaying(out isPlaying);
			if (!isPlaying)
			{
				FmodChannelFading.stop();
				if (IsValidIndex(_fadingPlayListIndex)
					&& PlayList[_fadingPlayListIndex].IsLoaded)
				{
					try
					{
						if (!PlayList[_fadingPlayListIndex].IsPcm)
						{
							ReleasePlaylistSound(PlayList[_fadingPlayListIndex]);
						}
					}
					catch { }
				}
				FmodChannelFading = default;
				_fadingPlayListIndex = -1;
			}
		}
		public void UpdateCrossfade(int elapsedMs)
		{
			if (!_isCrossfading) return;
			if (!FmodChannelFading.hasHandle() && !FmodChannel.hasHandle())
			{
				_isCrossfading = false;
				return;
			}

			_crossfadeElapsedMs += elapsedMs;
			float t = Math.Min((float)_crossfadeElapsedMs / CrossfadeDurationMs, 1.0f);
			if (_isCrossfadeVolumeFixed)
			{
				// NonStopMix：音量固定のまま並走させる
				if (FmodChannelFading.hasHandle())
					FmodChannelFading.setVolume(_masterVolume);
				if (FmodChannel.hasHandle())
					FmodChannel.setVolume(_masterVolume);
			}
			else
			{
				// クロスフェード：旧チャンネルをフェードアウト、新チャンネルをフェードイン
				if (FmodChannelFading.hasHandle())
					FmodChannelFading.setVolume(_masterVolume * (1.0f - t));

				if (FmodChannel.hasHandle())
					FmodChannel.setVolume(_masterVolume * t);
			}
			if (t >= 1.0f)
			{
				if (FmodChannelFading.hasHandle())
					FmodChannelFading.stop();

				if (_fadingPlayListIndex >= 0 && _fadingPlayListIndex < PlayList.Count
					&& PlayList[_fadingPlayListIndex].IsLoaded)
				{
					if (!PlayList[_fadingPlayListIndex].IsPcm)
					{
						ReleasePlaylistSound(PlayList[_fadingPlayListIndex]);
					}
				}

				FmodChannelFading = default;
				_fadingPlayListIndex = -1;
				_isCrossfading = false;
				_crossfadeElapsedMs = 0;
			}
		}

		/// <summary>
		/// インデックスがプレイリスト内に存在するかチェックする。存在しない場合は再生操作をスキップするために使用。
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private bool IsValidIndex(int index)
		{
			return index >= 0 && index < PlayList.Count;
		}
	}
}
