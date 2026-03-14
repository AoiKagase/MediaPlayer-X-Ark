using System;
using System.Drawing;
using System.Windows.Forms;
using MediaPlayer_X_Ark.Skin;
using System.IO;
using System.Numerics;

namespace MediaPlayer_X_Ark
{
    public partial class MainForm : Form
    {
        bool initialize = false;
        public static PlayerEngine player;

        OldSkinSystem oldSkinSystem;
		private ToolTip _toolTip;
        private int playingIndex = 0;
        private PlayListForm playListForm;
        private OptionsForm optionsForm;
		private CDForm cdForm;
		private static Engine.Configration config;
        private bool nowplaying = false;
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
            TransparencyKey = oldSkinSystem.MainForm.TransparentKey;
            Width = oldSkinSystem.MainForm.Position.Width;
            Height = oldSkinSystem.MainForm.Position.Height;

            // 設定反映：スペクトラム領域
            Spectrum.Left = oldSkinSystem.ImgSpectrum.Position.Left;
            Spectrum.Top = oldSkinSystem.ImgSpectrum.Position.Top;
            Spectrum.Width = oldSkinSystem.ImgSpectrum.Position.Width;
            Spectrum.Height = oldSkinSystem.ImgSpectrum.Position.Height;
            Graphics g;
            // スペクトラム画像の保持
            if (File.Exists(oldSkinSystem.ImgSpectrum.ImageFile))
                Spectrum.BitmapSpectrum = new Bitmap(oldSkinSystem.ImgSpectrum.Image);
            else
            {
                if (oldSkinSystem.ImgSpectrum.Image != null)
                {
                    g = Graphics.FromImage(oldSkinSystem.ImgSpectrum.Image);
                    g.Clear(oldSkinSystem.ImgSpectrum.Color);
                    g.Dispose();
                }
            }
            g = Graphics.FromImage(Spectrum.BitmapSnow);
            g.Clear(oldSkinSystem.ImgSpectrum.Color);
            g.Dispose();
            g = Graphics.FromImage(Spectrum.BitmapWave);
            g.Clear(oldSkinSystem.ImgSpectrum.Color);
            g.Dispose();

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
            this.Refresh();
            playListForm.Refresh();
            playListForm.Left = Left - ((FormComponents)oldSkinSystem["PlayListForm"]).Position.Left;
            playListForm.Top = Top - ((FormComponents)oldSkinSystem["PlayListForm"]).Position.Top;
            playListForm.BackgroundImage = ((FormComponents)oldSkinSystem["PlayListForm"]).BackImage;
            playListForm.Width = ((FormComponents)oldSkinSystem["PlayListForm"]).Position.Width;
            playListForm.Height = ((FormComponents)oldSkinSystem["PlayListForm"]).Position.Height;
            playListForm.TransparencyKey = ((FormComponents)oldSkinSystem["PlayListForm"]).TransparentKey;
            foreach (Control c in playListForm.Controls)
            {
                cName = c.Name;
                if (c.GetType() == typeof(DataGridView))
                {
                    ((DataGridView)c).BackgroundColor = ((PListGrid)oldSkinSystem[cName]).ListBackColor;
                    ((DataGridView)c).RowsDefaultCellStyle.BackColor = ((PListGrid)oldSkinSystem[cName]).ListBackColor;
                    ((DataGridView)c).RowsDefaultCellStyle.ForeColor = ((PListGrid)oldSkinSystem[cName]).ListForeColor;
                    ((DataGridView)c).ForeColor = ((PListGrid)oldSkinSystem[cName]).ListForeColor;
                    ((DataGridView)c).Left = ((PListGrid)oldSkinSystem[cName]).ListPosition.Left;
                    ((DataGridView)c).Top = ((PListGrid)oldSkinSystem[cName]).ListPosition.Top;
                    ((DataGridView)c).Width = ((PListGrid)oldSkinSystem[cName]).ListPosition.Width;
                    ((DataGridView)c).Height = ((PListGrid)oldSkinSystem[cName]).ListPosition.Height;
                }
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
            }
        }

        /// <summary>
        /// ファイルを開く
        /// </summary>
        /// <param name="fileName"></param>
        public void OpenFile(string fileName)
        {
            // Open File
            if (player.CreateSound(fileName, out playingIndex) == FMOD.RESULT.OK)
            {
                PlayLoad();
            }
        }

        /// <summary>
        /// Indexを指定して再生する。(主にプレイリストから直接再生)
        /// </summary>
        /// <param name="index"></param>
        public void PlayLoad(int index)
        {
            playingIndex = index;
            PlayLoad();
        }

        private void PlayLoad()
        {
			// デバイスの反映
			player.SetDevice(config.settings.Device);

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

            nowplaying = true;
        }
        /// <summary>
        /// ボタンクリック時のイベント（MouseDown時）
        /// </summary>
        /// <param name="button"></param>
        public void BtnDownEvent(ref object button)
        {
            // 背景画像を押下時の画像へ変更
            ((Button)button).BackgroundImage = ((ButtonComponents)oldSkinSystem[((Button)button).Name]).DownImage;
            ((Button)button).Refresh();
        }
        /// <summary>
        /// ボタンクリック時のイベント（MouseUp時）
        /// </summary>
        /// <param name="button"></param>
        public void BtnUpEvent(ref object button)
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

			// ① 設定を先に読み込む
			config = new Engine.Configration(ref player);

			// ② OutputType と SoftwareFormat は init() より前に設定
			player.SetOutputTypeBeforeInit(config.GetOutputType());
			player.SetSoftwareFormat(config.GetSampleRate(), config.GetSpeakerMode());

			// ③ init() を実行
			player.Initialize();

			// ④ Device は init() 後でOK
			player.SetDevice(config.settings.Device);

            playListForm = new PlayListForm(this);
//            playListForm.Show(this);
            optionsForm = new OptionsForm(ref player, ref config);
			cdForm = new CDForm(this);
			//            optionsForm.Show(this);
			// 予定：設定ファイルの読み込み スキンファイルの指定も含む
			// 旧形式（XSF）のスキンファイルの場合はOldSkinSystem
			// 新形式（JSON）の場合はNewSkinSystemへインスタンス切替
			// スキンシステム
			oldSkinSystem = new OldSkinSystem();

            // スキンロード
            SkinLoad(config.settings.Skin);
            Spectrum.Initialize(Color.Black);

            // ボリューム最大値を強制100（旧形式スキンはこの数値を変動出来ていた為、処理簡略化を考慮する）
            SldVolume.Maximum = 100;
            SldVolume.Value = config.settings.Volume;
            initialize = true;

            // 起動パラメータを取得し、ファイルパスが取得出来るならばOpen関数へ引き渡す
            string[] parameters = System.Environment.GetCommandLineArgs();
            if (parameters.Length > 1)
            {
                if (File.Exists(parameters[1]))
                {
                    OpenFile(parameters[1]);
                }
            }
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
                playListForm.Activate();
                this.Activate();
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

                playListForm.Left = Left - ((FormComponents)oldSkinSystem["PlayListForm"]).Position.Left;
                playListForm.Top = Top - ((FormComponents)oldSkinSystem["PlayListForm"]).Position.Top;
            }
        }

        /// <summary>
        /// フォームクローズ処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            config.Save();
			cdForm?.Dispose();   // 追加
			player.Dispose();  // 明示的に解放
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
            float[] mFFT = player.spectrum.UpdateSpectrum();
            Spectrum.mFFT = mFFT;
			// Waveデータ（追加）
			float[] mWave = player.wave.GetWaveData();
			Spectrum.mWave = mWave;

									 // 曲調トラックバーの反映 (シーク中はボタン側で動作する為動かさない)
			if (this.seekValue == 0)
                SldTrack.Value = (int)player.GetPosition();
            if (!player.IsPlaying())
            {
                if (player.PlayList.Count > 1)
                {
                    switch(player.loop)
                    {
                        case LOOP_MODE.LOOP_NONE:
                            if (nowplaying && playingIndex > -1 && playingIndex < player.PlayList.Count - 1)
                            {
                                playingIndex++;
                                PlayLoad();
                            }
                            break;
                        case LOOP_MODE.LOOP_ONE_REPEAT:
                            if (nowplaying)
                               PlayLoad();
                            break;
                        case LOOP_MODE.LOOP_ALL:
                            if (nowplaying)
                            {
                                if (playingIndex > -1 && playingIndex < player.PlayList.Count - 1)
                                {
                                    playingIndex++;
                                    PlayLoad();
                                }
                                else
                                {
                                    if (playingIndex == player.PlayList.Count - 1)
                                        playingIndex = 0;
                                    PlayLoad();
                                }
                            }
                            break;
                    }
                }
            }

            TimeSpan time1 = TimeSpan.FromMilliseconds(SldTrack.Value);
            TimeSpan time2 = TimeSpan.FromMilliseconds(SldTrack.Maximum);
            LabelTime.Value.Text = time1.ToString(@"mm\:ss") + "/" + time2.ToString(@"mm\:ss");

            if (player.lastError != "" && player.lastErrCode != FMOD.RESULT.OK)
            {
                LabelTitle.Value.Text = player.lastErrFunction + " - " + player.lastError;
            }
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
		private const int SeekStep = 1000;       // 1回あたりのシーク量（ミリ秒）
		private const int SeekMaxValue = 10000;  // 加速の上限（ミリ秒）

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
            switch(player.loop)
            {
                case LOOP_MODE.LOOP_NONE:
                    // 背景画像を元画像へ変更
                    ((Button)sender).BackgroundImage = ((ButtonComponents)oldSkinSystem[((Button)sender).Name]).BackImage;
                    break;
                case LOOP_MODE.LOOP_ONE_REPEAT:
                    ((Button)sender).BackgroundImage = ((ButtonComponents)oldSkinSystem[((Button)sender).Name]).DownImage;
                    break;
                case LOOP_MODE.LOOP_ALL:
                    ((Button)sender).BackgroundImage = ((ButtonComponents)oldSkinSystem[((Button)sender).Name]).OptionalImage;
                    break;

            }
            ((Button)sender).Refresh();
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
            if (this.IsDisposed || OpenFileDialog == null)
                return;

            try
            {
                if (OpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    OpenFile(OpenFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ファイルのオープンに失敗しました。\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (playingIndex < player.PlayList.Count)
                    PlayLoad();
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
            nowplaying = false;
        }


        /// <summary>
        /// 閉じるボタンのクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            playListForm.Close();
            playListForm.Dispose();
            optionsForm.Close();
            optionsForm.Dispose();
			cdForm.Close();      // 追加
			cdForm.Dispose();    // 追加
								 // 終了
			Close();
        }
        private void BtnBack_Click(object sender, EventArgs e)
        {
            // ループ無し：最初の曲まで減算
            // １曲ループ：最初の曲まで減算
            // 全曲ループ：最初の曲まで減算、最初の曲から最後の曲へ戻る
            switch(player.loop)
            {
                case LOOP_MODE.LOOP_NONE:
                case LOOP_MODE.LOOP_ONE_REPEAT:
                    if (playingIndex > 0)
                        playingIndex--;
                    break;
                case LOOP_MODE.LOOP_ALL:
                    if (playingIndex > 0)
                        playingIndex--;
                    else
                        playingIndex = player.PlayList.Count - 1;
                    break;

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
            // ループ無し：最後の曲まで加算
            // １曲ループ：最後の曲まで加算
            // 全曲ループ：最後の曲まで加算、最後の曲から最初の曲へ戻る
            switch (player.loop)
            {
                case LOOP_MODE.LOOP_NONE:
                case LOOP_MODE.LOOP_ONE_REPEAT:
                    if (playingIndex < player.PlayList.Count - 1)
                        playingIndex++;
                    break;
                case LOOP_MODE.LOOP_ALL:
                    if (playingIndex < player.PlayList.Count - 1)
                        playingIndex++;
                    else
                        playingIndex = 0;
                    break;
            }
            PlayLoad();
        }
        private void BtnRandom_Click(object sender, EventArgs e)
        {
        }
        private void BtnLoop_Click(object sender, EventArgs e)
        {
            switch(player.loop)
            {
                case LOOP_MODE.LOOP_NONE:
                    player.loop = LOOP_MODE.LOOP_ONE_REPEAT;
                    break;
                case LOOP_MODE.LOOP_ONE_REPEAT:
                    player.loop = LOOP_MODE.LOOP_ALL;
                    break;
                case LOOP_MODE.LOOP_ALL:
                    player.loop = LOOP_MODE.LOOP_NONE;
                    break;
            }
            ((Button)sender).BackgroundImage = ((ButtonComponents)oldSkinSystem[((Button)sender).Name]).DownImage;
            ((Button)sender).Refresh();
        }
        private void BtnSetting_Click(object sender, EventArgs e)
        {
            optionsForm.Show();
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
            float pan = ((float)SldPan.Value) / 10f;
            player.SetPan(pan);
            config.settings.Pan = SldPan.Value;
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
            config.settings.Volume = SldVolume.Value;
            _toolTip.Hide(this);
        }

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
        #endregion

        private void SeekiTimer_Tick(object sender, EventArgs e)
        {
			if (seeking == 0) return;

			// seekValueを増加させるが上限を設ける
			seekValue = Math.Min(seekValue + SeekStep, SeekMaxValue);
			int newValue;
			if (seeking == 1)  // 早送り
			{
				newValue = SldTrack.Value + seekValue;
				// Maximumを超えないようにクランプ
				SldTrack.Value = Math.Min(newValue, SldTrack.Maximum);
			}
			else if (seeking == 2)  // 早戻し
			{
				newValue = SldTrack.Value - seekValue;
				// 0を下回らないようにクランプ
				SldTrack.Value = Math.Max(newValue, SldTrack.Minimum);
			}
        }

        private void Spectrum_Click(object sender, EventArgs e)
        {
            Spectrum.Mode = (Spectrum.Mode + 1) % 3;
        }

        private void BtnCD_Click(object sender, EventArgs e)
        {
			cdForm.Show();
		}

        private void BtnCD_MouseDown(object sender, MouseEventArgs e)
        {
            BtnDownEvent(ref sender);
        }

        private void BtnCD_MouseUp(object sender, MouseEventArgs e)
        {
            BtnUpEvent(ref sender);
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            //コントロール内にドロップされたとき実行される
            //ドロップされたすべてのファイル名を取得する
            string[] fileName =
                (string[])e.Data.GetData(DataFormats.FileDrop, false);

            int idx = 0;
            int temp = 0;
            foreach(string file in fileName)
            {
                // 最初の1曲目
                if (idx++ == 0)
                {
                    // 再生中ではない場合
                    if (!player.IsPlaying())
                    {
                        // 最初の１つはOpen=>Play処理を行う
                        OpenFile(file);
                        continue;
                    }
                }
                // 後はOpenのみでプレイリストへ追加
                player.CreateSound(file, out temp);
            }
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            //コントロール内にドラッグされたとき実行される
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                //ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
                e.Effect = DragDropEffects.Copy;
            else
                //ファイル以外は受け付けない
                e.Effect = DragDropEffects.None;
        }
    }
}
