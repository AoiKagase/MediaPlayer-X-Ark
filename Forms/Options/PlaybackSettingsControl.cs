using MediaPlayer_X_Ark.Engine;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
	public class PlaybackSettingsControl : OptionsControlBase
	{
		private CheckBox _chkEnabled;
		private Label _lblDuration;
		private TrackBar _trkDuration;
		private Label _lblDurationVal;
		private Button _btnSave;

		public PlaybackSettingsControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
			BuildLayout();
		}

		public override void LoadSettings()
		{
			_chkEnabled.Checked = Config.settings.CrossfadeEnabled;
			_trkDuration.Value = Math.Max(1, Math.Min(15,
				Config.settings.CrossfadeDurationMs / 1000));
			UpdateDurationLabel();
			UpdateControlsEnabled();
		}

		public override void SaveSettings()
		{
			Config.settings.CrossfadeEnabled = _chkEnabled.Checked;
			Config.settings.CrossfadeDurationMs = _trkDuration.Value * 1000;

			// エンジンに即時反映
			Engine.CrossfadeEnabled = _chkEnabled.Checked;
			Engine.CrossfadeDurationMs = _trkDuration.Value * 1000;

			Config.Save();
		}

		private void BuildLayout()
		{
			const int lineH = 28;

			// ── セクション：クロスフェード ────────────────────────
			var lblSection = new Label
			{
				Text = "クロスフェード",
				Location = new Point(0, 0),
				AutoSize = true,
				Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold),
			};

			var pnlLine = new Panel
			{
				Location = new Point(0, 20),
				Size = new Size(480, 1),
				BackColor = Color.Gray,
			};

			_chkEnabled = new CheckBox
			{
				Text = "クロスフェードを有効にする",
				Location = new Point(0, 28),
				AutoSize = true,
			};
			_chkEnabled.CheckedChanged += (s, e) => UpdateControlsEnabled();

			_lblDuration = new Label
			{
				Text = "フェード時間：",
				Location = new Point(0, 64),
				AutoSize = true,
			};

			_trkDuration = new TrackBar
			{
				Location = new Point(90, 58),
				Size = new Size(240, 40),
				Minimum = 1,
				Maximum = 15,
				TickFrequency = 1,
				SmallChange = 1,
				LargeChange = 2,
				Value = 3,
			};
			_trkDuration.ValueChanged += (s, e) => UpdateDurationLabel();

			_lblDurationVal = new Label
			{
				Text = "3 秒",
				Location = new Point(338, 64),
				AutoSize = true,
			};

			// ── 将来の再生設定はここに追加 ────────────────────────
			// 例：
			//   ReplayGain（音量の正規化）
			//   ギャップレス再生
			//   フェードイン（曲開始時）

			_btnSave = new Button
			{
				Text = "保存",
				Location = new Point(0, 112),
				Size = new Size(80, lineH),
				BackColor = Color.FromArgb(0, 120, 215),
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
			};
			_btnSave.Click += (s, e) => SaveSettings();

			Controls.AddRange(new Control[]
			{
				lblSection, pnlLine,
				_chkEnabled, _lblDuration, _trkDuration, _lblDurationVal,
				_btnSave,
			});
		}

		private void UpdateDurationLabel()
			=> _lblDurationVal.Text = $"{_trkDuration.Value} 秒";

		private void UpdateControlsEnabled()
		{
			_trkDuration.Enabled = _chkEnabled.Checked;
			_lblDuration.Enabled = _chkEnabled.Checked;
			_lblDurationVal.Enabled = _chkEnabled.Checked;
		}
	}
}