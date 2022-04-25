using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
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
        public int Effect_GEQ_10 { get; set; }

        
        /// <summary>
        /// スキン
        /// </summary>
        public string Skin { get; set; }
    }

    public class Configration
    {
        public ConfigurationData settings;

        public Configration(ref PlayerEngine engine)
        {
            if (File.Exists("config.json"))
            {
                string jsonString = File.ReadAllText("config.json", Encoding.UTF8);
                settings = JsonSerializer.Deserialize<ConfigurationData>(jsonString);
            }
            else
            {
                settings = new ConfigurationData();
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
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(settings, options);

            File.WriteAllText("config.json", jsonString);
        }

        public FMOD.SOUND_FORMAT getSoundFormat()
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

        public FMOD.OUTPUTTYPE getOutputType()
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

        public SOFTWARE_SAMPLE_RATE getSampleRate()
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

        public FMOD.SPEAKERMODE getSpeakerMode()
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
    }
}
