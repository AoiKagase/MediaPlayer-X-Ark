using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Timer Timer
        {
            get { return this.ScrollTime; }
            set { this.ScrollTime = value; }
        }
        public ScrollLabel()
        {
            InitializeComponent();
        }

        private void ScrollLabel_Load(object sender, EventArgs e)
        {
            this.Label.Left = 0;
            this.Label.Top = 0;
            this.Label.Width = this.Width;
            this.Label.Height = this.Height;
            this.ScrollTime.Interval = 100;
            this.Label.Text = "";
        }

        private void ScrollTime_Tick(object sender, EventArgs e)
        {
            if (Label.Left + Label.Width < 0)
                Label.Left = this.Width + 10;
            Label.Left--;
        }
    }
}
