using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace MediaPlayer_X_Ark.Engine.Effector
{
	public class SFXReverb : AbstractEffectorBase
	{
		private float _DecayTime;
		private float _EarlyDelay;
		private float _LateDelay;
		private float _HFReference;
		private float _HFDecayRatio;
		private float _Diffusion;
		private float _Density;
		private float _LowShelfFrequency;
		private float _LowShelfGain;
		private float _HighCut;
		private float _EarlyLateMix;
		private float _WetLevel;
		private float _DryLevel;

		private FMOD.REVERB_PROPERTIES _preset;

		public FMOD.REVERB_PROPERTIES Preset
		{
			get { return _preset; }
			set
			{
				_preset = value;
				SetPreset(_preset);
			}
		}
		/// <summary>
		/// Reverberation decay time at low-frequencies.
		/// Type: float Units: Milliseconds Range: [100, 20000] Default: 1500
		/// </summary>
		public float DecayTime
		{
			get
			{
				return _DecayTime;
			}
			set
			{
				if (_DecayTime != value)
				{
					_DecayTime = Math.Clamp(value, 100.0F, 20000.0F);
					SetParameterFloat((int)FMOD.DSP_SFXREVERB.DECAYTIME, _DecayTime);
				}
			}
		}

		/// <summary>
		/// Delay time of first reflection.
		/// Type: float Units: Milliseconds Range: [0, 300] Default: 20
		/// </summary>
		public float EarlyDelay
        {
            get
            {
				return _EarlyDelay;
            }
            set
            {
				_EarlyDelay = Math.Clamp(value, 0.0F, 300.0F);
				SetParameterFloat((int)FMOD.DSP_SFXREVERB.EARLYLATEMIX, _EarlyDelay);
			}
		}

		/// <summary>
		/// Late reverberation delay time relative to first reflection in milliseconds.
		/// Type: float Units: Milliseconds Range: [0, 100] Default: 40
		/// </summary>
		public float LateDelay
        {
            get
            {
				return _LateDelay;
            }
			set
			{
				_LateDelay = Math.Clamp(value, 0.0F, 100.0F);
				SetParameterFloat((int)FMOD.DSP_SFXREVERB.LATEDELAY, _LateDelay);
			}
		}

		/// <summary>
		/// Reference frequency for high-frequency decay.
		/// Type: float Units: Hertz Range: [20, 20000] Default: 5000
		/// </summary>
		public float HFReference
        {
            get
            {
				return _HFReference;
            }
			set
			{
				_HFReference = Math.Clamp(value, 20.0F, 20000.0F);
				SetParameterFloat((int)FMOD.DSP_SFXREVERB.HFREFERENCE, _HFReference);
			}
		}

		/// <summary>
		/// High-frequency decay time relative to decay time.
		/// Type: float Units: Percent Range: [10, 100] Default: 50
		/// </summary>
		public float HFDecayRatio
        {
            get
            {
				return _HFDecayRatio;
            }
            set
            {
				_HFDecayRatio = Math.Clamp(value, 10.0F, 100.0F);
				SetParameterFloat((int)FMOD.DSP_SFXREVERB.HFDECAYRATIO, _HFDecayRatio);
			}
		}

		/// <summary>
		/// Reverberation diffusion (echo density).
		/// Type: float Units: Percent Range: [10, 100] Default: 50
		/// </summary>
		public float Diffusion
        {
            get
            {
				return _Diffusion;
            }
            set
            {
				_Diffusion = Math.Clamp(value, 10.0F, 100.0F);
				SetParameterFloat((int)FMOD.DSP_SFXREVERB.DIFFUSION, _Diffusion);
			}
		}
		/// <summary>
		/// Reverberation density (modal density).
		/// Type: float Units: Percent Range: [10, 100] Default: 50
		/// </summary>
		public float Density
        {
            get
            {
				return _Density;
            }
            set
            {
				_Density = Math.Clamp(value, 10.0F, 100.0F);
				SetParameterFloat((int)FMOD.DSP_SFXREVERB.DENSITY, _Density);
			}
		}

		/// <summary>
		/// Transition frequency of low-shelf filter.
		/// Type: float Units: Hertz Range: [20, 1000] Default: 250
		/// </summary>
		public float LowShelfFrequency
        {
            get
            {
				return _LowShelfFrequency;
            }
            set
            {
				_LowShelfFrequency = Math.Clamp(value, 20.0F, 1000.0F);
				SetParameterFloat((int)FMOD.DSP_SFXREVERB.LOWSHELFFREQUENCY, _LowShelfFrequency);
			}
		}
		/// <summary>
		/// Gain of low-shelf filter.
		/// Type: float Units: Decibels Range: [-36, 12] Default: 0
		/// </summary>
		public float LowShelfGain
		{
			get
			{
				return _LowShelfGain;
			}
			set
			{
				_LowShelfGain = Math.Clamp(value, -36.0F, 12.0F);
				SetParameterFloat((int)FMOD.DSP_SFXREVERB.LOWSHELFGAIN, _LowShelfGain);
			}
		}

		/// <summary>
		/// Cutoff frequency of low-pass filter.
		/// Type: float Units: Hertz Range: [20, 20000] Default: 20000
		/// </summary>
		public float HighCut
		{
			get
			{
				return _HighCut;
			}
			set
			{
				_HighCut = Math.Clamp(value, 20.0F, 20000.0F);
				SetParameterFloat((int)FMOD.DSP_SFXREVERB.HIGHCUT, _HighCut);
			}
		}

		/// <summary>
		/// Blend ratio of late reverb to early reflections.
		/// Type: float Units: Percent Range: [0, 100] Default: 50
		/// </summary>
		public float EarlyLateMix
		{
			get
			{
				return _EarlyLateMix;
			}
			set
			{
				_EarlyLateMix = Math.Clamp(value, 0.0F, 100.0F);
				SetParameterFloat((int)FMOD.DSP_SFXREVERB.EARLYLATEMIX, _EarlyLateMix);
			}
		}

		/// <summary>
		/// Reverb signal level.
		/// Type: float Units: Decibels Range: [-80, 20] Default: -6
		/// </summary>
		public float WetLevel
		{
			get
			{
				return _WetLevel;
			}
			set
			{
				_WetLevel = Math.Clamp(value, -80.0F, 20.0F);
				SetParameterFloat((int)FMOD.DSP_SFXREVERB.WETLEVEL, _WetLevel);
			}
		}

		/// <summary>
		/// Dry signal level.
		/// Type: float Units: Decibels Range: [-80, 20] Default: 0
		/// </summary>
		public float DryLevel
		{
			get
			{
				return _DryLevel;
			}
			set
			{
				_DryLevel = Math.Clamp(value, -80.0F, 20.0F);
				SetParameterFloat((int)FMOD.DSP_SFXREVERB.DRYLEVEL, _DryLevel);
			}
		}

        /// <summary>
        /// CREATE DSP FOR LOWPASS FILTER
        /// </summary>
        /// <param name="system"></param>
        public SFXReverb(FMOD.System system) : base(system, FMOD.DSP_TYPE.SFXREVERB)
		{
		}

		public override void SetDefault()
		{
			SetPreset(FMOD.PRESET.OFF());
		}

		public void SetPreset(FMOD.REVERB_PROPERTIES preset)
        {
			DecayTime = preset.DecayTime;
			EarlyDelay = preset.EarlyDelay;
			LateDelay = preset.LateDelay;
			HFReference = preset.HFReference;
			HFDecayRatio = preset.HFDecayRatio;
			Diffusion = preset.Diffusion;
			Density = preset.Density;
			LowShelfFrequency = preset.LowShelfFrequency;
			LowShelfGain = preset.LowShelfGain;
			HighCut = preset.HighCut;
			EarlyLateMix = preset.EarlyLateMix;
			WetLevel = preset.WetLevel;
			DryLevel = 0;
		}
	}
}
