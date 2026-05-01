using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Controls
{
	public class RoundedPictureBox : PictureBox
	{
		private int _cornerRadius = 12;
		private Color _borderColor = Color.Empty;
		private int _borderWidth;

		public RoundedPictureBox()
		{
			SetStyle(
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.ResizeRedraw |
				ControlStyles.UserPaint,
				true);
		}

		[DefaultValue(12)]
		public int CornerRadius
		{
			get => _cornerRadius;
			set
			{
				var normalized = Math.Max(0, value);
				if (_cornerRadius == normalized) return;
				_cornerRadius = normalized;
				Invalidate();
			}
		}

		[DefaultValue(typeof(Color), "Empty")]
		public Color BorderColor
		{
			get => _borderColor;
			set
			{
				if (_borderColor == value) return;
				_borderColor = value;
				Invalidate();
			}
		}

		[DefaultValue(0)]
		public int BorderWidth
		{
			get => _borderWidth;
			set
			{
				var normalized = Math.Max(0, value);
				if (_borderWidth == normalized) return;
				_borderWidth = normalized;
				Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			var g = pe.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			using var path = CreateRoundPath(ClientRectangle, _cornerRadius);
			using (var backBrush = new SolidBrush(BackColor))
				g.FillPath(backBrush, path);

			if (Image != null)
			{
				var imageRect = GetImageRectangle(Image, ClientRectangle, SizeMode);
				var state = g.Save();
				g.SetClip(path);
				g.DrawImage(Image, imageRect);
				g.Restore(state);
			}

			if (_borderWidth > 0 && _borderColor != Color.Empty)
			{
				var inset = _borderWidth / 2f;
				var borderRect = new RectangleF(
					ClientRectangle.X + inset,
					ClientRectangle.Y + inset,
					Math.Max(0, ClientRectangle.Width - _borderWidth),
					Math.Max(0, ClientRectangle.Height - _borderWidth));

				using var borderPath = CreateRoundPath(borderRect, Math.Max(0, _cornerRadius - inset));
				using var pen = new Pen(_borderColor, _borderWidth);
				g.DrawPath(pen, borderPath);
			}
		}

		private static RectangleF GetImageRectangle(Image image, Rectangle bounds, PictureBoxSizeMode sizeMode)
		{
			if (bounds.Width <= 0 || bounds.Height <= 0)
				return RectangleF.Empty;

			return sizeMode switch
			{
				PictureBoxSizeMode.StretchImage => bounds,
				PictureBoxSizeMode.CenterImage => new RectangleF(
					bounds.X + (bounds.Width - image.Width) / 2f,
					bounds.Y + (bounds.Height - image.Height) / 2f,
					image.Width,
					image.Height),
				PictureBoxSizeMode.AutoSize => new RectangleF(bounds.Location, image.Size),
				_ => GetZoomRectangle(image, bounds),
			};
		}

		private static RectangleF GetZoomRectangle(Image image, Rectangle bounds)
		{
			if (image.Width <= 0 || image.Height <= 0)
				return RectangleF.Empty;

			float scale = Math.Min(bounds.Width / (float)image.Width, bounds.Height / (float)image.Height);
			float width = image.Width * scale;
			float height = image.Height * scale;

			return new RectangleF(
				bounds.X + (bounds.Width - width) / 2f,
				bounds.Y + (bounds.Height - height) / 2f,
				width,
				height);
		}

		private static GraphicsPath CreateRoundPath(Rectangle rect, float radius)
			=> CreateRoundPath(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), radius);

		private static GraphicsPath CreateRoundPath(RectangleF rect, float radius)
		{
			var path = new GraphicsPath();
			if (rect.Width <= 0 || rect.Height <= 0)
				return path;

			radius = Math.Min(radius, Math.Min(rect.Width, rect.Height) / 2f);
			if (radius <= 0)
			{
				path.AddRectangle(rect);
				path.CloseFigure();
				return path;
			}

			float diameter = radius * 2f;
			path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
			path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
			path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
			path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
			path.CloseFigure();
			return path;
		}
	}
}
