using System;
using System.Collections.Generic;

namespace MediaPlayer_X_Ark.Engine.CD
{
	/// <summary>CDDB/MusicBrainz 問い合わせ結果の1候補。</summary>
	public class CddbResult
	{
		/// <summary>アルバムアーティスト</summary>
		public string Artist { get; set; } = "";

		/// <summary>アルバムタイトル</summary>
		public string Album { get; set; } = "";

		/// <summary>年（取得できた場合）</summary>
		public string Year { get; set; } = "";

		/// <summary>ジャンル（gnudb カテゴリ等）</summary>
		public string Genre { get; set; } = "";

		/// <summary>トラックタイトル一覧（0始まり）</summary>
		public List<string> Tracks { get; set; } = new List<string>();

		/// <summary>
		/// 情報ソース（"MusicBrainz" またはサーバーURL）
		/// </summary>
		public string Source { get; set; } = "";

		/// <summary>
		/// CoverArt の生バイナリ。取得できなかった場合は空配列。
		/// </summary>
		public byte[] CoverArtData { get; set; } = Array.Empty<byte>();

		/// <summary>
		/// UI表示用の短いソース名。
		/// MusicBrainz はそのまま、URL はホスト名のみを返す。
		/// 例: "http://gnudb.gnudb.org/~cddb/cddb.cgi" → "gnudb.gnudb.org"
		/// </summary>
		public string SourceLabel
		{
			get
			{
				if (string.IsNullOrEmpty(Source)) return "";
				if (Source == "MusicBrainz") return "MusicBrainz";
				if (Uri.TryCreate(Source, UriKind.Absolute, out var uri))
					return uri.Host;
				return Source;
			}
		}

		public override string ToString()
			=> string.IsNullOrEmpty(Artist)
				? Album
				: $"{Artist} / {Album}{(string.IsNullOrEmpty(Year) ? "" : $" ({Year})")}";

		/// <summary>選択ダイアログ用の表示文字列。</summary>
		public string ToDisplayString()
			=> $"[{SourceLabel}]  {ToString()}";
	}
}
