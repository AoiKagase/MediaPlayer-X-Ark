using MediaPlayer_X_Ark.Engine;
using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using MediaPlayer_X_Ark.Engine.Render;
using MediaPlayer_X_Ark.Engine.Update;
using MediaPlayer_X_Ark.Forms;
using MediaPlayer_X_Ark.Skin;
using MediaPlayer_X_Ark.Skin.New;
using NFluidsynth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
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
        private Engine.Discord.DiscordPresenceService _discordPresence;
        private int _sleepTimerRemaining = 0; // 残り秒数（0=無効）
        private Forms.ErrorToastForm _currentToast;

		private INewSkinSystem _currentSkin;
		private SkinApplicator _skinApplicator;
		private int _spectrumUpdateCounter;

		// スキン適用後の値バックアップ（設定オーバーライドをOFFにした時に復元するため）
		private System.Drawing.Font _skinTitleFont;
		private System.Drawing.Font _skinTimeFont;
		private int _skinTitleScrollInterval;

		public INewSkinSystem CurrentSkin => _currentSkin;
		public event EventHandler StartupReady;

		private OpenFileDialog _openFileDialogMedia;
		private Bitmap _waveformBitmap = null;
		private PictureBox _waveformArea = null;
		private CancellationTokenSource _coverArtCts;
		private int _waveformRefreshCounter = 0;
		private readonly List<Form> _managedForms = new List<Form>();
		public IEnumerable<Form> ManagedForms => _managedForms.Where(f => f != null && !f.IsDisposed);
		private int seekValue;
		private int seeking;
		private const int SeekStep = 1000;       // 1回あたりのシーク量（ミリ秒）
		private const int SeekMaxValue = 10000;  // 加速の上限（ミリ秒）
		private static int ClampTrackSliderValue(uint value)
			=> (int)Math.Min(value, (uint)int.MaxValue);

        private float _abStart = -1f;
        private float _abEnd = -1f;
		public MainForm()
		{
			D2DContext.Initialize();
			InitializeComponent();
			ApplicationIcon.ApplyTo(this, notifyIcon);
			DpiChanged += MainForm_DpiChanged;
			this.Opacity = 0;
			Spectrum.Visible = false;
		}

		public void ApplyDisplaySettings()
		{
			Spectrum.Mode = _config.settings.DefaultSpectrumMode;
			Spectrum.SnowBlockEnabled = _config.settings.SnowBlockEnabled;
		}

		private void RefreshSkinBackups()
		{
			_skinTitleFont = LabelTitle.Value.Font;
			_skinTimeFont = LabelTime.Value.Font;
			_skinTitleScrollInterval = LabelTitle.Timer.Interval;
		}

		private void ReapplyCurrentSkinLayout()
		{
			if (_skinApplicator == null)
				return;

			_skinApplicator.ApplyToMainForm(this, Spectrum);
			if (_playListForm != null && !_playListForm.IsDisposed)
			{
				_skinApplicator.ApplyToPlayListForm(_playListForm);
				if (_currentSkin?.SubForms.TryGetValue("PlayListForm", out var playListDef) == true
					&& playListDef.MagnetMode)
				{
					_skinApplicator.UpdatePlayListPosition(this, _playListForm);
				}
			}
			if (_fileInfoForm != null && !_fileInfoForm.IsDisposed)
				_fileInfoForm.ApplySkin(_skinApplicator, HasSubFormSkin("FileInfoForm"));
			if (_miniPlayerForm != null && !_miniPlayerForm.IsDisposed)
				_skinApplicator.ApplyToMiniPlayerForm(_miniPlayerForm);

			SetupWaveformTarget();
			RefreshSkinBackups();
			ApplySpectrumVisualSettings();
		}

		public int ScaleSkinValue(int value)
			=> _skinApplicator?.ScaleValue(this, value) ?? value;

		private bool HasSubFormSkin(string formName)
			=> _currentSkin?.SubForms?.ContainsKey(formName) == true;

		/// <summary>
		/// スキンロード
		/// 設定ファイルからスキンファイルパスを取得して投げる
		/// </summary>
		/// <param name="skinFile"></param>
		public void SkinLoad(string skinFile)
		{
			using (var pkg = SkinPackage.Open(skinFile))
			{
				INewSkinSystem loadedSkin = null;
				if (pkg.Format == SkinPackage.SkinFormat.NewXsk)
				{
					var skin = new NewSkinSystem();
					skin.Open(pkg.DefinitionPath);
					loadedSkin = skin;
				}
				_currentSkin = loadedSkin;
				_skinApplicator = _currentSkin != null ? new SkinApplicator(_currentSkin) : null;
				if (_currentSkin != null)
				{
					_skinApplicator.ApplyToMainForm(this, Spectrum);
					_skinApplicator.ApplyToPlayListForm(_playListForm);
				}
                _fileInfoForm?.ApplySkin(_skinApplicator, HasSubFormSkin("FileInfoForm"));
                if (_currentSkin != null)
                    _skinApplicator.ApplyToMiniPlayerForm(_miniPlayerForm);

				// スキン適用後の値をバックアップ（設定オーバーライドOFF時の復元用）
				RefreshSkinBackups();

                SetupWaveformTarget();
				_config.settings.Skin = skinFile;

				SldVolume.Maximum = 150;
				SldVolume.Value = _config.settings.Volume;

				ApplySpectrumVisualSettings();
			}
		}
		/// <summary>
		/// スキンロード後・設定保存後に呼ぶ。スペクトラム表示色・速度・フォントを設定値で統合適用する。
		/// </summary>
		public void ApplySpectrumVisualSettings()
		{
			var s = _config.settings;

			// ウェーブ色（設定優先でなければスキン値、またはデフォルト）
			if (s.UseCustomWaveColor)
			{
				Spectrum.WaveColorL = ColorTranslator.FromHtml('#' + s.WaveColorL);
				Spectrum.WaveColorR = ColorTranslator.FromHtml('#' + s.WaveColorR);
			}
			else
			{
				var skinL = _currentSkin?.Spectrum?.WaveColorL ?? System.Drawing.Color.Empty;
				var skinR = _currentSkin?.Spectrum?.WaveColorR ?? System.Drawing.Color.Empty;
				Spectrum.WaveColorL = skinL.IsEmpty ? System.Drawing.Color.Lime : skinL;
				Spectrum.WaveColorR = skinR.IsEmpty ? System.Drawing.Color.Cyan : skinR;
			}
			Spectrum.RefreshBrushes();

			// バー色
			if (s.UseCustomSpectrumBarColor)
			{
				var bmp = new System.Drawing.Bitmap(Math.Max(1, Spectrum.Width), Math.Max(1, Spectrum.Height));
				using var g = System.Drawing.Graphics.FromImage(bmp);
				g.Clear(ColorTranslator.FromHtml('#' + s.SpectrumBarColor));
				Spectrum.BitmapSpectrum = bmp;
			}
			else
			{
				// スキンのビットマップを再生成して復元
				var skinSp = _currentSkin?.Spectrum;
				if (skinSp != null)
				{
					System.Drawing.Bitmap bmp;
					if (skinSp.Image != null)
					{
						bmp = new System.Drawing.Bitmap(skinSp.Image);
					}
					else
					{
						bmp = new System.Drawing.Bitmap(Math.Max(1, Spectrum.Width), Math.Max(1, Spectrum.Height));
						using var g = System.Drawing.Graphics.FromImage(bmp);
						g.Clear(skinSp.Color);
					}
					Spectrum.BitmapSpectrum = bmp;
				}
			}

			// スノー落下速度（px/秒 → px/frame）
			Spectrum.SnowFallSpeed = s.SnowFallSpeedPxPerSec * (s.SpectrumUpdateIntervalMs / 1000f);

			// タイトルフォント
			if (s.UseCustomTitleFont && !string.IsNullOrEmpty(s.TitleFontName) && s.TitleFontSize > 0)
			{
				var style = s.TitleFontBold ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular;
				LabelTitle.Value.Font = new System.Drawing.Font(
					s.TitleFontName,
					s.TitleFontSize,
					style,
					System.Drawing.GraphicsUnit.Point);
			}
			else if (_skinTitleFont != null)
				LabelTitle.Value.Font = _skinTitleFont;
			LabelTitle.RefreshLabelLayout();

			// 時間表示フォント
			if (s.UseCustomTimeFont && !string.IsNullOrEmpty(s.TimeFontName) && s.TimeFontSize > 0)
			{
				var style = s.TimeFontBold ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular;
				LabelTime.Value.Font = new System.Drawing.Font(
					s.TimeFontName,
					s.TimeFontSize,
					style,
					System.Drawing.GraphicsUnit.Point);
			}
			else if (_skinTimeFont != null)
				LabelTime.Value.Font = _skinTimeFont;
			LabelTime.RefreshLabelLayout();

			// タイトルスクロール速度
			if (s.UseCustomTitleScrollInterval && s.TitleScrollIntervalMs > 0)
				LabelTitle.Timer.Interval = s.TitleScrollIntervalMs;
			else if (_skinTitleScrollInterval > 0)
				LabelTitle.Timer.Interval = _skinTitleScrollInterval;

			// ウェーブフォーム色変更の即時反映
			if (_controller.IsValidTrackIndex(_controller.PlayingIndex))
				UpdateWaveformBitmap(_controller.PlayingIndex);

			QueueSpectrumRedraw();
		}

		private void QueueSpectrumRedraw()
		{
			if (Spectrum.IsDisposed)
				return;

			Spectrum.Invalidate();

			if (!Spectrum.IsHandleCreated)
				return;

			Spectrum.BeginInvoke((Action)(() =>
			{
				if (!Spectrum.IsDisposed && Spectrum.IsHandleCreated)
				{
					Spectrum.RenderFrame();
				}
			}));
		}

		/// <summary>
		/// 波形表示エリアの初期化。スキン定義に従い描画対象を設定する。
		/// </summary>
		private void SetupWaveformTarget()
		{
			if (_waveformArea != null)
			{
				Controls.Remove(_waveformArea);
				_waveformArea.Dispose();
				_waveformArea = null;
			}

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
				int x = _skinApplicator?.ScaleValue(this, wDef.Location.X) ?? wDef.Location.X;
				int y = _skinApplicator?.ScaleValue(this, wDef.Location.Y) ?? wDef.Location.Y;
				int width = _skinApplicator?.ScaleValue(this, wDef.Location.W) ?? wDef.Location.W;
				int height = _skinApplicator?.ScaleValue(this, wDef.Location.H) ?? wDef.Location.H;
				_waveformArea = new PictureBox
				{
					Location = new Point(x, y),
					Size = new System.Drawing.Size(width, height),
					BackColor = Color.Transparent,
					SizeMode = PictureBoxSizeMode.StretchImage,
				};
				Controls.Add(_waveformArea);
				_waveformArea.BringToFront();
			}
			// target="trackbar" の場合、_waveformArea は null のまま。
			// ApplyWaveformBitmap が SldTrack.BackgroundImage に描画する。
		}

		public void BtnMouseDown(object sender, MouseEventArgs e)
			=> _skinApplicator?.SetButtonDown((Button)sender);
		public void BtnMouseUp(object sender, MouseEventArgs e)
			=> _skinApplicator?.SetButtonUp((Button)sender);

		#region MainForm Event
		/// <summary>
		/// フォームロード処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void MainForm_Load(object sender, EventArgs e)
		{
			this.Visible = false;
			var syncCtx = SynchronizationContext.Current;

			_toolTip = new ToolTip(components);
			notifyIcon.Visible = false;
			notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
			notifyIcon.Icon = this.Icon;
			this.KeyPreview = true;

			_engine = new PlayerEngine();
			// 設定はエンジン初期化後に読み込む
			_config = new Configuration(_engine);

			// FMOD 初期化（最重量部分）をバックグラウンドで実行。
			// await で制御が戻りフォームが描画される。
			PlayerController ctrl = null;
			await Task.Run(() => ctrl = new PlayerController(_engine, _config, syncCtx));
			_controller = ctrl;

			_controller.TrackChanged += OnTrackChanged;
			_controller.PlaybackStateChanged += OnPlaybackStateChanged;
			_controller.WaveformReady += OnWaveformReady;
			_controller.ErrorOccurred += (s, e) =>
			{
				if (!e.IsOk)
					ShowErrorToast(e.Message);
			};

			_playListForm = new PlayListForm(this, _controller, _config);
			_fileInfoForm = new FileInfoForm(this, _controller);
			_miniPlayerForm = new MiniPlayerForm(this, _controller);
			// _optionsForm, _cdForm は初回表示時に遅延生成

			_managedForms.Add(_fileInfoForm);
			_managedForms.Add(_playListForm);
			_managedForms.Add(_miniPlayerForm);

			Spectrum.Initialize();
			SkinLoad(_config.settings.Skin);
			Spectrum.Mode = _config.settings.DefaultSpectrumMode;
			Spectrum.SnowBlockEnabled = _config.settings.SnowBlockEnabled;
			QueueSpectrumRedraw();
			SetMouseDownEvent();

			InitContextMenu();

			_openFileDialogMedia = new OpenFileDialog();
			_openFileDialogMedia.Filter = SupportedFormats.BuildOpenFileFilter();
			_openFileDialogMedia.Multiselect = true;

			this.TopMost = _config.settings.AlwaysOnTop;

			if (_config.settings.DiscordRichPresenceEnabled
				    && !string.IsNullOrWhiteSpace(_config.settings.DiscordApplicationId))
			{
				_discordPresence = new Engine.Discord.DiscordPresenceService(
					_controller, _config.settings.DiscordApplicationId);
				_discordPresence.Enabled = true;
			}

			if (_config.settings.AutoUpdateCheckEnabled
				&& !string.IsNullOrWhiteSpace(_config.settings.UpdateGitHubRepo))
				_ = CheckForUpdateOnStartupAsync();

			initialize = true;

			// 起動パラメータを取得し、ファイルパスが取得出来るならばOpen関数へ引き渡す
			string[] parameters = System.Environment.GetCommandLineArgs();
			if (parameters.Length > 1 && File.Exists(parameters[1]))
			{
				_controller.OpenAndPlay(parameters[1]);
			}
			this.Opacity = 1;
			this.Visible = true;
			StartupReady?.Invoke(this, EventArgs.Empty);
		}
		private async Task CheckForUpdateOnStartupAsync()
		{
			var syncCtx = SynchronizationContext.Current;
			var info = await Task.Run(() =>
				UpdateChecker.CheckAsync(_config.settings.UpdateGitHubRepo));
			if (info == null)
				return;
			syncCtx.Post(_ => new UpdateAvailableDialog(info).ShowDialog(this), null);
		}

		private void OnPlaybackStateChanged() { }
		private void OnTrackChanged(int index)
		{
			// インデックスが不正なら何もしない（再生停止などで -1 になることがあるため）
			if (!_controller.IsValidTrackIndex(index)) 
				return;

            _abStart = -1f;
            _abEnd = -1f;
            _controller.ClearAbRepeat();

            SldTrack.Maximum = ClampTrackSliderValue(_controller.GetLength());
			SldTrack.Value = 0;
			_controller.SetVolume(SldVolume.Value);
			_controller.SetPan(SldPan.Value);

			if (_fileInfoForm != null && _fileInfoForm.Visible)
				_fileInfoForm.LoadInfo();

			LabelTitle.Value.Text = _controller.BuildTitleText();
			LoadMainCoverArt(index);

			_waveformBitmap?.Dispose();
			_waveformBitmap = null;
			if (_waveformArea != null) _waveformArea.Image = null;
			else SldTrack.BackgroundImage = null;
			var entry = _controller.Engine.PlayList[index];
			if (_controller.Engine.WaveformEnabled && entry.WaveformReady)
				UpdateWaveformBitmap(index);
		}
		private void OnWaveformReady(int index)
		{
			if (!_controller.Engine.WaveformEnabled) return;
			if (index != _controller.Engine.PlayingIndex) return;
			UpdateWaveformBitmap(index);
		}

		private void LoadMainCoverArt(int index)
		{
			_coverArtCts?.Cancel();
			_coverArtCts?.Dispose();
			_coverArtCts = new CancellationTokenSource();
			var ct = _coverArtCts.Token;

			if (!_controller.IsValidTrackIndex(index))
			{
				SetMainCoverImage(CreateDummyCoverArt());
				return;
			}

			var cover = _controller.Engine.GetCoverArt(index);
			if (cover != null)
			{
				SetMainCoverImage(cover);
				return;
			}

			SetMainCoverImage(CreateDummyCoverArt());
			_ = FetchMainCoverArtFallbackAsync(index, ct);
		}

		private async Task FetchMainCoverArtFallbackAsync(int index, CancellationToken ct)
		{
			if (!_controller.IsValidTrackIndex(index)) return;
			var item = _controller.Engine.PlayList[index];
			Image img = null;

			var result = _controller.Engine.GetTag("COVERART", index, out FMOD.TAG coverTag);
			if (result == FMOD.RESULT.OK && coverTag.datatype == FMOD.TAGDATATYPE.BINARY)
			{
				try
				{
					byte[] imgData = new byte[coverTag.datalen];
					System.Runtime.InteropServices.Marshal.Copy(coverTag.data, imgData, 0, (int)coverTag.datalen);
					using var ms = new MemoryStream(imgData);
					using var tmp = Image.FromStream(ms);
					img = new Bitmap(tmp);
					ApplyFetchedMainCover(index, img, ct);
					return;
				}
				catch
				{
				}
			}

			if (!string.IsNullOrEmpty(item.MusicBrainzDiscId))
			{
				try
				{
					img = await CoverArtClient.FetchByDiscIdAsync(item.MusicBrainzDiscId, ct);
				}
				catch
				{
				}
			}

			if (img == null && item.IsCueTrack && item.CueSheetRef != null
				&& !string.IsNullOrEmpty(item.CueSheetRef.DiscId)
				&& (string.IsNullOrEmpty(item.Artist) || string.IsNullOrEmpty(item.Album)))
			{
				try
				{
					var cddbResults = await Engine.CD.CddbClient.QueryByCueAsync(
						item.CueSheetRef,
						_config.settings.CddbServers,
						ct);

					if (cddbResults.Count > 0)
					{
						var best = cddbResults[0];
						var firstCueTrack = _controller.Engine.PlayList.First(p =>
							p.IsCueTrack && p.CueSheetRef == item.CueSheetRef);

						for (int i = 0; i < _controller.Engine.PlayList.Count; i++)
						{
							var entry = _controller.Engine.PlayList[i];
							if (!entry.IsCueTrack || entry.CueSheetRef != item.CueSheetRef) continue;

							int trackIdx = i - _controller.Engine.PlayList.IndexOf(firstCueTrack);
							if (string.IsNullOrEmpty(entry.Artist))
								entry.Artist = best.Artist ?? "";
							if (string.IsNullOrEmpty(entry.Album))
								entry.Album = best.Album ?? "";
							if (trackIdx >= 0 && trackIdx < best.Tracks.Count && entry.Title.StartsWith("Track "))
								entry.Title = best.Tracks[trackIdx];
						}
					}
				}
				catch
				{
				}
			}

			if (img == null)
			{
				const int waitMs = 500;
				const int maxRetries = 6;
				string artist = null;
				string album = null;

				for (int i = 0; i < maxRetries; i++)
				{
					if (ct.IsCancellationRequested) return;
					if (!_controller.IsValidTrackIndex(index)) return;

					artist = _controller.Engine.PlayList[index].Artist;
					album = _controller.Engine.PlayList[index].Album;

					if (!string.IsNullOrEmpty(album)) break;
					await Task.Delay(waitMs, ct);
				}

				if (!string.IsNullOrEmpty(album))
				{
					try
					{
						img = await CoverArtClient.FetchByArtistAlbumAsync(artist, album, ct);
					}
					catch
					{
					}
				}
			}

			ApplyFetchedMainCover(index, img, ct);
		}

		private void ApplyFetchedMainCover(int index, Image img, CancellationToken ct)
		{
			if (img == null)
				return;
			if (ct.IsCancellationRequested || IsDisposed || picCover.IsDisposed
				|| index != _controller.Engine.PlayingIndex)
			{
				img.Dispose();
				return;
			}

			if (InvokeRequired)
				BeginInvoke(new Action(() => ApplyFetchedMainCover(index, img, ct)));
			else
				SetMainCoverImage(img);
		}

		private void SetMainCoverImage(Image image)
		{
			var old = picCover.Image;
			picCover.Image = image;
			picCover.Invalidate();
			if (old != null && !ReferenceEquals(old, image))
				old.Dispose();
		}

		private Image CreateDummyCoverArt()
		{
			var width = Math.Max(1, picCover.Width);
			var height = Math.Max(1, picCover.Height);
			var bmp = new Bitmap(width, height);
			using var g = Graphics.FromImage(bmp);
			g.Clear(Color.DimGray);
			using var font = new Font("Arial", 10);
			var text = "No Image";
			var textSize = g.MeasureString(text, font);
			g.DrawString(
				text,
				font,
				Brushes.White,
				new PointF((width - textSize.Width) / 2f, (height - textSize.Height) / 2f));
			return bmp;
		}

		private void UpdateWaveformBitmap(int index)
		{
			// インデックスが不正なら何もしない（安全策。通常は OnTrackChanged で既にチェック済みのはず）
			if (!_controller.IsValidTrackIndex(index))
				return;

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
					BtnPlay_Click(this, EventArgs.Empty);
					return true;

				case Keys.Enter:
					if (_controller.IsPlaying)
						_controller.SetPosition(0);
					else
						_controller.PlayAt(0);
					return true;

				case Keys.S:
					BtnStop_Click(this, EventArgs.Empty);
					return true;

				case Keys.X:
					_controller.PlayNext(true);
					return true;

				case Keys.Z:
					_controller.PlayPrevious(true);
					return true;

				// ── シーク（キーリピートによる加速は SeekiTimer と seeking フラグで実装）──

				case Keys.Right:
					seeking = 1;
					return true;

				case Keys.Left:
					seeking = 2;
					return true;

				// ── 音量 ──────────────────────────────────────────────────

				case Keys.Up:
					SldVolume.Value = Math.Min(SldVolume.Value + 5, SldVolume.Maximum);
					_controller.SetVolume(SldVolume.Value);
					return true;

				case Keys.Down:
					SldVolume.Value = Math.Max(SldVolume.Value - 5, SldVolume.Minimum);
					_controller.SetVolume(SldVolume.Value);
					return true;

				// ── モード切替 ────────────────────────────────────────────

				case Keys.L:
					BtnLoop_Click(BtnLoop, EventArgs.Empty);
					return true;

				case Keys.R:
					BtnRandom_Click(BtnRandom, EventArgs.Empty);
					return true;

				// ── UI ────────────────────────────────────────────────────

				case Keys.Escape:
					BtnMinisize_Click(this, EventArgs.Empty);
					return true;

                case Keys.A:
					_abStart = (float)_controller.GetPosition()
							 / Math.Max(1f, _controller.GetLength());
					_controller.SetAbStart((uint)_controller.GetPosition());
					_controller.UpdatePreciseTimer();
					return true;

				case Keys.B:
					// A点より後のみ有効
					if (_controller.AbRepeatEnabled == false
						|| (uint)_controller.GetPosition() > _controller.AbStart)
					{
						_abEnd = (float)_controller.GetPosition()
							   / Math.Max(1f, _controller.GetLength());
						_controller.SetAbEnd((uint)_controller.GetPosition());
						_controller.UpdatePreciseTimer();
					}
					return true;

				case Keys.C:
					_abStart = -1f;
					_abEnd = -1f;
					_controller.ClearAbRepeat();
					_controller.UpdatePreciseTimer();
					return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		/// <summary>
		/// マウス押下でウィンドウ移動の基点を記録し、関連フォームを前面に出す。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_MouseDown(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				mousePoint = new Point(e.X, e.Y);
				this.Activate();

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
						_skinApplicator?.UpdatePlayListPosition(this, _playListForm);
					}
				}

				_currentToast?.UpdatePosition(this);
			}
		}

		private void ShowErrorToast(string message)
		{
			if (_currentToast != null && !_currentToast.IsDisposed)
				_currentToast.Close();

			_currentToast = new Forms.ErrorToastForm(message);
			_currentToast.FormClosed += (s, e) => _currentToast = null;
			_currentToast.ShowToast(this);
		}

		/// <summary>
		/// フォームクローズ処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
            D2DContext.Dispose();
            _discordPresence?.Dispose();
            _coverArtCts?.Cancel();
            _coverArtCts?.Dispose();
            picCover.Image?.Dispose();
            picCover.Image = null;
            _controller.Close();
			_config.Save();
			_fileInfoForm?.Dispose();
			_cdForm?.Dispose();   // 追加
            _miniPlayerForm?.Dispose();
            _engine.Dispose();
			_engine = null;
			notifyIcon.Visible = false;
			notifyIcon.Dispose();
			SkinPackage.CleanupTempDirectory();
		}
		#endregion
		#region Timer Event

		/// <summary>
		/// タイマー処理。リアルタイム処理が必要なものはすべてここで処理する。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PlayerTimer_Tick(object sender, EventArgs e)
		{
			if (!initialize || _engine == null || _engine.spectrum == null)
				return;

			_spectrumUpdateCounter += Timer.Interval;
			if (_spectrumUpdateCounter >= _config.settings.SpectrumUpdateIntervalMs)
			{
				_spectrumUpdateCounter = 0;
				Spectrum.mFFT = _engine.spectrum.UpdateSpectrum();
				Spectrum.mWaveL = _engine.wave.GetWaveDataByChannel(0);
				Spectrum.mWaveR = _engine.wave.GetWaveDataByChannel(1);
				Spectrum.RenderFrame();
				if (!Spectrum.Visible)
					Spectrum.Visible = true;
			}
			// シーク中は SeekiTimer 側でスライダーを動かすためスキップする
			if (this.seekValue == 0)
				SldTrack.Value = Math.Min(
					ClampTrackSliderValue(_controller.GetPosition()),
					SldTrack.Maximum);

			TimeSpan time1 = TimeSpan.FromMilliseconds(SldTrack.Value);
			TimeSpan time2 = TimeSpan.FromMilliseconds(SldTrack.Maximum);
			LabelTime.Value.Text = time1.ToString(@"hh\:mm\:ss") + "/" + time2.ToString(@"hh\:mm\:ss");

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
			_controller.OnTimerTick(Timer.Interval);
		}
		private void UpdateWaveformPlayedRatio(float ratio)
		{
			if (!_controller.IsValidTrackIndex(_controller.PlayingIndex))
				return;

			var wDef = (_currentSkin)?.WaveForm;
			if (wDef == null) return;  // スキン未定義なら何もしない

			var entry = _controller.Engine.PlayList[_controller.PlayingIndex];
			if (!entry.WaveformReady) return;

			var (w, h) = GetWaveformSize(wDef);

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
		/// <summary>WaveformDef からカラー設定を構築。UseCustomWaveformColors=true の場合は設定値を優先する。</summary>
		private WaveformRenderer.WaveformColors BuildWaveformColors(
			WaveformComponents wDef)
		{
			var s = _config.settings;
			if (s.UseCustomWaveformColors)
			{
				return new WaveformRenderer.WaveformColors
				{
					ColorL   = ParseSkinColor(s.WaveformColorL),
					ColorR   = ParseSkinColor(s.WaveformColorR),
					ColorMix = ParseSkinColor(s.WaveformColorMix),
					Played   = ParseSkinColor(s.WaveformColorPlayed),
					Unplayed = ParseSkinColor(s.WaveformColorUnplayed),
				};
			}
			return new WaveformRenderer.WaveformColors
			{
				ColorL   = wDef.ColorL,
				ColorR   = wDef.ColorR,
				ColorMix = wDef.ColorMix,
				Played   = wDef.ColorPlayed,
				Unplayed = wDef.ColorUnplayed,
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
					var files = _openFileDialogMedia.FileNames;
					if (files.Length > 1)
						_controller.OpenMultipleAndPlay(files);
					else
						_controller.OpenAndPlay(files[0]);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("ファイルのオープンに失敗しました。\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				// ダイアログを閉じた直後の最初の MouseDown は意図しない操作になるため抑制する
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
			_controller.Stop();
		}


		/// <summary>
		/// 閉じるボタンのクリック
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnClose_Click(object sender, EventArgs e)
		{
			_fileInfoForm?.Close();
			_fileInfoForm?.Dispose();
			_playListForm?.Close();
			_playListForm?.Dispose();
			_optionsForm?.Close();
			_optionsForm?.Dispose();
			_cdForm?.Close();
			_cdForm?.Dispose();
			_miniPlayerForm?.Hide();
			_miniPlayerForm?.Dispose();
			Close();
		}
		public void FormClose()
		{
			BtnClose_Click(this, EventArgs.Empty);
		}
		private void BtnBack_Click(object sender, EventArgs e)
		{
			_controller.SetVolume(SldVolume.Value);
			_controller.PlayPrevious(true);
		}
		private void BtnSeekBack_Click(object sender, EventArgs e)
		{
		}
		private void BtnPause_Click(object sender, EventArgs e)
		{
			_controller.SetVolume(SldVolume.Value);
			_controller.TogglePlayPause();
		}
		private void BtnSeekForward_Click(object sender, EventArgs e)
		{
		}
		private void BtnNext_Click(object sender, EventArgs e)
		{
			_controller.SetVolume(SldVolume.Value);
			_controller.PlayNext(true);
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
			if (_optionsForm == null)
			{
				_optionsForm = new OptionsForm(this, _controller, _config);
				_managedForms.Add(_optionsForm);
			}
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
				_skinApplicator?.UpdatePlayListPosition(this, _playListForm);
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
		private void NotifyIcon_DoubleClick(object sender, EventArgs e)
		{
			this.Show();
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

			if (_controller.IsValidTrackIndex(_controller.PlayingIndex) == false)
				return;

			seekValue = Math.Min(seekValue + SeekStep, SeekMaxValue);
			if (seeking == 1)
			{
				long newValue = (long)SldTrack.Value + seekValue;
				SldTrack.Value = (int)Math.Min(newValue, SldTrack.Maximum);
			}
			else if (seeking == 2)
			{
				long newValue = (long)SldTrack.Value - seekValue;
				SldTrack.Value = (int)Math.Max(newValue, SldTrack.Minimum);
			}
		}

		private void Spectrum_Click(object sender, EventArgs e)
		{
			Spectrum.Mode = (Spectrum.Mode + 1) % 5;
		}

		private void BtnCD_Click(object sender, EventArgs e)
		{
			if (_cdForm == null)
			{
				_cdForm = new CDForm(this, _controller, _config);
				_managedForms.Add(_cdForm);
			}
			_cdForm.Show();
		}

		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
			string[] fileName =
				(string[])e.Data.GetData(DataFormats.FileDrop, false);

			_controller.OpenFiles(fileName);
		}

		private void MainForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
			else
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
			ConfigureContextMenu(contextMenu);
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
			menuAutoUpdateCheck.Click += async (s, e) =>
			{
				var info = await UpdateChecker.CheckAsync(_config.settings.UpdateGitHubRepo);
				if (info == null)
					MessageBox.Show("最新バージョンを使用中です。", "更新確認",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				else
					new UpdateAvailableDialog(info).ShowDialog(this);
			};
			var menuAbout = new ToolStripMenuItem("About(&C)", null, null, "menuAbout");
			var menuHelp = new ToolStripMenuItem("ヘルプ(&V)", null, null, "menuHelp");
			var menuMinimize = new ToolStripMenuItem("最小化(&X)", null, BtnMinisize_Click, "menuMinimize");
			var menuExit = new ToolStripMenuItem("閉じる(&Z)", null, BtnClose_Click, "menuExit");

			var menuFileInfo = new ToolStripMenuItem("ファイル情報");
			var menuPlayback = new ToolStripMenuItem("再生操作");
			var menuLibrary = new ToolStripMenuItem("ライブラリ");
			var menuCustomize = new ToolStripMenuItem("設定と拡張");
			var menuSupport = new ToolStripMenuItem("ヘルプと更新");

			menuPlayMode.DropDownItems.AddRange(new ToolStripItem[] {
				menuPlayModeNormal, menuPlayModeRandom, menuPlayModeRepeat, menuPlayModeLoop });
			menuPlayback.DropDownItems.AddRange(new ToolStripItem[]
			{
				menuPlay,
				menuPause,
				menuStop,
				new ToolStripSeparator(),
				menuBack,
				menuNext,
				new ToolStripSeparator(),
				menuPlayMode,
			});
			menuLibrary.DropDownItems.AddRange(new ToolStripItem[]
			{
				menuFileInfo,
				menuPlayList,
				menuSleep,
			});
			menuCustomize.DropDownItems.AddRange(new ToolStripItem[]
			{
				menuOption,
				menuEffects,
				menuEqualizer,
				menuExtensions,
				menuSkinSelect,
			});
			menuSupport.DropDownItems.AddRange(new ToolStripItem[]
			{
				menuAutoUpdateCheck,
				menuAbout,
				menuHelp,
			});

			menuPlayList.Click += (s, e) => BtnPlaylist_Click(s, e);
			menuOption.Click += (s, e) => BtnSetting_Click(s, e);
			menuHelp.Click += MenuHelp_Click;
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
			menuEffects.Click += (s, e) => OpenOptionsTab("PITCH");
			menuEqualizer.Click += (s, e) => OpenOptionsTab("GEQ");
			menuExtensions.Click += (s, e) => OpenOptionsTab("EXTENSIONS");
			menuSkinSelect.Click += (s, e) => OpenOptionsTab("SKIN");
			menuAbout.Click += (s, e) => OpenOptionsTab("ABOUT");

			contextMenu.Items.AddRange(new ToolStripItem[] {
				menuOpen,
				menuUrlOpen,
				new ToolStripSeparator(),
				menuPlayback,
				menuLibrary,
				menuCustomize,
				new ToolStripSeparator(),
				menuSupport,
				new ToolStripSeparator(),
				menuMinimize,
				menuExit
			});
			contextMenu.Name = "contextMenu";

			// PlayMode
			menuPlayModeNormal.Click += (s, e) => { _controller.SetLoopMode(LOOP_MODE.LOOP_NONE); _skinApplicator?.UpdateLoopButton(BtnLoop, _controller.GetLoopMode()); };
			menuPlayModeRandom.Click += (s, e) => { _controller.ToggleRandom(); _skinApplicator?.UpdateRandomButton(BtnRandom, _controller.GetLoopMode()); };
			menuPlayModeRepeat.Click += (s, e) => { _controller.SetLoopMode(LOOP_MODE.LOOP_ONE_REPEAT); _skinApplicator?.UpdateLoopButton(BtnLoop, _controller.GetLoopMode()); };
			menuPlayModeLoop.Click += (s, e) => {_controller.SetLoopMode(LOOP_MODE.LOOP_ALL); _skinApplicator?.UpdateLoopButton(BtnLoop, _controller.GetLoopMode()); };

			contextMenu.Opening += ContextMenu_Opening;


			var trayMenuRestore = new ToolStripMenuItem("復元");
			var trayMenuExit = new ToolStripMenuItem("終了");

			trayMenuRestore.Click += (s, e) => NotifyIcon_DoubleClick(s, e);
			trayMenuExit.Click += (s, e) => Application.Exit();

			notifyIcon.ContextMenuStrip = new ContextMenuStrip();
			ConfigureContextMenu(notifyIcon.ContextMenuStrip);
			notifyIcon.ContextMenuStrip.Items.AddRange(new ToolStripItem[]
			{
				trayMenuRestore,
				new ToolStripSeparator(),
				trayMenuExit,
			});
		}

		private static void ConfigureContextMenu(ContextMenuStrip menu)
		{
			menu.ShowImageMargin = false;
			menu.ShowCheckMargin = true;
			menu.AutoSize = true;
		}

		private void MainForm_DpiChanged(object sender, DpiChangedEventArgs e)
		{
			if (!IsHandleCreated || _currentSkin == null)
				return;

			BeginInvoke((Action)ReapplyCurrentSkinLayout);
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
			menuPlayModeNormal.Checked = (_controller.GetLoopMode() & LOOP_MODE.LOOP_NONE) != 0;
			menuPlayModeRandom.Checked = (_controller.GetLoopMode() & LOOP_MODE.LOOP_RANDOM) != 0;
			menuPlayModeRepeat.Checked = (_controller.GetLoopMode() & LOOP_MODE.LOOP_ONE_REPEAT) != 0;
			menuPlayModeLoop.Checked = (_controller.GetLoopMode() & LOOP_MODE.LOOP_ALL) != 0;
		}
        public void SetDiscordPresenceEnabled(bool enabled, string applicationId)
        {
            if (_discordPresence != null)
            {
                _discordPresence.Dispose();
                _discordPresence = null;
            }
            if (enabled && !string.IsNullOrWhiteSpace(applicationId))
            {
                _discordPresence = new Engine.Discord.DiscordPresenceService(
                    _controller, applicationId);
                _discordPresence.Enabled = true;
            }
        }

		private void OpenOptionsTab(string tabName)
		{
			if (_optionsForm == null)
			{
				_optionsForm = new OptionsForm(this, _controller, _config);
				_managedForms.Add(_optionsForm);
			}
			_optionsForm.Show();
			_optionsForm.SelectTab(tabName);
		}

		private void MenuHelp_Click(object sender, EventArgs e)
		{
			string helpPath = Path.Combine(Application.StartupPath, "Docs", "help.html");
			if (!File.Exists(helpPath))
			{
				MessageBox.Show("ヘルプファイルが見つかりません。\n" + helpPath,
					"ヘルプ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = helpPath,
					UseShellExecute = true,
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show("ヘルプを開けませんでした。\n" + ex.Message,
					"ヘルプ", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
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
		private const int WM_SYSCOMMAND = 0x0112;
		private const int SC_CLOSE = 0xF060;

		private const int FAPPCOMMAND_MASK = 0xF000;
		private const int FAPPCOMMAND_MOUSE = 0x8000;

		private const int APPCOMMAND_MEDIA_PLAY_PAUSE = 14;
		private const int APPCOMMAND_MEDIA_STOP = 13;
		private const int APPCOMMAND_MEDIA_NEXTTRACK = 11;
		private const int APPCOMMAND_MEDIA_PREVIOUSTRACK = 12;

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_SYSCOMMAND && (int)(m.WParam.ToInt64() & 0xFFF0) == SC_CLOSE)
			{
				BtnClose_Click(this, EventArgs.Empty);
				return;
			}
			if (m.Msg == WM_APPCOMMAND)
			{
				int device = (int)(m.LParam.ToInt64() >> 16) & FAPPCOMMAND_MASK;
				if (device == FAPPCOMMAND_MOUSE)
				{
					base.WndProc(ref m);
					return;
				}

				int command = (int)(m.LParam.ToInt64() >> 16) & 0xFFF;
				bool handled = true;
				switch (command)
				{
					case APPCOMMAND_MEDIA_PLAY_PAUSE:
						BtnPlay_Click(this, EventArgs.Empty);
						break;
					case APPCOMMAND_MEDIA_STOP:
						BtnStop_Click(this, EventArgs.Empty);
						break;
					case APPCOMMAND_MEDIA_NEXTTRACK:
						_controller.PlayNext(true);
						break;
					case APPCOMMAND_MEDIA_PREVIOUSTRACK:
						_controller.PlayPrevious(true);
						break;
					default:
						handled = false;
						break;
				}
				if (handled)
				{
					m.Result = (IntPtr)1;
					return;
				}
			}
			base.WndProc(ref m);
		}
        public void RestoreFromMini()
        {
            _miniPlayerForm.Hide();
            this.Show();
            this.Activate();
        }
    }
}
