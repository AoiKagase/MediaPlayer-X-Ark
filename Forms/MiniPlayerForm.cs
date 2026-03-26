using MediaPlayer_X_Ark.Engine;
using MediaPlayer_X_Ark.Engine.Player;
using MediaPlayer_X_Ark.Skin;
using System;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms
{
    public partial class MiniPlayerForm : Form
    {
        private readonly PlayerController _controller;
        private readonly MainForm _mainForm;
        private SkinApplicator _skinApplicator;

        public MiniPlayerForm(MainForm mainForm, PlayerController controller,
                              SkinApplicator skinApplicator = null)
        {
            InitializeComponent();
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
            SldTrack.Maximum = (int)_controller.GetLength();
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
            if (_controller == null) return;
            SldTrack.Value = (int)_controller.GetPosition();
        }

        // ── フォームクローズ ─────────────────────────────────────
        private void MiniPlayerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            _mainForm.RestoreFromMini();
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
    }
}