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
		protected ChannelGroup cgroup;
		protected string lastError;

		private POS position;
        private int v;
        private ChannelGroup fmodChannelGroup;

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
			this.cgroup = channelGroup;

			if (FmodSystem.createDSPByType(DSP_TYPE.FFT, out mFFT) == RESULT.OK)
			{
				mFFT.setParameterInt((int)DSP_FFT.WINDOWTYPE, (int)DSP_FFT_WINDOW.HANNING);
				mFFT.setParameterInt((int)DSP_FFT.WINDOWSIZE, windowSize * 2);

				this.lastError = Error.String(cgroup.addDSP(CHANNELCONTROL_DSP_INDEX.HEAD, mFFT));
			}
		}

		~FmodSpectrum()
        {
			if (mFFT.hasHandle())
				cgroup.removeDSP(mFFT);

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

		public POS UpdateSpectrum(IntPtr hdc1, IntPtr hdc2, float width, float height)
		{
			Rectangle line1 = new Rectangle(0, 0, (int)width, (int)height);
			Rectangle line2;
			float maxHeight = 0;
			float lineHeight = 0;

			if (mFFT.hasHandle())
            {
				IntPtr unmanagedData;
				uint length;

				if (mFFT.getParameterData((int) DSP_FFT.SPECTRUMDATA, out unmanagedData, out length) == RESULT.OK)
                {
					DSP_PARAMETER_FFT fftData = (DSP_PARAMETER_FFT)Marshal.PtrToStructure(unmanagedData, typeof(DSP_PARAMETER_FFT));
					if (fftData.numchannels > 0)
                    {
						if (mFFTSpectrum == null)
                        {
							// Allocate the fft spectrum buffer once
							for (int i = 0; i < fftData.numchannels; ++i)
							{
								mFFTSpectrum = new float[fftData.length];
							}
						}
						fftData.getSpectrum(0, ref mFFTSpectrum);
						position.X = width * -0.5f;
						for (int i = 0; i < windowSize; ++i)
                        {
							float level = lin2dB(mFFTSpectrum[i]);
							line1 = new Rectangle(line1.Left+(int)width / windowSize, (int)((80 + level) * height), 1, (int)height);

							BitBlt(hdc1, line1.Left, line1.Top, 1, line1.Bottom, hdc2, line1.Left, line1.Top, TernaryRasterOperations.SRCCOPY);

						}
					}
                }
            }
			return this.position;
		}

		private float lin2dB(float linear)
		{
			return Math.Clamp((float)Math.Log10(linear) * 20.0f, -80.0f, 0.0f);
		}
	}
}
