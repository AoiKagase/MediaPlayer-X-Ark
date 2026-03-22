using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
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
		private LinkLabel _lnkGitHub;

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

			_lnkGitHub = new LinkLabel
			{
				Location = new Point(pad, pad + (lineHeight + 12) * 4),
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

			Controls.AddRange(new Control[]
			{
				_lblAppName, _lblVersion,
				_lblCopyright, _lblCompany, _lnkGitHub,
			});
		}

		public override void LoadSettings()
		{
			var asm = Assembly.GetExecutingAssembly();
			var info = FileVersionInfo.GetVersionInfo(asm.Location);

			_lblAppName.Text = info.ProductName ?? "MediaPlayer X-Ark";
			_lblVersion.Text = "Version " + (info.ProductVersion ?? "1.0.0.0");
			_lblCopyright.Text = info.LegalCopyright ?? "";
			_lblCompany.Text = info.CompanyName ?? "";
			_lnkGitHub.Text = "https://github.com/AoiKagase/MediaPlayer-X-Ark";
		}

		public override void SaveSettings() { }
	}
}