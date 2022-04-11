using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector
{
	public class Chorus : AbstractEffectorBase
	{
		private float _mix;
		private float _rate;
		private float _depth;

		/// <summary>
		/// Percentage of wet signal in mix.
		/// Type: float Units: Percentage Range: [0, 100] Default: 50
		/// </summary>
		public float Mix
		{
			get
			{
				return _mix;
			}
			set
			{
				if (_mix != value)
				{
					_mix = Math.Clamp(value, 0.0F, 100.0F);
					SetParameterFloat((int)FMOD.DSP_CHORUS.MIX, _mix);
				}
			}
		}

		/// <summary>
		/// Chorus modulation rate.
		/// Type: float Units: Hertz Range: [0, 20] Default: 0.8
		/// </summary>
		public float Rate
		{
			get
			{
				return _rate;
			}
			set
			{
				if (value != _rate)
				{
					_rate = Math.Clamp(value, 0.0F, 20.0F);
					SetParameterFloat((int)FMOD.DSP_CHORUS.RATE, _rate);
				}
			}
		}

		/// <summary>
		/// Chorus modulation depth.
		/// Type: float Units: Milliseconds Range: [0, 100] Default: 3
		/// </summary>
		public float Depth
		{
			get
			{
				return _depth;
			}
			set
			{
				if (value != _depth)
				{
					_depth = Math.Clamp(value, 0.0F, 100.0F);
					SetParameterFloat((int)FMOD.DSP_CHORUS.DEPTH, _depth);
				}
			}
		}
		/// <summary>
		/// CREATE DSP FOR LOWPASS FILTER
		/// </summary>
		/// <param name="system"></param>
		public Chorus(FMOD.System system) : base(system, FMOD.DSP_TYPE.CHORUS)
		{
		}
	}
}
