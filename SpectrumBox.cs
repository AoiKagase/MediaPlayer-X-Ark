using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark
{
	/// <summary>
	/// Spectrum Analyzer Controll
	/// </summary>
    public partial class SpectrumBox : PictureBox
	{
		/// <summary>
		/// Graphics Buffer
		/// </summary>
		private Graphics gAnalyzer;
		private Graphics gBuffer;
		private Graphics gSnow;

		/// <summary>
		/// HDC Pointers
		/// </summary>
		private IntPtr hdcBuffer;
		private IntPtr hdc1Analyzer;
		private IntPtr hdc3Snow;
		private IntPtr hdc2AnalyzerSrc;
		private IntPtr hdc3SnowSrc;

		/// <summary>
		/// HBitmap Object
		/// </summary>
		private IntPtr Prog1;
		private IntPtr Prog2;

		/// <summary>
		/// 画像
		/// </summary>
		private Color _BackColor;
		private Bitmap _BackImage;
		private Bitmap _BitmapSpectrum;
		private Bitmap _BitmapSnow;
		private Bitmap _BitmapWave;

		/// <summary>
		/// スレッド
		/// </summary>
		private Thread DrawThread;

		/// <summary>
		/// スペクトラム領域
		/// </summary>
		private const int windowSize = 1024;

		/// <summary>
		/// 初期化済
		/// </summary>
		private bool Initialized { get; set; }
		/// <summary>
		/// 次フレーム数
		/// </summary>
		private double nextframe = (double)System.Environment.TickCount;
		/// <summary>
		/// FFT数値
		/// </summary>
		private float[] _mFFT;
		private float[] _mWave;
		/// <summary>
		/// Snow block
		/// </summary>
		private float[] analyzerSnow;
		/// <summary>
		/// 1FPS秒
		/// </summary>
		private float wait = 1000f / 60f;
		/// <summary>
		/// ソース領域
		/// </summary>
		private RECT src1 = new RECT(0, 0, 1, 1);

		/// <summary>
		/// スペクトラム間隔
		/// </summary>
		public int Mode { get; set; }
		public override Color BackColor
		{
			get { return _BackColor; }
			set
			{
				_BackColor = value;
			}
		}
		public override Image BackgroundImage
		{
			get { return _BackImage; }
			set
			{
				if (value != null)
				{
					_BackImage = (Bitmap)value;
				} else
				{
					if (_BackImage != null)
						_BackImage.Dispose();
				}
			}
		}
		/// <summary>
		/// Spectrum Analyzer Image.
		/// </summary>
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
				} else
				{
					if(_BitmapSpectrum != null)
						_BitmapSpectrum.Dispose();
				}
			}
		}

		/// <summary>
		/// Snow Block Image.
		/// </summary>
		public Bitmap BitmapSnow
		{
			get { return _BitmapSnow; }
			set
			{
				if (value != null)
                {
					_BitmapSnow = value;
				} else
				{
					if (_BitmapSnow != null)
						_BitmapSnow.Dispose();
				}
			}
		}

		/// <summary>
		/// Wave Image.
		/// </summary>
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
				} else
				{
					if (_BitmapWave != null)
						_BitmapWave.Dispose();
				}
			}
        }
		/// <summary>
		/// FFT
		/// </summary>
		public float[] mFFT
		{
			get { return _mFFT; }
			set { _mFFT = value; }
		}
		public float[] mWave
		{
			get { return _mWave; }
			set { _mWave = value; }
		}
		public SpectrumBox()
		{
			InitializeComponent();

			// スタイルの指定
			SetStyle(ControlStyles.AllPaintingInWmPaint |// ちらつき抑える
				ControlStyles.Opaque, true);            // 背景は描画しない

			// 空オブジェクトの作成
			_BitmapSpectrum = new Bitmap(this.Width, this.Height);
			_BitmapSnow = new Bitmap(this.Width, this.Height);
			_BitmapWave = new Bitmap(this.Width, this.Height);

			// 破棄処理
			this.Disposed += (sender, args) =>
			{
				Initialized = false;
			};
		}

		/// <summary>
		/// Initialize
		/// </summary>
		/// <param name="backColor">背景色</param>
		public void Initialize()
		{
			// 背景色設定
//			_BackColor = backColor;
			// Snow blockエリア確保
			analyzerSnow = new float[windowSize];

			// 初期化完了
			Initialized = true;

			// スレッド作成
			DrawThread = new Thread(new ThreadStart(DrawSpectrum));
			DrawThread.Start();
		}

		/// <summary>
		/// メインループ処理
		/// </summary>
		public void DrawSpectrum()
		{
			// 各座標初期化
			RECT line1 = new RECT(0, 0, 0, 0);
			RECT line2 = new RECT(0, 0, this.Width, this.Height); // BackGround
			RECT line3 = new RECT(0, 0, 0, 0);  // Snow

			// HBitmapポインタ取得
			IntPtr hBSrc = _BitmapSpectrum.GetHbitmap(Color.Transparent);
			IntPtr hBSnow = _BitmapSnow.GetHbitmap(Color.White);

			// バックバッファーを保持する
			gAnalyzer = this.CreateGraphics();
			gSnow = Graphics.FromImage(_BitmapSnow);
			gBuffer = Graphics.FromImage(_BitmapSpectrum);

			// Snow block色設定
			using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
			{
				gSnow.FillRectangle(brush, 0, 0, this.Width, this.Height);
			}

			// HDC確保
			hdcBuffer = gBuffer.GetHdc();
			hdc1Analyzer = gAnalyzer.GetHdc();
			hdc2AnalyzerSrc = Win32API.CreateCompatibleDC(hdc1Analyzer);
			hdc3Snow = gSnow.GetHdc();
			hdc3SnowSrc = Win32API.CreateCompatibleDC(hdc3Snow);

			// 初期化済みであればループ開始
			while (Initialized)
			{
				// HBitmapオブジェクト確保
				Prog1 = Win32API.SelectObject(hdc2AnalyzerSrc, hBSrc);
				Prog2 = Win32API.SelectObject(hdc3SnowSrc, hBSnow);

				// 次フレーム数を超えている場合
				if ((double)System.Environment.TickCount >= nextframe)
				{
					// かつ＋1FPS秒（さらに次フレーム）以内
					if ((double)System.Environment.TickCount < nextframe + wait)
					{
						// 描画クリア
						Win32API.PatBlt(hdcBuffer, 0, 0, this.Width, this.Height, 0);

						switch(Mode)
						{
							// WAVE MODE
							case 4:
								if (mWave != null)
								{
									// Wave描画
									int prevX = 0;
									int prevY = this.Height / 2;

									for (int i = 0; i < mWave.Length && i < this.Width; i++)
									{
										int x = i;
										// -1.0〜+1.0 を 0〜Height にマッピング
										int y = (int)((1.0f - mWave[i]) * this.Height / 2f);
										y = Math.Max(0, Math.Min(this.Height - 1, y));

										// BitBltで直接描画する既存構造に合わせるため
										// gBuffer（バックバッファ）に直接描画
										using (var pen = new Pen(Color.Lime, 1))
										using (var g = Graphics.FromHdc(hdcBuffer))
										{
											g.DrawLine(pen, prevX, prevY, x, y);
										}
										prevX = x;
										prevY = y;
									}
								}
								break;
							// SNOW BLOCK
							case 3:
							default:
								// FFT取得済み
								if (mFFT != null)
								{
									// バーの高さ初期化
									int lineHeight = 0;

									// 横間隔の取得
									int step = (Mode > 0) ? Mode * 2 : 1;

									// 画像処理用の座標計算開始
									// 横間隔で間引き有り
									for (int i = 0; i < windowSize; i += step)
									{
										// バー高さ取得
										lineHeight = this.Height - (int)((this.Height / 80f) * ((lin2dB(mFFT[i]) + 80f)));
										//                                Math.Clamp((float)Math.Log10(linear) * 20.0f, -80.0f, 0.0f);

										// 横位置（左）
										line3.Left = i;
										line3.Bottom = this.Height;
										// 横位置（右）左位置+1pxを基準として横間隔/2分広げる
										// スペクトラム領域より描画域が広い場合はバーの横幅を広げる
										if (this.Width > windowSize)
											line3.Right = i + (this.Width / windowSize) + (int)(Mode / 2f);
										else
											line3.Right = i + 1 + (int)(Mode / 2f);

										// SnowBlockの位置計算
										// 1フレーム前のSnowBlock高さより現フレームのバー高さの方が高い場合は押し上げる
										if (analyzerSnow[i] > lineHeight)
											line3.Bottom = (int)(analyzerSnow[i] = lineHeight);
										// 落下
										else if (analyzerSnow[i] < this.Height)
											line3.Bottom = (int)(analyzerSnow[i] += 0.2f);

										// SnowBlockの上位置 下位値の-1px
										line3.Top = line3.Bottom - 1;

										// バー位置
										line1.Bottom = this.Height; // 下部固定
										line1.Top = lineHeight;     // 上部計算
										line1.Left = line3.Left;    // Snow Block左に合わせる(前処理で計算済み)
										line1.Right = line3.Right;  // Snow Block右に合わせる(前処理で計算済み)

										// 描画元画像の読み取り位置
										src1.Top = lineHeight;
										src1.Bottom = this.Height;

										// バックバッファへ描画
										// SnowBlock
										Win32API.BitBlt(hdcBuffer, line3.Left, line3.Top, line3.Right - line3.Left, 1, hdc3SnowSrc, 0, 0, Win32API.TernaryRasterOperations.SRCCOPY);
										if (Mode != 3)
										{
											// Spectrum Bar
											Win32API.BitBlt(hdcBuffer, line1.Left, line1.Top, line1.Right - line1.Left, line1.Bottom - line1.Top, hdc2AnalyzerSrc, line1.Left, line1.Top, Win32API.TernaryRasterOperations.SRCCOPY);
										}
									}
								}
								break;
						}
						// バックバッファから転送
						Win32API.BitBlt(hdc1Analyzer, 0, 0, this.Width, this.Height, hdcBuffer, 0, 0, Win32API.TernaryRasterOperations.SRCCOPY);
					}
					// 次フレーム計算
					nextframe += wait;
				}

				// HBitmapオブジェクトリリース
				Win32API.DeleteObject(hBSrc);
				Win32API.DeleteObject(hBSnow);
				Win32API.DeleteObject(Prog2);
				Win32API.DeleteObject(Prog1);

				// おまじない
				Application.DoEvents();
				Thread.Sleep(1);
			}

			// リソース解放
			Win32API.DeleteDC(hdc2AnalyzerSrc);
			Win32API.DeleteDC(hdc3SnowSrc);
			Win32API.DeleteObject(hdc2AnalyzerSrc);
			Win32API.DeleteObject(hdc3SnowSrc);

			gAnalyzer.ReleaseHdc(hdc1Analyzer);
			gSnow.ReleaseHdc(hdc3Snow);
			gAnalyzer.Dispose();
			gSnow.Dispose();
		}
		/// <summary>
		/// Linear to Decibel
		/// </summary>
		/// <param name="linear"></param>
		/// <returns></returns>
		private float lin2dB(float linear)
		{
			return Math.Clamp((float)Math.Log10(linear) * 20.0f, -80.0f, 0.0f);
		}
	}
}
