using FMOD;
using MediaPlayer_X_Ark.Engine.Player;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Visualize
{
	/// <summary>
	/// FMOD の OPENONLY モードを使って音声ファイルを解析し、
	/// 波形サマリー（RMS列）を生成する。
	/// 再生中のサウンドとは独立した別オブジェクトで処理するため再生に影響しない。
	/// </summary>
	public class WaveformAnalyzer
	{
		/// <summary>1曲あたりの波形サンプル数（シークバー幅の上限に合わせる）</summary>
		public const int SampleCount = 1000;

		private readonly FMOD.System _fmodSystem;
		private readonly object _fmodLock;

		public WaveformAnalyzer(FMOD.System system, object fmodLock)
		{
			_fmodSystem = system;
			_fmodLock = fmodLock;
		}

		/// <summary>
		/// 非同期で波形解析を実行する。
		/// 完了後に PlayList エントリへ結果をセットし onCompleted を呼ぶ。
		/// </summary>
		public Task AnalyzeAsync(
			string filename,
			PlayList entry,
			Action<PlayList> onCompleted,
			CancellationToken ct = default)
		{
			return Task.Run(() =>
			{
				try
				{
					Analyze(filename, entry, ct);
					if (!ct.IsCancellationRequested)
						onCompleted?.Invoke(entry);
				}
				catch (OperationCanceledException) { }
				catch { /* 解析失敗は無視（波形なし状態のまま） */ }
			}, ct);
		}

		private void Analyze(string filename, PlayList entry, CancellationToken ct)
		{
			FMOD.Sound sound = default;

			// ── FMOD でファイルを開く（再生しない） ──────────────────────
			var info = new FMOD.CREATESOUNDEXINFO();
			info.cbsize = Marshal.SizeOf(info);

			FMOD.RESULT result;
			lock (_fmodLock)
			{
				result = _fmodSystem.createSound(
					filename,
					FMOD.MODE.OPENONLY | FMOD.MODE.ACCURATETIME,
					ref info,
					out sound);
			}

			if (result != FMOD.RESULT.OK || !sound.hasHandle()) return;

			try
			{
				ct.ThrowIfCancellationRequested();

				// ── フォーマット情報を取得 ────────────────────────────────
				sound.getFormat(
					out FMOD.SOUND_TYPE _,
					out FMOD.SOUND_FORMAT __,
					out int channels,
					out int bits);

				sound.getLength(out uint lengthPcm, FMOD.TIMEUNIT.PCM);
				if (lengthPcm == 0 || channels == 0) return;

				// ── 1ブロックあたりのバイト数 ─────────────────────────────
				int bytesPerSample = Math.Max(bits / 8, 1);
				int blockFrames = (int)(lengthPcm / SampleCount);
				if (blockFrames <= 0) blockFrames = 1;
				int blockBytes = blockFrames * channels * bytesPerSample;

				// readData の最大バッファは 65536 に制限
				blockBytes = Math.Min(blockBytes, 65536);

				var buffer = new byte[blockBytes];
				var rmsL = new float[SampleCount];
				var rmsR = new float[SampleCount];

				// ── ブロックごとに RMS を計算 ─────────────────────────────
				for (int i = 0; i < SampleCount; i++)
				{
					ct.ThrowIfCancellationRequested();

					uint seekPcm = (uint)((long)i * lengthPcm / SampleCount);
					sound.seekData(seekPcm);

					sound.readData(buffer, out uint read);
					if (read == 0) break;

					int frames = (int)(read / (uint)(channels * bytesPerSample));
					double sumL = 0.0, sumR = 0.0;

					for (int f = 0; f < frames; f++)
					{
						int baseIdx = f * channels * bytesPerSample;
						if (baseIdx + bytesPerSample > (int)read) break;

						float sL = ReadSample(buffer, baseIdx, bits);
						sumL += sL * sL;

						if (channels >= 2)
						{
							float sR = ReadSample(buffer, baseIdx + bytesPerSample, bits);
							sumR += sR * sR;
						}
						else
						{
							sumR += sL * sL;
						}
					}

					rmsL[i] = frames > 0 ? (float)Math.Sqrt(sumL / frames) : 0f;
					rmsR[i] = frames > 0 ? (float)Math.Sqrt(sumR / frames) : 0f;
				}

				// ── 正規化（全体の最大値を 1.0 に揃える） ────────────────
				float maxVal = 0f;
				for (int i = 0; i < SampleCount; i++)
				{
					if (rmsL[i] > maxVal) maxVal = rmsL[i];
					if (rmsR[i] > maxVal) maxVal = rmsR[i];
				}

				if (maxVal > 0f)
				{
					for (int i = 0; i < SampleCount; i++)
					{
						rmsL[i] /= maxVal;
						rmsR[i] /= maxVal;
					}
				}

				entry.WaveformL = rmsL;
				entry.WaveformR = rmsR;

                // ── 末尾無音開始インデックスを計算（正規化前のRMSで判定）────────
                // 閾値：正規化前の最大値の 1% 未満を無音とみなす
                float silenceThreshold = maxVal * 0.01f;
                int audioEndIndex = SampleCount - 1;

                for (int i = SampleCount - 1; i >= 0; i--)
                {
                    if (rmsL[i] > silenceThreshold || rmsR[i] > silenceThreshold)
                    {
                        audioEndIndex = i;
                        break;
                    }
                }

                // ── 末尾高精度解析で AudioEndMs を確定 ────────────────────
                entry.AudioEndMs = AnalyzeTailPrecise(sound, entry.LengthMs, silenceThreshold);
            }
			finally
			{
				if (sound.hasHandle())
					sound.release();
			}
		}

		/// <summary>バイト配列から正規化サンプル値（-1.0〜1.0）を読み取る</summary>
		private static float ReadSample(byte[] buf, int offset, int bits)
		{
			if (offset + bits / 8 > buf.Length) return 0f;
			return bits switch
			{
				16 => BitConverter.ToInt16(buf, offset) / 32768f,
				24 => Read24bit(buf, offset) / 8388608f,
				32 => BitConverter.ToInt32(buf, offset) / 2147483648f,
				_ => 0f,
			};
		}

		private static int Read24bit(byte[] buf, int offset)
		{
			int value = buf[offset] | (buf[offset + 1] << 8) | (buf[offset + 2] << 16);
			if ((value & 0x800000) != 0) value |= unchecked((int)0xFF000000);
			return value;
		}

        /// <summary>
        /// 末尾5秒間を高密度（1ms精度）で再解析し、実音終了位置(ms)を返す。
        /// </summary>
        private int AnalyzeTailPrecise(
            FMOD.Sound sound, uint lengthMs, float silenceThreshold)
        {
            if (lengthMs == 0) return -1;

            // 末尾何秒を高精度解析するか
            const int tailSec = 3;
            uint tailStartMs = lengthMs > tailSec * 2000
                ? lengthMs - (uint)(tailSec * 2000)
                : 0;

            // フォーマット情報取得
            sound.getFormat(out _, out _, out int channels, out int bits);
            sound.getLength(out uint lengthPcm, FMOD.TIMEUNIT.PCM);
            if (lengthPcm == 0 || channels == 0) return -1;

            int bytesPerSample = Math.Max(bits / 8, 1);

            // 1ms あたりの PCM フレーム数（44100Hz ≒ 44フレーム/ms）
            // FMOD の getDefaults で周波数取得
            sound.getDefaults(out float freq, out _);
            int framesPerMs = Math.Max(1, (int)(freq / 1000f));

            int blockFrames = framesPerMs;  // 1msブロック
            int blockBytes = blockFrames * channels * bytesPerSample;
            blockBytes = Math.Min(blockBytes, 65536);
            var buffer = new byte[blockBytes];

            // 末尾から先頭方向に1msずつスキャン
            uint tailStartPcm = (uint)((long)tailStartMs * lengthPcm / lengthMs);

            int lastAudioMs = (int)tailStartMs;  // 初期値：解析範囲先頭

            int scanMs = (int)(lengthMs - tailStartMs);
            for (int i = 0; i < scanMs; i++)
            {
                uint seekPcm = tailStartPcm + (uint)(i * framesPerMs);
                if (seekPcm >= lengthPcm) break;

                sound.seekData(seekPcm);
                sound.readData(buffer, out uint read);
                if (read == 0) break;

                int frames = (int)(read / (uint)(channels * bytesPerSample));
                double sumL = 0.0, sumR = 0.0;

                for (int f = 0; f < frames; f++)
                {
                    int baseIdx = f * channels * bytesPerSample;
                    if (baseIdx + bytesPerSample > (int)read) break;

                    float sL = ReadSample(buffer, baseIdx, bits);
                    sumL += sL * sL;

                    if (channels >= 2)
                    {
                        float sR = ReadSample(buffer, baseIdx + bytesPerSample, bits);
                        sumR += sR * sR;
                    }
                    else
                    {
                        sumR += sL * sL;
                    }
                }

                float rmsL = frames > 0 ? (float)Math.Sqrt(sumL / frames) : 0f;
                float rmsR = frames > 0 ? (float)Math.Sqrt(sumR / frames) : 0f;

                if (rmsL > silenceThreshold || rmsR > silenceThreshold)
                    lastAudioMs = (int)tailStartMs + i;
            }

            return lastAudioMs;
        }
    }
}