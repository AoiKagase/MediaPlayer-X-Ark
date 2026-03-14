using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaPlayer_X_Ark.Engine;

namespace MediaPlayer_X_Ark
{
	public partial class CDForm : Form
	{
		private MainForm _mainForm;
		private CdReader _cdReader;

		public CDForm(MainForm mainForm)
		{
			InitializeComponent();
			_mainForm = mainForm;
		}

		private void CDForm_Load(object sender, EventArgs e)
		{
			var drives = CdReader.GetCdDrives();
			foreach (var drive in drives)
				cmbDrive.Items.Add(drive);

			if (cmbDrive.Items.Count > 0)
				cmbDrive.SelectedIndex = 0;
			else
				lblStatus.Text = "CDドライブが見つかりません";
		}

		private void cmbDrive_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadTrackList();
		}

		private void LoadTrackList()
		{
			lstTracks.Items.Clear();
			try
			{
				_cdReader?.Dispose();
				_cdReader = null;

				char driveLetter = cmbDrive.SelectedItem.ToString()[0];
				_cdReader = new CdReader(driveLetter);

				foreach (var track in _cdReader.Tracks)
					lstTracks.Items.Add($"{track.Title}  [{track.DurationText}]");

				lblStatus.Text = $"{_cdReader.AudioTracks} トラック";
			}
			catch (Exception ex)
			{
				lblStatus.Text = $"読み取り失敗: {ex.Message}";
			}
		}

		private void lstTracks_DoubleClick(object sender, EventArgs e)
		{
			if (lstTracks.SelectedIndex < 0) return;
			_ = LoadAndPlayAsync(lstTracks.SelectedIndex, playImmediately: true);
		}

		private async Task LoadAndPlayAsync(int trackIndex, bool playImmediately)
		{
			SetButtonsEnabled(false);
			lblStatus.Text = $"Track {trackIndex + 1:D2} 読み込み中...";

			try
			{
				byte[] pcmData = await Task.Run(() => _cdReader.ReadTrack(trackIndex));

				string title = _cdReader.Tracks[trackIndex].Title;
				int index;
				var result = MainForm.player.CreateSoundFromPCM(pcmData, title, out index);

				if (result == FMOD.RESULT.OK)
				{
					if (playImmediately)
						_mainForm.PlayLoad(index);
					lblStatus.Text = $"{title} 完了";
				}
				else
				{
					lblStatus.Text = $"読み込み失敗: {FMOD.Error.String(result)}";
				}
			}
			catch (Exception ex)
			{
				lblStatus.Text = $"エラー: {ex.Message}";
			}
			finally
			{
				SetButtonsEnabled(true);
			}
		}

		/// <summary>
		/// 全選択
		/// </summary>
		private void BtnSelectAll_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < lstTracks.Items.Count; i++)
				lstTracks.SetSelected(i, true);
		}

		/// <summary>
		/// 全解除
		/// </summary>
		private void BtnDeselectAll_Click(object sender, EventArgs e)
		{
			lstTracks.ClearSelected();
		}

		/// <summary>
		/// プレイリストに追加
		/// </summary>
		private async void BtnAddPlaylist_Click(object sender, EventArgs e)
		{
			if (lstTracks.SelectedIndices.Count == 0) return;

			// SelectedIndices はコレクション変化に弱いので先にコピー
			int[] indices = new int[lstTracks.SelectedIndices.Count];
			lstTracks.SelectedIndices.CopyTo(indices, 0);

			SetButtonsEnabled(false);

			for (int i = 0; i < indices.Length; i++)
			{
				int trackIndex = indices[i];
				lblStatus.Text = $"Track {trackIndex + 1:D2} 読み込み中... ({i + 1}/{indices.Length})";

				try
				{
					byte[] pcmData = await Task.Run(() => _cdReader.ReadTrack(trackIndex));

					string title = _cdReader.Tracks[trackIndex].Title;
					int index;
					MainForm.player.CreateSoundFromPCM(pcmData, title, out index);
				}
				catch (Exception ex)
				{
					lblStatus.Text = $"Track {trackIndex + 1:D2} エラー: {ex.Message}";
					SetButtonsEnabled(true);
					return;
				}
			}

			lblStatus.Text = $"{indices.Length} トラックをプレイリストに追加しました";
			SetButtonsEnabled(true);
		}

		/// <summary>
		/// プレイリスト全消去
		/// </summary>
		private void BtnClearPlaylist_Click(object sender, EventArgs e)
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
				lblStatus.Text = "プレイリストを消去しました";
			}
		}

		/// <summary>
		/// 更新
		/// </summary>
		private void BtnRefresh_Click(object sender, EventArgs e)
		{
			LoadTrackList();
		}

		/// <summary>
		/// 取り出し
		/// </summary>
		private void BtnEject_Click(object sender, EventArgs e)
		{
			if (_cdReader == null) return;
			try
			{
				_cdReader.Eject();
				lstTracks.Items.Clear();
				lblStatus.Text = "取り出しました";
			}
			catch
			{
				lblStatus.Text = "取り出しに失敗しました";
			}
		}

		/// <summary>
		/// 閉じる
		/// </summary>
		private void BtnClose_Click(object sender, EventArgs e)
		{
			Hide();
		}

		private void CDForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}

		private void SetButtonsEnabled(bool enabled)
		{
			BtnSelectAll.Enabled = enabled;
			BtnDeselectAll.Enabled = enabled;
			BtnAddPlaylist.Enabled = enabled;
			BtnClearPlaylist.Enabled = enabled;
			BtnRefresh.Enabled = enabled;
			BtnEject.Enabled = enabled;
			BtnClose.Enabled = enabled;
			cmbDrive.Enabled = enabled;
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			_cdReader?.Dispose();
			base.OnFormClosed(e);
		}
	}
}