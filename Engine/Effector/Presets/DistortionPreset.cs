using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector.Presets
{
    public class DistortionPreset : EffectPreset
    {
        public override string EffectName => "Distortion";
        public float Level { get; set; } = 0;
    }
}
