using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using MediaPlayer_X_Ark.Engine.Player;
namespace MediaPlayer_X_Ark.Engine.Config
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

		public bool RestorePlaylist { get; set; } = false;
		public bool RestorePosition { get; set; } = false;
		public bool AutoSavePlaylist { get; set; } = false;
		public bool AlwaysOnTop { get; set; } = false;
		public int OpenFileAction { get; set; } = 0;
		public int DefaultSpectrumMode { get; set; } = 0;
		public bool SnowBlockEnabled { get; set; } = false;
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

		public string SoundFontPath { get; set; } = "";
		/// ===================================
		/// エフェクト
		/// ===================================
		public CfgEffectors Effectors { get; set; } = new CfgEffectors();
        public Dictionary<string, string> EffectPresets { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// スキン
        /// </summary>
        public string Skin { get; set; }

		public CfgBuffer Buffer { get; set; } = new CfgBuffer();

		public string LastMediaDirectory { get; set; } = "";

		public string LastPlaylistPath { get; set; } = "";
		public int LastPlayingIndex { get; set; } = -1;
		public uint LastPlayingPosition { get; set; } = 0;

		public List<string> CddbServers { get; set; } = DefaultCddbServers();

		public static List<string> DefaultCddbServers() => new List<string>
        {
			"http://freedbtest.dyndns.org:80/~cddb/cddb.cgi",  // 日本優先
			"http://gnudb.gnudb.org/~cddb/cddb.cgi",        // フォールバック
        };

		/// <summary>クロスフェードの有効/無効</summary>
		public bool CrossfadeEnabled { get; set; } = false;

		/// <summary>クロスフェード時間（ミリ秒）デフォルト3秒</summary>
		public int CrossfadeDurationMs { get; set; } = 3000;
        /// <summary>NonStopMixの有効/無効（クロスフェードと排他）</summary>
        public bool NonStopMixEnabled { get; set; } = false;
		/// <summary>
		/// NonStopMix切替オフセット（秒）。
		/// 負値 = 実音終了より早く切る、0 = 無音検知時間。
		/// 範囲：-1000.0〜+0.0
		/// </summary>
		public float NonStopMixOffsetSec { get; set; } = 0.0f;
		/// <summary>ReplayGainの有効/無効</summary>
		public bool ReplayGainEnabled { get; set; } = false;

		/// <summary>
		/// ReplayGainモード
		///   0 = トラック（曲単位で正規化）
		///   1 = アルバム（アルバム単位で正規化）
		/// </summary>
		public int ReplayGainMode { get; set; } = 0;

		/// <summary>プリアンプゲイン（dB）-6〜+6</summary>
		public float ReplayGainPreamp { get; set; } = 0.0f;
        /// <summary>Discord Rich Presence の有効/無効</summary>
        public bool DiscordRichPresenceEnabled { get; set; } = false;
        public string DiscordApplicationId { get; set; } = "";
    }
	public class CfgBuffer
	{
		/// <summary>
		/// ストリームバッファサイズ（KB）
		/// デフォルト：128KB、推奨範囲：16〜512
		/// </summary>
		public int StreamBufferSizeKB { get; set; } = 128;

		/// <summary>
		/// DSPバッファサイズ（サンプル数）
		/// デフォルト：2048、推奨値：512/1024/2048/4096
		/// </summary>
		public int DspBufferSize { get; set; } = 2048;

		/// <summary>
		/// DSPバッファ数
		/// デフォルト：4
		/// </summary>
		public int DspBufferCount { get; set; } = 4;
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
        public CfgNormalize Normalize { get; set; } = new CfgNormalize();
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

        // Helper to update band value by index to avoid duplicated switch logic across UI
        public void SetByIndex(int index, int value)
        {
            switch (index)
            {
                case 0: GEQ_32 = value; break;
                case 1: GEQ_60 = value; break;
                case 2: GEQ_125 = value; break;
                case 3: GEQ_250 = value; break;
                case 4: GEQ_500 = value; break;
                case 5: GEQ_1K = value; break;
                case 6: GEQ_2K = value; break;
                case 7: GEQ_4K = value; break;
                case 8: GEQ_8K = value; break;
                case 9: GEQ_16K = value; break;
                case 10: GEQ_20K = value; break;
                case 11: GEQ_22K = value; break;
                default: break;
            }
        }
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

	public class CfgNormalize
	{
		public bool Enable { get; set; } = false;

		/// <summary>フェードタイム（ms） Range: [1, 20000]</summary>
		public float FadeTime { get; set; } = 5000.0f;

		/// <summary>最大出力振幅 Range: [0, 10]</summary>
		public float MaxAmp { get; set; } = 1.0f;

		/// <summary>無音閾値 Range: [0, 1]</summary>
		public float Threshold { get; set; } = 0.0f;
	}

	public class Configuration : IConfigService
    {
        public ConfigurationData settings { get; set; }
        protected IPlayerEngine engine;

        // Backwards-compatible shim to expose existing functionality via IConfigService
        public IConfigService AsService() => new ConfigServiceAdapter(this);

        // Constructor no longer requires a `ref` parameter. Pass the engine instance by value.
        public Configuration(IPlayerEngine engine)
        {
            this.engine = engine;
            if (File.Exists(Path.Combine(Application.StartupPath, "config.json")))
            {
                string jsonString = File.ReadAllText(Path.Combine(Application.StartupPath, "config.json"), Encoding.UTF8);
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
                settings.Device = engine.GetDeviceGUID();
				// 新形式を優先、なければ旧形式
				var defaultXsk = Path.Combine(Application.StartupPath, "Skins", "Default", "Default.xsk");
                var defaultXsf = ".\\bbbs\\bs.xsf"; // 既存のデフォルト

                settings.Skin = File.Exists(defaultXsk) ? defaultXsk : defaultXsf;
            }
		}

		// Adapter to expose Configuration via IConfigService without breaking existing code
		internal class ConfigServiceAdapter : IConfigService
        {
            private readonly Configuration _cfg;
            public ConfigServiceAdapter(Configuration cfg) { _cfg = cfg; }
            public ConfigurationData settings => _cfg.settings;
            public void Save() => _cfg.Save();
            public FMOD.OUTPUTTYPE GetOutputType() => _cfg.GetOutputType();
        }

        public void Save()
        {
            GetEqualizerValue();

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(settings, options);

            File.WriteAllText(Path.Combine(Application.StartupPath, "config.json"), jsonString);
        }

        public FMOD.OUTPUTTYPE GetOutputType()
        {
            switch (settings.OutputType)
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

        public void GetEqualizerValue()
        {
            // engine or effector may be null during early startup; guard against that
            if (engine == null || engine.effector == null || engine.effector.GEqualizer == null)
                return;

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
