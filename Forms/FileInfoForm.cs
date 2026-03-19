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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms
{
    public partial class FileInfoForm : Form
    {
        private readonly IPlayerEngine _player;
        private int _currentIndex {  get; set; }

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
                int freq, channels, bits;
                FMOD.SOUND_FORMAT fmt;
                FMOD.SOUND_TYPE type;
                item.Sound.getFormat(out type, out fmt, out channels, out bits);
                item.Sound.getDefaults(out float defaultFreq, out _);
                lblSampleRateVal.Text = $"{(int)defaultFreq}Hz";
                lblChannelVal.Text = channels == 1 ? "Mono" : "Stereo";
            }

			// カバーアート
			// TODO: プラグインシステム実装後にカバーアート取得に差し替え
			var cover = MainForm.player.GetCoverArt(_currentIndex);
			picCover.Image = cover ?? SetDummyCoverArt();
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
