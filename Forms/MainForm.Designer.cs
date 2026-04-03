
using MediaPlayer_X_Ark.Controls;

namespace MediaPlayer_X_Ark
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			BtnPlay = new System.Windows.Forms.Button();
			BtnStop = new System.Windows.Forms.Button();
			Timer = new System.Windows.Forms.Timer(components);
			OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
			BtnOpen = new System.Windows.Forms.Button();
			BtnClose = new System.Windows.Forms.Button();
			BtnMinisize = new System.Windows.Forms.Button();
			BtnPlaylist = new System.Windows.Forms.Button();
			BtnSetting = new System.Windows.Forms.Button();
			BtnLoop = new System.Windows.Forms.Button();
			BtnRandom = new System.Windows.Forms.Button();
			BtnNext = new System.Windows.Forms.Button();
			BtnSeekForward = new System.Windows.Forms.Button();
			BtnPause = new System.Windows.Forms.Button();
			BtnSeekBack = new System.Windows.Forms.Button();
			BtnBack = new System.Windows.Forms.Button();
			SldVolume = new CustomSlider();
			SldPan = new CustomSlider();
			SldTrack = new CustomSlider();
			LabelTitle = new ScrollLabel();
			LabelTime = new ScrollLabel();
			SeekiTimer = new System.Windows.Forms.Timer(components);
			Spectrum = new SpectrumAnalyzer();
			BtnCD = new System.Windows.Forms.Button();
			contextMenu = new System.Windows.Forms.ContextMenuStrip(components);
			notifyIcon = new System.Windows.Forms.NotifyIcon(components);
			((System.ComponentModel.ISupportInitialize)Spectrum).BeginInit();
			SuspendLayout();
			// 
			// BtnPlay
			// 
			BtnPlay.AutoSize = true;
			BtnPlay.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			BtnPlay.FlatAppearance.BorderSize = 0;
			BtnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnPlay.Location = new System.Drawing.Point(174, 63);
			BtnPlay.Name = "BtnPlay";
			BtnPlay.Size = new System.Drawing.Size(75, 27);
			BtnPlay.TabIndex = 0;
			BtnPlay.UseVisualStyleBackColor = false;
			BtnPlay.Click += BtnPlay_Click;
			// 
			// BtnStop
			// 
			BtnStop.AutoSize = true;
			BtnStop.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnStop.FlatAppearance.BorderSize = 0;
			BtnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnStop.Location = new System.Drawing.Point(255, 63);
			BtnStop.Name = "BtnStop";
			BtnStop.Size = new System.Drawing.Size(75, 27);
			BtnStop.TabIndex = 0;
			BtnStop.UseVisualStyleBackColor = false;
			BtnStop.Click += BtnStop_Click;
			// 
			// Timer
			// 
			Timer.Enabled = true;
			Timer.Interval = 60;
			Timer.Tick += PlayerTimer_Tick;
			// 
			// BtnOpen
			// 
			BtnOpen.AutoSize = true;
			BtnOpen.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnOpen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			BtnOpen.FlatAppearance.BorderSize = 0;
			BtnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnOpen.Location = new System.Drawing.Point(93, 63);
			BtnOpen.Name = "BtnOpen";
			BtnOpen.Size = new System.Drawing.Size(75, 27);
			BtnOpen.TabIndex = 2;
			BtnOpen.UseVisualStyleBackColor = false;
			BtnOpen.Click += BtnOpenFile_Click;
			// 
			// BtnClose
			// 
			BtnClose.AutoSize = true;
			BtnClose.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			BtnClose.FlatAppearance.BorderSize = 0;
			BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnClose.Location = new System.Drawing.Point(12, 63);
			BtnClose.Name = "BtnClose";
			BtnClose.Size = new System.Drawing.Size(75, 27);
			BtnClose.TabIndex = 3;
			BtnClose.UseVisualStyleBackColor = false;
			BtnClose.Click += BtnClose_Click;
			// 
			// BtnMinisize
			// 
			BtnMinisize.AutoSize = true;
			BtnMinisize.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnMinisize.FlatAppearance.BorderSize = 0;
			BtnMinisize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnMinisize.Location = new System.Drawing.Point(93, 161);
			BtnMinisize.Name = "BtnMinisize";
			BtnMinisize.Size = new System.Drawing.Size(75, 27);
			BtnMinisize.TabIndex = 5;
			BtnMinisize.UseVisualStyleBackColor = false;
			BtnMinisize.Click += BtnMinisize_Click;
			// 
			// BtnPlaylist
			// 
			BtnPlaylist.AutoSize = true;
			BtnPlaylist.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnPlaylist.FlatAppearance.BorderSize = 0;
			BtnPlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnPlaylist.Location = new System.Drawing.Point(12, 161);
			BtnPlaylist.Name = "BtnPlaylist";
			BtnPlaylist.Size = new System.Drawing.Size(75, 27);
			BtnPlaylist.TabIndex = 6;
			BtnPlaylist.UseVisualStyleBackColor = false;
			BtnPlaylist.Click += BtnPlaylist_Click;
			// 
			// BtnSetting
			// 
			BtnSetting.AutoSize = true;
			BtnSetting.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnSetting.FlatAppearance.BorderSize = 0;
			BtnSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnSetting.Location = new System.Drawing.Point(255, 128);
			BtnSetting.Name = "BtnSetting";
			BtnSetting.Size = new System.Drawing.Size(75, 27);
			BtnSetting.TabIndex = 7;
			BtnSetting.UseVisualStyleBackColor = false;
			BtnSetting.Click += BtnSetting_Click;
			// 
			// BtnLoop
			// 
			BtnLoop.AutoSize = true;
			BtnLoop.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnLoop.FlatAppearance.BorderSize = 0;
			BtnLoop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnLoop.Location = new System.Drawing.Point(174, 128);
			BtnLoop.Name = "BtnLoop";
			BtnLoop.Size = new System.Drawing.Size(75, 27);
			BtnLoop.TabIndex = 8;
			BtnLoop.UseVisualStyleBackColor = false;
			BtnLoop.Click += BtnLoop_Click;
			BtnLoop.MouseUp += BtnLoop_MouseUp;
			// 
			// BtnRandom
			// 
			BtnRandom.AutoSize = true;
			BtnRandom.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnRandom.FlatAppearance.BorderSize = 0;
			BtnRandom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnRandom.Location = new System.Drawing.Point(93, 129);
			BtnRandom.Name = "BtnRandom";
			BtnRandom.Size = new System.Drawing.Size(75, 27);
			BtnRandom.TabIndex = 9;
			BtnRandom.UseVisualStyleBackColor = false;
			BtnRandom.Click += BtnRandom_Click;
			BtnRandom.MouseUp += BtnRandom_MouseUp;
			// 
			// BtnNext
			// 
			BtnNext.AutoSize = true;
			BtnNext.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnNext.FlatAppearance.BorderSize = 0;
			BtnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnNext.Location = new System.Drawing.Point(12, 128);
			BtnNext.Name = "BtnNext";
			BtnNext.Size = new System.Drawing.Size(75, 27);
			BtnNext.TabIndex = 10;
			BtnNext.UseVisualStyleBackColor = false;
			BtnNext.Click += BtnNext_Click;
			// 
			// BtnSeekForward
			// 
			BtnSeekForward.AutoSize = true;
			BtnSeekForward.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnSeekForward.FlatAppearance.BorderSize = 0;
			BtnSeekForward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnSeekForward.Location = new System.Drawing.Point(255, 95);
			BtnSeekForward.Name = "BtnSeekForward";
			BtnSeekForward.Size = new System.Drawing.Size(75, 27);
			BtnSeekForward.TabIndex = 11;
			BtnSeekForward.UseVisualStyleBackColor = false;
			BtnSeekForward.Click += BtnSeekForward_Click;
			// 
			// BtnPause
			// 
			BtnPause.AutoSize = true;
			BtnPause.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnPause.FlatAppearance.BorderSize = 0;
			BtnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnPause.Location = new System.Drawing.Point(174, 96);
			BtnPause.Name = "BtnPause";
			BtnPause.Size = new System.Drawing.Size(75, 27);
			BtnPause.TabIndex = 12;
			BtnPause.UseVisualStyleBackColor = false;
			BtnPause.Click += BtnPause_Click;
			// 
			// BtnSeekBack
			// 
			BtnSeekBack.AutoSize = true;
			BtnSeekBack.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnSeekBack.FlatAppearance.BorderSize = 0;
			BtnSeekBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnSeekBack.Location = new System.Drawing.Point(93, 96);
			BtnSeekBack.Name = "BtnSeekBack";
			BtnSeekBack.Size = new System.Drawing.Size(75, 27);
			BtnSeekBack.TabIndex = 13;
			BtnSeekBack.UseVisualStyleBackColor = false;
			BtnSeekBack.Click += BtnSeekBack_Click;
			// 
			// BtnBack
			// 
			BtnBack.AutoSize = true;
			BtnBack.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnBack.FlatAppearance.BorderSize = 0;
			BtnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnBack.Location = new System.Drawing.Point(12, 95);
			BtnBack.Name = "BtnBack";
			BtnBack.Size = new System.Drawing.Size(75, 27);
			BtnBack.TabIndex = 14;
			BtnBack.UseVisualStyleBackColor = false;
			BtnBack.Click += BtnBack_Click;
			// 
			// SldVolume
			// 
			SldVolume.BackColor = System.Drawing.Color.Transparent;
			SldVolume.Location = new System.Drawing.Point(149, 12);
			SldVolume.Maximum = 0;
			SldVolume.Minimum = 0;
			SldVolume.Name = "SldVolume";
			SldVolume.Orientation = System.Windows.Forms.Orientation.Horizontal;
			SldVolume.Size = new System.Drawing.Size(100, 42);
			SldVolume.SliderImage = null;
			SldVolume.TabIndex = 15;
			SldVolume.Value = 0;
			SldVolume.SliderMoved += SldVolume_SliderMoved;
			SldVolume.SliderMoving += SldVolume_SliderMoving;
			// 
			// SldPan
			// 
			SldPan.BackColor = System.Drawing.Color.Transparent;
			SldPan.Location = new System.Drawing.Point(257, 12);
			SldPan.Maximum = 0;
			SldPan.Minimum = 0;
			SldPan.Name = "SldPan";
			SldPan.Orientation = System.Windows.Forms.Orientation.Horizontal;
			SldPan.Size = new System.Drawing.Size(100, 42);
			SldPan.SliderImage = null;
			SldPan.TabIndex = 16;
			SldPan.Value = 0;
			SldPan.SliderMoved += SldPan_SliderMoved;
			SldPan.SliderMoving += SldPan_SliderMoving;
			// 
			// SldTrack
			// 
			SldTrack.BackColor = System.Drawing.Color.Transparent;
			SldTrack.Location = new System.Drawing.Point(363, 12);
			SldTrack.Maximum = 0;
			SldTrack.Minimum = 0;
			SldTrack.Name = "SldTrack";
			SldTrack.Orientation = System.Windows.Forms.Orientation.Horizontal;
			SldTrack.Size = new System.Drawing.Size(100, 42);
			SldTrack.SliderImage = null;
			SldTrack.TabIndex = 17;
			SldTrack.Value = 0;
			SldTrack.ValueChanged += SldTrack_ValueChanged;
			SldTrack.SliderMoved += SldTrack_SliderMoved;
			SldTrack.SliderMoving += SldTrack_SliderMoving;
			// 
			// LabelTitle
			// 
			LabelTitle.BackColor = System.Drawing.SystemColors.Window;
			LabelTitle.Location = new System.Drawing.Point(257, 161);
			LabelTitle.Name = "LabelTitle";
			LabelTitle.ScrollEnable = true;
			LabelTitle.Size = new System.Drawing.Size(73, 27);
			LabelTitle.TabIndex = 18;
			// 
			// LabelTime
			// 
			LabelTime.BackColor = System.Drawing.SystemColors.Window;
			LabelTime.Location = new System.Drawing.Point(174, 161);
			LabelTime.Name = "LabelTime";
			LabelTime.ScrollEnable = false;
			LabelTime.Size = new System.Drawing.Size(75, 27);
			LabelTime.TabIndex = 19;
			// 
			// SeekiTimer
			// 
			SeekiTimer.Enabled = true;
			SeekiTimer.Tick += SeekiTimer_Tick;
			// 
			// Spectrum
			// 
			Spectrum.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			Spectrum.BitmapBackground = null;
			Spectrum.BitmapSpectrum = null;
			Spectrum.Location = new System.Drawing.Point(12, 12);
			Spectrum.mFFT = null;
			Spectrum.Mode = 0;
			Spectrum.mWaveL = null;
			Spectrum.mWaveR = null;
			Spectrum.Name = "Spectrum";
			Spectrum.Size = new System.Drawing.Size(128, 54);
			Spectrum.SnowBlockEnabled = true;
			Spectrum.SnowFallSpeed = 0.72F;
			Spectrum.TabIndex = 20;
			Spectrum.TabStop = false;
			Spectrum.WaveColorL = System.Drawing.Color.Lime;
			Spectrum.WaveColorR = System.Drawing.Color.Cyan;
			Spectrum.Click += Spectrum_Click;
			// 
			// BtnCD
			// 
			BtnCD.AutoSize = true;
			BtnCD.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnCD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			BtnCD.FlatAppearance.BorderSize = 0;
			BtnCD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnCD.Location = new System.Drawing.Point(176, 161);
			BtnCD.Name = "BtnCD";
			BtnCD.Size = new System.Drawing.Size(75, 27);
			BtnCD.TabIndex = 21;
			BtnCD.UseVisualStyleBackColor = false;
			BtnCD.Click += BtnCD_Click;
			// 
			// contextMenu
			// 
			contextMenu.Name = "contextMenu";
			contextMenu.Size = new System.Drawing.Size(61, 4);
			// 
			// notifyIcon
			// 
			notifyIcon.Text = "MediaPlayer X-Ark";
			notifyIcon.Visible = true;
			// 
			// MainForm
			// 
			AllowDrop = true;
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			BackColor = System.Drawing.SystemColors.Window;
			BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			ClientSize = new System.Drawing.Size(520, 227);
			ContextMenuStrip = contextMenu;
			ControlBox = false;
			Controls.Add(BtnPlaylist);
			Controls.Add(BtnCD);
			Controls.Add(Spectrum);
			Controls.Add(SldVolume);
			Controls.Add(SldPan);
			Controls.Add(SldTrack);
			Controls.Add(LabelTime);
			Controls.Add(LabelTitle);
			Controls.Add(BtnBack);
			Controls.Add(BtnSeekBack);
			Controls.Add(BtnPause);
			Controls.Add(BtnSeekForward);
			Controls.Add(BtnNext);
			Controls.Add(BtnRandom);
			Controls.Add(BtnLoop);
			Controls.Add(BtnSetting);
			Controls.Add(BtnMinisize);
			Controls.Add(BtnClose);
			Controls.Add(BtnOpen);
			Controls.Add(BtnStop);
			Controls.Add(BtnPlay);
			DoubleBuffered = true;
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			Name = "MainForm";
			Text = "Media Player X-Ark Zwei";
			FormClosed += MainForm_FormClosed;
			Load += MainForm_Load;
			DragDrop += MainForm_DragDrop;
			DragEnter += MainForm_DragEnter;
			KeyDown += MainForm_KeyDown;
			KeyUp += MainForm_KeyUp;
			MouseDown += MainForm_MouseDown;
			MouseMove += MainForm_MouseMove;
			((System.ComponentModel.ISupportInitialize)Spectrum).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.Button BtnPlay;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.Button BtnOpen;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Button BtnMinisize;
        private System.Windows.Forms.Button BtnPlaylist;
        private System.Windows.Forms.Button BtnSetting;
        private System.Windows.Forms.Button BtnLoop;
        private System.Windows.Forms.Button BtnRandom;
        private System.Windows.Forms.Button BtnNext;
        private System.Windows.Forms.Button BtnSeekForward;
        private System.Windows.Forms.Button BtnPause;
        private System.Windows.Forms.Button BtnSeekBack;
        private System.Windows.Forms.Button BtnBack;
        private CustomSlider SldVolume;
        private CustomSlider SldPan;
        private CustomSlider SldTrack;
        private ScrollLabel LabelTitle;
        private ScrollLabel LabelTime;
        private System.Windows.Forms.Timer SeekiTimer;
        public SpectrumAnalyzer Spectrum;
        private System.Windows.Forms.Button BtnCD;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}

