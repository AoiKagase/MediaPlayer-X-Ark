using NFluidsynth;
using System;
using System.Diagnostics;
using System.IO;

namespace MediaPlayer_X_Ark.Engine.Player
{
	public class FluidSynthMidiRenderer : IDisposable
	{
		public enum MidiCompatibilityProfile
		{
			Auto,
			LegacyGsReset,
			PreserveMidiState,
		}

		private bool _disposed = false;

		public MidiCompatibilityProfile LastProfileUsed { get; private set; } =
			MidiCompatibilityProfile.Auto;

		/// <summary>
		/// MIDIファイルをFluidSynthでPCMにレンダリングする
		/// </summary>
		/// <param name="midiPath">MIDIファイルパス</param>
		/// <param name="soundFontPath">SF2/DLSファイルパス</param>
		/// <param name="sampleRate">サンプルレート（デフォルト44100）</param>
		/// <param name="profile">互換モード。Auto の場合は MIDI 内容から自動判定する。</param>
		/// <returns>PCMバイト列（44100Hz / ステレオ / 16bit）</returns>
		public byte[] Render(
			string midiPath,
			string soundFontPath,
			int sampleRate = 44100,
			MidiCompatibilityProfile profile = MidiCompatibilityProfile.Auto)
		{
			var resolvedProfile = ResolveProfile(midiPath, profile);
			LastProfileUsed = resolvedProfile;

			var settings = new Settings();
			settings[ConfigurationKeys.SynthSampleRate].DoubleValue = sampleRate;
			settings[ConfigurationKeys.SynthAudioChannels].IntValue = 2;

			if (resolvedProfile == MidiCompatibilityProfile.LegacyGsReset)
				settings[ConfigurationKeys.SynthMidiBankSelect].StringValue = "gs";

			Debug.WriteLine(
				$"[FluidSynth] Render start profile={resolvedProfile} midi=\"{midiPath}\" sf2=\"{soundFontPath}\" sampleRate={sampleRate}");

			using (var synth = new Synth(settings))
			using (var player = new NFluidsynth.Player(synth))
			{
				synth.LoadSoundFont(soundFontPath, true);

				if (resolvedProfile == MidiCompatibilityProfile.LegacyGsReset)
					ApplyLegacyGsReset(synth);

				player.Add(midiPath);
				player.Play();

				var ms = new MemoryStream();
				var leftBuf = new float[sampleRate];
				var rightBuf = new float[sampleRate];

				while (player.Status == FluidPlayerStatus.Playing)
				{
					synth.WriteSampleFloat(
						sampleRate,
						leftBuf, 0, 1,
						rightBuf, 0, 1);

					for (int i = 0; i < sampleRate; i++)
					{
						short l = (short)Math.Max(short.MinValue,
							Math.Min(short.MaxValue, (int)(leftBuf[i] * 32767)));
						ms.WriteByte((byte)(l & 0xFF));
						ms.WriteByte((byte)((l >> 8) & 0xFF));

						short r = (short)Math.Max(short.MinValue,
							Math.Min(short.MaxValue, (int)(rightBuf[i] * 32767)));
						ms.WriteByte((byte)(r & 0xFF));
						ms.WriteByte((byte)((r >> 8) & 0xFF));
					}
				}

				return ms.ToArray();
			}
		}

		private static void ApplyLegacyGsReset(Synth synth)
		{
			synth.SystemReset();

			for (int ch = 0; ch < 16; ch++)
			{
				synth.ProgramChange(ch, 0);
				synth.CC(ch, 7, 100);
				synth.CC(ch, 11, 127);
				synth.CC(ch, 10, 64);
				synth.CC(ch, 91, 40);
				synth.CC(ch, 93, 0);
			}

			synth.ProgramChange(9, 0);
		}

		private static MidiCompatibilityProfile ResolveProfile(
			string midiPath,
			MidiCompatibilityProfile requestedProfile)
		{
			if (requestedProfile != MidiCompatibilityProfile.Auto)
				return requestedProfile;

			var forcedProfile = TryReadForcedProfile();
			if (forcedProfile.HasValue)
			{
				Debug.WriteLine(
					$"[FluidSynth] Forced profile={forcedProfile.Value} midi=\"{midiPath}\" source=\"XARK_FLUIDSYNTH_PROFILE\"");
				return forcedProfile.Value;
			}

			try
			{
				var analysis = AnalyzeMidiForCompatibility(midiPath);
				if (analysis.RequiresPreserveMidiState)
				{
					Debug.WriteLine(
						$"[FluidSynth] Auto profile=PreserveMidiState midi=\"{midiPath}\" reason=\"{analysis.Reason}\"");
					return MidiCompatibilityProfile.PreserveMidiState;
				}

				Debug.WriteLine(
					$"[FluidSynth] Auto profile=LegacyGsReset midi=\"{midiPath}\" reason=\"{analysis.Reason}\"");
			}
			catch (Exception ex)
			{
				Debug.WriteLine(
					$"[FluidSynth] Auto profile analysis failed midi=\"{midiPath}\" error=\"{ex.Message}\"");
			}

			return MidiCompatibilityProfile.LegacyGsReset;
		}

		private static MidiCompatibilityProfile? TryReadForcedProfile()
		{
			var raw = Environment.GetEnvironmentVariable("XARK_FLUIDSYNTH_PROFILE");
			if (string.IsNullOrWhiteSpace(raw))
				return null;

			if (Enum.TryParse(raw.Trim(), true, out MidiCompatibilityProfile profile))
				return profile;

			Debug.WriteLine(
				$"[FluidSynth] Ignoring invalid XARK_FLUIDSYNTH_PROFILE value=\"{raw}\"");
			return null;
		}

		private static MidiCompatibilityAnalysis AnalyzeMidiForCompatibility(string midiPath)
		{
			var bytes = File.ReadAllBytes(midiPath);
			if (bytes.Length < 14)
				return MidiCompatibilityAnalysis.Legacy("header_too_small");

			int pos = 14;
			int tracks = ReadBigEndian16(bytes, 10);
			bool foundSysEx = false;
			bool foundNrpnOrRpn = false;

			for (int track = 0; track < tracks && pos + 8 <= bytes.Length; track++)
			{
				if (!IsChunk(bytes, pos, "MTrk"))
					break;

				int chunkLength = ReadBigEndian32(bytes, pos + 4);
				pos += 8;
				int end = Math.Min(pos + chunkLength, bytes.Length);
				int runningStatus = -1;

				while (pos < end)
				{
					ReadVariableLength(bytes, ref pos, end);
					if (pos >= end)
						break;

					int status = bytes[pos];
					if (status >= 0x80)
					{
						pos++;
						if (status < 0xF0)
							runningStatus = status;
					}
					else
					{
						status = runningStatus;
					}

					if (status < 0)
						break;

					if (status == 0xF0 || status == 0xF7)
					{
						foundSysEx = true;
						int len = ReadVariableLength(bytes, ref pos, end);
						pos = Math.Min(pos + len, end);
						continue;
					}

					if (status == 0xFF)
					{
						if (pos >= end)
							break;

						pos++;
						int len = ReadVariableLength(bytes, ref pos, end);
						pos = Math.Min(pos + len, end);
						continue;
					}

					int eventType = status & 0xF0;
					switch (eventType)
					{
						case 0x80:
						case 0x90:
						case 0xA0:
						case 0xE0:
							pos = Math.Min(pos + 2, end);
							break;
						case 0xB0:
							if (pos + 1 >= end)
							{
								pos = end;
								break;
							}

							int controller = bytes[pos];
							pos += 2;
							if (controller == 98 || controller == 99 || controller == 100 || controller == 101)
								foundNrpnOrRpn = true;
							break;
						case 0xC0:
						case 0xD0:
							pos = Math.Min(pos + 1, end);
							break;
						default:
							pos = end;
							break;
					}
				}
			}

			if (foundSysEx || foundNrpnOrRpn)
			{
				string reason = foundSysEx && foundNrpnOrRpn
					? "sysex_and_nrpn_or_rpn"
					: foundSysEx ? "sysex" : "nrpn_or_rpn";
				return MidiCompatibilityAnalysis.Preserve(reason);
			}

			return MidiCompatibilityAnalysis.Legacy("plain_gm_like_midi");
		}

		private static bool IsChunk(byte[] bytes, int offset, string chunkId)
		{
			return offset + 4 <= bytes.Length
				&& bytes[offset] == chunkId[0]
				&& bytes[offset + 1] == chunkId[1]
				&& bytes[offset + 2] == chunkId[2]
				&& bytes[offset + 3] == chunkId[3];
		}

		private static int ReadBigEndian16(byte[] bytes, int offset)
		{
			return (bytes[offset] << 8) | bytes[offset + 1];
		}

		private static int ReadBigEndian32(byte[] bytes, int offset)
		{
			return (bytes[offset] << 24)
				| (bytes[offset + 1] << 16)
				| (bytes[offset + 2] << 8)
				| bytes[offset + 3];
		}

		private static int ReadVariableLength(byte[] bytes, ref int pos, int end)
		{
			int value = 0;
			while (pos < end)
			{
				int current = bytes[pos++];
				value = (value << 7) | (current & 0x7F);
				if ((current & 0x80) == 0)
					break;
			}

			return value;
		}

		private sealed class MidiCompatibilityAnalysis
		{
			public bool RequiresPreserveMidiState { get; }
			public string Reason { get; }

			private MidiCompatibilityAnalysis(bool requiresPreserveMidiState, string reason)
			{
				RequiresPreserveMidiState = requiresPreserveMidiState;
				Reason = reason;
			}

			public static MidiCompatibilityAnalysis Preserve(string reason)
			{
				return new MidiCompatibilityAnalysis(true, reason);
			}

			public static MidiCompatibilityAnalysis Legacy(string reason)
			{
				return new MidiCompatibilityAnalysis(false, reason);
			}
		}

		public void Dispose()
		{
			if (!_disposed)
				_disposed = true;
		}
	}
}
