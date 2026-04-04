using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Visualize;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace MediaPlayer_X_Ark.Engine.Player
{
    public enum MidiRendererBackend
    {
        Auto = 0,
        FluidSynth = 1,
        BassMidi = 2,
        ArkMidi = 3,
    }

    public interface IPlayerEngine : IDisposable
    {
        // ── 状態・プロパティ ──────────────────────────
        IReadOnlyList<PluginLoadResult> LoadedPlugins { get; }
        event EventHandler<PlayerErrorEventArgs> ErrorOccurred;
        bool FluidSynthAvailable { get; }
        bool BassMidiAvailable { get; }
        bool ArkMidiAvailable { get; }
		string SoundFontPath { get; set; }
        MidiRendererBackend MidiRendererBackend { get; set; }
		BindingList<PlayList> PlayList { get; }
        List<DEVICE_INFO> DeviceList { get; }
        LOOP_MODE loop { get; set; }
        FmodSpectrum spectrum { get; }
        FmodWave wave { get; }
        Effector.Effectors effector { get; }
        int PlayingIndex { get; }
        bool NowPlaying { get; }
		/// <summary>クロスフェード有効フラグ</summary>
		bool CrossfadeEnabled { get; set; }

		/// <summary>クロスフェード時間（ミリ秒）</summary>
		int CrossfadeDurationMs { get; set; }

		/// <summary>
		/// 残り時間検知によるフェード開始済みフラグ。
		/// PlayNext() 実行後に true になり、PlaySound() 内でリセットされる。
		/// </summary>
		bool CrossfadeTriggered { get; set; }
        /// <summary>NonStopMix有効フラグ（クロスフェードと排他）</summary>
        bool NonStopMixEnabled { get; set; }
        /// <summary>ReplayGain有効フラグ</summary>
        bool ReplayGainEnabled { get; set; }

		/// <summary>ReplayGainモード（0=トラック, 1=アルバム）</summary>
		int ReplayGainMode { get; set; }

		/// <summary>プリアンプゲイン（dB）</summary>
		float ReplayGainPreamp { get; set; }
		/// <summary>波形解析・表示を有効にするか（スキンに Waveform セクションがある場合のみ true）</summary>
		bool WaveformEnabled { get; set; }
        int ChannelCount { get; }
		/// <summary>
		/// 波形解析完了時に発火するイベント。
		/// 引数は PlayList インデックス。UIスレッドへの Invoke が必要。
		/// </summary>
		event Action<int> WaveformReady;
		// ── 初期化 ───────────────────────────────────
		void Initialize(CfgBuffer bufferSettings = null);

        // ── 再生制御 ─────────────────────────────────
        bool IsPlaying();
        FMOD.RESULT PlaySound(int index);
		FMOD.RESULT PlaySoundPaused(int index, uint position = 0);
		FMOD.RESULT PlayUrl(string url);
        void Stop();
        void SwitchPause();
		void PlayNext(int fromIndex = -1, bool manual = false);
		void PlayPrevious(int fromIndex = -1, bool manual = false);
		void ReleaseNonStopFadingIfDone();
		void BuildShuffleQueue();
        void UpdateShuffleQueueOnRemove(int removedIndex);

		/// <summary>
		/// クロスフェードの音量を進行させる。
		/// MainForm の PlayerTimer_Tick から毎フレーム呼ぶ。
		/// </summary>
		/// <param name="elapsedMs">前回呼び出しからの経過時間（ms）</param>
		void UpdateCrossfade(int elapsedMs);
		// ── サウンド管理 ─────────────────────────────
		FMOD.RESULT CreateSound(string filename, out int index);
        uint GetLength(int index);
        uint GetPosition();
        void SetPosition(uint position);
        Bitmap GetCoverArt(int index);
        FMOD.OPENSTATE GetOpenState(int index,
                           out uint buffered,
                           out bool starving,
                           out bool diskBusy);
        FMOD.RESULT CreateSoundFromPCM(byte[] pcmData, string title, out int index);

        void ClearPlayList();
        // ── 音量・パン ───────────────────────────────
        void SetVolume(float vol);
        int GetVolume();
        void SetPan(float pan);

        // ── 出力設定 ─────────────────────────────────
        void SetOutputTypeBeforeInit(FMOD.OUTPUTTYPE outputtype);
        void SetOutputType(FMOD.OUTPUTTYPE outputtype);
        FMOD.OUTPUTTYPE GetOutputType();

        // ── デバイス ─────────────────────────────────
        void GetDeviceList();
        int GetDevice();
        string GetDeviceGUID();
        void SetDevice(int driver);
        void SetDevice(string driver);
        List<DEVICE_INFO> GetDeviceListForOutputType(FMOD.OUTPUTTYPE outputType);
        List<DEVICE_INFO> GetCurrentDeviceList();

        void Sort<T>(Func<PlayList, T> keySelector);
        void LoadPlugins();
    }
}
