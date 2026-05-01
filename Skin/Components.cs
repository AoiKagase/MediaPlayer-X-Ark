using MediaPlayer_X_Ark.Skin.New.Parts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Skin
{
    public class FormComponents
    {
        public Image BackImage;
        public RECT Position;
        public Color TransparentKey;
        public bool MagnetMode;

        public Color BackColor;
        public Color ForeColor;
        public Font Font;
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

    public class LabelComponents
    {
        public bool Enabled;
        public int Interval;
        public bool ScrollEnable;
        public HorizontalAlignment HorizontalAlign;
        public RECT Position;
        public Font Font;
        public Color BackColor;
        public Color FontColor;
        public Color BorderColor;
        public int BorderWidth;
    }

    public class SpectrumComponents
    {
        public string ImageFile;
        public Image Image;
        public Color Color;
        public RECT Position;
        public bool Enabled;
        /// <summary>ウェーブラインL色。スキン未定義時は Color.Empty → フォールバックは Lime。</summary>
        public Color WaveColorL;
        /// <summary>ウェーブラインR色。スキン未定義時は Color.Empty → フォールバックは Cyan。</summary>
        public Color WaveColorR;
    }

    public class GridComponents
    {
        public Color ListBackColor;
        public Color ListForeColor;
        public RECT ListPosition;
    }

    public class WaveformComponents
    {
        /// <summary>"trackbar" or "area"</summary>
        public string Target;
        public string Mode;
        public float Exponent;
        public Color ColorL;
        public Color ColorR;
        public Color ColorMix;
        public Color ColorPlayed;
        public Color ColorUnplayed;
        // target="area" の場合のみ使用
        public Location Location;
    }
    public class PictureComponents
	{
		public string ImageFile;
		public Image Image;
		public Color Color;
		public RECT Position;
		public Color BorderColor;
		public int BorderWidth;
		public int CornerRadius;
		public bool Enabled;
	}
}
