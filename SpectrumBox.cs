using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark
{
    public partial class SpectrumBox : PictureBox
	{
		protected Graphics gAnalyzer;
		protected Graphics gBuffer;
		protected Graphics gSnow;

		protected IntPtr hdcBuffer;
		protected IntPtr hdc1Analyzer;
		protected IntPtr hdc3Snow;
		protected IntPtr hdc2AnalyzerSrc;
		protected IntPtr hdc3SnowSrc;

		protected IntPtr Prog1;
		protected IntPtr Prog2;

		private Color _BackColor;
		/// <summary>
		/// 画像
		/// </summary>
		private Bitmap _BitmapSpectrum;
		private Bitmap _BitmapSnow;
		private Bitmap _BitmapWave;
		private const int windowSize = 1024;
		private Thread DrawThread;

		public Bitmap BitmapSpectrum
        {
			get
			{
				return _BitmapSpectrum;
			}
			set
			{
				if (value != null)
                {
					_BitmapSpectrum = value;
				}
			}
		}

		public Bitmap BitmapSnow
		{
			get { return _BitmapSnow; }
			set
			{
				if (value != null)
                {
					_BitmapSnow = value;
				}
			}
		}

		public Bitmap BitmapWave
        {
			get
			{
				return _BitmapWave;
			}
			set
			{
				if (value != null)
                {
					_BitmapWave = value;
				}
			}
        }
		/// <summary>
		/// 表示対象ハンドル
		/// </summary>
		protected IntPtr DisplayHandle { get { return Handle; } }

		public SpectrumBox()
		{
			InitializeComponent();

			// スタイルの指定
			SetStyle(ControlStyles.AllPaintingInWmPaint |// ちらつき抑える
				ControlStyles.Opaque, true);            // 背景は描画しない

			_BitmapSpectrum = new Bitmap(this.Width, this.Height);
			_BitmapSnow = new Bitmap(this.Width, this.Height);
			_BitmapWave = new Bitmap(this.Width, this.Height);
			this.Disposed += (sender, args) =>
			{
				Initialized = false;
			};
		}

		/// <summary>
		/// DirectXデバイスの初期化
		/// </summary>
		public void Initialize(System.Drawing.Color backColor)
		{
			_BackColor = backColor;
			analyzerSnow = new int[windowSize];
			Initialized = true;
			DrawThread = new Thread(new ThreadStart(DrawSpectrum));
			DrawThread.Start();

		}

		private float[] _mFFT;
		public float[] mFFT
        {
			get { return _mFFT; }
			set { _mFFT = value; }
        }
		private bool Initialized { get; set; }
		public int Mode { get; set; }
		private int[] analyzerSnow;
		double nextframe = (double)System.Environment.TickCount;
		float wait = 1000f / 60f;
		RECT src1 = new RECT(0, 0, 1, 76);
		RECT src2 = new RECT(0, 0, 1, 1);
		/// <summary>
		/// メインループ処理
		/// </summary>
		public void DrawSpectrum()
		{
			RECT line1 = new RECT(0, 0, 0, 0);
			RECT line2 = new RECT(0, 0, this.Width, this.Height); // BackGround
			RECT line3 = new RECT(0, 0, 0, 0);  // Snow

			IntPtr hBSrc = _BitmapSpectrum.GetHbitmap(_BackColor);
			IntPtr hBSnow = _BitmapSnow.GetHbitmap(Color.White);

			// バックバッファーを保持する
			gAnalyzer = this.CreateGraphics();
			gSnow = Graphics.FromImage(_BitmapSnow);
			gBuffer = Graphics.FromImage(_BitmapSpectrum);

			using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
			{
				gSnow.FillRectangle(brush, 0, 0, this.Width, this.Height);
			}
			hdcBuffer = gBuffer.GetHdc();
			hdc1Analyzer = gAnalyzer.GetHdc();
			hdc2AnalyzerSrc = Win32API.CreateCompatibleDC(hdc1Analyzer);
			hdc3Snow = gSnow.GetHdc();
			hdc3SnowSrc = Win32API.CreateCompatibleDC(hdc3Snow);

			while (Initialized)
			{
				Prog1 = Win32API.SelectObject(hdc2AnalyzerSrc, hBSrc);
				Prog2 = Win32API.SelectObject(hdc3SnowSrc, hBSnow);
				if ((double)System.Environment.TickCount >= nextframe)
				{
					// 画像描画
					if ((double)System.Environment.TickCount < nextframe + wait)
					{

						Win32API.FillRect(hdcBuffer, ref line2, Win32API.CreateSolidBrush(0xffffff00));

						// 計算処理
						if (mFFT != null)
						{

							int lineHeight = 0;
							int step = (Mode > 0) ? Mode * 2 : 1;

							// 画像処理用の座標計算開始
							for (int i = 0; i < windowSize; i += step)
							{
								lineHeight = this.Height - (int)((lin2dB(mFFT[i]) + 80f) * 1.2f);

								line3.Left = i;
								if (this.Width > windowSize)
									line3.Right = i + (this.Width / windowSize) + (int)(Mode / 2f);
								else
									line3.Right = i + 1 + (int)(Mode / 2f);

								if (analyzerSnow[i] > lineHeight)
									line3.Bottom = analyzerSnow[i] = lineHeight;
								else if (analyzerSnow[i] < this.Height)
									line3.Bottom = analyzerSnow[i] ++;

								line3.Top = line3.Bottom - 1;

								line1.Bottom = this.Height;
								line1.Top = lineHeight;
								line1.Left = line3.Left;
								line1.Right = line3.Right;
								src1.Top = lineHeight;
								//描画処理
								Win32API.BitBlt(hdcBuffer, line3.Left, line3.Top, line3.Right - line3.Left, 1, hdc3SnowSrc, 0, 0, Win32API.TernaryRasterOperations.SRCCOPY);
								Win32API.BitBlt(hdcBuffer, line1.Left, line1.Top, line1.Right - line1.Left, line1.Bottom - line1.Top, hdc2AnalyzerSrc, line1.Left, line1.Top, Win32API.TernaryRasterOperations.SRCCOPY);
							}
						}
						Win32API.BitBlt(hdc1Analyzer, 0, 0, this.Width, this.Height, hdcBuffer, 0, 0, Win32API.TernaryRasterOperations.SRCCOPY);
					}

					nextframe += wait;
				}
				Win32API.DeleteObject(hBSrc);
				Win32API.DeleteObject(hBSnow);
				Win32API.DeleteObject(Prog2);
				Win32API.DeleteObject(Prog1);

				Application.DoEvents();
			}
			Win32API.DeleteDC(hdc2AnalyzerSrc);
			Win32API.DeleteDC(hdc3SnowSrc);
			Win32API.DeleteObject(hdc2AnalyzerSrc);
			Win32API.DeleteObject(hdc3SnowSrc);
			gAnalyzer.ReleaseHdc(hdc1Analyzer);
			gSnow.ReleaseHdc(hdc3Snow);
			gAnalyzer.Dispose();
			gSnow.Dispose();
		}
		private float lin2dB(float linear)
		{
			return Math.Clamp((float)Math.Log10(linear) * 20.0f, -80.0f, 0.0f);
		}
	}
}
