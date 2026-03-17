namespace MediaPlayer_X_Ark.Engine
{
    // Temporary static holder used during incremental migration to DI.
    public static class PlayerEngineStaticHolder
    {
        // Existing code constructs PlayerEngine manually in various places; provide a single instance for now.
        // Exposed as a field so it can be passed by `ref` during the incremental migration.
        public static MediaPlayer_X_Ark.PlayerEngine EngineInstance = new MediaPlayer_X_Ark.PlayerEngine();
        public static IConfigService ConfigService { get; set; }
    }
}
