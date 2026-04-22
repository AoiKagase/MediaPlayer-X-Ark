using MediaPlayer_X_Ark.Engine.Render;
using SharpGen.Runtime;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Threading;
using System.Windows.Forms;
using Vortice.DCommon;
using Vortice.Direct2D1;
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
		public System.Drawing.Color WaveColorL { get; set; } = System.Drawing.Color.Lime;
		public System.Drawing.Color WaveColorR { get; set; } = System.Drawing.Color.Cyan;
		/// <summary>スノーブロック落下速度（px/frame）。MainFormが px/秒 × 更新間隔 で計算して設定する。</summary>
		public float SnowFallSpeed { get; set; } = 0.72f;

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
					var cloned = CloneBitmap(value);
					_bitmapBackground?.Dispose();
					_bitmapBackground = cloned;
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
					var cloned = CloneBitmap(value);
					_bitmapSpectrum?.Dispose();
					_bitmapSpectrum = cloned;
					_d2dBitmapSpectrum?.Dispose();
					_d2dBitmapSpectrum = CreateD2DBitmap(_bitmapSpectrum);
				}

				Invalidate();
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
				return;

			_analyzerSnow = new float[WindowSize];
			_initialized = true;
			Invalidate();
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (D2DContext.Factory == null)
				return;

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

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			if (_initialized)
				_analyzerSnow = new float[WindowSize];
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			RenderFrame();
		}

		protected override void OnPaintBackground(PaintEventArgs pevent) { }

		private void CreateRenderTarget()
		{
			if (!IsHandleCreated || D2DContext.Factory == null)
				return;

			DisposeDeviceResources();
			var pixelSize = GetClientPixelSize();
			var renderTargetProperties = new RenderTargetProperties
			{
				DpiX = 96f,
				DpiY = 96f
			};

			_renderTarget = D2DContext.Factory.CreateHwndRenderTarget(
				renderTargetProperties,
				new HwndRenderTargetProperties
				{
					Hwnd = Handle,
					PixelSize = pixelSize,
					PresentOptions = PresentOptions.None
				});

			_snowBrush      = _renderTarget.CreateSolidColorBrush(new Color4(1f, 1f, 1f, 1f));
			_waveLeftBrush  = _renderTarget.CreateSolidColorBrush(ToColor4(WaveColorL));
			_waveRightBrush = _renderTarget.CreateSolidColorBrush(ToColor4(WaveColorR));

			RefreshBackgroundBitmap();
			_d2dBitmapSpectrum = CreateD2DBitmap(_bitmapSpectrum);
		}

		/// <summary>
		/// RenderTarget を再生成せずにウェーブブラシだけ再構築する。
		/// WaveColorL/R を変更した後に呼ぶこと。
		/// </summary>
		public void RefreshBrushes()
		{
			lock (_bitmapLock)
			{
				if (_renderTarget == null)
					return;
				_waveLeftBrush?.Dispose();
				_waveRightBrush?.Dispose();
				_waveLeftBrush  = _renderTarget.CreateSolidColorBrush(ToColor4(WaveColorL));
				_waveRightBrush = _renderTarget.CreateSolidColorBrush(ToColor4(WaveColorR));
			}
		}

		private void DisposeDeviceResources()
		{
			_waveRightBrush?.Dispose();    _waveRightBrush    = null;
			_waveLeftBrush?.Dispose();     _waveLeftBrush     = null;
			_snowBrush?.Dispose();         _snowBrush         = null;
			_d2dBitmapSpectrum?.Dispose(); _d2dBitmapSpectrum = null;
			_d2dBitmapBackground?.Dispose(); _d2dBitmapBackground = null;
			_renderTarget?.Dispose();      _renderTarget      = null;
		}

		private int _isRendering = 0; // 0 = idle, 1 = rendering

		public void RenderFrame()
		{
			if (Interlocked.Exchange(ref _isRendering, 1) == 1)
				return;
			try
			{
				lock (_bitmapLock)
				{
					if (!_initialized || !IsHandleCreated || IsDisposed)
						return;

					if (_renderTarget == null)
					{
						CreateRenderTarget();
						if (_renderTarget == null)
							return;
					}

					var targetSize = _renderTarget.PixelSize;
					var pixelSize = GetClientPixelSize();
					if (targetSize.Width != pixelSize.Width || targetSize.Height != pixelSize.Height)
					{
						_renderTarget.Resize(pixelSize);
						_analyzerSnow = new float[WindowSize];
						RefreshBackgroundBitmap();
					}

					bool began = false;
					try
					{
						_renderTarget.BeginDraw();
						began = true;
						_renderTarget.Transform = Matrix3x2.Identity;

						_renderTarget.Clear(ToColor4(_backColor.IsEmpty ? System.Drawing.Color.Black : _backColor));
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
					}
					catch (Exception ex)
					{
						System.Diagnostics.Debug.WriteLine($"RenderFrame unexpected: {ex}");
					}
					finally
					{
						if (began)
						{
							_renderTarget.Transform = Matrix3x2.Identity;
							try { _renderTarget?.EndDraw(); }
							catch (SharpGenException ex)
							{
								Debug.WriteLine($"EndDraw failed: 0x{ex.HResult:X}");
								DisposeDeviceResources();
								if (IsHandleCreated && !IsDisposed)
									BeginInvoke((Action)Invalidate);
							}
							catch { DisposeDeviceResources(); }
						}
					}
				}
			}
			finally
			{
				Interlocked.Exchange(ref _isRendering, 0);
			}
		}

		private void DrawBackground()
		{
			if (_d2dBitmapBackground == null)
				return;

			var sourceSize = _d2dBitmapBackground.PixelSize;
			int width = Math.Max(1, ClientSize.Width);
			int height = Math.Max(1, ClientSize.Height);
			_renderTarget.DrawBitmap(
				_d2dBitmapBackground,
				new Rect(0, 0, width, height),
				1.0f,
				BitmapInterpolationMode.Linear,
				new Rect(0, 0, sourceSize.Width, sourceSize.Height));
		}

		private void DrawWave()
		{
			int height = Math.Max(1, ClientSize.Height);
			int width  = Math.Max(1, ClientSize.Width);

			if (_mWaveL != null && _waveLeftBrush != null)
			{
				using var path = D2DContext.Factory.CreatePathGeometry();
				using var sink = path.Open();
				float startY = _mWaveL.Length > 0
					? Math.Clamp((1.0f - _mWaveL[0]) * height / 2f, 0, height - 1)
					: height / 2f;
				sink.BeginFigure(new Vector2(0, startY), FigureBegin.Hollow);
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
				float startY = _mWaveR.Length > 0
					? Math.Clamp(height - ((1.0f - _mWaveR[0]) * height / 2f), 0, height - 1)
					: height / 2f;
				sink.BeginFigure(new Vector2(0, startY), FigureBegin.Hollow);
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
				return;

			int width = Math.Max(1, ClientSize.Width);
			int height = Math.Max(1, ClientSize.Height);
			int step      = Mode > 0 ? Mode * 2 : 1;
			int fftLength = Math.Min(WindowSize, _mFFT.Length);

			for (int i = 0; i < fftLength; i += step)
			{
				float db         = lin2dB(Math.Max(_mFFT[i], float.Epsilon));
				int   lineHeight = height - (int)((height / 80f) * (db + 80f));
				lineHeight = Math.Clamp(lineHeight, 0, height);

				int left  = i;
				int right = width > WindowSize
					? i + (width / WindowSize) + (int)(Mode / 2f)
					: i + 1 + (int)(Mode / 2f);
				right = Math.Clamp(Math.Min(right, width), Math.Min(left + 1, width), width);

				int snowBottom = height;
				if (_analyzerSnow[i] > lineHeight)
					snowBottom = (int)(_analyzerSnow[i] = lineHeight);
				else if (_analyzerSnow[i] < height)
					snowBottom = (int)(_analyzerSnow[i] += SnowFallSpeed);

				if (SnowBlockEnabled && _snowBrush != null)
					_renderTarget.FillRectangle(new Rect(left, snowBottom - 1, right - left, 1), _snowBrush);

				if (_d2dBitmapSpectrum == null)
					continue;

				int barHeight = height - lineHeight;
				if (barHeight <= 0)
					continue;

				float srcLeft   = Math.Clamp(left,        0, _d2dBitmapSpectrum.PixelSize.Width);
				float srcTop    = Math.Clamp(lineHeight,   0, _d2dBitmapSpectrum.PixelSize.Height);
				float srcRight  = Math.Clamp(right,        0, _d2dBitmapSpectrum.PixelSize.Width);
				float srcBottom = Math.Clamp(height,       0, _d2dBitmapSpectrum.PixelSize.Height);

				if (srcRight <= srcLeft || srcBottom <= srcTop)
					continue;

				_renderTarget.DrawBitmap(
					_d2dBitmapSpectrum,
					new Rect(left, lineHeight, right - left, barHeight),
					1f,
					BitmapInterpolationMode.NearestNeighbor,
					new Rect(srcLeft, srcTop, srcRight - srcLeft, srcBottom - srcTop));
			}
		}

		private void RefreshBackgroundBitmap()
		{
			_d2dBitmapBackground?.Dispose();
			_d2dBitmapBackground = null;

			if (_renderTarget == null || _bitmapBackground == null)
				return;

			_d2dBitmapBackground = CreateD2DBitmap(_bitmapBackground);
		}

		private void DisposeManagedBitmaps()
		{
			_bitmapSpectrum?.Dispose();   _bitmapSpectrum   = null;
			_bitmapBackground?.Dispose(); _bitmapBackground = null;
		}

		private ID2D1Bitmap CreateD2DBitmap(Bitmap source)
		{
			if (_renderTarget == null || source == null || source.Width <= 0 || source.Height <= 0)
				return null;

			Bitmap converted;
			try
			{
				converted = Ensure32bppPArgb(source);
			}
			catch (ArgumentException ex)
			{
				Debug.WriteLine($"CreateD2DBitmap skipped invalid bitmap: {ex.Message}");
				return null;
			}

			using (converted)
			{
			var bitmapData = converted.LockBits(
				new Rectangle(0, 0, converted.Width, converted.Height),
				ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			try
			{
				var props = new BitmapProperties(
					new Vortice.DCommon.PixelFormat(Vortice.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied));

				return _renderTarget.CreateBitmap(
					new SizeI(converted.Width, converted.Height),
					bitmapData.Scan0,
					(uint)Math.Abs(bitmapData.Stride),
					props);
			}
			finally
			{
				converted.UnlockBits(bitmapData);
			}
			}
		}

		private static Bitmap CloneBitmap(Bitmap source)
		{
			if (source == null)
				return null;

			return (Bitmap)source.Clone();
		}

		private static Bitmap Ensure32bppPArgb(Bitmap source)
		{
			if (source.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppPArgb)
				return (Bitmap)source.Clone();

			var converted = new Bitmap(source.Width, source.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			using var g = Graphics.FromImage(converted);
			g.DrawImage(source, new Rectangle(0, 0, converted.Width, converted.Height));
			return converted;
		}

		private static Color4 ToColor4(System.Drawing.Color color)
			=> new Color4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);

		private float lin2dB(float linear)
			=> Math.Clamp((float)Math.Log10(linear) * 20.0f, -80.0f, 0.0f);

		private SizeI GetClientPixelSize()
		{
			int width = Math.Max(1, ClientSize.Width);
			int height = Math.Max(1, ClientSize.Height);
			return new SizeI(width, height);
		}
	}
}
