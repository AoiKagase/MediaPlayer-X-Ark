using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
	public class DisplaySettingsControl : OptionsControlBase
	{
		private ComboBox _cmbSpectrumMode;
		private CheckBox _chkSnowBlock;
		private Button _btnSave;
		private MainForm _mainForm;

		public DisplaySettingsControl(IPlayerEngine engine, IConfigService config, MainForm mainForm)
			: base(engine, config)
		{
			_mainForm = mainForm;
			BuildLayout();
		}

		private void BuildLayout()
		{
			var y = 16;
			const int lineHeight = 28;

			// ===========================
			// スペクトラム設定
			// ===========================
			var grpSpectrum = new GroupBox
			{
				Text = "スペクトラム",
				Location = new Point(16, y),
				Size = new Size(400, 90),
			};

			var lblMode = new Label
			{
				Text = "デフォルトモード",
				Location = new Point(12, 28),
				AutoSize = true,
			};

			_cmbSpectrumMode = new ComboBox
			{
				Location = new Point(120, 24),
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

			_chkSnowBlock = new CheckBox
			{
				Text = "SnowBlock モードを有効にする",
				Location = new Point(12, 24 + lineHeight),
				AutoSize = true,
			};

			grpSpectrum.Controls.AddRange(new Control[]
			{
				lblMode, _cmbSpectrumMode, _chkSnowBlock
			});

			y += grpSpectrum.Height + 12;

			// ===========================
			// 保存ボタン
			// ===========================
			_btnSave = new Button
			{
				Text = "適用",
				Location = new Point(16, y),
				Size = new Size(75, 23),
			};
			_btnSave.Click += BtnSave_Click;

			Controls.AddRange(new Control[]
			{
				grpSpectrum, _btnSave
			});
		}

		public override void LoadSettings()
		{
			_cmbSpectrumMode.SelectedIndex = Config.settings.DefaultSpectrumMode;
			_chkSnowBlock.Checked = Config.settings.SnowBlockEnabled;
		}

		public override void SaveSettings()
		{
			Config.settings.DefaultSpectrumMode = _cmbSpectrumMode.SelectedIndex;
			Config.settings.SnowBlockEnabled = _chkSnowBlock.Checked;

			// ★即時反映
			_mainForm.Spectrum.Mode = Config.settings.DefaultSpectrumMode;
			_mainForm.Spectrum.SnowBlockEnabled = Config.settings.SnowBlockEnabled;
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			SaveSettings();
			Config.Save();
		}
	}
}