namespace MediaPlayer_X_Ark.Engine.Effector
{
    /// <summary>
    /// IEffectorDsp の FMOD 実装。
    /// FMOD への直接依存はこのクラスに集約する。
    /// </summary>
    internal sealed class FmodDspBridge : IEffectorDsp
    {
        private FMOD.DSP _dsp;
        private FMOD.ChannelGroup _channelGroup;

        public bool IsValid => _dsp.hasHandle();

        public bool Bypass
        {
            get { _dsp.getBypass(out bool b); return b; }
            set { _dsp.setBypass(value); }
        }

        public FmodDspBridge(FMOD.System system, FMOD.DSP_TYPE dspType)
        {
            if (system.createDSPByType(dspType, out _dsp) == FMOD.RESULT.OK)
            {
                system.getMasterChannelGroup(out _channelGroup);
                _channelGroup.addDSP(0, _dsp);
            }
        }

        public FMOD.RESULT GetParameterFloat(int index, out float value)
            => _dsp.getParameterFloat(index, out value);

        public FMOD.RESULT SetParameterFloat(int index, float value)
            => _dsp.setParameterFloat(index, value);

        public FMOD.RESULT SetParameterBool(int index, bool value)
            => _dsp.setParameterBool(index, value);

        public FMOD.RESULT SetParameterData(int index, byte[] value)
            => _dsp.setParameterData(index, value);

        public void Release()
        {
            if (_dsp.hasHandle())
                _channelGroup.removeDSP(_dsp);
            _dsp.release();
        }
    }
}