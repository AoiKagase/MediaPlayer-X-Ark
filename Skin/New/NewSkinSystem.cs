using MediaPlayer_X_Ark.Skin.New;
using MediaPlayer_X_Ark.Skin.New.Parts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Skin.New
{
	/// <summary>
	/// 新形式スキンシステム（JSON定義 + スプライトシート）
	/// </summary>
	public class NewSkinSystem : INewSkinSystem
	{
		// ===========================
		// JSON デシリアライズ用クラス
		// ===========================
		public class SkinMeta
		{
			[JsonPropertyName("name")] public string Name { get; set; }
			[JsonPropertyName("author")] public string Author { get; set; }
			[JsonPropertyName("description")] public string Description { get; set; }
		}

		public class SkinSettings
		{
			[JsonPropertyName("transparentKey")] public string TransparentKey { get; set; }
		}

		public class SkinJson
		{
			[JsonPropertyName("version")]
			public string Version { get; set; }

			[JsonPropertyName("meta")]
			public SkinMeta Meta { get; set; }

			[JsonPropertyName("settings")]
			public SkinSettings Settings { get; set; }

			[JsonPropertyName("images")]
			public Dictionary<string, string> Images { get; set; }

			[JsonPropertyName("MainForm")]
			public MainFormDef MainForm { get; set; }

			[JsonPropertyName("SubForms")]
			public Dictionary<string, SubFormDef> SubForms { get; set; }
		}

		// ===========================
		// ロード済みスキンデータ
		// ===========================
		/// <summary>
		/// MainForm
		/// </summary>
		public FormComponents MainForm { get; private set; }
        public Dictionary<string, SliderComponents> Sliders { get; private set; }
        public SpectrumComponents Spectrum { get; private set; }
        public WaveformComponents WaveForm { get; private set; }

        public Dictionary<string, FormComponents> SubForms { get; private set; }
        public Dictionary<string, Dictionary<string, ButtonComponents>> Buttons { get; private set; }
		public Dictionary<string, Dictionary<string, LabelComponents>> Labels { get; private set; }
		public Dictionary<string, Dictionary<string, GridComponents>> Grids { get; private set; }


        // ===========================
        // ロード済み画像キャッシュ
        // ===========================
        private Dictionary<string, Bitmap> _imageCache
			= new Dictionary<string, Bitmap>();

		private string _skinDir;

        // ===========================
        // Open
        // ===========================
        public void Open(string jsonPath)
		{
			_skinDir = Path.GetDirectoryName(jsonPath);
			// 古いキャッシュを破棄
			foreach (var bmp in _imageCache.Values)
				bmp?.Dispose();
			_imageCache.Clear();
			string json;
            SkinJson skin;
            try
			{
                json = File.ReadAllText(jsonPath, System.Text.Encoding.UTF8);
                skin = JsonSerializer.Deserialize<SkinJson>(json);
            } catch (Exception ex)
			{
				return;
			}

            // 画像をキャッシュにロード
            if (skin.Images != null)
			{
				foreach (var kv in skin.Images)
				{
					// / と \ 両方対応
					var relativePath = kv.Value.Replace('/', Path.DirectorySeparatorChar);
					var imgPath = Path.Combine(_skinDir, relativePath);
					if (File.Exists(imgPath))
					{
						// ファイルロックを避けるためメモリストリーム経由でロード
						using (var stream = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
						{
							_imageCache[kv.Key] = new Bitmap(stream);
						}
					}
				}
			}

			// MainForm
			var mf = skin.MainForm;
			MainForm = new FormComponents
			{
				BackImage = CropImage(mf.Src.ImageKey, mf.Src),
				TransparentKey = ParseColor(skin.Settings?.TransparentKey ?? "202030"),
				Position = new RECT
				{
					Width = mf.Location.W,
					Height = mf.Location.H,
				}
			};
            // MainForm: ボタン類
            Buttons = new Dictionary<string, Dictionary<string, ButtonComponents>>();
            foreach (var kv in skin.MainForm.Buttons ?? new Dictionary<string, PartsButtons>())
				Buttons["MainForm"][kv.Key] = LoadButton(skin.MainForm.Buttons, kv.Key);

			// MainForm: Sliders
			Sliders = new Dictionary<string, SliderComponents>();
			foreach (var kv in skin.MainForm.Sliders ?? new Dictionary<string, PartsSliders>())
				Sliders[kv.Key] = LoadSlider(skin.MainForm.Sliders, kv.Key);

			// MainForm: Labels
			Labels = new Dictionary<string, Dictionary<string, LabelComponents>>();
			foreach (var kv in skin.MainForm.Labels ?? new Dictionary<string, PartsTextArea>())
				Labels["MainForm"][kv.Key] = LoadText(skin.MainForm.Labels, kv.Key);

			// MainForm: Spectrum
			var sp = skin.MainForm.Spectrum;
            Spectrum = new SpectrumComponents
			{
				Image = CropImage(sp.Src.ImageKey, sp.Src),
				Color = ParseColor(sp.Color ?? "000000"),
				Position = new RECT
				{
					Left = sp.Location.X,
					Top = sp.Location.Y,
					Width = sp.Location.W,
					Height = sp.Location.H,
				},
				Enabled = true,
			};

			// WaveForm
			// TODO: テスト用にデフォルト値を入れているが、正式にはスキン側で定義必須。NULLの場合は非表示
			WaveForm = new WaveformComponents
			{
				Target = skin.MainForm.WaveArea?.Target ?? "trackbar",
				Mode = skin.MainForm.WaveArea?.Mode ?? "normal",
                Exponent = skin.MainForm.WaveArea?.Exponent ?? 1.0f,
                ColorL = ParseColor(skin.MainForm.WaveArea?.ColorL ?? "FF0000"),
                ColorR = ParseColor(skin.MainForm.WaveArea?.ColorR ?? "0000FF"),
                ColorMix = ParseColor(skin.MainForm.WaveArea?.ColorMix ?? "FF00FF"),
                ColorPlayed = ParseColor(skin.MainForm.WaveArea?.ColorPlayed ?? "00FF00"),
                ColorUnplayed = ParseColor(skin.MainForm.WaveArea?.ColorUnplayed ?? "202020"),
                // target="area" の場合のみ使用
                Location = new Location
				{
					X = skin.MainForm.WaveArea?.Location.X ?? 0,
					Y = skin.MainForm.WaveArea?.Location.Y ?? 0,
					W = skin.MainForm.WaveArea?.Location.W ?? 0,
					H = skin.MainForm.WaveArea?.Location.H ?? 0,
                }
            };

            // SubForms
			foreach (var kv in skin.SubForms ?? new Dictionary<string, SubFormDef>())
			{
				// Form
				SubForms = new Dictionary<string, FormComponents>
				{
					[kv.Key] = new FormComponents
					{
						BackImage = CropImage(kv.Value.Src.ImageKey, kv.Value.Src),
						TransparentKey = ParseColor(skin.Settings?.TransparentKey ?? "202030"),
						Position = new RECT
						{
							Left = kv.Value.Offset.X,
							Top = kv.Value.Offset.Y,
							Width = kv.Value.Src.W,
							Height = kv.Value.Src.H,
						},
						MagnetMode = kv.Value.Magnetic,
                    }
                };

                // Buttons
                foreach (var btnKv in kv.Value.Buttons ?? new Dictionary<string, PartsButtons>())
				{
					if (!Buttons.ContainsKey(kv.Key))
						Buttons[kv.Key] = new Dictionary<string, ButtonComponents>();
					Buttons[kv.Key][btnKv.Key] = LoadButton(kv.Value.Buttons, btnKv.Key);
                }

				// Labels
				foreach (var labelKv in kv.Value.Labels ?? new Dictionary<string, PartsTextArea>())
				{
					if (!Labels.ContainsKey(kv.Key))
						Labels[kv.Key] = new Dictionary<string, LabelComponents>();
					Labels[kv.Key][labelKv.Key] = LoadText(kv.Value.Labels, labelKv.Key);
                }

				// Grids
				foreach (var gridKv in kv.Value.Grids ?? new Dictionary<string, PartsGrids>())
				{
					if (!Grids.ContainsKey(kv.Key))
						Grids[kv.Key] = new Dictionary<string, GridComponents>();
					Grids[kv.Key][gridKv.Key] = new GridComponents()
					{
						ListBackColor = ParseColor(gridKv.Value.BackColor ?? "000001"),
						ListForeColor = ParseColor(gridKv.Value.ForeColor ?? "FFFFFF"),
						ListPosition = new RECT
						{
							Left = gridKv.Value.Location.X,
							Top = gridKv.Value.Location.Y,
							Width = gridKv.Value.Location.W,
							Height = gridKv.Value.Location.H,
                        }
                    };
                }
            }
		}

		// ===========================
		// ヘルパー
		// ===========================

		/// <summary>スプライトシートから指定矩形を切り出す</summary>
		private Image CropImage(string imageKey, SpriteRect rect)
		{
			if (imageKey == null || rect == null) return null;
			if (!_imageCache.TryGetValue(imageKey, out var src)) return null;

			// rect が (0,0,0,0) の場合は画像全体を返す
			if (rect.W == 0 && rect.H == 0)
				return new Bitmap(src);

			var bmp = new Bitmap(rect.W, rect.H);
			using (var g = Graphics.FromImage(bmp))
				g.DrawImage(src,
					new Rectangle(0, 0, rect.W, rect.H),
					rect.ToRectangle(),
					GraphicsUnit.Pixel);
			return bmp;
		}

		private ButtonComponents LoadButton(Dictionary<string, PartsButtons> buttons, string key)
		{
			var result = new ButtonComponents { Enabled = false };

			if (buttons == null || !buttons.TryGetValue(key, out var def))
				return result;

			result.BackImage = CropImage(def.Up.ImageKey, def.Up);

			// downImage キーが指定されていれば別画像から、なければ同画像からクロップ
			result.DownImage = def.Down.ImageKey != null
				? CropImage(def.Down.ImageKey, new SpriteRect())
				: CropImage(def.Down.ImageKey, def.Down);

			result.Enabled = !def.IsDisabled && result.BackImage != null;

			// optional
			if (def.Optional.ImageKey != null)
			{
				result.OptionalImage = CropImage(def.Optional.ImageKey, new SpriteRect());
				result.Toggle = result.OptionalImage != null;
			}
			else if (def.Optional != null)
			{
				result.OptionalImage = CropImage(def.Optional.ImageKey, def.Optional);
				result.Toggle = result.OptionalImage != null;
			}

			result.Position = new RECT
			{
				Left = def.Location.X,
				Top = def.Location.Y,
				Width = result.BackImage?.Width ?? 0,
				Height = result.BackImage?.Height ?? 0,
			};

			return result;
		}

		private SliderComponents LoadSlider(Dictionary<string, PartsSliders> sliders, string key)
		{
			var result = new SliderComponents { Enabled = false };
			if (sliders == null || !sliders.TryGetValue(key, out var def))
				return result;

			result.SliderImage = CropImage(def.Src.ImageKey, def.Src);
			if (result.SliderImage == null) return result;

			result.Orientation = def.Orientation?.ToLower() == "vertical"
				? Orientation.Vertical : Orientation.Horizontal;
			result.Minimum = def.Min;
			result.Maximum = def.Max;
			result.Position = new RECT
			{
				Left = def.Location.X,
				Top = def.Location.Y,
				Width = result.Orientation == Orientation.Horizontal
					? def.Location.W - def.Location.X + result.SliderImage.Width
					: result.SliderImage.Width,
				Height = result.Orientation == Orientation.Vertical
					? def.Location.H - def.Location.Y + result.SliderImage.Height
					: result.SliderImage.Height,
			};
			result.Enabled = true;
			return result;
		}

		private LabelComponents LoadText(Dictionary<string, PartsTextArea> texts, string key)
		{
			var result = new LabelComponents { Enabled = false };
			if (texts == null || !texts.TryGetValue(key, out var def))
				return result;

			var style = FontStyle.Regular;
			if (def.Bold) style |= FontStyle.Bold;
			if (def.Italic) style |= FontStyle.Italic;

			result.Font = new Font(def.Font ?? "Yu Gothic UI",
								def.Size > 0 ? def.Size : 9, style,
								GraphicsUnit.Point);
			result.FontColor = ParseColor(def.ForeColor ?? "FFFFFF");
			result.Position = new RECT
			{
				Left = def.Location.X,
				Top = def.Location.Y,
				Width = def.Location.W,
				Height = def.Location.H,
			};
			result.Interval = def.Interval;
			result.ScrollEnable = def.ScrollEnable;
			result.Enabled = true;
			return result;
		}

		private Color ParseColor(string hex)
		{
			if (string.IsNullOrEmpty(hex)) return Color.Black;
			hex = hex.TrimStart('#');
			if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber,
				null, out int val))
				return ColorTranslator.FromWin32(val);
			return Color.Black;
		}
	}
}