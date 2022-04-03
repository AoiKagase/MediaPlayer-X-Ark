using System;
using System.Drawing;
using System.Windows.Forms;
using MediaPlayer_X_Ark.Skin;
using System.IO;

namespace MediaPlayer_X_Ark
{
    public partial class MainForm : Form
    {
        bool initialize = false;
        public static PlayerEngine player;

        Graphics gSpectrum;
        Bitmap bmpSpectrumSrc;
        OldSkinSystem oldSkinSystem;
		private ToolTip _toolTip;
        private int playingIndex = 0;
        private PlayListForm playListForm;

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// スキンロード
        /// 設定ファイルからスキンファイルパスを取得して投げる
        /// </summary>
        /// <param name="skinFile"></param>
        private void SkinLoad(string skinFile)
        {
            // 旧形式特化の処理なので今後変更予定

            // 開く（oldSkinSystemクラス内に設定を全てロード）
            oldSkinSystem.Open(skinFile);

            // 設定反映：メインフォーム
            BackgroundImage = oldSkinSystem.MainForm.BackImage;
            Width = oldSkinSystem.MainForm.BackImage.Width;
            Height = oldSkinSystem.MainForm.BackImage.Height;

            // 設定反映：スペクトラム領域
            Spectrum.Left = oldSkinSystem.ImgSpectrum.Position.Left;
            Spectrum.Top = oldSkinSystem.ImgSpectrum.Position.Top;
            Spectrum.Width = oldSkinSystem.ImgSpectrum.Position.Width;
            Spectrum.Height = oldSkinSystem.ImgSpectrum.Position.Height;
            // スペクトラム画像の保持
            if (oldSkinSystem.ImgSpectrum.Image != null)
                bmpSpectrumSrc = new Bitmap(oldSkinSystem.ImgSpectrum.Image);
            else
            {
                bmpSpectrumSrc = new Bitmap(Spectrum.Width, Spectrum.Height);
                Graphics g = Graphics.FromImage(bmpSpectrumSrc);
                g.Clear(oldSkinSystem.ImgSpectrum.Color);
            }
            // スペクトラム領域のグラフィックインスタンス生成
            gSpectrum = Spectrum.CreateGraphics();
            // スペクトラム領域の初期化
            player.spectrum.Initialize(gSpectrum, bmpSpectrumSrc);

            string cName = "";
            foreach(Control c in Controls)
            {
                cName = c.Name;
                // 設定反映：ボタン類
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
                // 設定反映：スライダー類
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
                // 設定反映：文字領域（スクロールタイトル等）
                if (c.GetType() == typeof(ScrollLabel))
                {
                    ((ScrollLabel)c).BackColor = Color.Transparent;
                    ((ScrollLabel)c).Value.Font = ((GraphicComponents)oldSkinSystem[cName]).Font;
                    ((ScrollLabel)c).Value.ForeColor = ((GraphicComponents)oldSkinSystem[cName]).FontColor;
                    ((ScrollLabel)c).Top = ((GraphicComponents)oldSkinSystem[cName]).Position.Top;
                    ((ScrollLabel)c).Left = ((GraphicComponents)oldSkinSystem[cName]).Position.Left;
                    ((ScrollLabel)c).Width = ((GraphicComponents)oldSkinSystem[cName]).Position.Width;
                    ((ScrollLabel)c).Height = ((GraphicComponents)oldSkinSystem[cName]).Position.Height;
                    ((ScrollLabel)c).Enabled = ((GraphicComponents)oldSkinSystem[cName]).Enabled;
                    ((ScrollLabel)c).Visible = ((GraphicComponents)oldSkinSystem[cName]).Enabled;
                    ((ScrollLabel)c).Timer.Interval = ((GraphicComponents)oldSkinSystem[cName]).Interval > 0 ? ((GraphicComponents)oldSkinSystem[cName]).Interval : 100;
                    ((ScrollLabel)c).Timer.Enabled = ((GraphicComponents)oldSkinSystem[cName]).Interval > 0 ? true : false;
                }
            }
        }

        /// <summary>
        /// ファイルを開く
        /// </summary>
        /// <param name="fileName"></param>
        private void OpenFile(string fileName)
        {
            // Open File
            if (player.CreateSound(fileName, out playingIndex) == FMOD.RESULT.OK)
            {
                PlayLoad();
            }
        }

        private void PlayLoad()
        {
            // =====================================================================
            // 本来は先に設定を終わらせてから再生させたいが、
            // 設定するためのFMOD-Channelインスタンスが再生後に生成されるためやむを得ず
            // (何か方法があるかもしれない)
            // =====================================================================

            // 再生
            player.PlaySound(playingIndex);

            // 曲長に合わせてトラックバーの総量調整
            SldTrack.Maximum = (int)player.GetLength(playingIndex);

            // ボリュームを設定値へ
            float volume = ((float)SldVolume.Value) / 100f;
            player.SetVolume(volume);

            // PANを設定値へ
            float pan = ((float)SldPan.Value) / 10f;
            player.SetPan(pan);

            // タグ取得
            player.GetTags(playingIndex);
            LabelTitle.Value.Text = (!string.IsNullOrEmpty(player.PlayList[playingIndex].Title)) ? player.PlayList[playingIndex].Title : Path.GetFileName(player.PlayList[playingIndex].FileName);
            LabelTitle.Value.Text += (!string.IsNullOrEmpty(player.PlayList[playingIndex].Artist)) ? (" - " + player.PlayList[playingIndex].Artist) : "";
            LabelTitle.Value.Text += (!string.IsNullOrEmpty(player.PlayList[playingIndex].Album)) ? (" - " + player.PlayList[playingIndex].Album) : "";
        }
        /// <summary>
        /// ボタンクリック時のイベント（MouseDown時）
        /// </summary>
        /// <param name="button"></param>
        private void BtnDownEvent(ref object button)
        {
            // 背景画像を押下時の画像へ変更
            ((Button)button).BackgroundImage = ((ButtonComponents)oldSkinSystem[((Button)button).Name]).DownImage;
            ((Button)button).Refresh();
        }
        /// <summary>
        /// ボタンクリック時のイベント（MouseUp時）
        /// </summary>
        /// <param name="button"></param>
        private void BtnUpEvent(ref object button)
        {
            // 背景画像を元画像へ変更
            ((Button)button).BackgroundImage = ((ButtonComponents)oldSkinSystem[((Button)button).Name]).BackImage;
            ((Button)button).Refresh();
        }

        /// =============================================================
        /// 各コントロールイベント
        /// =============================================================
        #region MainForm Event
        /// <summary>
        /// フォームロード処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // ===================================
            // インスタンスの生成
            // ===================================
            // ツールチップ
            _toolTip = new ToolTip(components);
            // FMODサウンドエンジン
            player = new PlayerEngine();

            // 予定：設定ファイルの読み込み スキンファイルの指定も含む
            // 旧形式（XSF）のスキンファイルの場合はOldSkinSystem
            // 新形式（JSON）の場合はNewSkinSystemへインスタンス切替
            // スキンシステム
            oldSkinSystem = new OldSkinSystem();
            // スキンロード
            SkinLoad("RRS\\rack.xsf");

            // ボリューム最大値を強制100（旧形式スキンはこの数値を変動出来ていた為、処理簡略化を考慮する）
            SldVolume.Maximum = 100;

            initialize = true;

            playListForm = new PlayListForm();
            playListForm.Show();
        }

        /// <summary>
        /// 本体ドラッグによるウィンドウ移動
        /// </summary>
        private Point mousePoint;

        /// <summary>
        /// フォーム内のマウス押下処理
        /// 位置の記憶
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                //位置を記憶する
                mousePoint = new Point(e.X, e.Y);
            }
        }

        /// <summary>
        /// フォーム内のマウス移動処理
        /// フォームの位置をマウス移動量に応じて移動する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Left += e.X - mousePoint.X;
                Top += e.Y - mousePoint.Y;
            }
        }

        /// <summary>
        /// フォームクローズ処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            player = null;
        }
        #endregion

        #region Timer Event
        /// <summary>
        /// タイマー処理
        /// リアルタイム処理が必要なものは全てここで処理する
        /// (スレッド化したい)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerTimer_Tick(object sender, EventArgs e)
        {
            // 初期化済みの場合のみ処理する
            if (!initialize)
                return;

            // スペクトラム画像の反映
            player.spectrum.UpdateSpectrum(gSpectrum, ref bmpSpectrumSrc, Spectrum.Width, Spectrum.Height, 0);

            // 曲調トラックバーの反映 (シーク中はボタン側で動作する為動かさない)
            if (this.seekValue == 0)
                SldTrack.Value = (int)player.GetPosition();
            TimeSpan time1 = TimeSpan.FromMilliseconds(SldTrack.Value);
            TimeSpan time2 = TimeSpan.FromMilliseconds(SldTrack.Maximum);

            LabelTime.Value.Text = time1.ToString(@"mm\:ss") + "/" + time2.ToString(@"mm\:ss");
        }
        #endregion

        #region Button MouseDown Event
        /// <summary>
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
        private void BtnBack_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }
        private void BtnPlay_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }
        private int seekValue;
        private int seeking;
        private void BtnSeekBack_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
            seeking = 2;
        }
        private void BtnPause_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }
        private void BtnSeekForward_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
            seeking = 1;
        }
        private void BtnNext_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }
        private void BtnRandom_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }
        private void BtnLoop_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }
        private void BtnSetting_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }
        private void BtnPlaylist_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }
        private void BtnMinisize_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }
        #endregion

        #region Button MouseUp Event
        private void BtnBack_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
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
        private void BtnSeekBack_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
            this.seekValue = 0;
            this.seeking = 0;
        }
        private void BtnPause_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }
        private void BtnSeekForward_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
            this.seekValue = 0;
            this.seeking = 0;
        }
        private void BtnNext_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }
        private void BtnRandom_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }
        private void BtnLoop_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }
        private void BtnSetting_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }
        private void BtnPlaylist_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }
        private void BtnMinisize_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }
        #endregion

        #region Button Click Event
        /// <summary>
        /// ファイルを開くボタンをクリック
        /// ファイルオープンダイアログにてファイル選択後、自動で再生する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenFile(OpenFileDialog.FileName);
            }
        }

        /// <summary>
        /// 再生/一時停止ボタンのクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPlay_Click(object sender, EventArgs e)
        {
            // プレイ中の場合はポーズする
            if (player.IsPlaying())
                player.Pause();
            else
                player.PlaySound(playingIndex);
        }

        /// <summary>
        /// 停止ボタンのクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStop_Click(object sender, EventArgs e)
        {
            // 問答無用の停止
            player.Stop();
        }


        /// <summary>
        /// 閉じるボタンのクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            // 終了
            Close();
        }
        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (playingIndex > 0)
            {
                playingIndex--;
            }
            PlayLoad();
        }
        private void BtnSeekBack_Click(object sender, EventArgs e)
        {
        }
        private void BtnPause_Click(object sender, EventArgs e)
        {
        }
        private void BtnSeekForward_Click(object sender, EventArgs e)
        {
        }
        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (playingIndex < player.PlayList.Count)
            {
                playingIndex++;
            }
            PlayLoad();
        }
        private void BtnRandom_Click(object sender, EventArgs e)
        {
        }
        private void BtnLoop_Click(object sender, EventArgs e)
        {
        }
        private void BtnSetting_Click(object sender, EventArgs e)
        {
        }
        private void BtnPlaylist_Click(object sender, EventArgs e)
        {
            playListForm.Show();
        }
        private void BtnMinisize_Click(object sender, EventArgs e)
        {
        }
        #endregion

        #region Slider Event
        /// <summary>
        /// トラックスライダー
        /// 移動時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SldTrack_SliderMoving(object sender, MouseEventArgs e)
        {
            TimeSpan time = TimeSpan.FromMilliseconds(SldTrack.Value);
            _toolTip.Show(time.ToString(@"hh\:mm\:ss"), this, ((CustomSlider)(sender)).Left, ((CustomSlider)(sender)).Top);
        }

        /// <summary>
        /// トラックスライダー
        /// 移動確定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SldTrack_SliderMoved(object sender, MouseEventArgs e)
        {
            uint time = (uint)SldTrack.Value;
            _toolTip.Hide(this);
            player.SetPosition(time);
        }

        /// <summary>
        /// パンスライダー
        /// 移動時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SldPan_SliderMoving(object sender, MouseEventArgs e)
        {
            _toolTip.Show(SldPan.Value.ToString(), this, ((CustomSlider)(sender)).Left, ((CustomSlider)(sender)).Top);
            float pan = ((float)SldPan.Value) / 10f;
            player.SetPan(pan);
        }

        /// <summary>
        /// パンスライダー
        /// 移動確定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SldPan_SliderMoved(object sender, MouseEventArgs e)
        {
            _toolTip.Hide(this);
        }

        /// <summary>
        /// ボリュームスライダー
        /// 移動時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SldVolume_SliderMoving(object sender, MouseEventArgs e)
        {
            float volume = ((float)SldVolume.Value) / 100f;
            player.SetVolume(volume);
            _toolTip.Show(SldVolume.Value.ToString("0"), this, ((CustomSlider)(sender)).Left, ((CustomSlider)(sender)).Top);
        }
        /// <summary>
        /// ボリュームスライダー
        /// 移動確定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SldVolume_SliderMoved(object sender, MouseEventArgs e)
        {
            float volume = ((float)SldVolume.Value) / 100f;
            player.SetVolume(volume);
            _toolTip.Hide(this);
        }
        #endregion

        private void SldTrack_ValueChanged(object sender, EventArgs e)
        {
            if (this.seekValue > 0)
            {
                TimeSpan stime = TimeSpan.FromMilliseconds(SldTrack.Value);
                _toolTip.Show(stime.ToString(@"hh\:mm\:ss"), this, ((CustomSlider)(sender)).Left, ((CustomSlider)(sender)).Top, 1);
                uint time = (uint)SldTrack.Value;
                player.SetPosition(time);
            }
        }

        private void SeekiTimer_Tick(object sender, EventArgs e)
        {
            switch (seeking)
            {
                case 0:
                    break;
                case 1:
                    this.seekValue += 100;
                    this.SldTrack.Value += seekValue;
                    break;
                case 2:
                    this.seekValue += 100;
                    this.SldTrack.Value -= seekValue;
                    break;
            }
        }
    }
}
