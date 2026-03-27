namespace MediaPlayer_X_Ark.Engine.Player
{
    public class PluginLoadResult
    {
        public string FileName { get; set; }
        public string PluginName { get; set; }
        public bool Success { get; set; }
        public FMOD.PLUGINTYPE Type { get; set; }
        public uint Version { get; set; }

        public string TypeLabel => Type switch
        {
            FMOD.PLUGINTYPE.CODEC => "Codec",
            FMOD.PLUGINTYPE.DSP => "DSP",
            FMOD.PLUGINTYPE.OUTPUT => "Output",
            _ => "Unknown",
        };

        public string VersionLabel
        {
            get
            {
                uint major = (Version >> 16) & 0xFFFF;
                uint minor = Version & 0xFFFF;
                return $"{major}.{minor:D2}";
            }
        }
    }
}