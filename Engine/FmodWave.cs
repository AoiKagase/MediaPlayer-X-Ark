using FMOD;
using System;
using System.Runtime.InteropServices;

namespace MediaPlayer_X_Ark.Engine
{
	public class FmodWave
	{
		// メインスレッドと共有するバッファ
		private readonly object _bufferLock = new object();
		private float[] _waveBuffer;
		private int _channels;

		private FMOD.DSP _dsp;
		private FMOD.System _fmodSystem;
		private FMOD.ChannelGroup _fmodChannelGroup;

		// コールバックをGCに回収されないようフィールドで保持
		private FMOD.DSP_READ_CALLBACK _readCallback;

		public FmodWave(ref FMOD.System system, ref FMOD.ChannelGroup channelGroup)
		{
			_fmodSystem = system;
			_fmodChannelGroup = channelGroup;
			CreateDSP();
		}

		private void CreateDSP()
		{
			// コールバックをフィールドに保持（GC対策）
			_readCallback = WaveDataCallback;

			FMOD.DSP_DESCRIPTION desc = new FMOD.DSP_DESCRIPTION();
			desc.pluginsdkversion = 0x00010000;
			desc.numinputbuffers = 1;
			desc.numoutputbuffers = 1;
			desc.read = _readCallback;  // DSP_READCALLBACK

			if (_fmodSystem.createDSP(ref desc, out _dsp) == FMOD.RESULT.OK)
			{
				_fmodChannelGroup.addDSP(FMOD.CHANNELCONTROL_DSP_INDEX.HEAD, _dsp);
			}
		}

		/// <summary>
		/// DSPコールバック：FMODミキサースレッドから毎ブロック呼ばれる
		/// シグネチャは DSP_READCALLBACK に合わせる
		/// </summary>
		private FMOD.RESULT WaveDataCallback(
			ref FMOD.DSP_STATE dsp_state,
			IntPtr inbuffer,
			IntPtr outbuffer,
			uint length,
			int inchannels,
			ref int outchannels)
		{
			// 音声をそのままスルー出力
			int bytes = (int)(length * inchannels * sizeof(float));
			unsafe
			{
				Buffer.MemoryCopy(
					inbuffer.ToPointer(),
					outbuffer.ToPointer(),
					bytes, bytes);
			}

			// PCMデータをコピーしてメインスレッドへ渡す
			float[] temp = new float[length * inchannels];
			Marshal.Copy(inbuffer, temp, 0, temp.Length);

			lock (_bufferLock)
			{
				_waveBuffer = temp;
				_channels = inchannels;
			}

			return FMOD.RESULT.OK;
		}

		/// <summary>
		/// メインスレッドから呼ぶ。チャンネル0のWaveデータを返す。
		/// </summary>
		public float[] GetWaveData()
		{
			lock (_bufferLock)
			{
				if (_waveBuffer == null || _channels == 0)
					return null;

				// インターリーブされたPCMからチャンネル0だけ抽出
				float[] result = new float[_waveBuffer.Length / _channels];
				for (int i = 0; i < result.Length; i++)
					result[i] = _waveBuffer[i * _channels];

				return result;
			}
		}

		~FmodWave()
		{
			if (_dsp.hasHandle())
			{
				_fmodChannelGroup.removeDSP(_dsp);
				_dsp.release();
			}
		}
	}
}
