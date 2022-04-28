using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MediaPlayer_X_Ark.Engine.Effector
{
	public class GEqualizer : INotifyPropertyChanged
	{
		/// <summary>
		/// EQ CENTER BAND
		/// </summary>
		public enum EQ_HZ
		{
			HZ_32 = 0,
			HZ_60,
			HZ_125,
			HZ_250,
			HZ_500,
			HZ_1K,
			HZ_2K,
			HZ_4K,
			HZ_8K,
			HZ_16K,
			HZ_20K,
			HZ_22K,
			HZ_MAX,
		}

		protected FMOD.System _system;
		protected FMOD.DSP[] _dsp;
		protected FMOD.ChannelGroup _channelGroup;

		private bool _enabled;

		public bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				if (_enabled != value)
					_enabled = value;
				NotifyPropertyChanged("Enabled");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private float[] _gain = new float[(int)EQ_HZ.HZ_MAX];
		public float[] Gain
		{
			get 
			{
				return _gain;
			}
		}

        /// <summary>
        /// CREATE DSP FOR LOWPASS FILTER
        /// </summary>
        /// <param name="system"></param>
        public GEqualizer(FMOD.System system)
		{
			_system = system;
			_system.getMasterChannelGroup(out _channelGroup);
			_dsp = new FMOD.DSP[12];
			for (int i = 0; i < 12; i++)
			{
				if (system.createDSPByType(FMOD.DSP_TYPE.PARAMEQ, out _dsp[i]) == FMOD.RESULT.OK)
				{
					_channelGroup.addDSP(0, _dsp[i]);
					SetDefault(i);
				}
			}
		}

		public void SetDefault(int index)
		{
			SetParameterFloat(index, (int)FMOD.DSP_PARAMEQ.CENTER, GetIndexToHz(index));
			SetParameterFloat(index, (int)FMOD.DSP_PARAMEQ.GAIN, 0);
			SetParameterFloat(index, (int)FMOD.DSP_PARAMEQ.BANDWIDTH, 1);
		}

		public void SetGain(EQ_HZ hz, float value)
        {
			_gain[(int)hz] = value;
			SetParameterFloat((int)hz, (int)FMOD.DSP_PARAMEQ.GAIN, value);
			NotifyPropertyChanged("Gain");
		}

		private float GetIndexToHz(int index)
        {
            switch (index)
            {
				case 0:
					return 32.0F;
				case 1:
					return 60.0F;
				case 2:
					return 125;
				case 3:
					return 250;
				case 4:
					return 500;
				case 5:
					return 1000;
				case 6:
					return 2000;
				case 7:
					return 4000;
				case 8:
					return 8000;
				case 9:
					return 16000;
				case 10:
					return 20000;
				case 11:
					return 22000;
				default:
					return 8000;
			}
        }

		public FMOD.RESULT Switch(bool sw)
		{
			FMOD.RESULT result = FMOD.RESULT.OK;
			for(int i = 0; i < 12; i++)
            {
				bool active;
				result = _dsp[i].getBypass(out active);
				if (active)
				{
					if (sw == true)
						result = _dsp[i].setBypass(false);
				}
				else
				{
					if (sw == false)
						result = _dsp[i].setBypass(true);
				}
			}
			Enabled = sw;
			return result;
		}

		public FMOD.RESULT GetParameterFloat(int index, int type, out float value)
		{
			return _dsp[index].getParameterFloat(type, out value);
		}

		public FMOD.RESULT SetParameterFloat(int index, int type, float value)
		{
			return _dsp[index].setParameterFloat(type, value);
		}
	}
}
