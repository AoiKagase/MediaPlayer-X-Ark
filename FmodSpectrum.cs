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

    class FmodSpectrum
	{
		protected DSP mFFT;
		protected float[] mFFTSpectrum;
		protected FMOD.System FmodSystem;
		protected int windowSize;
		protected ChannelGroup FmodChannelGroup;
		protected string lastError;

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
        }
		enum TernaryRasterOperations : uint
		{
			SRCCOPY = 0x00CC0020,
			SRCPAINT = 0x00EE0086,
			SRCAND = 0x008800C6,
			SRCINVERT = 0x00660046,
			SRCERASE = 0x00440328,
			NOTSRCCOPY = 0x00330008,
			NOTSRCERASE = 0x001100A6,
			MERGECOPY = 0x00C000CA,
			MERGEPAINT = 0x00BB0226,
			PATCOPY = 0x00F00021,
			PATPAINT = 0x00FB0A09,
			PATINVERT = 0x005A0049,
			DSTINVERT = 0x00550009,
			BLACKNESS = 0x00000042,
			WHITENESS = 0x00FF0062,
			CAPTUREBLT = 0x40000000
		}
		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight,
		  IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

		private static float[] lineBottom = new float[128];

		public void UpdateSpectrum(Graphics g1, Graphics g2, int width, int height, int mode)
		{
			// BitBlt用にPictureBoxのHDCを取得
			Rectangle line1;

			int lineHeight = 0;
			bool isPlaying;

			FmodChannelGroup.isPlaying(out isPlaying);

			if (!isPlaying)
			{
				g1.Clear(Color.White);
				return;
			}

			IntPtr hdc1 = g1.GetHdc();
			IntPtr hdc2 = g2.GetHdc();
			// DSPを作成済み
			if (mFFT.hasHandle())
            {
				IntPtr unmanagedData;
				uint length;

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


						int step = (mode > 0) ? mode * 2 : 1;
						// 画像処理用の座標計算開始
						line1 = new Rectangle(0, 0, 0, 0);
						for (int i = 0; i < windowSize; i+=step)
                        {
							lineHeight = height - (int)(lin2dB(mFFTSpectrum[i]) + 80) * 2;

							line1 = new Rectangle(i + (width / windowSize), lineHeight, (windowSize / width), height - lineHeight);
//							g1.FillRectangle(Brushes.Red, line1.X, line1.Y, line1.Width, line1.Height);

							BitBlt(hdc1, line1.X, line1.Y, line1.Width, line1.Height, hdc2, line1.X, line1.Y, TernaryRasterOperations.SRCCOPY);
						}
					}
                }
            }
			
			g1.ReleaseHdc(hdc1);
			g2.ReleaseHdc(hdc2);
			return;
		}

		private float lin2dB(float linear)
		{
			return Math.Clamp((float)Math.Log10(linear) * 20.0f, -80.0f, 0.0f);
		}
	}
}
