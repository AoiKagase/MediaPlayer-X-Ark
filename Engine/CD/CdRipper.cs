using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.CD
{
  /// <summary>
  /// CDDAのPCMデータをWAV / FLAC / ALACファイルとして保存する。
  /// </summary>
  public static class CdRipper
  {
    public enum OutputFormat { Wav, Flac, Alac }

    /// <summary>
    /// 1トラックをリップして保存する。
    /// </summary>
    /// <param name="pcmData">ReadTrack() で取得した44100Hz/16bit/ステレオのPCMデータ</param>
    /// <param name="outputPath">保存先フルパス（拡張子込み）</param>
    /// <param name="format">出力フォーマット</param>
    /// <param name="meta">タグ情報（null可）</param>
    /// <param name="ct">キャンセルトークン</param>
    public static Task RipAsync(
      byte[] pcmData,
      string outputPath,
      OutputFormat format,
      RipMetadata meta,
      IProgress<int> progress = null,
      CancellationToken ct = default)
    {
      return Task.Run(() =>
      {
        ct.ThrowIfCancellationRequested();

        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

        switch (format)
        {
          case OutputFormat.Wav:  WriteWav(pcmData, outputPath, progress, ct); break;
          case OutputFormat.Flac: WriteFlac(pcmData, outputPath, progress, ct); break;
          case OutputFormat.Alac: WriteAlac(pcmData, outputPath, progress, ct); break;
        }

        if (meta != null && format != OutputFormat.Alac)
          WriteTags(outputPath, meta);
      }, ct);
    }

    // ── WAV ──────────────────────────────────────────────────────────

    private static void WriteWav(byte[] pcmData, string path, IProgress<int> progress, CancellationToken ct)
    {
      const int sampleRate  = 44100;
      const short channels  = 2;
      const short bitsPerSample = 16;
      int byteRate  = sampleRate * channels * (bitsPerSample / 8);
      short blockAlign = (short)(channels * (bitsPerSample / 8));

      using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
      using var bw = new BinaryWriter(fs);

      // RIFF ヘッダ
      bw.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
      bw.Write((uint)(36 + pcmData.Length));
      bw.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"));

      // fmt チャンク
      bw.Write(System.Text.Encoding.ASCII.GetBytes("fmt "));
      bw.Write((uint)16);
      bw.Write((short)1);         // PCM
      bw.Write(channels);
      bw.Write((uint)sampleRate);
      bw.Write((uint)byteRate);
      bw.Write(blockAlign);
      bw.Write(bitsPerSample);

      // data チャンク
      bw.Write(System.Text.Encoding.ASCII.GetBytes("data"));
      bw.Write((uint)pcmData.Length);

      // PCMデータをチャンク書き込み（進捗報告付き）
      const int chunkSize = 65536;
      int written = 0;
      while (written < pcmData.Length)
      {
        ct.ThrowIfCancellationRequested();
        int len = Math.Min(chunkSize, pcmData.Length - written);
        bw.Write(pcmData, written, len);
        written += len;
        progress?.Report((int)((long)written * 100 / pcmData.Length));
      }
    }

    // ── FLAC ─────────────────────────────────────────────────────────

    private static void WriteFlac(byte[] pcmData, string path, IProgress<int> progress, CancellationToken ct)
    {
      const int bytesPerFrame = 4; // 16bit stereo @ 2ch
      var createParams = new FlacEncoderCreateParams
      {
        sample_rate = 44100,
        channels = 2,
        bits_per_sample = 16,
        frames_per_packet = 4096,
      };

      if ((pcmData.Length % bytesPerFrame) != 0)
        throw new InvalidOperationException("FLAC input PCM length is not aligned to audio frame size.");

      int chunkSize = createParams.frames_per_packet * bytesPerFrame;
      int written = 0;

      using var encoder = new FlacEncoder(path, createParams);
      while (written < pcmData.Length)
      {
        ct.ThrowIfCancellationRequested();

        int len = Math.Min(chunkSize, pcmData.Length - written);
        var chunk = new byte[len];
        Buffer.BlockCopy(pcmData, written, chunk, 0, len);

        encoder.Write(chunk);
        written += len;
        progress?.Report((int)((long)written * 100 / pcmData.Length));
      }

      encoder.Close();
    }

    // ── ALAC ─────────────────────────────────────────────────────────

    private static void WriteAlac(byte[] pcmData, string path, IProgress<int> progress, CancellationToken ct)
    {
      const int bytesPerFrame = 4; // 16bit stereo @ 2ch
      var createParams = new AlacEncoderCreateParams
      {
        sample_rate = 44100,
        channels = 2,
        bits_per_sample = 16,
        frames_per_packet = 4096,
      };

      if ((pcmData.Length % bytesPerFrame) != 0)
        throw new InvalidOperationException("ALAC input PCM length is not aligned to audio frame size.");

      int chunkSize = createParams.frames_per_packet * bytesPerFrame;
      int written = 0;

      using var encoder = new AlacEncoder(path, createParams);
      while (written < pcmData.Length)
      {
        ct.ThrowIfCancellationRequested();

        int len = Math.Min(chunkSize, pcmData.Length - written);
        var chunk = new byte[len];
        Buffer.BlockCopy(pcmData, written, chunk, 0, len);

        encoder.Write(chunk);
        written += len;
        progress?.Report((int)((long)written * 100 / pcmData.Length));
      }

      encoder.Close();
    }

    // ── メタデータ書き込み（ATL経由） ────────────────────────────────

    private static void WriteTags(string path, RipMetadata meta)
    {
      try
      {
        var track = new ATL.Track(path);
        if (!string.IsNullOrEmpty(meta.Title))   track.Title       = meta.Title;
        if (!string.IsNullOrEmpty(meta.Artist))  track.Artist      = meta.Artist;
        if (!string.IsNullOrEmpty(meta.Album))   track.Album       = meta.Album;
        if (meta.TrackNumber > 0)                track.TrackNumber = (ushort)meta.TrackNumber;
        if (meta.TrackTotal > 0)                 track.TrackTotal  = (ushort)meta.TrackTotal;
        if (meta.Year > 0)                       track.Year        = meta.Year;
        track.Save();
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine($"[CdRipper] Tag write failed: {ex.Message}");
      }
    }

    /// <summary>
    /// 保存先ファイル名を生成する（トラック番号_タイトル.拡張子）。
    /// </summary>
    public static string BuildFileName(string folder, OutputFormat format, int trackNumber, string title)
    {
      string ext = format switch
      {
        OutputFormat.Flac => ".flac",
        OutputFormat.Alac => ".m4a",
        _                 => ".wav",
      };

      string safeName = SanitizeFileName($"{trackNumber:D2} {title}");
      return Path.Combine(folder, safeName + ext);
    }

    private static string SanitizeFileName(string name)
    {
      foreach (char c in Path.GetInvalidFileNameChars())
        name = name.Replace(c, '_');
      return name.Trim();
    }
  }

  /// <summary>保存時に付与するタグ情報。</summary>
  public class RipMetadata
  {
    public string Title       { get; set; }
    public string Artist      { get; set; }
    public string Album       { get; set; }
    public int    TrackNumber { get; set; }
    public int    TrackTotal  { get; set; }
    public int    Year        { get; set; }
  }
}
