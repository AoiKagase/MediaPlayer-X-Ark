namespace MediaPlayer_X_Ark.Engine.Effector.Presets
{
    public class NormalizePreset : EffectPreset
    {
        public override string EffectName => "Normalize";

        public float FadeTime  { get; set; } = 5000.0f;
        public float MaxAmp    { get; set; } = 1.0f;
        public float Threshold { get; set; } = 0.0f;
    }
}
