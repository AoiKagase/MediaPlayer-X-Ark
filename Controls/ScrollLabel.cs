using System;
using System.Drawing;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark
{
    public partial class ScrollLabel : UserControl
    {
        private Font _baseLabelFont;
        private bool _adjustingFont;

        public Label Value
        {
            get
            {
                return this.Label;
            }
        }

        public System.Windows.Forms.Timer Timer
        {
            get { return this.ScrollTime; }
            set { this.ScrollTime = value; }
        }

        public bool ScrollEnable
        {
            get;
            set;
        }

        public HorizontalAlignment HorizontalAlign
        {
            get;
            set;
        } = HorizontalAlignment.Left;

        public ScrollLabel()
        {
            InitializeComponent();
            Resize += (_, _) => UpdateLabelBounds();
            Label.TextChanged += (_, _) => UpdateLabelBounds();
            Label.FontChanged += (_, _) =>
            {
                if (!_adjustingFont)
                    _baseLabelFont = (Font)Label.Font.Clone();
            };
        }

        private void ScrollLabel_Load(object sender, EventArgs e)
        {
            _baseLabelFont = (Font)Label.Font.Clone();
            UpdateLabelBounds();
            this.ScrollTime.Interval = 100;
            this.Label.Text = "";
        }

        private void ScrollTime_Tick(object sender, EventArgs e)
        {
            if (this.ScrollEnable)
            {
                if (Label.Left + Label.Width < 0)
                    Label.Left = this.Width + 10;
                Label.Left--;
            }
        }

        private void UpdateLabelBounds()
        {
            Label.Margin = Padding.Empty;
            Label.Padding = Padding.Empty;
            Label.UseMnemonic = false;

            if (ScrollEnable)
            {
                Label.AutoSize = true;
                Label.MaximumSize = Size.Empty;
                Label.Left = Math.Min(Label.Left, Width);
                Label.Top = Math.Max(0, (Height - Label.Height) / 2);
            }
            else
            {
                Label.AutoSize = false;
                Label.Left = 0;
                Label.Top = 0;
                Label.Width = Width;
                Label.Height = Height;
                Label.TextAlign = HorizontalAlign switch
                {
                    HorizontalAlignment.Center => ContentAlignment.MiddleCenter,
                    HorizontalAlignment.Right => ContentAlignment.MiddleRight,
                    _ => ContentAlignment.MiddleLeft,
                };
                FitFontToBounds();
            }
        }

        public void RefreshLabelLayout()
        {
            UpdateLabelBounds();
            Label.Invalidate();
        }

        private void FitFontToBounds()
        {
            if (Label.Width <= 0 || Label.Height <= 0)
                return;

            var sourceFont = _baseLabelFont ?? Label.Font;
            float size = sourceFont.Size;
            TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.SingleLine;
            _adjustingFont = true;
            try
            {
                while (size > 1f)
                {
                    using var testFont = new Font(sourceFont.FontFamily, size, sourceFont.Style, GraphicsUnit.Point);
                    Size measured = TextRenderer.MeasureText(Label.Text ?? string.Empty, testFont, new Size(int.MaxValue, int.MaxValue), flags);
                    if (measured.Width <= Label.Width && measured.Height <= Label.Height)
                    {
                        if (!FontEquals(Label.Font, testFont))
                            Label.Font = (Font)testFont.Clone();
                        return;
                    }

                    size -= 0.25f;
                }
            }
            finally
            {
                _adjustingFont = false;
            }
        }

        private static bool FontEquals(Font left, Font right)
            => left != null
            && right != null
            && left.FontFamily.Name == right.FontFamily.Name
            && Math.Abs(left.Size - right.Size) < 0.01f
            && left.Style == right.Style
            && left.Unit == right.Unit;
    }
}
