using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Effector
{
    public class Effectors
    {
        public Chorus Chorus { get; private set; }
        public Compressor Compressor { get; private set; }
        public Distortion Distortion { get; private set; }
        public Echo Echo { get; private set; }
        public Flanger Flanger { get; private set; }
        public Highpass Highpass { get; private set; }
        public Lowpass Lowpass { get; private set; }
        public SFXReverb SFXReverb { get; private set; }

        public Effectors(FMOD.System system)
        {
            Chorus = new Chorus(system);
            Compressor = new Compressor(system);
            Distortion = new Distortion(system);
            Echo = new Echo(system);
            Flanger = new Flanger(system);
            Highpass = new Highpass(system);
            Lowpass = new Lowpass(system);
            SFXReverb = new SFXReverb(system);
            Initialize();
        }

        public void Initialize()
        {
            Chorus.Switch(false);
            Compressor.Switch(false);
            Distortion.Switch(false);
            Echo.Switch(false);
            Flanger.Switch(false);
            Highpass.Switch(false);
            Lowpass.Switch(false);
            SFXReverb.Switch(false);
        }
    }
}
