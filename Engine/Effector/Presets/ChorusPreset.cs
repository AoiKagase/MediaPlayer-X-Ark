using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector.Presets
{
    public class ChorusPreset : EffectPreset
    {
        public override string EffectName => "Chorus";
        public float Mix { get; set; } = 50;
        public float Rate { get; set; } = 0.8f;
        public float Depth { get; set; } = 3;
    }
}
