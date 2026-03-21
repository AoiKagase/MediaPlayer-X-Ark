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
		private CheckBox _chkReplayGain;
		private RadioButton _rdoTrack;
		private RadioButton _rdoAlbum;
		private Label _lblPreamp;
		private TrackBar _trkPreamp;
		private Label _lblPreampVal;
		private Label _lblRgSection;
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
			_chkReplayGain.Checked = Config.settings.ReplayGainEnabled;
			_rdoAlbum.Checked = Config.settings.ReplayGainMode == 1;
			_rdoTrack.Checked = Config.settings.ReplayGainMode == 0;
			_trkPreamp.Value = (int)Math.Max(-12, Math.Min(12, Config.settings.ReplayGainPreamp));
			_lblPreampVal.Text = $"{_trkPreamp.Value:+0;-0;0} dB";
			UpdateReplayGainEnabled();
			UpdateControlsEnabled();
		}

		public override void SaveSettings()
		{
			Config.settings.CrossfadeEnabled = _chkEnabled.Checked;
			Config.settings.CrossfadeDurationMs = _trkDuration.Value * 1000;

			// エンジンに即時反映
			Engine.CrossfadeEnabled = _chkEnabled.Checked;
			Engine.CrossfadeDurationMs = _trkDuration.Value * 1000;
			Config.settings.ReplayGainEnabled = _chkReplayGain.Checked;
			Config.settings.ReplayGainMode = _rdoAlbum.Checked ? 1 : 0;
			Config.settings.ReplayGainPreamp = _trkPreamp.Value;
			Engine.ReplayGainEnabled = _chkReplayGain.Checked;
			Engine.ReplayGainMode = _rdoAlbum.Checked ? 1 : 0;
			Engine.ReplayGainPreamp = _trkPreamp.Value;
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

			// ReplayGain セクション区切り
			_lblRgSection = new Label
			{
				Text = "音量の正規化（ReplayGain）",
				Location = new Point(0, 128),
				AutoSize = true,
				Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold),
			};

			var pnlRgLine = new Panel
			{
				Location = new Point(0, 148),
				Size = new Size(480, 1),
				BackColor = Color.Gray,
			};

			_chkReplayGain = new CheckBox
			{
				Text = "ReplayGainを有効にする",
				Location = new Point(0, 156),
				AutoSize = true,
			};
			_chkReplayGain.CheckedChanged += (s, e) => UpdateReplayGainEnabled();

			_rdoTrack = new RadioButton
			{
				Text = "トラック",
				Location = new Point(16, 184),
				AutoSize = true,
				Checked = true,
			};

			_rdoAlbum = new RadioButton
			{
				Text = "アルバム",
				Location = new Point(100, 184),
				AutoSize = true,
			};

			_lblPreamp = new Label
			{
				Text = "プリアンプ：",
				Location = new Point(0, 216),
				AutoSize = true,
			};

			_trkPreamp = new TrackBar
			{
				Location = new Point(90, 210),
				Size = new Size(240, 40),
				Minimum = -12,
				Maximum = 12,
				TickFrequency = 2,
				SmallChange = 1,
				Value = 0,
			};
			_trkPreamp.ValueChanged += (s, e) =>
				_lblPreampVal.Text = $"{_trkPreamp.Value:+0;-0;0} dB";

			_lblPreampVal = new Label
			{
				Text = "0 dB",
				Location = new Point(338, 216),
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
				Location = new Point(0, 268),
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
				 _lblRgSection, pnlRgLine,
				_chkReplayGain, _rdoTrack, _rdoAlbum,
				_lblPreamp, _trkPreamp, _lblPreampVal,
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
		private void UpdateReplayGainEnabled()
		{
			bool on = _chkReplayGain.Checked;
			_rdoTrack.Enabled = on;
			_rdoAlbum.Enabled = on;
			_lblPreamp.Enabled = on;
			_trkPreamp.Enabled = on;
			_lblPreampVal.Enabled = on;
		}
	}
}