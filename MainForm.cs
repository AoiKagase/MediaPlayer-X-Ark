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
            this.SldVolume.Maximum = 100;
            initialize = true;
        }

        private void SkinLoad(string skinFile)
        {
            oldSkinSystem.Open(skinFile);
            this.BackgroundImage = oldSkinSystem.MainForm.BackImage;
            this.Width = oldSkinSystem.MainForm.BackImage.Width;
            this.Height = oldSkinSystem.MainForm.BackImage.Height;

            this.Spectrum.Left = oldSkinSystem.ImgSpectrum.Position.Left;
            this.Spectrum.Top = oldSkinSystem.ImgSpectrum.Position.Top;
            this.Spectrum.Width = oldSkinSystem.ImgSpectrum.Position.Width;
            this.Spectrum.Height = oldSkinSystem.ImgSpectrum.Position.Height;

            string cName = "";
            foreach(Control c in this.Controls)
            {
                cName = c.Name;
                if (c.GetType() == typeof(Button))
                {
                    ((Button)c).BackgroundImage = ((ButtonComponents)oldSkinSystem[cName]).BackImage;
                    ((Button)c).Top = ((ButtonComponents)oldSkinSystem[cName]).Position.Top;
                    ((Button)c).Left = ((ButtonComponents)oldSkinSystem[cName]).Position.Left;
                    ((Button)c).Width = ((ButtonComponents)oldSkinSystem[cName]).Position.Width;
                    ((Button)c).Height = ((ButtonComponents)oldSkinSystem[cName]).Position.Height;
                    ((Button)c).Enabled = ((ButtonComponents)oldSkinSystem[cName]).Enabled;
                    ((Button)c).Visible = ((ButtonComponents)oldSkinSystem[cName]).Enabled;
                    ((Button)c).Refresh();
                }
                if (c.GetType() == typeof(CustomSlider))
                {
                    ((CustomSlider)c).SliderImage = ((SliderComponents)oldSkinSystem[cName]).SliderImage;
                    ((CustomSlider)c).Orientation = ((SliderComponents)oldSkinSystem[cName]).Orientation;
                    ((CustomSlider)c).Minimum = ((SliderComponents)oldSkinSystem[cName]).Minimum;
                    ((CustomSlider)c).Maximum = ((SliderComponents)oldSkinSystem[cName]).Maximum;
                    ((CustomSlider)c).Top = ((SliderComponents)oldSkinSystem[cName]).Position.Top;
                    ((CustomSlider)c).Left = ((SliderComponents)oldSkinSystem[cName]).Position.Left;
                    ((CustomSlider)c).Width = ((SliderComponents)oldSkinSystem[cName]).Position.Width;
                    ((CustomSlider)c).Height = ((SliderComponents)oldSkinSystem[cName]).Position.Height;
                    ((CustomSlider)c).Enabled = ((SliderComponents)oldSkinSystem[cName]).Enabled;
                    ((CustomSlider)c).Visible = ((SliderComponents)oldSkinSystem[cName]).Enabled;
                    ((CustomSlider)c).Value = 0;
                    ((CustomSlider)c).Refresh();
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
            this.SldTrack.Value = (int) player.GetPosition();
            this.SldVolume.Value = (int)player.GetVolume();
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
                    this.SldTrack.Maximum = (int)player.GetLength();
                }
            } else
            {
//              MessageBox.Show(player.lastError);
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
            ((Button)button).BackgroundImage = ((ButtonComponents)oldSkinSystem[((Button)button).Name]).DownImage;
            ((Button)button).Refresh();
        }
        private void BtnUpEvent(ref object button)
        {
            ((Button)button).BackgroundImage = ((ButtonComponents)oldSkinSystem[((Button)button).Name]).BackImage;
            ((Button)button).Refresh();
        }

        /// <summary>
        /// 本体ドラッグによるウィンドウ移動
        /// </summary>
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
            }
        }


        /// <summary>
        /// ボタン操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
