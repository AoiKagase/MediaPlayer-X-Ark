namespace MediaPlayer_X_Ark.Engine
{
    // Temporary static holder used during incremental migration to DI.
    public static class PlayerEngineStaticHolder
    {
        public static IPlayerEngine EngineInstance;
        public static IConfigService ConfigService { get; set; }
    }
}
