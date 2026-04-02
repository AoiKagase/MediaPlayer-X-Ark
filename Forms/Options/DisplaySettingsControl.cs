using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
	public class DisplaySettingsControl : OptionsControlBase
	{
		// ── スペクトラム（既存） ──────────────────────────────────────
		private ComboBox _cmbSpectrumMode;
		private CheckBox _chkSnowBlock;

		// ── スペクトラム更新間隔 ──────────────────────────────────────
		private TrackBar _trackSpectrumInterval;
		private Label _lblIntervalValue;

		// ── ウェーブ色 ────────────────────────────────────────────────
		private Button _btnWaveColorL;
		private Button _btnWaveColorR;
		private CheckBox _chkWaveColorOverride;
		private Color _waveColorL = Color.Lime;
		private Color _waveColorR = Color.Cyan;

		// ── バー色 ────────────────────────────────────────────────────
		private Button _btnBarColor;
		private CheckBox _chkBarColorOverride;
		private Color _barColor = Color.White;

		// ── スノー落下速度 ────────────────────────────────────────────
		private TrackBar _trackSnowSpeed;
		private Label _lblSnowSpeed;

		// ── ウェーブフォーム色 ────────────────────────────────────────
		private Button _btnWfColorL;
		private Button _btnWfColorR;
		private Button _btnWfColorMix;
		private Button _btnWfColorPlayed;
		private Button _btnWfColorUnplayed;
		private CheckBox _chkWaveformColorOverride;
		private Color _wfColorL = Color.FromArgb(0, 200, 100);
		private Color _wfColorR = Color.FromArgb(0, 100, 200);
		private Color _wfColorMix = Color.FromArgb(0, 180, 120);
		private Color _wfColorPlayed = Color.FromArgb(100, 100, 100);
		private Color _wfColorUnplayed = Color.FromArgb(50, 50, 50);

		// ── タイトルフォント ──────────────────────────────────────────
		private ComboBox _cmbTitleFont;
		private NumericUpDown _numTitleFontSize;
		private CheckBox _chkTitleBold;
		private CheckBox _chkTitleFontOverride;

		// ── 時間表示フォント ──────────────────────────────────────────
		private ComboBox _cmbTimeFont;
		private NumericUpDown _numTimeFontSize;
		private CheckBox _chkTimeBold;
		private CheckBox _chkTimeFontOverride;

		// ── タイトルスクロール速度 ────────────────────────────────────
		private TrackBar _trackTitleScroll;
		private Label _lblScrollSpeed;
		private CheckBox _chkScrollOverride;

		private Button _btnSave;
		private MainForm _mainForm;

		public DisplaySettingsControl(IPlayerEngine engine, IConfigService config, MainForm mainForm)
			: base(engine, config)
		{
			_mainForm = mainForm;
			AutoScroll = true;
			AutoScrollMinSize = new Size(460, 0);
			BuildLayout();
		}

		private void BuildLayout()
		{
			var y = 16;
			const int grpMargin = 12;
			const int controlX = 134;

			// ═══════════════════════════════════════════════════
			// GroupBox A: スペクトラム基本設定
			// ═══════════════════════════════════════════════════
			var grpSpectrum = new GroupBox
			{
				Text = "スペクトラム",
				Location = new Point(16, y),
				Size = new Size(440, 200),
			};

			// デフォルトモード
			grpSpectrum.Controls.Add(new Label
			{
				Text = "デフォルトモード",
				Location = new Point(12, 26),
				AutoSize = true,
			});
			_cmbSpectrumMode = new ComboBox
			{
				Location = new Point(controlX, 22),
				Size = new Size(200, 23),
				DropDownStyle = ComboBoxStyle.DropDownList,
			};
			_cmbSpectrumMode.Items.AddRange(new object[]
			{
				"0: スペクトラム（密）",
				"1: スペクトラム",
				"2: スペクトラム（中）",
				"3: スペクトラム（疎）",
				"4: ウェーブフォーム",
			});
			grpSpectrum.Controls.Add(_cmbSpectrumMode);

			// SnowBlock
			_chkSnowBlock = new CheckBox
			{
				Text = "SnowBlock モードを有効にする",
				Location = new Point(12, 52),
				AutoSize = true,
			};
			grpSpectrum.Controls.Add(_chkSnowBlock);

			// SnowBlock 落下速度
			grpSpectrum.Controls.Add(new Label
			{
				Text = "落下速度",
				Location = new Point(12, 82),
				AutoSize = true,
			});
			_trackSnowSpeed = new TrackBar
			{
				Location = new Point(controlX, 76),
				Size = new Size(180, 30),
				Minimum = 2,
				Maximum = 50,
				TickFrequency = 5,
				SmallChange = 1,
				LargeChange = 5,
			};
			_trackSnowSpeed.ValueChanged += (_, _) =>
				_lblSnowSpeed.Text = $"{_trackSnowSpeed.Value} px/秒";
			grpSpectrum.Controls.Add(_trackSnowSpeed);

			_lblSnowSpeed = new Label
			{
				Location = new Point(320, 82),
				Size = new Size(80, 20),
				Text = "12 px/秒",
			};
			grpSpectrum.Controls.Add(_lblSnowSpeed);

			// 更新間隔
			grpSpectrum.Controls.Add(new Label
			{
				Text = "更新間隔",
				Location = new Point(12, 122),
				AutoSize = true,
			});
			_trackSpectrumInterval = new TrackBar
			{
				Location = new Point(controlX, 116),
				Size = new Size(180, 30),
				Minimum = 33,
				Maximum = 250,
				TickFrequency = 10,
				SmallChange = 5,
				LargeChange = 20,
			};
			_trackSpectrumInterval.ValueChanged += (_, _) =>
				_lblIntervalValue.Text = FormatInterval(_trackSpectrumInterval.Value);
			grpSpectrum.Controls.Add(_trackSpectrumInterval);

			_lblIntervalValue = new Label
			{
				Location = new Point(320, 122),
				Size = new Size(110, 20),
				Text = "60ms / 16fps",
			};
			grpSpectrum.Controls.Add(_lblIntervalValue);

			y += grpSpectrum.Height + grpMargin;
			Controls.Add(grpSpectrum);

			// ═══════════════════════════════════════════════════
			// GroupBox B: スペクトラム表示色
			// ═══════════════════════════════════════════════════
			var grpColor = new GroupBox
			{
				Text = "スペクトラム表示色",
				Location = new Point(16, y),
				Size = new Size(440, 170),
			};

			// ウェーブL
			grpColor.Controls.Add(new Label
			{
				Text = "ウェーブ L",
				Location = new Point(12, 28),
				AutoSize = true,
			});
			_btnWaveColorL = MakeColorButton(new Point(controlX, 24));
			_btnWaveColorL.Click += (_, _) => PickColor(ref _waveColorL, _btnWaveColorL);
			grpColor.Controls.Add(_btnWaveColorL);

			// ウェーブR
			grpColor.Controls.Add(new Label
			{
				Text = "ウェーブ R",
				Location = new Point(12, 58),
				AutoSize = true,
			});
			_btnWaveColorR = MakeColorButton(new Point(controlX, 54));
			_btnWaveColorR.Click += (_, _) => PickColor(ref _waveColorR, _btnWaveColorR);
			grpColor.Controls.Add(_btnWaveColorR);

			_chkWaveColorOverride = new CheckBox
			{
				Text = "設定の色をスキンより優先する",
				Location = new Point(12, 82),
				AutoSize = true,
			};
			grpColor.Controls.Add(_chkWaveColorOverride);

			// バー色
			grpColor.Controls.Add(new Label
			{
				Text = "スペクトラムバー",
				Location = new Point(12, 112),
				AutoSize = true,
			});
			_btnBarColor = MakeColorButton(new Point(controlX, 108));
			_btnBarColor.Click += (_, _) => PickColor(ref _barColor, _btnBarColor);
			grpColor.Controls.Add(_btnBarColor);

			_chkBarColorOverride = new CheckBox
			{
				Text = "設定の色をスキンより優先する（スキンの画像より優先）",
				Location = new Point(12, 138),
				AutoSize = true,
			};
			grpColor.Controls.Add(_chkBarColorOverride);

			y += grpColor.Height + grpMargin;
			Controls.Add(grpColor);

			// ═══════════════════════════════════════════════════
			// GroupBox C: ウェーブフォーム表示色
			// ═══════════════════════════════════════════════════
			var grpWaveform = new GroupBox
			{
				Text = "ウェーブフォーム表示色",
				Location = new Point(16, y),
				Size = new Size(440, 196),
			};

			grpWaveform.Controls.Add(new Label { Text = "ウェーブフォーム L", Location = new Point(12, 28), AutoSize = true });
			_btnWfColorL = MakeColorButton(new Point(controlX, 24));
			_btnWfColorL.Click += (_, _) => PickColor(ref _wfColorL, _btnWfColorL);
			grpWaveform.Controls.Add(_btnWfColorL);

			grpWaveform.Controls.Add(new Label { Text = "ウェーブフォーム R", Location = new Point(12, 58), AutoSize = true });
			_btnWfColorR = MakeColorButton(new Point(controlX, 54));
			_btnWfColorR.Click += (_, _) => PickColor(ref _wfColorR, _btnWfColorR);
			grpWaveform.Controls.Add(_btnWfColorR);

			grpWaveform.Controls.Add(new Label { Text = "ミックス", Location = new Point(12, 88), AutoSize = true });
			_btnWfColorMix = MakeColorButton(new Point(controlX, 84));
			_btnWfColorMix.Click += (_, _) => PickColor(ref _wfColorMix, _btnWfColorMix);
			grpWaveform.Controls.Add(_btnWfColorMix);

			grpWaveform.Controls.Add(new Label { Text = "再生済み", Location = new Point(12, 118), AutoSize = true });
			_btnWfColorPlayed = MakeColorButton(new Point(controlX, 114));
			_btnWfColorPlayed.Click += (_, _) => PickColor(ref _wfColorPlayed, _btnWfColorPlayed);
			grpWaveform.Controls.Add(_btnWfColorPlayed);

			grpWaveform.Controls.Add(new Label { Text = "未再生", Location = new Point(12, 148), AutoSize = true });
			_btnWfColorUnplayed = MakeColorButton(new Point(controlX, 144));
			_btnWfColorUnplayed.Click += (_, _) => PickColor(ref _wfColorUnplayed, _btnWfColorUnplayed);
			grpWaveform.Controls.Add(_btnWfColorUnplayed);

			_chkWaveformColorOverride = new CheckBox
			{
				Text = "設定の色をスキンより優先する",
				Location = new Point(12, 172),
				AutoSize = true,
			};
			grpWaveform.Controls.Add(_chkWaveformColorOverride);

			y += grpWaveform.Height + grpMargin;
			Controls.Add(grpWaveform);

			// ═══════════════════════════════════════════════════
			// GroupBox D: テキスト表示
			// ═══════════════════════════════════════════════════
			var grpText = new GroupBox
			{
				Text = "テキスト表示",
				Location = new Point(16, y),
				Size = new Size(440, 210),
			};

			// タイトルフォント
			grpText.Controls.Add(new Label
			{
				Text = "タイトルフォント",
				Location = new Point(12, 26),
				AutoSize = true,
			});
			_cmbTitleFont = MakeFontCombo(new Point(controlX, 22), 160);
			grpText.Controls.Add(_cmbTitleFont);
			grpText.Controls.Add(new Label
			{
				Text = "サイズ",
				Location = new Point(302, 26),
				AutoSize = true,
			});
			_numTitleFontSize = MakeSizeSpinner(new Point(348, 22));
			grpText.Controls.Add(_numTitleFontSize);

			_chkTitleBold = new CheckBox
			{
				Text = "太字",
				Location = new Point(controlX, 52),
				AutoSize = true,
			};
			grpText.Controls.Add(_chkTitleBold);

			_chkTitleFontOverride = new CheckBox
			{
				Text = "設定のフォントをスキンより優先する",
				Location = new Point(12, 76),
				AutoSize = true,
			};
			grpText.Controls.Add(_chkTitleFontOverride);

			// 区切り線（Label で代用）
			grpText.Controls.Add(new Label
			{
				BorderStyle = BorderStyle.Fixed3D,
				Location = new Point(12, 104),
				Size = new Size(410, 2),
			});

			// 時間表示フォント
			grpText.Controls.Add(new Label
			{
				Text = "時間表示フォント",
				Location = new Point(12, 116),
				AutoSize = true,
			});
			_cmbTimeFont = MakeFontCombo(new Point(controlX, 112), 160);
			grpText.Controls.Add(_cmbTimeFont);
			grpText.Controls.Add(new Label
			{
				Text = "サイズ",
				Location = new Point(302, 116),
				AutoSize = true,
			});
			_numTimeFontSize = MakeSizeSpinner(new Point(348, 112));
			grpText.Controls.Add(_numTimeFontSize);

			_chkTimeBold = new CheckBox
			{
				Text = "太字",
				Location = new Point(controlX, 142),
				AutoSize = true,
			};
			grpText.Controls.Add(_chkTimeBold);

			_chkTimeFontOverride = new CheckBox
			{
				Text = "設定のフォントをスキンより優先する",
				Location = new Point(12, 166),
				AutoSize = true,
			};
			grpText.Controls.Add(_chkTimeFontOverride);

			// タイトルスクロール速度
			grpText.Controls.Add(new Label
			{
				Text = "スクロール間隔",
				Location = new Point(12, 192),
				AutoSize = true,
			});
			_trackTitleScroll = new TrackBar
			{
				Location = new Point(controlX, 186),
				Size = new Size(180, 30),
				Minimum = 30,
				Maximum = 500,
				TickFrequency = 50,
				SmallChange = 10,
				LargeChange = 50,
			};
			_trackTitleScroll.ValueChanged += (_, _) =>
				_lblScrollSpeed.Text = $"{_trackTitleScroll.Value} ms";
			grpText.Controls.Add(_trackTitleScroll);

			_lblScrollSpeed = new Label
			{
				Location = new Point(320, 192),
				Size = new Size(60, 20),
				Text = "100 ms",
			};
			grpText.Controls.Add(_lblScrollSpeed);

			_chkScrollOverride = new CheckBox
			{
				Text = "設定値をスキンより優先する",
				Location = new Point(12, 216),
				AutoSize = true,
			};
			// GroupBox サイズを少し大きくしてチェックボックスが収まるようにする
			grpText.Size = new Size(440, 246);
			grpText.Controls.Add(_chkScrollOverride);

			y += grpText.Height + grpMargin;
			Controls.Add(grpText);

			// ═══════════════════════════════════════════════════
			// 保存ボタン
			// ═══════════════════════════════════════════════════
			_btnSave = new Button
			{
				Text = "適用",
				Location = new Point(16, y),
				Size = OptionsStyle.SaveButtonSize,
				BackColor = OptionsStyle.PrimaryBlue,
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
			};
			_btnSave.Click += (_, _) => SaveSettings();
			Controls.Add(_btnSave);
		}

		// ── ヘルパー ────────────────────────────────────────────────────

		private static Button MakeColorButton(Point location) => new Button
		{
			Location = location,
			Size = new Size(60, 23),
			FlatStyle = FlatStyle.Flat,
			FlatAppearance = { BorderSize = 1 },
		};

		private static ComboBox MakeFontCombo(Point location, int width)
		{
			var cmb = new ComboBox
			{
				Location = location,
				Size = new Size(width, 23),
				DropDownStyle = ComboBoxStyle.DropDown,
				AutoCompleteMode = AutoCompleteMode.SuggestAppend,
				AutoCompleteSource = AutoCompleteSource.ListItems,
			};
			foreach (var ff in FontFamily.Families)
				cmb.Items.Add(ff.Name);
			return cmb;
		}

		private static NumericUpDown MakeSizeSpinner(Point location) => new NumericUpDown
		{
			Location = location,
			Size = new Size(60, 23),
			Minimum = 6,
			Maximum = 72,
			DecimalPlaces = 1,
			Increment = 0.5m,
			Value = 9,
		};

		private void PickColor(ref Color colorField, Button button)
		{
			using var dlg = new ColorDialog { Color = colorField, FullOpen = true };
			if (dlg.ShowDialog() != DialogResult.OK)
				return;
			colorField = dlg.Color;
			button.BackColor = colorField;
		}

		private static string FormatInterval(int ms)
		{
			double fps = 1000.0 / ms;
			return $"{ms}ms / {fps:F1}fps";
		}

		private static string ColorToHex(Color c) => $"{c.R:X2}{c.G:X2}{c.B:X2}";

		// ── LoadSettings / SaveSettings ────────────────────────────────

		public override void LoadSettings()
		{
			var s = Config.settings;

			// スペクトラム基本
			_cmbSpectrumMode.SelectedIndex = s.DefaultSpectrumMode;
			_chkSnowBlock.Checked = s.SnowBlockEnabled;
			_trackSpectrumInterval.Value = Math.Clamp(s.SpectrumUpdateIntervalMs, 33, 250);
			_lblIntervalValue.Text = FormatInterval(_trackSpectrumInterval.Value);

			// ウェーブ色
			_waveColorL = ParseHex(s.WaveColorL, Color.Lime);
			_waveColorR = ParseHex(s.WaveColorR, Color.Cyan);
			_btnWaveColorL.BackColor = _waveColorL;
			_btnWaveColorR.BackColor = _waveColorR;
			_chkWaveColorOverride.Checked = s.UseCustomWaveColor;

			// バー色
			_barColor = ParseHex(s.SpectrumBarColor, Color.White);
			_btnBarColor.BackColor = _barColor;
			_chkBarColorOverride.Checked = s.UseCustomSpectrumBarColor;

			// スノー速度
			_trackSnowSpeed.Value = Math.Clamp((int)s.SnowFallSpeedPxPerSec, 2, 50);
			_lblSnowSpeed.Text = $"{_trackSnowSpeed.Value} px/秒";

			// ウェーブフォーム色
			_wfColorL       = ParseHex(s.WaveformColorL,       Color.FromArgb(0, 200, 100));
			_wfColorR       = ParseHex(s.WaveformColorR,       Color.FromArgb(0, 100, 200));
			_wfColorMix     = ParseHex(s.WaveformColorMix,     Color.FromArgb(0, 180, 120));
			_wfColorPlayed  = ParseHex(s.WaveformColorPlayed,  Color.FromArgb(100, 100, 100));
			_wfColorUnplayed= ParseHex(s.WaveformColorUnplayed,Color.FromArgb(50, 50, 50));
			_btnWfColorL.BackColor        = _wfColorL;
			_btnWfColorR.BackColor        = _wfColorR;
			_btnWfColorMix.BackColor      = _wfColorMix;
			_btnWfColorPlayed.BackColor   = _wfColorPlayed;
			_btnWfColorUnplayed.BackColor = _wfColorUnplayed;
			_chkWaveformColorOverride.Checked = s.UseCustomWaveformColors;

			// タイトルフォント
			SelectFontInCombo(_cmbTitleFont, s.TitleFontName);
			_numTitleFontSize.Value = s.TitleFontSize >= 6 && s.TitleFontSize <= 72
				? (decimal)s.TitleFontSize : 9;
			_chkTitleBold.Checked = s.TitleFontBold;
			_chkTitleFontOverride.Checked = s.UseCustomTitleFont;

			// 時間表示フォント
			SelectFontInCombo(_cmbTimeFont, s.TimeFontName);
			_numTimeFontSize.Value = s.TimeFontSize >= 6 && s.TimeFontSize <= 72
				? (decimal)s.TimeFontSize : 9;
			_chkTimeBold.Checked = s.TimeFontBold;
			_chkTimeFontOverride.Checked = s.UseCustomTimeFont;

			// スクロール速度
			_trackTitleScroll.Value = Math.Clamp(
				s.TitleScrollIntervalMs > 0 ? s.TitleScrollIntervalMs : 100, 30, 500);
			_lblScrollSpeed.Text = $"{_trackTitleScroll.Value} ms";
			_chkScrollOverride.Checked = s.UseCustomTitleScrollInterval;
		}

		public override void SaveSettings()
		{
			var s = Config.settings;

			// スペクトラム基本
			s.DefaultSpectrumMode = _cmbSpectrumMode.SelectedIndex;
			s.SnowBlockEnabled = _chkSnowBlock.Checked;
			s.SpectrumUpdateIntervalMs = _trackSpectrumInterval.Value;

			// ウェーブ色
			s.WaveColorL = ColorToHex(_waveColorL);
			s.WaveColorR = ColorToHex(_waveColorR);
			s.UseCustomWaveColor = _chkWaveColorOverride.Checked;

			// バー色
			s.SpectrumBarColor = ColorToHex(_barColor);
			s.UseCustomSpectrumBarColor = _chkBarColorOverride.Checked;

			// スノー速度
			s.SnowFallSpeedPxPerSec = _trackSnowSpeed.Value;

			// ウェーブフォーム色
			s.WaveformColorL        = ColorToHex(_wfColorL);
			s.WaveformColorR        = ColorToHex(_wfColorR);
			s.WaveformColorMix      = ColorToHex(_wfColorMix);
			s.WaveformColorPlayed   = ColorToHex(_wfColorPlayed);
			s.WaveformColorUnplayed = ColorToHex(_wfColorUnplayed);
			s.UseCustomWaveformColors = _chkWaveformColorOverride.Checked;

			// タイトルフォント
			s.TitleFontName = _cmbTitleFont.Text;
			s.TitleFontSize = (float)_numTitleFontSize.Value;
			s.TitleFontBold = _chkTitleBold.Checked;
			s.UseCustomTitleFont = _chkTitleFontOverride.Checked;

			// 時間表示フォント
			s.TimeFontName = _cmbTimeFont.Text;
			s.TimeFontSize = (float)_numTimeFontSize.Value;
			s.TimeFontBold = _chkTimeBold.Checked;
			s.UseCustomTimeFont = _chkTimeFontOverride.Checked;

			// スクロール速度
			s.TitleScrollIntervalMs = _trackTitleScroll.Value;
			s.UseCustomTitleScrollInterval = _chkScrollOverride.Checked;

			// 既存の即時反映
			_mainForm.Spectrum.Mode = s.DefaultSpectrumMode;
			_mainForm.Spectrum.SnowBlockEnabled = s.SnowBlockEnabled;

			Config.Save();

			// 新規設定の即時反映
			_mainForm.ApplySpectrumVisualSettings();
		}

		private static Color ParseHex(string hex, Color fallback)
		{
			if (string.IsNullOrWhiteSpace(hex))
				return fallback;
			try
			{
				return ColorTranslator.FromHtml('#' + hex.TrimStart('#'));
			}
			catch
			{
				return fallback;
			}
		}

		private static void SelectFontInCombo(ComboBox cmb, string fontName)
		{
			if (string.IsNullOrEmpty(fontName))
				return;
			var idx = cmb.Items.IndexOf(fontName);
			if (idx >= 0)
				cmb.SelectedIndex = idx;
		}
	}
}
