using System;
using System.ComponentModel;
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
		private readonly object _bitmapLock = new object();

		/// <summary>
		/// 画像
		/// </summary>
		private Color _BackColor;
		private Bitmap _BitmapBackground;
		private Bitmap _BitmapSpectrum;
		private Bitmap _BitmapSnow;
		private Bitmap _BitmapWave;
        // バックバッファ（ダブルバッファリング用）
        private Bitmap _backBuffer;
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
		private float[] _mWaveL;
		private float[] _mWaveR;
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
		public Bitmap BitmapBackground {
			get
			{
				lock (_bitmapLock) { return _BitmapBackground; }
			}
			set
			{
				lock (_bitmapLock)
				{
					if (value != null)
					{
						_BitmapBackground = value;
					}
					else
					{
						if (_BitmapBackground != null)
							_BitmapBackground.Dispose();
					}
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
				lock (_bitmapLock)
				{
					return _BitmapSpectrum;
				}
			}
			set
			{
				lock (_bitmapLock)
				{
					if (value != null)
					{
						_BitmapSpectrum = value;
					}
					else
					{
						if (_BitmapSpectrum != null)
							_BitmapSpectrum.Dispose();
					}
				}
			}
		}

		/// <summary>
		/// Snow Block Image.
		/// </summary>
		public Bitmap BitmapSnow
		{
			get
			{
				lock (_bitmapLock) { return _BitmapSnow; }
			}
			set
			{
				lock (_bitmapLock)
				{
					if (value != null)
					{
						_BitmapSnow = value;
					}
					else
					{
						if (_BitmapSnow != null)
							_BitmapSnow.Dispose();
					}
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
				lock (_bitmapLock)
				{
					return _BitmapWave;
				}
			}
			set
			{
				lock (_bitmapLock)
				{
					if (value != null)
					{
						_BitmapWave = value;
					}
					else
					{
						if (_BitmapWave != null)
							_BitmapWave.Dispose();
					}
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
		public float[] mWaveL
		{
			get { return _mWaveL; }
			set { _mWaveL = value; }
		}
		public float[] mWaveR
		{
			get { return _mWaveR; }
			set { _mWaveR = value; }
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
			
			RECT line1 = new RECT(0, 0, 0, 0);
            RECT line3 = new RECT(0, 0, 0, 0);

            // バックバッファ初期化
            _backBuffer = new Bitmap(this.Width, this.Height);

			using (var snowBrush = new SolidBrush(Color.White))
			{
                // ★スレッド起動時点の時刻にリセット
                nextframe = (double)System.Environment.TickCount64 - 1;
				while (Initialized)
				{
					double now = (double)System.Environment.TickCount64;
					if (now >= nextframe)
					{
						// ★サイズ変更を検知して再生成
						if (_backBuffer.Width != this.Width || _backBuffer.Height != this.Height)
						{
							_backBuffer.Dispose();
							_backBuffer = new Bitmap(this.Width, this.Height);
							// Snow も再初期化
							analyzerSnow = new float[windowSize];
						}
						// バックバッファへ描画
						lock (_bitmapLock)
						{
							using (var g = Graphics.FromImage(_backBuffer))
							{
								// ① 背景クリア（PatBlt / BitBlt の代替）
								if (_BitmapBackground != null)
									g.DrawImage(_BitmapBackground, 0, 0);
								else
									g.Clear(Color.Black);

								switch (Mode)
								{
									// WAVE MODE
									case 4:
										// L ch（ライム色）
										if (mWaveL != null)
										{
											int prevX = 0, prevY = this.Height / 2;
											using (var pen = new Pen(Color.Lime, 1))
											{
												for (int i = 0; i < mWaveL.Length && i < this.Width; i++)
												{
													int x = i;
													int y = (int)((1.0f - mWaveL[i]) * this.Height / 2f);
													y = Math.Max(0, Math.Min(this.Height - 1, y));
													g.DrawLine(pen, prevX, prevY, x, y);
													prevX = x; prevY = y;
												}
											}
										}
										// R ch（シアン色）
										if (mWaveR != null)
										{
											int prevX = 0, prevY = this.Height / 2;
											using (var pen = new Pen(Color.Cyan, 1))
											{
												for (int i = 0; i < mWaveR.Length && i < this.Width; i++)
												{
													int x = i;
													int y = this.Height - (int)((1.0f - mWaveR[i]) * this.Height / 2f);
													y = Math.Max(0, Math.Min(this.Height - 1, y));
													g.DrawLine(pen, prevX, prevY, x, y);
													prevX = x; prevY = y;
												}
											}
										}
										break;

									// SNOW BLOCK / BAR
									case 3:
									default:
										if (mFFT != null)
										{
											int step = (Mode > 0) ? Mode * 2 : 1;

											for (int i = 0; i < windowSize; i += step)
											{
												int lineHeight = this.Height - (int)((this.Height / 80f) * (lin2dB(mFFT[i]) + 80f));

												line3.Left = i;
												line3.Bottom = this.Height;
												line3.Right = (this.Width > windowSize)
													? i + (this.Width / windowSize) + (int)(Mode / 2f)
													: i + 1 + (int)(Mode / 2f);

												if (analyzerSnow[i] > lineHeight)
													line3.Bottom = (int)(analyzerSnow[i] = lineHeight);
												else if (analyzerSnow[i] < this.Height)
													line3.Bottom = (int)(analyzerSnow[i] += 0.2f);

												line3.Top = line3.Bottom - 1;

												line1.Bottom = this.Height;
												line1.Top = lineHeight;
												line1.Left = line3.Left;
												line1.Right = line3.Right;

												// SnowBlock 描画
												//if (_BitmapSnow != null)
												//{
												//	var snowSrc = new Rectangle(0, 0, line3.Right - line3.Left, 1);
												//	var snowDst = new Rectangle(line3.Left, line3.Top, line3.Right - line3.Left, 1);
												//	g.DrawImage(_BitmapSnow, snowDst, snowSrc, GraphicsUnit.Pixel);
												//}
												g.FillRectangle(snowBrush, line3.Left, line3.Top,
													line3.Right - line3.Left, 1);

												// Spectrum Bar 描画
												if (Mode != 3 && _BitmapSpectrum != null)
												{
													var barSrc = new Rectangle(line1.Left, line1.Top, line1.Right - line1.Left, line1.Bottom - line1.Top);
													var barDst = new Rectangle(line1.Left, line1.Top, line1.Right - line1.Left, line1.Bottom - line1.Top);
													g.DrawImage(_BitmapSpectrum, barDst, barSrc, GraphicsUnit.Pixel);
												}
											}
										}
										break;
								}
							}
						}

						// ② バックバッファを画面へ転送
						if (this.IsHandleCreated && !this.IsDisposed)
						{
							try
							{
								this.Invoke((Action)(() =>
								{
									if (!this.IsDisposed)
									{
										using (var g = this.CreateGraphics())
											g.DrawImage(_backBuffer, 0, 0);
									}
								}));
							}
							catch (ObjectDisposedException) { }
							catch (InvalidAsynchronousStateException) { }
						}
						nextframe = now + wait;
					}
					Thread.Sleep(1);
				}
			}
            // リソース解放
            _backBuffer?.Dispose();
            _backBuffer = null;
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
