using System;
using System.Drawing;
using System.Windows.Forms;
using MediaPlayer_X_Ark.Skin;
namespace MediaPlayer_X_Ark
{
    public partial class MainForm : Form
    {
        bool initialize = false;

        Graphics g1;
        Graphics g2;
        Bitmap bitmap;
        PlayerEngine player;
        OldSkinSystem oldSkinSystem;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            player = new PlayerEngine();
            oldSkinSystem = new OldSkinSystem();


            this.SkinLoad("bbbs\\bs.xsf");

            g1 = Spectrum.CreateGraphics();
            bitmap = new Bitmap(oldSkinSystem.ImgSpectrum.BackImage);
            player.spectrum.Initialize(g1, bitmap);

            initialize = true;
        }

        private void SkinLoad(string skinFile)
        {
            oldSkinSystem.Open(skinFile);
            this.BackgroundImage = oldSkinSystem.MainForm.BackImage;
            this.Width = oldSkinSystem.MainForm.BackImage.Width;
            this.Height = oldSkinSystem.MainForm.BackImage.Height;

            this.Spectrum.Left = oldSkinSystem.ImgSpectrum.position.Left;
            this.Spectrum.Top = oldSkinSystem.ImgSpectrum.position.Top;
            this.Spectrum.Width = oldSkinSystem.ImgSpectrum.position.Width;
            this.Spectrum.Height = oldSkinSystem.ImgSpectrum.position.Height;

            string cName = "";
            foreach(Control c in this.Controls)
            {
                if (c.GetType() == typeof(Button))
                {
                    cName = c.Name;
                    ((Button)c).BackgroundImage = ((Components)oldSkinSystem[cName]).BackImage;
                    ((Button)c).Top = ((Components)oldSkinSystem[cName]).position.Top;
                    ((Button)c).Left = ((Components)oldSkinSystem[cName]).position.Left;
                    ((Button)c).Width = ((Components)oldSkinSystem[cName]).position.Width;
                    ((Button)c).Height = ((Components)oldSkinSystem[cName]).position.Height;
                    ((Button)c).Enabled = ((Components)oldSkinSystem[cName]).Enabled;
                    ((Button)c).Visible = ((Components)oldSkinSystem[cName]).Enabled;
                    ((Button)c).Refresh();
                }
            }
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            player.PlaySound();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            player.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!initialize)
                return;

            //            g1.Clear(Color.White);
            //            g1.FillRectangle(brush, 0, 0, Spectrum.Width, Spectrum.Height);
            player.spectrum.UpdateSpectrum(g1,ref bitmap, Spectrum.Width, Spectrum.Height, 1);

//            statusStrip1.Text = player.lastError;
        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Open File
                if (player.CreateSound(OpenFileDialog.FileName) == FMOD.RESULT.OK)
                {
                    player.PlaySound();
                }
            } else
            {
                MessageBox.Show(player.lastError);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnDownEvent(ref object button)
        {
            ((Button)button).BackgroundImage = ((Components)oldSkinSystem[((Button)button).Name]).DownImage;
            ((Button)button).Refresh();
        }
        private void BtnUpEvent(ref object button)
        {
            ((Button)button).BackgroundImage = ((Components)oldSkinSystem[((Button)button).Name]).BackImage;
            ((Button)button).Refresh();
        }

        private Point mousePoint;
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                //位置を記憶する
                mousePoint = new Point(e.X, e.Y);
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;
                //または、つぎのようにする
                //this.Location = new Point(
                //    this.Location.X + e.X - mousePoint.X,
                //    this.Location.Y + e.Y - mousePoint.Y);
            }
        }

        private void BtnOpen_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }

        private void BtnClose_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }

        private void BtnStop_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }


        private void BtnPlay_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }

        private void BtnClose_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }

        private void BtnOpen_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);

        }
        private void BtnPlay_MouseUp(object sender, MouseEventArgs e)
        {

            BtnUpEvent(ref sender);
        }

        private void BtnStop_MouseUp(object sender, MouseEventArgs e)
        {

            BtnUpEvent(ref sender);
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {

        }

        private void BtnBack_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }

        private void BtnBack_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }

        private void BtnSeekBack_Click(object sender, EventArgs e)
        {

        }

        private void BtnSeekBack_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }

        private void BtnSeekBack_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {

        }

        private void BtnPause_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }

        private void BtnPause_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }

        private void BtnSeekForward_Click(object sender, EventArgs e)
        {

        }

        private void BtnSeekForward_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }

        private void BtnSeekForward_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {

        }

        private void BtnNext_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }

        private void BtnNext_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }

        private void BtnRandom_Click(object sender, EventArgs e)
        {

        }

        private void BtnRandom_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }

        private void BtnRandom_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }

        private void BtnLoop_Click(object sender, EventArgs e)
        {

        }

        private void BtnLoop_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }

        private void BtnLoop_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }

        private void BtnSetting_Click(object sender, EventArgs e)
        {

        }

        private void BtnSetting_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }

        private void BtnSetting_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }

        private void BtnPlaylist_Click(object sender, EventArgs e)
        {

        }

        private void BtnPlaylist_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }

        private void BtnPlaylist_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }

        private void BtnMinisize_Click(object sender, EventArgs e)
        {

        }

        private void BtnMinisize_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }

        private void BtnMinisize_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }
    }
}
