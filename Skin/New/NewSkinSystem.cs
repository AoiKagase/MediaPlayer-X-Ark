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

			[JsonPropertyName("mainForm")]
			public MainFormDef MainForm { get; set; }

			[JsonPropertyName("subForms")]
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
        public Dictionary<string, Dictionary<string, SliderComponents>> FormSliders { get; private set; }
        public SpectrumComponents Spectrum { get; private set; }
        public WaveformComponents WaveForm { get; private set; }

        public Dictionary<string, FormComponents> SubForms { get; private set; }
        public Dictionary<string, Dictionary<string, ButtonComponents>> Buttons { get; private set; }
		public Dictionary<string, Dictionary<string, LabelComponents>> Labels { get; private set; }
		public Dictionary<string, Dictionary<string, GridComponents>> Grids { get; private set; }
		public Dictionary<string, Dictionary<string, PictureComponents>> Pictures { get; private set; }

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
			foreach (var bmp in _imageCache.Values)
				bmp?.Dispose();
			_imageCache.Clear();
			string json;
            SkinJson skin;
            try
			{
                json = File.ReadAllText(jsonPath, System.Text.Encoding.UTF8);
                skin = JsonSerializer.Deserialize<SkinJson>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            } catch (Exception ex)
			{
				MessageBox.Show($"スキンの読み込みに失敗しました。\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
				return;
			}

            if (skin.Images != null)
			{
				foreach (var kv in skin.Images)
				{
					var relativePath = kv.Value.Replace('/', Path.DirectorySeparatorChar);
					var imgPath = Path.Combine(_skinDir, relativePath);
					if (File.Exists(imgPath))
					{
						// ファイルロック回避のためメモリストリーム経由でロードする
						using (var stream = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
						{
							_imageCache[kv.Key] = new Bitmap(stream);
						}
					}
				}
			}

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
            Buttons = new Dictionary<string, Dictionary<string, ButtonComponents>>();
            Buttons["MainForm"] = new Dictionary<string, ButtonComponents>();
            foreach (var kv in skin.MainForm.Buttons ?? new Dictionary<string, PartsButtons>())
				Buttons["MainForm"][kv.Key] = LoadButton(skin.MainForm.Buttons, kv.Key);

			Sliders = new Dictionary<string, SliderComponents>();
			FormSliders = new Dictionary<string, Dictionary<string, SliderComponents>>();
			foreach (var kv in skin.MainForm.Sliders ?? new Dictionary<string, PartsSliders>())
				Sliders[kv.Key] = LoadSlider(skin.MainForm.Sliders, kv.Key);
			FormSliders["MainForm"] = Sliders;

			Labels = new Dictionary<string, Dictionary<string, LabelComponents>>();
            Labels["MainForm"] = new Dictionary<string, LabelComponents>();
            foreach (var kv in skin.MainForm.Labels ?? new Dictionary<string, PartsTextArea>())
				Labels["MainForm"][kv.Key] = LoadText(skin.MainForm.Labels, kv.Key);

			Pictures = new Dictionary<string, Dictionary<string, PictureComponents>>();
			Pictures["MainForm"] = new Dictionary<string, PictureComponents>();
			foreach (var kv in skin.MainForm.Pictures ?? new Dictionary<string, PartsPictureArea>())
				Pictures["MainForm"][kv.Key] = LoadPictures(skin.MainForm.Pictures, kv.Key);

			var sp = mf.Spectrum;
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
				WaveColorL = sp.WaveColorL != null ? ParseColor(sp.WaveColorL) : Color.Empty,
				WaveColorR = sp.WaveColorR != null ? ParseColor(sp.WaveColorR) : Color.Empty,
			};

			// スキン側で未定義の場合はデフォルト値にフォールバックする
			WaveForm = new WaveformComponents
			{
				Target = mf.WaveArea?.Target ?? "trackbar",
				Mode = mf.WaveArea?.Mode ?? "normal",
                Exponent = mf.WaveArea?.Exponent ?? 1.0f,
                ColorL = ParseColor(mf.WaveArea?.ColorL ?? "FF0000"),
                ColorR = ParseColor(mf.WaveArea?.ColorR ?? "0000FF"),
                ColorMix = ParseColor(mf.WaveArea?.ColorMix ?? "FF00FF"),
                ColorPlayed = ParseColor(mf.WaveArea?.ColorPlayed ?? "00FF00"),
                ColorUnplayed = ParseColor(mf.WaveArea?.ColorUnplayed ?? "202020"),
                Location = new Location
				{
					X = mf.WaveArea?.Location?.X ?? 0,
					Y = mf.WaveArea?.Location?.Y ?? 0,
					W = mf.WaveArea?.Location?.W ?? 0,
					H = mf.WaveArea?.Location?.H ?? 0,
                }
            };

            SubForms = new Dictionary<string, FormComponents>();
            Grids = new Dictionary<string, Dictionary<string, GridComponents>>();
            foreach (var kv in skin.SubForms ?? new Dictionary<string, SubFormDef>())
			{
                SubForms[kv.Key] = new FormComponents
				{
					BackImage = CropImage(kv.Value.Src?.ImageKey, kv.Value.Src),
                    BackColor = ParseColorOrEmpty(kv.Value.BackColor), 
                    ForeColor = ParseColorOrEmpty(kv.Value.ForeColor), 
                    Font = kv.Value.Font != null             
				        ? new Font(kv.Value.Font,
		               kv.Value.FontSize > 0 ? kv.Value.FontSize : 9,
			           FontStyle.Regular, GraphicsUnit.Point)
						: null,
                    TransparentKey = ParseColor(skin.Settings?.TransparentKey ?? "202030"),
					Position = new RECT
					{
						Left = kv.Value.Offset?.X ?? 0,
						Top = kv.Value.Offset?.Y ?? 0,
                        Width = kv.Value.Location?.W ?? 0,
                        Height = kv.Value.Location?.H ?? 0,
                    },
					MagnetMode = kv.Value.Magnetic,
                };

                foreach (var btnKv in kv.Value.Buttons ?? new Dictionary<string, PartsButtons>())
				{
					if (!Buttons.ContainsKey(kv.Key))
						Buttons[kv.Key] = new Dictionary<string, ButtonComponents>();
					Buttons[kv.Key][btnKv.Key] = LoadButton(kv.Value.Buttons, btnKv.Key);
                }

				foreach (var labelKv in kv.Value.Labels ?? new Dictionary<string, PartsTextArea>())
				{
					if (!Labels.ContainsKey(kv.Key))
						Labels[kv.Key] = new Dictionary<string, LabelComponents>();
					Labels[kv.Key][labelKv.Key] = LoadText(kv.Value.Labels, labelKv.Key);
                }

				foreach (var sliderKv in kv.Value.Sliders ?? new Dictionary<string, PartsSliders>())
				{
					if (!FormSliders.ContainsKey(kv.Key))
						FormSliders[kv.Key] = new Dictionary<string, SliderComponents>();
					FormSliders[kv.Key][sliderKv.Key] = LoadSlider(kv.Value.Sliders, sliderKv.Key);
				}

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

				foreach (var picKv in kv.Value.Pictures ?? new Dictionary<string, PartsPictureArea>())
				{
					if (!Pictures.ContainsKey(kv.Key))
						Pictures[kv.Key] = new Dictionary<string, PictureComponents>();
					Pictures[kv.Key][picKv.Key] = LoadPictures(kv.Value.Pictures, picKv.Key);
				}
			}
		}

		// ===========================
		// ヘルパー
		// ===========================

		/// <summary>スプライトシートから指定矩形を切り出す</summary>
		private Image CropImage(string imageKey = null, SpriteRect rect = null)
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

			result.Enabled = !def.IsDisabled;
			if (def.IsDisabled)
				return result;

			result.BackImage = CropImage(def.Up?.ImageKey, def.Up);

            // downImage キーが指定されていれば別画像から、なければ同画像からクロップ
            result.DownImage = CropImage(def.Down?.ImageKey, def.Down);

			result.Enabled = !def.IsDisabled && result.BackImage != null;

            if (def.Optional != null)
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

			result.Enabled = !def.IsDisabled;
			if (def.IsDisabled)
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
                Width = def.Location.W,
                Height = def.Location.H,
            };
			result.Enabled = true;
			return result;
		}

		private LabelComponents LoadText(Dictionary<string, PartsTextArea> texts, string key)
		{
			var result = new LabelComponents { Enabled = false };
			if (texts == null || !texts.TryGetValue(key, out var def))
				return result;

			result.Enabled = !def.IsDisabled;
			if (def.IsDisabled)
				return result;

			var style = FontStyle.Regular;
			if (def.Bold) style |= FontStyle.Bold;
			if (def.Italic) style |= FontStyle.Italic;

			result.Font = new Font(def.Font ?? "Yu Gothic UI",
								def.Size > 0 ? def.Size : 9, style,
								GraphicsUnit.Point);
			result.FontColor = ParseColor(def.ForeColor ?? "FFFFFF");
			result.HorizontalAlign = ParseHorizontalAlignment(def.Align);
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

		private static HorizontalAlignment ParseHorizontalAlignment(string align)
		{
			return align?.Trim().ToLowerInvariant() switch
			{
				"center" => HorizontalAlignment.Center,
				"right" => HorizontalAlignment.Right,
				_ => HorizontalAlignment.Left,
			};
		}
		private PictureComponents LoadPictures(Dictionary<string, PartsPictureArea> picts, string key)
		{
			var result = new PictureComponents { Enabled = false };
			if (picts == null || !picts.TryGetValue(key, out var def))
				return result;

			result.Enabled = !def.IsDisabled;
			if (def.IsDisabled)
				return result;

			if (def.Src != null)
			{
				result.Image = CropImage(def.Src.ImageKey, def.Src);
				if (result.Image == null) return result;
			}
			result.BorderColor = ParseColor(def.BorderColor ?? "000000");
			result.BorderWidth = def.BorderWidth;

			result.Position = new RECT
			{
				Left = def.Location.X,
				Top = def.Location.Y,
				Width = def.Location.W,
				Height = def.Location.H,
			};

			result.Enabled = !def.IsDisabled;
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
        private Color ParseColorOrEmpty(string hex)
        {
            if (string.IsNullOrEmpty(hex)) return Color.Empty;
            return ParseColor(hex);
        }
    }
}
