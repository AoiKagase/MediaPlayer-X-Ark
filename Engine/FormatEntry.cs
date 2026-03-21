namespace MediaPlayer_X_Ark.Engine
{
    /// <summary>
    /// ファイル形式の定義エントリ。
    /// </summary>
    public sealed class FormatEntry
    {
        /// <summary>グループ名（UI上のカテゴリ）例: "音声ファイル"</summary>
        public string Group { get; }

        /// <summary>拡張子（先頭ドットあり）例: ".mp3"</summary>
        public string Ext { get; }

        /// <summary>形式の説明 例: "MPEG Audio Layer 3"</summary>
        public string Description { get; }

        /// <summary>
        /// 対応ソース。
        /// 組み込みは "FMOD" / "FluidSynth" / "App"、
        /// プラグイン由来は IFormatProvider.ProviderName が入る。
        /// </summary>
        public string Source { get; }

        public FormatEntry(string group, string ext, string description, string source)
        {
            Group       = group;
            Ext         = ext.StartsWith(".") ? ext.ToLower() : ("." + ext.ToLower());
            Description = description;
            Source      = source;
        }
    }
}
