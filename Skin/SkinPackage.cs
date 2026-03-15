using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Skin
{
	/// <summary>
	/// スキンファイルの形式を判別し、必要に応じてZIPを展開するクラス。
	/// .xsk（ZIP）と従来のフォルダ形式（.xsf）の両方に対応する。
	/// </summary>
	public class SkinPackage : IDisposable
	{
		private bool _disposed = false;

		/// <summary>展開後の定義ファイルパス</summary>
		public string DefinitionPath { get; private set; }

		/// <summary>展開後のスキンディレクトリ</summary>
		public string SkinDirectory { get; private set; }

		/// <summary>スキン形式</summary>
		public SkinFormat Format { get; private set; }

		/// <summary>メイン画像パス（プレビュー用）</summary>
		public string MainImagePath { get; private set; }
		/// <summary>元の（解決前の）スキンパス</summary>
		public string OriginalPath { get; private set; }
		public enum SkinFormat
		{
			/// <summary>新形式：.xsk（ZIP + JSON）</summary>
			NewXsk,
			/// <summary>旧形式：.xsf（INI）</summary>
			OldXsf,
		}

		private static readonly string TempBase =
			Path.Combine(Application.StartupPath, "Skins", "Temp");

		/// <summary>
		/// スキンを開く。形式を自動判別する。
		/// </summary>
		public static SkinPackage Open(string skinPath)
		{
			if (!File.Exists(skinPath))
			{
				// デフォルトスキンにフォールバック
				skinPath = Path.Combine(
					Application.StartupPath,
					"Skins", "bbbs.xsk");
			}

			var pkg = new SkinPackage();
			var ext = Path.GetExtension(skinPath).ToLower();
			pkg.OriginalPath = skinPath; // 解決前のパスを保存

			if (ext == ".xsk")
			{
				pkg.Format = SkinFormat.NewXsk;
				pkg.SkinDirectory = Path.Combine(
					TempBase,
					Path.GetFileNameWithoutExtension(skinPath));

				Directory.CreateDirectory(pkg.SkinDirectory);
				ZipFile.ExtractToDirectory(skinPath, pkg.SkinDirectory, true);


				// JSONファイルを探す
				var jsonFiles = Directory.GetFiles(
					pkg.SkinDirectory, "*.json", SearchOption.AllDirectories);
				if (jsonFiles.Length == 0)
					throw new FileNotFoundException(
						"スキンパッケージ内にJSONファイルが見つかりません。");

				pkg.DefinitionPath = jsonFiles[0];
				pkg.SkinDirectory = Path.GetDirectoryName(jsonFiles[0]);
				pkg.MainImagePath = FindMainImage(pkg.SkinDirectory);
			}
			else
			{
				// 旧形式：.xsf
				pkg.Format = SkinFormat.OldXsf;

				// 絶対パス・相対パスどちらにも対応
				if (!Path.IsPathRooted(skinPath))
				{
					skinPath = Path.Combine(
						System.Windows.Forms.Application.StartupPath,
						"Skins",
						skinPath);
				}

				if (!File.Exists(skinPath))
				{
					// デフォルトスキンにフォールバック
					skinPath = Path.Combine(
						System.Windows.Forms.Application.StartupPath,
						"Skins", "Default", "Default.xsf");
				}

				pkg.DefinitionPath = skinPath;
				pkg.SkinDirectory = Path.GetDirectoryName(skinPath);

				// メイン画像を探す（プレビュー用）
				pkg.MainImagePath = FindMainImage(pkg.SkinDirectory);
			}

			return pkg;
		}

		/// <summary>
		/// スキンディレクトリからメイン画像を探す
		/// </summary>
		private static string FindMainImage(string dir)
		{
			// 新形式: main.png / main.bmp
			foreach (var name in new[] { "main.png", "main.bmp", "back.png", "back.bmp" })
			{
				var path = Path.Combine(dir, name);
				if (File.Exists(path)) return path;
			}
			// 旧形式: 先頭の画像ファイルを返す
			var images = Directory.GetFiles(dir, "*.bmp");
			return images.Length > 0 ? images[0] : null;
		}

		/// <summary>
		/// 全スキンの一時ディレクトリを削除する（アプリ終了時に呼ぶ）
		/// </summary>
		public static void CleanupTempDirectory()
		{
			if (Directory.Exists(TempBase))
			{
				try { Directory.Delete(TempBase, recursive: true); }
				catch { /* 削除失敗は無視 */ }
			}
		}

		public void Dispose()
		{
			if (!_disposed)
				_disposed = true;
		}
	}
}