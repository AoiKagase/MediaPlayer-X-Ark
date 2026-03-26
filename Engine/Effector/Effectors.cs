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

        private IEffector[] AllEffectors() => new IEffector[]
        {
            GEqualizer, Frequency, PitchShift, Chorus, Compressor,
            Distortion, Echo, Flanger, Highpass, Lowpass, SFXReverb, Normalize
        };
        public void Initialize()
		{
            foreach (var e in AllEffectors())
                e.Switch(false);
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
			Distortion.Level = s.Distortion.Level;
            if (s.Distortion.Enable) Distortion.Switch(true);

            // Chorus
            Chorus.Mix = s.Chorus.Mix;
            Chorus.Rate = s.Chorus.Rate;
            Chorus.Depth = s.Chorus.Depth;
            if (s.Chorus.Enable) Chorus.Switch(true);

            // Echo
            Echo.Delay = s.Echo.Delay;
            Echo.Feedback = s.Echo.Feedback;
            Echo.DryLevel = s.Echo.Dry;
            Echo.WetLevel = s.Echo.Wet;
			if (s.Echo.Enable) Echo.Switch(true);

            // Flanger
            Flanger.Mix = s.Flanger.Mix;
            Flanger.Rate = s.Flanger.Rate;
            Flanger.Depth = s.Flanger.Depth;
            if (s.Flanger.Enable) Flanger.Switch(true);

            // Highpass
            Highpass.CutOff = s.Highpass.Cutoff;
            Highpass.Resonance = s.Highpass.Resonance;
            if (s.Highpass.Enable) Highpass.Switch(true);

            // Lowpass
            Lowpass.CutOff = s.Lowpass.Cutoff;
            Lowpass.Resonance = s.Lowpass.Resonance;
            if (s.Lowpass.Enable) Lowpass.Switch(true);

            // Compressor
            Compressor.Threshold = s.Compressor.Threshold;
            Compressor.Ratio = s.Compressor.Ratio;
            Compressor.Attack = s.Compressor.Attack;
            Compressor.Release = s.Compressor.Release;
            Compressor.Gain = s.Compressor.Gain;
            if (s.Compressor.Enable) Compressor.Switch(true);

            // Reverb
            SFXReverb.DecayTime = s.Reverb.DecayTime;
            SFXReverb.EarlyDelay = s.Reverb.EarlyDelay;
            SFXReverb.LateDelay = s.Reverb.LateDelay;
            SFXReverb.HFReference = s.Reverb.HFRef;
            SFXReverb.HFDecayRatio = s.Reverb.HFDecayRatio;
            SFXReverb.Diffusion = s.Reverb.Diffusion;
            SFXReverb.Density = s.Reverb.Density;
            SFXReverb.LowShelfFrequency = s.Reverb.LowShelfFrequency;
            SFXReverb.LowShelfGain = s.Reverb.LowShelfGain;
            SFXReverb.HighCut = s.Reverb.HighCut;
            SFXReverb.EarlyLateMix = s.Reverb.EarlyLate;
            SFXReverb.WetLevel = s.Reverb.WetLevel;
            SFXReverb.DryLevel = s.Reverb.DryLevel;
            if (s.Reverb.Enable) SFXReverb.Switch(true);

            // Normalize
			Normalize.MaxAmp = s.Normalize.MaxAmp;
			Normalize.FadeTime = s.Normalize.FadeTime;
			Normalize.Threshold = s.Normalize.Threshold;
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
