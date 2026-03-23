using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using System;

namespace MediaPlayer_X_Ark.Engine.Effector
{
	public class Effectors
	{
		public GEqualizer GEqualizer { get; private set; }
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
		public Normalize Normalize { get; private set; }

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
		public Effectors(FMOD.System system, IPlayerEngine _engine)
		{
			Frequency = new Frequency(system, _engine);
			PitchShift = new PitchShift(system);
			Chorus = new Chorus(system);
			Compressor = new Compressor(system);
			Distortion = new Distortion(system);
			Echo = new Echo(system);
			Flanger = new Flanger(system);
			Highpass = new Highpass(system);
			Lowpass = new Lowpass(system);
			SFXReverb = new SFXReverb(system);
			Normalize = new Normalize(system);
			GEqualizer = new GEqualizer(system);
			Initialize();
		}

		public void Initialize()
		{
			GEqualizer.Switch(false);
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
			Normalize.Switch(false);
		}

        public void ApplySettings(CfgEffectors s)
        {
            // GEQ
            for (int i = 0; i < 12; i++)
                GEqualizer.SetGain((GEqualizer.EQ_HZ)i, (float)GetGEQByIndex(s.GEqualizer, i) / 10f);
            if (s.GEqualizer.Enable) GEqualizer.Switch(true);

            // PitchShift
            PitchShift.Pitch = s.PitchShift.Pitch / 100f;
            float[] fftsizes = { 256f, 512f, 1024f, 2048f, 4096f };
            PitchShift.FFTSize = fftsizes[Math.Max(0, Math.Min(4, s.PitchShift.FFT))];
            if (s.PitchShift.Enable) PitchShift.Switch(true);

            // Frequency
            Frequency.SetFrequency(s.Frequency.Frequency);
            if (s.Frequency.Enable) Frequency.Switch(true);

            // Speed（PitchShift + Frequency を連動させるため SpeedEnabled 経由で設定）
            if (s.Speed.Enable)
            {
                Speed = s.Speed.Speed;
                SpeedEnabled = true;
            }

            // Distortion
            if (s.Distortion.Enable) Distortion.Switch(true);

            // Chorus
            if (s.Chorus.Enable) Chorus.Switch(true);

            // Echo
            if (s.Echo.Enable) Echo.Switch(true);

            // Flanger
            if (s.Flanger.Enable) Flanger.Switch(true);

            // Highpass
            if (s.Highpass.Enable) Highpass.Switch(true);

            // Lowpass
            if (s.Lowpass.Enable) Lowpass.Switch(true);

            // Compressor
            if (s.Compressor.Enable) Compressor.Switch(true);

            // Reverb
            if (s.Reverb.Enable) SFXReverb.Switch(true);

            // Normalize
            if (s.Normalize.Enable) Normalize.Switch(true);
        }
        private static decimal GetGEQByIndex(CfgGEqualizer geq, int index) => index switch
        {
            0 => geq.GEQ_32,
            1 => geq.GEQ_60,
            2 => geq.GEQ_125,
            3 => geq.GEQ_250,
            4 => geq.GEQ_500,
            5 => geq.GEQ_1K,
            6 => geq.GEQ_2K,
            7 => geq.GEQ_4K,
            8 => geq.GEQ_8K,
            9 => geq.GEQ_16K,
            10 => geq.GEQ_20K,
            11 => geq.GEQ_22K,
            _ => 0
        };
    }
}
