using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Engine.Update
{
	public class UpdateApplier
	{
		private static readonly HttpClient _http = new HttpClient
		{
			Timeout = TimeSpan.FromMinutes(10),
			DefaultRequestHeaders = { { "User-Agent", "MediaPlayerXArk/1.0" } }
		};

		private static readonly string _updateZipPath =
			Path.Combine(Application.StartupPath, "_update.zip");
		private static readonly string _updateDir =
			Path.Combine(Application.StartupPath, "_update");
		private static readonly string _batPath =
			Path.Combine(Application.StartupPath, "_update", "apply_update.bat");

		/// <summary>
		/// 更新ZIPをダウンロードして _update/ に展開し、バッチスクリプトを生成する。
		/// 完了後、呼び出し元は LaunchBatchAndExit() を呼ぶこと。
		/// </summary>
		public async Task DownloadAndPrepareAsync(
			UpdateInfo info,
			IProgress<double> progress = null,
			CancellationToken ct = default)
		{
			// 前回の残骸をクリア
			if (File.Exists(_updateZipPath))
				File.Delete(_updateZipPath);
			if (Directory.Exists(_updateDir))
				Directory.Delete(_updateDir, true);

			try
			{
				// ストリーミングダウンロード（進捗通知付き）
				using var response = await _http.GetAsync(
					info.DownloadUrl,
					HttpCompletionOption.ResponseHeadersRead,
					ct);
				response.EnsureSuccessStatusCode();

				var totalBytes = response.Content.Headers.ContentLength ?? -1L;
				using var srcStream = await response.Content.ReadAsStreamAsync(ct);
				using var destStream = File.Create(_updateZipPath);

				var buffer = new byte[81920];
				long downloadedBytes = 0L;
				int bytesRead;
				while ((bytesRead = await srcStream.ReadAsync(buffer, ct)) > 0)
				{
					await destStream.WriteAsync(buffer.AsMemory(0, bytesRead), ct);
					downloadedBytes += bytesRead;
					if (totalBytes > 0)
						progress?.Report((double)downloadedBytes / totalBytes);
				}

				// 展開
				progress?.Report(1.0);
				Directory.CreateDirectory(_updateDir);
				ZipFile.ExtractToDirectory(_updateZipPath, _updateDir, overwriteFiles: true);

				// バッチ生成
				GenerateBatchScript();
			}
			catch
			{
				Cleanup();
				throw;
			}
			finally
			{
				if (File.Exists(_updateZipPath))
					File.Delete(_updateZipPath);
			}
		}

		/// <summary>
		/// バッチスクリプトを起動してアプリケーションを終了する。
		/// DownloadAndPrepareAsync の完了後に呼ぶこと。
		/// </summary>
		public static void LaunchBatchAndExit()
		{
			if (!File.Exists(_batPath))
				return;
			Process.Start(new ProcessStartInfo
			{
				FileName = _batPath,
				WorkingDirectory = Application.StartupPath,
				UseShellExecute = true,
				WindowStyle = ProcessWindowStyle.Minimized,
			});
			Application.Exit();
		}

		private static void GenerateBatchScript()
		{
			var sb = new StringBuilder();
			sb.AppendLine("@echo off");
			sb.AppendLine("timeout /T 3 /NOBREAK >nul");
			sb.AppendLine("xcopy /E /Y \"_update\\*\" \".\\\"");
			sb.AppendLine("start \"\" \"x-ark.exe\"");
			sb.AppendLine("rmdir /S /Q \"_update\"");
			sb.AppendLine("del \"%~f0\"");
			File.WriteAllText(_batPath, sb.ToString(), Encoding.GetEncoding(932));
		}

		private static void Cleanup()
		{
			try
			{
				if (File.Exists(_updateZipPath))
					File.Delete(_updateZipPath);
				if (Directory.Exists(_updateDir))
					Directory.Delete(_updateDir, true);
			}
			catch { }
		}
	}
}
