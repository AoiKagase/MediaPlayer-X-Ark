using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector
{
	public class Effectors
	{
		public Frequency Frequency { get; private set; }
		public PitchShift PitchShift { get; private set; }
		public Chorus Chorus { get; private set; }
		public Compressor Compressor { get; private set; }
		public Distortion Distortion { get; private set; }
		public Echo Echo { get; private set; }
		public Flanger Flanger { get; private set; }
		public Highpass Highpass { get; private set; }
		public Lowpass Lowpass { get; private set; }
		public SFXReverb SFXReverb { get; private set; }

		public GEqualizer GEqualizer { get; private set; }

		private int _speed;
		public int Speed
		{
			get
			{
				return _speed;
			}
			set
			{
				if (_speed != value)
				{
					_speed = value;

					float pitch = PitchShiftPercentage(_speed);
					if (pitch != 0.0)
						Frequency.Hz = (44100f / pitch);
					PitchShift.Pitch = pitch;
				}
			}
		}
		private bool _speedEnabled;
		public bool SpeedEnabled
		{
			get
			{
				return _speedEnabled;				
			}
			set
			{
				if (_speedEnabled != value)
                {
					_speedEnabled = value;
					if (_speedEnabled)
                    {
						Frequency.Switch(true);
						PitchShift.Switch(true);
                    }
					else
                    {
						Frequency.Switch(false);
						PitchShift.Switch(false);
					}
				}
			}
		}

		private float PitchShiftPercentage(float shift)
		{
			if (shift >= 0)
			{
				return 1 - 0.5f * (shift / 100);
			}
			return 1 - 1 * (shift / 100);
		}
		public Effectors(FMOD.System system)
		{
			Frequency = new Frequency(system);
			PitchShift = new PitchShift(system);
			Chorus = new Chorus(system);
			Compressor = new Compressor(system);
			Distortion = new Distortion(system);
			Echo = new Echo(system);
			Flanger = new Flanger(system);
			Highpass = new Highpass(system);
			Lowpass = new Lowpass(system);
			SFXReverb = new SFXReverb(system);
			GEqualizer = new GEqualizer(system);
			Initialize();
		}

		public void Initialize()
		{
			Frequency.Switch(false);
			PitchShift.Switch(false);
			Chorus.Switch(false);
			Compressor.Switch(false);
			Distortion.Switch(false);
			Echo.Switch(false);
			Flanger.Switch(false);
			Highpass.Switch(false);
			Lowpass.Switch(false);
			SFXReverb.Switch(false);
			GEqualizer.Switch(false);
		}
	}
}
