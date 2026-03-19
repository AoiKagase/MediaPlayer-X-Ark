using MediaPlayer_X_Ark.Engine;
using MediaPlayer_X_Ark.Forms;
using MediaPlayer_X_Ark.Skin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark
{
    public partial class MainForm : Form
    {
        bool initialize = false;

        public static IPlayerEngine player;
        private static IConfigService config;

        private ToolTip _toolTip;

        private PlayListForm playListForm;
        private OptionsForm optionsForm;
        private CDForm cdForm;

        private int _sleepTimerRemaining = 0; // 残り秒数（0=無効）

        private ISkinSystem _currentSkin;
        public ISkinSystem CurrentSkin => _currentSkin;

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// スキンロード
        /// 設定ファイルからスキンファイルパスを取得して投げる
        /// </summary>
        /// <param name="skinFile"></param>
        public void SkinLoad(string skinFile)
        {
            using (var pkg = SkinPackage.Open(skinFile))
            {
                if (pkg.Format == SkinPackage.SkinFormat.NewXsk)
                {
                    // 新形式
                    var skin = new NewSkinSystem();
                    skin.Open(pkg.DefinitionPath);
                    _currentSkin = skin;
                }
                else
                {
                    // 旧形式はOldSkinSystem自身がパス解決するため
                    // 元のパス（相対パス）をそのまま渡す
                    var skin = new OldSkinSystem();
                    skin.Open(pkg.OriginalPath);
                    _currentSkin = skin;
                }
                ApplySkin(_currentSkin);
                // プレビュー用メイン画像パスを保存
                config.settings.Skin = skinFile;

                // ボリューム最大値を強制100（旧形式スキンはこの数値を変動出来ていた為、処理簡略化を考慮する）
                SldVolume.Maximum = 150;
                SldVolume.Value = config.settings.Volume;
            }
        }
        /// <summary>
        /// スキンデータをフォームに適用する。新旧形式共通。
        /// </summary>
        private void ApplySkin(ISkinSystem skin)
        {
            // メインフォーム
            BackgroundImage = skin.MainForm.BackImage;
            TransparencyKey = skin.MainForm.TransparentKey;
            Width = skin.MainForm.Position.Width;
            Height = skin.MainForm.Position.Height;

            // スペクトラム
            // AutoScaleModeの影響を排除するためSuspendLayout/ResumeLayoutで囲む
            SuspendLayout();
            Spectrum.Left = skin.ImgSpectrum.Position.Left;
            Spectrum.Top = skin.ImgSpectrum.Position.Top;
            Spectrum.Width = skin.ImgSpectrum.Position.Width;
            Spectrum.Height = skin.ImgSpectrum.Position.Height;
            ResumeLayout(false);

            // サイズ変更後にビットマップを新サイズで再作成
            Spectrum.BitmapSnow = new Bitmap(skin.ImgSpectrum.Position.Width, skin.ImgSpectrum.Position.Height);
            Spectrum.BitmapWave = new Bitmap(skin.ImgSpectrum.Position.Width, skin.ImgSpectrum.Position.Height);
            Spectrum.BitmapBackground = new Bitmap(skin.ImgSpectrum.Position.Width, skin.ImgSpectrum.Position.Height);

            if (skin.ImgSpectrum.Image != null)
            {
                Spectrum.BitmapSpectrum = new Bitmap(skin.ImgSpectrum.Image);
            }
            else
            {
                Spectrum.BitmapSpectrum = new Bitmap(skin.ImgSpectrum.Position.Width, skin.ImgSpectrum.Position.Height);
                using (var g = Graphics.FromImage(Spectrum.BitmapSpectrum))
                    g.Clear(skin.ImgSpectrum.Color);
                using (var g = Graphics.FromImage(Spectrum.BitmapSnow))
                    g.Clear(skin.ImgSpectrum.Color);
                using (var g = Graphics.FromImage(Spectrum.BitmapWave))
                    g.Clear(skin.ImgSpectrum.Color);
            }
            // スペクトラム背景画像の設定
            // メインフォームの背景画像からスペクトラム領域を切り出す
            if (skin.MainForm.BackImage != null)
            {
                var rect = new Rectangle(
                    skin.ImgSpectrum.Position.Left,
                    skin.ImgSpectrum.Position.Top,
                    skin.ImgSpectrum.Position.Width,
                    skin.ImgSpectrum.Position.Height);

                var bmp = new Bitmap(rect.Width, rect.Height);
                using (var g = Graphics.FromImage(bmp))
                    g.DrawImage(skin.MainForm.BackImage,
                        new Rectangle(0, 0, rect.Width, rect.Height),
                        rect,
                        GraphicsUnit.Pixel);

                Spectrum.BitmapBackground = bmp;
            }
            else
            {
                Spectrum.BitmapBackground = null;
            }

            // コントロール名 → スキンプロパティのマッピング
            var buttonMap = new Dictionary<string, ButtonComponents>
            {
                { "BtnOpen",        skin.BtnOpen        },
                { "BtnClose",       skin.BtnClose       },
                { "BtnPlay",        skin.BtnPlay        },
                { "BtnStop",        skin.BtnStop        },
                { "BtnBack",        skin.BtnBack        },
                { "BtnSeekBack",    skin.BtnSeekBack    },
                { "BtnPause",       skin.BtnPause       },
                { "BtnSeekForward", skin.BtnSeekForward },
                { "BtnNext",        skin.BtnNext        },
                { "BtnRandom",      skin.BtnRandom      },
                { "BtnLoop",        skin.BtnLoop        },
                { "BtnSetting",     skin.BtnSetting     },
                { "BtnPlaylist",    skin.BtnPlaylist    },
                { "BtnMinisize",    skin.BtnMinisize    },
                { "BtnCD",          skin.BtnCD          },
                    };

            var sliderMap = new Dictionary<string, SliderComponents>
            {
                { "SldVolume", skin.SldVolume },
                { "SldPan",    skin.SldPan   },
                { "SldTrack",  skin.SldTrack },
            };

            var labelMap = new Dictionary<string, GraphicComponents>
            {
                { "LabelTitle", skin.LabelTitle },
                { "LabelTime",  skin.LabelTime  },
            };

            foreach (Control c in Controls)
            {
                string cName = c.Name;

                if (c is Button btn && buttonMap.TryGetValue(cName, out var bc))
                {
                    if (bc.BackImage == null || !bc.Enabled)
                    {
                        // 定義なし or 無効のボタンは非表示
                        btn.Visible = false;
                        btn.Enabled = false;
                        continue;
                    }
                    btn.AutoSize = false;
                    btn.BackgroundImage = bc.BackImage;
                    btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
                    btn.Top = bc.Position.Top;
                    btn.Left = bc.Position.Left;
                    btn.Width = bc.Position.Width;
                    btn.Height = bc.Position.Height;
                    btn.Enabled = bc.Enabled;
                    btn.Visible = bc.Enabled;
                    btn.Refresh();
                }
                else if (c is CustomSlider slider && sliderMap.TryGetValue(cName, out var sc))
                {
                    if (sc.SliderImage == null) continue;
                    slider.SliderImage = sc.SliderImage;
                    slider.Orientation = sc.Orientation;
                    slider.Minimum = sc.Minimum;
                    slider.Maximum = sc.Maximum;
                    slider.Top = sc.Position.Top;
                    slider.Left = sc.Position.Left;
                    slider.Width = sc.Position.Width;
                    slider.Height = sc.Position.Height;
                    slider.Enabled = sc.Enabled;
                    slider.Visible = sc.Enabled;
                    slider.Value = 0;
                    slider.Refresh();
                }
                else if (c is ScrollLabel lbl && labelMap.TryGetValue(cName, out var gc))
                {
                    lbl.BackColor = Color.Transparent;
                    lbl.Value.Font = gc.Font;
                    lbl.Value.ForeColor = gc.FontColor;
                    lbl.Top = gc.Position.Top;
                    lbl.Left = gc.Position.Left;
                    lbl.Width = gc.Position.Width;
                    lbl.Height = gc.Position.Height;
                    lbl.Enabled = gc.Enabled;
                    lbl.Visible = gc.Enabled;

                    // 内部Labelのサイズをリセット
                    lbl.Value.Left = 0;
                    lbl.Value.Width = gc.Position.Width;
                    lbl.Value.Height = gc.Position.Height;

                    // スクロール設定
                    lbl.ScrollEnable = gc.ScrollEnable;
                    lbl.Timer.Interval = gc.Interval > 0 ? gc.Interval : 100;
                    lbl.Timer.Enabled = gc.Interval > 0;
                }
            }

            this.Refresh();

            // プレイリストフォーム
            playListForm.Left = Left - skin.PlayListForm.Position.Left;
            playListForm.Top = Top - skin.PlayListForm.Position.Top;
            playListForm.BackgroundImage = skin.PlayListForm.BackImage;
            playListForm.Width = skin.PlayListForm.Position.Width;
            playListForm.Height = skin.PlayListForm.Position.Height;
            playListForm.TransparencyKey = skin.PlayListForm.TransparentKey;
            playListForm.Refresh();

            var plButtonMap = new Dictionary<string, ButtonComponents>
            {
                { "PBtnOpen",   skin.PBtnOpen   },
                { "PBtnSave",   skin.PBtnSave   },
                { "PBtnRemove", skin.PBtnRemove },
                { "PBtnUp",     skin.PBtnUp     },
                { "PBtnDown",   skin.PBtnDown   },
                { "PBtnClose",  skin.PBtnClose  },
                { "PBtnClear",  skin.PBtnClear  },
            };

            foreach (Control c in playListForm.Controls)
            {
                string cName = c.Name;

                if (c is DataGridView grid)
                {
                    grid.BackgroundColor = skin.PlayListGrid.ListBackColor;
                    grid.RowsDefaultCellStyle.BackColor = skin.PlayListGrid.ListBackColor;
                    grid.RowsDefaultCellStyle.ForeColor = skin.PlayListGrid.ListForeColor;
                    grid.ForeColor = skin.PlayListGrid.ListForeColor;
                    grid.Left = skin.PlayListGrid.ListPosition.Left;
                    grid.Top = skin.PlayListGrid.ListPosition.Top;
                    grid.Width = skin.PlayListGrid.ListPosition.Width;
                    grid.Height = skin.PlayListGrid.ListPosition.Height;
                }
                else if (c is Button btn && plButtonMap.TryGetValue(cName, out var bc))
                {
                    if (bc.BackImage == null || !bc.Enabled)
                    {
                        btn.Visible = false;
                        btn.Enabled = false;
                        continue;
                    }
                    btn.AutoSize = false;
                    btn.BackgroundImage = bc.BackImage;
                    btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
                    btn.Top = bc.Position.Top;
                    btn.Left = bc.Position.Left;
                    btn.Width = bc.Position.Width;
                    btn.Height = bc.Position.Height;
                    btn.Enabled = bc.Enabled;
                    btn.Visible = bc.Enabled;
                    btn.Refresh();
                }
            }
        }
        /// <summary>
        /// ファイルを開く
        /// </summary>
        /// <param name="fileName"></param>
        public void OpenFile(string fileName)
        {
            int idx;
            // Open File
            if (player.CreateSound(fileName, out idx) == FMOD.RESULT.OK)
                PlayLoad(idx);
        }

        /// <summary>
        /// Indexを指定して再生する。(主にプレイリストから直接再生)
        /// </summary>
        /// <param name="index"></param>
        public void PlayLoad(int index)
        {
            player.SetDevice(config.settings.Device);
            player.PlaySound(index);
            UpdateTrackUI();
        }
        private void PlayLoad() => PlayLoad(player.PlayingIndex);

        /// <summary>
        /// ボタンクリック時のイベント（MouseDown時）
        /// </summary>
        /// <param name="button"></param>
        public void BtnDownEvent(ref object button)
        {
            var btn = (Button)button;
            if (_currentSkin == null) return;
            try
            {
                // コントロール名からスキンデータを取得
                var map = GetButtonMap();
                if (map.TryGetValue(btn.Name, out var bc))
                    btn.BackgroundImage = bc.DownImage;
            }
            catch { }
            btn.Refresh();
        }
        /// <summary>
        /// ボタンクリック時のイベント（MouseUp時）
        /// </summary>
        /// <param name="button"></param>
        public void BtnUpEvent(ref object button)
        {
            var btn = (Button)button;
            if (_currentSkin == null) return;
            try
            {
                var map = GetButtonMap();
                if (map.TryGetValue(btn.Name, out var bc))
                    btn.BackgroundImage = bc.BackImage;
            }
            catch { }
            btn.Refresh();
        }
        private Dictionary<string, ButtonComponents> GetButtonMap()
        {
            return new Dictionary<string, ButtonComponents>
            {
                { "BtnOpen",        _currentSkin.BtnOpen        },
                { "BtnClose",       _currentSkin.BtnClose       },
                { "BtnPlay",        _currentSkin.BtnPlay        },
                { "BtnStop",        _currentSkin.BtnStop        },
                { "BtnBack",        _currentSkin.BtnBack        },
                { "BtnSeekBack",    _currentSkin.BtnSeekBack    },
                { "BtnPause",       _currentSkin.BtnPause       },
                { "BtnSeekForward", _currentSkin.BtnSeekForward },
                { "BtnNext",        _currentSkin.BtnNext        },
                { "BtnRandom",      _currentSkin.BtnRandom      },
                { "BtnLoop",        _currentSkin.BtnLoop        },
                { "BtnSetting",     _currentSkin.BtnSetting     },
                { "BtnPlaylist",    _currentSkin.BtnPlaylist    },
                { "BtnMinisize",    _currentSkin.BtnMinisize    },
                { "BtnCD",          _currentSkin.BtnCD          },
                { "PBtnOpen",       _currentSkin.PBtnOpen       },
                { "PBtnSave",       _currentSkin.PBtnSave       },
                { "PBtnRemove",     _currentSkin.PBtnRemove     },
                { "PBtnUp",         _currentSkin.PBtnUp         },
                { "PBtnDown",       _currentSkin.PBtnDown       },
                { "PBtnClose",      _currentSkin.PBtnClose      },
                { "PBtnClear",      _currentSkin.PBtnClear      },
            };
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
            notifyIcon.Visible = false;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            notifyIcon.Icon = this.Icon;
            this.KeyPreview = true;

            // FMODサウンドエンジン
            var engine = new PlayerEngine();
            player = engine;
            // ① 設定を先に読み込む
            config = new Configuration(engine);

            // ② OutputType と SoftwareFormat は init() より前に設定
            player.SetOutputTypeBeforeInit(config.GetOutputType());

            // ③ init() を実行
            player.Initialize(config.settings.Buffer);

            // ④ Device は init() 後でOK
            player.SetDevice(config.settings.Device);

            // ★曲終了イベント購読
            player.TrackEnded += (s, e) =>
            {
                this.BeginInvoke((Action)(() =>
                {
                    if (!player.NowPlaying) return;
                    player.PlayNext();
                    UpdateTrackUI();
                }));
            };

            playListForm = new PlayListForm(this);
            playListForm.Owner = this;

            optionsForm = new OptionsForm(player, config, this);
            cdForm = new CDForm(this);

            // 予定：設定ファイルの読み込み スキンファイルの指定も含む
            // 旧形式（XSF）のスキンファイルの場合はOldSkinSystem
            // 新形式（JSON）の場合はNewSkinSystemへインスタンス切替
            // スキンロード
            SkinLoad(config.settings.Skin);
            Spectrum.Initialize();

            InitContextMenu();

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

        private void UpdateTrackUI()
        {
            int index = player.PlayingIndex;
            if (index < 0 || index >= player.PlayList.Count) return;

            SldTrack.Maximum = (int)player.GetLength(index);
            float volume = ((float)SldVolume.Value) / 100f;
            player.SetVolume(volume);
            float pan = ((float)SldPan.Value) / 10f;
            player.SetPan(pan);

            player.GetTags(index);
            var item = player.PlayList[index];
            LabelTitle.Value.Text = (!string.IsNullOrEmpty(item.Title)) ? item.Title : Path.GetFileName(item.FileName);
            LabelTitle.Value.Text += (!string.IsNullOrEmpty(item.Artist)) ? (" - " + item.Artist) : "";
            LabelTitle.Value.Text += (!string.IsNullOrEmpty(item.Album)) ? (" - " + item.Album) : "";
        }

        /// <summary>
        /// 本体ドラッグによるウィンドウ移動
        /// </summary>
        private Point mousePoint;
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    BtnPlay_Click(sender, e);
                    break;
                case Keys.Enter:
                    if (player.PlayingIndex >= 0) PlayLoad(player.PlayingIndex);
                    break;
                case Keys.S:
                    BtnStop_Click(sender, e);
                    break;
                case Keys.B:
                    player.PlayNext();
                    UpdateTrackUI();
                    break;
                case Keys.Z:
                    player.PlayPrevious();
                    UpdateTrackUI();
                    break;
                case Keys.Right:
                    //                  player.SetPosition((uint)Math.Min(SldTrack.Value + SeekStep, SldTrack.Maximum));
                    e.Handled = true;
                    seeking = 1;

                    break;
                case Keys.Left:
                    //                  player.SetPosition((uint)Math.Max(SldTrack.Value - SeekStep, SldTrack.Minimum));
                    seeking = 2;
                    e.Handled = true;
                    break;
                case Keys.Up:
                    SldVolume.Value = Math.Min(SldVolume.Value + 5, SldVolume.Maximum);
                    player.SetVolume(((float)SldVolume.Value) / 100f);
                    e.Handled = true;
                    break;
                case Keys.Down:
                    SldVolume.Value = Math.Max(SldVolume.Value - 5, SldVolume.Minimum);
                    player.SetVolume(((float)SldVolume.Value) / 100f);
                    e.Handled = true;
                    break;
                case Keys.L:
                    BtnLoop_Click(sender, e);
                    break;
                case Keys.R:
                    SetPlayMode(LOOP_MODE.LOOP_RANDOM);
                    break;
                case Keys.Escape:
                    BtnMinisize_Click(sender, e);
                    break;
            }
        }
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
                // マグネットモードONのスキンの場合はドッキング位置に表示
                if (_currentSkin != null && _currentSkin.PlayListForm.MagnetMode)
                {
                    playListForm.Left = Left - _currentSkin.PlayListForm.Position.Left;
                    playListForm.Top = Top - _currentSkin.PlayListForm.Position.Top;
                }
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
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            SkinPackage.CleanupTempDirectory(); // 追加
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
            if (!initialize || player == null || player.spectrum == null) return;

            // スペクトラム画像の反映
            Spectrum.mFFT = player.spectrum.UpdateSpectrum();
            Spectrum.mWaveL = player.wave.GetWaveDataByChannel(0);
            Spectrum.mWaveR = player.wave.GetWaveDataByChannel(1);

            // 曲調トラックバーの反映 (シーク中はボタン側で動作する為動かさない)
            if (this.seekValue == 0)
                SldTrack.Value = (int)player.GetPosition();

            TimeSpan time1 = TimeSpan.FromMilliseconds(SldTrack.Value);
            TimeSpan time2 = TimeSpan.FromMilliseconds(SldTrack.Maximum);
            LabelTime.Value.Text = time1.ToString(@"mm\:ss") + "/" + time2.ToString(@"mm\:ss");

            if (player.lastError != "" && player.lastErrCode != FMOD.RESULT.OK)
            {
                LabelTitle.Value.Text = player.lastErrFunction + " - " + player.lastError;
            }


            if (_sleepTimerRemaining > 0)
            {
                _sleepTimerRemaining -= Timer.Interval;
                if (_sleepTimerRemaining <= 0)
                {
                    _sleepTimerRemaining = 0;
                    player.Stop();
                }
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
            switch (player.loop)
            {
                case LOOP_MODE.LOOP_NONE:
                    ((Button)sender).BackgroundImage = _currentSkin.BtnLoop.BackImage;
                    break;
                case LOOP_MODE.LOOP_ONE_REPEAT:
                    ((Button)sender).BackgroundImage = _currentSkin.BtnLoop.DownImage;
                    break;
                case LOOP_MODE.LOOP_ALL:
                    ((Button)sender).BackgroundImage = _currentSkin.BtnLoop.OptionalImage;
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
                if (player.PlayingIndex < player.PlayList.Count)
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
            player.PlayPrevious();
            UpdateTrackUI();
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
            player.PlayNext();
            UpdateTrackUI();
        }
        private void BtnRandom_Click(object sender, EventArgs e)
        {
        }
        private void BtnLoop_Click(object sender, EventArgs e)
        {
            switch (player.loop)
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
            ((Button)sender).BackgroundImage = _currentSkin.BtnLoop.DownImage;
            ((Button)sender).Refresh();
        }
        private void BtnSetting_Click(object sender, EventArgs e)
        {
            optionsForm.Show();
        }
        private void BtnPlaylist_Click(object sender, EventArgs e)
        {
            if (playListForm.Visible)
            {
                playListForm.Hide();
                return;
            }

            playListForm.Show(this);

            playListForm.Left = Left - _currentSkin.PlayListForm.Position.Left;
            playListForm.Top = Top - _currentSkin.PlayListForm.Position.Top;

        }
        private void BtnMinisize_Click(object sender, EventArgs e)
        {
            this.Hide();
            playListForm.Hide();
            notifyIcon.Visible = true;
        }
        // NotifyIcon ダブルクリックで復元
        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            if (player.PlayingIndex >= 0)
                playListForm.Show(this);
            notifyIcon.Visible = false;
            this.Activate();
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
            Spectrum.Mode = (Spectrum.Mode + 1) % 5;
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
            foreach (string file in fileName)
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

        private void InitContextMenu()
        {
            var menuSleep = new ToolStripMenuItem("スリープタイマー");
            var menuSleep15 = new ToolStripMenuItem("15分後");
            var menuSleep30 = new ToolStripMenuItem("30分後");
            var menuSleep60 = new ToolStripMenuItem("60分後");
            var menuSleepCancel = new ToolStripMenuItem("キャンセル");

            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(menuSleep);

            menuOpen.Click += (s, e) => BtnOpenFile_Click(s, e);
            menuUrlOpen.Click += (s, e) => BtnUrlOpen_Click(s, e);
            menuPlay.Click += (s, e) => BtnPlay_Click(s, e);
            menuPause.Click += (s, e) => BtnPause_Click(s, e);
            menuStop.Click += (s, e) => BtnStop_Click(s, e);
            menuBack.Click += (s, e) => BtnBack_Click(s, e);
            menuForward.Click += (s, e) => BtnSeekForward_Click(s, e);
            menuPlayList.Click += (s, e) => BtnPlaylist_Click(s, e);
            menuOption.Click += (s, e) => BtnSetting_Click(s, e);
            menuMinimize.Click += (s, e) => this.WindowState = FormWindowState.Minimized;
            menuExit.Click += (s, e) => Application.Exit();

            menuSleep15.Click += (s, e) => _sleepTimerRemaining = 15 * 60;
            menuSleep30.Click += (s, e) => _sleepTimerRemaining = 30 * 60;
            menuSleep60.Click += (s, e) => _sleepTimerRemaining = 60 * 60;
            menuSleepCancel.Click += (s, e) => _sleepTimerRemaining = 0;

            // PlayMode
            menuPlayModeNormal.Click += (s, e) => SetPlayMode(LOOP_MODE.LOOP_NONE);
            menuPlayModeRandom.Click += (s, e) => SetPlayMode(LOOP_MODE.LOOP_RANDOM);
            menuPlayModeRepeat.Click += (s, e) => SetPlayMode(LOOP_MODE.LOOP_ONE_REPEAT);
            menuPlayModeLoop.Click += (s, e) => SetPlayMode(LOOP_MODE.LOOP_ALL);
            menuSleep.DropDownItems.AddRange(new ToolStripItem[]
            {
                menuSleep15,
                menuSleep30,
                menuSleep60,
                new ToolStripSeparator(),
                menuSleepCancel,
            });

            // Effects / Equalizer / Extensions / SkinSelect は
            // OptionsForm の該当タブを開く形にする
            menuEffects.Click += (s, e) => OpenOptionsTab("PITCH");
            menuEqualizer.Click += (s, e) => OpenOptionsTab("GEQ");
            menuExtensions.Click += (s, e) => OpenOptionsTab("EXTENSIONS");
            menuSkinSelect.Click += (s, e) => OpenOptionsTab("SKIN");
            menuAbout.Click += (s, e) => OpenOptionsTab("ABOUT");

            // 開く前にチェック状態を更新
            contextMenu.Opening += ContextMenu_Opening;


            var trayMenuRestore = new ToolStripMenuItem("復元");
            var trayMenuExit = new ToolStripMenuItem("終了");

            trayMenuRestore.Click += (s, e) => NotifyIcon_DoubleClick(s, e);
            trayMenuExit.Click += (s, e) => Application.Exit();

            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                trayMenuRestore,
                new ToolStripSeparator(),
                trayMenuExit,
            });
        }
        private void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // PlayMode チェック状態を更新
            menuPlayModeRandom.Enabled = false; // 未実装
            menuPlayModeNormal.Checked = (player.loop & LOOP_MODE.LOOP_NONE) != 0;
            menuPlayModeRandom.Checked = (player.loop & LOOP_MODE.LOOP_RANDOM) != 0;
            menuPlayModeRepeat.Checked = (player.loop & LOOP_MODE.LOOP_ONE_REPEAT) != 0;
            menuPlayModeLoop.Checked = (player.loop & LOOP_MODE.LOOP_ALL) != 0;
        }

        private void SetPlayMode(LOOP_MODE mode)
        {
            if (mode == LOOP_MODE.LOOP_RANDOM)
            {
                // ランダムはトグル
                player.loop ^= LOOP_MODE.LOOP_RANDOM;
                if ((player.loop & LOOP_MODE.LOOP_RANDOM) != 0)
                    player.BuildShuffleQueue(); // ONになった時点で生成
            }
            else
            {
                // ランダムフラグを保持しつつ他のモードを切り替え
                bool isRandom = (player.loop & LOOP_MODE.LOOP_RANDOM) != 0;
                player.loop = mode;
                if (isRandom) player.loop |= LOOP_MODE.LOOP_RANDOM;
            }
        }

        private void OpenOptionsTab(string tabName)
        {
            optionsForm.Show();
            optionsForm.SelectTab(tabName);
        }

        private void BtnUrlOpen_Click(object sender, EventArgs e)
        {
            using (var form = new UrlInputForm())
            {
                if (form.ShowDialog(this) != DialogResult.OK) return;
                string url = form.Url;

                if (string.IsNullOrWhiteSpace(url)) return;
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    MessageBox.Show("URLはhttp://またはhttps://で始まる必要があります。",
                        "URL Open", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int index;
                var result = player.CreateSound(url, out index);
                if (result != FMOD.RESULT.OK)
                {
                    MessageBox.Show($"URLを開けませんでした。\n{player.lastError}",
                        "URL Open", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // ★失敗した場合はPlayListから削除
                    if (index >= 0 && index < player.PlayList.Count)
                        player.PlayList.RemoveAt(index);
                    return;
                }

                result = player.PlaySound(index);
                if (result != FMOD.RESULT.OK)
                {
                    MessageBox.Show($"URLを再生できませんでした。\n{player.lastError}",
                        "URL Open", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // ★失敗した場合はPlayListから削除
                    if (index >= 0 && index < player.PlayList.Count)
                        player.PlayList.RemoveAt(index);
                }
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                case Keys.Left:
                    this.seekValue = 0;
                    this.seeking = 0;
                    e.Handled = true;
                    break;
            }
        }

        private const int WM_APPCOMMAND = 0x0319;

        // メディアキーコマンド値
        private const int APPCOMMAND_MEDIA_PLAY_PAUSE = 14;
        private const int APPCOMMAND_MEDIA_STOP = 13;
        private const int APPCOMMAND_MEDIA_NEXTTRACK = 11;
        private const int APPCOMMAND_MEDIA_PREVIOUSTRACK = 12;
        //private const int APPCOMMAND_VOLUME_UP = 10;
        //private const int APPCOMMAND_VOLUME_DOWN = 9;
        //private const int APPCOMMAND_VOLUME_MUTE = 8;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_APPCOMMAND)
            {
                int command = (int)(m.LParam.ToInt64() >> 16) & 0xFFF;
                switch (command)
                {
                    case APPCOMMAND_MEDIA_PLAY_PAUSE:
                        BtnPlay_Click(this, EventArgs.Empty);
                        break;
                    case APPCOMMAND_MEDIA_STOP:
                        BtnStop_Click(this, EventArgs.Empty);
                        break;
                    case APPCOMMAND_MEDIA_NEXTTRACK:
                        player.PlayNext();
                        UpdateTrackUI();
                        break;
                    case APPCOMMAND_MEDIA_PREVIOUSTRACK:
                        player.PlayPrevious();
                        UpdateTrackUI();
                        break;
                    //case APPCOMMAND_VOLUME_UP:
                    //    SldVolume.Value = Math.Min(SldVolume.Value + 5, SldVolume.Maximum);
                    //    player.SetVolume(((float)SldVolume.Value) / 100f);
                    //    break;
                    //case APPCOMMAND_VOLUME_DOWN:
                    //    SldVolume.Value = Math.Max(SldVolume.Value - 5, SldVolume.Minimum);
                    //    player.SetVolume(((float)SldVolume.Value) / 100f);
                    //    break;
                    //case APPCOMMAND_VOLUME_MUTE:
                    //    player.SetVolume(0f);
                    //    break;
                }
                m.Result = (IntPtr)1; // 処理済みを通知
                return;
            }
            base.WndProc(ref m);
        }
    }
}