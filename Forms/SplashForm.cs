using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms
{
	public class SplashForm : Form
	{
		private const string AppLogoRelativePath = @"Resources\Icons\x-ark-icon.png";
		private const string FmodLogoRelativePath = @"Resources\Attribution\FMOD_Logo_Black_Transparent.png";
		private DateTime _shownAt;
		private PictureBox _picAppLogo;
		private PictureBox _picFmodLogo;
		private Label _lblAppName;
		private Label _lblFmodCredit;

		public SplashForm()
		{
			BuildLayout();
		}

		private void BuildLayout()
		{
			SuspendLayout();

			AutoScaleMode = AutoScaleMode.Dpi;
			BackColor = Color.FromArgb(245, 247, 250);
			ClientSize = new Size(520, 300);
			FormBorderStyle = FormBorderStyle.None;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterScreen;
			TopMost = true;

			_picAppLogo = new PictureBox
			{
				Location = new Point(42, 58),
				Size = new Size(96, 96),
				SizeMode = PictureBoxSizeMode.Zoom,
				BackColor = Color.Transparent,
			};

			_lblAppName = new Label
			{
				Location = new Point(160, 76),
				Size = new Size(320, 42),
				Font = new Font("Yu Gothic UI", 20f, FontStyle.Bold),
				ForeColor = Color.FromArgb(28, 34, 44),
				Text = "MediaPlayer X-Ark",
			};

			var lblLoading = new Label
			{
				Location = new Point(163, 124),
				Size = new Size(300, 24),
				Font = new Font("Yu Gothic UI", 9f),
				ForeColor = Color.FromArgb(92, 101, 116),
				Text = "Starting audio engine...",
			};

			_picFmodLogo = new PictureBox
			{
				Location = new Point(160, 184),
				Size = new Size(190, 56),
				SizeMode = PictureBoxSizeMode.Zoom,
				BackColor = Color.Transparent,
			};

			_lblFmodCredit = new Label
			{
				Location = new Point(160, 244),
				Size = new Size(330, 24),
				Font = new Font("Yu Gothic UI", 8.5f),
				ForeColor = Color.FromArgb(80, 88, 102),
				Text = "Audio Engine: FMOD Studio by Firelight Technologies Pty Ltd.",
			};

			Controls.AddRange(new Control[]
			{
				_picAppLogo,
				_lblAppName,
				lblLoading,
				_picFmodLogo,
				_lblFmodCredit,
			});

			LoadImage(_picAppLogo, AppLogoRelativePath);
			LoadImage(_picFmodLogo, FmodLogoRelativePath);
			ResumeLayout(false);
		}

		private static void LoadImage(PictureBox pictureBox, string relativePath)
		{
			string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
			if (!File.Exists(path))
				path = Path.Combine(AppContext.BaseDirectory, relativePath);
			if (!File.Exists(path))
				return;

			using var stream = File.OpenRead(path);
			using var image = Image.FromStream(stream);
			pictureBox.Image = new Bitmap(image);
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			_shownAt = DateTime.UtcNow;
		}

		public void CloseAfterMinimumDisplay(int milliseconds)
		{
			if (InvokeRequired)
			{
				BeginInvoke((Action)(() => CloseAfterMinimumDisplay(milliseconds)));
				return;
			}

			if (IsDisposed)
				return;

			var elapsed = (int)(DateTime.UtcNow - _shownAt).TotalMilliseconds;
			int delay = Math.Max(0, milliseconds - elapsed);
			if (delay == 0)
			{
				Close();
				return;
			}

			var timer = new Timer { Interval = delay };
			timer.Tick += (s, e) =>
			{
				timer.Stop();
				timer.Dispose();
				if (!IsDisposed)
					Close();
			};
			timer.Start();
		}
	}
}
