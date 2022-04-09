using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine
{
    public class FmodEffector
    {
        private FMOD.Channel _channel;
        private FMOD.DSP _dsp_lowpass;
        private FMOD.DSP _dsp_highpass;
        private FMOD.DSP _dsp_echo;
        private FMOD.DSP _dsp_flanger;
        private FMOD.DSP _dsp_distortion;
        private FMOD.DSP _dsp_chorus;
        private FMOD.DSP _dsp_compressor;
        private FMOD.DSP _dsp_reverb;
        private FMOD.DSP _dsp_pitch1;
        private FMOD.DSP _dsp_pitch2;
        private FMOD.DSP[] _dsp_p_equalizer;

        private bool _switch_lowpass;
        private bool _switch_highpass;
        private bool _switch_echo;
        private bool _switch_flanger;
        private bool _switch_distortion;
        private bool _switch_chorus;
        private bool _switch_compressor;
        private bool _switch_reverb;
        private bool _switch_pitch1;
        private bool _switch_pitch2;
        private bool _switch_p_equalizer;

        public void SwitchLowpass(bool sw)
        {
            FMOD.RESULT result;
            bool active;
            _dsp_lowpass.getActive(out active);
            if (active)
                if (sw == false)
                    _channel.removeDSP(_dsp_lowpass);
            else
                if (sw == true)
                    _channel.addDSP(0, _dsp_lowpass);
        }
    }
}
