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
		private static readonly string _errorLogPath =
			Path.Combine(Application.StartupPath, "_update_error.log");
		private static readonly string _traceLogPath =
			Path.Combine(Application.StartupPath, "_update_trace.log");
		private const string UpdaterExeName = "updater.exe";
		private const string TargetExeName = "x-ark.exe";

		/// <summary>
		/// 更新ZIPをダウンロードして _update/ に展開し、updater.exe の起動準備を行う。
		/// 完了後、呼び出し元は LaunchUpdaterAndExit() を呼ぶこと。
		/// </summary>
		public async Task DownloadAndPrepareAsync(
			UpdateInfo info,
			IProgress<double> progress = null,
			CancellationToken ct = default)
		{
			Trace("DownloadAndPrepareAsync:start");

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
				using (var srcStream = await response.Content.ReadAsStreamAsync(ct))
				using (var destStream = File.Create(_updateZipPath))
				{
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
				}
				Trace("DownloadAndPrepareAsync:download-complete");

				// 展開
				progress?.Report(1.0);
				Trace("DownloadAndPrepareAsync:extract-start");
				Directory.CreateDirectory(_updateDir);
				ZipFile.ExtractToDirectory(_updateZipPath, _updateDir, overwriteFiles: true);
				Trace("DownloadAndPrepareAsync:extract-complete");

				// 本体同梱の updater.exe の存在確認
				Trace("DownloadAndPrepareAsync:updater-check-start");
				if (ResolveUpdaterPath() == null)
					throw new FileNotFoundException("updater.exe がアプリケーションフォルダに見つかりません。", UpdaterExeName);
				Trace("DownloadAndPrepareAsync:updater-check-complete");
			}
			catch (Exception ex)
			{
				Trace("DownloadAndPrepareAsync:error");
				WriteErrorLog(ex);
				Cleanup();
				throw;
			}
			finally
			{
				if (File.Exists(_updateZipPath))
					File.Delete(_updateZipPath);
			}
		}

		public static void CleanupStagedUpdateArtifacts()
		{
			var startupPath = Application.StartupPath;
			var updateZipPath = Path.Combine(startupPath, "_update.zip");
			var updateDir = Path.Combine(startupPath, "_update");

			for (var attempt = 0; attempt < 10; attempt++)
			{
				try
				{
					if (File.Exists(updateZipPath))
						File.Delete(updateZipPath);

					if (Directory.Exists(updateDir))
						Directory.Delete(updateDir, true);

					Trace("CleanupStagedUpdateArtifacts:completed");
					return;
				}
				catch (Exception ex)
				{
					Trace($"CleanupStagedUpdateArtifacts:retry:{attempt + 1}:{ex.GetType().Name}");
					Thread.Sleep(300);
				}
			}

			Trace("CleanupStagedUpdateArtifacts:skipped");
		}

		/// <summary>
		/// 本体同梱の updater.exe を起動してアプリケーションを終了する。
		/// DownloadAndPrepareAsync の完了後に呼ぶこと。
		/// </summary>
		public static void LaunchUpdaterAndExit()
		{
			Trace("LaunchUpdaterAndExit:start");
			var updaterPath = ResolveUpdaterPath();
			if (updaterPath == null)
			{
				Trace("LaunchUpdaterAndExit:updater-not-found");
				return;
			}

			var currentPid = Process.GetCurrentProcess().Id;
			var appDir = NormalizeArgumentPath(Application.StartupPath);
			var sourceDir = NormalizeArgumentPath(_updateDir);
			var arguments =
				$"--pid {currentPid} " +
				$"--app-dir \"{appDir}\" " +
				$"--source-dir \"{sourceDir}\" " +
				$"--exe-name \"{TargetExeName}\"";
			Trace($"LaunchUpdaterAndExit:path={updaterPath}");
			Trace($"LaunchUpdaterAndExit:args={arguments}");

			try
			{
				using var process = Process.Start(new ProcessStartInfo
				{
					FileName = updaterPath,
					Arguments = arguments,
					WorkingDirectory = Application.StartupPath,
					UseShellExecute = false,
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden,
				});
				Trace(process == null
					? "LaunchUpdaterAndExit:process-null"
					: $"LaunchUpdaterAndExit:process-started pid={process.Id}");
			}
			catch (Exception ex)
			{
				Trace($"LaunchUpdaterAndExit:exception={ex}");
				throw;
			}
			Application.Exit();
			Environment.Exit(0);
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

		private static void Trace(string message)
		{
			try
			{
				File.AppendAllText(
					_traceLogPath,
					$"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {message}{Environment.NewLine}",
					Encoding.UTF8);
			}
			catch
			{
			}
		}

		private static string ResolveUpdaterPath()
		{
			try
			{
				var directPath = Path.Combine(Application.StartupPath, UpdaterExeName);
				if (File.Exists(directPath))
					return directPath;
			}
			catch
			{
			}

			return null;
		}

		private static void WriteErrorLog(Exception ex)
		{
			try
			{
				var sb = new StringBuilder();
				sb.AppendLine($"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
				sb.AppendLine($"StartupPath: {Application.StartupPath}");
				sb.AppendLine($"UpdateZipPath: {_updateZipPath}");
				sb.AppendLine($"UpdateDir: {_updateDir}");
				sb.AppendLine();
				sb.AppendLine("ZIP exists: " + File.Exists(_updateZipPath));

				if (File.Exists(_updateZipPath))
				{
					var info = new FileInfo(_updateZipPath);
					sb.AppendLine($"ZIP size: {info.Length} bytes");
					sb.AppendLine("ZIP entries:");

					try
					{
						using var archive = ZipFile.OpenRead(_updateZipPath);
						foreach (var entry in archive.Entries)
							sb.AppendLine($"  {entry.FullName} | Compressed={entry.CompressedLength} | Size={entry.Length}");
					}
					catch (Exception zipEx)
					{
						sb.AppendLine("Failed to inspect ZIP entries:");
						sb.AppendLine(zipEx.ToString());
					}
				}

				sb.AppendLine();
				sb.AppendLine("Exception:");
				sb.AppendLine(ex.ToString());
				sb.AppendLine();
				File.WriteAllText(_errorLogPath, sb.ToString(), Encoding.UTF8);
			}
			catch
			{
			}
		}

		private static string NormalizeArgumentPath(string path)
		{
			return Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		}
	}
}
