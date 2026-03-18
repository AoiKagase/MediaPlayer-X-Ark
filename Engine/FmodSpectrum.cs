using FMOD;
using System;
using System.Runtime.InteropServices;

namespace MediaPlayer_X_Ark
{
	public struct POS
	{
		public float X;
		public float Y;
	}

    public class FmodSpectrum : IDisposable
    {
        private bool _disposed = false;

        protected DSP mFFT;
		protected float[] mFFTSpectrum;
		protected FMOD.System FmodSystem;
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
			this.FmodChannelGroup = channelGroup;

			if (FmodSystem.createDSPByType(DSP_TYPE.FFT, out mFFT) == RESULT.OK)
			{
				mFFT.setParameterInt((int)DSP_FFT.WINDOW, (int)DSP_FFT_WINDOW_TYPE.BLACKMAN);
				mFFT.setParameterInt((int)DSP_FFT.WINDOWSIZE, windowSize * 2);

				this.lastError = Error.String(FmodChannelGroup.addDSP(CHANNELCONTROL_DSP_INDEX.HEAD, mFFT));

			}
		}
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (mFFT.hasHandle())
            {
                FmodChannelGroup.removeDSP(mFFT);
                mFFT.release();
            }

            _disposed = true;
        }

        ~FmodSpectrum()
        {
            Dispose(false);
        }

		private static float[] lineBottom = new float[128];
		public float[] UpdateSpectrum()
		{
			bool isPlaying;
			bool paused;

			FmodChannelGroup.isPlaying(out isPlaying);
			FmodChannelGroup.getPaused(out paused);

			if (!isPlaying || paused)
			{
				return mFFTSpectrum;
			}

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
					}
                }
            }
			return mFFTSpectrum;
		}
	}
}
