using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MediaPlayer_X_Ark.Engine.CD;

public enum SrlaEncoderResultCode
{
    Ok = 0,
    InvalidArgument = -1,
    Io = -2,
    State = -3,
    Encode = -4,
    Internal = -5,
    NotFound = -6,
    BufferTooSmall = -7,
    TagInvalid = -8
}

[StructLayout(LayoutKind.Sequential)]
public struct SrlaEncoderCreateParams
{
    public int sample_rate;
    public int channels;
    public int bits_per_sample;
    public int frames_per_packet;
    public int srla_preset;
    public int srla_max_block_size;
    public int srla_lookahead_samples;
    public int srla_ltp_order;
}

[StructLayout(LayoutKind.Sequential)]
public struct SrlaEncoderStats
{
    public ulong total_pcm_bytes_received;
    public ulong total_pcm_frames_received;
    public ulong pending_pcm_bytes;
    public ulong encoded_frames;
    public ulong encoded_packet_count;
    public ulong encoded_payload_bytes;
    public int is_closed;
}

public enum SrlaApeItemKind
{
    Utf8 = 0,
    Binary = 1,
    External = 2
}

public sealed class SrlaApeTagItem
{
    public required string Key { get; init; }

    public required byte[] Value { get; init; }

    public SrlaApeItemKind Kind { get; init; } = SrlaApeItemKind.Utf8;
}

internal enum SrlaTagWriteMode
{
    Replace = 0,
    AppendOrReplace = 1
}

[StructLayout(LayoutKind.Sequential)]
internal struct SrlaApeTagItemNative
{
    public IntPtr key_utf8;
    public IntPtr value_data;
    public int value_size;
    public int value_kind;
}

[StructLayout(LayoutKind.Sequential)]
internal struct SrlaApeTagItemInfoNative
{
    public IntPtr key_utf8;
    public int value_size;
    public int value_kind;
}

internal static class SrlaEncoderNative
{
    private const string DllName = "SRLAEncoder.dll";

    [DllImport(DllName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int SrlaEncoder_Open(
        string output_path,
        in SrlaEncoderCreateParams @params,
        out IntPtr out_handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int SrlaEncoder_WritePcmBytes(
        IntPtr handle,
        IntPtr pcm_bytes,
        int byte_count);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int SrlaEncoder_Close(IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void SrlaEncoder_Dispose(IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr SrlaEncoder_GetLastError(IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int SrlaEncoder_GetStats(IntPtr handle, out SrlaEncoderStats out_stats);

    [DllImport(DllName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int SrlaEncoder_EncodePcmToFile(
        string output_path,
        in SrlaEncoderCreateParams @params,
        IntPtr pcm_bytes,
        int byte_count);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr SrlaEncoder_GetBuildId();

    [DllImport(DllName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int SrlaTag_WriteApeTag(
        string path,
        [In] SrlaApeTagItemNative[] items,
        int item_count,
        int write_mode);

    [DllImport(DllName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int SrlaTag_RemoveApeTag(string path);

    [DllImport(DllName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int SrlaTag_OpenApeTag(string path, out IntPtr out_handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int SrlaTag_GetItemCount(IntPtr handle, out int out_count);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int SrlaTag_GetItemInfo(IntPtr handle, int index, out SrlaApeTagItemInfoNative out_info);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int SrlaTag_CopyItemValue(
        IntPtr handle,
        int index,
        IntPtr buffer,
        int buffer_size,
        out int out_bytes_written);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr SrlaTag_GetLastError(IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void SrlaTag_Dispose(IntPtr handle);
}

public sealed class SrlaEncoder : IDisposable
{
    private IntPtr _handle;
    private bool _closed;

    public SrlaEncoder(string outputPath, SrlaEncoderCreateParams createParams)
    {
        var rc = (SrlaEncoderResultCode)SrlaEncoderNative.SrlaEncoder_Open(outputPath, in createParams, out _handle);
        if (rc != SrlaEncoderResultCode.Ok)
        {
            throw new InvalidOperationException($"SrlaEncoder_Open failed: {rc}");
        }
    }

    public void Write(byte[] pcmBytes)
    {
        if (_closed)
        {
            throw new ObjectDisposedException(nameof(SrlaEncoder));
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
            var rc = (SrlaEncoderResultCode)SrlaEncoderNative.SrlaEncoder_WritePcmBytes(_handle, ptr, pcmBytes.Length);
            if (rc != SrlaEncoderResultCode.Ok)
            {
                throw new InvalidOperationException($"SrlaEncoder_WritePcmBytes failed: {rc}, detail: {GetLastError()}");
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

        var rc = (SrlaEncoderResultCode)SrlaEncoderNative.SrlaEncoder_Close(_handle);
        if (rc != SrlaEncoderResultCode.Ok)
        {
            throw new InvalidOperationException($"SrlaEncoder_Close failed: {rc}, detail: {GetLastError()}");
        }
        _closed = true;
    }

    public string GetLastError()
    {
        if (_handle == IntPtr.Zero)
        {
            return "handle is null";
        }
        var p = SrlaEncoderNative.SrlaEncoder_GetLastError(_handle);
        return p == IntPtr.Zero ? "unknown" : Marshal.PtrToStringAnsi(p) ?? "unknown";
    }

    public SrlaEncoderStats GetStats()
    {
        if (_handle == IntPtr.Zero)
        {
            throw new ObjectDisposedException(nameof(SrlaEncoder));
        }

        var rc = (SrlaEncoderResultCode)SrlaEncoderNative.SrlaEncoder_GetStats(_handle, out var stats);
        if (rc != SrlaEncoderResultCode.Ok)
        {
            throw new InvalidOperationException($"SrlaEncoder_GetStats failed: {rc}, detail: {GetLastError()}");
        }
        return stats;
    }

    public static string GetLoadedBuildId()
    {
        var p = SrlaEncoderNative.SrlaEncoder_GetBuildId();
        return p == IntPtr.Zero ? "unknown" : Marshal.PtrToStringAnsi(p) ?? "unknown";
    }

    public void Dispose()
    {
        if (_handle != IntPtr.Zero)
        {
            SrlaEncoderNative.SrlaEncoder_Dispose(_handle);
            _handle = IntPtr.Zero;
        }
        _closed = true;
        GC.SuppressFinalize(this);
    }

    ~SrlaEncoder()
    {
        Dispose();
    }
}

public static class SrlaTag
{
    public static void WriteApeTag(string path, IReadOnlyList<SrlaApeTagItem> items)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(items);

        var nativeItems = new SrlaApeTagItemNative[items.Count];
        var allocations = new List<IntPtr>(items.Count * 2);
        try
        {
            for (var index = 0; index < items.Count; ++index)
            {
                var item = items[index] ?? throw new ArgumentNullException(nameof(items), "Tag item is null");
                if (string.IsNullOrWhiteSpace(item.Key))
                {
                    throw new ArgumentException("Tag key must not be empty", nameof(items));
                }

                var keyUtf8 = System.Text.Encoding.UTF8.GetBytes(item.Key + '\0');
                var keyPtr = Marshal.AllocHGlobal(keyUtf8.Length);
                Marshal.Copy(keyUtf8, 0, keyPtr, keyUtf8.Length);
                allocations.Add(keyPtr);

                var value = item.Value ?? throw new ArgumentException("Tag value must not be null", nameof(items));
                var valuePtr = IntPtr.Zero;
                if (value.Length > 0)
                {
                    valuePtr = Marshal.AllocHGlobal(value.Length);
                    Marshal.Copy(value, 0, valuePtr, value.Length);
                    allocations.Add(valuePtr);
                }

                nativeItems[index] = new SrlaApeTagItemNative
                {
                    key_utf8 = keyPtr,
                    value_data = valuePtr,
                    value_size = value.Length,
                    value_kind = (int)item.Kind
                };
            }

            var rc = (SrlaEncoderResultCode)SrlaEncoderNative.SrlaTag_WriteApeTag(
                path,
                nativeItems,
                nativeItems.Length,
                (int)SrlaTagWriteMode.AppendOrReplace);
            if (rc != SrlaEncoderResultCode.Ok)
            {
                throw new InvalidOperationException($"SrlaTag_WriteApeTag failed: {rc}");
            }
        }
        finally
        {
            foreach (var ptr in allocations)
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }

    public static void RemoveApeTag(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        var rc = (SrlaEncoderResultCode)SrlaEncoderNative.SrlaTag_RemoveApeTag(path);
        if (rc != SrlaEncoderResultCode.Ok)
        {
            throw new InvalidOperationException($"SrlaTag_RemoveApeTag failed: {rc}");
        }
    }

    public static IReadOnlyList<SrlaApeTagItem> ReadApeTag(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        var rc = (SrlaEncoderResultCode)SrlaEncoderNative.SrlaTag_OpenApeTag(path, out var handle);
        if (rc != SrlaEncoderResultCode.Ok)
        {
            throw new InvalidOperationException($"SrlaTag_OpenApeTag failed: {rc}");
        }

        try
        {
            rc = (SrlaEncoderResultCode)SrlaEncoderNative.SrlaTag_GetItemCount(handle, out var itemCount);
            if (rc != SrlaEncoderResultCode.Ok)
            {
                throw new InvalidOperationException($"SrlaTag_GetItemCount failed: {rc}, detail: {GetLastError(handle)}");
            }

            var result = new List<SrlaApeTagItem>(itemCount);
            for (var index = 0; index < itemCount; ++index)
            {
                rc = (SrlaEncoderResultCode)SrlaEncoderNative.SrlaTag_GetItemInfo(handle, index, out var info);
                if (rc != SrlaEncoderResultCode.Ok)
                {
                    throw new InvalidOperationException($"SrlaTag_GetItemInfo failed: {rc}, detail: {GetLastError(handle)}");
                }

                var value = Array.Empty<byte>();
                if (info.value_size > 0)
                {
                    value = new byte[info.value_size];
                    var pinned = GCHandle.Alloc(value, GCHandleType.Pinned);
                    try
                    {
                        rc = (SrlaEncoderResultCode)SrlaEncoderNative.SrlaTag_CopyItemValue(
                            handle,
                            index,
                            pinned.AddrOfPinnedObject(),
                            value.Length,
                            out var bytesWritten);
                        if (rc != SrlaEncoderResultCode.Ok)
                        {
                            throw new InvalidOperationException($"SrlaTag_CopyItemValue failed: {rc}, detail: {GetLastError(handle)}");
                        }
                        if (bytesWritten != value.Length)
                        {
                            throw new InvalidOperationException("SrlaTag_CopyItemValue returned unexpected byte count");
                        }
                    }
                    finally
                    {
                        pinned.Free();
                    }
                }

                result.Add(new SrlaApeTagItem
                {
                    Key = Marshal.PtrToStringAnsi(info.key_utf8) ?? string.Empty,
                    Value = value,
                    Kind = (SrlaApeItemKind)info.value_kind
                });
            }

            return result;
        }
        finally
        {
            SrlaEncoderNative.SrlaTag_Dispose(handle);
        }
    }

    private static string GetLastError(IntPtr handle)
    {
        var p = SrlaEncoderNative.SrlaTag_GetLastError(handle);
        return p == IntPtr.Zero ? "unknown" : Marshal.PtrToStringAnsi(p) ?? "unknown";
    }
}
