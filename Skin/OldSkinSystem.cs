using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MediaPlayer_X_Ark.Skin
{
	public class FormComponents
	{
		public Image BackImage;
		public RECT Position;
		public Color TransparentKey;
		public bool MagnetMode;
	}

	public class ButtonComponents
	{
		public Image BackImage;
		public Image DownImage;
		public Image OptionalImage;

		public RECT Position;
		public bool Toggle;
		public bool Enabled;
	}
	public class SliderComponents
	{
		public bool Enabled;
		public Image SliderImage;
		public Orientation Orientation;
		public RECT Position;
		public int Maximum;
		public int Minimum;
	}
	
	public class GraphicComponents
    {
		public bool Enabled;
		public int Interval;
		public bool ScrollEnable;
		public RECT Position;
		public Font Font;
		public Color BackColor;
		public Color FontColor;
    }

    public class SpectrumComponents
    {
		public string ImageFile;
		public Image Image;
		public Color Color;
		public RECT Position;
		public bool Enabled;
    }

    public class PListGrid
    {
		public Color ListBackColor;
		public Color ListForeColor;
		public RECT ListPosition;
	}

	class OldSkinSystem : ISkinSystem
	{
		private string defaultSkinDir = "Skins\\";
		private string defaultSkin = "Default\\Default.xsf";
		// 既存のフィールドをプロパティに変更
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

		public OldSkinSystem()
		{

		}

		public void Open(string loadSkinFile)
		{
			uint result = 0;
			string skinDir;
			string skinFile;
			string extension;
			StringBuilder nValue = new StringBuilder(256);
			uint capacity = Convert.ToUInt32(nValue.Capacity);

			if (File.Exists(loadSkinFile))
			{
				skinFile = loadSkinFile;
				skinDir = Path.GetDirectoryName(loadSkinFile);
			}
			else
			{
				if (!File.Exists(Application.StartupPath + defaultSkinDir + loadSkinFile))
				{
					skinFile = Application.StartupPath + defaultSkinDir + defaultSkin;
				}
				else
				{
					skinFile = (Application.StartupPath + defaultSkinDir + loadSkinFile);
				}
				skinDir = Path.GetDirectoryName(Application.StartupPath + defaultSkinDir + loadSkinFile);
			}

			result = Win32API.GetPrivateProfileString("SkinSetting", "-Extension", "bmp", nValue, capacity, skinFile);
			extension = nValue.ToString();
			result = Win32API.GetPrivateProfileString("SkinSetting", "-BackPicture", "back.bmp", nValue, capacity, skinFile);
			MainForm = new FormComponents
			{
				BackImage = LoadImage(skinDir + "\\" + nValue.ToString()),
				TransparentKey = ColorTranslator.FromWin32(0x202030),
				Position = new RECT
				{
					Width = (int)Win32API.GetPrivateProfileInt("SkinSetting", "-MainWidth", 0, skinFile),
					Height = (int)Win32API.GetPrivateProfileInt("SkinSetting", "-MainHeight", 0, skinFile),
				}
			};

			BtnOpen = LoadButtonComponents(skinDir, "12", extension, "ButtonVector", "-Open", skinFile);
			BtnClose = LoadButtonComponents(skinDir, "15", extension, "ButtonVector", "-Close", skinFile);
			BtnPlay = LoadButtonComponents(skinDir, "2", extension, "ButtonVector", "-Play", skinFile);
			BtnStop = LoadButtonComponents(skinDir, "4", extension, "ButtonVector", "-Stop", skinFile);
			BtnBack = LoadButtonComponents(skinDir, "0", extension, "ButtonVector", "-Back", skinFile);
			BtnSeekBack = LoadButtonComponents(skinDir, "1", extension, "ButtonVector", "-SeekBack", skinFile);
			BtnPause = LoadButtonComponents(skinDir, "3", extension, "ButtonVector", "-Pause", skinFile);
			BtnSeekForward = LoadButtonComponents(skinDir, "5", extension, "ButtonVector", "-SeekForward", skinFile);
			BtnNext = LoadButtonComponents(skinDir, "6", extension, "ButtonVector", "-Forward", skinFile);
			BtnRandom = LoadButtonComponents(skinDir, "10", extension, "ButtonVector", "-Random", skinFile);
			BtnLoop = LoadButtonComponents(skinDir, "11", extension, "ButtonVector", "-Loop", skinFile);
			BtnPlaylist = LoadButtonComponents(skinDir, "13", extension, "ButtonVector", "-PlayList", skinFile);
			BtnSetting = LoadButtonComponents(skinDir, "14", extension, "ButtonVector", "-Setting", skinFile);
			BtnMinisize = LoadButtonComponents(skinDir, "17", extension, "ButtonVector", "-MiniSize", skinFile);
			BtnCD = LoadButtonComponents(skinDir, "16", extension, "ButtonVector", "-OpenCD", skinFile);


			ImgSpectrum = new SpectrumComponents();
			result = Win32API.GetPrivateProfileString("GraphicArea", "-SpectrumPicture", "", nValue, capacity, skinFile);
			if (ImgSpectrum.Image != null)
			{
				ImgSpectrum.Image.Dispose();
			}
			if (nValue.Length > 0)
			{
				ImgSpectrum.ImageFile = skinDir + "\\" + nValue.ToString();
				ImgSpectrum.Image = LoadImage(ImgSpectrum.ImageFile);
			} else
			{
				ImgSpectrum.ImageFile = "";
				if (ImgSpectrum.Image != null)
				{
					ImgSpectrum.Image.Dispose();
				}
			}
			ImgSpectrum.Position = new RECT
			{
				Left = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-SpectrumAreaX", 0, skinFile),
				Top = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-SpectrumAreaY", 0, skinFile),
				Width = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-SpectrumAreaWidth", 0, skinFile),
				Height = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-SpectrumAreaHeight", 0, skinFile),
			};

			if (ImgSpectrum.Image == null)
			{
				result = Win32API.GetPrivateProfileString("GraphicArea", "-SpectrumColor", "FFFFFF", nValue, capacity, skinFile);
				if (nValue.ToString() != "")
				{
					ImgSpectrum.Color = LoadColor(nValue.ToString());
					ImgSpectrum.Image = new Bitmap(ImgSpectrum.Position.Width, ImgSpectrum.Position.Height);
					using (var g = Graphics.FromImage(ImgSpectrum.Image))
					{
						g.Clear(ImgSpectrum.Color);
					}
				}
			}

			ImgSpectrum.Enabled = true;

			SldVolume = LoadSliderComponents(skinDir, "VolumeSlider", skinFile);
			SldPan = LoadSliderComponents(skinDir, "PanSlider", skinFile);
			SldTrack = LoadSliderComponents(skinDir, "TrackSlider", skinFile);

			// TITLE
			result = Win32API.GetPrivateProfileString("GraphicArea", "-TitleFont", "", nValue, capacity, skinFile);
			int bold = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-TitleFontBold", 0, skinFile);
			int italic = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-TitleFontItalic", 0, skinFile);
			int size = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-TitleFontSize", 9, skinFile);
			string fontName = nValue.ToString();
			result = Win32API.GetPrivateProfileString("GraphicArea", "-TitleFontColor", "", nValue, capacity, skinFile);
			LabelTitle = new GraphicComponents();
			LabelTitle.FontColor = LoadColor(nValue.ToString());
			LabelTitle.Font = new Font(fontName, size, ((bold > 0) ? FontStyle.Bold : FontStyle.Regular) | ((italic > 0) ? FontStyle.Italic : FontStyle.Regular), GraphicsUnit.Point);
			LabelTitle.Position.Left = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaX", 0, skinFile);
			LabelTitle.Position.Top = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaY", 0, skinFile);
			LabelTitle.Position.Width = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaWidth", 0, skinFile);
			LabelTitle.Position.Height = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaHeight", 0, skinFile);
			LabelTitle.Interval = 100;
			LabelTitle.Enabled = true;

			// TIME
			result = Win32API.GetPrivateProfileString("GraphicArea", "-TimeTxtFont", "", nValue, capacity, skinFile);
			bold = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-TimeTxtFontBold", 0, skinFile);
			italic = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-TimeTxtFontItalic", 0, skinFile);
			size = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-TimeTxtFontSize", 9, skinFile);
			fontName = nValue.ToString();
			result = Win32API.GetPrivateProfileString("GraphicArea", "-TimeTxtColor", "", nValue, capacity, skinFile);
			LabelTime = new GraphicComponents();
			LabelTime.FontColor = LoadColor(nValue.ToString());
			LabelTime.Font = new Font(fontName, size, ((bold > 0) ? FontStyle.Bold : FontStyle.Regular) | ((italic > 0) ? FontStyle.Italic : FontStyle.Regular), GraphicsUnit.Point);
			LabelTime.Position.Left = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-TimeTxtAreaX", 0, skinFile);
			LabelTime.Position.Top = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-TimeTxtAreaY", 0, skinFile);
			LabelTime.Position.Width = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaWidth", 0, skinFile);
			LabelTime.Position.Height = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaHeight", 0, skinFile);
			LabelTime.Interval = 0;
			LabelTime.Enabled = true;
			LabelTitle.ScrollEnable = LabelTitle.Interval > 0;
			LabelTime.ScrollEnable = LabelTime.Interval > 0;
			// Playlist
			result = Win32API.GetPrivateProfileString("PlayListMain", "-BackPic", "playlist\\playlists.bmp", nValue, capacity, skinFile);
			PlayListForm = new FormComponents();
			PlayListGrid = new PListGrid();

			PlayListForm.BackImage = LoadImage(skinDir + "\\" + nValue.ToString());
			result = Win32API.GetPrivateProfileString("PlayListMain", "-ListBackColor", "000001", nValue, capacity, skinFile);
			PlayListGrid.ListBackColor = LoadColor(nValue.ToString());
			result = Win32API.GetPrivateProfileString("PlayListMain", "-ListForeColor", "FFFFFF", nValue, capacity, skinFile);
			PlayListGrid.ListForeColor = LoadColor(nValue.ToString());
			PlayListForm.MagnetMode = Win32API.GetPrivateProfileInt("PlayListMain", "-MagnetMode", 0, skinFile) > 0;
			PlayListForm.Position = new RECT();
			PlayListForm.Position.Left = (int)Win32API.GetPrivateProfileInt("PlayListMain", "-MainX", 0, skinFile);
			PlayListForm.Position.Top = (int)Win32API.GetPrivateProfileInt("PlayListMain", "-MainY", 0, skinFile);
			PlayListForm.Position.Width = (int)Win32API.GetPrivateProfileInt("PlayListMain", "-MainWidth", 0, skinFile);
			PlayListForm.Position.Height = (int)Win32API.GetPrivateProfileInt("PlayListMain", "-MainHeight", 0, skinFile);
			PlayListForm.TransparentKey = ColorTranslator.FromWin32(0x202030);

			PlayListGrid.ListPosition = new RECT();
			PlayListGrid.ListPosition.Left = (int)Win32API.GetPrivateProfileInt("PlayListMain", "-ListX", 0, skinFile);
			PlayListGrid.ListPosition.Top = (int)Win32API.GetPrivateProfileInt("PlayListMain", "-ListY", 0, skinFile);
			PlayListGrid.ListPosition.Width = (int)Win32API.GetPrivateProfileInt("PlayListMain", "-ListWidth", 0, skinFile);
			PlayListGrid.ListPosition.Height = (int)Win32API.GetPrivateProfileInt("PlayListMain", "-ListHeight", 0, skinFile);

			PBtnOpen = LoadPListComponents(skinDir, extension, "0", "-POpen", skinFile);
			PBtnSave = LoadPListComponents(skinDir, extension, "1", "-PSave", skinFile);
			PBtnRemove = LoadPListComponents(skinDir, extension, "2", "-PRemove", skinFile);
			PBtnUp = LoadPListComponents(skinDir, extension, "3", "-PUp", skinFile);
			PBtnDown = LoadPListComponents(skinDir, extension, "4", "-PDown", skinFile);
			PBtnClose = LoadPListComponents(skinDir, extension, "5", "-PClose", skinFile);
			PBtnClear = LoadPListComponents(skinDir, extension, "6", "-PClear", skinFile);

			/*

			BtnOpen.position.Top = (int)Win32API.GetPrivateProfileInt("ButtonVector", "-OpenCDY", 0, skinFile);
			BtnOpen.position.Left = (int)Win32API.GetPrivateProfileInt("ButtonVector", "-OpenCDX", 0, skinFile);

						result = Win32API.GetPrivateProfileString("GraphicArea", "-ClearColor", Hex$(RGB(48, 32, 32)), nName, Leng, skinFile);
						result = Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaX", 0, skinFile);
						result = Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaY", 0, skinFile);
						result = Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaWidth", 0, skinFile);
						result = Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaHeight", 0, skinFile);

						result = Win32API.GetPrivateProfileString("GraphicArea", "-SpectrumWaveColor", "FFFFFF", nName, Leng, skinFile);
						result = Win32API.GetPrivateProfileString("GraphicArea", "-SpectrumPicture", "", nName, Leng, skinFile);

						result = Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaBackColor", 0, skinFile);
						result = Win32API.GetPrivateProfileInt("GraphicArea", "-TitleAreaX", 0, skinFile);
						result = Win32API.GetPrivateProfileInt("GraphicArea", "-TitleAreaY", 0, skinFile);

						result = Win32API.GetPrivateProfileInt("GraphicArea", "-FileTxtArea", 0, skinFile);
						result = Win32API.GetPrivateProfileInt("GraphicArea", "-FileTxtAreaX", 0, skinFile);
						result = Win32API.GetPrivateProfileInt("GraphicArea", "-FileTxtAreaY", 0, skinFile);
						result = Win32API.GetPrivateProfileString("GraphicArea", "-FileTxtColor", "FFFFFF", nName, Leng, skinFile);
						result = Win32API.GetPrivateProfileString("GraphicArea", "-FileTxtFont", "MS UI Gothic", nName, Leng, skinFile);
						result = Win32API.GetPrivateProfileInt("GraphicArea", "-FileTxtFontBold", 0, skinFile);
						result = Win32API.GetPrivateProfileInt("GraphicArea", "-FileTxtFontItalic", 0, skinFile);
						result = Win32API.GetPrivateProfileInt("GraphicArea", "-FileTxtFontSize", 0, skinFile);

						result = Win32API.GetPrivateProfileInt("PlayListMain", "-ListTab", 0, skinFile);
						result = Win32API.GetPrivateProfileInt("PlaylistMain", "-ListCharCount", 30, skinFile);
			*/
		}

		//インデクサの定義
		public object this[string propertyName]
		{
			get
			{
				return typeof(OldSkinSystem).GetField(propertyName).GetValue(this);
			}
			set
			{
				typeof(OldSkinSystem).GetProperty(propertyName).SetValue(this, value);
			}
		}

		private ButtonComponents LoadButtonComponents(
			string skinDir, string buttonNo, string extension,
			string section, string key, string skinFile)
		{
			ButtonComponents result = new ButtonComponents { Enabled = false }; 

			if (File.Exists(skinDir + "\\0-" + buttonNo + "." + extension))
			{
				result.BackImage = Image.FromFile(skinDir + "\\0-" + buttonNo + "." + extension);
				if (File.Exists(skinDir + "\\1-" + buttonNo + "." + extension))
				{
					result.DownImage = Image.FromFile(skinDir + "\\1-" + buttonNo + "." + extension);
					if (File.Exists(skinDir + "\\2-" + buttonNo + "." + extension))
					{
						result.OptionalImage = Image.FromFile(skinDir + "\\2-" + buttonNo + "." + extension);
						result.Toggle = true;
					}
					result.Position.Top = (int)Win32API.GetPrivateProfileInt(section, key + "Y", 0, skinFile);
					result.Position.Left = (int)Win32API.GetPrivateProfileInt(section, key + "X", 0, skinFile);
					result.Position.Width = result.BackImage.Width;
					result.Position.Height= result.BackImage.Height;
					result.Enabled = true;
				}
				else
					result.Enabled = false;
			}
			else
			{
				result.Enabled = false;
			}
			return result;
		}

		private SliderComponents LoadSliderComponents(string skinDir, string section, string skinFile)
		{
			SliderComponents result = new SliderComponents { Enabled = false };
			StringBuilder sb = new StringBuilder(256);
			uint ret;
			Win32API.GetPrivateProfileString(section, "-BarPicture", "bar.bmp", sb, Convert.ToUInt32(sb.Capacity), skinFile);
			result.SliderImage = LoadImage(skinDir + "\\" + sb.ToString());
			if (result.SliderImage != null)
			{
				ret = Win32API.GetPrivateProfileInt(section, "-BarVector", 0, skinFile);
				if (ret == 0)
					result.Orientation = Orientation.Horizontal;
				else
					result.Orientation = Orientation.Vertical;

				result.Position.Left = (int)Win32API.GetPrivateProfileInt(section, "-BarX", 0, skinFile);
				result.Position.Top = (int)Win32API.GetPrivateProfileInt(section, "-BarY", 0, skinFile);
				result.Minimum = (int)Win32API.GetPrivateProfileInt(section, "-BarMin", 0, skinFile);
				result.Maximum = (int)Win32API.GetPrivateProfileInt(section, "-BarMax", 0, skinFile);
				if (result.Orientation == Orientation.Horizontal)
				{
					result.Position.Width = (int)Win32API.GetPrivateProfileInt(section, "-BarAreaX2", 0, skinFile) - result.Position.Left + result.SliderImage.Width;
					result.Position.Height = result.SliderImage.Height;
				}
				else
				{
					result.Position.Width = result.SliderImage.Width;
					result.Position.Height = (int)Win32API.GetPrivateProfileInt(section, "-BarAreaY2", 0, skinFile) - result.Position.Top + result.SliderImage.Height;
				}
				result.Enabled = true;
			}
			return result;
        }

		private Image LoadImage(string path)
        {
			if (File.Exists (path))
				return Image.FromFile(path);
			else
				return null;
        }

		private Color LoadColor(string color)
        {
			if (color == "")
				color = "0000000";
			return ColorTranslator.FromWin32(Int32.Parse(color, System.Globalization.NumberStyles.HexNumber));
        }

		private ButtonComponents LoadPListComponents(string skinDir, string extension, string buttonNo, string key, string skinFile)
		{
			ButtonComponents result = new ButtonComponents { Enabled = false };
			if (File.Exists(skinDir + "\\playlist\\p" + buttonNo + "-0." + extension))
			{
				result.BackImage = Image.FromFile(skinDir + "\\playlist\\p" + buttonNo + "-0." + extension);
				if (File.Exists(skinDir + "\\playlist\\p" + buttonNo + "-1." + extension))
				{
					result.DownImage = Image.FromFile(skinDir + "\\playlist\\p" + buttonNo + "-1." + extension);
					result.Position.Top = (int)Win32API.GetPrivateProfileInt("PlayListButton", key + "Y", 0, skinFile);
					result.Position.Left = (int)Win32API.GetPrivateProfileInt("PlayListButton", key + "X", 0, skinFile);
					result.Position.Width = result.BackImage.Width;
					result.Position.Height = result.BackImage.Height;
					result.Enabled = true;
				}
				else
					result.Enabled = false;
			}
			else
			{
				result.Enabled = false;
			}
			return result;
        }
	}
}
