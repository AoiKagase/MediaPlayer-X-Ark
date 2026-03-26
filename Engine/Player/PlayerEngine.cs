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

    public class PlayerEngine : IPlayerEngine
	{
		private bool _disposed = false;  // 二重解放防止フラグ
		private readonly SemaphoreSlim _tagLoadSemaphore = new SemaphoreSlim(3, 3);

        public FmodSpectrum spectrum { get; private set; }
        public FmodWave wave { get; private set; }
        public Effector.Effectors effector { get; private set; }
        public LOOP_MODE loop { get; set; }
        public event EventHandler<PlayerErrorEventArgs> ErrorOccurred;
        public int PlayingIndex { get; private set; } = -1;
        protected bool initialized = false;
        private bool _nowPlaying = false;
        public bool NowPlaying => _nowPlaying;
        // FMOD SYSTEM.
        public BindingList<Engine.Player.PlayList> PlayList { get; set; } = new BindingList<Engine.Player.PlayList>();
        protected FMOD.System FmodSystem;
		protected FMOD.ChannelGroup FmodChannelGroup;
		protected FMOD.Channel FmodChannel;

		// SOUND DEVICES.
		protected FMOD.OUTPUTTYPE FmodOutputType;


		protected uint FmodVersion;

		protected List<DEVICE_INFO> FmodDeviceList = new List<DEVICE_INFO>();

		private const int channelCount = 2;
		public int ChannelCount => channelCount;
		private List<int> _shuffleQueue = new List<int>();
        private int _shuffleQueueIndex = 0;
        private readonly Random _rng = new Random();

		// クロスフェード用フィールド
		private FMOD.Channel FmodChannelFading;   // フェードアウト中の旧チャンネル
		private int _fadingPlayListIndex = -1;  // フェードアウト中のPlayListインデックス
		private int _crossfadeElapsedMs = 0;    // フェード経過時間
		private bool _isCrossfading = false;
		private float _masterVolume = 1.0f; // SetVolume で設定された音量を保持
		public bool CrossfadeEnabled { get; set; } = false;
		public int CrossfadeDurationMs { get; set; } = 3000;
		public bool CrossfadeTriggered { get; set; } = false;
        public bool NonStopMixEnabled { get; set; } = false;
        // SF2パス
        private string _soundFontPath = "";
		private readonly object _fmodLock = new object();
		private WaveformAnalyzer _waveformAnalyzer;
		// キャンセル管理（曲が切り替わったら前の解析を中断）
		private System.Threading.CancellationTokenSource _waveformCts;
		public bool WaveformEnabled { get; set; } = false;
		public event Action<int> WaveformReady;
		public string SoundFontPath
		{
			get => _soundFontPath;
			set => _soundFontPath = value ?? "";
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

		/// <summary>
		/// Constructor
		/// </summary>
		public PlayerEngine()
        {
			CreateSystem();
//			Initialize();
		}

		/// <summary>
		/// Destructor
		/// </summary>
		// 既存のデストラクタはDisposeを呼ぶだけにする
		~PlayerEngine()
		{
			Dispose(false);
		}

		// 外部から明示的に呼ぶ用
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);  // デストラクタを呼ばせない
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) return;  // 二重解放防止
			if (initialized)
            {
                if (disposing)
				{
					spectrum?.Dispose();
                    wave?.Dispose();
					_tagLoadSemaphore?.Dispose(); 
				}

				// Relase FMOD handles for Channel.
				if (FmodChannel.hasHandle())
					FmodChannel.stop();
				_waveformCts?.Cancel();
				_waveformCts?.Dispose();
				if (FmodChannelFading.hasHandle())
					FmodChannelFading.stop();
				// Relase FMOD handles for ChannelGroup.
				if (FmodChannelGroup.hasHandle())
					FmodChannelGroup.release();

				// Relase FMOD handles for Sound.
				for (int i = 0; i < PlayList.Count; i++)
				{
					// ★IsLoadedチェックを追加
					if (PlayList[i].IsLoaded && PlayList[i].Sound.hasHandle())
						PlayList[i].Sound.release();
				}
				PlayList.Clear();

				// Relase FMOD handles for System.
				if (FmodSystem.hasHandle())
				{
					FmodSystem.close();
					FmodSystem.release();
				}
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
			// Initialize() 内の先頭に追加
			var fluidSynthPath = Path.Combine(
				AppDomain.CurrentDomain.BaseDirectory, "Libs", "fluidsynth.dll");
			_fluidSynthAvailable = File.Exists(fluidSynthPath);

			// System Create.
			{
				// Get Version.
				if (FmodCallFunction(FmodSystem.getVersion(out FmodVersion)) == FMOD.RESULT.OK)
				{
					// Version Check.
					if (FmodVersion != FMOD.VERSION.number)
					{
                        ErrorOccurred?.Invoke(this, new PlayerErrorEventArgs(
                            nameof(Initialize),
                            $"FMOD version mismatch. Found: {FmodVersion:X}, Required: {FMOD.VERSION.number:X}",
                            -1));
                        return;
					}
				}

				// init()より前にバッファ設定を適用
				if (bufferSettings != null)
				{
					FmodSystem.setStreamBufferSize(
						(uint)(bufferSettings.StreamBufferSizeKB * 1024),
						FMOD.TIMEUNIT.RAWBYTES);
					FmodSystem.setDSPBufferSize(
						(uint)bufferSettings.DspBufferSize,
						bufferSettings.DspBufferCount);
				}

				// System Init.
				if (FmodCallFunction(FmodSystem.init(channelCount, FMOD.INITFLAGS.NORMAL, IntPtr.Zero)) == RESULT.OK)
				{
					// Create Channel Group.
					if (FmodCallFunction(FmodSystem.createChannelGroup("Channel 01", out FmodChannelGroup)) == RESULT.OK)
                    {
						spectrum = new FmodSpectrum(ref FmodSystem, 1024, ref this.FmodChannelGroup);
						wave = new FmodWave(ref FmodSystem, ref FmodChannelGroup);

						LoadPlugins();

//						FmodCallFunction(FmodSystem.getChannel(0, out FmodChannel));

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
			uint handle;
			PLUGINTYPE plugintype;
			uint version;

			FmodCallFunction(FmodSystem.setPluginPath(".\\Plugins"));
			FmodCallFunction(FmodSystem.loadPlugin("codec_mp4.dll", out handle, 100));
			FmodSystem.getPluginInfo(handle, out plugintype, out version);
//			MessageBox.Show(lastError); 
			return;
        }

		/// <summary>
		/// Pause
		/// </summary>
		public void Pause()
        {
			bool paused;
			if (FmodCallFunction(FmodChannel.getPaused(out paused)) == RESULT.OK)
            {
				FmodCallFunction(FmodChannel.setPaused(!paused));
			}
		}

		/// <summary>
		/// Device Setting: Output Type
		/// </summary>
		/// <param name="outputtype">
		/// [x] AUTODETECT,			Picks the best output mode for the platform. This is the default.
		/// [ ] UNKNOWN,			All - 3rd party plugin, unknown. This is for use with System::getOutput only.
		/// [ ]	NOSOUND,			All - Perform all mixing but discard the final output.
		/// [ ] WAVWRITER,			All - Writes output to a .wav file.
		/// [ ] NOSOUND_NRT,		All - Non-realtime version of FMOD_OUTPUTTYPE_NOSOUND, one mix per System::update.
		/// [?] WAVWRITER_NRT,		All - Non-realtime version of FMOD_OUTPUTTYPE_WAVWRITER, one mix per System::update.
		///	[x] WASAPI,				Win / UWP / Xbox One / Game Core - Windows Audio Session API. (Default on Windows, Xbox One, Game Core and UWP)
		///	[x] ASIO,				Win - Low latency ASIO 2.0.
		/// [ ] PULSEAUDIO,			Linux - Pulse Audio. (Default on Linux if available)
		/// [ ] ALSA,				Linux - Advanced Linux Sound Architecture. (Default on Linux if PulseAudio isn't available)
		/// [ ]	COREAUDIO,			Mac / iOS - Core Audio. (Default on Mac and iOS)
		/// [ ] AUDIOTRACK,			Android - Java Audio Track. (Default on Android 2.2 and below)
		/// [ ]	OPENSL,				Android - OpenSL ES. (Default on Android 2.3 up to 7.1)
		/// [ ] AUDIOOUT,			PS4 / PS5 - Audio Out. (Default on PS4, PS5)
		/// [ ]	AUDIO3D,			PS4 - Audio3D.
		/// [ ] WEBAUDIO,			HTML5 - Web Audio ScriptProcessorNode output. (Default on HTML5 if AudioWorkletNode isn't available)
		/// [ ]	NNAUDIO,			Switch - nn::audio. (Default on Switch)
		/// [X] WINSONIC,			Win10 / Xbox One / Game Core - Windows Sonic.
		/// [ ]	AAUDIO,				Android - AAudio. (Default on Android 8.1 and above)
		/// [ ] AUDIOWORKLET,		HTML5 - Web Audio AudioWorkletNode output. (Default on HTML5 if available)
		/// [ ] MAX,				Maximum number of output types supported.
		/// OutputType設定。必ずInitialize()より前に呼ぶこと。
		/// </param>
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

		/// <summary>
		/// IsPlaying
		/// </summary>
		/// <returns>boolean</returns>
		public bool IsPlaying()
		{
			bool result = false;
			if (FmodChannel.hasHandle())
            {
				// STOPした際にFmodChannelの関数は
				// FMOD_ERR_INVALID_HANDLEを返すのでエラーチェックしない
				FmodChannel.isPlaying(out result);
				return result;
			}
			return false;
		}


		/// <summary>
		/// Get Device list. for SoundCards.
		/// </summary>
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
					}
				}
			}
		}

		/// <summary>
		/// Get Now selected device id;
		/// </summary>
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
			for(int i = 0; i < FmodDeviceList.Count(); i++)
            {
				if(FmodDeviceList[i].deviceId == driver)
                {
					return FmodDeviceList[i].GUID.ToString();
                }
            }
			return "";
		}
		/// <summary>
		/// Set selected device.
		/// </summary>
		/// <param name="driver">fmod device list number.</param>
		public void SetDevice(int driver)
        {
			FmodSystem.setDriver(driver);
        }

		/// <summary>
		/// Set selected device.
		/// </summary>
		/// <param name="driver">System GUID</param>
		public void SetDevice(string driver)
		{
			for(int i = 0; i < FmodDeviceList.Count(); i++)
            {
				if (FmodDeviceList[i].GUID.Equals(driver))
                {
					FmodSystem.setDriver(FmodDeviceList[i].deviceId);
					return;
				}
			}
		}
		public uint GetPosition()
        {
			uint position = 0;
			if (FmodChannel.hasHandle() && IsPlaying())
				FmodCallFunction(FmodChannel.getPosition(out position, TIMEUNIT.MS));
			return position;
        }
		public void SetPosition(uint position)
        {
			FmodCallFunction(FmodChannel.setPosition(position, TIMEUNIT.MS));
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

		public FMOD.RESULT PlaySound(int index)
		{
			if (index >= PlayList.Count) 
				return FMOD.RESULT.OK;

			// ★チャンネルが有効な場合のみstop（二重発火防止）
			StartCrossfadeOrStop();  // ← ここを変更
			CrossfadeTriggered = false;

			// クロスフェード中は旧チャンネルが使用中のサウンドを解放しないよう保護する
			int fadingIndex = _fadingPlayListIndex;  // フェードアウト中のインデックスを保持

			// Sound解放
			for (int i = 0; i < PlayList.Count; i++)
			{
				if (i == index) continue;
				if (i == index + 1) continue;
				if (i == fadingIndex) continue;  // ← フェードアウト中は解放しない
				if (PlayList[i].IsLoaded)
				{
					PlayList[i].Sound.release();
					PlayList[i].Sound = default;
				}
			}

			// ロード
			var loadResult = LoadSound(index);
			if (loadResult != FMOD.RESULT.OK) return loadResult;

			PlayingIndex = index;
			// クロスフェード時は音量0で開始してフェードイン
			float startVolume = CrossfadeEnabled && _isCrossfading ? 0f : _masterVolume;

			var result = FmodCallFunction(FmodSystem.playSound(
				PlayList[index].Sound, FmodChannelGroup, false, out FmodChannel));

			if (result == FMOD.RESULT.OK && ReplayGainEnabled)
				ApplyReplayGain(index);

			_nowPlaying = true;

			return result;
		}

		// ── ApplyReplayGain() 追加 ────────────────────────────────────────
		private void ApplyReplayGain(int index)
		{
			if (!FmodChannel.hasHandle()) return;

			var entry = PlayList[index];

			// モードに応じてゲイン値を選択
			float? gainDb = ReplayGainMode == 1
				? (entry.ReplayGainAlbum ?? entry.ReplayGainTrack)  // アルバム優先
				: (entry.ReplayGainTrack ?? entry.ReplayGainAlbum); // トラック優先

			if (gainDb == null) return;  // タグなし → 適用しない

			// dB → 線形変換（プリアンプ込み）
			// volume = 10 ^ ((gainDb + preamp) / 20)
			float totalDb = gainDb.Value + ReplayGainPreamp;
			float linearGain = (float)Math.Pow(10.0, totalDb / 20.0);

			// マスター音量と合算（クリッピング防止で上限1.0）
			float finalVolume = Math.Min(_masterVolume * linearGain, 1.0f);

			FmodChannel.setVolume(finalVolume);
		}
		public FMOD.RESULT PlaySoundPaused(int index, uint position = 0)
		{
			if (index >= PlayList.Count) return FMOD.RESULT.OK;

			if (FmodChannel.hasHandle())
				FmodChannel.stop();

			var loadResult = LoadSound(index);
			if (loadResult != FMOD.RESULT.OK) return loadResult;

			PlayingIndex = index;

			// ★paused=true で再生開始（音が出ない）
			var result = FmodCallFunction(FmodSystem.playSound(
				PlayList[index].Sound, FmodChannelGroup, true, out FmodChannel));

			if (result == FMOD.RESULT.OK && position > 0)
				FmodChannel.setPosition(position, FMOD.TIMEUNIT.MS);

			_nowPlaying = true;
			return result;
		}
		public uint GetLength(int index)
        {
			uint length = 0;
            if (index >= PlayList.Count || index < 0)
                return 0;

            FmodCallFunction(PlayList[index].Sound.getLength(out length, TIMEUNIT.MS));

			// ★FMODで取得できない場合はATLの値を使用
			if (length == 0 || length == 0xFFFFFFFF)
				length = PlayList[index].LengthMs;

			return length;
        }

		/// <summary>
		/// Create Sound.
		/// Removed CDDA support.
		/// </summary>
		/// <param name="filename"></param>
		public RESULT CreateSound(string filename, out int index)
        {
			index = 0;

			// ★URLの場合はバックグラウンドタグ取得をスキップ
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
		private async Task LoadTagsOnlyAsync(int index)
		{
			await _tagLoadSemaphore.WaitAsync();
			try
			{
				if (index < 0 || index >= PlayList.Count) return;

				string filename = PlayList[index].FileName;

				await Task.Run(() =>
				{
					try
					{
						var track = new ATL.Track(filename);
						PlayList[index].Title = track.Title;
						PlayList[index].Artist = track.Artist;
						PlayList[index].Album = track.Album;
						PlayList[index].SetLength((uint)track.DurationMs);

						// ★ ReplayGain タグを取得
						// ATL では AdditionalFields に "REPLAYGAIN_TRACK_GAIN" などが入っている
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

		// ── ParseReplayGainDb() ヘルパー追加 ─────────────────────────────
		// "-6.54 dB" → -6.54f を返す。解析失敗時は null。
		private static float? ParseReplayGainDb(string value)
		{
			if (string.IsNullOrEmpty(value)) return null;
			// "dB" や空白を除去して数値部分を取得
			string num = value.Replace("dB", "").Replace("dB", "").Trim();
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
				PlayList.Add(plist);
				index = PlayList.Count - 1;
			}
			return result;
		}
		private bool _fluidSynthAvailable = false;
		public bool FluidSynthAvailable => _fluidSynthAvailable;

        private static readonly HashSet<string> _trackerExtensions = new HashSet<string>
			{
				".mod", ".xm", ".it", ".s3m"
			};
        private FMOD.RESULT LoadSound(int index)
		{
			if (index < 0 || index >= PlayList.Count)
				return FMOD.RESULT.ERR_INVALID_PARAM;

			// 既にロード済みの場合はスキップ
			if (PlayList[index].IsLoaded)
				return FMOD.RESULT.OK;

			string filename = PlayList[index].FileName;
			FMOD.Sound sound;
			FMOD.RESULT result;
			FMOD.CREATESOUNDEXINFO info = new FMOD.CREATESOUNDEXINFO();
			info.cbsize = Marshal.SizeOf(info);
            string ext = Path.GetExtension(filename).ToLower();
            if (ext == ".mid")
			{
				if (_fluidSynthAvailable && !string.IsNullOrEmpty(_soundFontPath)
					&& File.Exists(_soundFontPath))
				{
					// ★FluidSynthでPCMにレンダリング
					try
					{
						using (var renderer = new FluidSynthMidiRenderer())
						  {
							var pcm = renderer.Render(filename, _soundFontPath);
							System.Diagnostics.Debug.WriteLine($"PCM size: {pcm.Length}");

							if (pcm != null && pcm.Length > 0)
							{
								// ★PlayListを経由せず直接FMOD Soundを生成
								FMOD.CREATESOUNDEXINFO pcmInfo = new FMOD.CREATESOUNDEXINFO();
								pcmInfo.cbsize = Marshal.SizeOf(pcmInfo);
								pcmInfo.length = (uint)pcm.Length;
								pcmInfo.numchannels = 2;
								pcmInfo.defaultfrequency = 44100;
								pcmInfo.format = FMOD.SOUND_FORMAT.PCM16;

								result = FmodCallFunction(FmodSystem.createSound(
									pcm,
									FMOD.MODE.OPENMEMORY | FMOD.MODE.OPENRAW |
									FMOD.MODE._2D | FMOD.MODE.CREATESAMPLE,
									ref pcmInfo,
									out sound));

								if (result == FMOD.RESULT.OK)
								{
									PlayList[index].Sound = sound;
									return result;
								}
							}
						}
					}
					catch (Exception ex)
					{
                        ErrorOccurred?.Invoke(this, new PlayerErrorEventArgs(
							nameof(LoadSound),
							$"FluidSynth error: {ex.Message}",
					    -1));
                        // フォールバック：FMODのDLSで再生
                    }
				}

				info.suggestedsoundtype = FMOD.SOUND_TYPE.MIDI;
				// ★SF2ファイルが設定されている場合は適用
				IntPtr dlsPtr = IntPtr.Zero;
				if (!string.IsNullOrEmpty(_soundFontPath) &&
					File.Exists(_soundFontPath) &&
					!_fluidSynthAvailable) // FluidSynth未導入時のみDLSを使用
				{
					dlsPtr = Marshal.StringToHGlobalAnsi(_soundFontPath);
					info.dlsname = dlsPtr;
				}
				try
				{
					result = FmodCallFunction(FmodSystem.createSound(
						filename, FMOD.MODE.DEFAULT, ref info, out sound));
				} finally
				{
					if (dlsPtr != IntPtr.Zero)
						Marshal.FreeHGlobal(dlsPtr);
				}
			}
            else if (_trackerExtensions.Contains(ext))
            {
                // トラッカー形式はcreateSound
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
				_ = StartWaveformAnalysisAsync(filename, index);
			}
			return result;
		}

		private async System.Threading.Tasks.Task StartWaveformAnalysisAsync(
			string filename, int index)
		{
			// 前回の解析をキャンセル
			_waveformCts?.Cancel();
			_waveformCts?.Dispose();
			_waveformCts = new System.Threading.CancellationTokenSource();
			var ct = _waveformCts.Token;

			await _waveformAnalyzer.AnalyzeAsync(
				filename,
				PlayList[index],
				entry =>
				{
					// 解析完了 → インデックスを特定してイベント発火
					int idx = PlayList.IndexOf(entry);
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
				// ★ロード済みのものだけ解放
				if (PlayList[i].IsLoaded)
					PlayList[i].Sound.release();
			}
			PlayList.Clear();
		}

		public void CreateSoundForMidi(string filename)
        {
		}
		public void PlayNext()
		{
            if (PlayList.Count == 0) return;

            if ((loop & LOOP_MODE.LOOP_RANDOM) != 0)
            {
                // キューが終わったら再シャッフル
                if (_shuffleQueueIndex >= _shuffleQueue.Count)
                    BuildShuffleQueue();
                PlaySound(_shuffleQueue[_shuffleQueueIndex++]);
                return;
            }

            int next;
            switch (loop)
            {
                case LOOP_MODE.LOOP_ONE_REPEAT:
                    next = PlayingIndex;
                    break;
                case LOOP_MODE.LOOP_ALL:
                    next = (PlayingIndex < PlayList.Count - 1) ? PlayingIndex + 1 : 0;
                    break;
                default: // LOOP_NONE
                    if (PlayingIndex >= PlayList.Count - 1) { _nowPlaying = false; return; }
                    next = PlayingIndex + 1;
                    break;
            }
			PlaySound(next);
		}
        public void Sort<T>(Func<Engine.Player.PlayList, T> keySelector)
        {
            var playingItem = (PlayingIndex >= 0 && PlayingIndex < PlayList.Count)
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

        public void PlayPrevious()
        {
            if (PlayList.Count == 0) return;

            if ((loop & LOOP_MODE.LOOP_RANDOM) != 0)
            {
                // キューを1つ戻る（最低0）
                _shuffleQueueIndex = Math.Max(0, _shuffleQueueIndex - 2);
                if (_shuffleQueueIndex < _shuffleQueue.Count)
                    PlaySound(_shuffleQueue[_shuffleQueueIndex++]);
                return;
            }

            int prev;
            switch (loop)
            {
                case LOOP_MODE.LOOP_ALL:
                    prev = (PlayingIndex > 0) ? PlayingIndex - 1 : PlayList.Count - 1;
                    break;
                default:
                    prev = Math.Max(0, PlayingIndex - 1);
                    break;
            }
            PlaySound(prev);
        }
        /// <summary>
        /// Stop Player
        /// </summary>
        /// <param name="channel"></param>
        public void Stop()
        {
			_nowPlaying = false;
			PlayingIndex = -1;
			if (FmodChannel.hasHandle())
				FmodChannel.stop();
			if (FmodChannelFading.hasHandle())
				FmodChannelFading.stop();
			FmodChannelFading = default;
			_isCrossfading = false;
			_crossfadeElapsedMs = 0;
		}

		/// <summary>
		/// Set Volume.
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="vol"></param>
		public void SetVolume(float vol)
        {
			_masterVolume = vol;
			FmodChannel.setVolume(vol);
			// フェードアウト中のチャンネルには触らない
        }

		public int GetVolume()
        {
			float volume;
			FmodChannel.getVolume(out volume);
			return (int) (volume * 100);
        }
		/// <summary>
		/// Set Pan
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="pan"></param>
		public void SetPan(float pan)
        {
			FmodChannel.setPan(pan);
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
				// 出力タイプをinit前に設定
				if (tempSystem.setOutput(outputType) != FMOD.RESULT.OK)
					return list;

				// 最小構成でinit（1ch、サウンド再生なし）
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
					}
				}
			}
			finally
			{
				// 必ずクリーンアップ
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

            // Fisher-Yatesシャッフル
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

            // プレイリストに追加せず直接再生
            FmodCallFunction(FmodSystem.playSound(sound, FmodChannelGroup, false, out FmodChannel));

            _nowPlaying = true;
            return FMOD.RESULT.OK;
        }

		public Bitmap GetCoverArt(int index)
		{
			if (index < 0 || index >= PlayList.Count) return null;
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
            // NonStopMix：フェードなし即切り替え
            if (NonStopMixEnabled)
            {
                if (FmodChannel.hasHandle())
                    FmodChannel.stop();
                _fadingPlayListIndex = -1;
                return;
            }
            if (!CrossfadeEnabled || !FmodChannel.hasHandle() || !IsPlaying())
			{
				// クロスフェード無効または再生中でなければ即停止
				if (FmodChannel.hasHandle())
					FmodChannel.stop();
				_fadingPlayListIndex = -1;
				return;
			}

			// 現在のチャンネルをフェードアウト用に退避
			if (FmodChannelFading.hasHandle())
			{
				// 前回のフェードが残っていたら先に止めてサウンドを解放
				FmodChannelFading.stop();
				if (_fadingPlayListIndex >= 0 && _fadingPlayListIndex < PlayList.Count
					&& PlayList[_fadingPlayListIndex].IsLoaded)
				{
					PlayList[_fadingPlayListIndex].Sound.release();
					PlayList[_fadingPlayListIndex].Sound = default;
				}
			}
			_fadingPlayListIndex = PlayingIndex;  // ← 旧曲のインデックスを記録
			FmodChannelFading = FmodChannel;
			FmodChannel = default;
			_crossfadeElapsedMs = 0;
			_isCrossfading = true;
		}
		// ── UpdateCrossfade() 追加 ────────────────────────────────────────
		// MainForm の PlayerTimer_Tick から毎フレーム呼ぶ。
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

			// フェードアウト（旧チャンネル）
			if (FmodChannelFading.hasHandle())
				FmodChannelFading.setVolume(_masterVolume * (1.0f - t));

			// フェードイン（新チャンネル）
			if (FmodChannel.hasHandle())
				FmodChannel.setVolume(_masterVolume * t);

			// フェード完了
			if (t >= 1.0f)
			{
				if (FmodChannelFading.hasHandle())
					FmodChannelFading.stop();

				// フェード完了後に旧曲のサウンドを解放する
				if (_fadingPlayListIndex >= 0 && _fadingPlayListIndex < PlayList.Count
					&& PlayList[_fadingPlayListIndex].IsLoaded)
				{
					PlayList[_fadingPlayListIndex].Sound.release();
					PlayList[_fadingPlayListIndex].Sound = default;
				}

				FmodChannelFading = default;
				_fadingPlayListIndex = -1; 
				_isCrossfading = false;
				_crossfadeElapsedMs = 0;
			}
		}
	}
}
