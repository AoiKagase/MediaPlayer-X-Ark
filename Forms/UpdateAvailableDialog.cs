using MediaPlayer_X_Ark.Engine.Update;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms
{
	public class UpdateAvailableDialog : Form
	{
		private readonly UpdateInfo _info;
		private readonly string _currentVersion;

		private Label _lblTitle;
		private Label _lblVersionInfo;
		private Label _lblReleaseDate;
		private RichTextBox _rtbNotes;
		private ProgressBar _progressBar;
		private Label _lblStatus;
		private Button _btnDownload;
		private Button _btnClose;
		private readonly string _traceLogPath =
			Path.Combine(Application.StartupPath, "_update_trace.log");

		private CancellationTokenSource _cts;

		public UpdateAvailableDialog(UpdateInfo info)
		{
			_info = info;
			_currentVersion = UpdateChecker.GetCurrentVersion().ToString();
			BuildLayout();
			MediaPlayer_X_Ark.ApplicationIcon.ApplyTo(this);
		}

		private void BuildLayout()
		{
			this.Text = "アップデートが利用可能です";
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.StartPosition = FormStartPosition.CenterParent;
			this.ClientSize = new Size(480, 380);
			this.MaximizeBox = false;
			this.MinimizeBox = false;

			const int pad = 16;

			_lblTitle = new Label
			{
				Location = new Point(pad, pad),
				Size = new Size(448, 28),
				Font = new Font("Yu Gothic UI", 13f, FontStyle.Bold),
				Text = "新しいバージョンが利用可能です",
			};

			_lblVersionInfo = new Label
			{
				Location = new Point(pad, pad + 36),
				AutoSize = true,
				Font = new Font("Yu Gothic UI", 9f),
				Text = $"新バージョン: {_info.Version}　（現在: {_currentVersion}）",
			};

			_lblReleaseDate = new Label
			{
				Location = new Point(pad, pad + 58),
				AutoSize = true,
				Font = new Font("Yu Gothic UI", 9f),
				ForeColor = Color.Gray,
				Text = $"リリース日: {_info.ReleaseDate}",
			};

			_rtbNotes = new RichTextBox
			{
				Location = new Point(pad, pad + 84),
				Size = new Size(448, 168),
				ReadOnly = true,
				BorderStyle = BorderStyle.FixedSingle,
				Font = new Font("Yu Gothic UI", 9f),
				BackColor = Color.FromArgb(248, 248, 248),
				Text = _info.ReleaseNotes ?? "",
				ScrollBars = RichTextBoxScrollBars.Vertical,
			};

			_progressBar = new ProgressBar
			{
				Location = new Point(pad, pad + 264),
				Size = new Size(448, 18),
				Minimum = 0,
				Maximum = 100,
				Visible = false,
			};

			_lblStatus = new Label
			{
				Location = new Point(pad, pad + 288),
				Size = new Size(448, 20),
				Font = new Font("Yu Gothic UI", 9f),
				ForeColor = Color.Gray,
				Text = "",
			};

			_btnDownload = new Button
			{
				Location = new Point(272, 340),
				Size = new Size(160, 28),
				Text = "ダウンロードして更新",
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(0, 120, 215),
				ForeColor = Color.White,
				Font = new Font("Yu Gothic UI", 9f),
			};
			_btnDownload.Click += BtnDownload_Click;

			_btnClose = new Button
			{
				Location = new Point(440 - 72, 340),
				Size = new Size(56, 28),
				Text = "後で",
				FlatStyle = FlatStyle.Flat,
				Font = new Font("Yu Gothic UI", 9f),
			};
			_btnClose.Click += (s, e) => this.Close();

			// ボタン位置を右寄せで整理
			_btnClose.Location = new Point(448 - _btnClose.Width, 340);
			_btnDownload.Location = new Point(_btnClose.Left - _btnDownload.Width - 8, 340);

			Controls.AddRange(new Control[]
			{
				_lblTitle, _lblVersionInfo, _lblReleaseDate,
				_rtbNotes, _progressBar, _lblStatus,
				_btnDownload, _btnClose,
			});
		}

		private async void BtnDownload_Click(object sender, EventArgs e)
		{
			Trace("BtnDownload_Click:start");
			_btnDownload.Enabled = false;
			_btnClose.Enabled = false;
			_progressBar.Visible = true;
			_lblStatus.Text = "ダウンロード中...";
			_lblStatus.ForeColor = Color.Gray;

			_cts = new CancellationTokenSource();
			var applier = new UpdateApplier();
			var progress = new Progress<double>(v =>
			{
				_progressBar.Value = Math.Min(100, (int)(v * 100));
				_lblStatus.Text = $"ダウンロード中... {_progressBar.Value}%";
			});

			try
			{
				Trace("BtnDownload_Click:await-prepare");
				await applier.DownloadAndPrepareAsync(_info, progress, _cts.Token);
				Trace("BtnDownload_Click:prepare-complete");
				_progressBar.Value = 100;
				_lblStatus.Text = "準備完了。アプリケーションを再起動します...";
				_lblStatus.ForeColor = Color.FromArgb(0, 128, 0);
				await Task.Delay(1000);
				Trace("BtnDownload_Click:launch");
				UpdateApplier.LaunchUpdaterAndExit();
			}
			catch (OperationCanceledException)
			{
				Trace("BtnDownload_Click:canceled");
				_lblStatus.Text = "キャンセルされました。";
				_btnDownload.Enabled = true;
				_btnClose.Enabled = true;
			}
			catch (Exception ex)
			{
				Trace("BtnDownload_Click:error");
				_lblStatus.Text = $"失敗: {ex.Message} (_update_error.log を確認)";
				_lblStatus.ForeColor = Color.Red;
				_btnDownload.Enabled = true;
				_btnClose.Enabled = true;
			}
		}

		private void Trace(string message)
		{
			try
			{
				File.AppendAllText(
					_traceLogPath,
					$"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {message}{Environment.NewLine}");
			}
			catch
			{
			}
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			_cts?.Cancel();
			base.OnFormClosed(e);
		}
	}
}
