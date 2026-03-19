using FMOD;
using MediaPlayer_X_Ark.Engine;
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
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace MediaPlayer_X_Ark.Engine
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
        // フィールド追加（GC対策）
        private FMOD.CHANNELCONTROL_CALLBACK _channelEndCallback;

        // イベント
        public event EventHandler TrackEnded;

        [DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string dllToLoad);

        public FmodSpectrum spectrum { get; private set; }
        public FmodWave wave { get; private set; }
        public Effector.Effectors effector { get; private set; }
        public LOOP_MODE loop { get; set; }
        public string lastError { get; protected set; } = "";
        public string lastErrFunction { get; protected set; } = "";
        public FMOD.RESULT lastErrCode { get; protected set; }
        public int PlayingIndex { get; private set; } = -1;
        protected bool initialized = false;
        private bool _nowPlaying = false;
        public bool NowPlaying => _nowPlaying;
        // FMOD SYSTEM.
        public BindingList<Engine.PlayList> PlayList { get; set; } = new BindingList<Engine.PlayList>();
        protected FMOD.System FmodSystem;
		protected FMOD.ChannelGroup FmodChannelGroup;
		protected FMOD.Channel FmodChannel;

		// SOUND DEVICES.
		protected FMOD.OUTPUTTYPE FmodOutputType;


		protected uint FmodVersion;

		protected List<DEVICE_INFO> FmodDeviceList = new List<DEVICE_INFO>();

        private const int channelCount = 1;

        private List<int> _shuffleQueue = new List<int>();
        private int _shuffleQueueIndex = 0;
        private readonly Random _rng = new Random();

        public List<DEVICE_INFO> DeviceList
        {
            get { return FmodDeviceList; }
        }

		protected FMOD.RESULT FmodCallFunction(FMOD.RESULT result, [CallerMemberName] string callerMethodName = "")
        {
			lastError = FMOD.Error.String(result);
			lastErrCode = result;
			lastErrFunction = callerMethodName;
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
			// System Create.
			{
				// Get Version.
				if (FmodCallFunction(FmodSystem.getVersion(out FmodVersion)) == FMOD.RESULT.OK)
				{
					// Version Check.
					if (FmodVersion != FMOD.VERSION.number)
					{
						lastError = "Error!  You are using an old version of FMOD "
								+ FmodVersion.ToString("X")
								+ ". This program requires "
								+ FMOD.VERSION.number.ToString("X");
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

						PlayList = new BindingList<Engine.PlayList>();

						effector = new Engine.Effector.Effectors(FmodSystem);

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


		/// <summary>
		/// Play Sound for Loaded Channels.
		/// </summary>
		/// <param name="channel"></param>
		public RESULT PlaySound(int index)
        {
            if (index >= PlayList.Count)
				return FMOD.RESULT.OK;

            // ★再生前にロード
            var loadResult = LoadSound(index);
			if (loadResult != FMOD.RESULT.OK)
				return loadResult;

            PlayingIndex = index;

            // 再生
            var result = FmodCallFunction(FmodSystem.playSound(
				PlayList[index].Sound, FmodChannelGroup, false, out FmodChannel));

			// ★再生中以外の不要なSoundを解放
			for (int i = 0; i < PlayList.Count; i++)
			{
				if (i == index) continue;
				// 次曲はプリロードのため保持（ギャップレス再生への布石）
				if (i == index + 1) continue;
				if (PlayList[i].IsLoaded && i != index)
				{
					PlayList[i].Sound.release();
					PlayList[i].Sound = default;
				}
			}

            // ★コールバック設定（再生開始ごとに登録）
            _channelEndCallback = (channelraw, controltype, callbacktype, cd1, cd2) =>
            {
                if (callbacktype == FMOD.CHANNELCONTROL_CALLBACK_TYPE.END)
                    TrackEnded?.Invoke(this, EventArgs.Empty);
                return FMOD.RESULT.OK;
            };
            FmodChannel.setCallback(_channelEndCallback);

            PlayingIndex = index;
            _nowPlaying = true;

            return result;
        }

		public uint GetLength(int index)
        {
			uint length = 0;
            if (index >= PlayList.Count)
                return 0;

            FmodCallFunction(PlayList[index].Sound.getLength(out length, TIMEUNIT.MS));
			return length;
        }
		/// <summary>
		/// Get Sound file Tags.
		/// </summary>
        public void GetTags(int index)
        {
			FMOD.TAG tags;

			int numtags = 0;
			int updated = 0;

			if (index >= PlayList.Count)
				return;

            PlayList[index].Sound.getNumTags(out numtags, out updated);

			if (updated > 0)
            {
				for (int i = 0; i < numtags; i++)
//				while(FmodSound.getTag(null, -1, out tags) == FMOD.RESULT.OK)
                {
					PlayList[index].Sound.getTag(null, i, out tags);
					if (tags.type == TAGTYPE.ID3V1 || tags.type == TAGTYPE.ID3V2 || tags.type == TAGTYPE.VORBISCOMMENT)
                    {
						string tagname = ((string)tags.name).ToUpper();
						if (tagname.Equals("ARTIST") || tagname.Equals("ARTIST NAME"))
							PlayList[index].Artist = Marshal.PtrToStringUTF8(tags.data, (int)tags.datalen);

						if ((tagname).Equals("TITLE") || tagname.Equals("TRACK TITLE"))
							PlayList[index].Title = Marshal.PtrToStringUTF8(tags.data, (int)tags.datalen);

						if ((tagname).Equals("AUTHOR"))
							PlayList[index].Artist = Marshal.PtrToStringUTF8(tags.data, (int)tags.datalen);

						if ((tagname).Equals("ALBUM") || tagname.Equals("ALBUM TITLE"))
							PlayList[index].Album = Marshal.PtrToStringUTF8(tags.data, (int)tags.datalen);

					}
				}
			}

			int channel;
			SOUND_TYPE soundtype;
			SOUND_FORMAT soundformat;
			int bit;
			uint length;

			PlayList[index].Sound.getFormat(out soundtype, out soundformat, out channel, out bit);
			PlayList[index].Sound.getLength(out length, FMOD.TIMEUNIT.MS);

			PlayList[index].SoundType = soundtype;
			PlayList[index].Format = soundformat;
			PlayList[index].Bit = bit;
			PlayList[index].SetLength(length);
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

				var plist = new Engine.PlayList(filename);
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
					FMOD.Sound sound;
					FMOD.CREATESOUNDEXINFO info = new FMOD.CREATESOUNDEXINFO();
					info.cbsize = Marshal.SizeOf(info);

					FMOD.RESULT result;
					if (Path.GetExtension(filename).Equals(".mid"))
					{
						info.suggestedsoundtype = FMOD.SOUND_TYPE.MIDI;
						result = FmodSystem.createSound(
							filename, FMOD.MODE.DEFAULT, ref info, out sound);
					}
					else
					{
						result = FmodSystem.createStream(
							filename, FMOD.MODE.DEFAULT, ref info, out sound);
					}

					if (result != FMOD.RESULT.OK) return;

					try
					{
						// ★一時的にSoundを代入してGetTagsを呼ぶ
						PlayList[index].Sound = sound;

						uint length;
						sound.getLength(out length, FMOD.TIMEUNIT.MS);
						PlayList[index].SetLength(length);
						GetTags(index);
					}
					finally
					{
						sound.release();
						PlayList[index].Sound = default;
					}
				});
			}
			catch { }
			finally
			{
				_tagLoadSemaphore.Release();
			}

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
				var plist = new Engine.PlayList(title, sound);
				PlayList.Add(plist);
				index = PlayList.Count - 1;
			}
			return result;
		}

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
				info.suggestedsoundtype = FMOD.SOUND_TYPE.MIDI;
				result = FmodCallFunction(FmodSystem.createSound(
					filename, FMOD.MODE.DEFAULT, ref info, out sound));
			}
            else if (_trackerExtensions.Contains(ext))
            {
                // トラッカー形式はcreateSound
                result = FmodCallFunction(FmodSystem.createSound(
                    filename, FMOD.MODE.DEFAULT, ref info, out sound));
            }
            else
			{
				result = FmodCallFunction(FmodSystem.createStream(
					filename, FMOD.MODE.DEFAULT, ref info, out sound));
			}

			if (result == FMOD.RESULT.OK)
			{
				PlayList[index].Sound = sound;
				GetTags(index);
			}

			return result;
		}
		/// <summary>
		/// プレイリストを全消去する。
		/// </summary>
		public void ClearPlayList()
		{
            PlayingIndex = -1;
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
        public void Sort<T>(Func<Engine.PlayList, T> keySelector)
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
            if (FmodChannel.hasHandle() && IsPlaying())
				FmodCallFunction(FmodChannel.stop());
		}

		/// <summary>
		/// Set Volume.
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="vol"></param>
		public void SetVolume(float vol)
        {
			FmodChannel.setVolume(vol);
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

            // コールバック設定
            _channelEndCallback = (channelraw, controltype, callbacktype, cd1, cd2) =>
            {
                if (callbacktype == FMOD.CHANNELCONTROL_CALLBACK_TYPE.END)
                    TrackEnded?.Invoke(this, EventArgs.Empty);
                return FMOD.RESULT.OK;
            };
            FmodChannel.setCallback(_channelEndCallback);

            _nowPlaying = true;
            return FMOD.RESULT.OK;
        }

        public Bitmap GetCoverArt(int index)
        {
            if (index < 0 || index >= PlayList.Count) return null;
            if (!PlayList[index].IsLoaded) return null;

            int tagCount, updatedCount;
            PlayList[index].Sound.getNumTags(out tagCount, out updatedCount);
            System.Diagnostics.Debug.WriteLine($"tagCount={tagCount} updatedCount={updatedCount}");
            var coverTagNames = new HashSet<string> { "APIC", "PICTURE", "WM/Picture", "METADATA_BLOCK_PICTURE" };
            for (int i = 0; i < tagCount; i++)
            {
                FMOD.TAG tag;
                if (PlayList[index].Sound.getTag(null, i, out tag) != FMOD.RESULT.OK) continue;
                string tagName = (string)tag.name;
                // デバッグ用：全タグ名を出力
                System.Diagnostics.Debug.WriteLine($"Tag[{i}]: dataType={tag.datatype} name={tagName} type={tag.type} datalen={tag.datalen}");

                if (!coverTagNames.Contains(tagName)) continue;

                try
                {
                    byte[] data = new byte[tag.datalen];
                    Marshal.Copy(tag.data, data, 0, (int)tag.datalen);
                    using (var ms = new MemoryStream(data))
                        return new Bitmap(ms);
                }
                catch { }
            }
            return null;
        }
    }
}
