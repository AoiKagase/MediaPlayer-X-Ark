using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector
{
	public class Flanger : AbstractEffectorBase
	{
		private float _mix;
		private float _depth;
		private float _rate;

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
					SetParameterFloat((int)FMOD.DSP_FLANGE.MIX, _mix);
				}
			}
		}

		/// <summary>
		/// Flange speed.
		/// Type: float Units: Hertz Range: [0, 20] Default: 0.1
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
					SetParameterFloat((int)FMOD.DSP_FLANGE.RATE, _rate);
				}
			}
		}

		/// <summary>
		/// Flange depth.
		/// Type: float Units: Linear Range: [0.01, 1] Default: 1
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
					_depth = Math.Clamp(value, 0.01F, 1.0F);
					SetParameterFloat((int)FMOD.DSP_FLANGE.DEPTH, _depth);
				}
			}
		}
		/// <summary>
		/// CREATE DSP FOR LOWPASS FILTER
		/// </summary>
		/// <param name="system"></param>
		public Flanger(FMOD.System system) : base(system, FMOD.DSP_TYPE.FLANGE)
		{
		}

        public override void SetDefault()
        {
			Mix = 50;
			Rate = 0.1f;
			Depth = 1;
        }
    }
}
