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

		public void UpdateSpectrum(Graphics g1, Graphics g2, float width, float height)
		{
			// BitBlt用にPictureBoxのHDCを取得
			IntPtr hdc1 = g1.GetHdc();
			IntPtr hdc2 = g2.GetHdc();
			Rectangle line1 = new Rectangle(0, 0, (int)width, (int)height);

//			float maxHeight = 0;
//			float lineHeight = 0;

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
							//for (int i = 0; i < fftData.numchannels; ++i)
							//{
							mFFTSpectrum = new float[fftData.length];
							//}
						}

						// channel = 0? 
						// スペクトラム値の取得
						fftData.getSpectrum(0, ref mFFTSpectrum);

						// 画像処理用の座標計算開始
						line1 = new Rectangle(0, 0, 0, 0);
						for (int i = 0; i < windowSize; ++i)
                        {
							float level = lin2dB(mFFTSpectrum[i]);
							line1 = new Rectangle(line1.Left+((int)width / windowSize), (int)((80 + level) * 0.1), 1, (int)height);

							BitBlt(hdc1, line1.Left, line1.Top, 1, line1.Bottom, hdc2, line1.Left, line1.Top, TernaryRasterOperations.SRCCOPY);
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
