using MediaPlayer_X_Ark.Skin.NewSkinSystem.NewSkinSystem;
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
			public SubFormDef SubForms { get; set; }
		}

		// ===========================
		// ロード済みスキンデータ
		// ===========================
		public FormComponents MainForm { get; private set; }
		public SpectrumComponents ImgSpectrum { get; private set; }
		// 追加（辞書プロパティ）
		public Dictionary<string, ButtonComponents> Buttons { get; private set; }
		public Dictionary<string, SliderComponents> Sliders { get; private set; }
		public Dictionary<string, LabelComponents> Labels { get; private set; }

		private static readonly Dictionary<string, ButtonComponents> _emptyButtons = new Dictionary<string, ButtonComponents>();

		private Dictionary<string, FormComponents> _forms;
		private Dictionary<string, PListGrid> _grids;
		private Dictionary<string, Dictionary<string, ButtonComponents>> _formButtons;

		public Dictionary<string, FormComponents> Forms => _forms;
		public Dictionary<string, PListGrid> Grids => _grids;
		public Dictionary<string, Dictionary<string, ButtonComponents>> FormButtons => _formButtons;
		public FormComponents this[string formName] =>
			_forms.TryGetValue(formName, out var f) ? f : null;

		public Dictionary<string, ButtonComponents> GetFormButtons(string formName) =>
			_formButtons.TryGetValue(formName, out var b) ? b : _emptyButtons;

		// ===========================
		// インデクサ（OldSkinSystemとの互換）
		// ===========================
		//public object this[string propertyName]
		//{
		//	get { return typeof(NewSkinSystem).GetProperty(propertyName).GetValue(this); }
		//}

		// ===========================
		// ロード済み画像キャッシュ
		// ===========================
		private Dictionary<string, Bitmap> _imageCache
			= new Dictionary<string, Bitmap>();

		private string _skinDir;
		public WaveformDef Waveform { get; private set; } // null = 未定義（スキン非対応）
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

			var json = File.ReadAllText(jsonPath, System.Text.Encoding.UTF8);
			var skin = JsonSerializer.Deserialize<SkinJson>(json);

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
				BackImage = CropImage(mf.Image, mf.Src),
				TransparentKey = ParseColor(skin.Settings?.TransparentKey ?? "202030"),
				Position = new RECT
				{
					Width = mf.Width,
					Height = mf.Height,
				}
			};

			// Buttons
			Buttons = new Dictionary<string, ButtonComponents>();
			foreach (var kv in skin.Buttons ?? new Dictionary<string, ButtonDef>())
				Buttons[kv.Key] = LoadButton(skin.Buttons, kv.Key);

			// Sliders
			Sliders = new Dictionary<string, SliderComponents>();
			foreach (var kv in skin.Sliders ?? new Dictionary<string, SliderDef>())
				Sliders[kv.Key] = LoadSlider(skin.Sliders, kv.Key);

			// Labels
			Labels = new Dictionary<string, LabelComponents>();
			foreach (var kv in skin.Text ?? new Dictionary<string, TextDef>())
				Labels[kv.Key] = LoadText(skin.Text, kv.Key);


			// Spectrum
			var sp = skin.Spectrum;
			ImgSpectrum = new SpectrumComponents
			{
				Image = CropImage(sp.Image, sp.Src),
				Color = ParseColor(sp.Color ?? "000000"),
				Position = new RECT
				{
					Left = sp.X,
					Top = sp.Y,
					Width = sp.Width,
					Height = sp.Height,
				},
				Enabled = true,
			};
			// WaveForm
			// TODO: テスト用にデフォルト値を入れているが、正式にはスキン側で定義必須。NULLの場合は非表示
			Waveform = skin.Waveform ?? new WaveformDef();
			// PlayListForm
			var pl = skin.PlayList;
			_forms = new Dictionary<string, FormComponents>
			{
				["PlayListForm"] = new FormComponents
				{
					BackImage = CropImage(pl.Image, pl.Src),
					TransparentKey = ParseColor(skin.Settings?.TransparentKey ?? "202030"),
					Position = new RECT
					{
						Left = pl.OffsetX,
						Top = pl.OffsetY,
						Width = pl.Width,
						Height = pl.Height,
					},
					MagnetMode = pl.MagnetMode,
				}
			};
			_grids = new Dictionary<string, PListGrid>
			{
				["PlayListGrid"] = new PListGrid
				{
					ListBackColor = ParseColor(pl.ListBackColor ?? "000001"),
					ListForeColor = ParseColor(pl.ListForeColor ?? "FFFFFF"),
					ListPosition = new RECT
					{
						Left = pl.ListX,
						Top = pl.ListY,
						Width = pl.ListWidth,
						Height = pl.ListHeight,
					},
				}
			};

			// FormButtons
			_formButtons = new Dictionary<string, Dictionary<string, ButtonComponents>>();
			var plButtons = new Dictionary<string, ButtonComponents>();
			foreach (var kv in pl.Buttons ?? new Dictionary<string, ButtonDef>())
				plButtons[kv.Key] = LoadButton(pl.Buttons, kv.Key);
			_formButtons["PlayListForm"] = plButtons;
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

		private ButtonComponents LoadButton(
			Dictionary<string, ButtonDef> buttons, string key)
		{
			var result = new ButtonComponents { Enabled = false };

			if (buttons == null || !buttons.TryGetValue(key, out var def))
				return result;

			result.BackImage = CropImage(def.Image, def.Normal);

			// downImage キーが指定されていれば別画像から、なければ同画像からクロップ
			result.DownImage = def.DownImage != null
				? CropImage(def.DownImage, new SpriteRect())
				: CropImage(def.Image, def.Down);

			result.Enabled = def.Enabled && result.BackImage != null;

			// optional
			if (def.OptionalImage != null)
			{
				result.OptionalImage = CropImage(def.OptionalImage, new SpriteRect());
				result.Toggle = result.OptionalImage != null;
			}
			else if (def.Optional != null)
			{
				result.OptionalImage = CropImage(def.Image, def.Optional);
				result.Toggle = result.OptionalImage != null;
			}

			result.Position = new RECT
			{
				Left = def.X,
				Top = def.Y,
				Width = result.BackImage?.Width ?? 0,
				Height = result.BackImage?.Height ?? 0,
			};

			return result;
		}

		private SliderComponents LoadSlider(
			Dictionary<string, SliderDef> sliders, string key)
		{
			var result = new SliderComponents { Enabled = false };
			if (sliders == null || !sliders.TryGetValue(key, out var def))
				return result;

			result.SliderImage = CropImage(def.Image, def.Src);
			if (result.SliderImage == null) return result;

			result.Orientation = def.Orientation?.ToLower() == "vertical"
				? Orientation.Vertical : Orientation.Horizontal;
			result.Minimum = def.Min;
			result.Maximum = def.Max;
			result.Position = new RECT
			{
				Left = def.X,
				Top = def.Y,
				Width = result.Orientation == Orientation.Horizontal
					? def.AreaX2 - def.X + result.SliderImage.Width
					: result.SliderImage.Width,
				Height = result.Orientation == Orientation.Vertical
					? def.AreaY2 - def.Y + result.SliderImage.Height
					: result.SliderImage.Height,
			};
			result.Enabled = true;
			return result;
		}

		private LabelComponents LoadText(
			Dictionary<string, TextDef> texts, string key)
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
			result.FontColor = ParseColor(def.Color ?? "FFFFFF");
			result.Position = new RECT
			{
				Left = def.X,
				Top = def.Y,
				Width = def.Width,
				Height = def.Height,
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