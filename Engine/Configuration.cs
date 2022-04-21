using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine
{
    public class ConfigurationData
    {
        /// ===================================
        /// 出力設定
        /// ===================================
        /// <summary>
        /// 出力形式
        /// </summary>
        public int OutputType { get; set; }

        /// <summary>
        /// デバイス
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// サンプルレート
        /// </summary>
        public int SampleRate { get; set; }

        /// <summary>
        /// フォーマット
        /// </summary>
        public int Format { get; set; }

        /// <summary>
        /// サンプリングモード
        /// </summary>
        public int SamplingMode { get; set; }

        /// <summary>
        /// スピーカーモード
        /// </summary>
        public int SpeakerMode { get; set; }

        /// ===================================
        /// エフェクト
        /// ===================================
        public int EffectGEQ_10 { get; set; }

    }

    public class Configration
    {
        public ConfigurationData settings;


    }
}
