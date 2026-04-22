using System;
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
            Label.AutoSize = false;
            Label.Top = 0;
            Label.Height = Height;
            if (!ScrollEnable)
                Label.Left = 0;
            if (Label.Width < Width)
                Label.Width = Width;
        }
    }
}
