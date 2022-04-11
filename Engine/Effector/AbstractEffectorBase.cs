using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector
{
	public class AbstractEffectorBase
	{
		protected FMOD.System _system;
		protected FMOD.DSP _dsp;
		protected FMOD.ChannelGroup _channelGroup;

		public AbstractEffectorBase(FMOD.System system, FMOD.DSP_TYPE dspType)
		{
			if (system.createDSPByType(dspType, out _dsp) == FMOD.RESULT.OK)
			{
				_system = system;
				_system.getMasterChannelGroup(out _channelGroup);
				_channelGroup.addDSP(0, _dsp);
			}
		}

		~AbstractEffectorBase()
		{
			if (_dsp.hasHandle())
				_channelGroup.removeDSP(_dsp);

			_dsp.release();
			//_channelGroup.release();
		}

		public FMOD.RESULT Switch(bool sw)
		{
			FMOD.RESULT result = FMOD.RESULT.OK;
			bool active;
			_dsp.getBypass(out active);
			if (active)
            {
				if (sw == true)
					_dsp.setBypass(false);
			}
			else
            {
				if (sw == false)
					_dsp.setBypass(true);
			}

			return result;
		}
		public FMOD.RESULT GetParameterFloat(int type, out float value)
		{
			return _dsp.getParameterFloat(type, out value);
		}

		public FMOD.RESULT SetParameterFloat(int type, float value)
		{
			return _dsp.setParameterFloat(type, value);
		}
		public FMOD.RESULT SetParameterBool(int type, bool value)
		{
			return _dsp.setParameterBool(type, value);
		}
		public FMOD.RESULT SetParameterData(int type, byte[] value)
		{
			return _dsp.setParameterData(type, value);
		}
	}
}
