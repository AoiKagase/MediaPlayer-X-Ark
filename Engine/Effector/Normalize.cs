namespace MediaPlayer_X_Ark.Engine.Effector
{
    /// <summary>
    /// FMOD Normalize DSP。
    /// 音声信号のピークを一定レベルに揃えるラウドネスマキシマイザ。
    ///
    /// FMOD DSP_NORMALIZE パラメーター：
    ///   FADETIME  : フェードタイム（ms）  Range: [1, 20000]  Default: 5000
    ///   THRESHOLD : ゲインを適用しない無音閾値  Range: [0, 1]  Default: 0
    ///   MAXAMP    : 最大出力振幅  Range: [0, 10]  Default: 1
    /// </summary>
    public class Normalize : AbstractEffectorBase
    {
        private float _fadeTime;
        private float _threshold;
        private float _maxAmp;

        /// <summary>
        /// ゲイン変化のフェードタイム（ms）。
        /// 急激なゲイン変化を滑らかにする。
        /// Range: [1, 20000]  Default: 5000
        /// </summary>
        public float FadeTime
        {
            get => _fadeTime;
            set
            {
                if (_fadeTime != value)
                {
                    _fadeTime = System.Math.Clamp(value, 1.0f, 20000.0f);
                    SetParameterFloat((int)FMOD.DSP_NORMALIZE.FADETIME, _fadeTime);
                }
            }
        }

        /// <summary>
        /// ゲインを適用しない無音閾値。
        /// 0 = 常に適用、1 = フルスケール以上のみ適用。
        /// Range: [0, 1]  Default: 0
        /// </summary>
        public float Threshold
        {
            get => _threshold;
            set
            {
                if (_threshold != value)
                {
                    _threshold = System.Math.Clamp(value, 0.0f, 1.0f);
                    SetParameterFloat((int)FMOD.DSP_NORMALIZE.THRESHOLD, _threshold);
                }
            }
        }

        /// <summary>
        /// 最大出力振幅（線形）。
        /// 1.0 = クリッピングなし、1以上 = ブースト可能（歪みに注意）。
        /// Range: [0, 10]  Default: 1
        /// </summary>
        public float MaxAmp
        {
            get => _maxAmp;
            set
            {
                if (_maxAmp != value)
                {
                    _maxAmp = System.Math.Clamp(value, 0.0f, 10.0f);
                    SetParameterFloat((int)FMOD.DSP_NORMALIZE.MAXAMP, _maxAmp);
                }
            }
        }

        public Normalize(FMOD.System system)
        {
            Initialize(system, FMOD.DSP_TYPE.NORMALIZE);
        }

        public override void SetDefault()
        {
            FadeTime  = 5000.0f;
            Threshold = 0.0f;
            MaxAmp    = 1.0f;
        }
    }
}
