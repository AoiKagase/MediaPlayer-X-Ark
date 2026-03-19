
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
                player = null;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
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
            Spectrum = new SpectrumBox();
            BtnCD = new System.Windows.Forms.Button();
            contextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            menuOpen = new System.Windows.Forms.ToolStripMenuItem();
            menuUrlOpen = new System.Windows.Forms.ToolStripMenuItem();
            menuPlay = new System.Windows.Forms.ToolStripMenuItem();
            menuPause = new System.Windows.Forms.ToolStripMenuItem();
            menuStop = new System.Windows.Forms.ToolStripMenuItem();
            menuBack = new System.Windows.Forms.ToolStripMenuItem();
            menuForward = new System.Windows.Forms.ToolStripMenuItem();
            menuPlayMode = new System.Windows.Forms.ToolStripMenuItem();
            menuPlayModeNormal = new System.Windows.Forms.ToolStripMenuItem();
            menuPlayModeRandom = new System.Windows.Forms.ToolStripMenuItem();
            menuPlayModeRepeat = new System.Windows.Forms.ToolStripMenuItem();
            menuPlayModeLoop = new System.Windows.Forms.ToolStripMenuItem();
            menuPlayList = new System.Windows.Forms.ToolStripMenuItem();
            menuOption = new System.Windows.Forms.ToolStripMenuItem();
            menuEffects = new System.Windows.Forms.ToolStripMenuItem();
            menuEqualizer = new System.Windows.Forms.ToolStripMenuItem();
            menuExtensions = new System.Windows.Forms.ToolStripMenuItem();
            menuSkinSelect = new System.Windows.Forms.ToolStripMenuItem();
            menuAutoUpdateCheck = new System.Windows.Forms.ToolStripMenuItem();
            menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            menuMinimize = new System.Windows.Forms.ToolStripMenuItem();
            menuExit = new System.Windows.Forms.ToolStripMenuItem();
            notifyIcon = new System.Windows.Forms.NotifyIcon(components);
            ((System.ComponentModel.ISupportInitialize)Spectrum).BeginInit();
            contextMenu.SuspendLayout();
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
            BtnPlay.MouseDown += BtnPlay_MouseDown;
            BtnPlay.MouseUp += BtnPlay_MouseUp;
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
            BtnStop.MouseDown += BtnStop_MouseDown;
            BtnStop.MouseUp += BtnStop_MouseUp;
            // 
            // Timer
            // 
            Timer.Enabled = true;
            Timer.Tick += PlayerTimer_Tick;
            // 
            // OpenFileDialog
            // 
            OpenFileDialog.FileName = "openFileDialog1";
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
            BtnOpen.MouseDown += BtnOpen_MouseDown;
            BtnOpen.MouseUp += BtnOpen_MouseUp;
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
            BtnClose.MouseDown += BtnClose_MouseDown;
            BtnClose.MouseUp += BtnClose_MouseUp;
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
            BtnMinisize.MouseDown += BtnMinisize_MouseDown;
            BtnMinisize.MouseUp += BtnMinisize_MouseUp;
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
            BtnPlaylist.MouseDown += BtnPlaylist_MouseDown;
            BtnPlaylist.MouseUp += BtnPlaylist_MouseUp;
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
            BtnSetting.MouseDown += BtnSetting_MouseDown;
            BtnSetting.MouseUp += BtnSetting_MouseUp;
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
            BtnLoop.MouseDown += BtnLoop_MouseDown;
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
            BtnRandom.MouseDown += BtnRandom_MouseDown;
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
            BtnNext.MouseDown += BtnNext_MouseDown;
            BtnNext.MouseUp += BtnNext_MouseUp;
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
            BtnSeekForward.MouseDown += BtnSeekForward_MouseDown;
            BtnSeekForward.MouseUp += BtnSeekForward_MouseUp;
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
            BtnPause.MouseDown += BtnPause_MouseDown;
            BtnPause.MouseUp += BtnPause_MouseUp;
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
            BtnSeekBack.MouseDown += BtnSeekBack_MouseDown;
            BtnSeekBack.MouseUp += BtnSeekBack_MouseUp;
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
            BtnBack.MouseDown += BtnBack_MouseDown;
            BtnBack.MouseUp += BtnBack_MouseUp;
            // 
            // SldVolume
            // 
            SldVolume.BackColor = System.Drawing.Color.Transparent;
            SldVolume.Location = new System.Drawing.Point(138, 12);
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
            SldPan.Location = new System.Drawing.Point(244, 12);
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
            SldTrack.Location = new System.Drawing.Point(350, 12);
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
            Spectrum.Location = new System.Drawing.Point(138, 275);
            Spectrum.mFFT = null;
            Spectrum.Mode = 0;
            Spectrum.mWaveL = null;
            Spectrum.mWaveR = null;
            Spectrum.Name = "Spectrum";
            Spectrum.Size = new System.Drawing.Size(128, 54);
            Spectrum.TabIndex = 20;
            Spectrum.TabStop = false;
            Spectrum.Click += Spectrum_Click;
            // 
            // BtnCD
            // 
            BtnCD.AutoSize = true;
            BtnCD.BackColor = System.Drawing.SystemColors.HotTrack;
            BtnCD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            BtnCD.FlatAppearance.BorderSize = 0;
            BtnCD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnCD.Location = new System.Drawing.Point(12, 194);
            BtnCD.Name = "BtnCD";
            BtnCD.Size = new System.Drawing.Size(75, 27);
            BtnCD.TabIndex = 21;
            BtnCD.UseVisualStyleBackColor = false;
            BtnCD.Click += BtnCD_Click;
            BtnCD.MouseDown += BtnCD_MouseDown;
            BtnCD.MouseUp += BtnCD_MouseUp;
            // 
            // contextMenu
            // 
            contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { menuOpen, menuUrlOpen, menuPlay, menuPause, menuStop, menuBack, menuForward, menuPlayMode, menuPlayList, menuOption, menuEffects, menuEqualizer, menuExtensions, menuSkinSelect, menuAutoUpdateCheck, menuAbout, menuHelp, menuMinimize, menuExit });
            contextMenu.Name = "contextMenu";
            contextMenu.Size = new System.Drawing.Size(193, 458);
            // 
            // menuOpen
            // 
            menuOpen.Name = "menuOpen";
            menuOpen.Size = new System.Drawing.Size(192, 22);
            menuOpen.Text = "Open(&O)";
            // 
            // menuUrlOpen
            // 
            menuUrlOpen.Name = "menuUrlOpen";
            menuUrlOpen.Size = new System.Drawing.Size(192, 22);
            menuUrlOpen.Text = "URL Open(&R)";
            // 
            // menuPlay
            // 
            menuPlay.Name = "menuPlay";
            menuPlay.Size = new System.Drawing.Size(192, 22);
            menuPlay.Text = "Play(&P)";
            // 
            // menuPause
            // 
            menuPause.Name = "menuPause";
            menuPause.Size = new System.Drawing.Size(192, 22);
            menuPause.Text = "Pause(&H)";
            // 
            // menuStop
            // 
            menuStop.Name = "menuStop";
            menuStop.Size = new System.Drawing.Size(192, 22);
            menuStop.Text = "Stop(&S)";
            // 
            // menuBack
            // 
            menuBack.Name = "menuBack";
            menuBack.Size = new System.Drawing.Size(192, 22);
            menuBack.Text = "Back(&B)";
            // 
            // menuForward
            // 
            menuForward.Name = "menuForward";
            menuForward.Size = new System.Drawing.Size(192, 22);
            menuForward.Text = "Forward(&F)";
            // 
            // menuPlayMode
            // 
            menuPlayMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { menuPlayModeNormal, menuPlayModeRandom, menuPlayModeRepeat, menuPlayModeLoop });
            menuPlayMode.Name = "menuPlayMode";
            menuPlayMode.Size = new System.Drawing.Size(192, 22);
            menuPlayMode.Text = "PlayMode";
            // 
            // menuPlayModeNormal
            // 
            menuPlayModeNormal.Name = "menuPlayModeNormal";
            menuPlayModeNormal.Size = new System.Drawing.Size(118, 22);
            menuPlayModeNormal.Text = "Normal";
            // 
            // menuPlayModeRandom
            // 
            menuPlayModeRandom.Name = "menuPlayModeRandom";
            menuPlayModeRandom.Size = new System.Drawing.Size(118, 22);
            menuPlayModeRandom.Text = "Random";
            // 
            // menuPlayModeRepeat
            // 
            menuPlayModeRepeat.Name = "menuPlayModeRepeat";
            menuPlayModeRepeat.Size = new System.Drawing.Size(118, 22);
            menuPlayModeRepeat.Text = "Repeat";
            // 
            // menuPlayModeLoop
            // 
            menuPlayModeLoop.Name = "menuPlayModeLoop";
            menuPlayModeLoop.Size = new System.Drawing.Size(118, 22);
            menuPlayModeLoop.Text = "Loop";
            // 
            // menuPlayList
            // 
            menuPlayList.Name = "menuPlayList";
            menuPlayList.Size = new System.Drawing.Size(192, 22);
            menuPlayList.Text = "PlayList(&L)";
            // 
            // menuOption
            // 
            menuOption.Name = "menuOption";
            menuOption.Size = new System.Drawing.Size(192, 22);
            menuOption.Text = "Option(&T)";
            // 
            // menuEffects
            // 
            menuEffects.Name = "menuEffects";
            menuEffects.Size = new System.Drawing.Size(192, 22);
            menuEffects.Text = "Effects(&E)";
            // 
            // menuEqualizer
            // 
            menuEqualizer.Name = "menuEqualizer";
            menuEqualizer.Size = new System.Drawing.Size(192, 22);
            menuEqualizer.Text = "Equalizer(&Q)";
            // 
            // menuExtensions
            // 
            menuExtensions.Name = "menuExtensions";
            menuExtensions.Size = new System.Drawing.Size(192, 22);
            menuExtensions.Text = "Extensions(&D)";
            // 
            // menuSkinSelect
            // 
            menuSkinSelect.Name = "menuSkinSelect";
            menuSkinSelect.Size = new System.Drawing.Size(192, 22);
            menuSkinSelect.Text = "Skin Select(&A)";
            // 
            // menuAutoUpdateCheck
            // 
            menuAutoUpdateCheck.Name = "menuAutoUpdateCheck";
            menuAutoUpdateCheck.Size = new System.Drawing.Size(192, 22);
            menuAutoUpdateCheck.Text = "Auto Update Check(&U)";
            // 
            // menuAbout
            // 
            menuAbout.Name = "menuAbout";
            menuAbout.Size = new System.Drawing.Size(192, 22);
            menuAbout.Text = "About(&C)";
            // 
            // menuHelp
            // 
            menuHelp.Name = "menuHelp";
            menuHelp.Size = new System.Drawing.Size(192, 22);
            menuHelp.Text = "Help(&V)";
            // 
            // menuMinimize
            // 
            menuMinimize.Name = "menuMinimize";
            menuMinimize.Size = new System.Drawing.Size(192, 22);
            menuMinimize.Text = "Minimize(&X)";
            // 
            // menuExit
            // 
            menuExit.Name = "menuExit";
            menuExit.Size = new System.Drawing.Size(192, 22);
            menuExit.Text = "Exit(&Z)";
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
            ClientSize = new System.Drawing.Size(500, 500);
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
            MouseDown += MainForm_MouseDown;
            MouseMove += MainForm_MouseMove;
            ((System.ComponentModel.ISupportInitialize)Spectrum).EndInit();
            contextMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuOpen;
        private System.Windows.Forms.ToolStripMenuItem menuUrlOpen;
        private System.Windows.Forms.ToolStripMenuItem menuPlay;
        private System.Windows.Forms.ToolStripMenuItem menuPause;
        private System.Windows.Forms.ToolStripMenuItem menuStop;
        private System.Windows.Forms.ToolStripMenuItem menuBack;
        private System.Windows.Forms.ToolStripMenuItem menuForward;
        private System.Windows.Forms.ToolStripMenuItem menuPlayMode;
        private System.Windows.Forms.ToolStripMenuItem menuPlayModeNormal;
        private System.Windows.Forms.ToolStripMenuItem menuPlayModeRandom;
        private System.Windows.Forms.ToolStripMenuItem menuPlayModeRepeat;
        private System.Windows.Forms.ToolStripMenuItem menuPlayModeLoop;
        private System.Windows.Forms.ToolStripMenuItem menuPlayList;
        private System.Windows.Forms.ToolStripMenuItem menuOption;
        private System.Windows.Forms.ToolStripMenuItem menuEffects;
        private System.Windows.Forms.ToolStripMenuItem menuEqualizer;
        private System.Windows.Forms.ToolStripMenuItem menuExtensions;
        private System.Windows.Forms.ToolStripMenuItem menuSkinSelect;
        private System.Windows.Forms.ToolStripMenuItem menuAutoUpdateCheck;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuMinimize;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
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
        private SpectrumBox Spectrum;
        private System.Windows.Forms.Button BtnCD;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}

