using System.Collections.Generic;

namespace MediaPlayer_X_Ark.Engine.CUE
{
  /// <summary>CUEシートのデータモデル</summary>
  public class CueSheet
  {
    public string CuePath { get; set; }
    /// <summary>単一ファイルCUEの音声ファイルパス（マルチファイル時は最初のファイル）</summary>
    public string AudioPath { get; set; }
    public string Performer { get; set; }
    public string Title { get; set; }
    /// <summary>REM DISCID（FreeDB形式 8桁hex）</summary>
    public string DiscId { get; set; }
    /// <summary>trueの場合、トラックごとに別ファイルを参照するマルチファイルCUE</summary>
    public bool IsMultiFile { get; set; }
    /// <summary>ディスク総再生時間（ms）。CreateCueSoundsで設定される</summary>
    public int TotalDurationMs { get; set; }
    public List<CueTrack> Tracks { get; } = new List<CueTrack>();
  }

  /// <summary>CUEシート内の1トラック</summary>
  public class CueTrack
  {
    public int Number { get; set; }
    public string Title { get; set; }
    /// <summary>null = CueSheet.Performer を使用</summary>
    public string Performer { get; set; }
    /// <summary>マルチファイルCUE時の個別音声ファイルパス</summary>
    public string AudioFile { get; set; }
    /// <summary>ファイル先頭からの開始位置（ms）</summary>
    public int StartMs { get; set; }
    /// <summary>終了位置（ms）。-1 = ファイル末尾</summary>
    public int EndMs { get; set; } = -1;
    /// <summary>CDDBクエリ用セクタ数（75sectors/sec換算）</summary>
    public int StartSector { get; set; }
  }
}
