using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaPlayer_X_Ark.Engine
{
    /// <summary>
    /// アプリケーションが対応するファイル形式のレジストリ。
    ///
    /// 組み込み形式（Built-in）は FMOD Core の公式対応フォーマットに準拠する。
    ///   Source = "FMOD"       … FMOD Core ネイティブデコード
    ///   Source = "FluidSynth" … FluidSynth 経由の MIDI
    ///   Source = "App"        … アプリ側処理（プレイリスト・CUEシート）
    ///
    /// FMOD Codec プラグイン（Plugins/*.dll）でデコードされる形式は
    /// PlayerEngine.LoadPlugins() が DLL ロード成功後に
    /// RegisterLoadedCodec() を呼ぶことで動的に追加される。
    ///   Source = DLL ファイル名（例: "codec_mp4.dll"）
    /// </summary>
    public static class SupportedFormats
    {
        // ─────────────────────────────────────────
        //  FMOD Core 組み込み対応形式
        //  FMOD Core が codec plugin なしで再生できる形式のみ列挙する。
        //  MP4/AAC 等 codec が必要なものは _codecFormatMap に定義する。
        // ─────────────────────────────────────────

        private static readonly FormatEntry[] _builtIn =
        {
            // ── 音声: FMOD Core ネイティブ ───────────────────────────
            new FormatEntry("音声ファイル", ".wav",  "Waveform Audio",                "FMOD"),
            new FormatEntry("音声ファイル", ".mp3",  "MPEG Audio Layer 3",            "FMOD"),
            new FormatEntry("音声ファイル", ".mp2",  "MPEG Audio Layer 2",            "FMOD"),
            new FormatEntry("音声ファイル", ".ogg",  "Ogg Vorbis",                    "FMOD"),
            new FormatEntry("音声ファイル", ".flac", "Free Lossless Audio Codec",     "FMOD"),
            new FormatEntry("音声ファイル", ".aiff", "Audio Interchange File Format", "FMOD"),
            new FormatEntry("音声ファイル", ".aif",  "Audio Interchange File Format", "FMOD"),
            new FormatEntry("音声ファイル", ".fsb",  "FMOD Sample Bank",              "FMOD"),
            // Windows Media 系は OS の MF codec 経由で FMOD が再生する
            new FormatEntry("音声ファイル", ".wma",  "Windows Media Audio",            "FMOD"),
            new FormatEntry("音声ファイル", ".asf",  "Advanced Systems Format",        "FMOD"),
            new FormatEntry("音声ファイル", ".asx",  "Windows Media Audio Redirector", "FMOD"),
            new FormatEntry("音声ファイル", ".wax",  "Windows Media Audio Redirector", "FMOD"),

            // ── MIDI: デフォルトは FMOD（FluidSynth 導入時は ExtensionsControl が表示を切り替える）
            new FormatEntry("MIDI", ".mid",  "MIDI Sequence",           "FMOD"),
            new FormatEntry("MIDI", ".midi", "MIDI Sequence",           "FMOD"),
            new FormatEntry("MIDI", ".rmi",  "RIFF MIDI",               "FMOD"),
            new FormatEntry("MIDI", ".kar",  "Karaoke MIDI",            "FMOD"),
            new FormatEntry("MIDI", ".dls",  "DownLoadable Sound (SF)", "FMOD"),

            // ── トラッカー: FMOD Core ネイティブ ─────────────────────
            new FormatEntry("トラッカー", ".mod", "Amiga MOD",                     "FMOD"),
            new FormatEntry("トラッカー", ".s3m", "ScreamTracker 3",               "FMOD"),
            new FormatEntry("トラッカー", ".xm",  "FastTracker 2 Extended Module", "FMOD"),
            new FormatEntry("トラッカー", ".it",  "Impulse Tracker",               "FMOD"),
            new FormatEntry("トラッカー", ".mo3", "BASS MO3 (compressed MOD)",     "FMOD"),
            new FormatEntry("トラッカー", ".umx", "Unreal Music Package",          "FMOD"),
            new FormatEntry("トラッカー", ".669", "Composer 669",                  "FMOD"),
            new FormatEntry("トラッカー", ".mtm", "MultiTracker Module",           "FMOD"),

            // ── プレイリスト / CUEシート: アプリ側処理 ───────────────
            new FormatEntry("プレイリスト", ".m3u",  "M3U Playlist",          "App"),
            new FormatEntry("プレイリスト", ".m3u8", "M3U8 Playlist (UTF-8)", "App"),
            new FormatEntry("プレイリスト", ".pls",  "PLS Playlist",          "App"),
            new FormatEntry("CUEシート",   ".cue",  "Cue Sheet",             "App"),
        };

        // ─────────────────────────────────────────
        //  FMOD Codec プラグイン → 対応形式マッピング表
        //
        //  新しい codec DLL を追加する場合はここだけ編集する。
        //  キー = DLL ファイル名（大文字小文字無視）
        // ─────────────────────────────────────────

        private static readonly Dictionary<string, FormatEntry[]> _codecFormatMap =
            new Dictionary<string, FormatEntry[]>(StringComparer.OrdinalIgnoreCase)
        {
            // ── codec_mp4.dll ─────────────────────────────────────────
            ["codec_mp4.dll"] = new[]
            {
                new FormatEntry("音声ファイル", ".aac", "Advanced Audio Coding", "codec_mp4.dll"),
                new FormatEntry("音声ファイル", ".m4a", "MPEG-4 Audio",          "codec_mp4.dll"),
                new FormatEntry("動画ファイル", ".mp4", "MPEG-4 Video",          "codec_mp4.dll"),
                new FormatEntry("動画ファイル", ".m4v", "iTunes Video",          "codec_mp4.dll"),
                new FormatEntry("動画ファイル", ".3gp", "3GPP Media",            "codec_mp4.dll"),
                new FormatEntry("動画ファイル", ".3g2", "3GPP2 Media",           "codec_mp4.dll"),
            },

            // ── codec_wma.dll（保留）────────────────────────────────
            // ["codec_wma.dll"] = new[]
            // {
            //     new FormatEntry("動画ファイル", ".wmv", "Windows Media Video", "codec_wma.dll"),
            // },
        };

        // ─────────────────────────────────────────
        //  ロード済み codec エントリ
        // ─────────────────────────────────────────

        private static readonly List<FormatEntry> _codecEntries = new List<FormatEntry>();
        private static readonly object _lock = new object();

        /// <summary>
        /// FMOD が codec DLL のロードに成功した後、PlayerEngine.LoadPlugins() から呼び出す。
        /// _codecFormatMap に定義がなければ無視される（エラーにはならない）。
        /// </summary>
        /// <param name="dllFileName">DLL ファイル名（例: "codec_mp4.dll"）パス不要</param>
        public static void RegisterLoadedCodec(string dllFileName)
        {
            if (string.IsNullOrEmpty(dllFileName)) return;
            if (!_codecFormatMap.TryGetValue(dllFileName, out var entries)) return;

            lock (_lock)
            {
                if (_codecEntries.Any(e =>
                        string.Equals(e.Source, dllFileName, StringComparison.OrdinalIgnoreCase)))
                    return;
                _codecEntries.AddRange(entries);
            }
        }

        // ─────────────────────────────────────────
        //  公開 API
        // ─────────────────────────────────────────

        /// <summary>FMOD Core 組み込み形式（読み取り専用）。</summary>
        public static IReadOnlyList<FormatEntry> BuiltIn => _builtIn;

        /// <summary>
        /// 組み込み + ロード済み codec の全形式を返す。
        /// Initialize() / LoadPlugins() の後に呼ぶことで codec 分も含まれる。
        /// </summary>
        public static IEnumerable<FormatEntry> GetAll()
        {
            foreach (var e in _builtIn) yield return e;

            List<FormatEntry> snapshot;
            lock (_lock) { snapshot = new List<FormatEntry>(_codecEntries); }
            foreach (var e in snapshot) yield return e;
        }

        /// <summary>指定した拡張子が対応形式か確認する（大文字小文字無視）。</summary>
        public static bool IsSupported(string ext)
        {
            if (string.IsNullOrEmpty(ext)) return false;
            string n = ext.StartsWith(".") ? ext.ToLower() : "." + ext.ToLower();
            return GetAll().Any(e => e.Ext == n);
        }

        /// <summary>全対応拡張子の一覧（重複除去）。OpenFileDialog の Filter 生成などに使用する。</summary>
        public static IEnumerable<string> GetAllExtensions()
            => GetAll().Select(e => e.Ext).Distinct();

        /// <summary>
        /// OpenFileDialog 用フィルター文字列を生成する。
        /// LoadPlugins() 後に呼ぶことで codec 形式も含まれる。
        /// </summary>
        public static string BuildOpenFileFilter()
        {
            var all     = GetAll().ToList();
            var allExts = all.Select(e => "*" + e.Ext).Distinct().ToArray();
            var allPart = $"全対応ファイル ({string.Join(";", allExts)})|{string.Join(";", allExts)}";

            var groupParts = all.Select(e => e.Group).Distinct().Select(g =>
            {
                var exts = all.Where(e => e.Group == g).Select(e => "*" + e.Ext).Distinct().ToArray();
                return $"{g} ({string.Join(";", exts)})|{string.Join(";", exts)}";
            });

            return string.Join("|",
                new[] { allPart }.Concat(groupParts).Append("すべてのファイル (*.*)|*.*"));
        }

        /// <summary>ロード済み codec DLL 名の一覧（ログ用）。</summary>
        public static IEnumerable<string> GetLoadedCodecNames()
        {
            lock (_lock) return _codecEntries.Select(e => e.Source).Distinct().ToList();
        }

        /// <summary>指定した codec DLL がロード済みか確認する。</summary>
        public static bool IsCodecLoaded(string dllFileName)
        {
            lock (_lock) return _codecEntries.Any(e =>
                string.Equals(e.Source, dllFileName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
