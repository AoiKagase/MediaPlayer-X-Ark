using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector
{
	public class Distortion : AbstractEffectorBase
	{
		private float _level;

		/// <summary>
		/// Distortion value.
		/// Type: float Units: Linear Range: [0, 1] Default: 0.5
		/// </summary>
		public float Level
		{
			get
			{
				GetParameterFloat((int)FMOD.DSP_DISTORTION.LEVEL, out _level);
				return _level;
			}
			set
			{
				if (_level != value)
				{
					_level = Math.Clamp(value, 0.0F, 1.0F);
					SetParameterFloat((int)FMOD.DSP_DISTORTION.LEVEL, _level);
				}
			}
		}


		/// <summary>
		/// CREATE DSP FOR LOWPASS FILTER
		/// </summary>
		/// <param name="system"></param>
		public Distortion(FMOD.System system) : base(system, FMOD.DSP_TYPE.DISTORTION)
		{
		}

        public override void SetDefault()
        {
			Level = 0.5f;
        }
    }
}
