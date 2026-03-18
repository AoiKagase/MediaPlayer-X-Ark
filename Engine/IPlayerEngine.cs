using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MediaPlayer_X_Ark.Engine
{
    public interface IPlayerEngine : IDisposable
    {
        // ── 状態・プロパティ ──────────────────────────
        string lastError { get; }
        string lastErrFunction { get; }
        FMOD.RESULT lastErrCode { get; }

        BindingList<PlayList> PlayList { get; }
        List<DEVICE_INFO> DeviceList { get; }
        LOOP_MODE loop { get; set; }
        FmodSpectrum spectrum { get; }
        FmodWave wave { get; }
        Effector.Effectors effector { get; }

        // ── 初期化 ───────────────────────────────────
        void Initialize(CfgBuffer bufferSettings = null);

        // ── 再生制御 ─────────────────────────────────
        bool IsPlaying();
        FMOD.RESULT PlaySound(int index);
        void Stop();
        void Pause();

        // ── サウンド管理 ─────────────────────────────
        FMOD.RESULT CreateSound(string filename, out int index);
        uint GetLength(int index);
        uint GetPosition();
        void SetPosition(uint position);
        void GetTags(int index);
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
   	}
}