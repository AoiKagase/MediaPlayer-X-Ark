using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
	public class PlaybackSettingsControl : OptionsControlBase
	{
		// クロスフェード
		private CheckBox _chkEnabled;
		private Label _lblDuration;
		private TrackBar _trkDuration;
		private Label _lblDurationVal;

        // NonStopMix
        private CheckBox _chkNonStopMix;

		// ReplayGain
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
			// クロスフェード
			_chkEnabled.Checked = Config.settings.CrossfadeEnabled;
            _trkDuration.Value = Math.Max(1, Math.Min(15,
				Config.settings.CrossfadeDurationMs / 1000));
			UpdateDurationLabel();

            // NonStopMix
            _chkNonStopMix.Checked = Config.settings.NonStopMixEnabled;

			// ReplayGain
            _chkReplayGain.Checked = Config.settings.ReplayGainEnabled;
			_rdoAlbum.Checked = Config.settings.ReplayGainMode == 1;
			_rdoTrack.Checked = Config.settings.ReplayGainMode == 0;
			_trkPreamp.Value = (int)Math.Max(-12, Math.Min(12, Config.settings.ReplayGainPreamp));
			_lblPreampVal.Text = $"{_trkPreamp.Value:+0;-0;0} dB";

            UpdateControlsEnabled();
            UpdateNonStopMixEnabled();
            UpdateReplayGainEnabled();
		}

		public override void SaveSettings()
		{
            // クロスフェード
            Config.settings.CrossfadeEnabled = _chkEnabled.Checked;
			Config.settings.CrossfadeDurationMs = _trkDuration.Value * 1000;
            Engine.CrossfadeEnabled = _chkEnabled.Checked;
            Engine.CrossfadeDurationMs = _trkDuration.Value * 1000;

            // NonStopMix
            Config.settings.NonStopMixEnabled = _chkNonStopMix.Checked;
			Engine.NonStopMixEnabled = _chkNonStopMix.Checked;

            // ReplayGain
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

            // ── クロスフェード ────────────────────────────────────
            var lblCfSection = new Label
            {
                Text = "クロスフェード",
                Location = new Point(0, 0),
                AutoSize = true,
                Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold),
            };
            var pnlCfLine = new Panel
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
            _chkEnabled.CheckedChanged += (s, e) =>
            {
                if (_chkEnabled.Checked) _chkNonStopMix.Checked = false;
                UpdateControlsEnabled();
            };
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

			// ── NonStopMix ────────────────────────────────────────
			var lblNsmSection = new Label
			{
				Text = "NonStopMix",
				Location = new Point(0, 112),
				AutoSize = true,
				Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold),
			};
			var pnlNsmLine = new Panel
			{
				Location = new Point(0, 132),
				Size = new Size(480, 1),
				BackColor = Color.Gray,
			};
			_chkNonStopMix = new CheckBox
			{
				Text = "NonStopMix を有効にする（クロスフェードと排他）",
				Location = new Point(0, 140),
				AutoSize = true,
			};
			_chkNonStopMix.CheckedChanged += (s, e) =>
			{
				if (_chkNonStopMix.Checked) _chkEnabled.Checked = false;
				UpdateNonStopMixEnabled();
			};

			// ── ReplayGain ────────────────────────────────────────
			var lblRgSection = new Label
            {
                Text = "音量の正規化（ReplayGain）",
                Location = new Point(0, 176),
                AutoSize = true,
                Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold),
            };
            var pnlRgLine = new Panel
            {
                Location = new Point(0, 196),
                Size = new Size(480, 1),
                BackColor = Color.Gray,
            };
            _chkReplayGain = new CheckBox
            {
                Text = "ReplayGainを有効にする",
                Location = new Point(0, 204),
                AutoSize = true,
            };
            _chkReplayGain.CheckedChanged += (s, e) => UpdateReplayGainEnabled();
            _rdoTrack = new RadioButton
            {
                Text = "トラック",
                Location = new Point(16, 232),
                AutoSize = true,
                Checked = true,
            };
            _rdoAlbum = new RadioButton
            {
                Text = "アルバム",
                Location = new Point(100, 232),
                AutoSize = true,
            };
            _lblPreamp = new Label
            {
                Text = "プリアンプ：",
                Location = new Point(0, 264),
                AutoSize = true,
            };
            _trkPreamp = new TrackBar
            {
                Location = new Point(90, 258),
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
                Location = new Point(338, 264),
                AutoSize = true,
            };

            // ── 保存ボタン ─────────────────────────────────────────
            _btnSave = new Button
            {
                Text = "保存",
                Location = new Point(0, 316),
                Size = new Size(80, lineH),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
            };
            _btnSave.Click += (s, e) => SaveSettings();

            Controls.AddRange(new Control[]
            {
                lblCfSection, pnlCfLine,
                _chkEnabled, _lblDuration, _trkDuration, _lblDurationVal,
                lblNsmSection, pnlNsmLine,
                _chkNonStopMix,
        		lblRgSection, pnlRgLine,
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
        private void UpdateNonStopMixEnabled()
        {
			bool on = _chkNonStopMix.Checked;
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