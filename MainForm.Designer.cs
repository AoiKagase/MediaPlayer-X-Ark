
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
            this.components = new System.ComponentModel.Container();
            this.BtnPlay = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.Spectrum = new System.Windows.Forms.PictureBox();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.BtnOpen = new System.Windows.Forms.Button();
            this.BtnClose = new System.Windows.Forms.Button();
            this.BtnMinisize = new System.Windows.Forms.Button();
            this.BtnPlaylist = new System.Windows.Forms.Button();
            this.BtnSetting = new System.Windows.Forms.Button();
            this.BtnLoop = new System.Windows.Forms.Button();
            this.BtnRandom = new System.Windows.Forms.Button();
            this.BtnNext = new System.Windows.Forms.Button();
            this.BtnSeekForward = new System.Windows.Forms.Button();
            this.BtnPause = new System.Windows.Forms.Button();
            this.BtnSeekBack = new System.Windows.Forms.Button();
            this.BtnBack = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Spectrum)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnPlay
            // 
            this.BtnPlay.AutoSize = true;
            this.BtnPlay.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnPlay.FlatAppearance.BorderSize = 0;
            this.BtnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPlay.Location = new System.Drawing.Point(174, 63);
            this.BtnPlay.Name = "BtnPlay";
            this.BtnPlay.Size = new System.Drawing.Size(75, 27);
            this.BtnPlay.TabIndex = 0;
            this.BtnPlay.UseVisualStyleBackColor = false;
            this.BtnPlay.Click += new System.EventHandler(this.BtnPlay_Click);
            this.BtnPlay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnPlay_MouseDown);
            this.BtnPlay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnPlay_MouseUp);
            // 
            // BtnStop
            // 
            this.BtnStop.AutoSize = true;
            this.BtnStop.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnStop.FlatAppearance.BorderSize = 0;
            this.BtnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStop.Location = new System.Drawing.Point(255, 63);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(75, 27);
            this.BtnStop.TabIndex = 0;
            this.BtnStop.UseVisualStyleBackColor = false;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            this.BtnStop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnStop_MouseDown);
            this.BtnStop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnStop_MouseUp);
            // 
            // Spectrum
            // 
            this.Spectrum.Location = new System.Drawing.Point(12, 12);
            this.Spectrum.Name = "Spectrum";
            this.Spectrum.Size = new System.Drawing.Size(104, 42);
            this.Spectrum.TabIndex = 1;
            this.Spectrum.TabStop = false;
            // 
            // Timer
            // 
            this.Timer.Enabled = true;
            this.Timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.FileName = "openFileDialog1";
            // 
            // BtnOpen
            // 
            this.BtnOpen.AutoSize = true;
            this.BtnOpen.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnOpen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnOpen.FlatAppearance.BorderSize = 0;
            this.BtnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOpen.Location = new System.Drawing.Point(93, 63);
            this.BtnOpen.Name = "BtnOpen";
            this.BtnOpen.Size = new System.Drawing.Size(75, 27);
            this.BtnOpen.TabIndex = 2;
            this.BtnOpen.UseVisualStyleBackColor = false;
            this.BtnOpen.Click += new System.EventHandler(this.BtnOpenFile_Click);
            this.BtnOpen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnOpen_MouseDown);
            this.BtnOpen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnOpen_MouseUp);
            // 
            // BtnClose
            // 
            this.BtnClose.AutoSize = true;
            this.BtnClose.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnClose.FlatAppearance.BorderSize = 0;
            this.BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClose.Location = new System.Drawing.Point(12, 63);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 27);
            this.BtnClose.TabIndex = 3;
            this.BtnClose.UseVisualStyleBackColor = false;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            this.BtnClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnClose_MouseDown);
            this.BtnClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnClose_MouseUp);
            // 
            // BtnMinisize
            // 
            this.BtnMinisize.AutoSize = true;
            this.BtnMinisize.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnMinisize.FlatAppearance.BorderSize = 0;
            this.BtnMinisize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMinisize.Location = new System.Drawing.Point(93, 161);
            this.BtnMinisize.Name = "BtnMinisize";
            this.BtnMinisize.Size = new System.Drawing.Size(75, 27);
            this.BtnMinisize.TabIndex = 5;
            this.BtnMinisize.UseVisualStyleBackColor = false;
            this.BtnMinisize.Click += new System.EventHandler(this.BtnMinisize_Click);
            this.BtnMinisize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnMinisize_MouseDown);
            this.BtnMinisize.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnMinisize_MouseUp);
            // 
            // BtnPlaylist
            // 
            this.BtnPlaylist.AutoSize = true;
            this.BtnPlaylist.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnPlaylist.FlatAppearance.BorderSize = 0;
            this.BtnPlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPlaylist.Location = new System.Drawing.Point(12, 161);
            this.BtnPlaylist.Name = "BtnPlaylist";
            this.BtnPlaylist.Size = new System.Drawing.Size(75, 27);
            this.BtnPlaylist.TabIndex = 6;
            this.BtnPlaylist.UseVisualStyleBackColor = false;
            this.BtnPlaylist.Click += new System.EventHandler(this.BtnPlaylist_Click);
            this.BtnPlaylist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnPlaylist_MouseDown);
            this.BtnPlaylist.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnPlaylist_MouseUp);
            // 
            // BtnSetting
            // 
            this.BtnSetting.AutoSize = true;
            this.BtnSetting.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnSetting.FlatAppearance.BorderSize = 0;
            this.BtnSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSetting.Location = new System.Drawing.Point(255, 128);
            this.BtnSetting.Name = "BtnSetting";
            this.BtnSetting.Size = new System.Drawing.Size(75, 27);
            this.BtnSetting.TabIndex = 7;
            this.BtnSetting.UseVisualStyleBackColor = false;
            this.BtnSetting.Click += new System.EventHandler(this.BtnSetting_Click);
            this.BtnSetting.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnSetting_MouseDown);
            this.BtnSetting.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnSetting_MouseUp);
            // 
            // BtnLoop
            // 
            this.BtnLoop.AutoSize = true;
            this.BtnLoop.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnLoop.FlatAppearance.BorderSize = 0;
            this.BtnLoop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnLoop.Location = new System.Drawing.Point(174, 128);
            this.BtnLoop.Name = "BtnLoop";
            this.BtnLoop.Size = new System.Drawing.Size(75, 27);
            this.BtnLoop.TabIndex = 8;
            this.BtnLoop.UseVisualStyleBackColor = false;
            this.BtnLoop.Click += new System.EventHandler(this.BtnLoop_Click);
            this.BtnLoop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnLoop_MouseDown);
            this.BtnLoop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnLoop_MouseUp);
            // 
            // BtnRandom
            // 
            this.BtnRandom.AutoSize = true;
            this.BtnRandom.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnRandom.FlatAppearance.BorderSize = 0;
            this.BtnRandom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRandom.Location = new System.Drawing.Point(93, 129);
            this.BtnRandom.Name = "BtnRandom";
            this.BtnRandom.Size = new System.Drawing.Size(75, 27);
            this.BtnRandom.TabIndex = 9;
            this.BtnRandom.UseVisualStyleBackColor = false;
            this.BtnRandom.Click += new System.EventHandler(this.BtnRandom_Click);
            this.BtnRandom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnRandom_MouseDown);
            this.BtnRandom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnRandom_MouseUp);
            // 
            // BtnNext
            // 
            this.BtnNext.AutoSize = true;
            this.BtnNext.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnNext.FlatAppearance.BorderSize = 0;
            this.BtnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnNext.Location = new System.Drawing.Point(12, 128);
            this.BtnNext.Name = "BtnNext";
            this.BtnNext.Size = new System.Drawing.Size(75, 27);
            this.BtnNext.TabIndex = 10;
            this.BtnNext.UseVisualStyleBackColor = false;
            this.BtnNext.Click += new System.EventHandler(this.BtnNext_Click);
            this.BtnNext.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnNext_MouseDown);
            this.BtnNext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnNext_MouseUp);
            // 
            // BtnSeekForward
            // 
            this.BtnSeekForward.AutoSize = true;
            this.BtnSeekForward.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnSeekForward.FlatAppearance.BorderSize = 0;
            this.BtnSeekForward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSeekForward.Location = new System.Drawing.Point(255, 95);
            this.BtnSeekForward.Name = "BtnSeekForward";
            this.BtnSeekForward.Size = new System.Drawing.Size(75, 27);
            this.BtnSeekForward.TabIndex = 11;
            this.BtnSeekForward.UseVisualStyleBackColor = false;
            this.BtnSeekForward.Click += new System.EventHandler(this.BtnSeekForward_Click);
            this.BtnSeekForward.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnSeekForward_MouseDown);
            this.BtnSeekForward.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnSeekForward_MouseUp);
            // 
            // BtnPause
            // 
            this.BtnPause.AutoSize = true;
            this.BtnPause.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnPause.FlatAppearance.BorderSize = 0;
            this.BtnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPause.Location = new System.Drawing.Point(174, 96);
            this.BtnPause.Name = "BtnPause";
            this.BtnPause.Size = new System.Drawing.Size(75, 27);
            this.BtnPause.TabIndex = 12;
            this.BtnPause.UseVisualStyleBackColor = false;
            this.BtnPause.Click += new System.EventHandler(this.BtnPause_Click);
            this.BtnPause.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnPause_MouseDown);
            this.BtnPause.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnPause_MouseUp);
            // 
            // BtnSeekBack
            // 
            this.BtnSeekBack.AutoSize = true;
            this.BtnSeekBack.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnSeekBack.FlatAppearance.BorderSize = 0;
            this.BtnSeekBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSeekBack.Location = new System.Drawing.Point(93, 96);
            this.BtnSeekBack.Name = "BtnSeekBack";
            this.BtnSeekBack.Size = new System.Drawing.Size(75, 27);
            this.BtnSeekBack.TabIndex = 13;
            this.BtnSeekBack.UseVisualStyleBackColor = false;
            this.BtnSeekBack.Click += new System.EventHandler(this.BtnSeekBack_Click);
            this.BtnSeekBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnSeekBack_MouseDown);
            this.BtnSeekBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnSeekBack_MouseUp);
            // 
            // BtnBack
            // 
            this.BtnBack.AutoSize = true;
            this.BtnBack.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnBack.FlatAppearance.BorderSize = 0;
            this.BtnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBack.Location = new System.Drawing.Point(12, 95);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(75, 27);
            this.BtnBack.TabIndex = 14;
            this.BtnBack.UseVisualStyleBackColor = false;
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            this.BtnBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnBack_MouseDown);
            this.BtnBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnBack_MouseUp);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(500, 500);
            this.ControlBox = false;
            this.Controls.Add(this.BtnBack);
            this.Controls.Add(this.BtnSeekBack);
            this.Controls.Add(this.BtnPause);
            this.Controls.Add(this.BtnSeekForward);
            this.Controls.Add(this.BtnNext);
            this.Controls.Add(this.BtnRandom);
            this.Controls.Add(this.BtnLoop);
            this.Controls.Add(this.BtnSetting);
            this.Controls.Add(this.BtnPlaylist);
            this.Controls.Add(this.BtnMinisize);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnOpen);
            this.Controls.Add(this.Spectrum);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnPlay);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.Spectrum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnPlay;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.PictureBox Spectrum;
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
    }
}

