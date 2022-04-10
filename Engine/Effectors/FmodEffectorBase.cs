using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine
{
    public class FmodEffectorBase
    {
        protected FMOD.System _system;
        protected FMOD.DSP _dsp;
        protected FMOD.ChannelGroup _channelGroup;

        public FmodEffectorBase(FMOD.System system, FMOD.DSP_TYPE dspType)
        {
            if (system.createDSPByType(dspType, out _dsp) == FMOD.RESULT.OK)
            {
                _system = system;
                _system.getMasterChannelGroup(out _channelGroup);
                _channelGroup.addDSP(0, _dsp);
            }
        }

        ~FmodEffectorBase()
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
            _dsp.getActive(out active);
            if (active)
                if (sw == false)
                    _dsp.setActive(false);
            else
                if (sw == true)
                    _dsp.setActive(true);

            return result;
        }

        public FMOD.RESULT SetParameter(int type, float value)
        {
            return _dsp.setParameterFloat(type, value);
        }
    }
}
