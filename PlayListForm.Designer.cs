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
            this.PlayListGrid = new System.Windows.Forms.DataGridView();
            this.PBtnOpen = new System.Windows.Forms.Button();
            this.PBtnSave = new System.Windows.Forms.Button();
            this.PBtnRemove = new System.Windows.Forms.Button();
            this.PBtnUp = new System.Windows.Forms.Button();
            this.PBtnDown = new System.Windows.Forms.Button();
            this.PBtnClose = new System.Windows.Forms.Button();
            this.PBtnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PlayListGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // PlayListGrid
            // 
            this.PlayListGrid.AllowUserToAddRows = false;
            this.PlayListGrid.AllowUserToDeleteRows = false;
            this.PlayListGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PlayListGrid.Location = new System.Drawing.Point(12, 12);
            this.PlayListGrid.Name = "PlayListGrid";
            this.PlayListGrid.ReadOnly = true;
            this.PlayListGrid.RowTemplate.Height = 25;
            this.PlayListGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PlayListGrid.Size = new System.Drawing.Size(38, 22);
            this.PlayListGrid.TabIndex = 0;
            // 
            // PBtnOpen
            // 
            this.PBtnOpen.AutoSize = true;
            this.PBtnOpen.BackColor = System.Drawing.SystemColors.HotTrack;
            this.PBtnOpen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PBtnOpen.FlatAppearance.BorderSize = 0;
            this.PBtnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PBtnOpen.Location = new System.Drawing.Point(12, 40);
            this.PBtnOpen.Name = "PBtnOpen";
            this.PBtnOpen.Size = new System.Drawing.Size(38, 14);
            this.PBtnOpen.TabIndex = 4;
            this.PBtnOpen.UseVisualStyleBackColor = false;
            this.PBtnOpen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PBtnOpen_MouseDown);
            this.PBtnOpen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PBtnOpen_MouseUp);
            // 
            // PBtnSave
            // 
            this.PBtnSave.AutoSize = true;
            this.PBtnSave.BackColor = System.Drawing.SystemColors.HotTrack;
            this.PBtnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PBtnSave.FlatAppearance.BorderSize = 0;
            this.PBtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PBtnSave.Location = new System.Drawing.Point(56, 40);
            this.PBtnSave.Name = "PBtnSave";
            this.PBtnSave.Size = new System.Drawing.Size(31, 14);
            this.PBtnSave.TabIndex = 5;
            this.PBtnSave.UseVisualStyleBackColor = false;
            // 
            // PBtnRemove
            // 
            this.PBtnRemove.AutoSize = true;
            this.PBtnRemove.BackColor = System.Drawing.SystemColors.HotTrack;
            this.PBtnRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PBtnRemove.FlatAppearance.BorderSize = 0;
            this.PBtnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PBtnRemove.Location = new System.Drawing.Point(93, 40);
            this.PBtnRemove.Name = "PBtnRemove";
            this.PBtnRemove.Size = new System.Drawing.Size(28, 14);
            this.PBtnRemove.TabIndex = 6;
            this.PBtnRemove.UseVisualStyleBackColor = false;
            // 
            // PBtnUp
            // 
            this.PBtnUp.AutoSize = true;
            this.PBtnUp.BackColor = System.Drawing.SystemColors.HotTrack;
            this.PBtnUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PBtnUp.FlatAppearance.BorderSize = 0;
            this.PBtnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PBtnUp.Location = new System.Drawing.Point(12, 80);
            this.PBtnUp.Name = "PBtnUp";
            this.PBtnUp.Size = new System.Drawing.Size(38, 17);
            this.PBtnUp.TabIndex = 7;
            this.PBtnUp.UseVisualStyleBackColor = false;
            // 
            // PBtnDown
            // 
            this.PBtnDown.AutoSize = true;
            this.PBtnDown.BackColor = System.Drawing.SystemColors.HotTrack;
            this.PBtnDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PBtnDown.FlatAppearance.BorderSize = 0;
            this.PBtnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PBtnDown.Location = new System.Drawing.Point(93, 60);
            this.PBtnDown.Name = "PBtnDown";
            this.PBtnDown.Size = new System.Drawing.Size(28, 14);
            this.PBtnDown.TabIndex = 8;
            this.PBtnDown.UseVisualStyleBackColor = false;
            // 
            // PBtnClose
            // 
            this.PBtnClose.AutoSize = true;
            this.PBtnClose.BackColor = System.Drawing.SystemColors.HotTrack;
            this.PBtnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PBtnClose.FlatAppearance.BorderSize = 0;
            this.PBtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PBtnClose.Location = new System.Drawing.Point(12, 60);
            this.PBtnClose.Name = "PBtnClose";
            this.PBtnClose.Size = new System.Drawing.Size(38, 14);
            this.PBtnClose.TabIndex = 9;
            this.PBtnClose.UseVisualStyleBackColor = false;
            // 
            // PBtnClear
            // 
            this.PBtnClear.AutoSize = true;
            this.PBtnClear.BackColor = System.Drawing.SystemColors.HotTrack;
            this.PBtnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PBtnClear.FlatAppearance.BorderSize = 0;
            this.PBtnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PBtnClear.Location = new System.Drawing.Point(56, 60);
            this.PBtnClear.Name = "PBtnClear";
            this.PBtnClear.Size = new System.Drawing.Size(31, 14);
            this.PBtnClear.TabIndex = 10;
            this.PBtnClear.UseVisualStyleBackColor = false;
            // 
            // PlayListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(128, 103);
            this.ControlBox = false;
            this.Controls.Add(this.PBtnClear);
            this.Controls.Add(this.PBtnClose);
            this.Controls.Add(this.PBtnDown);
            this.Controls.Add(this.PBtnUp);
            this.Controls.Add(this.PBtnRemove);
            this.Controls.Add(this.PBtnSave);
            this.Controls.Add(this.PBtnOpen);
            this.Controls.Add(this.PlayListGrid);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PlayListForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "PlayList";
            this.Load += new System.EventHandler(this.PlayList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PlayListGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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