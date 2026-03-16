using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector.Presets
{
    public class GEqualizerPreset : EffectPreset
    {
        public override string EffectName => "GEQ";
        public float Hz32 { get; set; } = 0;
        public float Hz60 { get; set; } = 0;
        public float Hz125 { get; set; } = 0;
        public float Hz250 { get; set; } = 0;
        public float Hz500 { get; set; } = 0;
        public float Hz1K { get; set; } = 0;
        public float Hz2K { get; set; } = 0;
        public float Hz4K { get; set; } = 0;
        public float Hz8K { get; set; } = 0;
        public float Hz16K { get; set; } = 0;
        public float Hz20K { get; set; } = 0;
        public float Hz22K { get; set; } = 0;
    }
}
