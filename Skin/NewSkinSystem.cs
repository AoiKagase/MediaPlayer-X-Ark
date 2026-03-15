using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Skin
{
	/// <summary>
	/// 新形式スキンシステム（JSON定義 + スプライトシート）
	/// </summary>
	public class NewSkinSystem : ISkinSystem
	{
		// ===========================
		// JSON デシリアライズ用クラス
		// ===========================
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

			[JsonPropertyName("Buttons")]
			public Dictionary<string, ButtonDef> Buttons { get; set; }

			[JsonPropertyName("Sliders")]
			public Dictionary<string, SliderDef> Sliders { get; set; }

			[JsonPropertyName("Spectrum")]
			public SpectrumDef Spectrum { get; set; }

			[JsonPropertyName("Text")]
			public Dictionary<string, TextDef> Text { get; set; }

			[JsonPropertyName("PlayList")]
			public PlayListDef PlayList { get; set; }
		}

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

		public class SpriteRect
		{
			[JsonPropertyName("x")] public int X { get; set; }
			[JsonPropertyName("y")] public int Y { get; set; }
			[JsonPropertyName("w")] public int W { get; set; }
			[JsonPropertyName("h")] public int H { get; set; }
			public Rectangle ToRectangle() => new Rectangle(X, Y, W, H);
		}

		public class MainFormDef
		{
			[JsonPropertyName("image")] public string Image { get; set; }
			[JsonPropertyName("src")] public SpriteRect Src { get; set; }
			[JsonPropertyName("width")] public int Width { get; set; }
			[JsonPropertyName("height")] public int Height { get; set; }
		}

		public class ButtonDef
		{
			[JsonPropertyName("image")] public string Image { get; set; }
			[JsonPropertyName("normal")] public SpriteRect Normal { get; set; }
			[JsonPropertyName("down")] public SpriteRect Down { get; set; }
			[JsonPropertyName("optional")] public SpriteRect Optional { get; set; }
			[JsonPropertyName("downImage")] public string DownImage { get; set; }     // 追加
			[JsonPropertyName("optionalImage")] public string OptionalImage { get; set; } // 追加
			[JsonPropertyName("x")] public int X { get; set; }
			[JsonPropertyName("y")] public int Y { get; set; }
			[JsonPropertyName("enabled")] public bool Enabled { get; set; }
		}

		public class SliderDef
		{
			[JsonPropertyName("image")] public string Image { get; set; }
			[JsonPropertyName("src")] public SpriteRect Src { get; set; }
			[JsonPropertyName("orientation")] public string Orientation { get; set; }
			[JsonPropertyName("x")] public int X { get; set; }
			[JsonPropertyName("y")] public int Y { get; set; }
			[JsonPropertyName("areaX2")] public int AreaX2 { get; set; }
			[JsonPropertyName("areaY2")] public int AreaY2 { get; set; }
			[JsonPropertyName("min")] public int Min { get; set; }
			[JsonPropertyName("max")] public int Max { get; set; }
		}

		public class SpectrumDef
		{
			[JsonPropertyName("image")] public string Image { get; set; }
			[JsonPropertyName("src")] public SpriteRect Src { get; set; }
			[JsonPropertyName("x")] public int X { get; set; }
			[JsonPropertyName("y")] public int Y { get; set; }
			[JsonPropertyName("width")] public int Width { get; set; }
			[JsonPropertyName("height")] public int Height { get; set; }
			[JsonPropertyName("color")] public string Color { get; set; }
		}

		public class TextDef
		{
			[JsonPropertyName("x")] public int X { get; set; }
			[JsonPropertyName("y")] public int Y { get; set; }
			[JsonPropertyName("width")] public int Width { get; set; }
			[JsonPropertyName("height")] public int Height { get; set; }
			[JsonPropertyName("font")] public string Font { get; set; }
			[JsonPropertyName("size")] public int Size { get; set; }
			[JsonPropertyName("bold")] public bool Bold { get; set; }
			[JsonPropertyName("italic")] public bool Italic { get; set; }
			[JsonPropertyName("color")] public string Color { get; set; }
			[JsonPropertyName("interval")] public int Interval { get; set; }
			[JsonPropertyName("scrollEnable")] public bool ScrollEnable { get; set; }
		}

		public class PlayListDef
		{
			[JsonPropertyName("image")] public string Image { get; set; }
			[JsonPropertyName("src")] public SpriteRect Src { get; set; }
			[JsonPropertyName("width")] public int Width { get; set; }
			[JsonPropertyName("height")] public int Height { get; set; }
			[JsonPropertyName("offsetX")] public int OffsetX { get; set; }
			[JsonPropertyName("offsetY")] public int OffsetY { get; set; }
			[JsonPropertyName("listX")] public int ListX { get; set; }
			[JsonPropertyName("listY")] public int ListY { get; set; }
			[JsonPropertyName("listWidth")] public int ListWidth { get; set; }
			[JsonPropertyName("listHeight")] public int ListHeight { get; set; }
			[JsonPropertyName("listBackColor")] public string ListBackColor { get; set; }
			[JsonPropertyName("listForeColor")] public string ListForeColor { get; set; }
			[JsonPropertyName("magnetMode")] public bool MagnetMode { get; set; }
			[JsonPropertyName("Buttons")] public Dictionary<string, ButtonDef> Buttons { get; set; }
		}

		// ===========================
		// ロード済みスキンデータ
		// ===========================
		public FormComponents MainForm { get; private set; }
		public SpectrumComponents ImgSpectrum { get; private set; }
		public ButtonComponents BtnOpen { get; private set; }
		public ButtonComponents BtnClose { get; private set; }
		public ButtonComponents BtnPlay { get; private set; }
		public ButtonComponents BtnStop { get; private set; }
		public ButtonComponents BtnBack { get; private set; }
		public ButtonComponents BtnSeekBack { get; private set; }
		public ButtonComponents BtnPause { get; private set; }
		public ButtonComponents BtnSeekForward { get; private set; }
		public ButtonComponents BtnNext { get; private set; }
		public ButtonComponents BtnRandom { get; private set; }
		public ButtonComponents BtnLoop { get; private set; }
		public ButtonComponents BtnSetting { get; private set; }
		public ButtonComponents BtnPlaylist { get; private set; }
		public ButtonComponents BtnMinisize { get; private set; }
		public ButtonComponents BtnCD { get; private set; }
		public SliderComponents SldVolume { get; private set; }
		public SliderComponents SldPan { get; private set; }
		public SliderComponents SldTrack { get; private set; }
		public GraphicComponents LabelTitle { get; private set; }
		public GraphicComponents LabelTime { get; private set; }
		public FormComponents PlayListForm { get; private set; }
		public PListGrid PlayListGrid { get; private set; }
		public ButtonComponents PBtnOpen { get; private set; }
		public ButtonComponents PBtnSave { get; private set; }
		public ButtonComponents PBtnRemove { get; private set; }
		public ButtonComponents PBtnUp { get; private set; }
		public ButtonComponents PBtnDown { get; private set; }
		public ButtonComponents PBtnClose { get; private set; }
		public ButtonComponents PBtnClear { get; private set; }

		// ===========================
		// インデクサ（OldSkinSystemとの互換）
		// ===========================
		public object this[string propertyName]
		{
			get { return typeof(NewSkinSystem).GetProperty(propertyName).GetValue(this); }
		}

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
			BtnOpen = LoadButton(skin.Buttons, "BtnOpen");
			BtnClose = LoadButton(skin.Buttons, "BtnClose");
			BtnPlay = LoadButton(skin.Buttons, "BtnPlay");
			BtnStop = LoadButton(skin.Buttons, "BtnStop");
			BtnBack = LoadButton(skin.Buttons, "BtnBack");
			BtnSeekBack = LoadButton(skin.Buttons, "BtnSeekBack");
			BtnPause = LoadButton(skin.Buttons, "BtnPause");
			BtnSeekForward = LoadButton(skin.Buttons, "BtnSeekForward");
			BtnNext = LoadButton(skin.Buttons, "BtnNext");
			BtnRandom = LoadButton(skin.Buttons, "BtnRandom");
			BtnLoop = LoadButton(skin.Buttons, "BtnLoop");
			BtnSetting = LoadButton(skin.Buttons, "BtnSetting");
			BtnPlaylist = LoadButton(skin.Buttons, "BtnPlaylist");
			BtnMinisize = LoadButton(skin.Buttons, "BtnMinisize");
			BtnCD = LoadButton(skin.Buttons, "BtnCD");

			// Sliders
			SldVolume = LoadSlider(skin.Sliders, "SldVolume");
			SldPan = LoadSlider(skin.Sliders, "SldPan");
			SldTrack = LoadSlider(skin.Sliders, "SldTrack");

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

			// Text
			LabelTitle = LoadText(skin.Text, "LabelTitle");
			LabelTime = LoadText(skin.Text, "LabelTime");

			// PlayList
			var pl = skin.PlayList;
			PlayListForm = new FormComponents
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
			};
			PlayListGrid = new PListGrid
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
			};

			PBtnOpen = LoadButton(pl.Buttons, "PBtnOpen");
			PBtnSave = LoadButton(pl.Buttons, "PBtnSave");
			PBtnRemove = LoadButton(pl.Buttons, "PBtnRemove");
			PBtnUp = LoadButton(pl.Buttons, "PBtnUp");
			PBtnDown = LoadButton(pl.Buttons, "PBtnDown");
			PBtnClose = LoadButton(pl.Buttons, "PBtnClose");
			PBtnClear = LoadButton(pl.Buttons, "PBtnClear");
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

		private GraphicComponents LoadText(
			Dictionary<string, TextDef> texts, string key)
		{
			var result = new GraphicComponents { Enabled = false };
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