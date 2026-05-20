using MediaPlayer_X_Ark.Engine;
using MediaPlayer_X_Ark.Engine.Player;
using MediaPlayer_X_Ark.Skin;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms
{
	public partial class MiniPlayerForm : Form
	{
		private static int ClampTrackSliderValue(uint value)
			=> (int)Math.Min(value, (uint)int.MaxValue);

		private readonly PlayerController _controller;
		private readonly MainForm _mainForm;
		private SkinApplicator _skinApplicator;

		public MiniPlayerForm(MainForm mainForm, PlayerController controller,
							  SkinApplicator skinApplicator = null)
		{
			InitializeComponent();
			MediaPlayer_X_Ark.ApplicationIcon.ApplyTo(this);
			_mainForm = mainForm;
			_controller = controller;
			_skinApplicator = skinApplicator;

			this.Owner = mainForm;

			// イベント購読
			_controller.TrackChanged += OnTrackChanged;
			_controller.PlaybackStateChanged += OnPlaybackStateChanged;
		}

		public void ApplySkin(SkinApplicator applicator)
		{
			_skinApplicator = applicator;
			_skinApplicator?.ApplyToMiniPlayerForm(this);
		}

		private void MiniPlayerForm_Load(object sender, EventArgs e)
		{
			UpdateTitle();
			UpdatePlayPauseButton();
			SldVolume.Value = _controller.GetVolume();
		}

		private void OnTrackChanged(int index)
		{
			if (InvokeRequired) { Invoke(new Action(() => OnTrackChanged(index))); return; }
			UpdateTitle();
			SldTrack.Maximum = ClampTrackSliderValue(_controller.GetLength());
			SldTrack.Value = 0;
		}

		private void OnPlaybackStateChanged()
		{
			if (InvokeRequired) { Invoke(new Action(OnPlaybackStateChanged)); return; }
			UpdatePlayPauseButton();
		}

		private void UpdateTitle()
		{
			LabelTitle.Value.Text = _controller.BuildTitleText();
		}

		private void UpdatePlayPauseButton()
		{
			BtnPlay.Visible = !_controller.IsPlaying;
			BtnPause.Visible = _controller.IsPlaying;
		}

		// ── ボタン ──────────────────────────────────────────────
		private void BtnPlay_Click(object sender, EventArgs e)
			=> _controller.TogglePlayPause();

		private void BtnPause_Click(object sender, EventArgs e)
			=> _controller.TogglePlayPause();

		private void BtnStop_Click(object sender, EventArgs e)
			=> _controller.Stop();

		private void BtnBack_Click(object sender, EventArgs e)
			=> _controller.PlayPrevious();

		private void BtnNext_Click(object sender, EventArgs e)
			=> _controller.PlayNext();

		private void BtnClose_Click(object sender, EventArgs e)
		{
			// MiniPlayerForm を閉じて MainForm へ復帰
			_mainForm.RestoreFromMini();
		}

		// ── スライダー ───────────────────────────────────────────
		private void SldTrack_SliderMoved(object sender, MouseEventArgs e)
			=> _controller.SetPosition((uint)SldTrack.Value);

		private void SldVolume_SliderMoving(object sender, MouseEventArgs e)
			=> _controller.SetVolume(SldVolume.Value);

		private void SldVolume_SliderMoved(object sender, MouseEventArgs e)
			=> _controller.SetVolume(SldVolume.Value);

		// ── タイマー（シークバー更新）──────────────────────────
		private void MiniTimer_Tick(object sender, EventArgs e)
		{
			if (_controller == null || !_controller.IsPlaying)
				return;

			SldTrack.Value = Math.Min(
				ClampTrackSliderValue(_controller.GetPosition()),
				SldTrack.Maximum);
		}

		// ── フォームクローズ ─────────────────────────────────────
		private void MiniPlayerForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			if (e.CloseReason == CloseReason.UserClosing)
				_mainForm.FormClose();
			else
				_mainForm.RestoreFromMini();
		}

		protected override void WndProc(ref Message m)
		{
			const int WM_SYSCOMMAND = 0x0112;
			const int SC_CLOSE = 0xF060;
			if (m.Msg == WM_SYSCOMMAND && (int)(m.WParam.ToInt64() & 0xFFF0) == SC_CLOSE)
			{
				_mainForm.FormClose();
				return;
			}
			base.WndProc(ref m);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_controller.TrackChanged -= OnTrackChanged;
				_controller.PlaybackStateChanged -= OnPlaybackStateChanged;
				components?.Dispose();
			}
			base.Dispose(disposing);
		}

		private void MiniPlayerForm_Activated(object sender, EventArgs e)
		{
			var optionForm = _mainForm.ManagedForms.FirstOrDefault(f => f.Name == "OptionsForm");
			if (optionForm != null && optionForm.IsHandleCreated && optionForm.Visible)
			{
				Win32API.SetWindowPos(optionForm.Handle, Win32API.HWND_TOP, 0, 0, 0, 0,
					Win32API.SWP_NOMOVE | Win32API.SWP_NOSIZE | Win32API.SWP_NOACTIVATE);
			}
		}
		/// <summary>
		/// 本体ドラッグによるウィンドウ移動
		/// </summary>
		private Point mousePoint;
		private void MiniPlayerForm_MouseDown(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				mousePoint = new Point(e.X, e.Y);
				this.Activate();
			}
		}

		private void MiniPlayerForm_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				Left += e.X - mousePoint.X;
				Top += e.Y - mousePoint.Y;
			}
		}
	}
}
