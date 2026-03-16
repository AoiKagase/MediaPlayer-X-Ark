using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector.Presets
{
    public class EchoPreset : EffectPreset
    {
        public override string EffectName => "Echo";
        public float Delay { get; set; } = 500;
        public float Feedback { get; set; } = 50;
        public float Dry { get; set; } = 0;
        public float Wet { get; set; } = 0;
    }
}
