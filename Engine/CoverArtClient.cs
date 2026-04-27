using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine
{
	/// <summary>
	/// MusicBrainz Cover Art Archive からカバー画像を取得するクライアント。
	///
	/// 優先順位：
	///   1. MusicBrainz Disc ID（CDの場合）→ Cover Art Archive 直引き
	///   2. Artist + Album 名 → MusicBrainz 検索 → Cover Art Archive
	/// </summary>
	public static class CoverArtClient
	{
		private const string MbApiBase = "https://musicbrainz.org/ws/2";
		private const string CaaBase = "https://coverartarchive.org";
		private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

		private static readonly HttpClient _http = new HttpClient
		{
			Timeout = Timeout,
			DefaultRequestHeaders = { { "User-Agent", Engine.Update.AppVersion.UserAgent } }
		};

		// ─────────────────────────────────────────
		//  公開 API
		// ─────────────────────────────────────────

		/// <summary>
		/// MusicBrainz Disc ID を使ってカバー画像を取得する（CD用）。
		/// Disc ID から release MBID を解決し Cover Art Archive に問い合わせる。
		/// </summary>
		public static async Task<Image> FetchByDiscIdAsync(
			string discId, CancellationToken ct = default)
		{
			var bytes = await FetchBytesByDiscIdAsync(discId, ct);
			return bytes == null ? null : CreateImage(bytes);
		}

		/// <summary>
		/// MusicBrainz Disc ID を使ってカバー画像の生バイナリを取得する（CD用）。
		/// Disc ID から release MBID を解決し Cover Art Archive に問い合わせる。
		/// </summary>
		public static async Task<byte[]> FetchBytesByDiscIdAsync(
			string discId, CancellationToken ct = default)
		{
			if (string.IsNullOrEmpty(discId)) return null;
			try
			{
				// DiscID → 最初の release MBID を取得
				string mbid = await ResolveReleaseMbidByDiscId(discId, ct);
				if (mbid == null) return null;
				return await FetchFromCaa(mbid, ct);
			}
			catch { return null; }
		}

		/// <summary>
		/// アーティスト名とアルバム名でカバー画像を取得する（一般ファイル用）。
		/// MusicBrainz でリリースを検索し Cover Art Archive に問い合わせる。
		/// </summary>
		public static async Task<Image> FetchByArtistAlbumAsync(
			string artist, string album, CancellationToken ct = default)
		{
			var bytes = await FetchBytesByArtistAlbumAsync(artist, album, ct);
			return bytes == null ? null : CreateImage(bytes);
		}

		/// <summary>
		/// アーティスト名とアルバム名でカバー画像の生バイナリを取得する（一般ファイル用）。
		/// MusicBrainz でリリースを検索し Cover Art Archive に問い合わせる。
		/// </summary>
		public static async Task<byte[]> FetchBytesByArtistAlbumAsync(
			string artist, string album, CancellationToken ct = default)
		{
			if (string.IsNullOrEmpty(album)) return null;
			try
			{
				string mbid = await ResolveReleaseMbidBySearch(artist, album, ct);
				if (mbid == null)
				{
					System.Diagnostics.Debug.WriteLine(
						$"[CoverArt] MBID not found: artist={artist} album={album}");
					return null;
				}

				var bytes = await FetchFromCaa(mbid, ct);
				System.Diagnostics.Debug.WriteLine(
					$"[CoverArt] {(bytes != null ? "OK" : "No image")} mbid={mbid}");
				return bytes;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"[CoverArt] Error: {ex.Message}");
				return null;
			}
		}

		// ─────────────────────────────────────────
		//  MBID 解決
		// ─────────────────────────────────────────

		private static async Task<string> ResolveReleaseMbidByDiscId(
			string discId, CancellationToken ct)
		{
			string url = $"{MbApiBase}/discid/{discId}?fmt=json";
			string json = await _http.GetStringAsync(url, ct);

			using var doc = JsonDocument.Parse(json);
			var root = doc.RootElement;

			// releases[0].id
			if (root.TryGetProperty("releases", out var releases))
				foreach (var rel in releases.EnumerateArray())
					if (rel.TryGetProperty("id", out var id))
						return id.GetString();

			return null;
		}

		private static async Task<string> ResolveReleaseMbidBySearch(
			string artist, string album, CancellationToken ct)
		{
			// Lucene クエリ: release:"album" AND artist:"artist"
			string query = string.IsNullOrEmpty(artist)
				? Uri.EscapeDataString($"release:\"{album}\"")
				: Uri.EscapeDataString($"release:\"{album}\" AND artist:\"{artist}\"");

			string url = $"{MbApiBase}/release?query={query}&limit=1&fmt=json";
			string json = await _http.GetStringAsync(url, ct);

			using var doc = JsonDocument.Parse(json);
			var root = doc.RootElement;

			// releases[0].id
			if (root.TryGetProperty("releases", out var releases))
				foreach (var rel in releases.EnumerateArray())
					if (rel.TryGetProperty("id", out var id))
						return id.GetString();

			return null;
		}

		// ─────────────────────────────────────────
		//  Cover Art Archive 取得
		// ─────────────────────────────────────────

		/// <summary>
		/// Cover Art Archive から front カバー画像を取得する。
		/// リダイレクトを自動追従し画像を返す。
		/// </summary>
		private static Image CreateImage(byte[] bytes)
		{
			using var ms = new MemoryStream(bytes);
			using var tmp = new Bitmap(ms);
			return new Bitmap(tmp);
		}

		private static async Task<byte[]> FetchFromCaa(string releaseMbid, CancellationToken ct)
		{
			string url = $"{CaaBase}/release/{releaseMbid}/front-500";

			var response = await _http.GetAsync(url, ct);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsByteArrayAsync(ct);
		}
	}
}
