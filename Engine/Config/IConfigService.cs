using System;

namespace MediaPlayer_X_Ark.Engine.Config
{
    // Minimal configuration service interface — expand as needed during migration.
    public interface IConfigService
    {
        public ConfigurationData settings { get; }
        void Save();
        FMOD.OUTPUTTYPE GetOutputType();
    }
}
