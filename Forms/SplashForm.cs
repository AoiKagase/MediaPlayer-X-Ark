using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms
{
	public class SplashForm : Form
	{
		private const string AppLogoRelativePath = @"Resources\Icons\x-ark-icon.png";
		private const string AppWordmarkRelativePath = @"Resources\Brand\media-player-x-ark-logo.png";
		private const string FmodLogoRelativePath = @"Resources\Attribution\FMOD_Logo_Black_Transparent.png";
		private DateTime _shownAt;
		private PictureBox _picAppLogo;
		private PictureBox _picAppWordmark;
		private PictureBox _picFmodLogo;
		private Label _lblFmodCredit;

		public SplashForm()
		{
			BuildLayout();
		}

		private void BuildLayout()
		{
			SuspendLayout();

			AutoScaleMode = AutoScaleMode.Dpi;
			BackColor = Color.FromArgb(246, 249, 252);
			ClientSize = new Size(560, 320);
			FormBorderStyle = FormBorderStyle.None;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterScreen;
			TopMost = true;

			_picAppLogo = new PictureBox
			{
				Location = new Point(48, 54),
				Size = new Size(112, 112),
				SizeMode = PictureBoxSizeMode.Zoom,
				BackColor = Color.Transparent,
			};

			_picAppWordmark = new PictureBox
			{
				Location = new Point(184, 76),
				Size = new Size(330, 50),
				SizeMode = PictureBoxSizeMode.Zoom,
				BackColor = Color.Transparent,
			};

			var lblLoading = new Label
			{
				Location = new Point(187, 132),
				Size = new Size(300, 24),
				Font = new Font("Yu Gothic UI", 9.5f),
				ForeColor = Color.FromArgb(0, 112, 176),
				BackColor = Color.Transparent,
				Text = "Initializing audio engine",
			};

			_picFmodLogo = new PictureBox
			{
				Location = new Point(48, 230),
				Size = new Size(174, 48),
				SizeMode = PictureBoxSizeMode.Zoom,
				BackColor = Color.Transparent,
			};

			_lblFmodCredit = new Label
			{
				Location = new Point(242, 243),
				Size = new Size(270, 34),
				Font = new Font("Yu Gothic UI", 8.5f),
				ForeColor = Color.FromArgb(67, 74, 88),
				Text = "Audio Engine: FMOD Studio by Firelight Technologies Pty Ltd.",
			};

			Controls.AddRange(new Control[]
			{
				_picAppLogo,
				_picAppWordmark,
				lblLoading,
				_picFmodLogo,
				_lblFmodCredit,
			});

			LoadImage(_picAppLogo, AppLogoRelativePath);
			LoadImage(_picAppWordmark, AppWordmarkRelativePath);
			LoadImage(_picFmodLogo, FmodLogoRelativePath);
			ResumeLayout(false);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			using var background = new LinearGradientBrush(ClientRectangle,
				Color.FromArgb(250, 252, 255),
				Color.FromArgb(226, 238, 248),
				LinearGradientMode.ForwardDiagonal);
			e.Graphics.FillRectangle(background, ClientRectangle);

			using var glow = new SolidBrush(Color.FromArgb(82, 173, 214, 245));
			e.Graphics.FillEllipse(glow, new Rectangle(12, 18, 184, 184));

			using var logoPlate = new SolidBrush(Color.FromArgb(216, 255, 255, 255));
			using var logoPlatePath = CreateRoundRectPath(new Rectangle(38, 44, 132, 132), 16);
			e.Graphics.FillPath(logoPlate, logoPlatePath);

			using var accent = new LinearGradientBrush(new Rectangle(184, 154, 290, 3),
				Color.FromArgb(0, 155, 220),
				Color.FromArgb(89, 105, 230),
				LinearGradientMode.Horizontal);
			e.Graphics.FillRectangle(accent, new Rectangle(184, 154, 290, 3));

			using var panelShadow = new SolidBrush(Color.FromArgb(24, 88, 110, 132));
			using var panelShadowPath = CreateRoundRectPath(new Rectangle(34, 219, 496, 78), 8);
			e.Graphics.FillPath(panelShadow, panelShadowPath);

			using var panelBrush = new SolidBrush(Color.FromArgb(248, 255, 255, 255));
			using var panelPath = CreateRoundRectPath(new Rectangle(32, 216, 496, 78), 8);
			e.Graphics.FillPath(panelBrush, panelPath);

			using var borderPen = new Pen(Color.FromArgb(100, 168, 190, 214), 1);
			e.Graphics.DrawRectangle(borderPen, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
		}

		private static GraphicsPath CreateRoundRectPath(Rectangle bounds, int radius)
		{
			int diameter = radius * 2;
			var path = new GraphicsPath();
			path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
			path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
			path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
			path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
			path.CloseFigure();
			return path;
		}

		private static void LoadImage(PictureBox pictureBox, string relativePath)
		{
			var assembly = typeof(SplashForm).Assembly;
			using var stream = OpenEmbeddedResource(assembly, relativePath);
			if (stream == null)
				return;

			using var image = Image.FromStream(stream);
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
