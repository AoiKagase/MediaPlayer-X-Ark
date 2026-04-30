using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using MediaPlayer_X_Ark.Engine.Update;
using MediaPlayer_X_Ark.Forms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
		private PictureBox _picAppLogo;
		private PictureBox _picAppWordmark;
		private PictureBox _picFmodLogo;
		private Label _lblFmodCredit;
		private Label _lblThirdPartyCredit;
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

			_picAppLogo = new PictureBox
			{
				Location = new Point(pad, pad),
				Size = new Size(64, 64),
				SizeMode = PictureBoxSizeMode.Zoom,
				BackColor = Color.Transparent,
			};

			_picAppWordmark = new PictureBox
			{
				Location = new Point(pad + 82, pad + 4),
				Size = new Size(360, 38),
				SizeMode = PictureBoxSizeMode.Zoom,
				BackColor = Color.Transparent,
			};

			_lblAppName = new Label
			{
				Location = new Point(pad + 82, pad + 4),
				Size = new Size(400, 32),
				Font = new Font("Yu Gothic UI", 16f, FontStyle.Bold),
				Visible = false,
			};

			_lblVersion = new Label
			{
				Location = new Point(pad + 84, pad + lineHeight + 12),
				AutoSize = true,
				Font = new Font("Yu Gothic UI", 9f),
			};

			_lblCopyright = new Label
			{
				Location = new Point(pad, pad + (lineHeight + 12) * 2 + 24),
				AutoSize = true,
				Font = new Font("Yu Gothic UI", 9f),
			};

			_lblCompany = new Label
			{
				Location = new Point(pad, pad + (lineHeight + 12) * 3 + 24),
				AutoSize = true,
				Font = new Font("Yu Gothic UI", 9f),
			};

			_picFmodLogo = new PictureBox
			{
				Location = new Point(pad, pad + (lineHeight + 12) * 4 + 24),
				Size = new Size(220, 64),
				SizeMode = PictureBoxSizeMode.Zoom,
				BackColor = Color.Transparent,
			};

			_lblFmodCredit = new Label
			{
				Location = new Point(pad, pad + (lineHeight + 12) * 4 + 96),
				AutoSize = true,
				Font = new Font("Yu Gothic UI", 9f),
				Text = "Audio Engine: FMOD Studio by Firelight Technologies Pty Ltd.",
			};

			_lblThirdPartyCredit = new Label
			{
				Location = new Point(pad, pad + (lineHeight + 12) * 6 + 48),
				AutoSize = false,
				Size = new Size(560, 118),
				Font = new Font("Yu Gothic UI", 9f),
				Text = "Third-party components and notices:\r\n" +
					"Runtime libraries: FMOD Studio, nfluidsynth, Vortice.Windows, ATL, DiscordRPC, Newtonsoft.Json.\r\n" +
					"Bundled components: AlacEncoder, FlacEncoder, SRLAEncoder, XArkMidiEngine, codec_srla, codec_wma.\r\n" +
					"License summary: MIT, MPL, FMOD EULA, and licenses of transitive dependencies apply.\r\n" +
					"See THIRD_PARTY_NOTICES.txt and THIRD_PARTY_LIBRARIES.txt for the complete inventory.",
			};

			_lnkGitHub = new LinkLabel
			{
				Location = new Point(pad, pad + (lineHeight + 12) * 10 + 32),
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
				Location = new Point(pad, pad + (lineHeight + 12) * 12 + 32),
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
				Location = new Point(pad, pad + (lineHeight + 12) * 12 + 68),
				AutoSize = true,
				Font = new Font("Yu Gothic UI", 9f),
				ForeColor = Color.Gray,
			};

			Controls.AddRange(new Control[]
			{
				_picAppLogo, _picAppWordmark, _lblAppName, _lblVersion,
				_lblCopyright, _lblCompany, _picFmodLogo, _lblFmodCredit, _lblThirdPartyCredit, _lnkGitHub,
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
			LoadImage(_picAppLogo, @"Resources\Icons\x-ark-icon.png");
			LoadImage(_picAppWordmark, @"Resources\Brand\media-player-x-ark-logo.png");
			LoadImage(_picFmodLogo, @"Resources\Attribution\FMOD_Logo_Black_Transparent.png");
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

		private static void LoadImage(PictureBox pictureBox, string relativePath)
		{
			var assembly = typeof(AboutControl).Assembly;
			using var stream = OpenEmbeddedResource(assembly, relativePath);
			if (stream == null)
				return;

			using var image = Image.FromStream(stream);
			pictureBox.Image?.Dispose();
			pictureBox.Image = new Bitmap(image);
		}

		private static Stream OpenEmbeddedResource(System.Reflection.Assembly assembly, string relativePath)
		{
			string resourceSuffix = relativePath.Replace('\\', '.').Replace('/', '.');
			foreach (string resourceName in assembly.GetManifestResourceNames())
			{
				if (resourceName.EndsWith(resourceSuffix, StringComparison.Ordinal))
					return assembly.GetManifestResourceStream(resourceName);
			}

			return null;
		}
	}
}
