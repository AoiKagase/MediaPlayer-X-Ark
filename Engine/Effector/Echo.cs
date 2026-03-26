using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector
{
	public class Echo : AbstractEffectorBase
	{
		private float _delay;
		private float _feedback;
		private float _drylevel;
		private float _wetlevel;

		/// <summary>
		/// Echo delay.
		/// Type: float Units: Milliseconds Range: [1, 5000] Default: 500
		/// </summary>
		public float Delay
		{
			get
			{
				return _delay;
			}
			set
			{
				if (_delay != value)
				{
					_delay = Math.Clamp(value, 1.0F, 5000.0F);
					SetParameterFloat((int)FMOD.DSP_ECHO.DELAY, _delay);
				}
			}
		}

		/// <summary>
		/// Echo decay per delay. 100.0 = No decay, 0.0 = total decay.
		/// Type: float Units: Percentage Range: [0, 100] Default: 50
		/// </summary>
		public float Feedback
		{
			get
			{
				return _feedback;
			}
			set
			{
				if (value != _feedback)
				{
					_feedback = Math.Clamp(value, 0.0F, 100.0F);
					SetParameterFloat((int)FMOD.DSP_ECHO.FEEDBACK, _feedback);
				}
			}
		}

		/// <summary>
		/// Original sound volume.
		/// Type: float Units: Decibels Range: [-80, 10] Default: 0
		/// </summary>
		public float DryLevel
		{
			get
			{
				return _drylevel;
			}
			set
			{
				if (value != _drylevel)
				{
					_drylevel = Math.Clamp(value, -80.0F, 10.0F);
					SetParameterFloat((int)FMOD.DSP_ECHO.DRYLEVEL, _drylevel);
				}
			}
		}

		/// <summary>
		/// Volume of echo signal to pass to output.
		/// Type: float Units: Decibels Range: [-80, 10] Default: 0
		/// </summary>
		public float WetLevel
		{
			get
			{
				return _wetlevel;
			}
			set
			{
				if (value != _wetlevel)
				{
					_wetlevel = Math.Clamp(value, -80.0F, 10.0F);
					SetParameterFloat((int)FMOD.DSP_ECHO.WETLEVEL, _wetlevel);
				}
			}
		}
		/// <summary>
		/// CREATE DSP FOR LOWPASS FILTER
		/// </summary>
		/// <param name="system"></param>
		public Echo(FMOD.System system)
		{
			Initialize(system, FMOD.DSP_TYPE.ECHO);
        }

        public override void SetDefault()
        {
			Delay = 500;
			Feedback = 50;
			DryLevel = 0;
			WetLevel = 0;
		}
	}
}
