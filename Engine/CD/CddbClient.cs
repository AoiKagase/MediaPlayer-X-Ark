using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.CD
{
	/// <summary>
	/// CDDBメタデータ問い合わせクライアント。
	/// 設定されたサーバーリストを上から順に試み、
	/// 結果が空の場合は MusicBrainz にフォールバックする。
	/// </summary>
	public static class CddbClient
	{
		private const string MbBaseUrl = "https://musicbrainz.org/ws/2";
		private const string UserAgent = "MediaPlayerXArk/1.0 (contact@example.com)";
		private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

		private static readonly HttpClient _http = new HttpClient
		{
			Timeout = Timeout,
			DefaultRequestHeaders = { { "User-Agent", UserAgent } }
		};

		// ─────────────────────────────────────────
		//  公開 API
		// ─────────────────────────────────────────

		/// <summary>
		/// CdReader の情報を元に CDDB 問い合わせを行い、候補リストを返す。
		/// serverUrls のサーバーを上から順に試み、全て失敗なら MusicBrainz へフォールバック。
		/// </summary>
		public static async Task<List<CddbResult>> QueryAsync(
			CdReader cd,
			IEnumerable<string> serverUrls,
			CancellationToken ct = default)
		{
			// ① 設定サーバーを順番に試みる
			var results = await QueryGnudbAsync(cd, serverUrls, ct);

			// ② 全サーバーで取得できなければ MusicBrainz
			if (results.Count == 0)
			{
				try { results = await QueryMusicBrainzAsync(cd, ct); }
				catch { }
			}

			return results;
		}

		// ─────────────────────────────────────────
		//  CDDB プロトコル（gnudb / FreeDB 互換）
		// ─────────────────────────────────────────

		private static async Task<List<CddbResult>> QueryGnudbAsync(
			CdReader cd, IEnumerable<string> serverUrls, CancellationToken ct)
		{
			string queryCmd = BuildQueryCommand(cd);

			foreach (var baseUrl in serverUrls)
			{
				if (string.IsNullOrWhiteSpace(baseUrl)) continue;

				List<CddbResult> results = null;
				try
				{
					results = await TryQueryServer(baseUrl, queryCmd, cd, ct);
				}
				catch { /* このサーバーが失敗したら次へ */ }

				if (results != null && results.Count > 0)
				{
					// 応答したサーバーの URL を Source に記録する
					foreach (var r in results)
						r.Source = baseUrl;
					return results;
				}
			}

			return new List<CddbResult>();
		}

		/// <summary>
		/// 1サーバーへの問い合わせ。HELLO → QUERY → READ の順に実行する。
		/// HELLO は失敗しても QUERY を試みる（サーバーによっては不要）。
		/// </summary>
		private static async Task<List<CddbResult>> TryQueryServer(
			string baseUrl, string queryCmd, CdReader cd, CancellationToken ct)
		{
			// --- HELLO（失敗しても続行）---
			try
			{
				string helloUrl = BuildCddbUrl(baseUrl,
					"cddb hello user localhost MediaPlayerXArk 1.0");
				await _http.GetStringAsync(helloUrl);
			}
			catch { /* HELLO 失敗は無視して QUERY へ進む */ }

			// --- QUERY ---
			string queryUrl = BuildCddbUrl(baseUrl, queryCmd);
			string queryResponse = await _http.GetStringAsync(queryUrl);

			var matches = ParseGnudbQuery(queryResponse);
			if (matches.Count == 0) return new List<CddbResult>();

			// --- READ（候補を全件取得）---
			var results = new List<CddbResult>();
			foreach (var (category, discId) in matches)
			{
				try
				{
					string readUrl = BuildCddbUrl(baseUrl, $"cddb read {category} {discId}");
					string readResponse = await _http.GetStringAsync(readUrl);
					var result = ParseGnudbRead(readResponse, category, cd.Tracks.Count);
					if (result != null)
						results.Add(result);
				}
				catch { /* 1件の READ 失敗は無視して次候補へ */ }
			}

			// READ が全件失敗した場合でも matches があれば空リストを返す
			// （次のサーバーへは進まない）
			return results;
		}

		// ─────────────────────────────────────────
		//  URL 構築
		// ─────────────────────────────────────────

		private static string BuildCddbUrl(string baseUrl, string cmd)
		{
			string encoded = Uri.EscapeDataString(cmd);
			return $"{baseUrl}?cmd={encoded}&hello=user+localhost+MediaPlayerXArk+1.0&proto=6";
		}

		private static string BuildQueryCommand(CdReader cd)
		{
			// cddb query <discid> <ntrks> <off1> ... <offN> <total_seconds>
			var sb = new StringBuilder("cddb query ");
			sb.Append(cd.FreeDbDiscId.ToString("x8"));
			sb.Append(' ');
			sb.Append(cd.Tracks.Count);

			foreach (var track in cd.Tracks)
			{
				sb.Append(' ');
				sb.Append(track.StartSector + 150);  // +150 = MSF offset
			}

			sb.Append(' ');
			sb.Append(cd.TotalSeconds);

			return sb.ToString();
		}

		// ─────────────────────────────────────────
		//  CDDB レスポンス解析
		// ─────────────────────────────────────────

		private static List<(string Category, string DiscId)> ParseGnudbQuery(string response)
		{
			var result = new List<(string, string)>();
			if (string.IsNullOrEmpty(response)) return result;

			var lines = response.Split('\n');
			if (lines.Length == 0) return result;

			string firstLine = lines[0].Trim();

			// 200: 完全一致1件
			if (firstLine.StartsWith("200 "))
			{
				var parts = firstLine.Substring(4).Split(' ');
				if (parts.Length >= 2)
					result.Add((parts[0], parts[1]));
				return result;
			}

			// 211/210: 複数候補
			if (firstLine.StartsWith("211 ") || firstLine.StartsWith("210 "))
			{
				foreach (var line in lines.Skip(1))
				{
					string trimmed = line.Trim();
					if (trimmed == ".") break;
					var parts = trimmed.Split(' ');
					if (parts.Length >= 2)
						result.Add((parts[0], parts[1]));
				}
				return result;
			}

			return result;
		}

		private static CddbResult ParseGnudbRead(string response, string category, int trackCount)
		{
			if (string.IsNullOrEmpty(response)) return null;

			var result = new CddbResult { Genre = category };  // Source は呼び出し側で設定
			var trackTitles = new Dictionary<int, string>();

			foreach (var rawLine in response.Split('\n'))
			{
				string line = rawLine.Trim();
				if (line.StartsWith("#") || line.StartsWith(".")) continue;

				int eq = line.IndexOf('=');
				if (eq < 0) continue;

				string key = line.Substring(0, eq).Trim().ToUpper();
				string value = line.Substring(eq + 1).Trim();

				if (key == "DTITLE")
				{
					int slash = value.IndexOf(" / ");
					if (slash >= 0)
					{
						result.Artist = value.Substring(0, slash).Trim();
						result.Album = value.Substring(slash + 3).Trim();
					}
					else
					{
						result.Album = value;
					}
				}
				else if (key == "DYEAR")
				{
					result.Year = value;
				}
				else if (key == "DGENRE" && !string.IsNullOrEmpty(value))
				{
					result.Genre = value;
				}
				else if (key.StartsWith("TTITLE"))
				{
					if (int.TryParse(key.Substring(6), out int n))
					{
						// 複数行に分割されたタイトルを連結する
						if (trackTitles.ContainsKey(n))
							trackTitles[n] += value;
						else
							trackTitles[n] = value;
					}
				}
			}

			for (int i = 0; i < trackCount; i++)
				result.Tracks.Add(trackTitles.TryGetValue(i, out var t) ? t : $"Track {i + 1:D2}");

			return string.IsNullOrEmpty(result.Album) ? null : result;
		}

		// ─────────────────────────────────────────
		//  MusicBrainz（JSON API）
		// ─────────────────────────────────────────

		private static async Task<List<CddbResult>> QueryMusicBrainzAsync(
			CdReader cd, CancellationToken ct)
		{
			string url = $"{MbBaseUrl}/discid/{cd.MusicBrainzId}?fmt=json&inc=recordings+artists";
			string json;
			try
			{
				json = await _http.GetStringAsync(url);
			}
			catch (HttpRequestException ex) when (ex.Message.Contains("404"))
			{
				return new List<CddbResult>();
			}

			return ParseMusicBrainzJson(json);
		}

		private static List<CddbResult> ParseMusicBrainzJson(string json)
		{
			var results = new List<CddbResult>();
			if (string.IsNullOrEmpty(json)) return results;

			try
			{
				using var doc = JsonDocument.Parse(json);
				var root = doc.RootElement;

				if (!root.TryGetProperty("releases", out var releases)) return results;

				foreach (var release in releases.EnumerateArray())
				{
					var result = new CddbResult { Source = "MusicBrainz" };

					if (release.TryGetProperty("title", out var title))
						result.Album = title.GetString() ?? "";

					if (release.TryGetProperty("date", out var date))
						result.Year = date.GetString()?.Length >= 4
							? date.GetString().Substring(0, 4) : "";

					if (release.TryGetProperty("artist-credit", out var credits))
					{
						var names = new List<string>();
						foreach (var credit in credits.EnumerateArray())
							if (credit.TryGetProperty("artist", out var artist)
								&& artist.TryGetProperty("name", out var aname))
								names.Add(aname.GetString() ?? "");
						result.Artist = string.Join(", ", names);
					}

					if (release.TryGetProperty("media", out var mediaArr))
					{
						foreach (var medium in mediaArr.EnumerateArray())
						{
							if (!medium.TryGetProperty("tracks", out var tracks)) continue;
							foreach (var track in tracks.EnumerateArray())
							{
								string trackTitle = "";
								if (track.TryGetProperty("title", out var tTitle))
									trackTitle = tTitle.GetString() ?? "";
								result.Tracks.Add(trackTitle);
							}
							break;
						}
					}

					if (!string.IsNullOrEmpty(result.Album))
						results.Add(result);
				}
			}
			catch { }

			return results;
		}
	}
}
