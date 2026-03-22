using NFluidsynth;
using System;
using System.Drawing.Drawing2D;
using System.IO;

namespace MediaPlayer_X_Ark.Engine.Player
{
	public class FluidSynthMidiRenderer : IDisposable
	{
		private bool _disposed = false;

		/// <summary>
		/// MIDIファイルをFluidSynthでPCMにレンダリングする
		/// </summary>
		/// <param name="midiPath">MIDIファイルパス</param>
		/// <param name="soundFontPath">SF2/DLSファイルパス</param>
		/// <param name="sampleRate">サンプルレート（デフォルト44100）</param>
		/// <returns>PCMバイト列（44100Hz / ステレオ / 16bit）</returns>
		public byte[] Render(string midiPath, string soundFontPath, int sampleRate = 44100)
		{
			var settings = new Settings();
			
			settings[ConfigurationKeys.SynthSampleRate].DoubleValue = sampleRate;
			settings[ConfigurationKeys.SynthAudioChannels].IntValue = 2;
			//settings[ConfigurationKeys.PlayerTimingSource].StringValue = "sample";
			//settings[ConfigurationKeys.SynthLockMemory].IntValue = 0;
			settings[ConfigurationKeys.SynthMidiBankSelect].StringValue = "gs";
			using (var synth = new Synth(settings))
			using (var player = new NFluidsynth.Player(synth))
			{
				synth.LoadSoundFont(soundFontPath, true);
				// ★GM System On リセット
				synth.SystemReset();

				// ★全チャンネルにGMデフォルト設定を適用
				for (int ch = 0; ch < 16; ch++)
				{
					synth.ProgramChange(ch, 0);  // プログラム0にリセット
					synth.CC(ch, 7, 100);        // Volume
					synth.CC(ch, 11, 127);       // Expression
					synth.CC(ch, 10, 64);        // Pan center
					synth.CC(ch, 91, 40);        // Reverb
					synth.CC(ch, 93, 0);         // Chorus
				}

				// ★Channel 10はドラムバンク（Bank 128）
				synth.ProgramChange(9, 0);
				player.Add(midiPath);
				player.Play();

				var ms = new MemoryStream();
				// ステレオ分離バッファを使う
				var leftBuf  = new float[sampleRate];
				var rightBuf = new float[sampleRate];

				while (player.Status == FluidPlayerStatus.Playing)
				{
					synth.WriteSampleFloat(
						sampleRate,
						leftBuf, 0, 1,
						rightBuf, 0, 1);

					for (int i = 0; i < sampleRate; i++)
					{
						// Left
						short l = (short)Math.Max(short.MinValue,
							Math.Min(short.MaxValue, (int)(leftBuf[i] * 32767)));
						ms.WriteByte((byte)(l & 0xFF));
						ms.WriteByte((byte)(l >> 8 & 0xFF));

						// Right
						short r = (short)Math.Max(short.MinValue,
							Math.Min(short.MaxValue, (int)(rightBuf[i] * 32767)));
						ms.WriteByte((byte)(r & 0xFF));
						ms.WriteByte((byte)(r >> 8 & 0xFF));
					}
				}

				return ms.ToArray();
			}
		}

		public void Dispose()
		{
			if (!_disposed)
				_disposed = true;
		}
	}
}