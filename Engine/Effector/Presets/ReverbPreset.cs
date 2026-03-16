using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector.Presets
{
    public class ReverbPreset : EffectPreset
    {
        public override string EffectName => "Reverb";
        public float DecayTime { get; set; } = 1500;
        public float EarlyDelay { get; set; } = 20;
        public float LateDelay { get; set; } = 40;
        public float HFReference { get; set; } = 5000;
        public float HFDecayRatio { get; set; } = 50;
        public float Diffusion { get; set; } = 50;
        public float Density { get; set; } = 50;
        public float LowShelfFreq { get; set; } = 250;
        public float LowShelfGain { get; set; } = 0;
        public float HighCut { get; set; } = 20000;
        public float EarlyLateMix { get; set; } = 50;
        public float WetLevel { get; set; } = -6;
        public float DryLevel { get; set; } = 0;
    }
}
