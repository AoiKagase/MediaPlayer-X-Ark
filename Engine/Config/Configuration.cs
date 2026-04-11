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
        /// <summary>パン（-10〜+10）</summary>
        public int Pan { get; set; }
        /// <summary>音量（0〜150）</summary>
        public int Volume { get; set; } = 100;

		public bool RestorePlaylist { get; set; } = false;
		public bool RestorePosition { get; set; } = false;
		public bool AutoSavePlaylist { get; set; } = false;
		public bool AlwaysOnTop { get; set; } = false;
		public int OpenFileAction { get; set; } = 0;
		public int DefaultSpectrumMode { get; set; } = 0;
		public bool SnowBlockEnabled { get; set; } = false;
		/// <summary>出力形式（0=AUTODETECT, 1=WASAPI, 2=ASIO, 3=WinSonic）</summary>
		public int OutputType { get; set; }

        /// <summary>デバイス GUID</summary>
        public string Device { get; set; } = "";

		public string SoundFontPath { get; set; } = "";
		public MidiRendererBackend MidiRendererBackend { get; set; } = MidiRendererBackend.XArkMidi;
		public CfgEffectors Effectors { get; set; } = new CfgEffectors();
        public Dictionary<string, string> EffectPresets { get; set; } = new Dictionary<string, string>();
        /// <summary>スキンファイルパス（.xsk）</summary>
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

		/// <summary>true のとき MusicBrainz を優先し gnudb をフォールバックにする</summary>
		public bool CddbPreferMusicBrainz { get; set; } = false;

		/// <summary>クロスフェードの有効/無効</summary>
		public bool CrossfadeEnabled { get; set; } = false;

		/// <summary>クロスフェード時間（ミリ秒）デフォルト3秒</summary>
		public int CrossfadeDurationMs { get; set; } = 3000;
        /// <summary>ギャップレス再生の有効/無効（クロスフェードと排他）</summary>
        public bool NonStopMixEnabled { get; set; } = false;
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

		/// <summary>起動時に自動で更新チェックを行うか</summary>
		public bool AutoUpdateCheckEnabled { get; set; } = true;
		/// <summary>GitHub リポジトリ（owner/repo 形式）</summary>
		public string UpdateGitHubRepo { get; set; } = "AoiKagase/MediaPlayer-X-Ark";

		// ── スペクトラム表示設定 ────────────────────────────────────────
		/// <summary>スペクトラム更新間隔（ms）。Timer.Interval（60ms固定）とは独立したカウンターで制御。</summary>
		public int SpectrumUpdateIntervalMs { get; set; } = 60;

		/// <summary>ウェーブラインL色（RRGGBB形式）</summary>
		public string WaveColorL { get; set; } = "00FF00";
		/// <summary>ウェーブラインR色（RRGGBB形式）</summary>
		public string WaveColorR { get; set; } = "00FFFF";
		/// <summary>true=設定色を使用、false=スキン定義色（未定義時はデフォルト）</summary>
		public bool UseCustomWaveColor { get; set; } = false;

		/// <summary>スペクトラムバー色（RRGGBB形式）</summary>
		public string SpectrumBarColor { get; set; } = "FFFFFF";
		/// <summary>true=設定色で単色塗り、false=スキン画像を使用</summary>
		public bool UseCustomSpectrumBarColor { get; set; } = false;

		/// <summary>スノーブロック落下速度（px/秒）。フレームレート非依存。デフォルト12 = 0.72px/frame @ 60ms</summary>
		public float SnowFallSpeedPxPerSec { get; set; } = 12f;

		// ── ウェーブフォーム表示色 ────────────────────────────────────────
		/// <summary>ウェーブフォームL色（RRGGBB形式）</summary>
		public string WaveformColorL { get; set; } = "00C864";
		/// <summary>ウェーブフォームR色（RRGGBB形式）</summary>
		public string WaveformColorR { get; set; } = "0064C8";
		/// <summary>ウェーブフォームMix色（RRGGBB形式）</summary>
		public string WaveformColorMix { get; set; } = "00B478";
		/// <summary>再生済み部分の色（RRGGBB形式）</summary>
		public string WaveformColorPlayed { get; set; } = "646464";
		/// <summary>未再生部分の色（RRGGBB形式）</summary>
		public string WaveformColorUnplayed { get; set; } = "323232";
		/// <summary>true=設定色を使用、false=スキン定義色</summary>
		public bool UseCustomWaveformColors { get; set; } = false;

		// ── テキスト表示設定 ──────────────────────────────────────────
		/// <summary>タイトルラベルのフォント名</summary>
		public string TitleFontName { get; set; } = "";
		/// <summary>タイトルラベルのフォントサイズ（pt）</summary>
		public float TitleFontSize { get; set; } = 0f;
		/// <summary>タイトルラベルフォントを太字にするか</summary>
		public bool TitleFontBold { get; set; } = false;
		/// <summary>true=設定フォントを使用、false=スキン定義フォント</summary>
		public bool UseCustomTitleFont { get; set; } = false;

		/// <summary>時間表示ラベルのフォント名</summary>
		public string TimeFontName { get; set; } = "";
		/// <summary>時間表示ラベルのフォントサイズ（pt）</summary>
		public float TimeFontSize { get; set; } = 0f;
		/// <summary>時間表示ラベルフォントを太字にするか</summary>
		public bool TimeFontBold { get; set; } = false;
		/// <summary>true=設定フォントを使用、false=スキン定義フォント</summary>
		public bool UseCustomTimeFont { get; set; } = false;

		/// <summary>タイトルスクロール間隔（ms/tick）。0=スキン定義値を使用。</summary>
		public int TitleScrollIntervalMs { get; set; } = 100;
		/// <summary>true=設定値を使用、false=スキン定義値</summary>
		public bool UseCustomTitleScrollInterval { get; set; } = false;
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

        /// <summary>バンドインデックスでゲイン値を設定する（UI側のswitch重複を避けるためのヘルパー）</summary>
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
		private const int DefaultVolume = 100;
		private const int MinVolume = 0;
		private const int MaxVolume = 150;
		private const int MinPan = -10;
		private const int MaxPan = 10;

        public ConfigurationData settings { get; set; }
        protected IPlayerEngine engine;

        public IConfigService AsService() => new ConfigServiceAdapter(this);

        public Configuration(IPlayerEngine engine)
        {
            this.engine = engine;
            if (File.Exists(Path.Combine(Application.StartupPath, "config.json")))
            {
                string jsonString = File.ReadAllText(Path.Combine(Application.StartupPath, "config.json"), Encoding.UTF8);
                settings = JsonSerializer.Deserialize<ConfigurationData>(jsonString) ?? new ConfigurationData();
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
				var defaultXsk = Path.Combine(Application.StartupPath, "Skins", "Default", "Default.xsk");
                var defaultXsf = ".\\bbbs\\bs.xsf";

                settings.Skin = File.Exists(defaultXsk) ? defaultXsk : defaultXsf;
            }

			NormalizeSettings();
		}

		private void NormalizeSettings()
		{
			settings ??= new ConfigurationData();
			settings.Volume = Math.Clamp(settings.Volume, MinVolume, MaxVolume);
			settings.Pan = Math.Clamp(settings.Pan, MinPan, MaxPan);

			if (settings.Volume == 0 && !File.Exists(Path.Combine(Application.StartupPath, "config.json")))
				settings.Volume = DefaultVolume;
		}

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
            // 起動直後はエンジンやエフェクターが null の場合があるため保護する
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
