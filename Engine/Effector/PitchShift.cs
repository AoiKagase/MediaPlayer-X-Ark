using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector
{
	public class PitchShift : AbstractEffectorBase
	{
		private float _pitch;
		private float _fftsize;
//		private float _overlap;

		/// <summary>
		/// Pitch value. 0.5 to 2.0. Default = 1.0. 0.5 = one octave down, 2.0 = one octave up. 1.0 does not change the pitch.
		/// Type: float Range: [0.5, 2] Default: 1
		/// </summary>
		public float Pitch
		{
			get
			{
				return _pitch;
			}
			set
			{
				if (_pitch != value)
				{
					_pitch = Math.Clamp(value, 0.5F, 2.0F);
					SetParameterFloat((int)FMOD.DSP_PITCHSHIFT.PITCH, _pitch);
				}
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		/// FFT window size - 256, 512, 1024, 2048, 4096. Increase this to reduce 'smearing'. This effect is a warbling sound similar to when an mp3 is encoded at very low bitrates.
		/// Type: float Default: 1024
		/// </summary>
		public float FFTSize
		{
			get
			{
				return _fftsize;
			}
			set
			{
				if (value != _fftsize)
				{
					_fftsize = Math.Clamp(value, 0.0F, 4096.0F);
					SetParameterFloat((int)FMOD.DSP_PITCHSHIFT.FFTSIZE, _fftsize);
				}
			}
		}

		/// <summary>
		/// Removed. Do not use. FMOD now uses 4 overlaps and cannot be changed.
		/// </summary>
		//public float Overlap
		//{
		//	get
		//	{
		//		return _overlap;
		//	}
		//	set
		//	{
		//		if (value != _overlap)
		//		{
		//			_overlap = Math.Clamp(value, 0.0F, 100.0F);
		//			SetParameterFloat((int)FMOD.DSP_PITCHSHIFT.OVERLAP, _overlap);
		//		}
		//	}
		//}

		/// <summary>
		/// CREATE DSP FOR LOWPASS FILTER
		/// </summary>
		/// <param name="system"></param>
		public PitchShift(FMOD.System system)
		{
			Initialize(system, FMOD.DSP_TYPE.PITCHSHIFT);
        }

		/// <summary>
		/// Set default parameters.
		/// </summary>
		public override void SetDefault()
        {
			Pitch = 1;
			FFTSize = 1024;
        }
	}
}
