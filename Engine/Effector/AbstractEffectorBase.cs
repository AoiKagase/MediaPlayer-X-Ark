using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MediaPlayer_X_Ark.Engine.Effector
{
	public class AbstractEffectorBase : IEffector
    {
        protected IEffectorDsp _dsp;
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

        /// <summary>
        /// FMODブリッジ経由でDSPを初期化する。
        /// 各エフェクタークラスのコンストラクタから呼ぶ。
        /// </summary>
        protected void Initialize(FMOD.System system, FMOD.DSP_TYPE dspType)
        {
            _dsp = new FmodDspBridge(system, dspType);
            if (_dsp.IsValid)
                SetDefault();
        }
        ~AbstractEffectorBase()
        {
            _dsp?.Release();
        }

        public FMOD.RESULT Switch(bool sw)
        {
            if (_dsp == null || !_dsp.IsValid) return FMOD.RESULT.ERR_INVALID_HANDLE;
            _dsp.Bypass = !sw;
            Enabled = sw;
            return FMOD.RESULT.OK;
        }
        public FMOD.RESULT GetParameterFloat(int type, out float value)
        {
            if (_dsp == null || !_dsp.IsValid) { value = 0f; return FMOD.RESULT.ERR_INVALID_HANDLE; }
            return _dsp.GetParameterFloat(type, out value);
        }

        public FMOD.RESULT SetParameterFloat(int type, float value)
        {
            if (_dsp == null || !_dsp.IsValid) return FMOD.RESULT.ERR_INVALID_HANDLE;
            return _dsp.SetParameterFloat(type, value);
        }

        public FMOD.RESULT SetParameterBool(int type, bool value)
        {
            if (_dsp == null || !_dsp.IsValid) return FMOD.RESULT.ERR_INVALID_HANDLE;
            return _dsp.SetParameterBool(type, value);
        }

        public FMOD.RESULT SetParameterData(int type, byte[] value)
        {
            if (_dsp == null || !_dsp.IsValid) return FMOD.RESULT.ERR_INVALID_HANDLE;
            return _dsp.SetParameterData(type, value);
        }

        public virtual void SetDefault() { }
	}
}
