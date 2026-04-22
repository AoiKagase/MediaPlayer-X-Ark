using System;
using System.Drawing;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark
{
    public partial class ScrollLabel : UserControl
    {
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

        public ScrollLabel()
        {
            InitializeComponent();
            Resize += (_, _) => UpdateLabelBounds();
            Label.TextChanged += (_, _) => UpdateLabelBounds();
        }

        private void ScrollLabel_Load(object sender, EventArgs e)
        {
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
                Label.TextAlign = ContentAlignment.MiddleLeft;
            }
        }
    }
}
