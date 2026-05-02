namespace MediaPlayer_X_Ark.Forms
{
    partial class MiniPlayerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		//protected override void Dispose(bool disposing)
		//{
		//    if (disposing && (components != null))
		//    {
		//        components.Dispose();
		//    }
		//    base.Dispose(disposing);
		//}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			BtnBack = new System.Windows.Forms.Button();
			BtnPlay = new System.Windows.Forms.Button();
			BtnPause = new System.Windows.Forms.Button();
			BtnStop = new System.Windows.Forms.Button();
			BtnNext = new System.Windows.Forms.Button();
			BtnClose = new System.Windows.Forms.Button();
			LabelTitle = new ScrollLabel();
			SldTrack = new CustomSlider();
			SldVolume = new CustomSlider();
			contextMenu = new System.Windows.Forms.ContextMenuStrip(components);
			MiniTimer = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			// 
			// BtnBack
			// 
			BtnBack.AutoSize = true;
			BtnBack.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnBack.FlatAppearance.BorderSize = 0;
			BtnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnBack.Location = new System.Drawing.Point(12, 12);
			BtnBack.Name = "BtnBack";
			BtnBack.Size = new System.Drawing.Size(75, 27);
			BtnBack.TabIndex = 0;
			BtnBack.UseVisualStyleBackColor = false;
			BtnBack.Click += BtnBack_Click;
			// 
			// BtnPlay
			// 
			BtnPlay.AutoSize = true;
			BtnPlay.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnPlay.Location = new System.Drawing.Point(12, 41);
			BtnPlay.Name = "BtnPlay";
			BtnPlay.Size = new System.Drawing.Size(75, 27);
			BtnPlay.TabIndex = 1;
			BtnPlay.UseVisualStyleBackColor = false;
			BtnPlay.Click += BtnPlay_Click;
			// 
			// BtnPause
			// 
			BtnPause.AutoSize = true;
			BtnPause.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnPause.Location = new System.Drawing.Point(12, 70);
			BtnPause.Name = "BtnPause";
			BtnPause.Size = new System.Drawing.Size(75, 27);
			BtnPause.TabIndex = 2;
			BtnPause.UseVisualStyleBackColor = false;
			BtnPause.Click += BtnPause_Click;
			// 
			// BtnStop
			// 
			BtnStop.AutoSize = true;
			BtnStop.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnStop.Location = new System.Drawing.Point(12, 99);
			BtnStop.Name = "BtnStop";
			BtnStop.Size = new System.Drawing.Size(75, 27);
			BtnStop.TabIndex = 3;
			BtnStop.UseVisualStyleBackColor = false;
			BtnStop.Click += BtnStop_Click;
			// 
			// BtnNext
			// 
			BtnNext.AutoSize = true;
			BtnNext.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnNext.Location = new System.Drawing.Point(12, 128);
			BtnNext.Name = "BtnNext";
			BtnNext.Size = new System.Drawing.Size(75, 27);
			BtnNext.TabIndex = 4;
			BtnNext.UseVisualStyleBackColor = false;
			BtnNext.Click += BtnNext_Click;
			// 
			// BtnClose
			// 
			BtnClose.AutoSize = true;
			BtnClose.BackColor = System.Drawing.SystemColors.HotTrack;
			BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnClose.Location = new System.Drawing.Point(12, 157);
			BtnClose.Name = "BtnClose";
			BtnClose.Size = new System.Drawing.Size(75, 27);
			BtnClose.TabIndex = 5;
			BtnClose.UseVisualStyleBackColor = false;
			BtnClose.Click += BtnClose_Click;
			// 
			// LabelTitle
			// 
			LabelTitle.HorizontalAlign = System.Windows.Forms.HorizontalAlignment.Left;
			LabelTitle.Location = new System.Drawing.Point(12, 186);
			LabelTitle.Name = "LabelTitle";
			LabelTitle.ScrollEnable = false;
			LabelTitle.Size = new System.Drawing.Size(75, 24);
			LabelTitle.TabIndex = 6;
			// 
			// SldTrack
			// 
			SldTrack.BackColor = System.Drawing.Color.Transparent;
			SldTrack.Location = new System.Drawing.Point(12, 216);
			SldTrack.Maximum = 0;
			SldTrack.Minimum = 0;
			SldTrack.Name = "SldTrack";
			SldTrack.Orientation = System.Windows.Forms.Orientation.Horizontal;
			SldTrack.Size = new System.Drawing.Size(75, 25);
			SldTrack.SliderImage = null;
			SldTrack.TabIndex = 7;
			SldTrack.Value = 0;
			SldTrack.SliderMoved += SldTrack_SliderMoved;
			// 
			// SldVolume
			// 
			SldVolume.BackColor = System.Drawing.Color.Transparent;
			SldVolume.Location = new System.Drawing.Point(12, 247);
			SldVolume.Maximum = 0;
			SldVolume.Minimum = 0;
			SldVolume.Name = "SldVolume";
			SldVolume.Orientation = System.Windows.Forms.Orientation.Horizontal;
			SldVolume.Size = new System.Drawing.Size(75, 25);
			SldVolume.SliderImage = null;
			SldVolume.TabIndex = 8;
			SldVolume.Value = 0;
			SldVolume.SliderMoved += SldVolume_SliderMoved;
			SldVolume.SliderMoving += SldVolume_SliderMoving;
			// 
			// contextMenu
			// 
			contextMenu.Name = "contextMenu";
			contextMenu.Size = new System.Drawing.Size(61, 4);
			// 
			// MiniTimer
			// 
			MiniTimer.Tick += MiniTimer_Tick;
			// 
			// MiniPlayerForm
			// 
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			BackColor = System.Drawing.SystemColors.Window;
			BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			ClientSize = new System.Drawing.Size(120, 311);
			Controls.Add(SldVolume);
			Controls.Add(SldTrack);
			Controls.Add(LabelTitle);
			Controls.Add(BtnClose);
			Controls.Add(BtnNext);
			Controls.Add(BtnStop);
			Controls.Add(BtnPause);
			Controls.Add(BtnPlay);
			Controls.Add(BtnBack);
			DoubleBuffered = true;
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			Name = "MiniPlayerForm";
			Text = "Media Player X-Ark Zwei";
			Activated += MiniPlayerForm_Activated;
			FormClosing += MiniPlayerForm_FormClosing;
			Load += MiniPlayerForm_Load;
			MouseDown += MiniPlayerForm_MouseDown;
			MouseMove += MiniPlayerForm_MouseMove;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Button BtnBack;
        private System.Windows.Forms.Button BtnPlay;
        private System.Windows.Forms.Button BtnPause;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Button BtnNext;
        private System.Windows.Forms.Button BtnClose;
        private ScrollLabel LabelTitle;
        private CustomSlider SldTrack;
        private CustomSlider SldVolume;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.Timer MiniTimer;
    }
}