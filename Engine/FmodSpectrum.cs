using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using FMOD;
using System.Drawing;

namespace MediaPlayer_X_Ark
{
	public struct POS
	{
		public float X;
		public float Y;
	}

    public class FmodSpectrum
	{
		protected DSP mFFT;
		protected float[] mFFTSpectrum;
		protected FMOD.System FmodSystem;
		protected int windowSize;
		protected ChannelGroup FmodChannelGroup;
		protected string lastError;

		protected Graphics gAnalyzer;
		protected Graphics gBuffer;
		protected Graphics gSnow;

		protected IntPtr hdc1Analyzer;
		protected IntPtr hdc1Buffer;
		protected IntPtr hdc2AnalyzerSrc;
		protected IntPtr hdc3AnalyzerSnow;

		protected IntPtr Prog1;
		protected IntPtr Prog2;

		protected int[] analyzerSnow;

		public void Initialize(Graphics g1, Bitmap src)
        {
			gAnalyzer = g1;
			hdc1Analyzer = g1.GetHdc();
			hdc2AnalyzerSrc = Win32API.CreateCompatibleDC(hdc1Analyzer);
			Prog1 = Win32API.SelectObject(hdc2AnalyzerSrc, src.GetHbitmap(Color.Black));
			analyzerSnow = new int[windowSize];

			Bitmap snow = new Bitmap(src.Width, src.Height);
			Bitmap buff = new Bitmap(src.Width, src.Height);
			gSnow = Graphics.FromImage(snow);
			gBuffer = Graphics.FromImage(buff);
			using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
			{
				gSnow.FillRectangle(brush, 0, 0, src.Width, src.Height);
			}
			hdc3AnalyzerSnow = gSnow.GetHdc();
			hdc1Buffer = gBuffer.GetHdc();
			Prog2 = Win32API.SelectObject(hdc3AnalyzerSnow, snow.GetHbitmap(Color.White));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="system"></param>
		/// <param name="windowSize"></param>
		/// <param name="channelGroup"></param>
		public FmodSpectrum(ref FMOD.System system, int windowSize, ref ChannelGroup channelGroup)
		{
			this.FmodSystem = system;
			this.windowSize = windowSize;
			this.FmodChannelGroup = channelGroup;

			if (FmodSystem.createDSPByType(DSP_TYPE.FFT, out mFFT) == RESULT.OK)
			{
				mFFT.setParameterInt((int)DSP_FFT.WINDOWTYPE, (int)DSP_FFT_WINDOW.HANNING);
				mFFT.setParameterInt((int)DSP_FFT.WINDOWSIZE, windowSize * 2);

				this.lastError = Error.String(FmodChannelGroup.addDSP(CHANNELCONTROL_DSP_INDEX.HEAD, mFFT));
			}
		}

		~FmodSpectrum()
        {
			if (mFFT.hasHandle())
				FmodChannelGroup.removeDSP(mFFT);

			mFFT.release();

			Win32API.DeleteObject(Prog2);
			Win32API.DeleteObject(Prog1);
			Win32API.DeleteDC(hdc3AnalyzerSnow);
			Win32API.DeleteDC(hdc1Buffer);
			Win32API.DeleteDC(hdc2AnalyzerSrc);
			Win32API.DeleteDC(hdc1Analyzer);
			gBuffer.ReleaseHdc(hdc1Buffer);
			gSnow.ReleaseHdc(hdc3AnalyzerSnow);
			gAnalyzer.ReleaseHdc(hdc1Analyzer);

		}


		private static float[] lineBottom = new float[128];
		public void UpdateSpectrum(Graphics g1,ref Bitmap g2, int width, int height, int mode)
		{
			// BitBlt用にPictureBoxのHDCを取得
			RECT line2 = new RECT(0, 0, width, height);	// BackGround

			int lineHeight = 0;
			bool isPlaying;
			bool paused;

			FmodChannelGroup.isPlaying(out isPlaying);
			FmodChannelGroup.getPaused(out paused);

			if (!isPlaying || paused)
			{
				Win32API.FillRect(hdc1Buffer, ref line2, Win32API.CreateSolidBrush(0xffffff00));
				Win32API.BitBlt(hdc1Analyzer, 0, 0, width, height, hdc1Buffer, 0, 0, Win32API.TernaryRasterOperations.SRCCOPY);

				return;
			}

			// DSPを作成済み
			if (mFFT.hasHandle())
            {
				IntPtr unmanagedData;
				uint length;

				Win32API.FillRect(hdc1Buffer, ref line2, Win32API.CreateSolidBrush(0xffffff00));
				// スペクトラムデータの取得（RAW）
				if (mFFT.getParameterData((int) DSP_FFT.SPECTRUMDATA, out unmanagedData, out length) == RESULT.OK)
                {
					// スペクトラムデータをFFT構造体へ
					DSP_PARAMETER_FFT fftData = (DSP_PARAMETER_FFT)Marshal.PtrToStructure(unmanagedData, typeof(DSP_PARAMETER_FFT));

					// チャンネル数が１以上
					if (fftData.numchannels > 0)
                    {
						// スペクトラム値の初期化
						if (mFFTSpectrum == null)
                        {
							// Allocate the fft spectrum buffer once
							mFFTSpectrum = new float[fftData.length];
						}

						// channel = 0? 
						// スペクトラム値の取得
						fftData.getSpectrum(0, ref mFFTSpectrum);
						RECT line1 = new RECT();
						RECT line3 = new RECT(0, 0, 0, 0);  // Snow


						int step = (mode > 0) ? mode * 2 : 1;
						// 画像処理用の座標計算開始
						line1 = new Rectangle(0, 0, 0, 0);
						for (int i = 0; i < windowSize; i+=step)
                        {
							lineHeight = height - (int)((lin2dB(mFFTSpectrum[i]) + 80) * 0.8);

							line3.Left = i;
							if (width > windowSize)
								line3.Right = i + (width / windowSize) + (mode / 2);
							else
								line3.Right = i + 1 + mode / 2;

							if (analyzerSnow[i] > lineHeight)
								line3.Bottom = analyzerSnow[i] = lineHeight;
							else if (analyzerSnow[i] < height)
								line3.Bottom = analyzerSnow[i]++;

							line3.Top = line3.Bottom - 1;

							Win32API.BitBlt(hdc1Buffer, line3.Left, line3.Top, line3.Right - line3.Left, 1, hdc3AnalyzerSnow, line3.Left, line3.Top, Win32API.TernaryRasterOperations.SRCCOPY);

							line1.Bottom = height;
							line1.Top = lineHeight;
							line1.Left = line3.Left;
							line1.Right = line3.Right;

							Win32API.BitBlt(hdc1Buffer, line1.Left, line1.Top, line1.Right - line1.Left, line1.Bottom - line1.Top, hdc2AnalyzerSrc, line1.Left, line1.Top, Win32API.TernaryRasterOperations.SRCCOPY);
						}
					}
                }
            }
			Win32API.BitBlt(hdc1Analyzer, 0, 0, width, height, hdc1Buffer, 0, 0, Win32API.TernaryRasterOperations.SRCCOPY);
			return;
		}

		private float lin2dB(float linear)
		{
			return Math.Clamp((float)Math.Log10(linear) * 20.0f, -80.0f, 0.0f);
		}
	}
}
