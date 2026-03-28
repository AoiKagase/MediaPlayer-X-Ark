using System;
using System.IO;
using System.Text;

namespace MediaPlayer_X_Ark.Engine.CUE
{
  /// <summary>.cueファイルを解析してCueSheetを生成する</summary>
  public static class CueParser
  {
    public static CueSheet Parse(string cuePath)
    {
      var sheet = new CueSheet { CuePath = cuePath };
      string dir = Path.GetDirectoryName(Path.GetFullPath(cuePath)) ?? "";

      CueTrack currentTrack = null;
      string currentFile = null;
      string firstFile = null;
      bool multiFile = false;

      var lines = File.ReadAllLines(cuePath, DetectEncoding(cuePath));
      foreach (var rawLine in lines)
      {
        string line = rawLine.Trim();
        if (string.IsNullOrEmpty(line)) continue;
        string upper = line.ToUpperInvariant();

        if (upper.StartsWith("REM DISCID "))
        {
          sheet.DiscId = line.Substring(11).Trim();
        }
        else if (upper.StartsWith("PERFORMER "))
        {
          string val = ExtractValue(line.Substring(10));
          if (currentTrack != null) currentTrack.Performer = val;
          else sheet.Performer = val;
        }
        else if (upper.StartsWith("TITLE "))
        {
          string val = ExtractValue(line.Substring(6));
          if (currentTrack != null) currentTrack.Title = val;
          else sheet.Title = val;
        }
        else if (upper.StartsWith("FILE "))
        {
          // FILE "filename" WAVE/FLAC/... の形式
          string fileName = ExtractFileValue(line.Substring(5));
          string resolved = Path.IsPathRooted(fileName)
            ? fileName
            : Path.Combine(dir, fileName);

          if (firstFile == null) firstFile = resolved;
          else if (!string.Equals(firstFile, resolved, StringComparison.OrdinalIgnoreCase))
            multiFile = true;

          currentFile = resolved;
          if (sheet.AudioPath == null) sheet.AudioPath = resolved;
        }
        else if (upper.StartsWith("TRACK "))
        {
          var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
          if (parts.Length >= 3 &&
              string.Equals(parts[2], "AUDIO", StringComparison.OrdinalIgnoreCase) &&
              int.TryParse(parts[1], out int num))
          {
            currentTrack = new CueTrack { Number = num, AudioFile = currentFile };
            sheet.Tracks.Add(currentTrack);
          }
        }
        else if (upper.StartsWith("INDEX 01 ") && currentTrack != null)
        {
          string time = line.Substring(9).Trim();
          currentTrack.StartMs = TimeToMs(time);
          currentTrack.StartSector = TimeToSector(time);
        }
      }

      sheet.IsMultiFile = multiFile;

      // 単一ファイル: 各トラックのEndMs = 次トラックのStartMs
      if (!multiFile)
      {
        for (int i = 0; i < sheet.Tracks.Count - 1; i++)
          sheet.Tracks[i].EndMs = sheet.Tracks[i + 1].StartMs;
        // 最終トラックは EndMs = -1（ファイル末尾）
      }
      // マルチファイル: 全トラック EndMs = -1（各ファイルの末尾）

      return sheet;
    }

    /// <summary>mm:ss:ff → ms</summary>
    public static int TimeToMs(string time)
    {
      var p = time.Split(':');
      if (p.Length != 3) return 0;
      if (!int.TryParse(p[0], out int mm) ||
          !int.TryParse(p[1], out int ss) ||
          !int.TryParse(p[2], out int ff)) return 0;
      return (mm * 60 + ss) * 1000 + (int)Math.Round(ff * 1000.0 / 75);
    }

    /// <summary>mm:ss:ff → セクタ数（CDDBクエリ用）</summary>
    public static int TimeToSector(string time)
    {
      var p = time.Split(':');
      if (p.Length != 3) return 0;
      if (!int.TryParse(p[0], out int mm) ||
          !int.TryParse(p[1], out int ss) ||
          !int.TryParse(p[2], out int ff)) return 0;
      return (mm * 60 + ss) * 75 + ff;
    }

    /// <summary>クォート付き/なしの値を取り出す</summary>
    private static string ExtractValue(string s)
    {
      s = s.Trim();
      if (s.StartsWith("\""))
      {
        int end = s.IndexOf('"', 1);
        return end > 0 ? s.Substring(1, end - 1) : s.Substring(1);
      }
      int sp = s.IndexOf(' ');
      return sp > 0 ? s.Substring(0, sp) : s;
    }

    /// <summary>FILE行からファイル名を取り出す（末尾のファイル形式トークンを除去）</summary>
    private static string ExtractFileValue(string s)
    {
      s = s.Trim();
      if (s.StartsWith("\""))
      {
        int end = s.IndexOf('"', 1);
        return end > 0 ? s.Substring(1, end - 1) : s.Substring(1);
      }
      // クォートなし: "filename FORMAT" → 末尾のトークンを除去
      int lastSp = s.LastIndexOf(' ');
      return lastSp > 0 ? s.Substring(0, lastSp).Trim() : s;
    }

    /// <summary>BOMチェックによるエンコーディング検出（デフォルトShift_JIS）</summary>
    private static Encoding DetectEncoding(string path)
    {
      using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
      var bom = new byte[4];
      int n = fs.Read(bom, 0, 4);
      if (n >= 3 && bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF)
        return new UTF8Encoding(false);
      if (n >= 2 && bom[0] == 0xFF && bom[1] == 0xFE)
        return Encoding.Unicode;
      try { return Encoding.GetEncoding("shift_jis"); }
      catch { return Encoding.UTF8; }
    }
  }
}
