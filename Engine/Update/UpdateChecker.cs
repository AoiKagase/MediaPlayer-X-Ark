using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Engine.Update
{
	public static class UpdateChecker
	{
		private static readonly HttpClient _http = new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(10),
			DefaultRequestHeaders = { { "User-Agent", "MediaPlayerXArk/1.0" } }
		};

		/// <summary>
		/// GitHub Releases API で最新バージョンを確認する。
		/// 新しいバージョンがあれば UpdateInfo を返す。なければ null。
		/// 取得失敗時も null を返す（起動を妨げない）。
		/// </summary>
		/// <param name="ownerRepo">owner/repo 形式（例: AoiKagase/MediaPlayer-X-Ark）</param>
		public static async Task<UpdateInfo> CheckAsync(string ownerRepo, CancellationToken ct = default)
		{
			if (string.IsNullOrWhiteSpace(ownerRepo))
				return null;
			try
			{
				var apiUrl = $"https://api.github.com/repos/{ownerRepo}/releases/latest";
				var json = await _http.GetStringAsync(apiUrl, ct);
				var doc = JsonDocument.Parse(json);
				var root = doc.RootElement;

				var tagName = root.GetProperty("tag_name").GetString();
				var version = tagName?.TrimStart('v');
				var notes = root.GetProperty("body").GetString();
				var date = root.GetProperty("published_at").GetString()?[..10];

				string downloadUrl = null;
				foreach (var asset in root.GetProperty("assets").EnumerateArray())
				{
					var name = asset.GetProperty("name").GetString();
					if (name != null && name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
					{
						downloadUrl = asset.GetProperty("browser_download_url").GetString();
						break;
					}
				}

				if (string.IsNullOrEmpty(downloadUrl))
					return null;

				var info = new UpdateInfo
				{
					Version = version,
					ReleaseDate = date,
					ReleaseNotes = notes,
					DownloadUrl = downloadUrl,
				};
				var current = GetCurrentVersion();
				return info.IsNewerThan(current) ? info : null;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// AutoUpdater の比較基準となる現在のアプリバージョンを取得する。
		/// </summary>
		public static Version GetCurrentVersion()
		{
			if (Version.TryParse(AppVersion.Current, out var v))
				return v;
			return new Version(0, 0, 0, 0);
		}
	}
}
