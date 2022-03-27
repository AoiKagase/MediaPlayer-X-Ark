using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace MediaPlayer_X_Ark.Skin
{
    public struct Components
    {
        public Image BackImage;
        public Image DownImage;

        public RECT position;
    }
    class OldSkinSystem
    {
        private string defaultSkinDir = "Skins\\";
        private string defaultSkin = "Default\\Default.xsf";

        public Components MainForm;
        public Components ImgSpectrum;
        public Components BtnOpen;
        public Components BtnClose;
        public Components BtnPlay;
        public Components BtnStop;

        public OldSkinSystem()
        {

        }

        public void Open(string loadSkinFile)
        {
            uint result = 0;
            string skinDir;
            string skinFile;
            Components temp = new Components();
            StringBuilder nValue = new StringBuilder(256);
            if (!File.Exists(defaultSkinDir + loadSkinFile))
            {
                skinFile = Application.ExecutablePath + "\\" + defaultSkinDir + defaultSkin;
            }
            skinFile = (defaultSkinDir + loadSkinFile);
            skinDir = Path.GetDirectoryName(defaultSkinDir + loadSkinFile);

            result = Win32API.GetPrivateProfileString("SkinSetting", "-Extension", "bmp", nValue, Convert.ToUInt32(nValue.Capacity), skinFile);
            result = Win32API.GetPrivateProfileString("SkinSetting", "-BackPicture", "back.bmp", nValue, Convert.ToUInt32(nValue.Capacity), skinFile);
            MainForm.BackImage = Image.FromFile(skinDir + "\\" +  nValue.ToString());

            BtnOpen.BackImage = Image.FromFile(skinDir + "\\0-12.png");
            BtnOpen.DownImage = Image.FromFile(skinDir + "\\1-12.png");
            BtnOpen.position.Top = (int)Win32API.GetPrivateProfileInt("ButtonVector", "-OpenY", 0, skinFile);
            BtnOpen.position.Left = (int)Win32API.GetPrivateProfileInt("ButtonVector", "-OpenX", 0, skinFile);
            BtnOpen.position.Width = BtnOpen.BackImage.Width;
            BtnOpen.position.Height = BtnOpen.BackImage.Height;

            BtnClose.BackImage = Image.FromFile(skinDir + "\\0-15.png");
            BtnClose.DownImage = Image.FromFile(skinDir + "\\1-15.png");
            BtnClose.position.Top = (int)Win32API.GetPrivateProfileInt("ButtonVector", "-CloseY", 0, skinFile);
            BtnClose.position.Left = (int)Win32API.GetPrivateProfileInt("ButtonVector", "-CloseX", 0, skinFile);
            BtnClose.position.Width = BtnClose.BackImage.Width;
            BtnClose.position.Height = BtnClose.BackImage.Height;

            BtnPlay.BackImage = Image.FromFile(skinDir + "\\0-2.png");
            BtnPlay.DownImage = Image.FromFile(skinDir + "\\1-2.png");
            BtnPlay.position.Top = (int)Win32API.GetPrivateProfileInt("ButtonVector", "-PlayY", 0, skinFile);
            BtnPlay.position.Left = (int)Win32API.GetPrivateProfileInt("ButtonVector", "-PlayX", 0, skinFile);
            BtnPlay.position.Width = BtnPlay.BackImage.Width;
            BtnPlay.position.Height = BtnPlay.BackImage.Height;

            BtnStop.BackImage = Image.FromFile(skinDir + "\\0-4.png");
            BtnStop.DownImage = Image.FromFile(skinDir + "\\1-4.png");
            BtnStop.position.Top = (int)Win32API.GetPrivateProfileInt("ButtonVector", "-StopY", 0, skinFile);
            BtnStop.position.Left = (int)Win32API.GetPrivateProfileInt("ButtonVector", "-StopX", 0, skinFile);
            BtnStop.position.Width = BtnStop.BackImage.Width;
            BtnStop.position.Height = BtnStop.BackImage.Height;

            result = Win32API.GetPrivateProfileString("GraphicArea", "-SpectrumPicture", "", nValue, Convert.ToUInt32(nValue.Capacity), skinFile);
            ImgSpectrum.BackImage = Image.FromFile(skinDir + "\\" + nValue.ToString());
            ImgSpectrum.position.Top = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-SpectrumAreaY", 0, skinFile);
            ImgSpectrum.position.Left = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-SpectrumAreaX", 0, skinFile);
            ImgSpectrum.position.Width = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-SpectrumAreaWidth", 0, skinFile);
            ImgSpectrum.position.Height = (int)Win32API.GetPrivateProfileInt("GraphicArea", "-SpectrumAreaHeight", 0, skinFile);


            /*
                        result = Win32API.GetPrivateProfileInt("SkinSetting", "-MainWidth", 0, .SkinFile)
                        result = Win32API.GetPrivateProfileInt("SkinSetting", "-MainHeight", 0, .SkinFile)

                        BtnOpen.position.Top = (int)Win32API.GetPrivateProfileInt("ButtonVector", "-OpenCDY", 0, skinFile);
                        BtnOpen.position.Left = (int)Win32API.GetPrivateProfileInt("ButtonVector", "-OpenCDX", 0, skinFile);
                        result = Win32API.GetPrivateProfileInt("ButtonVector", "-BackY", 0, .SkinFile)
                             result = Win32API.GetPrivateProfileInt("ButtonVector", "-BackX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-SeekBackY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-SeekBackX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-PauseY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-PauseX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-StopY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-StopX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-SeekForwardY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-SeekForwardX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-ForwardY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-ForwardX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-RandomY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-RandomX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-LoopY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-LoopX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-SettingY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-SettingX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-PlayListY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-PlayListX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-MiniSizeY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("ButtonVector", "-MiniSizeX", 0, .SkinFile)

                            result = Win32API.GetPrivateProfileString("VolumeSlider", "-BarPicture", "bar.bmp", nName, Leng, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("VolumeSlider", "-BarVector", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("VolumeSlider", "-BarX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("VolumeSlider", "-BarY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("VolumeSlider", "-BarMin", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("VolumeSlider", "-BarMax", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("VolumeSlider", "-BarAreaX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("VolumeSlider", "-BarAreaX2", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("VolumeSlider", "-BarAreaY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("VolumeSlider", "-BarAreaY2", 0, .SkinFile)

                            result = Win32API.GetPrivateProfileString("TrackSlider", "-BarPicture", "bar.bmp", nName, Leng, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("TrackSlider", "-BarVector", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("TrackSlider", "-BarX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("TrackSlider", "-BarY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("TrackSlider", "-BarMin", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("TrackSlider", "-BarMax", 100, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("TrackSlider", "-BarAreaX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("TrackSlider", "-BarAreaX2", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("TrackSlider", "-BarAreaY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("TrackSlider", "-BarAreaY2", 0, .SkinFile)

                            result = Win32API.GetPrivateProfileString("PanSlider", "-BarPicture", "bar.bmp", nName, Leng, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("PanSlider", "-BarVector", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("PanSlider", "-BarX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("PanSlider", "-BarY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("PanSlider", "-BarMin", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("PanSlider", "-BarMax", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("PanSlider", "-BarAreaX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("PanSlider", "-BarAreaX2", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("PanSlider", "-BarAreaY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("PanSlider", "-BarAreaY2", 0, .SkinFile)

                            result = Win32API.GetPrivateProfileString("GraphicArea", "-ClearColor", Hex$(RGB(48, 32, 32)), nName, Leng, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaX", 0, .SkinFile)
                               result = Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaWidth", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaHeight", 0, .SkinFile)

                            result = Win32API.GetPrivateProfileString("GraphicArea", "-SpectrumColor", "FFFFFF", nName, Leng, .SkinFile)
                             result = Win32API.GetPrivateProfileString("GraphicArea", "-SpectrumWaveColor", "FFFFFF", nName, Leng, .SkinFile)
                             result = Win32API.GetPrivateProfileString("GraphicArea", "-SpectrumPicture", "", nName, Leng, .SkinFile)

                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-TextAreaBackColor", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-TitleAreaX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-TitleAreaY", 0, .SkinFile)

                            result = Win32API.GetPrivateProfileString("GraphicArea", "-TitleFont", "MS UI Gothic", nName, Leng, .SkinFile)
                            result = Win32API.GetPrivateProfileString("GraphicArea", "-TitleFontColor", "FFFFFF", nName, Leng, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-TitleFontBold", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-TitleFontItalic", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-TitleFontSize", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-TimeTxtAreaX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-TimeTxtAreaY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileString("GraphicArea", "-TimeTxtFont", "MS UI Gothic", nName, Leng, .SkinFile)
                            result = Win32API.GetPrivateProfileString("GraphicArea", "-TimeTxtColor", "FFFFFF", nName, Leng, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-TimeTxtFontBold", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-TimeTxtFontItalic", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-TimeTxtFontSize", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-FileTxtArea", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-FileTxtAreaX", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileInt("GraphicArea", "-FileTxtAreaY", 0, .SkinFile)
                            result = Win32API.GetPrivateProfileString("GraphicArea", "-FileTxtColor", "FFFFFF", nName, Leng, .SkinFile)
                            result = Win32API.GetPrivateProfileString("GraphicArea", "-FileTxtFont", "MS UI Gothic", nName, Leng, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("GraphicArea", "-FileTxtFontBold", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("GraphicArea", "-FileTxtFontItalic", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("GraphicArea", "-FileTxtFontSize", 0, .SkinFile)

                    result = Win32API.GetPrivateProfileString("PlayListMain", "-BackPic", "playlist\playlists.bmp", nName, Leng, .SkinFile)
                    result = Win32API.GetPrivateProfileString("PlayListMain", "-ListBackColor", "000001", nName, Leng, .SkinFile)
                    result = Win32API.GetPrivateProfileString("PlayListMain", "-ListForeColor", "FFFFFF", nName, Leng, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListMain", "-MagnetMode", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListMain", "-MainX", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListMain", "-MainY", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListMain", "-MainWidth", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListMain", "-MainHeight", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListMain", "-ListX", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListMain", "-ListY", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListMain", "-ListWidth", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListMain", "-ListHeight", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListMain", "-ListTab", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlaylistMain", "-ListCharCount", 30, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListButton", "-POpenX", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListButton", "-POpenY", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListButton", "-PSaveX", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListButton", "-PSaveY", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListButton", "-PRemoveX", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListButton", "-PRemoveY", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListButton", "-PUpX", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListButton", "-PUpY", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListButton", "-PDownX", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListButton", "-PDownY", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListButton", "-PCloseX", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListButton", "-PCloseY", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListButton", "-PClearX", 0, .SkinFile)
                    result = Win32API.GetPrivateProfileInt("PlayListButton", "-PClearY", 0, .SkinFile)
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
    }
}
