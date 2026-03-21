using MediaPlayer_X_Ark.Engine;
using MediaPlayer_X_Ark.Forms;
using MediaPlayer_X_Ark.Skin;
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

		public static IPlayerEngine player;
		private static IConfigService config;
		private FileInfoForm _fileInfoForm;
		private ToolTip _toolTip;

		private PlayListForm playListForm;
		private OptionsForm optionsForm;
		private CDForm cdForm;

		private int _sleepTimerRemaining = 0; // 残り秒数（0=無効）

		private ISkinSystem _currentSkin;
		public ISkinSystem CurrentSkin => _currentSkin;

		// 用途別にOpenFileDialogを分離
		private OpenFileDialog _openFileDialogMedia;   // 音楽ファイル用
		private OpenFileDialog _openFileDialogSkin;    // スキン用
													   // MainForm.cs フィールド追加
		private readonly List<Form> _managedForms = new List<Form>();
		public MainForm()
		{
			InitializeComponent();
		}
		public void ApplyDisplaySettings()
		{
			Spectrum.Mode = config.settings.DefaultSpectrumMode;
			Spectrum.SnowBlockEnabled = config.settings.SnowBlockEnabled;
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

			// MainFormコントロール
			foreach (Control c in Controls)
			{
				string cName = c.Name;

				if (c is Button btn && skin.Buttons.TryGetValue(cName, out var bc))
				{
					if (bc.BackImage == null || !bc.Enabled)
					{ btn.Visible = false; btn.Enabled = false; continue; }
					btn.AutoSize = false;
					btn.BackgroundImage = bc.BackImage;
					btn.BackgroundImageLayout = ImageLayout.None;
					btn.Top = bc.Position.Top;
					btn.Left = bc.Position.Left;
					btn.Width = bc.Position.Width;
					btn.Height = bc.Position.Height;
					btn.Enabled = bc.Enabled;
					btn.Visible = bc.Enabled;
					btn.Refresh();
				}
				else if (c is CustomSlider slider && skin.Sliders.TryGetValue(cName, out var sc))
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
				else if (c is ScrollLabel lbl && skin.Labels.TryGetValue(cName, out var gc))
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
					lbl.Value.Left = 0;
					lbl.Value.Width = gc.Position.Width;
					lbl.Value.Height = gc.Position.Height;
					lbl.ScrollEnable = gc.ScrollEnable;
					lbl.Timer.Interval = gc.Interval > 0 ? gc.Interval : 100;
					lbl.Timer.Enabled = gc.Interval > 0;
				}
			}

			this.Refresh();
			var plForm = _currentSkin["PlayListForm"];
			if (plForm != null)
			{
				playListForm.Left = Left - plForm.Position.Left;
				playListForm.Top = Top - plForm.Position.Top;
				playListForm.BackgroundImage = plForm.BackImage;
				playListForm.Width = plForm.Position.Width;
				playListForm.Height = plForm.Position.Height;
				playListForm.TransparencyKey = plForm.TransparentKey;
				playListForm.Refresh();
			}

			if (_currentSkin.Grids.TryGetValue("PlayListGrid", out var plGrid))
			{
				foreach (Control c in playListForm.Controls)
				{
					if (c is DataGridView grid)
					{
						grid.BackgroundColor = plGrid.ListBackColor;
						grid.RowsDefaultCellStyle.BackColor = plGrid.ListBackColor;
						grid.RowsDefaultCellStyle.ForeColor = plGrid.ListForeColor;
						grid.ForeColor = plGrid.ListForeColor;
						grid.Left = plGrid.ListPosition.Left;
						grid.Top = plGrid.ListPosition.Top;
						grid.Width = plGrid.ListPosition.Width;
						grid.Height = plGrid.ListPosition.Height;
					}
				}
			}

			// PlayListFormのボタン
			var plButtons = _currentSkin.GetFormButtons("PlayListForm");
			foreach (Control c in playListForm.Controls)
			{
				if (c is Button btn && plButtons.TryGetValue(c.Name, out var bc))
				{
					if (bc.BackImage == null || !bc.Enabled)
					{ btn.Visible = false; btn.Enabled = false; continue; }
					btn.AutoSize = false;
					btn.BackgroundImage = bc.BackImage;
					btn.BackgroundImageLayout = ImageLayout.None;
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
			if (player.CreateSound(fileName, out idx) != FMOD.RESULT.OK)
				return;

			switch (config.settings.OpenFileAction)
			{
				case 1: // 常に再生
					PlayLoad(idx);
					break;

				case 2: // 常に追加のみ
						// 再生しない
					break;

				default: // 再生中なら追加・停止中なら再生
					if (!player.IsPlaying())
						PlayLoad(idx);
					break;
			}
			// ★自動保存
			AutoSavePlaylist();
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
			var map = new Dictionary<string, ButtonComponents>(_currentSkin.Buttons);
			foreach (var formButtons in _currentSkin.FormButtons.Values)
				foreach (var kv in formButtons)
					map[kv.Key] = kv.Value;
			return map;
		}
		private bool _isHandlingTrackEnded = false;
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
			player.SoundFontPath = config.settings.SoundFontPath;

			playListForm = new PlayListForm(this);
			playListForm.Owner = this;

			optionsForm = new OptionsForm(player, config, this);
			cdForm = new CDForm(this, config);
			// ★管理リストに追加
			_managedForms.Add(playListForm);
			_managedForms.Add(optionsForm);
			_managedForms.Add(cdForm);
			// 予定：設定ファイルの読み込み スキンファイルの指定も含む
			// 旧形式（XSF）のスキンファイルの場合はOldSkinSystem
			// 新形式（JSON）の場合はNewSkinSystemへインスタンス切替
			// スキンロード
			SkinLoad(config.settings.Skin);
			Spectrum.Initialize();
			Spectrum.Mode = config.settings.DefaultSpectrumMode;
			Spectrum.SnowBlockEnabled = config.settings.SnowBlockEnabled;

			if (config.settings.RestorePlaylist)
			{
				var playlistPath = Path.Combine(
					Application.StartupPath, "last_playlist.json");
				if (File.Exists(playlistPath))
					RestorePlaylistFromFile(playlistPath);
			}
			if (config.settings.RestorePosition
				&& config.settings.LastPlayingIndex >= 0
				&& config.settings.LastPlayingIndex < player.PlayList.Count)
			{
				// ★一時停止状態で再生開始
				player.SetDevice(config.settings.Device);
				player.PlaySoundPaused(config.settings.LastPlayingIndex,
					config.settings.LastPlayingPosition);
				UpdateTrackUI();
			}
			InitContextMenu();

			_openFileDialogMedia = new OpenFileDialog();
			_openFileDialogMedia.Filter = "音楽ファイル|*.mp3;*.flac;*.ogg;*.wav;*.aac;*.m4a;*.wma;*.mid;*.mod;*.xm;*.it;*.s3m|すべてのファイル|*.*";
			_openFileDialogMedia.Multiselect = true;

			this.TopMost = config.settings.AlwaysOnTop;
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
		public void AutoSavePlaylist()
		{
			if (!config.settings.AutoSavePlaylist) return;
			SavePlaylistToFile(Path.Combine(
				Application.StartupPath, "last_playlist.json"));
		}
		private void RestorePlaylistFromFile(string path)
		{
			try
			{
				var list = System.Text.Json.JsonSerializer
					.Deserialize<List<string>>(
						File.ReadAllText(path, System.Text.Encoding.UTF8));

				if (list == null) return;

				foreach (var file in list)
				{
					if (File.Exists(file))
						player.CreateSound(file, out _);
				}
			}
			catch { }
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

			var item = player.PlayList[index];
			LabelTitle.Value.Text = (!string.IsNullOrEmpty(item.Title)) ? item.Title : Path.GetFileName(item.FileName);
			LabelTitle.Value.Text += (!string.IsNullOrEmpty(item.Artist)) ? (" - " + item.Artist) : "";
			LabelTitle.Value.Text += (!string.IsNullOrEmpty(item.Album)) ? (" - " + item.Album) : "";

			// ★FileInfoFormが開いている場合は自動更新
			if (_fileInfoForm != null && _fileInfoForm.Visible)
				_fileInfoForm.LoadInfo(index);
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

		// ─────────────────────────────────────────────────────────────────
		//  MainForm.cs へのキーボードショートカット修正
		//
		//  【問題】
		//    MainForm_KeyDown は KeyPreview=true でも以下のキーに届かない：
		//      - Up/Down/Left/Right … WinForms が ProcessDialogKey で先に消費する
		//      - Space/Enter        … フォーカスのあるボタンをクリックしてしまう
		//
		//  【解決策】
		//    ProcessCmdKey() をオーバーライドする。
		//    これはキーイベントの最上流（フォーカスより前）で発火するため
		//    全キーを確実に捕捉できる。
		//    MainForm_KeyDown / MainForm_KeyUp はシーク用に残す（後述）。
		//
		//  【適用方法】
		//    1. 以下の ProcessCmdKey メソッドを MainForm クラスに追加する
		//    2. MainForm_KeyDown の Space/Enter/S/B/Z/L/R/Escape/Up/Down の
		//       各 case を削除し、Left/Right の seeking フラグ設定だけ残す
		//       （Left/Right はキーリピートが必要なため KeyDown でも処理する）
		// ─────────────────────────────────────────────────────────────────

		// ★ MainForm クラスに追加するメソッド

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
					if (player.PlayingIndex >= 0)
						PlayLoad(player.PlayingIndex);
					return true;

				case Keys.S:
					// S: 停止
					BtnStop_Click(this, EventArgs.Empty);
					return true;

				case Keys.B:
					// B: 次の曲
					player.PlayNext();
					UpdateTrackUI();
					return true;

				case Keys.Z:
					// Z: 前の曲
					player.PlayPrevious();
					UpdateTrackUI();
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
					player.SetVolume(((float)SldVolume.Value) / 100f);
					return true;

				case Keys.Down:
					// Down: 音量-5
					SldVolume.Value = Math.Max(SldVolume.Value - 5, SldVolume.Minimum);
					player.SetVolume(((float)SldVolume.Value) / 100f);
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
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}


		// ─────────────────────────────────────────────────────────────────
		//  MainForm.cs 修正パッチ
		//
		//  問題：BtnLoop_Click が常に DownImage を設定するため、
		//        キーボードから呼ぶと2回目以降は画像変化なし＝無反応に見える。
		//        （マウス時は MouseUp が正しい画像に上書きするため問題が出ない）
		//
		//  修正：
		//    1. UpdateLoopButtonVisual()   — ループ状態→画像のヘルパーを追加
		//    2. UpdateRandomButtonVisual() — ランダム状態→画像のヘルパーを追加
		//    3. BtnLoop_Click  を修正（DownImage 固定を廃止）
		//    4. BtnLoop_MouseUp を修正（ヘルパーに委譲）
		//    5. BtnRandom_Click を修正
		//    6. BtnRandom_MouseUp を修正
		//    ProcessCmdKey は変更なし（BtnLoop/BtnRandom を sender に渡す既存実装でOK）
		// ─────────────────────────────────────────────────────────────────
		// ── 追加: ループボタン画像更新ヘルパー ─────────────────────────────
		/// <summary>
		/// player.loop の現在値に合わせて BtnLoop の背景画像を更新する。
		/// Click・MouseUp・キーボードショートカットの全経路で使用する。
		/// </summary>
		private void UpdateLoopButtonVisual(Button btn)
		{
			var bc = _currentSkin.Buttons["BtnLoop"];
			if (bc == null) 
				return;

			// LOOP_RANDOM フラグを除いた純粋なループモードで判定する
			var loopOnly = player.loop & ~LOOP_MODE.LOOP_RANDOM;

			switch (loopOnly)
			{
				case LOOP_MODE.LOOP_NONE:
					btn.BackgroundImage = bc.BackImage;
					break;
				case LOOP_MODE.LOOP_ONE_REPEAT:
					btn.BackgroundImage = bc.DownImage;
					break;
				case LOOP_MODE.LOOP_ALL:
					btn.BackgroundImage = bc.OptionalImage;
					break;
			}
			btn.Refresh();
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

				var plForm = _currentSkin?["PlayListForm"];
				if (plForm != null)
				{
					if (plForm.MagnetMode)
					{
						playListForm.Left = Left - plForm.Position.Left;
						playListForm.Top = Top - plForm.Position.Top;
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
			// ★プレイリスト自動保存
			if (config.settings.RestorePlaylist)
				SavePlaylistToFile(Path.Combine(
					Application.StartupPath, "last_playlist.json"));
			// ★再生位置を保存
			if (config.settings.RestorePosition)
			{
				config.settings.LastPlayingIndex = player.PlayingIndex;
				config.settings.LastPlayingPosition = player.GetPosition();
			}
			else
			{
				config.settings.LastPlayingIndex = -1;
				config.settings.LastPlayingPosition = 0;
			}
			config.Save();
			cdForm?.Dispose();   // 追加
			player.Dispose();  // 明示的に解放
			player = null;
			notifyIcon.Visible = false;
			notifyIcon.Dispose();
			SkinPackage.CleanupTempDirectory(); // 追加
		}
		#endregion
		private void SavePlaylistToFile(string path)
		{
			var list = player.PlayList.Select(p => p.FileName).ToList();
			File.WriteAllText(path,
				System.Text.Json.JsonSerializer.Serialize(list),
				System.Text.Encoding.UTF8);
		}
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
					UpdateSleepTimerMenu(null);
				}
			}

			// 曲終了検知（コールバックが信頼できないため）
			if (player.NowPlaying && !player.IsPlaying())
			{
				if (!_isHandlingTrackEnded)
				{
					_isHandlingTrackEnded = true;
					try
					{
						player.PlayNext();
						UpdateTrackUI();
					}
					finally
					{
						_isHandlingTrackEnded = false;
					}
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
			UpdateRandomButtonVisual((Button)sender);
		}
		private void BtnLoop_MouseUp(object sender, MouseEventArgs e)
		{
			UpdateLoopButtonVisual((Button)sender);
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
				_openFileDialogMedia.InitialDirectory = config.settings.LastMediaDirectory;
				if (_openFileDialogMedia.ShowDialog() == DialogResult.OK)
				{
					config.settings.LastMediaDirectory = Path.GetDirectoryName(_openFileDialogMedia.FileName);
					OpenFile(_openFileDialogMedia.FileName);
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

		private void UpdateRandomButtonVisual(Button btn)
		{
			var bc = _currentSkin.Buttons["BtnRandom"];
			if (bc != null)
			{
				btn.BackgroundImage = (player.loop & LOOP_MODE.LOOP_RANDOM) != 0
				? bc.DownImage
				: bc.BackImage;
				btn.Refresh();
			}

		}

		private void BtnRandom_Click(object sender, EventArgs e)
		{
			var btnRandom = _currentSkin.Buttons["BtnRandom"];
			if (btnRandom != null)
			{
				SetPlayMode(LOOP_MODE.LOOP_RANDOM);
				UpdateRandomButtonVisual((Button)sender);
			}
		}

		// ── 修正: BtnLoop_Click ────────────────────────────────────────────
		// Before:
		//   ((Button)sender).BackgroundImage = btnLoop.DownImage; ← 常に DownImage
		// After:
		//   UpdateLoopButtonVisual((Button)sender); 
		private void BtnLoop_Click(object sender, EventArgs e)
		{
			var btnLoop = _currentSkin.Buttons["BtnLoop"];
			if (btnLoop != null)
			{
				// LOOP_RANDOM フラグを除いた値で switch する
				switch (player.loop & ~LOOP_MODE.LOOP_RANDOM)
				{
					case LOOP_MODE.LOOP_NONE:
						SetPlayMode(LOOP_MODE.LOOP_ONE_REPEAT);
						break;
					case LOOP_MODE.LOOP_ONE_REPEAT:
						SetPlayMode(LOOP_MODE.LOOP_ALL);
						break;
					case LOOP_MODE.LOOP_ALL:
						SetPlayMode(LOOP_MODE.LOOP_NONE);
						break;
				}
				UpdateLoopButtonVisual((Button)sender);  // ← 新しい状態に合わせて画像更新
			}
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
			var plForm = _currentSkin?["PlayListForm"];

			if (plForm != null)
			{
				playListForm.Left = Left - plForm.Position.Left;
				playListForm.Top = Top - plForm.Position.Top;
			}

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
			var menuOpen = new ToolStripMenuItem("Open(&O)", null, BtnOpenFile_Click, "menuOpen");
			var menuUrlOpen = new ToolStripMenuItem("URL Open(&R)", null, BtnUrlOpen_Click, "menuUrlOpen");
			var menuPlay = new ToolStripMenuItem("Play(&P)", null, BtnPlay_Click, "menuPlay");
			var menuPause = new ToolStripMenuItem("Pause(&H)", null, BtnPause_Click, "menuPause");
			var menuStop = new ToolStripMenuItem("Stop(&S)", null, BtnStop_Click, "menuStop");
			var menuBack = new ToolStripMenuItem("Back(&Z)", null, BtnBack_Click, "menuBack");
			var menuNext = new ToolStripMenuItem("Next(&B)", null, BtnNext_Click, "menuNext");
			var menuPlayMode = new ToolStripMenuItem("PlayMode", null, null, "menuPlayMode");
			var menuPlayList = new ToolStripMenuItem("PlayList(&L)", null, BtnPlaylist_Click, "menuPlayList");
			var menuOption = new ToolStripMenuItem("Option(&T)", null, BtnSetting_Click, "menuOption");
			var menuEffects = new ToolStripMenuItem("Effects(&E)", null, null, "menuEffects");
			var menuEqualizer = new ToolStripMenuItem("Equalizer(&Q)", null, null, "menuEqualizer");
			var menuExtensions = new ToolStripMenuItem("Extensions(&D)", null, null, "menuExtensions");
			var menuSkinSelect = new ToolStripMenuItem("Skin Select(&A)", null, null, "menuSkinSelect");
			var menuAutoUpdateCheck = new ToolStripMenuItem("Auto Update Check(&U)", null, null, "menuAutoUpdateCheck");
			var menuAbout = new ToolStripMenuItem("About(&C)", null, null, "menuAbout");
			var menuHelp = new ToolStripMenuItem("Help(&V)", null, null, "menuHelp");
			var menuMinimize = new ToolStripMenuItem("Minimize(&X)", null, BtnMinisize_Click, "menuMinimize");
			var menuExit = new ToolStripMenuItem("Exit(&Z)", null, BtnClose_Click, "menuExit");

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
				if (player.PlayingIndex < 0) return;
				// 既存のフォームを使い回す
				if (_fileInfoForm == null || _fileInfoForm.IsDisposed)
					_fileInfoForm = new FileInfoForm(player);

				_fileInfoForm.LoadInfo(player.PlayingIndex);
				_fileInfoForm.Show(this);
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
			menuPlayModeNormal.Click += (s, e) => SetPlayMode(LOOP_MODE.LOOP_NONE);
			menuPlayModeRandom.Click += (s, e) => SetPlayMode(LOOP_MODE.LOOP_RANDOM);
			menuPlayModeRepeat.Click += (s, e) => SetPlayMode(LOOP_MODE.LOOP_ONE_REPEAT);
			menuPlayModeLoop.Click += (s, e) => SetPlayMode(LOOP_MODE.LOOP_ALL);

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

			var result = player.PlayUrl(url);
			if (result != FMOD.RESULT.OK)
			{
				MessageBox.Show($"URLを開けませんでした。\n{player.lastError}",
					"URL Open", MessageBoxButtons.OK, MessageBoxIcon.Error);
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