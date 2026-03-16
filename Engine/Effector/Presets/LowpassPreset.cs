using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector.Presets
{
    public class LowpassPreset : EffectPreset
    {
        public override string EffectName => "Lowpass";
        public float Cutoff { get; set; } = 5000;
        public float Resonance { get; set; } = 1;
    }
}
