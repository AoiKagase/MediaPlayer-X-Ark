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
    public partial class MainForm : Form
    {
        bool initialize = false;

        Graphics g1;
        Graphics g2;

        PlayerEngine player;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            g1 = Spectrum.CreateGraphics();
            Bitmap bitmap = new Bitmap(Spectrum.Width, Spectrum.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            g2 = Graphics.FromImage(bitmap);
            g2.FillRectangle(Brushes.Cyan, g2.VisibleClipBounds);

            player = new PlayerEngine();
            player.CreateSound("");

            initialize = true;
        }

        private void Play_Click(object sender, EventArgs e)
        {
            player.PlaySound();
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            player.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!initialize)
                return;

//            g1.Clear(Color.White);
//            g1.FillRectangle(brush, 0, 0, Spectrum.Width, Spectrum.Height);
            player.spectrum.UpdateSpectrum(g1, g2, Spectrum.Width, Spectrum.Height, 1);
        }
    }
}
