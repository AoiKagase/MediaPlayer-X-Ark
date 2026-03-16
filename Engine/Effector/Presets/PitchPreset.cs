using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector.Presets
{
    public class PitchPreset : EffectPreset
    {
        public override string EffectName => "Pitch";
        public float Pitch { get; set; } = 1.0f;
        public float FFTSize { get; set; } = 0;
        public float Frequency { get; set; } = 0;
        public float Speed { get; set; } = 0;
    }
}
