using MediaPlayer_X_Ark.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
	public class GeneralSettingsControl : OptionsControlBase
	{
		private CheckBox _chkRestorePlaylist;
		private CheckBox _chkRestorePosition;
		private CheckBox _chkAlwaysOnTop;
		private CheckBox _chkAutoSavePlaylist;

		private GroupBox _grpOpenFileAction;
		private RadioButton _rdoOpenFileAuto;    // 再生中なら追加・停止中なら再生
		private RadioButton _rdoOpenFilePlay;    // 常に再生
		private RadioButton _rdoOpenFileAdd;     // 常に追加のみ

		private Button _btnSave;

		public GeneralSettingsControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
			BuildLayout();
		}

		private void BuildLayout()
		{
			var y = 16;
			const int lineHeight = 28;

			// ===========================
			// 起動・プレイリスト
			// ===========================
			var grpStartup = new GroupBox
			{
				Text = "起動・プレイリスト",
				Location = new Point(16, y),
				Size = new Size(400, 130),
			};

			_chkRestorePlaylist = new CheckBox
			{
				Text = "前回のプレイリストを復元する",
				Location = new Point(12, 24),
				AutoSize = true,
			};
			_chkRestorePosition = new CheckBox
			{
				Text = "前回の再生位置から再開する",
				Location = new Point(12, 24 + lineHeight),
				AutoSize = true,
			};
			_chkAutoSavePlaylist = new CheckBox
			{
				Text = "プレイリストを自動保存する",
				Location = new Point(12, 24 + lineHeight * 2),
				AutoSize = true,
			};
			_chkAlwaysOnTop = new CheckBox
			{
				Text = "常に最前面に表示する",
				Location = new Point(12, 24 + lineHeight * 3),
				AutoSize = true,
			};

			grpStartup.Controls.AddRange(new Control[]
			{
				_chkRestorePlaylist, _chkRestorePosition,
				_chkAutoSavePlaylist, _chkAlwaysOnTop
			});

			y += grpStartup.Height + 12;

			// ===========================
			// ファイルを開いた時の動作
			// ===========================
			_grpOpenFileAction = new GroupBox
			{
				Text = "ファイルを開いた時の動作",
				Location = new Point(16, y),
				Size = new Size(400, 100),
			};

			_rdoOpenFileAuto = new RadioButton
			{
				Text = "再生中なら追加・停止中なら再生",
				Location = new Point(12, 24),
				AutoSize = true,
			};
			_rdoOpenFilePlay = new RadioButton
			{
				Text = "常に再生を開始する",
				Location = new Point(12, 24 + lineHeight),
				AutoSize = true,
			};
			_rdoOpenFileAdd = new RadioButton
			{
				Text = "常にプレイリストに追加のみ",
				Location = new Point(12, 24 + lineHeight * 2),
				AutoSize = true,
			};

			_grpOpenFileAction.Controls.AddRange(new Control[]
			{
				_rdoOpenFileAuto, _rdoOpenFilePlay, _rdoOpenFileAdd
			});

			y += _grpOpenFileAction.Height + 12;

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
				grpStartup, _grpOpenFileAction, _btnSave
			});
		}

		public override void LoadSettings()
		{
			_chkRestorePlaylist.Checked = Config.settings.RestorePlaylist;
			_chkRestorePosition.Checked = Config.settings.RestorePosition;
			_chkAutoSavePlaylist.Checked = Config.settings.AutoSavePlaylist;
			_chkAlwaysOnTop.Checked = Config.settings.AlwaysOnTop;

			switch (Config.settings.OpenFileAction)
			{
				case 1: _rdoOpenFilePlay.Checked = true; break;
				case 2: _rdoOpenFileAdd.Checked = true; break;
				default: _rdoOpenFileAuto.Checked = true; break;
			}
		}

		public override void SaveSettings()
		{
			Config.settings.RestorePlaylist = _chkRestorePlaylist.Checked;
			Config.settings.RestorePosition = _chkRestorePosition.Checked;
			Config.settings.AutoSavePlaylist = _chkAutoSavePlaylist.Checked;
			Config.settings.AlwaysOnTop = _chkAlwaysOnTop.Checked;

			if (_rdoOpenFilePlay.Checked) Config.settings.OpenFileAction = 1;
			else if (_rdoOpenFileAdd.Checked) Config.settings.OpenFileAction = 2;
			else Config.settings.OpenFileAction = 0;
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			SaveSettings();
			Config.Save();
		}
	}
}
