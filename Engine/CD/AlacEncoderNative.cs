using System;
using System.Runtime.InteropServices;

namespace MediaPlayer_X_Ark.Engine.CD;

public enum AlacEncoderResultCode
{
    Ok = 0,
    InvalidArgument = -1,
    Io = -2,
    State = -3,
    Encode = -4,
    Internal = -5
}

[StructLayout(LayoutKind.Sequential)]
public struct AlacEncoderCreateParams
{
    public int sample_rate;
    public int channels;
    public int bits_per_sample;
    public int frames_per_packet;
}

[StructLayout(LayoutKind.Sequential)]
public struct AlacEncoderStats
{
    public ulong total_pcm_bytes_received;
    public ulong total_pcm_frames_received;
    public ulong pending_pcm_bytes;
    public ulong encoded_frames;
    public ulong encoded_packet_count;
    public ulong encoded_payload_bytes;
    public int is_closed;
}

internal static class AlacEncoderNative
{
    private const string DllName = "AlacEncoder.dll";

    [DllImport(DllName, EntryPoint = "AlacMux_Open", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int AlacEncoder_Open(
        string output_path,
        in AlacEncoderCreateParams @params,
        out IntPtr out_handle);

    [DllImport(DllName, EntryPoint = "AlacMux_WritePcmBytes", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int AlacEncoder_WritePcmBytes(
        IntPtr handle,
        IntPtr pcm_bytes,
        int byte_count);

    [DllImport(DllName, EntryPoint = "AlacMux_Close", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int AlacEncoder_Close(IntPtr handle);

    [DllImport(DllName, EntryPoint = "AlacMux_Dispose", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void AlacEncoder_Dispose(IntPtr handle);

    [DllImport(DllName, EntryPoint = "AlacMux_GetLastError", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr AlacEncoder_GetLastError(IntPtr handle);

    [DllImport(DllName, EntryPoint = "AlacMux_GetStats", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int AlacEncoder_GetStats(IntPtr handle, out AlacEncoderStats out_stats);

    [DllImport(DllName, EntryPoint = "AlacMux_EncodePcmToFile", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int AlacEncoder_EncodePcmToFile(
        string output_path,
        in AlacEncoderCreateParams @params,
        IntPtr pcm_bytes,
        int byte_count);

    [DllImport(DllName, EntryPoint = "AlacMux_GetBuildId", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr AlacEncoder_GetBuildId();
}

public sealed class AlacEncoder : IDisposable
{
    private IntPtr _handle;
    private bool _closed;

    public AlacEncoder(string outputPath, AlacEncoderCreateParams createParams)
    {
        var rc = (AlacEncoderResultCode)AlacEncoderNative.AlacEncoder_Open(outputPath, in createParams, out _handle);
        if (rc != AlacEncoderResultCode.Ok)
        {
            throw new InvalidOperationException($"AlacEncoder_Open failed: {rc}");
        }
    }

    public void Write(byte[] pcmBytes)
    {
        if (_closed)
        {
            throw new ObjectDisposedException(nameof(AlacEncoder));
        }
        if (pcmBytes == null)
        {
            throw new ArgumentNullException(nameof(pcmBytes));
        }
        if (pcmBytes.Length == 0)
        {
            return;
        }

        var handle = GCHandle.Alloc(pcmBytes, GCHandleType.Pinned);
        try
        {
            var ptr = handle.AddrOfPinnedObject();
            var rc = (AlacEncoderResultCode)AlacEncoderNative.AlacEncoder_WritePcmBytes(_handle, ptr, pcmBytes.Length);
            if (rc != AlacEncoderResultCode.Ok)
            {
                throw new InvalidOperationException($"AlacEncoder_WritePcmBytes failed: {rc}, detail: {GetLastError()}");
            }
        }
        finally
        {
            handle.Free();
        }
    }

    public void Close()
    {
        if (_closed)
        {
            return;
        }

        var rc = (AlacEncoderResultCode)AlacEncoderNative.AlacEncoder_Close(_handle);
        if (rc != AlacEncoderResultCode.Ok)
        {
            throw new InvalidOperationException($"AlacEncoder_Close failed: {rc}");
        }
        _closed = true;
    }

    public string GetLastError()
    {
        if (_handle == IntPtr.Zero)
        {
            return "handle is null";
        }
        var p = AlacEncoderNative.AlacEncoder_GetLastError(_handle);
        return p == IntPtr.Zero ? "unknown" : Marshal.PtrToStringAnsi(p) ?? "unknown";
    }

    public AlacEncoderStats GetStats()
    {
        if (_handle == IntPtr.Zero)
        {
            throw new ObjectDisposedException(nameof(AlacEncoder));
        }

        var rc = (AlacEncoderResultCode)AlacEncoderNative.AlacEncoder_GetStats(_handle, out var stats);
        if (rc != AlacEncoderResultCode.Ok)
        {
            throw new InvalidOperationException($"AlacEncoder_GetStats failed: {rc}, detail: {GetLastError()}");
        }
        return stats;
    }

    public static string GetLoadedBuildId()
    {
        var p = AlacEncoderNative.AlacEncoder_GetBuildId();
        return p == IntPtr.Zero ? "unknown" : Marshal.PtrToStringAnsi(p) ?? "unknown";
    }

    public void Dispose()
    {
        if (_handle != IntPtr.Zero)
        {
            AlacEncoderNative.AlacEncoder_Dispose(_handle);
            _handle = IntPtr.Zero;
        }
        _closed = true;
        GC.SuppressFinalize(this);
    }

    ~AlacEncoder()
    {
        Dispose();
    }
}
