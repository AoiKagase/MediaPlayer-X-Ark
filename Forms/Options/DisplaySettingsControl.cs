using MediaPlayer_X_Ark.Engine;
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

		public DisplaySettingsControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
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
				"0: スペクトラム（バー）",
				"1: スペクトラム（ライン）",
				"2: スペクトラム（スノー）",
				"3: スペクトラム（スノーブロック）",
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
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			SaveSettings();
			Config.Save();
		}
	}
}