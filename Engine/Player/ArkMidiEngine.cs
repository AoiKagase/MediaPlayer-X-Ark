using System;
using System.IO;
using System.Runtime.InteropServices;

public static class ArkMidiEngine
{
    private const string DllName = "ArkMidiEngine.dll";

    public enum AmeResult : int
    {
        OK = 0,
        InvalidArg = -1,
        ParseMidi = -2,
        ParseSf2 = -3,
        OutOfMemory = -4,
        NotInitialized = -5,
        ParseDls = -6,
        Unsupported = -7,
        Io = -8,
    }

    public enum SoundBankKind : uint
    {
        Auto = 0,
        Sf2 = 1,
        Dls = 2,
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    private static extern AmeResult AmeCreateEngineFromPaths(
        string midiPath,
        string soundBankPath,
        SoundBankKind soundBankKind,
        uint sampleRate,
        uint numChannels,
        out IntPtr outEngine);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern AmeResult AmeRender(
        IntPtr engine,
        short[] outBuffer,
        uint numFrames,
        out uint outWritten);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int AmeIsFinished(IntPtr engine);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern void AmeDestroyEngine(IntPtr engine);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr AmeGetVersion();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr AmeGetLastError();

    public static string GetVersion()
        => Marshal.PtrToStringAnsi(AmeGetVersion()) ?? string.Empty;

    public static string GetLastError()
        => Marshal.PtrToStringAnsi(AmeGetLastError()) ?? string.Empty;

    public sealed class Engine : IDisposable
    {
        private IntPtr _handle;
        private bool _disposed;
        private readonly uint _sampleRate;
        private readonly uint _numChannels;

        private const int DefaultChunkFrames = 4096;
        private const int DefaultMaxPcmBytes = 512 * 1024 * 1024;
        private const int DefaultMaxRenderSeconds = 60 * 60;

        public Engine(string midiPath, string soundBankPath,
                      SoundBankKind soundBankKind = SoundBankKind.Auto,
                      uint sampleRate = 44100, uint numChannels = 2)
        {
            if (string.IsNullOrWhiteSpace(midiPath))
                throw new ArgumentException("midiPath is null or empty", nameof(midiPath));
            if (string.IsNullOrWhiteSpace(soundBankPath))
                throw new ArgumentException("soundBankPath is null or empty", nameof(soundBankPath));
            if (numChannels < 1 || numChannels > 2)
                throw new ArgumentOutOfRangeException(nameof(numChannels), "Must be 1 or 2");

            var result = AmeCreateEngineFromPaths(
                midiPath,
                soundBankPath,
                soundBankKind,
                sampleRate,
                numChannels,
                out _handle);

            if (result != AmeResult.OK)
                throw new ArkMidiException(result, GetLastError());

            _sampleRate = sampleRate;
            _numChannels = numChannels;
        }

        public uint Render(short[] buffer, uint numFrames)
        {
            ThrowIfDisposed();
            var requiredSamples = checked((int)(numFrames * _numChannels));
            if (buffer == null || buffer.Length < requiredSamples)
                throw new ArgumentException("buffer is smaller than required", nameof(buffer));

            var result = AmeRender(_handle, buffer, numFrames, out uint written);
            if (result != AmeResult.OK)
                throw new ArkMidiException(result, GetLastError());
            return written;
        }

        public bool IsFinished
        {
            get
            {
                if (_disposed) return true;
                return AmeIsFinished(_handle) != 0;
            }
        }

        public short[] RenderAll(uint chunkFrames = DefaultChunkFrames)
        {
            var pcm = RenderAllPcm16(chunkFrames);
            var result = new short[pcm.Length / sizeof(short)];
            Buffer.BlockCopy(pcm, 0, result, 0, pcm.Length);
            return result;
        }

        public byte[] RenderAllPcm16(
            uint chunkFrames = DefaultChunkFrames,
            int maxPcmBytes = DefaultMaxPcmBytes,
            int maxRenderSeconds = DefaultMaxRenderSeconds)
        {
            ThrowIfDisposed();
            if (chunkFrames == 0)
                throw new ArgumentOutOfRangeException(nameof(chunkFrames), "Must be greater than 0");
            if (maxPcmBytes <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxPcmBytes), "Must be greater than 0");
            if (maxRenderSeconds <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxRenderSeconds), "Must be greater than 0");

            ulong maxFramesByTime = (ulong)_sampleRate * (ulong)maxRenderSeconds;
            ulong maxFramesByBytes = (ulong)maxPcmBytes / ((ulong)_numChannels * sizeof(short));
            ulong maxFrames = Math.Min(maxFramesByTime, maxFramesByBytes);
            if (maxFrames == 0)
                throw new ArkMidiException(AmeResult.InvalidArg, "Render safety limit is too small.");

            int samplesPerChunk = checked((int)(chunkFrames * _numChannels));
            var sampleBuffer = new short[samplesPerChunk];
            var byteBuffer = new byte[samplesPerChunk * sizeof(short)];
            using var stream = new MemoryStream(Math.Min(maxPcmBytes, byteBuffer.Length * 4));
            ulong totalFrames = 0;

            while (!IsFinished)
            {
                uint written = Render(sampleBuffer, chunkFrames);
                if (written == 0) break;

                totalFrames += written;
                if (totalFrames > maxFrames)
                {
                    double renderedSeconds = (double)totalFrames / _sampleRate;
                    throw new ArkMidiException(
                        AmeResult.OutOfMemory,
                        $"Rendered PCM exceeded safety limit ({renderedSeconds:F1}s, {stream.Length / 1024 / 1024} MiB).");
                }

                int samplesWritten = checked((int)(written * _numChannels));
                int bytesWritten = checked(samplesWritten * sizeof(short));
                Buffer.BlockCopy(sampleBuffer, 0, byteBuffer, 0, bytesWritten);
                stream.Write(byteBuffer, 0, bytesWritten);
            }

            return stream.ToArray();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_handle != IntPtr.Zero)
                {
                    AmeDestroyEngine(_handle);
                    _handle = IntPtr.Zero;
                }
                _disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Engine));
        }
    }

    public class ArkMidiException : Exception
    {
        public AmeResult ErrorCode { get; }

        public ArkMidiException(AmeResult code, string message)
            : base($"[{code}] {message}")
        {
            ErrorCode = code;
        }
    }
}
