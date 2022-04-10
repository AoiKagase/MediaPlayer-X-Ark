using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine
{
	public class FmodEffectorLowpass : FmodEffectorBase
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
					if (value < 1.0F)
						value = 1.0F;
					if (value > 22000.0F)
						value = 22000.0F;

					_cutoff = value;
					SetParameter((int)FMOD.DSP_LOWPASS.CUTOFF, value);
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
					if (value < 0.0F)
						value = 0.0F;
					if (value > 10.0F)
						value = 10.0F;

					_resonance = value;
					SetParameter((int)FMOD.DSP_LOWPASS.RESONANCE, value);
				}
			}
		}

		/// <summary>
		/// CREATE DSP FOR LOWPASS FILTER
		/// </summary>
		/// <param name="system"></param>
		public FmodEffectorLowpass(FMOD.System system) : base(system, FMOD.DSP_TYPE.LOWPASS)
		{
		}
	}
}
