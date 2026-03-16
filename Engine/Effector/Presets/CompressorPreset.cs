using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector.Presets
{
    public class CompressorPreset : EffectPreset
    {
        public override string EffectName => "Compressor";
        public float Threshold { get; set; } = 0;
        public float Ratio { get; set; } = 2.5f;
        public float Attack { get; set; } = 20;
        public float Release { get; set; } = 100;
        public float Gain { get; set; } = 0;
        public bool Linked { get; set; } = false;
    }
}
