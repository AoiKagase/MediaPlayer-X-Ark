using MediaPlayer_X_Ark.Engine.CD;
using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark
{
	public partial class CDForm : Form
	{
		private MainForm _mainForm;
		private IConfigService _config;
		private IPlayerEngine _player;
		private CdReader _cdReader;
		private CddbResult _appliedResult;    // 適用中の結果（再適用用）
		private CancellationTokenSource _cddbCts;
		public CDForm(MainForm mainForm, IPlayerEngine player, IConfigService config)
		{
			InitializeComponent();
			_mainForm = mainForm;
			_config = config;
			_player = player;
			this.Owner = mainForm;
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
				var result = _player.CreateSoundFromPCM(pcmData, title, out index);

				if (result == FMOD.RESULT.OK)
				{
					// ★ PlayListIndex を記録
					_cdReader.Tracks[trackIndex].PlayListIndex = index;
					// ★ MusicBrainz Disc ID をセット
					_player.PlayList[index].MusicBrainzDiscId = _cdReader.MusicBrainzId;
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
					_player.CreateSoundFromPCM(pcmData, title, out index);
					// ★ PlayListIndex を記録
					_cdReader.Tracks[trackIndex].PlayListIndex = index;

					_player.PlayList[index].MusicBrainzDiscId = _cdReader.MusicBrainzId;
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
				_player.Stop();
				_player.ClearPlayList();
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
			BtnCddb.Enabled = enabled;
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			_cdReader?.Dispose();
			base.OnFormClosed(e);
		}

		// ── CDDB問い合わせボタン ───────────────────────────────────────────
		private async void BtnCddb_Click(object sender, EventArgs e)
		{
			if (_cdReader == null || _cdReader.Tracks.Count == 0)
			{
				lblStatus.Text = "CDが読み込まれていません";
				return;
			}

			// 前回のキャンセル
			_cddbCts?.Cancel();
			_cddbCts = new CancellationTokenSource();
			var ct = _cddbCts.Token;

			SetButtonsEnabled(false);
			lblStatus.Text = "CDDB 問い合わせ中...";

			List<CddbResult> results;
			try
			{
				results = await CddbClient.QueryAsync(
					  _cdReader,
					  _config.settings.CddbServers,  // ← 設定から取得
					  ct);
			}
			catch (OperationCanceledException)
			{
				lblStatus.Text = "キャンセルしました";
				SetButtonsEnabled(true);
				return;
			}
			catch (Exception ex)
			{
				lblStatus.Text = $"問い合わせ失敗: {ex.Message}";
				SetButtonsEnabled(true);
				return;
			}
			finally
			{
				SetButtonsEnabled(true);
			}

			if (results.Count == 0)
			{
				lblStatus.Text = "CDDB: 一致する情報が見つかりませんでした";
				return;
			}

			// 候補を選択
			CddbResult selected;
			if (results.Count == 1)
			{
				selected = results[0];
			}
			else
			{
				// 複数候補 → 選択ダイアログ
				selected = ShowCddbSelectionDialog(results);
				if (selected == null)
				{
					lblStatus.Text = "キャンセルしました";
					return;
				}
			}

			ApplyCddbResult(selected);
		}

		// ── 候補選択ダイアログ ─────────────────────────────────────────────
		private CddbResult ShowCddbSelectionDialog(List<CddbResult> results)
		{
			using var dlg = new Form
			{
				Text = "アルバムを選択",
				Size = new System.Drawing.Size(480, 320),
				StartPosition = FormStartPosition.CenterParent,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				MinimizeBox = false,
				MaximizeBox = false,
			};

			var lbl = new Label
			{
				Text = "複数の候補が見つかりました。使用するアルバム情報を選択してください：",
				Location = new System.Drawing.Point(12, 10),
				Size = new System.Drawing.Size(440, 32),
				AutoSize = false,
			};

			var list = new ListBox
			{
				Location = new System.Drawing.Point(12, 48),
				Size = new System.Drawing.Size(440, 180),
				SelectionMode = SelectionMode.One,
			};
			foreach (var r in results)
				list.Items.Add($"[{r.SourceLabel}]  {r}");
			list.SelectedIndex = 0;

			var btnOk = new Button
			{
				Text = "OK",
				DialogResult = DialogResult.OK,
				Location = new System.Drawing.Point(280, 244),
				Size = new System.Drawing.Size(80, 28),
			};
			var btnCancel = new Button
			{
				Text = "キャンセル",
				DialogResult = DialogResult.Cancel,
				Location = new System.Drawing.Point(372, 244),
				Size = new System.Drawing.Size(80, 28),
			};

			dlg.Controls.AddRange(new Control[] { lbl, list, btnOk, btnCancel });
			dlg.AcceptButton = btnOk;
			dlg.CancelButton = btnCancel;

			// ダブルクリックでOK
			list.DoubleClick += (s, e) => { dlg.DialogResult = DialogResult.OK; dlg.Close(); };

			return dlg.ShowDialog(this) == DialogResult.OK && list.SelectedIndex >= 0
				? results[list.SelectedIndex]
				: null;
		}

		// ── 結果適用 ──────────────────────────────────────────────────────
		private void ApplyCddbResult(CddbResult result)
		{
			_appliedResult = result;

			lstTracks.Items.Clear();
			for (int i = 0; i < _cdReader.Tracks.Count; i++)
			{
				string title = i < result.Tracks.Count
					? result.Tracks[i]
					: $"Track {i + 1:D2}";

				// CdTrackInfo のタイトルを更新
				_cdReader.Tracks[i].Title = title;
				lstTracks.Items.Add($"{title}  [{_cdReader.Tracks[i].DurationText}]");

				// ★ PlayListIndex が有効なエントリに Artist/Album/Title を直接書き込む
				int plIdx = _cdReader.Tracks[i].PlayListIndex;
				if (plIdx >= 0 && plIdx < _player.PlayList.Count)
				{
					var entry = _player.PlayList[plIdx];
					entry.Title = title;
					entry.Artist = result.Artist;
					entry.Album = result.Album;
					entry.SetLength((uint)_cdReader.Tracks[i].Duration.Milliseconds);
				}
			}

			lblStatus.Text = $"[{result.SourceLabel}] {result}";
		}
	}
}