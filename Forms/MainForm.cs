using MediaPlayer_X_Ark.Engine;
using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using MediaPlayer_X_Ark.Forms;
using MediaPlayer_X_Ark.Skin;
using MediaPlayer_X_Ark.Skin.New;
using NFluidsynth;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark
{
	public partial class MainForm : Form
	{
		bool initialize = false;

		private IPlayerEngine _engine;
		private IConfigService _config;
		private PlayerController _controller;
		public PlayerController Controller => _controller;
		private ToolTip _toolTip;

		private PlayListForm _playListForm;
		private OptionsForm _optionsForm;
		private CDForm _cdForm;
		private FileInfoForm _fileInfoForm;
        private MiniPlayerForm _miniPlayerForm;

        private int _sleepTimerRemaining = 0; // 残り秒数（0=無効）

		private INewSkinSystem _currentSkin;
		private SkinApplicator _skinApplicator;

		public INewSkinSystem CurrentSkin => _currentSkin;

		// 用途別にOpenFileDialogを分離
		private OpenFileDialog _openFileDialogMedia;   // 音楽ファイル用
		private Bitmap _waveformBitmap = null;
		private PictureBox _waveformArea = null;  // target="area" 用
		private int _waveformRefreshCounter = 0;
		private readonly List<Form> _managedForms = new List<Form>();

		private int seekValue;
		private int seeking;
		private const int SeekStep = 1000;       // 1回あたりのシーク量（ミリ秒）
		private const int SeekMaxValue = 10000;  // 加速の上限（ミリ秒）

        private float _abStart = -1f;
        private float _abEnd = -1f;
        public MainForm()
		{
			InitializeComponent();
		}

		public void ApplyDisplaySettings()
		{
			Spectrum.Mode = _config.settings.DefaultSpectrumMode;
			Spectrum.SnowBlockEnabled = _config.settings.SnowBlockEnabled;
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
				//else
				//{
				//	// 旧形式はOldSkinSystem自身がパス解決するため
				//	// 元のパス（相対パス）をそのまま渡す
				//	var skin = new OldSkinSystem();
				//	skin.Open(pkg.OriginalPath);
				//	_currentSkin = skin;
				//}
				_skinApplicator = new SkinApplicator(_currentSkin);
				_skinApplicator.ApplyToMainForm(this, Spectrum);
				_skinApplicator.ApplyToPlayListForm(_playListForm);
                _skinApplicator.ApplyToFileInfoForm(_fileInfoForm);
                _skinApplicator.ApplyToMiniPlayerForm(_miniPlayerForm);

                SetupWaveformTarget(); // TODO: テスト表示
				// プレビュー用メイン画像パスを保存
				_config.settings.Skin = skinFile;

				// ボリューム最大値を強制100（旧形式スキンはこの数値を変動出来ていた為、処理簡略化を考慮する）
				SldVolume.Maximum = 150;
				SldVolume.Value = _config.settings.Volume;
			}
		}
		/// <summary>
		/// TODO: テスト表示用
		/// </summary>
		private void SetupWaveformTarget()
		{
			if (_waveformArea != null)
			{
				Controls.Remove(_waveformArea);
				_waveformArea.Dispose();
				_waveformArea = null;
			}

			// NewSkinSystem 側で null → new WaveformDef() にしたので
			// ここでは常に non-null が来る（OldSkinSystem は下の else で無効化）
			var wDef = (_currentSkin as NewSkinSystem)?.WaveForm;
			if (wDef == null)
			{
				_controller.Engine.WaveformEnabled = false;
				SldTrack.BackgroundImage = null;
				return;
			}

			_controller.Engine.WaveformEnabled = true;

			if (wDef.Target == "area" && wDef.Location.W > 0 && wDef.Location.H > 0)
			{
				_waveformArea = new PictureBox
				{
					Location = new Point(wDef.Location.X, wDef.Location.Y),
					Size = new System.Drawing.Size(wDef.Location.W, wDef.Location.H),
					BackColor = Color.Transparent,
					SizeMode = PictureBoxSizeMode.StretchImage,
				};
				Controls.Add(_waveformArea);
				_waveformArea.BringToFront();
			}
			// target="trackbar" の場合は _waveformArea = null のまま
			// → ApplyWaveformBitmap が SldTrack.BackgroundImage に描画
		}

		public void BtnMouseDown(object sender, MouseEventArgs e)
			=> _skinApplicator?.SetButtonDown((Button)sender);
		public void BtnMouseUp(object sender, MouseEventArgs e)
			=> _skinApplicator?.SetButtonUp((Button)sender);

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
			_engine = new PlayerEngine();
			// ① 設定を先に読み込む
			_config = new Configuration(_engine);

			// コントローラー生成
			_controller = new PlayerController(_engine, _config);
			_controller.TrackChanged += OnTrackChanged;
			_controller.PlaybackStateChanged += OnPlaybackStateChanged;
			_controller.WaveformReady += OnWaveformReady;
			_playListForm = new PlayListForm(this, _controller, _config);
			_optionsForm = new OptionsForm(this, _controller, _config);
			_cdForm = new CDForm(this, _controller, _config);
			_fileInfoForm = new FileInfoForm(this, _controller);
            _miniPlayerForm = new MiniPlayerForm(this, _controller);
            
			// ★管理リストに追加
            _managedForms.Add(_fileInfoForm);
            _managedForms.Add(_playListForm);
			_managedForms.Add(_optionsForm);
			_managedForms.Add(_cdForm);
            _managedForms.Add(_miniPlayerForm);
            // 予定：設定ファイルの読み込み スキンファイルの指定も含む
            // 旧形式（XSF）のスキンファイルの場合はOldSkinSystem
            // 新形式（JSON）の場合はNewSkinSystemへインスタンス切替
            // スキンロード
            SkinLoad(_config.settings.Skin);
			Spectrum.Initialize();
			Spectrum.Mode = _config.settings.DefaultSpectrumMode;
			Spectrum.SnowBlockEnabled = _config.settings.SnowBlockEnabled;
			SetMouseDownEvent();

            _controller.ErrorOccurred += (s, e) =>
            {
                if (!e.IsOk)
                    LabelTitle.Value.Text = e.ToString();
            };

            InitContextMenu();

			_openFileDialogMedia = new OpenFileDialog();
			_openFileDialogMedia.Filter = "音楽ファイル|*.mp3;*.flac;*.ogg;*.wav;*.aac;*.m4a;*.wma;*.mid;*.mod;*.xm;*.it;*.s3m|すべてのファイル|*.*";
			_openFileDialogMedia.Multiselect = true;

			this.TopMost = _config.settings.AlwaysOnTop;
			initialize = true;

			// 起動パラメータを取得し、ファイルパスが取得出来るならばOpen関数へ引き渡す
			string[] parameters = System.Environment.GetCommandLineArgs();
			if (parameters.Length > 1)
			{
				if (File.Exists(parameters[1]))
				{
					_controller.OpenAndPlay(parameters[1]);
				}
			}
		}
		private void OnPlaybackStateChanged() { }
		private void OnTrackChanged(int index)
		{
            _abStart = -1f;
            _abEnd = -1f;
            _controller.ClearAbRepeat();

            SldTrack.Maximum = (int)_controller.GetLength();
			SldTrack.Value = 0;
			_controller.SetVolume(SldVolume.Value);
			_controller.SetPan(SldPan.Value);

			if (_fileInfoForm != null && _fileInfoForm.Visible)
				_fileInfoForm.LoadInfo();

			LabelTitle.Value.Text = _controller.BuildTitleText();

			_waveformBitmap?.Dispose();
			_waveformBitmap = null;
			if (_waveformArea != null) _waveformArea.Image = null;
			else SldTrack.BackgroundImage = null;
			// ── 追加：解析済みなら即再描画 ──
			var entry = _controller.Engine.PlayList[index];
			if (_controller.Engine.WaveformEnabled && entry.WaveformReady)
				UpdateWaveformBitmap(index);
		}
		private void OnWaveformReady(int index)
		{
			// _controller.WaveformReady は常にUIスレッドで発火するので
			// InvokeRequired チェック不要
			if (!_controller.Engine.WaveformEnabled) return;
			if (index != _controller.Engine.PlayingIndex) return;
			UpdateWaveformBitmap(index);
		}

		private void UpdateWaveformBitmap(int index)
		{
			var wDef = (_currentSkin)?.WaveForm;
			if (wDef == null) return;  // スキン未定義なら何もしない

			var entry = _controller.Engine.PlayList[index];
			var (w, h) = GetWaveformSize(wDef);
			var newBmp = WaveformRenderer.Render(
				ApplyExponent(entry.WaveformL, wDef.Exponent),
				ApplyExponent(entry.WaveformR, wDef.Exponent),
				w, h, playedRatio: 0f,
				mode: ParseWaveformMode(wDef.Mode),
				colors: BuildWaveformColors(wDef));

			_waveformBitmap?.Dispose();
			_waveformBitmap = newBmp;
			ApplyWaveformBitmap(newBmp, wDef.Target);
		}

		/// <summary>
		/// 本体ドラッグによるウィンドウ移動
		/// </summary>
		private Point mousePoint;
		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Right:
					seeking = 1;
					e.Handled = true;
					break;
				case Keys.Left:
					seeking = 2;
					e.Handled = true;
					break;
			}

		}

		/// <summary>
		/// キーボードショートカットの中核処理。
		/// ProcessCmdKey は WinForms の処理より前に発火するため
		/// Up/Down/Left/Right/Space などナビゲーションキーも確実に捕捉できる。
		/// </summary>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			// テキスト入力中のコントロールがフォーカスを持つ場合は素通し
			// （将来的に TextBox などを追加した場合の保険）
			if (ActiveControl is TextBox || ActiveControl is ComboBox)
				return base.ProcessCmdKey(ref msg, keyData);

			switch (keyData)
			{
				// ── 再生制御 ─────────────────────────────────────────────

				case Keys.Space:
					// Space: 再生/一時停止トグル
					BtnPlay_Click(this, EventArgs.Empty);
					return true;

				case Keys.Enter:
					// Enter: 現在曲を先頭から再生
					if (_controller.IsPlaying)
						_controller.SetPosition(0);
					else
						_controller.PlayAt(0);
					return true;

				case Keys.S:
					// S: 停止
					BtnStop_Click(this, EventArgs.Empty);
					return true;

				case Keys.X:
					// B: 次の曲
					_controller.PlayNext();
					return true;

				case Keys.Z:
					// Z: 前の曲
					_controller.PlayPrevious();
					return true;

				// ── シーク（キーリピートによる加速は SeekiTimer と seeking フラグで実装）──

				case Keys.Right:
					// Right: 早送り開始（キーリピートは KeyDown → SeekiTimer で継続）
					seeking = 1;
					return true;

				case Keys.Left:
					// Left: 早戻し開始
					seeking = 2;
					return true;

				// ── 音量 ──────────────────────────────────────────────────

				case Keys.Up:
					// Up: 音量+5
					SldVolume.Value = Math.Min(SldVolume.Value + 5, SldVolume.Maximum);
					_controller.SetVolume(SldVolume.Value);
					return true;

				case Keys.Down:
					// Down: 音量-5
					SldVolume.Value = Math.Max(SldVolume.Value - 5, SldVolume.Minimum);
					_controller.SetVolume(SldVolume.Value);
					return true;

				// ── モード切替 ────────────────────────────────────────────

				case Keys.L:
					// L: ループモード切替
					BtnLoop_Click(BtnLoop, EventArgs.Empty);
					return true;

				case Keys.R:
					// R: ランダムモード切替
					BtnRandom_Click(BtnRandom, EventArgs.Empty);
					return true;

				// ── UI ────────────────────────────────────────────────────

				case Keys.Escape:
					// Escape: ミニモード（タスクトレイへ）
					BtnMinisize_Click(this, EventArgs.Empty);
					return true;

                case Keys.A:
                    // A点セット
                    _abStart = (float)_controller.GetPosition()
                             / Math.Max(1f, _controller.GetLength());
                    _controller.SetAbStart((uint)_controller.GetPosition());
                    return true;

                case Keys.B:
                    // B点セット（A点より後のみ有効）
                    if (_controller.AbRepeatEnabled == false
                        || (uint)_controller.GetPosition() > _controller.AbStart)
                    {
                        _abEnd = (float)_controller.GetPosition()
                               / Math.Max(1f, _controller.GetLength());
                        _controller.SetAbEnd((uint)_controller.GetPosition());
                    }
                    return true;

                case Keys.C:
                    // AB リピートクリア
                    _abStart = -1f;
                    _abEnd = -1f;
                    _controller.ClearAbRepeat();
                    return true;
            }

			return base.ProcessCmdKey(ref msg, keyData);
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

				// ★関連フォームを前面に
				foreach (var form in _managedForms)
					if (form != null && !form.IsDisposed && form.Visible)
						form.BringToFront();
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
			if (SuppressNextMouseDown)
			{
				SuppressNextMouseDown = false;
				return;
			}
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				Left += e.X - mousePoint.X;
				Top += e.Y - mousePoint.Y;

				var plForm = _currentSkin?.SubForms["PlayListForm"];
				if (plForm != null)
				{
					if (plForm.MagnetMode)
					{
						// Main + Offset
						_playListForm.Left = Left + plForm.Position.Left;
						_playListForm.Top = Top + plForm.Position.Top;
					}
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
			_controller.Close();
			_config.Save();
			_fileInfoForm?.Dispose();
			_cdForm?.Dispose();   // 追加
            _miniPlayerForm?.Dispose();
            _engine.Dispose();  // 明示的に解放
			_engine = null;
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
			if (!initialize || _engine == null || _engine.spectrum == null) return;

            // NonStopMix 切替直前は高精度タイマーに切り替え
            int desiredInterval = _controller.TimerPrecisionRequested ? 10 : 100;
            if (Timer.Interval != desiredInterval)
                Timer.Interval = desiredInterval;

            // スペクトラム画像の反映
            Spectrum.mFFT = _engine.spectrum.UpdateSpectrum();
			Spectrum.mWaveL = _engine.wave.GetWaveDataByChannel(0);
			Spectrum.mWaveR = _engine.wave.GetWaveDataByChannel(1);

			// 曲調トラックバーの反映 (シーク中はボタン側で動作する為動かさない)
			if (this.seekValue == 0)
				SldTrack.Value = (int)_controller.GetPosition();

			TimeSpan time1 = TimeSpan.FromMilliseconds(SldTrack.Value);
			TimeSpan time2 = TimeSpan.FromMilliseconds(SldTrack.Maximum);
			LabelTime.Value.Text = time1.ToString(@"mm\:ss") + "/" + time2.ToString(@"mm\:ss");

			if (_sleepTimerRemaining > 0)
			{
				_sleepTimerRemaining -= Timer.Interval;
				if (_sleepTimerRemaining <= 0)
				{
					_sleepTimerRemaining = 0;
					_controller.Stop();
					UpdateSleepTimerMenu(null);
				}
			}
			_waveformRefreshCounter += Timer.Interval;
			if (_waveformRefreshCounter >= 60 && _waveformBitmap != null)
			{
				_waveformRefreshCounter = 0;
				float ratio = (float)_controller.GetPosition()
							/ Math.Max(1, _controller.GetLength());
				UpdateWaveformPlayedRatio(ratio);
			}
			// ── 曲終了検知（クロスフェード対応版）──────────────────────
			_controller.OnTimerTick(Timer.Interval);
		}
		private void UpdateWaveformPlayedRatio(float ratio)
		{
			var wDef = (_currentSkin)?.WaveForm;
			if (wDef == null) return;  // スキン未定義なら何もしない

			if (_controller.PlayingIndex < 0) return;
			var entry = _controller.Engine.PlayList[_controller.PlayingIndex];
			if (!entry.WaveformReady) return;

			var (w, h) = GetWaveformSize(wDef);

			// ABリピート範囲（未実装時は -1）
            float abStart = _abStart;
            float abEnd = _abEnd;

            var newBmp = WaveformRenderer.Render(
				 ApplyExponent(entry.WaveformL, wDef.Exponent),
				 ApplyExponent(entry.WaveformR, wDef.Exponent),
				 w, h, ratio,
				 mode: ParseWaveformMode(wDef.Mode),
				 colors: BuildWaveformColors(wDef),
				 abStart: abStart, abEnd: abEnd);

			var old = _waveformBitmap;
			_waveformBitmap = newBmp;
			ApplyWaveformBitmap(newBmp, wDef.Target);
			old?.Dispose();
		}

		private (int w, int h) GetWaveformSize(WaveformComponents wDef)
		{
			if (wDef.Target == "area" && _waveformArea != null)
				return (_waveformArea.Width, _waveformArea.Height);
			return (SldTrack.Width, SldTrack.Height);
		}

		private static WaveformRenderer.WaveformMode ParseWaveformMode(string mode)
	=> mode?.ToLower() switch
	{
		"stereo" => WaveformRenderer.WaveformMode.Stereo,
		"overlay" => WaveformRenderer.WaveformMode.Overlay,
		_ => WaveformRenderer.WaveformMode.Mix,
	};

		/// <summary>描画先に応じてビットマップをセット</summary>
		private void ApplyWaveformBitmap(Bitmap bmp, string target)
		{
			if (target == "area" && _waveformArea != null)
			{
				_waveformArea.Image = bmp;
				_waveformArea.Invalidate();
			}
			else
			{
				SldTrack.BackgroundImage = bmp;
				SldTrack.Invalidate();
			}
		}
		/// <summary>exponent カーブを適用したコピーを返す（元データを変更しない）</summary>
		private static float[] ApplyExponent(float[] src, float exponent)
		{
			if (src == null) return null;
			if (Math.Abs(exponent - 1.0f) < 0.001f) return src;  // 1.0なら無変換
			var dst = new float[src.Length];
			for (int i = 0; i < src.Length; i++)
				dst[i] = (float)Math.Pow(src[i], exponent);
			return dst;
		}
		/// <summary>WaveformDef からカラー設定を構築</summary>
		private WaveformRenderer.WaveformColors BuildWaveformColors(
			WaveformComponents wDef)
		{
			return new WaveformRenderer.WaveformColors
			{
				ColorL = wDef.ColorL,// ?? "00CC66",
				ColorR = wDef.ColorR,// ?? "0066CC",
				ColorMix = wDef.ColorMix,// ?? "00AA88",
				Played = wDef.ColorPlayed,// ?? "555555",
				Unplayed = wDef.ColorUnplayed,// ?? "333333",
			};
		}
		private static System.Drawing.Color ParseSkinColor(string hex)
		{
			hex = hex.TrimStart('#');
			if (int.TryParse(hex,
				System.Globalization.NumberStyles.HexNumber,
				null, out int val))
			{
				return System.Drawing.Color.FromArgb(
					255,
					(val >> 16) & 0xFF,
					(val >> 8) & 0xFF,
					 val & 0xFF);
			}
			return System.Drawing.Color.Gray;
		}
		#endregion

		#region Button MouseDown Event
		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SetMouseDownEvent()
		{
			foreach (Control c in this.Controls)
			{
				var parentName = c.Parent?.Name;
				var btnMap = _currentSkin.Buttons.TryGetValue(parentName, out var bm) ? bm : null;
                if (c is Button btn && bm.TryGetValue(c.Name, out var bc))
				{
					c.MouseDown -= BtnMouseDown;
					c.MouseDown += BtnMouseDown;
					c.MouseUp -= BtnMouseUp;
					c.MouseUp += BtnMouseUp;
				}
				switch (c.Name)
				{
					case "BtnSeekBack":
						c.MouseDown -= BtnSeekBack_MouseDown;
						c.MouseDown += BtnSeekBack_MouseDown;
						c.MouseUp -= BtnSeekBack_MouseUp;
						c.MouseUp += BtnSeekBack_MouseUp;
						break;
					case "BtnSeekForward":
						c.MouseDown -= BtnSeekForward_MouseDown;
						c.MouseDown += BtnSeekForward_MouseDown;
						c.MouseUp -= BtnSeekForward_MouseUp;
						c.MouseUp += BtnSeekForward_MouseUp;
						break; 
					case "BtnRandom":
						c.MouseUp -= BtnRandom_MouseUp;
						c.MouseUp += BtnRandom_MouseUp;
						break;
					case "BtnLoop":
						c.MouseUp -= BtnLoop_MouseUp;
						c.MouseUp += BtnLoop_MouseUp;
						break;
				}
			}
		}

		private void BtnSeekBack_MouseDown(object sender, MouseEventArgs e) => seeking = 2;
		private void BtnSeekForward_MouseDown(object sender, MouseEventArgs e) => seeking = 1;
		#endregion

		#region Button MouseUp Event
		private void BtnSeekBack_MouseUp(object sender, MouseEventArgs e)
		{
			this.seekValue = 0;
			this.seeking = 0;
		}
		private void BtnSeekForward_MouseUp(object sender, MouseEventArgs e)
		{
			this.seekValue = 0;
			this.seeking = 0;
		}
		private void BtnRandom_MouseUp(object sender, MouseEventArgs e)
		{
			_skinApplicator?.UpdateRandomButton((Button)sender, _controller.GetLoopMode());
		}
		private void BtnLoop_MouseUp(object sender, MouseEventArgs e)
		{
			_skinApplicator?.UpdateLoopButton((Button)sender, _controller.GetLoopMode());
		}
		#endregion

		#region Button Click Event
		public bool SuppressNextMouseDown { get; set; } = false;

		/// <summary>
		/// ファイルを開くボタンをクリック
		/// ファイルオープンダイアログにてファイル選択後、自動で再生する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnOpenFile_Click(object sender, EventArgs e)
		{
			if (this.IsDisposed || _openFileDialogMedia == null)
				return;

			try
			{
				_openFileDialogMedia.InitialDirectory = _config.settings.LastMediaDirectory;
				if (_openFileDialogMedia.ShowDialog() == DialogResult.OK)
				{
					_config.settings.LastMediaDirectory = Path.GetDirectoryName(_openFileDialogMedia.FileName);
					_controller.OpenAndPlay(_openFileDialogMedia.FileName);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("ファイルのオープンに失敗しました。\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				// ★ダイアログを閉じた後の最初のMouseDownを無視
				SuppressNextMouseDown = true;
			}
		}

		/// <summary>
		/// 再生/一時停止ボタンのクリック
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnPlay_Click(object sender, EventArgs e)
		{
			_controller.TogglePlayPause();
		}

		/// <summary>
		/// 停止ボタンのクリック
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnStop_Click(object sender, EventArgs e)
		{
			// 問答無用の停止
			_controller.Stop();
		}


		/// <summary>
		/// 閉じるボタンのクリック
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnClose_Click(object sender, EventArgs e)
		{
			_fileInfoForm.Close();
			_fileInfoForm.Dispose();
            _playListForm.Close();
			_playListForm.Dispose();
			_optionsForm.Close();
			_optionsForm.Dispose();
			_cdForm.Close();      // 追加
			_cdForm.Dispose();    // 追加
            _miniPlayerForm?.Hide();
            _miniPlayerForm?.Dispose();                              
			// 終了
            Close();
		}
		private void BtnBack_Click(object sender, EventArgs e)
		{
			// ループ無し：最初の曲まで減算
			// １曲ループ：最初の曲まで減算
			// 全曲ループ：最初の曲まで減算、最初の曲から最後の曲へ戻る
			_controller.PlayPrevious();
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
			_controller.PlayNext();
		}

		private void BtnRandom_Click(object sender, EventArgs e)
		{
			_controller.ToggleRandom();
			_skinApplicator?.UpdateRandomButton((Button)sender, _controller.GetLoopMode());
		}

		private void BtnLoop_Click(object sender, EventArgs e)
		{
			_controller.CycleLoop();
			_skinApplicator?.UpdateLoopButton((Button)sender, _controller.GetLoopMode());
		}
		private void BtnSetting_Click(object sender, EventArgs e)
		{
			_optionsForm.Show();
		}
		private void BtnPlaylist_Click(object sender, EventArgs e)
		{
			if (_playListForm.Visible)
			{
				_playListForm.Hide();
				return;
			}

			_playListForm.Show(this);
			var plForm = _currentSkin?.SubForms["PlayListForm"];

			if (plForm != null)
			{
				// Main + Offset
				_playListForm.Left = Left + plForm.Position.Left;
				_playListForm.Top = Top + plForm.Position.Top;
			}

		}
		private void BtnMinisize_Click(object sender, EventArgs e)
		{
			this.Hide();
			_playListForm.Hide();
            // スキンがミニプレイヤーフォームに対応しているならそちらを表示、そうでないならタスクトレイアイコンを表示
            if (_currentSkin.SubForms.TryGetValue("MiniPlayerForm", out var miniForm))
			{
                _miniPlayerForm.Show(this);
				_miniPlayerForm.Activate();
            }
            else
			{
                notifyIcon.Visible = true;
            }
		}
		// NotifyIcon ダブルクリックで復元
		private void NotifyIcon_DoubleClick(object sender, EventArgs e)
		{
			this.Show();
//			_playListForm.Show();
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
			_controller.SetPosition(time);
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
			_controller.SetPan(SldPan.Value);
		}

		/// <summary>
		/// パンスライダー
		/// 移動確定
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SldPan_SliderMoved(object sender, MouseEventArgs e)
		{
			_controller.SetPan(SldPan.Value);
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
			_controller.SetVolume(SldVolume.Value);
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
			_controller.SetVolume(SldVolume.Value);
			_toolTip.Hide(this);
		}

		private void SldTrack_ValueChanged(object sender, EventArgs e)
		{
			if (this.seekValue > 0)
			{
				TimeSpan stime = TimeSpan.FromMilliseconds(SldTrack.Value);
				_toolTip.Show(stime.ToString(@"hh\:mm\:ss"), this, ((CustomSlider)(sender)).Left, ((CustomSlider)(sender)).Top, 1);
				uint time = (uint)SldTrack.Value;
				_controller.SetPosition(time);
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
			_cdForm.Show();
		}

		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
			//コントロール内にドロップされたとき実行される
			//ドロップされたすべてのファイル名を取得する
			string[] fileName =
				(string[])e.Data.GetData(DataFormats.FileDrop, false);

			_controller.OpenFiles(fileName);
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

		ToolStripMenuItem menuPlayModeNormal = new ToolStripMenuItem("Normal", null, null, "menuPlayModeNormal");
		ToolStripMenuItem menuPlayModeRandom = new ToolStripMenuItem("Random", null, null, "menuPlayModeRandom");
		ToolStripMenuItem menuPlayModeRepeat = new ToolStripMenuItem("Repeat", null, null, "menuPlayModeRepeat");
		ToolStripMenuItem menuPlayModeLoop = new ToolStripMenuItem("Loop", null, null, "menuPlayModeLoop");
		ToolStripMenuItem menuSleep = new ToolStripMenuItem("スリープタイマー");
		ToolStripMenuItem menuSleep15 = new ToolStripMenuItem("15分後");
		ToolStripMenuItem menuSleep30 = new ToolStripMenuItem("30分後");
		ToolStripMenuItem menuSleep60 = new ToolStripMenuItem("60分後");
		ToolStripMenuItem menuSleepCancel = new ToolStripMenuItem("キャンセル");

		private void InitContextMenu()
		{
			var menuOpen = new ToolStripMenuItem("開く(&O)", null, BtnOpenFile_Click, "menuOpen");
			var menuUrlOpen = new ToolStripMenuItem("URLを開く(&R)", null, BtnUrlOpen_Click, "menuUrlOpen");
			var menuPlay = new ToolStripMenuItem("再生(&P)", null, BtnPlay_Click, "menuPlay");
			var menuPause = new ToolStripMenuItem("一時停止(&H)", null, BtnPause_Click, "menuPause");
			var menuStop = new ToolStripMenuItem("停止(&S)", null, BtnStop_Click, "menuStop");
			var menuBack = new ToolStripMenuItem("前へ(&Z)", null, BtnBack_Click, "menuBack");
			var menuNext = new ToolStripMenuItem("次へ(&B)", null, BtnNext_Click, "menuNext");
			var menuPlayMode = new ToolStripMenuItem("再生モード", null, null, "menuPlayMode");
			var menuPlayList = new ToolStripMenuItem("プレイリスト(&L)", null, BtnPlaylist_Click, "menuPlayList");
			var menuOption = new ToolStripMenuItem("設定(&T)", null, BtnSetting_Click, "menuOption");
			var menuEffects = new ToolStripMenuItem("エフェクト(&E)", null, null, "menuEffects");
			var menuEqualizer = new ToolStripMenuItem("イコライザ(&Q)", null, null, "menuEqualizer");
			var menuExtensions = new ToolStripMenuItem("関連付け(&D)", null, null, "menuExtensions");
			var menuSkinSelect = new ToolStripMenuItem("スキン設定(&A)", null, null, "menuSkinSelect");
			var menuAutoUpdateCheck = new ToolStripMenuItem("最新版確認(&U)", null, null, "menuAutoUpdateCheck");
			var menuAbout = new ToolStripMenuItem("About(&C)", null, null, "menuAbout");
			var menuHelp = new ToolStripMenuItem("ヘルプ(&V)", null, null, "menuHelp");
			var menuMinimize = new ToolStripMenuItem("最小化(&X)", null, BtnMinisize_Click, "menuMinimize");
			var menuExit = new ToolStripMenuItem("閉じる(&Z)", null, BtnClose_Click, "menuExit");

			var menuFileInfo = new ToolStripMenuItem("ファイル情報");

			menuOpen.Size = new System.Drawing.Size(192, 22);
			menuUrlOpen.Size = new System.Drawing.Size(192, 22);
			menuPlay.Size = new System.Drawing.Size(192, 22);
			menuPause.Size = new System.Drawing.Size(192, 22);
			menuStop.Size = new System.Drawing.Size(192, 22);
			menuBack.Size = new System.Drawing.Size(192, 22);
			menuNext.Size = new System.Drawing.Size(192, 22);
			menuPlayMode.Size = new System.Drawing.Size(192, 22);
			menuPlayModeNormal.Size = new System.Drawing.Size(118, 22);
			menuPlayModeRandom.Size = new System.Drawing.Size(118, 22);
			menuPlayModeRepeat.Size = new System.Drawing.Size(118, 22);
			menuHelp.Size = new System.Drawing.Size(192, 22);
			menuPlayModeLoop.Size = new System.Drawing.Size(118, 22);
			menuPlayList.Size = new System.Drawing.Size(192, 22);
			menuOption.Size = new System.Drawing.Size(192, 22);
			menuEffects.Size = new System.Drawing.Size(192, 22);
			menuEqualizer.Size = new System.Drawing.Size(192, 22);
			menuExtensions.Size = new System.Drawing.Size(192, 22);
			menuSkinSelect.Size = new System.Drawing.Size(192, 22);
			menuAutoUpdateCheck.Size = new System.Drawing.Size(192, 22);
			menuAbout.Size = new System.Drawing.Size(192, 22);
			menuMinimize.Size = new System.Drawing.Size(192, 22);
			menuExit.Size = new System.Drawing.Size(192, 22);

			menuPlayMode.DropDownItems.AddRange(new ToolStripItem[] {
				menuPlayModeNormal, menuPlayModeRandom, menuPlayModeRepeat, menuPlayModeLoop });

			menuPlayList.Click += (s, e) => BtnPlaylist_Click(s, e);
			menuOption.Click += (s, e) => BtnSetting_Click(s, e);
			menuMinimize.Click += (s, e) => this.WindowState = FormWindowState.Minimized;
			menuExit.Click += (s, e) => Application.Exit();

			menuSleep15.Click += (s, e) =>
			{
				_sleepTimerRemaining = 15 * 60 * 1000;
				UpdateSleepTimerMenu(menuSleep15);
			};
			menuSleep30.Click += (s, e) =>
			{
				_sleepTimerRemaining = 30 * 60 * 1000;
				UpdateSleepTimerMenu(menuSleep30);
			};
			menuSleep60.Click += (s, e) =>
			{
				_sleepTimerRemaining = 60 * 60 * 1000;
				UpdateSleepTimerMenu(menuSleep60);
			};
			menuSleepCancel.Click += (s, e) =>
			{
				_sleepTimerRemaining = 0;
				UpdateSleepTimerMenu(null);
			};
			menuSleep.DropDownItems.AddRange(new ToolStripItem[]
			{
				menuSleep15,
				menuSleep30,
				menuSleep60,
				new ToolStripSeparator(),
				menuSleepCancel,
			});

			menuFileInfo.Click += (s, e) =>
			{
				_fileInfoForm.LoadInfo();
				_fileInfoForm.Show();
				_fileInfoForm.Activate();
			};
			// Effects / Equalizer / Extensions / SkinSelect は
			// OptionsForm の該当タブを開く形にする
			menuEffects.Click += (s, e) => OpenOptionsTab("PITCH");
			menuEqualizer.Click += (s, e) => OpenOptionsTab("GEQ");
			menuExtensions.Click += (s, e) => OpenOptionsTab("EXTENSIONS");
			menuSkinSelect.Click += (s, e) => OpenOptionsTab("SKIN");
			menuAbout.Click += (s, e) => OpenOptionsTab("ABOUT");

			// 
			// contextMenu
			// 
			contextMenu.Items.AddRange(new ToolStripItem[] {
				menuOpen,
				menuUrlOpen,
				new ToolStripSeparator(),
				menuPlay,
				menuPause,
				menuStop,
				menuBack,
				menuNext,
				new ToolStripSeparator(),
				menuFileInfo,
				menuPlayMode,
				menuSleep,
				new ToolStripSeparator(),
				menuPlayList,
				menuOption,
				menuEffects,
				menuEqualizer,
				menuExtensions,
				menuSkinSelect,
				new ToolStripSeparator(),
				menuAutoUpdateCheck,
				new ToolStripSeparator(),
				menuAbout,
				menuHelp,
				new ToolStripSeparator(),
				menuMinimize,
				menuExit
			});
			contextMenu.Name = "contextMenu";
			contextMenu.Size = new System.Drawing.Size(193, 422);

			// PlayMode
			menuPlayModeNormal.Click += (s, e) => _controller.SetLoopMode(LOOP_MODE.LOOP_NONE);
			menuPlayModeRandom.Click += (s, e) => _controller.SetLoopMode(LOOP_MODE.LOOP_RANDOM);
			menuPlayModeRepeat.Click += (s, e) => _controller.SetLoopMode(LOOP_MODE.LOOP_ONE_REPEAT);
			menuPlayModeLoop.Click += (s, e) => _controller.SetLoopMode(LOOP_MODE.LOOP_ALL);

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
		private void UpdateSleepTimerMenu(ToolStripMenuItem selected)
		{
			menuSleep15.Checked = false;
			menuSleep30.Checked = false;
			menuSleep60.Checked = false;
			if (selected != null)
				selected.Checked = true;
		}
		private void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// PlayMode チェック状態を更新
			menuPlayModeRandom.Enabled = false; // 未実装
			menuPlayModeNormal.Checked = (_controller.GetLoopMode() & LOOP_MODE.LOOP_NONE) != 0;
			menuPlayModeRandom.Checked = (_controller.GetLoopMode() & LOOP_MODE.LOOP_RANDOM) != 0;
			menuPlayModeRepeat.Checked = (_controller.GetLoopMode() & LOOP_MODE.LOOP_ONE_REPEAT) != 0;
			menuPlayModeLoop.Checked = (_controller.GetLoopMode() & LOOP_MODE.LOOP_ALL) != 0;
		}

		//private void SetPlayMode(LOOP_MODE mode)
		//{
		//	if (mode == LOOP_MODE.LOOP_RANDOM)
		//	{
		//		// ランダムはトグル
		//		_player.loop ^= LOOP_MODE.LOOP_RANDOM;
		//		if ((_player.loop & LOOP_MODE.LOOP_RANDOM) != 0)
		//			_player.BuildShuffleQueue(); // ONになった時点で生成
		//	}
		//	else
		//	{
		//		// ランダムフラグを保持しつつ他のモードを切り替え
		//		bool isRandom = (_player.loop & LOOP_MODE.LOOP_RANDOM) != 0;
		//		_player.loop = mode;
		//		if (isRandom) _player.loop |= LOOP_MODE.LOOP_RANDOM;
		//	}
		//}

		private void OpenOptionsTab(string tabName)
		{
			_optionsForm.Show();
			_optionsForm.SelectTab(tabName);
		}

		private void BtnUrlOpen_Click(object sender, EventArgs e)
		{
			// UrlInputForm or InputBox でURL取得
			string url = Microsoft.VisualBasic.Interaction.InputBox(
				"再生するURLを入力してください",
				"URL Open",
				"https://");

			if (string.IsNullOrWhiteSpace(url)) return;
			if (!url.StartsWith("http://") && !url.StartsWith("https://"))
			{
				MessageBox.Show("URLはhttp://またはhttps://で始まる必要があります。",
					"URL Open", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

            if (!_controller.OpenUrl(url))
			{
                MessageBox.Show("URLを開けませんでした。",
                    "URL Open", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // 詳細メッセージは ErrorOccurred イベントで LabelTitle に表示される
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
						_controller.PlayNext();
						break;
					case APPCOMMAND_MEDIA_PREVIOUSTRACK:
						_controller.PlayPrevious();
						break;
						//case APPCOMMAND_VOLUME_UP:
						//    SldVolume.Value = Math.Min(SldVolume.Value + 5, SldVolume.Maximum);
						//    _player.SetVolume(((float)SldVolume.Value) / 100f);
						//    break;
						//case APPCOMMAND_VOLUME_DOWN:
						//    SldVolume.Value = Math.Max(SldVolume.Value - 5, SldVolume.Minimum);
						//    _player.SetVolume(((float)SldVolume.Value) / 100f);
						//    break;
						//case APPCOMMAND_VOLUME_MUTE:
						//    _player.SetVolume(0f);
						//    break;
				}
				m.Result = (IntPtr)1; // 処理済みを通知
				return;
			}
			base.WndProc(ref m);
		}
        public void RestoreFromMini()
        {
            _miniPlayerForm.Hide();
            this.Show();
            this.Activate();
            // PlayListForm が表示中だった場合は復元
            // （必要に応じて状態を保持して復元）
        }
    }
}