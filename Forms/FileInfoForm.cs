using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using MediaPlayer_X_Ark.Skin;
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
		private readonly IConfigService _config;
		private readonly MainForm _mainForm;
		private readonly Dictionary<Control, ControlAppearance> _defaultControlAppearances = new Dictionary<Control, ControlAppearance>();
		private FormAppearance _defaultFormAppearance;
		private int _currentIndex { get; set; }
		private CancellationTokenSource _coverArtCts;

		public FileInfoForm(MainForm mainform, PlayerController player)
		{
			_player = player.Engine;
			_config = player.Config;
			_mainForm = mainform;
			this.Owner = mainform;
			InitializeComponent();
			ApplicationIcon.ApplyTo(this);
			CaptureDefaultAppearance();

			InitFileNameContextMenu();
		}

		public void ApplySkin(SkinApplicator applicator, bool hasFileInfoSkin)
		{
			RestoreDefaultAppearance();

			if (!hasFileInfoSkin)
			{
				BtnClose.Visible = false;
				BtnClose.Enabled = false;
				if (Visible)
					LoadInfo();
				return;
			}

			FormBorderStyle = FormBorderStyle.None;
			applicator?.ApplyToFileInfoForm(this);
			if (Visible)
				LoadInfo();
		}

		private void CaptureDefaultAppearance()
		{
			_defaultFormAppearance = new FormAppearance
			{
				ClientSize = ClientSize,
				BackColor = BackColor,
				BackgroundImage = BackgroundImage,
				ForeColor = ForeColor,
				FormBorderStyle = FormBorderStyle,
				TransparencyKey = TransparencyKey,
			};
			CaptureControlAppearances(Controls);
		}

		private void CaptureControlAppearances(Control.ControlCollection controls)
		{
			foreach (Control control in controls)
			{
				_defaultControlAppearances[control] = new ControlAppearance
				{
					Location = control.Location,
					Size = control.Size,
					BackColor = control.BackColor,
					ForeColor = control.ForeColor,
					Font = control.Font,
					Visible = control.Visible,
					Enabled = control.Enabled,
					LabelBorderStyle = control is Label label ? label.BorderStyle : null,
					LabelTextAlign = control is Label labelAlign ? labelAlign.TextAlign : null,
				};

				if (control.Controls.Count > 0)
					CaptureControlAppearances(control.Controls);
			}
		}

		private void RestoreDefaultAppearance()
		{
			SuspendLayout();
			try
			{
				ClientSize = _defaultFormAppearance.ClientSize;
				BackColor = _defaultFormAppearance.BackColor;
				BackgroundImage = _defaultFormAppearance.BackgroundImage;
				ForeColor = _defaultFormAppearance.ForeColor;
				TransparencyKey = _defaultFormAppearance.TransparencyKey;
				RestoreControlAppearances(Controls);
				ShowDefaultControls(Controls);
				FormBorderStyle = _defaultFormAppearance.FormBorderStyle;
				UpdateStyles();
			}
			finally
			{
				ResumeLayout(false);
				PerformLayout();
				Refresh();
			}
		}

		private void ShowDefaultControls(Control.ControlCollection controls)
		{
			foreach (Control control in controls)
			{
				control.Visible = true;
				control.Enabled = true;
				control.BringToFront();

				if (control.Controls.Count > 0)
					ShowDefaultControls(control.Controls);
			}
		}

		private void RestoreControlAppearances(Control.ControlCollection controls)
		{
			foreach (Control control in controls)
			{
				if (_defaultControlAppearances.TryGetValue(control, out var appearance))
				{
					control.Location = appearance.Location;
					control.Size = appearance.Size;
					control.BackColor = appearance.BackColor;
					control.ForeColor = appearance.ForeColor;
					control.Font = appearance.Font;
					control.Visible = appearance.Visible;
					control.Enabled = appearance.Enabled;

					if (control is Label label)
					{
						if (appearance.LabelBorderStyle.HasValue)
							label.BorderStyle = appearance.LabelBorderStyle.Value;
						if (appearance.LabelTextAlign.HasValue)
							label.TextAlign = appearance.LabelTextAlign.Value;
					}
				}

				if (control.Controls.Count > 0)
					RestoreControlAppearances(control.Controls);
			}
		}

		private struct FormAppearance
		{
			public Size ClientSize;
			public Color BackColor;
			public Image BackgroundImage;
			public Color ForeColor;
			public FormBorderStyle FormBorderStyle;
			public Color TransparencyKey;
		}

		private struct ControlAppearance
		{
			public Point Location;
			public Size Size;
			public Color BackColor;
			public Color ForeColor;
			public Font Font;
			public bool Visible;
			public bool Enabled;
			public BorderStyle? LabelBorderStyle;
			public ContentAlignment? LabelTextAlign;
		}
		private void InitFileNameContextMenu()
		{
			var fileNameMenu = new ContextMenuStrip();
			fileNameMenu.ShowImageMargin = false;
			fileNameMenu.AutoSize = true;
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
			lblYearVal.Text = item.Year > 0 ? item.Year.ToString() : "-";
			lblTrackVal.Text = item.TrackNumber > 0
				? (item.TrackTotal > 0
					? $"{item.TrackNumber}/{item.TrackTotal}"
					: item.TrackNumber.ToString())
				: "-";
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

			// ① ファイルから直接取得できる場合はそれを優先（例：ATLが対応しておらず、コーデックプラグインが対応している場合など）
			FMOD.RESULT result = _player.GetTag("COVERART", index, out FMOD.TAG coverTag);
			_player.DumpTags(index);
			if (result == FMOD.RESULT.OK)
			{
				if (coverTag.datatype == FMOD.TAGDATATYPE.BINARY)
				{
					try
					{
						byte[] imgData = new byte[coverTag.datalen];
						System.Runtime.InteropServices.Marshal.Copy(coverTag.data, imgData, 0, (int)coverTag.datalen);
						using (var ms = new MemoryStream(imgData))
						{
							using var tmp = Image.FromStream(ms);
							img = new Bitmap(tmp);
							if (InvokeRequired)
								Invoke(new Action(() => { if (!picCover.IsDisposed) picCover.Image = img; }));
							else
								picCover.Image = img;
						}
						return;
					}
					catch
					{
					}
				}
			}

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

			// ② CUEトラック：REM DISCIDでCDDB問い合わせ（Artist/Albumが未設定の場合）
			if (img == null && item.IsCueTrack && item.CueSheetRef != null
				&& !string.IsNullOrEmpty(item.CueSheetRef.DiscId)
				&& (string.IsNullOrEmpty(item.Artist) || string.IsNullOrEmpty(item.Album)))
			{
				try
				{
					var cddbResults = await Engine.CD.CddbClient.QueryByCueAsync(
						item.CueSheetRef,
						_config.settings.CddbServers,
						ct);

					if (cddbResults.Count > 0)
					{
						var best = cddbResults[0];
						// 同じCUEシートの全トラックにタグを適用
						for (int i = 0; i < _player.PlayList.Count; i++)
						{
							var e = _player.PlayList[i];
							if (!e.IsCueTrack || e.CueSheetRef != item.CueSheetRef) continue;

							int trackIdx = i - _player.PlayList.IndexOf(
								_player.PlayList.First(p =>
									p.IsCueTrack && p.CueSheetRef == item.CueSheetRef));

							if (string.IsNullOrEmpty(e.Artist))
								e.Artist = best.Artist ?? "";
							if (string.IsNullOrEmpty(e.Album))
								e.Album = best.Album ?? "";
							if (trackIdx >= 0 && trackIdx < best.Tracks.Count
								&& e.Title.StartsWith("Track "))
								e.Title = best.Tracks[trackIdx];
						}

						// 現在表示中のエントリのラベルを更新
						if (index == _currentIndex)
						{
							var updated = _player.PlayList[index];
							if (InvokeRequired)
								Invoke(new Action(() =>
								{
									lblTitleVal.Text = updated.Title ?? "-";
									lblArtistVal.Text = updated.Artist ?? "-";
									lblAlbumVal.Text = updated.Album ?? "-";
								}));
							else
							{
								lblTitleVal.Text = updated.Title ?? "-";
								lblArtistVal.Text = updated.Artist ?? "-";
								lblAlbumVal.Text = updated.Album ?? "-";
							}
						}
					}
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

		private void FileInfoForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}

		private void FileInfoForm_Activated(object sender, EventArgs e)
		{
			var optionForm = _mainForm.ManagedForms.FirstOrDefault(f => f.Name == "OptionsForm");
			if (optionForm != null && optionForm.IsHandleCreated && optionForm.Visible)
			{
				Win32API.SetWindowPos(optionForm.Handle, Win32API.HWND_TOP, 0, 0, 0, 0,
					Win32API.SWP_NOMOVE | Win32API.SWP_NOSIZE | Win32API.SWP_NOACTIVATE);
			}
		}

		private void BtnClose_Click(object sender, EventArgs e)
		{
			Hide();
		}
	}
}
