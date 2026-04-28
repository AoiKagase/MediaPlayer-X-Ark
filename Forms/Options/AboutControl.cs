using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using MediaPlayer_X_Ark.Engine.Update;
using MediaPlayer_X_Ark.Forms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
	public class AboutControl : OptionsControlBase
	{
		private Label _lblAppName;
		private Label _lblVersion;
		private Label _lblCopyright;
		private Label _lblCompany;
		private Label _lblFmodCredit;
		private LinkLabel _lnkGitHub;
		private Button _btnCheckUpdate;
		private Label _lblUpdateStatus;

		public AboutControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
			BuildLayout();
		}

		private void BuildLayout()
		{
			const int pad = 24;
			const int lineHeight = 28;

			_lblAppName = new Label
			{
				Location = new Point(pad, pad),
				Size = new Size(400, 32),
				Font = new Font("Yu Gothic UI", 16f, FontStyle.Bold),
			};

			_lblVersion = new Label
			{
				Location = new Point(pad, pad + lineHeight + 12),
				AutoSize = true,
				Font = new Font("Yu Gothic UI", 9f),
			};

			_lblCopyright = new Label
			{
				Location = new Point(pad, pad + (lineHeight + 12) * 2),
				AutoSize = true,
				Font = new Font("Yu Gothic UI", 9f),
			};

			_lblCompany = new Label
			{
				Location = new Point(pad, pad + (lineHeight + 12) * 3),
				AutoSize = true,
				Font = new Font("Yu Gothic UI", 9f),
			};

			_lblFmodCredit = new Label
			{
				Location = new Point(pad, pad + (lineHeight + 12) * 4),
				AutoSize = true,
				Font = new Font("Yu Gothic UI", 9f),
				Text = "Audio Engine: FMOD by Firelight Technologies Pty Ltd.",
			};

			_lnkGitHub = new LinkLabel
			{
				Location = new Point(pad, pad + (lineHeight + 12) * 5),
				AutoSize = true,
				Font = new Font("Yu Gothic UI", 9f),
			};
			_lnkGitHub.LinkClicked += (s, e) =>
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = _lnkGitHub.Text,
					UseShellExecute = true,
				});
			};

			_btnCheckUpdate = new Button
			{
				Location = new Point(pad, pad + (lineHeight + 12) * 6),
				Size = new Size(160, 28),
				Text = "アップデートを確認",
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(0, 120, 215),
				ForeColor = Color.White,
				Font = new Font("Yu Gothic UI", 9f),
			};
			_btnCheckUpdate.Click += BtnCheckUpdate_Click;

			_lblUpdateStatus = new Label
			{
				Location = new Point(pad, pad + (lineHeight + 12) * 6 + 36),
				AutoSize = true,
				Font = new Font("Yu Gothic UI", 9f),
				ForeColor = Color.Gray,
			};

			Controls.AddRange(new Control[]
			{
				_lblAppName, _lblVersion,
				_lblCopyright, _lblCompany, _lblFmodCredit, _lnkGitHub,
				_btnCheckUpdate, _lblUpdateStatus,
			});
		}

		public override void LoadSettings()
		{
			var location = Assembly.GetExecutingAssembly().Location;
			if (string.IsNullOrEmpty(location))
				location = Environment.ProcessPath ?? string.Empty;
			var info = FileVersionInfo.GetVersionInfo(location);

			_lblAppName.Text = info.ProductName ?? "MediaPlayer X-Ark";
			_lblVersion.Text = "Version " + AppVersion.Current;
			_lblCopyright.Text = info.LegalCopyright ?? "";
			_lblCompany.Text = info.CompanyName ?? "";
			_lnkGitHub.Text = "https://github.com/AoiKagase/MediaPlayer-X-Ark";
		}

		public override void SaveSettings() { }

		private async void BtnCheckUpdate_Click(object sender, EventArgs e)
		{
			_btnCheckUpdate.Enabled = false;
			_lblUpdateStatus.Text = "チェック中...";
			_lblUpdateStatus.ForeColor = Color.Gray;

			var repo = Config.settings.UpdateGitHubRepo;
			if (string.IsNullOrWhiteSpace(repo))
			{
				_lblUpdateStatus.Text = "GitHubリポジトリが設定されていません。";
				_btnCheckUpdate.Enabled = true;
				return;
			}

			var info = await UpdateChecker.CheckAsync(repo);
			_btnCheckUpdate.Enabled = true;

			if (info == null)
			{
				_lblUpdateStatus.Text = "最新バージョンを使用中です。";
				_lblUpdateStatus.ForeColor = Color.Gray;
				return;
			}

			_lblUpdateStatus.Text = $"バージョン {info.Version} が利用可能です！";
			_lblUpdateStatus.ForeColor = Color.FromArgb(0, 120, 215);
			new UpdateAvailableDialog(info).ShowDialog(this.FindForm());
		}
	}
}
