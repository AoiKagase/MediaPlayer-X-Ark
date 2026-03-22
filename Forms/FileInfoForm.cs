using MediaPlayer_X_Ark.Engine.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms
{
    public partial class FileInfoForm : Form
    {
        private readonly IPlayerEngine _player;
		private readonly MainForm _mainForm;
        private int _currentIndex {  get; set; }
		private CancellationTokenSource _coverArtCts;

		public FileInfoForm(MainForm mainform, PlayerController player)
        {
            _player = player.Engine;
			_mainForm = mainform;
			this.Owner = mainform;
			InitializeComponent();

            InitFileNameContextMenu();
        }
        private void InitFileNameContextMenu()
        {
            var fileNameMenu = new ContextMenuStrip();
            var menuCopyFileName = new ToolStripMenuItem("ファイル名をコピー");
            var menuCopyFullPath = new ToolStripMenuItem("フルパスをコピー");

            menuCopyFileName.Click += (s, e) =>
            {
                if (_currentIndex < 0) return;
                Clipboard.SetText(Path.GetFileName(_player.PlayList[_currentIndex].FileName));
            };
            menuCopyFullPath.Click += (s, e) =>
            {
                if (_currentIndex < 0) return;
                Clipboard.SetText(_player.PlayList[_currentIndex].FileName);
            };

            fileNameMenu.Items.AddRange(new ToolStripItem[]
            {
        menuCopyFileName,
        menuCopyFullPath,
            });

            lblFileNameVal.ContextMenuStrip = fileNameMenu;
        }

		public void LoadInfo()
		{
			var index = _player.PlayingIndex;
			if (index < 0 || index >= _player.PlayList.Count) return;
			_currentIndex = index;
			var item = _player.PlayList[index];

			// 基本情報
			lblTitleVal.Text = item.Title ?? "-";
			lblArtistVal.Text = item.Artist ?? "-";
			lblAlbumVal.Text = item.Album ?? "-";
			lblFileNameVal.Text = item.FileName;
			lblFormatVal.Text = item.Format.ToString();
			lblBitVal.Text = item.Bit > 0 ? $"{item.Bit}bit" : "-";
			lblLengthVal.Text = item.length;

			// サンプルレート・チャンネル
			if (item.IsLoaded)
			{
				item.Sound.getFormat(out _, out _, out int channels, out _);
				item.Sound.getDefaults(out float defaultFreq, out _);
				lblSampleRateVal.Text = $"{(int)defaultFreq}Hz";
				lblChannelVal.Text = channels == 1 ? "Mono" : "Stereo";
			}

			// ── カバーアート取得（優先順位付き）──────────────────────────
			_coverArtCts?.Cancel();
			_coverArtCts = new System.Threading.CancellationTokenSource();
			var ct = _coverArtCts.Token;

			// ① ATL 埋め込みを最優先で試みる
			var cover = _player.GetCoverArt(_currentIndex);
			if (cover != null)
			{
				picCover.Image = cover;
				return;
			}

			// ATL で取得できなければダミーを表示してバックグラウンドで取得を試みる
			picCover.Image = SetDummyCoverArt();
			_ = FetchCoverArtFallbackAsync(index, ct);
		}

		/// <summary>
		/// MusicBrainz Cover Art Archive からカバー画像を非同期取得する。
		/// ATL によるタグ取得が非同期のため、Album が空の間は最大3秒待機してからリトライする。
		/// </summary>
		private async System.Threading.Tasks.Task FetchCoverArtFallbackAsync(
		int index, System.Threading.CancellationToken ct)
		{
			if (index < 0 || index >= _player.PlayList.Count) return;
			var item = _player.PlayList[index];

			System.Drawing.Image img = null;

			// ② CDトラック：MusicBrainz Disc ID で直接取得（高速・高精度）
			if (!string.IsNullOrEmpty(item.MusicBrainzDiscId))
			{
				try
				{
					img = await Engine.CoverArtClient.FetchByDiscIdAsync(
						item.MusicBrainzDiscId, ct);
					System.Diagnostics.Debug.WriteLine(
						$"[CoverArt] DiscId={item.MusicBrainzDiscId} → {(img != null ? "OK" : "No image")}");
				}
				catch { }
			}

			// ③ 通常ファイル or Disc ID 取得失敗：Artist + Album で検索
			//    タグ取得が非同期のため Album が空の間は最大3秒待機する
			if (img == null)
			{
				const int waitMs = 500;
				const int maxRetries = 6;

				string artist = null;
				string album = null;

				for (int i = 0; i < maxRetries; i++)
				{
					if (ct.IsCancellationRequested) return;
					if (index >= _player.PlayList.Count) return;

					artist = _player.PlayList[index].Artist;
					album = _player.PlayList[index].Album;

					if (!string.IsNullOrEmpty(album)) break;

					System.Diagnostics.Debug.WriteLine(
						$"[CoverArt] Waiting for tags... ({i + 1}/{maxRetries})");
					await System.Threading.Tasks.Task.Delay(waitMs, ct);
				}

				if (!string.IsNullOrEmpty(album))
				{
					try
					{
						img = await Engine.CoverArtClient.FetchByArtistAlbumAsync(
							artist, album, ct);
						System.Diagnostics.Debug.WriteLine(
							$"[CoverArt] Search artist={artist} album={album} → {(img != null ? "OK" : "No image")}");
					}
					catch { }
				}
			}

			if (ct.IsCancellationRequested || img == null) return;
			if (picCover.IsDisposed || IsDisposed) return;

			if (InvokeRequired)
				Invoke(new Action(() => { if (!picCover.IsDisposed) picCover.Image = img; }));
			else
				picCover.Image = img;
		}


		// カバーアートはダミー表示
		private Image SetDummyCoverArt()
        {
            // グレーの四角をダミーとして表示
            var bmp = new Bitmap(picCover.Width, picCover.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.DimGray);
                g.DrawString("No Image",
                    new Font("Arial", 10),
                    Brushes.White,
                    new PointF(picCover.Width / 2 - 30, picCover.Height / 2 - 8));
            }
            return bmp;
        }
        private void FileInfoForm_Load(object sender, EventArgs e)
        {

        }
    }
}
