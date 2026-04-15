namespace MediaPlayer_X_Ark
{
    partial class PlayListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			PlayListGrid = new System.Windows.Forms.DataGridView();
			PBtnOpen = new System.Windows.Forms.Button();
			PBtnSave = new System.Windows.Forms.Button();
			PBtnRemove = new System.Windows.Forms.Button();
			PBtnUp = new System.Windows.Forms.Button();
			PBtnDown = new System.Windows.Forms.Button();
			PBtnClose = new System.Windows.Forms.Button();
			PBtnClear = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)PlayListGrid).BeginInit();
			SuspendLayout();
			// 
			// PlayListGrid
			// 
			PlayListGrid.AllowUserToAddRows = false;
			PlayListGrid.AllowUserToDeleteRows = false;
			PlayListGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			PlayListGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			PlayListGrid.Location = new System.Drawing.Point(12, 12);
			PlayListGrid.Name = "PlayListGrid";
			PlayListGrid.ReadOnly = true;
			PlayListGrid.RowHeadersVisible = false;
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			PlayListGrid.RowsDefaultCellStyle = dataGridViewCellStyle1;
			PlayListGrid.RowTemplate.ReadOnly = true;
			PlayListGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			PlayListGrid.Size = new System.Drawing.Size(38, 22);
			PlayListGrid.TabIndex = 0;
			PlayListGrid.CellDoubleClick += PlayListGrid_CellDoubleClick;
			// 
			// PBtnOpen
			// 
			PBtnOpen.AutoSize = true;
			PBtnOpen.BackColor = System.Drawing.SystemColors.HotTrack;
			PBtnOpen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			PBtnOpen.FlatAppearance.BorderSize = 0;
			PBtnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			PBtnOpen.Location = new System.Drawing.Point(12, 40);
			PBtnOpen.Name = "PBtnOpen";
			PBtnOpen.Size = new System.Drawing.Size(38, 14);
			PBtnOpen.TabIndex = 4;
			PBtnOpen.UseVisualStyleBackColor = false;
			PBtnOpen.Click += PBtnOpen_Click;
			PBtnOpen.MouseDown += PBtnOpen_MouseDown;
			PBtnOpen.MouseUp += PBtnOpen_MouseUp;
			// 
			// PBtnSave
			// 
			PBtnSave.AutoSize = true;
			PBtnSave.BackColor = System.Drawing.SystemColors.HotTrack;
			PBtnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			PBtnSave.FlatAppearance.BorderSize = 0;
			PBtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			PBtnSave.Location = new System.Drawing.Point(56, 40);
			PBtnSave.Name = "PBtnSave";
			PBtnSave.Size = new System.Drawing.Size(31, 14);
			PBtnSave.TabIndex = 5;
			PBtnSave.UseVisualStyleBackColor = false;
			PBtnSave.Click += PBtnSave_Click;
			PBtnSave.MouseDown += PBtnSave_MouseDown;
			PBtnSave.MouseUp += PBtnSave_MouseUp;
			// 
			// PBtnRemove
			// 
			PBtnRemove.AutoSize = true;
			PBtnRemove.BackColor = System.Drawing.SystemColors.HotTrack;
			PBtnRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			PBtnRemove.FlatAppearance.BorderSize = 0;
			PBtnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			PBtnRemove.Location = new System.Drawing.Point(93, 40);
			PBtnRemove.Name = "PBtnRemove";
			PBtnRemove.Size = new System.Drawing.Size(28, 14);
			PBtnRemove.TabIndex = 6;
			PBtnRemove.UseVisualStyleBackColor = false;
			PBtnRemove.Click += PBtnRemove_Click;
			PBtnRemove.MouseDown += PBtnRemove_MouseDown;
			PBtnRemove.MouseUp += PBtnRemove_MouseUp;
			// 
			// PBtnUp
			// 
			PBtnUp.AutoSize = true;
			PBtnUp.BackColor = System.Drawing.SystemColors.HotTrack;
			PBtnUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			PBtnUp.FlatAppearance.BorderSize = 0;
			PBtnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			PBtnUp.Location = new System.Drawing.Point(12, 80);
			PBtnUp.Name = "PBtnUp";
			PBtnUp.Size = new System.Drawing.Size(38, 17);
			PBtnUp.TabIndex = 7;
			PBtnUp.UseVisualStyleBackColor = false;
			PBtnUp.Click += PBtnUp_Click;
			PBtnUp.MouseDown += PBtnUp_MouseDown;
			PBtnUp.MouseUp += PBtnUp_MouseUp;
			// 
			// PBtnDown
			// 
			PBtnDown.AutoSize = true;
			PBtnDown.BackColor = System.Drawing.SystemColors.HotTrack;
			PBtnDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			PBtnDown.FlatAppearance.BorderSize = 0;
			PBtnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			PBtnDown.Location = new System.Drawing.Point(93, 60);
			PBtnDown.Name = "PBtnDown";
			PBtnDown.Size = new System.Drawing.Size(28, 14);
			PBtnDown.TabIndex = 8;
			PBtnDown.UseVisualStyleBackColor = false;
			PBtnDown.Click += PBtnDown_Click;
			PBtnDown.MouseDown += PBtnDown_MouseDown;
			PBtnDown.MouseUp += PBtnDown_MouseUp;
			// 
			// PBtnClose
			// 
			PBtnClose.AutoSize = true;
			PBtnClose.BackColor = System.Drawing.SystemColors.HotTrack;
			PBtnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			PBtnClose.FlatAppearance.BorderSize = 0;
			PBtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			PBtnClose.Location = new System.Drawing.Point(12, 60);
			PBtnClose.Name = "PBtnClose";
			PBtnClose.Size = new System.Drawing.Size(38, 14);
			PBtnClose.TabIndex = 9;
			PBtnClose.UseVisualStyleBackColor = false;
			PBtnClose.Click += PBtnClose_Click;
			PBtnClose.MouseDown += PBtnClose_MouseDown;
			PBtnClose.MouseUp += PBtnClose_MouseUp;
			// 
			// PBtnClear
			// 
			PBtnClear.AutoSize = true;
			PBtnClear.BackColor = System.Drawing.SystemColors.HotTrack;
			PBtnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			PBtnClear.FlatAppearance.BorderSize = 0;
			PBtnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			PBtnClear.Location = new System.Drawing.Point(56, 60);
			PBtnClear.Name = "PBtnClear";
			PBtnClear.Size = new System.Drawing.Size(31, 14);
			PBtnClear.TabIndex = 10;
			PBtnClear.UseVisualStyleBackColor = false;
			PBtnClear.Click += PBtnClear_Click;
			PBtnClear.MouseDown += PBtnClear_MouseDown;
			PBtnClear.MouseUp += PBtnClear_MouseUp;
			// 
			// PlayListForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.SystemColors.ControlLight;
			BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			ClientSize = new System.Drawing.Size(128, 103);
			ControlBox = false;
			Controls.Add(PBtnClear);
			Controls.Add(PBtnClose);
			Controls.Add(PBtnDown);
			Controls.Add(PBtnUp);
			Controls.Add(PBtnRemove);
			Controls.Add(PBtnSave);
			Controls.Add(PBtnOpen);
			Controls.Add(PlayListGrid);
			DoubleBuffered = true;
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			Name = "PlayListForm";
			ShowIcon = false;
			ShowInTaskbar = false;
			Text = "PlayList";
			Activated += PlayListForm_Activated;
			FormClosing += PlayListForm_FormClosing;
			Load += PlayList_Load;
			MouseDown += PlayList_MouseDown;
			MouseMove += PlayList_MouseMove;
			((System.ComponentModel.ISupportInitialize)PlayListGrid).EndInit();
			ResumeLayout(false);
			PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView PlayListGrid;
        private System.Windows.Forms.Button PBtnOpen;
        private System.Windows.Forms.Button PBtnSave;
        private System.Windows.Forms.Button PBtnRemove;
        private System.Windows.Forms.Button PBtnUp;
        private System.Windows.Forms.Button PBtnDown;
        private System.Windows.Forms.Button PBtnClose;
        private System.Windows.Forms.Button PBtnClear;
    }
}