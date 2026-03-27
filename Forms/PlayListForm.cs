using ATL.Playlist;
using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Internal;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark
{
	public partial class PlayListForm : Form
	{
		private ContextMenuStrip _gridContextMenu;
		private ContextMenuStrip _formContextMenu;

		private MainForm _mainForm;
		private IPlayerEngine _player;
		private IConfigService _config;
		// When true, do not forward mouse messages to owner (used while modal dialogs are open)
		private bool _suppressForwarding = false;

		public PlayListForm(MainForm main, PlayerController player, IConfigService config)
		{
			_mainForm = main;
			_player = player.Engine;
			_config = config;

			this.Owner = main;
			InitializeComponent();
		}

		private void PlayList_Load(object sender, EventArgs e)
		{
			this.PlayListGrid.DataSource = _player.PlayList;

			//        public string FileName { get; set; }
			//        public FMOD.Sound Sound { get; set; }
			//        public string Title { get; set; }
			//        public string Artist { get; set; }
			//        public string Album { get; set; }
			//        public FMOD.SOUND_TYPE SoundType { get; set; }
			//        public FMOD.SOUND_FORMAT Format { get; set; }
			//        public int Bit { get; set; }
			//        public uint length { get; set; }
			this.PlayListGrid.Columns[1].Visible = false;
			this.PlayListGrid.Columns[2].Visible = false;

			this.PlayListGrid.Columns[0].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.PlayListGrid.Columns[3].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;

			// ★コンテキストメニュー初期化
			InitContextMenus();
		}
		private void InitContextMenus()
		{
			// グリッド用コンテキストメニュー
			_gridContextMenu = new ContextMenuStrip();
			var menuPlay = new ToolStripMenuItem("再生");
			var menuDelete = new ToolStripMenuItem("削除");
			var menuUp = new ToolStripMenuItem("上へ移動");
			var menuDown = new ToolStripMenuItem("下へ移動");

			menuPlay.Click += (s, e) =>
			{
				if (PlayListGrid.SelectedRows.Count > 0)
					_mainForm.Controller.PlayAt(PlayListGrid.SelectedRows[0].Index);
			};
			menuDelete.Click += (s, e) => PBtnRemove_Click(s, e);
			menuUp.Click += (s, e) => PBtnUp_Click(s, e);
			menuDown.Click += (s, e) => PBtnDown_Click(s, e);

			_gridContextMenu.Items.AddRange(new ToolStripItem[]
			{
				menuPlay,
				new ToolStripSeparator(),
				menuDelete,
				new ToolStripSeparator(),
				menuUp,
				menuDown,
			});

			// フォーム用コンテキストメニュー
			_formContextMenu = new ContextMenuStrip();
			var menuOpen = new ToolStripMenuItem("ファイルを開く");
			var menuSave = new ToolStripMenuItem("保存");
			var menuClear = new ToolStripMenuItem("全消去");
            var menuSort = new ToolStripMenuItem("並び替え");
            var menuSortFile = new ToolStripMenuItem("ファイル名順");
            var menuSortTitle = new ToolStripMenuItem("タイトル順");
            var menuSortArtist = new ToolStripMenuItem("アーティスト順");

            menuOpen.Click += (s, e) => PBtnOpen_Click(s, e);
			menuSave.Click += (s, e) => PBtnSave_Click(s, e);
			menuClear.Click += (s, e) => PBtnClear_Click(s, e);
            menuSortFile.Click += (s, e) => SortPlayList(x => x.FileName);
            menuSortTitle.Click += (s, e) => SortPlayList(x => x.Title ?? x.FileName);
			menuSortArtist.Click += (s, e) => SortPlayList(x => x.Artist ?? "");
            menuSort.DropDownItems.AddRange(new ToolStripItem[]
			{
				menuSortFile,
				menuSortTitle,
				menuSortArtist,
			});
            _formContextMenu.Items.AddRange(new ToolStripItem[]
			{
				menuOpen,
				menuSave,
				new ToolStripSeparator(),
				menuClear,
	            new ToolStripSeparator(),   // ← 追加
				menuSort
            });

			// グリッドの右クリックイベント
			PlayListGrid.MouseDown += PlayListGrid_MouseDown;

			// フォームの右クリックイベント
			this.MouseDown += PlayListForm_MouseDown_ContextMenu;
		}
        private void SortPlayList<T>(Func<PlayList, T> keySelector)
        {
            _player.Sort(keySelector);
        }
        private void PlayListGrid_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right) return;

			// クリック位置の行を選択
			var hitTest = PlayListGrid.HitTest(e.X, e.Y);
			if (hitTest.RowIndex >= 0)
			{
				PlayListGrid.ClearSelection();
				PlayListGrid.Rows[hitTest.RowIndex].Selected = true;

				// グリッド用メニュー：選択行がある場合のみ操作を有効化
				_gridContextMenu.Show(PlayListGrid, e.Location);
			}
			else
			{
				// 行以外の場所はフォーム用メニュー
				_formContextMenu.Show(PlayListGrid, e.Location);
			}
		}
		private void PlayListForm_MouseDown_ContextMenu(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right) return;
			_formContextMenu.Show(this, e.Location);
		}
		private void PBtnOpen_MouseDown(object sender, MouseEventArgs e)
		{
			_mainForm.BtnMouseDown(sender, e);
		}

		private void PBtnOpen_MouseUp(object sender, MouseEventArgs e)
		{
			_mainForm.BtnMouseUp(sender, e);
		}

		private void PlayListGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
				_mainForm.Controller.PlayAt(e.RowIndex);
		}

		private void PlayListForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
		}

		private void PBtnClear_MouseUp(object sender, MouseEventArgs e)
		{
			_mainForm.BtnMouseUp(sender, e);
		}

		private void PBtnClear_MouseDown(object sender, MouseEventArgs e)
		{
			_mainForm.BtnMouseDown(sender, e);
		}

		private void PBtnClose_MouseDown(object sender, MouseEventArgs e)
		{
			_mainForm.BtnMouseDown(sender, e);
		}

		private void PBtnClose_MouseUp(object sender, MouseEventArgs e)
		{
			_mainForm.BtnMouseUp(sender, e);
		}

		private void PBtnDown_MouseDown(object sender, MouseEventArgs e)
		{
			_mainForm.BtnMouseDown(sender, e);
		}

		private void PBtnDown_MouseUp(object sender, MouseEventArgs e)
		{
			_mainForm.BtnMouseUp(sender, e);
		}

		private void PBtnRemove_MouseDown(object sender, MouseEventArgs e)
		{
			_mainForm.BtnMouseDown(sender, e);
		}

		private void PBtnRemove_MouseUp(object sender, MouseEventArgs e)
		{
			_mainForm.BtnMouseUp(sender, e);
		}

		private void PBtnSave_MouseDown(object sender, MouseEventArgs e)
		{
			_mainForm.BtnMouseDown(sender, e);
		}

		private void PBtnSave_MouseUp(object sender, MouseEventArgs e)
		{
			_mainForm.BtnMouseUp(sender, e);
		}

		private void PBtnUp_MouseDown(object sender, MouseEventArgs e)
		{
			_mainForm.BtnMouseDown(sender, e);
		}

		private void PBtnUp_MouseUp(object sender, MouseEventArgs e)
		{
			_mainForm.BtnMouseUp(sender, e);
		}

		private void PBtnClose_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

		/// <summary>
		/// ファイルを開く
		/// </summary>
		private void PBtnOpen_Click(object sender, EventArgs e)
		{
			using (var dlg = new OpenFileDialog())
			{
				dlg.Filter = "プレイリストファイル|*.m3u;*.m3u8;*.pls;*.wpl|" +
							 "M3U|*.m3u;*.m3u8|" +
							 "PLS|*.pls|" +
							 "WPL|*.wpl|" +
							 "全てのファイル|*.*";
				dlg.DefaultExt = "m3u";
				// disable forwarding while modal dialog is shown to avoid re-entrancy / environment-specific issues
				try
				{
					_suppressForwarding = true;
					if (dlg.ShowDialog(this) != DialogResult.OK) return;
				}
				finally
				{
					_suppressForwarding = false;
				}

				try
				{
					var files = LoadPlaylist(dlg.FileName);
					int loaded = 0;
					foreach (var file in files)
					{
						if (System.IO.File.Exists(file))
						{
							int idx;
							_player.CreateSound(file, out idx);
							loaded++;
						}
					}
					if (loaded == 0)
						MessageBox.Show(
							"再生可能なファイルが見つかりませんでした。",
							"読み込み結果",
							MessageBoxButtons.OK,
							MessageBoxIcon.Information);
				}
				catch (Exception ex)
				{
					MessageBox.Show(
						$"プレイリストの読み込みに失敗しました。\n{ex.Message}",
						"エラー",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
				}
			}
		}

		/// <summary>
		/// 選択行を削除
		/// </summary>
		private void PBtnRemove_Click(object sender, EventArgs e)
		{
			var selected = PlayListGrid.SelectedRows;
			if (selected.Count == 0) return;

			// インデックスを降順にソートして後ろから削除
			var indices = selected
				.Cast<DataGridViewRow>()
				.Select(r => r.Index)
				.OrderByDescending(i => i)
				.ToList();

			// 再生中インデックスが削除対象に含まれる場合は停止
			if (indices.Contains(_player.PlayingIndex))
				_player.Stop();

			foreach (int i in indices)
			{
				if (_player.PlayList[i].Sound.hasHandle())
					_player.PlayList[i].Sound.release();
				_player.PlayList.RemoveAt(i);
                _player.UpdateShuffleQueueOnRemove(i);
            }

			_mainForm.Controller.AutoSavePlaylist();
		}

		/// <summary>
		/// 全消去
		/// </summary>
		private void PBtnClear_Click(object sender, EventArgs e)
		{
			var confirm = MessageBox.Show(
				"プレイリストを全て消去しますか？",
				"確認",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);

			if (confirm == DialogResult.Yes)
			{
				_player.Stop();
				_player.ClearPlayList();
			}
		}

		/// <summary>
		/// 選択行を上へ移動
		/// </summary>
		private void PBtnUp_Click(object sender, EventArgs e)
		{
			if (PlayListGrid.SelectedRows.Count == 0) return;
			int idx = PlayListGrid.SelectedRows[0].Index;
			if (idx <= 0) return;

			var item = _player.PlayList[idx];
			_player.PlayList.RemoveAt(idx);
			_player.PlayList.Insert(idx - 1, item);

			PlayListGrid.ClearSelection();
			PlayListGrid.Rows[idx - 1].Selected = true;
		}

		/// <summary>
		/// 選択行を下へ移動
		/// </summary>
		private void PBtnDown_Click(object sender, EventArgs e)
		{
			if (PlayListGrid.SelectedRows.Count == 0) return;
			int idx = PlayListGrid.SelectedRows[0].Index;
			if (idx >= _player.PlayList.Count - 1) return;

			var item = _player.PlayList[idx];
			_player.PlayList.RemoveAt(idx);
			_player.PlayList.Insert(idx + 1, item);

			PlayListGrid.ClearSelection();
			PlayListGrid.Rows[idx + 1].Selected = true;
		}

		/// <summary>
		/// プレイリストをM3U形式で保存
		/// </summary>
		private void PBtnSave_Click(object sender, EventArgs e)
		{
			if (_player.PlayList.Count == 0) return;

			using (var dlg = new SaveFileDialog())
			{
				dlg.Filter = "M3U プレイリスト|*.m3u|全てのファイル|*.*";
				dlg.DefaultExt = "m3u";
				if (dlg.ShowDialog() != DialogResult.OK) return;

				using (var writer = new System.IO.StreamWriter(dlg.FileName, false, System.Text.Encoding.UTF8))
				{
					writer.WriteLine("#EXTM3U");
					foreach (var item in _player.PlayList)
					{
						writer.WriteLine($"#EXTINF:-1,{item.Title}");
						writer.WriteLine(item.FileName);
					}
				}
			}
		}
		/// <summary>
		/// プレイリストファイルからファイルパス一覧を取得する
		/// </summary>
		private List<string> LoadPlaylist(string playlistPath)
		{
			var ext = System.IO.Path.GetExtension(playlistPath).ToLower();
			var dir = System.IO.Path.GetDirectoryName(playlistPath);

			switch (ext)
			{
				case ".m3u":
				case ".m3u8":
					return LoadM3U(playlistPath, dir);
				case ".pls":
					return LoadPLS(playlistPath, dir);
				case ".wpl":
					return LoadWPL(playlistPath, dir);
				default:
					return LoadM3U(playlistPath, dir); // 不明な場合はM3Uとして試みる
			}
		}
		private List<string> LoadM3U(string path, string baseDir)
		{
			var result = new List<string>();
			// M3U8はUTF-8、M3UはShift-JIS or UTF-8
			var encoding = path.EndsWith(".m3u8", StringComparison.OrdinalIgnoreCase)
				? System.Text.Encoding.UTF8
				: System.Text.Encoding.Default;

			foreach (var line in System.IO.File.ReadAllLines(path, encoding))
			{
				if (string.IsNullOrWhiteSpace(line)) continue;
				if (line.StartsWith("#")) continue;

				result.Add(ToAbsolutePath(line.Trim(), baseDir));
			}
			return result;
		}
		private List<string> LoadPLS(string path, string baseDir)
		{
			var result = new List<string>();
			foreach (var line in System.IO.File.ReadAllLines(path, System.Text.Encoding.Default))
			{
				// File1=... File2=... の形式
				if (!line.StartsWith("File", StringComparison.OrdinalIgnoreCase)) continue;
				var eq = line.IndexOf('=');
				if (eq < 0) continue;
				var value = line.Substring(eq + 1).Trim();
				if (string.IsNullOrEmpty(value)) continue;
				result.Add(ToAbsolutePath(value, baseDir));
			}
			return result;
		}
		private List<string> LoadWPL(string path, string baseDir)
		{
			var result = new List<string>();
			var doc = new System.Xml.XmlDocument();
			doc.Load(path);
			// <media src="..."/> タグを探す
			var nodes = doc.GetElementsByTagName("media");
			foreach (System.Xml.XmlNode node in nodes)
			{
				var src = node.Attributes?["src"]?.Value;
				if (string.IsNullOrEmpty(src)) continue;
				result.Add(ToAbsolutePath(src, baseDir));
			}
			return result;
		}
		/// <summary>
		/// 相対パスを絶対パスに変換する
		/// </summary>
		private string ToAbsolutePath(string filePath, string baseDir)
		{
			if (System.IO.Path.IsPathRooted(filePath))
				return filePath;
			return System.IO.Path.GetFullPath(
				System.IO.Path.Combine(baseDir, filePath));
		}

		/// <summary>
		/// 本体ドラッグによるウィンドウ移動
		/// </summary>
		private Point mousePoint;
		/// <summary>
		/// フォーム内のマウス押下処理
		/// 位置の記憶
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PlayList_MouseDown(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				//位置を記憶する
				mousePoint = new Point(e.X, e.Y);
				_mainForm.Activate();
			}
		}

		/// <summary>
		/// フォーム内のマウス移動処理
		/// フォームの位置をマウス移動量に応じて移動する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PlayList_MouseMove(object sender, MouseEventArgs e)
		{
			if (_mainForm.SuppressNextMouseDown)
			{
				_mainForm.SuppressNextMouseDown = false;
				return;
			}

			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				Left += e.X - mousePoint.X;
				Top += e.Y - mousePoint.Y;
				var plForm = _mainForm.CurrentSkin?.SubForms["PlayListForm"];
				if (plForm != null)
				{
					if (plForm.MagnetMode)
					{
						_mainForm.Left = Left - plForm.Position.Left;
						_mainForm.Top = Top - plForm.Position.Top;
					}
				}
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				//const int WS_EX_TRANSPARENT = 0x00000020;
				var cp = base.CreateParams;
				// 透過ピクセル上のみ透過させるため
				// WndProc の HTTRANSPARENT と組み合わせて使用
				return cp;
			}
		}
		protected override void WndProc(ref Message m)
		{
			const int WM_NCHITTEST = 0x0084;
			const int WM_MOUSEMOVE = 0x0200;
			const int WM_LBUTTONDOWN = 0x0201;
			const int WM_LBUTTONUP = 0x0202;
			const int WM_RBUTTONDOWN = 0x0204;
			const int WM_RBUTTONUP = 0x0205;
			const int HTTRANSPARENT = -1;

			if (m.Msg == WM_NCHITTEST)
			{
				try
				{
					base.WndProc(ref m);
					if (m.Result == (IntPtr)1)
					{
						// use ToInt64 to be safe on both 32/64-bit
						long lp = m.LParam.ToInt64();
						var screenPt = new Point(
							(short)(lp & 0xFFFF),
							(short)((lp >> 16) & 0xFFFF));
						if (IsTransparentPixel(PointToClient(screenPt)))
							m.Result = (IntPtr)HTTRANSPARENT;
					}
				}
				catch (Exception)
				{
					// swallow to avoid crashing due to unexpected message formats on some environments
				}
				return;
			}

			if (m.Msg == WM_MOUSEMOVE ||
				m.Msg == WM_LBUTTONDOWN ||
				m.Msg == WM_LBUTTONUP ||
				m.Msg == WM_RBUTTONDOWN ||
				m.Msg == WM_RBUTTONUP)
			{
				// do not forward messages while a modal dialog is active (some environments crash when forwarded)
				if (_suppressForwarding) { base.WndProc(ref m); return; }
				try
				{
					long lp = m.LParam.ToInt64();
					var clientPt = new Point(
						(short)(lp & 0xFFFF),
						(short)((lp >> 16) & 0xFFFF));

					if (IsTransparentPixel(clientPt))
					{
						var owner = this.Owner as Form;
						if (owner != null && owner.IsHandleCreated)
						{
							var screenPt = PointToScreen(clientPt);
							var mainPt = owner.PointToClient(screenPt);
							long newLp = (((long)(mainPt.Y & 0xFFFF)) << 16) | (long)(mainPt.X & 0xFFFF);
							var newLParam = new IntPtr(newLp);
							Win32API.PostMessage(
								owner.Handle,
								(uint)m.Msg,
								m.WParam,
								newLParam);
						}
						return;
					}
				}
				catch (Exception)
				{
					// swallow to prevent environment-specific crashes; if reproducible, collect stacktrace
				}
			}

			base.WndProc(ref m);
		}

		private bool IsTransparentPixel(Point pt)
		{
			var img = this.BackgroundImage as Bitmap;
			if (img == null) return false;
			if (pt.X < 0 || pt.Y < 0 || pt.X >= img.Width || pt.Y >= img.Height)
				return false;

			var pixel = img.GetPixel(pt.X, pt.Y);
			var plForm = _mainForm.CurrentSkin?.SubForms["PlayListForm"];
			if (plForm != null)
			{
				return pixel.R == plForm.TransparentKey.R &&
					   pixel.G == plForm.TransparentKey.G &&
					   pixel.B == plForm.TransparentKey.B;
			}
			return false;
		}
	}
}
