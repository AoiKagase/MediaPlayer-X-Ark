using System;
using System.Runtime.InteropServices;

namespace MediaPlayer_X_Ark.Engine.CD;

public enum FlacEncoderResultCode
{
    Ok = 0,
    InvalidArgument = -1,
    Io = -2,
    State = -3,
    Encode = -4,
    Internal = -5
}

[StructLayout(LayoutKind.Sequential)]
public struct FlacEncoderCreateParams
{
    public int sample_rate;
    public int channels;
    public int bits_per_sample;
    public int frames_per_packet;
}

[StructLayout(LayoutKind.Sequential)]
public struct FlacEncoderStats
{
    public ulong total_pcm_bytes_received;
    public ulong total_pcm_frames_received;
    public ulong pending_pcm_bytes;
    public ulong encoded_frames;
    public ulong encoded_packet_count;
    public ulong encoded_payload_bytes;
    public int is_closed;
}

internal static class FlacEncoderNative
{
    private const string DllName = "FlacEncoder.dll";

    [DllImport(DllName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int FlacEncoder_Open(
        string output_path,
        in FlacEncoderCreateParams @params,
        out IntPtr out_handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int FlacEncoder_WritePcmBytes(
        IntPtr handle,
        IntPtr pcm_bytes,
        int byte_count);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int FlacEncoder_Close(IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void FlacEncoder_Dispose(IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr FlacEncoder_GetLastError(IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int FlacEncoder_GetStats(IntPtr handle, out FlacEncoderStats out_stats);

    [DllImport(DllName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int FlacEncoder_EncodePcmToFile(
        string output_path,
        in FlacEncoderCreateParams @params,
        IntPtr pcm_bytes,
        int byte_count);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr FlacEncoder_GetBuildId();
}

public sealed class FlacEncoder : IDisposable
{
    private IntPtr _handle;
    private bool _closed;

    public FlacEncoder(string outputPath, FlacEncoderCreateParams createParams)
    {
        var rc = (FlacEncoderResultCode)FlacEncoderNative.FlacEncoder_Open(outputPath, in createParams, out _handle);
        if (rc != FlacEncoderResultCode.Ok)
        {
            throw new InvalidOperationException($"FlacEncoder_Open failed: {rc}");
        }
    }

    public void Write(byte[] pcmBytes)
    {
        if (_closed)
        {
            throw new ObjectDisposedException(nameof(FlacEncoder));
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
            var rc = (FlacEncoderResultCode)FlacEncoderNative.FlacEncoder_WritePcmBytes(_handle, ptr, pcmBytes.Length);
            if (rc != FlacEncoderResultCode.Ok)
            {
                throw new InvalidOperationException($"FlacEncoder_WritePcmBytes failed: {rc}, detail: {GetLastError()}");
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

        var rc = (FlacEncoderResultCode)FlacEncoderNative.FlacEncoder_Close(_handle);
        if (rc != FlacEncoderResultCode.Ok)
        {
            throw new InvalidOperationException($"FlacEncoder_Close failed: {rc}, detail: {GetLastError()}");
        }
        _closed = true;
    }

    public string GetLastError()
    {
        if (_handle == IntPtr.Zero)
        {
            return "handle is null";
        }
        var p = FlacEncoderNative.FlacEncoder_GetLastError(_handle);
        return p == IntPtr.Zero ? "unknown" : Marshal.PtrToStringAnsi(p) ?? "unknown";
    }

    public FlacEncoderStats GetStats()
    {
        if (_handle == IntPtr.Zero)
        {
            throw new ObjectDisposedException(nameof(FlacEncoder));
        }

        var rc = (FlacEncoderResultCode)FlacEncoderNative.FlacEncoder_GetStats(_handle, out var stats);
        if (rc != FlacEncoderResultCode.Ok)
        {
            throw new InvalidOperationException($"FlacEncoder_GetStats failed: {rc}, detail: {GetLastError()}");
        }
        return stats;
    }

    public static string GetLoadedBuildId()
    {
        var p = FlacEncoderNative.FlacEncoder_GetBuildId();
        return p == IntPtr.Zero ? "unknown" : Marshal.PtrToStringAnsi(p) ?? "unknown";
    }

    public void Dispose()
    {
        if (_handle != IntPtr.Zero)
        {
            FlacEncoderNative.FlacEncoder_Dispose(_handle);
            _handle = IntPtr.Zero;
        }
        _closed = true;
        GC.SuppressFinalize(this);
    }

    ~FlacEncoder()
    {
        Dispose();
    }
}
