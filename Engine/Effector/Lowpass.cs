using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector
{
	public class Lowpass : AbstractEffectorBase
	{
		private float _cutoff;
		private float _resonance;

		/// <summary>
		/// Lowpass cutoff frequency.
		/// Type: float Units: Hertz Range: [1, 22000] Default: 5000
		/// </summary>
		public float CutOff
		{
			get
			{
				return _cutoff;
			}
			set
			{
				if (_cutoff != value)
				{
					_cutoff = Math.Clamp(value, 1.0F, 22000.0F);
					SetParameterFloat((int)FMOD.DSP_LOWPASS.CUTOFF, _cutoff);
				}
			}
		}

		/// <summary>
		/// Lowpass resonance Q value.
		/// Type: float Range: [0, 10] Default: 1
		/// </summary>
		public float Resonance
		{
			get
			{
				return _resonance;
			}
			set
			{
				if (value != _resonance)
				{
					_resonance = Math.Clamp(value, 0.0F, 10.0F);
					SetParameterFloat((int)FMOD.DSP_LOWPASS.RESONANCE, _resonance);
				}
			}
		}

		/// <summary>
		/// CREATE DSP FOR LOWPASS FILTER
		/// </summary>
		/// <param name="system"></param>
		public Lowpass(FMOD.System system)
		{
			Initialize(system, FMOD.DSP_TYPE.LOWPASS);
        }
		public override void SetDefault()
		{
			CutOff = 5000;
			Resonance = 1;
		}
	}
}
