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
    public partial class Form1 : Form
    {
        PlayerEngine player;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            player = new PlayerEngine();

            player.CreateSound("D:\\Program Files (x86)\\Steam\\steamapps\\music\\UNDERTALE Soundtrack\\toby fox - UNDERTALE Soundtrack - 999 MEGALOVANIA.mp3");

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
            Graphics g1 = Spectrum.CreateGraphics();
            Graphics g2 = SpectrumSrc.CreateGraphics();
            Brush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
            g1.FillRectangle(brush, 0, 0, Spectrum.Width, Spectrum.Height);
            player.spectrum.UpdateSpectrum(g1.GetHdc(), g2.GetHdc(), Spectrum.Width, Spectrum.Height);
        }
    }
}
