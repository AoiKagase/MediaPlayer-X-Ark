using MediaPlayer_X_Ark.Engine.Render;
using SharpGen.Runtime;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Windows.Forms;
using Vortice.Direct2D1;
using Vortice.DCommon;
using Vortice.Mathematics;

namespace MediaPlayer_X_Ark.Controls
{
	/// <summary>
	/// Spectrum Analyzer Control (Direct2D 描画)
	/// </summary>
	public partial class SpectrumAnalyzer : PictureBox
	{
		private readonly object _bitmapLock = new();

		private Bitmap _bitmapBackground;
		private Bitmap _bitmapSpectrum;
		private Bitmap _bitmapSnow;
		private Bitmap _bitmapWave;

		private ID2D1HwndRenderTarget _renderTarget;
		private ID2D1Bitmap _d2dBitmapBackground;
		private ID2D1Bitmap _d2dBitmapSpectrum;
		private ID2D1SolidColorBrush _snowBrush;
		private ID2D1SolidColorBrush _waveLeftBrush;
		private ID2D1SolidColorBrush _waveRightBrush;

		private System.Drawing.Color _backColor;
		private const int WindowSize = 1024;
		private bool _initialized;
		private float[] _mFFT;
		private float[] _mWaveL;
		private float[] _mWaveR;
		private float[] _analyzerSnow;

		public bool SnowBlockEnabled { get; set; } = true;
		public int Mode { get; set; }

		public override System.Drawing.Color BackColor
		{
			get => _backColor;
			set => _backColor = value;
		}

		public Bitmap BitmapBackground
		{
			get
			{
				lock (_bitmapLock)
				{
					return _bitmapBackground;
				}
			}
			set
			{
				lock (_bitmapLock)
				{
					if (!ReferenceEquals(_bitmapBackground, value))
					{
						_bitmapBackground?.Dispose();
					}

					_bitmapBackground = value;
					RefreshBackgroundBitmap();
				}

				Invalidate();
			}
		}

		public Bitmap BitmapSpectrum
		{
			get
			{
				lock (_bitmapLock)
				{
					return _bitmapSpectrum;
				}
			}
			set
			{
				lock (_bitmapLock)
				{
					if (!ReferenceEquals(_bitmapSpectrum, value))
					{
						_bitmapSpectrum?.Dispose();
					}

					_bitmapSpectrum = value;
					_d2dBitmapSpectrum?.Dispose();
					_d2dBitmapSpectrum = CreateD2DBitmap(_bitmapSpectrum);
				}

				Invalidate();
			}
		}

		public Bitmap BitmapSnow
		{
			get
			{
				lock (_bitmapLock)
				{
					return _bitmapSnow;
				}
			}
			set
			{
				lock (_bitmapLock)
				{
					if (!ReferenceEquals(_bitmapSnow, value))
					{
						_bitmapSnow?.Dispose();
					}

					_bitmapSnow = value;
				}
			}
		}

		public Bitmap BitmapWave
		{
			get
			{
				lock (_bitmapLock)
				{
					return _bitmapWave;
				}
			}
			set
			{
				lock (_bitmapLock)
				{
					if (!ReferenceEquals(_bitmapWave, value))
					{
						_bitmapWave?.Dispose();
					}

					_bitmapWave = value;
				}
			}
		}

		public float[] mFFT
		{
			get => _mFFT;
			set => _mFFT = value;
		}

		public float[] mWaveL
		{
			get => _mWaveL;
			set => _mWaveL = value;
		}

		public float[] mWaveR
		{
			get => _mWaveR;
			set => _mWaveR = value;
		}

		public SpectrumAnalyzer()
		{
			SetStyle(
				ControlStyles.UserPaint |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.Opaque |
				ControlStyles.ResizeRedraw,
				true);
			UpdateStyles();

			Disposed += (_, _) =>
			{
				_initialized = false;
				lock (_bitmapLock)
				{
					DisposeDeviceResources();
					DisposeManagedBitmaps();
				}
			};
		}

		public void Initialize()
		{
			if (_initialized)
			{
				return;
			}

			_analyzerSnow = new float[WindowSize];
			_initialized = true;
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (D2DContext.Factory == null)
			{
				return;
			}

			lock (_bitmapLock)
			{
				CreateRenderTarget();
			}
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			lock (_bitmapLock)
			{
				DisposeDeviceResources();
			}

			base.OnHandleDestroyed(e);
		}

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			RefreshBackgroundFromParent();
		}

		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);
			RefreshBackgroundFromParent();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			if (_initialized)
			{
				_analyzerSnow = new float[WindowSize];
			}

			RefreshBackgroundFromParent();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			RenderFrame();
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}

		protected override void WndProc(ref Message m)
		{
			const int WM_ERASEBKGND = 0x0014;

			if (m.Msg == WM_ERASEBKGND)
			{
				m.Result = IntPtr.Zero;
				return;
			}

			base.WndProc(ref m);
		}

		private void CreateRenderTarget()
		{
			if (!IsHandleCreated || D2DContext.Factory == null)
			{
				return;
			}

			DisposeDeviceResources();

			_renderTarget = D2DContext.Factory.CreateHwndRenderTarget(
				new RenderTargetProperties(),
				new HwndRenderTargetProperties
				{
					Hwnd = Handle,
					PixelSize = new Vortice.Mathematics.SizeI(Math.Max(1, Width), Math.Max(1, Height)),
					PresentOptions = PresentOptions.None
				});

			_snowBrush = _renderTarget.CreateSolidColorBrush(new Color4(1f, 1f, 1f, 1f));
			_waveLeftBrush = _renderTarget.CreateSolidColorBrush(new Color4(0f, 1f, 0f, 1f));
			_waveRightBrush = _renderTarget.CreateSolidColorBrush(new Color4(0f, 1f, 1f, 1f));

			RefreshBackgroundBitmap();
			_d2dBitmapSpectrum = CreateD2DBitmap(_bitmapSpectrum);

		}

		private void DisposeDeviceResources()
		{
			_waveRightBrush?.Dispose();
			_waveRightBrush = null;
			_waveLeftBrush?.Dispose();
			_waveLeftBrush = null;
			_snowBrush?.Dispose();
			_snowBrush = null;
			_d2dBitmapSpectrum?.Dispose();
			_d2dBitmapSpectrum = null;
			_d2dBitmapBackground?.Dispose();
			_d2dBitmapBackground = null;
			_renderTarget?.Dispose();
			_renderTarget = null;
		}

		public void RenderFrame()
		{
			lock (_bitmapLock)
			{
				if (!_initialized || !IsHandleCreated || IsDisposed)
				{
					return;
				}

				if (_renderTarget == null)
				{
					CreateRenderTarget();
					if (_renderTarget == null)
					{
						return;
					}
				}

				var targetSize = _renderTarget.PixelSize;
				if (targetSize.Width != Width || targetSize.Height != Height)
				{
					_renderTarget.Resize(new SizeI(Math.Max(1, Width), Math.Max(1, Height)));
					_analyzerSnow = new float[WindowSize];
					RefreshBackgroundBitmap();
				}

				try
				{
					_renderTarget.BeginDraw();
					_renderTarget.Clear(ToColor4(_backColor.IsEmpty ? System.Drawing.Color.Black : _backColor));
					//_renderTarget.Clear(ToColor4(System.Drawing.Color.Red));

					DrawBackground();

					switch (Mode)
					{
						case 4:
							DrawWave();
							break;
						default:
							DrawSpectrum();
							break;
					}

					_renderTarget.EndDraw();
				}
				catch (SharpGenException ex)
				{
					System.Diagnostics.Debug.WriteLine($"D2D EndDraw failed: 0x{ex.HResult:X} - {ex.Message}");

					DisposeDeviceResources();
					if (IsHandleCreated && !IsDisposed)
					{
						BeginInvoke((Action)Invalidate);
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"RenderFrame unexpected: {ex}");
				}
				return;
			}
		}

		private void DrawBackground()
		{
			if (_d2dBitmapBackground == null)
			{
				return;
			}

			var sourceSize = _d2dBitmapBackground.PixelSize;
			var sourceRect = new Rect(0, 0, sourceSize.Width, sourceSize.Height);
			var destinationRect = new Rect(0, 0, Width, Height);

			_renderTarget.DrawBitmap(
				_d2dBitmapBackground,
				destinationRect,
				1.0f,
				BitmapInterpolationMode.Linear,
				sourceRect);
		}

		private void DrawWave()
		{
			int height = Height;
			int width = Width;

			if (_mWaveL != null && _waveLeftBrush != null)
			{
				using var path = D2DContext.Factory.CreatePathGeometry();
				using var sink = path.Open();
				sink.BeginFigure(new Vector2(0, height / 2f), FigureBegin.Hollow);

				for (int i = 1; i < _mWaveL.Length && i < width; i++)
				{
					float y = Math.Clamp((1.0f - _mWaveL[i]) * height / 2f, 0, height - 1);
					sink.AddLine(new Vector2(i, y));
				}

				sink.EndFigure(FigureEnd.Open);
				sink.Close();
				_renderTarget.DrawGeometry(path, _waveLeftBrush, 1f);
			}

			if (_mWaveR != null && _waveRightBrush != null)
			{
				using var path = D2DContext.Factory.CreatePathGeometry();
				using var sink = path.Open();
				sink.BeginFigure(new Vector2(0, height / 2f), FigureBegin.Hollow);

				for (int i = 1; i < _mWaveR.Length && i < width; i++)
				{
					float y = Math.Clamp(height - ((1.0f - _mWaveR[i]) * height / 2f), 0, height - 1);
					sink.AddLine(new Vector2(i, y));
				}

				sink.EndFigure(FigureEnd.Open);
				sink.Close();
				_renderTarget.DrawGeometry(path, _waveRightBrush, 1f);
			}
		}

		private void DrawSpectrum()
		{
			if (_mFFT == null || _analyzerSnow == null)
			{
				return;
			}

			int step = Mode > 0 ? Mode * 2 : 1;
			int fftLength = Math.Min(WindowSize, _mFFT.Length);

			for (int i = 0; i < fftLength; i += step)
			{
				float db = lin2dB(Math.Max(_mFFT[i], float.Epsilon));
				int lineHeight = Height - (int)((Height / 80f) * (db + 80f));
				lineHeight = Math.Clamp(lineHeight, 0, Height);

				int left = i;
				int right = Width > WindowSize
					? i + (Width / WindowSize) + (int)(Mode / 2f)
					: i + 1 + (int)(Mode / 2f);
				right = Math.Clamp(Math.Min(right, Width), Math.Min(left + 1, Width), Width);

				int snowBottom = Height;
				if (_analyzerSnow[i] > lineHeight)
				{
					snowBottom = (int)(_analyzerSnow[i] = lineHeight);
				}
				else if (_analyzerSnow[i] < Height)
				{
					snowBottom = (int)(_analyzerSnow[i] += 0.8f);
				}

				int snowTop = snowBottom - 1;

				if (SnowBlockEnabled && _snowBrush != null)
				{
					_renderTarget.FillRectangle(
						new Rect(left, snowTop, right - left, 1),
						_snowBrush);
				}

				if (_d2dBitmapSpectrum == null)
				{
					continue;
				}

				int barTop = lineHeight;
				int barHeight = Height - barTop;
				if (barHeight <= 0)
				{
					continue;
				}

				float srcLeft = Math.Clamp(left, 0, _d2dBitmapSpectrum.PixelSize.Width);
				float srcTop = Math.Clamp(barTop, 0, _d2dBitmapSpectrum.PixelSize.Height);
				float srcRight = Math.Clamp(right, 0, _d2dBitmapSpectrum.PixelSize.Width);
				float srcBottom = Math.Clamp(Height, 0, _d2dBitmapSpectrum.PixelSize.Height);

				if (srcRight <= srcLeft || srcBottom <= srcTop)
				{
					continue;
				}

				var srcRect = new Rect(srcLeft, srcTop, srcRight - srcLeft, srcBottom - srcTop);
				var dstRect = new Rect(left, barTop, right - left, Height - barTop);
				_renderTarget.DrawBitmap(
					_d2dBitmapSpectrum,
					dstRect,
					1f,
					BitmapInterpolationMode.NearestNeighbor,
					srcRect);
			}
		}

		private void RefreshBackgroundFromParent()
		{
			lock (_bitmapLock)
			{
				if (_bitmapBackground != null)
				{
					return;
				}

				RefreshBackgroundBitmap();
			}

			Invalidate();
		}

		private void RefreshBackgroundBitmap()
		{
			_d2dBitmapBackground?.Dispose();
			_d2dBitmapBackground = null;

			if (_renderTarget == null)
			{
				return;
			}

			var source = _bitmapBackground ?? CaptureParentBackground();
			if (source == null)
			{
				return;
			}

			try
			{
				_d2dBitmapBackground = CreateD2DBitmap(source);
			}
			finally
			{
				if (!ReferenceEquals(source, _bitmapBackground))
				{
					source.Dispose();
				}
			}
		}

		private void DisposeManagedBitmaps()
		{
			_bitmapWave?.Dispose();
			_bitmapWave = null;
			_bitmapSnow?.Dispose();
			_bitmapSnow = null;
			_bitmapSpectrum?.Dispose();
			_bitmapSpectrum = null;
			_bitmapBackground?.Dispose();
			_bitmapBackground = null;
		}

		private ID2D1Bitmap CreateD2DBitmap(Bitmap source)
		{
			if (_renderTarget == null || source == null || source.Width <= 0 || source.Height <= 0)
			{
				return null;
			}

			using var converted = Ensure32bppPArgb(source);
			var bitmapData = converted.LockBits(
				new Rectangle(0, 0, converted.Width, converted.Height),
				ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

			try
			{
				var properties = new BitmapProperties(
					new Vortice.DCommon.PixelFormat(Vortice.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied));

				return _renderTarget.CreateBitmap(
					new SizeI(converted.Width, converted.Height),
					bitmapData.Scan0,
					(uint)Math.Abs(bitmapData.Stride),
					properties);
			}
			finally
			{
				converted.UnlockBits(bitmapData);
			}
		}

		private static Bitmap Ensure32bppPArgb(Bitmap source)
		{
			if (source.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppPArgb)
			{
				return (Bitmap)source.Clone();
			}

			var converted = new Bitmap(source.Width, source.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			using var g = Graphics.FromImage(converted);
			g.DrawImage(source, new Rectangle(0, 0, converted.Width, converted.Height));
			return converted;
		}

		private Bitmap CaptureParentBackground()
		{
			var parentImage = Parent?.BackgroundImage;
			if (parentImage == null || Width <= 0 || Height <= 0)
			{
				return null;
			}

			var sourceRect = new Rectangle(Left, Top, Width, Height);
			sourceRect.Intersect(new Rectangle(Point.Empty, parentImage.Size));
			if (sourceRect.Width <= 0 || sourceRect.Height <= 0)
			{
				return null;
			}

			var captured = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			using var g = Graphics.FromImage(captured);
			g.Clear(System.Drawing.Color.Transparent);
			g.DrawImage(
				parentImage,
				new Rectangle(sourceRect.X - Left, sourceRect.Y - Top, sourceRect.Width, sourceRect.Height),
				sourceRect,
				GraphicsUnit.Pixel);
			return captured;
		}

		private static Color4 ToColor4(System.Drawing.Color color)
		{
			return new Color4(
				color.R / 255f,
				color.G / 255f,
				color.B / 255f,
				color.A / 255f);
		}

		private float lin2dB(float linear)
		{
			return Math.Clamp((float)Math.Log10(linear) * 20.0f, -80.0f, 0.0f);
		}
	}
}
