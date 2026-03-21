using MediaPlayer_X_Ark.Engine;
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
        private int _currentIndex {  get; set; }
		private CancellationTokenSource _coverArtCts;

		public FileInfoForm(IPlayerEngine player)
        {
            _player = player;
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

		// ── LoadInfo() の差し替え ─────────────────────────────────────────
		public void LoadInfo(int index)
		{
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

			// ── カバーアート取得（非同期・優先順位付き）──────────────────
			// 前回の非同期処理をキャンセル
			_coverArtCts?.Cancel();
			_coverArtCts = new CancellationTokenSource();
			var ct = _coverArtCts.Token;

			// まず ATL（埋め込み）を試みる
			var cover = MainForm.player.GetCoverArt(_currentIndex);
			if (cover != null)
			{
				picCover.Image = cover;
			}
			else
			{
				// ダミーを先に表示しておき、バックグラウンドで MusicBrainz を試みる
				picCover.Image = SetDummyCoverArt();
				_ = FetchCoverArtFallbackAsync(item.Artist, item.Album, ct);
			}
		}

		/// <summary>
		/// MusicBrainz Cover Art Archive からカバー画像を非同期取得する。
		/// 取得できたら UI スレッドで picCover に反映する。
		/// </summary>
		private async System.Threading.Tasks.Task FetchCoverArtFallbackAsync(
			string artist, string album, CancellationToken ct)
		{
			if (string.IsNullOrEmpty(album)) return;

			System.Drawing.Image img = null;
			try
			{
				img = await Engine.CoverArtClient.FetchByArtistAlbumAsync(artist, album, ct);
			}
			catch { return; }

			if (ct.IsCancellationRequested || img == null) return;

			// UI スレッドで更新
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
