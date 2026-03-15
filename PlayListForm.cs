using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		MainForm mainForm;
		public PlayListForm(MainForm main)
		{
			mainForm = main;
			InitializeComponent();
		}

		private void PlayList_Load(object sender, EventArgs e)
		{
			this.PlayListGrid.DataSource = MainForm.player.PlayList;

			//        public string FileName { get; set; }
			//        public FMOD.Sound Sound { get; set; }
			//        public string Title { get; set; }
			//        public string Artist { get; set; }
			//        public string Album { get; set; }
			//        public FMOD.SOUND_TYPE SoundType { get; set; }
			//        public FMOD.SOUND_FORMAT Format { get; set; }
			//        public int Bit { get; set; }
			//        public uint length { get; set; }
			this.PlayListGrid.Columns[0].Visible = false;
			this.PlayListGrid.Columns[1].Visible = false;
			this.PlayListGrid.Columns[3].Visible = false;
			this.PlayListGrid.Columns[4].Visible = false;
			this.PlayListGrid.Columns[5].Visible = false;
			this.PlayListGrid.Columns[6].Visible = false;
			this.PlayListGrid.Columns[7].Visible = false;

			this.PlayListGrid.Columns[2].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.PlayListGrid.Columns[8].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
		}

		private void PBtnOpen_MouseDown(object sender, MouseEventArgs e)
		{
			mainForm.BtnDownEvent(ref sender);
		}

		private void PBtnOpen_MouseUp(object sender, MouseEventArgs e)
		{
			mainForm.BtnUpEvent(ref sender);
		}

		private void PlayListGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
				mainForm.PlayLoad(e.RowIndex);
		}

		private void PlayListForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
		}

		private void PBtnClear_MouseUp(object sender, MouseEventArgs e)
		{
			mainForm.BtnUpEvent(ref sender);
		}

		private void PBtnClear_MouseDown(object sender, MouseEventArgs e)
		{
			mainForm.BtnDownEvent(ref sender);
		}

		private void PBtnClose_MouseDown(object sender, MouseEventArgs e)
		{
			mainForm.BtnDownEvent(ref sender);
		}

		private void PBtnClose_MouseUp(object sender, MouseEventArgs e)
		{
			mainForm.BtnUpEvent(ref sender);
		}

		private void PBtnDown_MouseDown(object sender, MouseEventArgs e)
		{
			mainForm.BtnDownEvent(ref sender);
		}

		private void PBtnDown_MouseUp(object sender, MouseEventArgs e)
		{
			mainForm.BtnUpEvent(ref sender);
		}

		private void PBtnRemove_MouseDown(object sender, MouseEventArgs e)
		{
			mainForm.BtnDownEvent(ref sender);
		}

		private void PBtnRemove_MouseUp(object sender, MouseEventArgs e)
		{
			mainForm.BtnUpEvent(ref sender);
		}

		private void PBtnSave_MouseDown(object sender, MouseEventArgs e)
		{
			mainForm.BtnDownEvent(ref sender);
		}

		private void PBtnSave_MouseUp(object sender, MouseEventArgs e)
		{
			mainForm.BtnUpEvent(ref sender);
		}

		private void PBtnUp_MouseDown(object sender, MouseEventArgs e)
		{
			mainForm.BtnDownEvent(ref sender);
		}

		private void PBtnUp_MouseUp(object sender, MouseEventArgs e)
		{
			mainForm.BtnUpEvent(ref sender);
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
				if (dlg.ShowDialog() != DialogResult.OK) return;

				try
				{
					var files = LoadPlaylist(dlg.FileName);
					int loaded = 0;
					foreach (var file in files)
					{
						if (System.IO.File.Exists(file))
						{
							int idx;
							MainForm.player.CreateSound(file, out idx);
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
			if (indices.Contains(mainForm.PlayingIndex))
				MainForm.player.Stop();

			foreach (int i in indices)
			{
				if (MainForm.player.PlayList[i].Sound.hasHandle())
					MainForm.player.PlayList[i].Sound.release();
				MainForm.player.PlayList.RemoveAt(i);
			}
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
				MainForm.player.Stop();
				MainForm.player.ClearPlayList();
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

			var item = MainForm.player.PlayList[idx];
			MainForm.player.PlayList.RemoveAt(idx);
			MainForm.player.PlayList.Insert(idx - 1, item);

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
			if (idx >= MainForm.player.PlayList.Count - 1) return;

			var item = MainForm.player.PlayList[idx];
			MainForm.player.PlayList.RemoveAt(idx);
			MainForm.player.PlayList.Insert(idx + 1, item);

			PlayListGrid.ClearSelection();
			PlayListGrid.Rows[idx + 1].Selected = true;
		}

		/// <summary>
		/// プレイリストをM3U形式で保存
		/// </summary>
		private void PBtnSave_Click(object sender, EventArgs e)
		{
			if (MainForm.player.PlayList.Count == 0) return;

			using (var dlg = new SaveFileDialog())
			{
				dlg.Filter = "M3U プレイリスト|*.m3u|全てのファイル|*.*";
				dlg.DefaultExt = "m3u";
				if (dlg.ShowDialog() != DialogResult.OK) return;

				using (var writer = new System.IO.StreamWriter(dlg.FileName, false, System.Text.Encoding.UTF8))
				{
					writer.WriteLine("#EXTM3U");
					foreach (var item in MainForm.player.PlayList)
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
	}
}
