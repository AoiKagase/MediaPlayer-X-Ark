using System;
using System.Drawing;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms
{
	public class ErrorToastForm : Form
	{
		private readonly System.Windows.Forms.Timer _fadeTimer;
		private double _displaySeconds = 0;
		private bool _fading = false;
		private const double FadeStep = 0.06;
		private const int WS_EX_NOACTIVATE = 0x08000000;
		private const int WS_EX_TOOLWINDOW = 0x00000080;
		private const int WS_EX_TRANSPARENT = 0x00000020;
		private const int WM_NCHITTEST = 0x0084;
		private static readonly IntPtr HTTRANSPARENT = new IntPtr(-1);

		protected override CreateParams CreateParams
		{
			get
			{
				var cp = base.CreateParams;
				cp.ExStyle |= WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW | WS_EX_TRANSPARENT;
				return cp;
			}
		}

		public ErrorToastForm(string message)
		{
			SetStyle(
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.UserPaint |
				ControlStyles.DoubleBuffer,
				true);

			FormBorderStyle = FormBorderStyle.None;
			MediaPlayer_X_Ark.ApplicationIcon.ApplyTo(this);
			ShowInTaskbar = false;
			TopMost = true;
			BackColor = Color.FromArgb(40, 40, 40);
			Opacity = 0.9;
			Size = new Size(300, 64);
			StartPosition = FormStartPosition.Manual;

			var icon = new HitTestTransparentLabel
			{
				Text = "⚠",
				ForeColor = Color.Orange,
				BackColor = Color.FromArgb(40, 40, 40),
				Font = new Font("Segoe UI", 18f, FontStyle.Regular),
				Location = new Point(8, 12),
				Size = new Size(36, 36),
				TextAlign = ContentAlignment.MiddleCenter,
			};

			var messageLabel = new HitTestTransparentLabel
			{
				Text = message,
				ForeColor = Color.White,
				BackColor = Color.FromArgb(40, 40, 40),
				Font = new Font("Segoe UI", 9f),
				Location = new Point(50, 8),
				Size = new Size(244, 48),
				TextAlign = ContentAlignment.MiddleLeft,
			};

			Controls.Add(icon);
			Controls.Add(messageLabel);

			_fadeTimer = new System.Windows.Forms.Timer { Interval = 80 };
			_fadeTimer.Tick += FadeTimer_Tick;
		}

		public void ShowToast(Form owner)
		{
			UpdatePosition(owner);
			_fadeTimer.Start();
			Show(owner);
		}

		public void UpdatePosition(Form owner)
		{
			if (IsDisposed)
				return;
			Left = owner.Left + owner.Width - Width - 8;
			Top = owner.Top + owner.Height - Height - 8;
		}

		private void FadeTimer_Tick(object sender, EventArgs e)
		{
			_displaySeconds += 0.08;

			if (!_fading && _displaySeconds >= 3.0)
				_fading = true;

			if (_fading)
			{
				Opacity -= FadeStep;
				if (Opacity <= 0)
				{
					_fadeTimer.Stop();
					Close();
				}
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_NCHITTEST)
			{
				m.Result = HTTRANSPARENT;
				return;
			}

			base.WndProc(ref m);
		}

		private sealed class HitTestTransparentLabel : Label
		{
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == WM_NCHITTEST)
				{
					m.Result = HTTRANSPARENT;
					return;
				}

				base.WndProc(ref m);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				_fadeTimer?.Dispose();
			base.Dispose(disposing);
		}
	}
}
