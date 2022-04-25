using FMOD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace MediaPlayer_X_Ark
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

	public enum SOFTWARE_SAMPLE_RATE
	{
		SAMPLE_5500HZ	= 5500,
		SAMPLE_6000HZ	= 6000,
		SAMPLE_7333HZ	= 7333,
		SAMPLE_8000HZ	= 8000,
		SAMPLE_11025HZ	= 11025,
		SAMPLE_16000HZ	= 16000,
		SAMPLE_22050HZ	= 22050,
		SAMPLE_32000HZ	= 32000,
		SAMPLE_44100HZ	= 44100,
		SAMPLE_48000HZ	= 48000,
		SAMPLE_88200HZ	= 88200,
		SAMPLE_96000HZ	= 96000,
		SAMPLE_192000HZ	= 192000,
	}

	public class PlayerEngine
	{
		[DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string dllToLoad);

		protected bool initialized = false;
		public string lastError = "";
		public FMOD.RESULT lastErrCode;

		// FMOD SYSTEM.
		public BindingList<Engine.PlayList> PlayList = new BindingList<Engine.PlayList>();
		protected FMOD.System FmodSystem;
		protected FMOD.ChannelGroup FmodChannelGroup;
		protected FMOD.Channel FmodChannel;

		// SOUND DEVICES.
		protected FMOD.OUTPUTTYPE FmodOutputType;


		protected uint FmodVersion;

		protected List<DEVICE_INFO> FmodDeviceList = new List<DEVICE_INFO>();

		private const int channelCount = 1;

		public FmodSpectrum spectrum;

		public Engine.Effector.Effectors effector;

		public List<DEVICE_INFO> DeviceList
        {
            get { return FmodDeviceList; }
        }

		protected FMOD.RESULT FmodCallFunction(FMOD.RESULT result)
        {
			lastError = FMOD.Error.String(result);
			lastErrCode = result;
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
		~PlayerEngine()
		{ 
			if (initialized)
            {
				// Relase FMOD handles for Channel.
				if (FmodChannel.hasHandle())
					FmodChannel.stop();

				// Relase FMOD handles for ChannelGroup.
				if (FmodChannelGroup.hasHandle())
					FmodChannelGroup.release();

				// Relase FMOD handles for Sound.
				for (int i = 0; i < PlayList.Count; i++)
                {
					if (PlayList[i].Sound.hasHandle())
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
		}

		protected FMOD.RESULT CreateSystem()
        {
			return FmodCallFunction(FMOD.Factory.System_Create(out FmodSystem));
		}
		/// <summary>
		/// System Initialize.
		/// </summary>
		public void Initialize()
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

				// System Init.
				if (FmodCallFunction(FmodSystem.init(channelCount, FMOD.INITFLAGS.NORMAL, IntPtr.Zero)) == RESULT.OK)
				{
					// Create Channel Group.
					if (FmodCallFunction(FmodSystem.createChannelGroup("Channel 01", out FmodChannelGroup)) == RESULT.OK)
                    {
						spectrum = new FmodSpectrum(ref FmodSystem, 1024, ref this.FmodChannelGroup);
						LoadPlugins();

//						FmodCallFunction(FmodSystem.getChannel(0, out FmodChannel));

						PlayList = new BindingList<Engine.PlayList>();

						effector = new Engine.Effector.Effectors(FmodSystem);

						GetDeviceList();

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
//			FmodCallFunction(FmodSystem.loadPlugin("codec_aac.dll", out handle));
//			FmodSystem.getPluginInfo(handle, out plugintype, out version);
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
		/// </param>
		public void SetOutputType(FMOD.OUTPUTTYPE outputtype)
		{
			GetOutputType();

			FmodOutputType = outputtype;
			FmodCallFunction(FmodSystem.setOutput(outputtype));
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
			RESULT fResult = FMOD.RESULT.OK;
			bool result = false;
			if (FmodChannel.hasHandle())
            {
				if ((fResult = FmodCallFunction(FmodChannel.isPlaying(out result))) == RESULT.OK)
				{
					return result;
				}
			}
			return (fResult == RESULT.OK);
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

		/// <summary>
		/// Set Software format. (Sample rate)
		/// </summary>
		/// <param name="samplerate"></param>
		/// <param name="speakermode"></param>
		public FMOD.RESULT SetSoftwareFormat(SOFTWARE_SAMPLE_RATE samplerate, FMOD.SPEAKERMODE speakermode)
        {
			return FmodCallFunction(FmodSystem.setSoftwareFormat((int) samplerate, speakermode, GetNumberRawSpeakers(speakermode)));
        }
		public FMOD.RESULT GetSoftwareFormat(out int samplerate, out FMOD.SPEAKERMODE speakermode, out int speakernum)
		{
			return FmodCallFunction(FmodSystem.getSoftwareFormat(out samplerate, out speakermode, out speakernum));
		}
		/// <summary>
		/// Raw Speaker Count
		/// </summary>
		/// <param name="speakermode"></param>
		/// <returns></returns>
		private int GetNumberRawSpeakers(FMOD.SPEAKERMODE speakermode)
        {
			int numRawSpeakers = 2;
			switch (speakermode)
			{
				case FMOD.SPEAKERMODE.DEFAULT:
					numRawSpeakers = 2;
					break;
				case FMOD.SPEAKERMODE.MONO:
					numRawSpeakers = 2;
					break;
				case FMOD.SPEAKERMODE.STEREO:
					numRawSpeakers = 2;
					break;
				case FMOD.SPEAKERMODE.QUAD:
					numRawSpeakers = 4;
					break;
				case FMOD.SPEAKERMODE.SURROUND:
					numRawSpeakers = 5;
					break;
				case FMOD.SPEAKERMODE._5POINT1:
					numRawSpeakers = 6;
					break;
				case FMOD.SPEAKERMODE._7POINT1:
					numRawSpeakers = 8;
					break;
				case FMOD.SPEAKERMODE._7POINT1POINT4:
					numRawSpeakers = 12;
					break;
			}
			return numRawSpeakers;
		}

		public uint GetPosition()
        {
			uint position = 0;
			if (FmodChannel.hasHandle())
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
			FMOD.RESULT result = RESULT.OK;
			if (PlayList[index].Sound.hasHandle())
            {
				result = FmodCallFunction(FmodSystem.playSound(PlayList[index].Sound, FmodChannelGroup, false, out FmodChannel));
				GetTags(index);
			}
			return result;
        }

		public uint GetLength(int index)
        {
			uint length = 0;
			FmodCallFunction(PlayList[index].Sound.getLength(out length, TIMEUNIT.MS));
			return length;
        }
		/// <summary>
		/// Get Sound file Tags.
		/// </summary>
        public void GetTags(int index)
        {
			FMOD.TAG tags;

			int numtags;
			int updated;

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
							PlayList[index].Artist = Marshal.PtrToStringUTF8(tags.data);

						if ((tagname).Equals("TITLE") || tagname.Equals("TRACK TITLE"))
							PlayList[index].Title = Marshal.PtrToStringUTF8(tags.data);

						if ((tagname).Equals("AUTHOR"))
							PlayList[index].Artist = Marshal.PtrToStringUTF8(tags.data);

						if ((tagname).Equals("ALBUM") || tagname.Equals("ALBUM TITLE"))
							PlayList[index].Album = Marshal.PtrToStringUTF8(tags.data);

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
		public RESULT CreateSound(string filename, SOUND_FORMAT format, out int index)
        {
			// CD Player
			//if (filename.Substring(0, 3).Equals("cd:"))
			//{
			//	FmodSystem.createStream(filename.Substring(4, 2), (FMOD.MODE._2D | FMOD.MODE.CREATESTREAM | FMOD.MODE.OPENONLY | FMOD.MODE.OPENMEMORY), out FmodSound);
			//}
			FMOD.Sound sound;
			FMOD.RESULT result;
			FMOD.CREATESOUNDEXINFO info = new FMOD.CREATESOUNDEXINFO();
			info.cbsize = Marshal.SizeOf(info);
			index = 0;
			if (Path.GetExtension(filename).Equals("mid"))
			{
				info.suggestedsoundtype = FMOD.SOUND_TYPE.MIDI;
				if ((result = FmodCallFunction(FmodSystem.createSound(filename, FMOD.MODE.DEFAULT, ref info, out sound))) == RESULT.OK)
				{
					Engine.PlayList plist = new Engine.PlayList(filename, sound);
					PlayList.Add(plist);
					index = PlayList.Count - 1;
				}
			}
			else
			{
				info.format = format;
				if ((result = FmodCallFunction(FmodSystem.createStream(filename, FMOD.MODE.DEFAULT, ref info, out sound))) == RESULT.OK)
				{
					Engine.PlayList plist = new Engine.PlayList(filename, sound);
					PlayList.Add(plist);
					index = PlayList.Count - 1;
				}
			}
			return result;
        }

		public void CreateSoundForMidi(string filename)
        {
		}

		/// <summary>
		/// Stop Player
		/// </summary>
		/// <param name="channel"></param>
		public void Stop()
        {
			FmodChannel.stop();
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

		public void GetWaveData()
        {
        }
	}
}
