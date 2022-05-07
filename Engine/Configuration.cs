using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;

namespace MediaPlayer_X_Ark.Engine
{
    public class ConfigurationData
    {
        /// ===================================
        /// 画面
        /// ===================================
        /// <summary>
        /// PAN
        /// </summary>
        public int Pan { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        public int Volume { get; set; }

        /// ===================================
        /// 出力設定
        /// ===================================
        /// <summary>
        /// 出力形式
        /// </summary>
        public int OutputType { get; set; }

        /// <summary>
        /// デバイス
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// サンプルレート
        /// </summary>
        public int SampleRate { get; set; }

        /// <summary>
        /// フォーマット
        /// </summary>
        public int Format { get; set; }

        /// <summary>
        /// サンプリングモード
        /// </summary>
        public int SamplingMode { get; set; }

        /// <summary>
        /// スピーカーモード
        /// </summary>
        public int SpeakerMode { get; set; }

        /// ===================================
        /// エフェクト
        /// ===================================
        public CfgEffectors Effectors { get; set; } = new CfgEffectors();
        
        /// <summary>
        /// スキン
        /// </summary>
        public string Skin { get; set; }
    }

    public class CfgEffectors
    {
        /// <summary>
        /// Graphic Equalizer
        /// </summary>
        [JsonPropertyName("GraphicEqualizer")]
        public CfgGEqualizer GEqualizer { get; set; } = new CfgGEqualizer();
        public CfgPitchShift PitchShift { get; set; } = new CfgPitchShift();
        public CfgFrequency Frequency { get; set; } = new CfgFrequency();
        public CfgSpeed Speed { get; set; } = new CfgSpeed();
        public CfgDistortion Distortion { get; set; } = new CfgDistortion();
        public CfgChorus Chorus { get; set; } = new CfgChorus();
        public CfgEcho Echo { get; set; } = new CfgEcho();
        public CfgFlanger Flanger { get; set; } = new CfgFlanger();
        public CfgHightpass Highpass { get; set; } = new CfgHightpass();
        public CfgLowpass Lowpass { get; set; } = new CfgLowpass();
        public CfgCompressor Compressor { get; set; } = new CfgCompressor();
        public CfgReverb Reverb { get; set; } = new CfgReverb();
    }

    public class CfgGEqualizer
    {
        public bool Enable { get; set; }
        public int Preset { get; set; }
        [JsonPropertyName("32")]
        public decimal GEQ_32 { get; set; }
        [JsonPropertyName("60")]
        public decimal GEQ_60 { get; set; }
        [JsonPropertyName("125")]
        public decimal GEQ_125 { get; set; }
        [JsonPropertyName("250")]
        public decimal GEQ_250 { get; set; }
        [JsonPropertyName("500")]
        public decimal GEQ_500 { get; set; }
        [JsonPropertyName("1000")]
        public decimal GEQ_1K { get; set; }
        [JsonPropertyName("2000")]
        public decimal GEQ_2K { get; set; }
        [JsonPropertyName("4000")]
        public decimal GEQ_4K { get; set; }
        [JsonPropertyName("8000")]
        public decimal GEQ_8K { get; set; }
        [JsonPropertyName("16000")]
        public decimal GEQ_16K { get; set; }
        [JsonPropertyName("20000")]
        public decimal GEQ_20K { get; set; }
        [JsonPropertyName("22000")]
        public decimal GEQ_22K { get; set; }
    }

    public class CfgDistortion
    {
        public bool Enable { get; set; }
        public int Level { get; set; }
    }

    public class CfgPitchShift
    {
        public bool Enable { get; set; }
        public int Pitch { get; set; }
        public int FFT { get; set; }
    }

    public class CfgFrequency
    {
        public bool Enable { get; set; }
        public int Frequency { get; set; }
    }

    public class CfgSpeed
    {
        public bool Enable { get; set; }

        public int Speed { get; set; }
    }

    public class CfgChorus
    {
        public bool Enable { get; set; }
        public int Mix { get; set; }
        public int Rate { get; set; }
        public int Depth { get; set; }
    }

    public class CfgEcho
    {
        public bool Enable { get; set; }
        public int Delay { get; set; }
        public int Feedback { get; set; }
        public int Dry { get; set; }
        public int Wet { get; set; }
    }

    public class CfgFlanger
    {
        public bool Enable { get; set; }
        public int Mix { get; set; }
        public int Rate { get; set; }
        public int Depth { get; set; }
    }

    public class CfgHightpass
    {
        public bool Enable { get; set; }
        public int Cutoff { get; set; }
        public int Resonance { get; set; }
    }

    public class CfgLowpass
    {
        public bool Enable { get; set; }
        public int Cutoff { get; set; }
        public int Resonance { get; set; }
    }

    public class CfgCompressor
    {
        public bool Enable { get; set; }
        public int Threshold { get; set; }
        public int Ratio { get; set; }
        public int Attack { get; set; }
        public int Release { get; set; }
        public int Gain { get; set; }
    }

    public class CfgReverb
    {
        public bool Enable { get; set; }
        public int DecayTime { get; set; }
        public int EarlyDelay { get; set; }
        public int LateDelay { get; set; }
        public int HFRef { get; set; }
        public int HFDecayRatio { get; set; }
        public int Diffusion { get; set; }
        public int Density { get; set; }
        public int LowShelfFrequency { get; set; }
        public int LowShelfGain { get; set; }
        public int HighCut { get; set; }
        public int EarlyLate { get; set; }
        public int WetLevel { get; set; }
        public int DryLevel { get; set; }
    }
    public class Configration
    {
        public ConfigurationData settings;
        protected PlayerEngine engine;

        public Configration(ref PlayerEngine engine)
        {
            this.engine = engine;
            if (File.Exists("config.json"))
            {
                string jsonString = File.ReadAllText("config.json", Encoding.UTF8);
                settings = JsonSerializer.Deserialize<ConfigurationData>(jsonString);
            }
            else
            {
                settings = new ConfigurationData();
                settings.Effectors = new CfgEffectors();
                switch (engine.GetOutputType())
                {
                    case FMOD.OUTPUTTYPE.AUTODETECT:
                        settings.OutputType = 0;
                        break;
                    case FMOD.OUTPUTTYPE.WASAPI:
                        settings.OutputType = 1;
                        break;
                    case FMOD.OUTPUTTYPE.ASIO:
                        settings.OutputType = 2;
                        break;
                    case FMOD.OUTPUTTYPE.WINSONIC:
                        settings.OutputType = 3;
                        break;
                }
                engine.SetDevice(0);
                settings.Device = engine.GetDeviceGUID();
                int sampleRate;
                FMOD.SPEAKERMODE speakermode;
                int speakernum;
                engine.GetSoftwareFormat(out sampleRate, out speakermode, out speakernum);
                settings.Format = 1;
                switch (sampleRate)
                {
                    case 192000:
                        settings.SampleRate = 0;
                        break;
                    case 96000:
                        settings.SampleRate = 1;
                        break;
                    case 88200:
                        settings.SampleRate = 2;
                        break;
                    case 48000:
                        settings.SampleRate = 3;
                        break;
                    case 44100:
                        settings.SampleRate = 4;
                        break;
                    case 32000:
                        settings.SampleRate = 5;
                        break;
                    case 22050:
                        settings.SampleRate = 6;
                        break;
                    case 16000:
                        settings.SampleRate = 7;
                        break;
                    case 11025:
                        settings.SampleRate = 8;
                        break;
                    case 8000:
                        settings.SampleRate = 9;
                        break;
                    case 7333:
                        settings.SampleRate = 10;
                        break;
                    case 6000:
                        settings.SampleRate = 11;
                        break;
                    case 5500:
                        settings.SampleRate = 12;
                        break;
                    default:
                        settings.SampleRate = 4;
                        break;
                }
//                settings.SampleRate = sampleRate;
                switch(speakermode)
                {
                    // デフォルト
                    case FMOD.SPEAKERMODE.DEFAULT:
                        settings.SpeakerMode = 0;
                        break;
                    // モノラル
                    case FMOD.SPEAKERMODE.MONO:
                        settings.SpeakerMode = 1;
                        break;
                    // ステレオ
                    case FMOD.SPEAKERMODE.STEREO:
                        settings.SpeakerMode = 2;
                        break;
                    // 4.0
                    case FMOD.SPEAKERMODE.QUAD:
                        settings.SpeakerMode = 3;
                        break;
                    // 5.0
                    case FMOD.SPEAKERMODE.SURROUND:
                        settings.SpeakerMode = 4;
                        break;
                    // 5.1
                    case FMOD.SPEAKERMODE._5POINT1:
                        settings.SpeakerMode = 5;
                        break;
                    // 7.1
                    case FMOD.SPEAKERMODE._7POINT1:
                        settings.SpeakerMode = 6;
                        break;
                    // 7.1.4
                    case FMOD.SPEAKERMODE._7POINT1POINT4:
                        settings.SpeakerMode = 7;
                        break;
                    default:
                        settings.SpeakerMode = 2;
                        break;
                }

                settings.Skin = "bbbs\\bs.xsf";
            }
        }

        public void Save()
        {
            GetEqualizerValue();

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(settings, options);

            File.WriteAllText("config.json", jsonString);
        }

        public FMOD.SOUND_FORMAT GetSoundFormat()
        {
            switch(settings.Format)
            {
                // 8bit integer PCM
                case 0:
                    return FMOD.SOUND_FORMAT.PCM8;
                // 16bit integer PCM
                case 1:
                    return FMOD.SOUND_FORMAT.PCM16;
                // 24bit integer PCM
                case 2:
                    return FMOD.SOUND_FORMAT.PCM24;
                // 32bit integer PCM
                case 3:
                    return FMOD.SOUND_FORMAT.PCM32;
                // 32bit floating point PCM
                case 4:
                    return FMOD.SOUND_FORMAT.PCMFLOAT;
                default:
                    return FMOD.SOUND_FORMAT.PCM16;
            }
        }

        public FMOD.OUTPUTTYPE GetOutputType()
        {
            switch(settings.OutputType)
            {
                case 0:
                    return FMOD.OUTPUTTYPE.AUTODETECT;
                case 1:
                    return FMOD.OUTPUTTYPE.WASAPI;
                case 2:
                    return FMOD.OUTPUTTYPE.ASIO;
                case 3:
                    return FMOD.OUTPUTTYPE.WINSONIC;
                default:
                    return FMOD.OUTPUTTYPE.AUTODETECT;
            }
        }

        public SOFTWARE_SAMPLE_RATE GetSampleRate()
        {
            switch (settings.SampleRate)
            {
                case 0:
                    return SOFTWARE_SAMPLE_RATE.SAMPLE_192000HZ;
                case 1:
                    return SOFTWARE_SAMPLE_RATE.SAMPLE_96000HZ;
                case 2:
                    return SOFTWARE_SAMPLE_RATE.SAMPLE_88200HZ;
                case 3:
                    return SOFTWARE_SAMPLE_RATE.SAMPLE_48000HZ;
                case 4:
                    return SOFTWARE_SAMPLE_RATE.SAMPLE_44100HZ;
                case 5:
                    return SOFTWARE_SAMPLE_RATE.SAMPLE_32000HZ;
                case 6:
                    return SOFTWARE_SAMPLE_RATE.SAMPLE_22050HZ;
                case 7:
                    return SOFTWARE_SAMPLE_RATE.SAMPLE_16000HZ;
                case 8:
                    return SOFTWARE_SAMPLE_RATE.SAMPLE_11025HZ;
                case 9:
                    return SOFTWARE_SAMPLE_RATE.SAMPLE_8000HZ;
                case 10:
                    return SOFTWARE_SAMPLE_RATE.SAMPLE_7333HZ;
                case 11:
                    return SOFTWARE_SAMPLE_RATE.SAMPLE_6000HZ;
                case 12:
                    return SOFTWARE_SAMPLE_RATE.SAMPLE_5500HZ;
                default:
                    return SOFTWARE_SAMPLE_RATE.SAMPLE_44100HZ;
            }
        }

        public FMOD.SPEAKERMODE GetSpeakerMode()
        {
            switch (settings.SpeakerMode)
            {
                // デフォルト
                case 0:
                    return FMOD.SPEAKERMODE.DEFAULT;
                // モノラル
                case 1:
                    return FMOD.SPEAKERMODE.MONO;
                // ステレオ
                case 2:
                    return FMOD.SPEAKERMODE.STEREO;
                // 4.0
                case 3:
                    return FMOD.SPEAKERMODE.QUAD;
                // 5.0
                case 4:
                    return FMOD.SPEAKERMODE.SURROUND;
                // 5.1
                case 5:
                    return FMOD.SPEAKERMODE._5POINT1;
                // 7.1
                case 6:
                    return FMOD.SPEAKERMODE._7POINT1;
                // 7.1.4
                case 7:
                    return FMOD.SPEAKERMODE._7POINT1POINT4;
                default:
                    return FMOD.SPEAKERMODE.DEFAULT;
            }
        }

        public void GetEqualizerValue()
        {
            settings.Effectors.GEqualizer.GEQ_32 = (int)(engine.effector.GEqualizer.GetGain(Effector.GEqualizer.EQ_HZ.HZ_32) * 10f);
            settings.Effectors.GEqualizer.GEQ_60 = (int)(engine.effector.GEqualizer.GetGain(Effector.GEqualizer.EQ_HZ.HZ_60) * 10f);
            settings.Effectors.GEqualizer.GEQ_125 = (int)(engine.effector.GEqualizer.GetGain(Effector.GEqualizer.EQ_HZ.HZ_125) * 10f);
            settings.Effectors.GEqualizer.GEQ_250 = (int)(engine.effector.GEqualizer.GetGain(Effector.GEqualizer.EQ_HZ.HZ_250) * 10f);
            settings.Effectors.GEqualizer.GEQ_500 = (int)(engine.effector.GEqualizer.GetGain(Effector.GEqualizer.EQ_HZ.HZ_500) * 10f);
            settings.Effectors.GEqualizer.GEQ_1K = (int)(engine.effector.GEqualizer.GetGain(Effector.GEqualizer.EQ_HZ.HZ_1K) * 10f);
            settings.Effectors.GEqualizer.GEQ_2K = (int)(engine.effector.GEqualizer.GetGain(Effector.GEqualizer.EQ_HZ.HZ_2K) * 10f);
            settings.Effectors.GEqualizer.GEQ_4K = (int)(engine.effector.GEqualizer.GetGain(Effector.GEqualizer.EQ_HZ.HZ_4K) * 10f);
            settings.Effectors.GEqualizer.GEQ_8K = (int)(engine.effector.GEqualizer.GetGain(Effector.GEqualizer.EQ_HZ.HZ_8K) * 10f);
            settings.Effectors.GEqualizer.GEQ_16K = (int)(engine.effector.GEqualizer.GetGain(Effector.GEqualizer.EQ_HZ.HZ_16K) * 10f);
            settings.Effectors.GEqualizer.GEQ_20K = (int)(engine.effector.GEqualizer.GetGain(Effector.GEqualizer.EQ_HZ.HZ_20K) * 10f);
            settings.Effectors.GEqualizer.GEQ_22K = (int)(engine.effector.GEqualizer.GetGain(Effector.GEqualizer.EQ_HZ.HZ_22K) * 10f);
        }
    }
}
