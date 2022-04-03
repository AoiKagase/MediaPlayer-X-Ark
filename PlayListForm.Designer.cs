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
            this.DataGridPlaylist = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridPlaylist)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridPlaylist
            // 
            this.DataGridPlaylist.AllowUserToAddRows = false;
            this.DataGridPlaylist.AllowUserToDeleteRows = false;
            this.DataGridPlaylist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridPlaylist.Location = new System.Drawing.Point(12, 12);
            this.DataGridPlaylist.Name = "DataGridPlaylist";
            this.DataGridPlaylist.ReadOnly = true;
            this.DataGridPlaylist.RowTemplate.Height = 25;
            this.DataGridPlaylist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridPlaylist.Size = new System.Drawing.Size(776, 426);
            this.DataGridPlaylist.TabIndex = 0;
            // 
            // PlayListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DataGridPlaylist);
            this.Name = "PlayListForm";
            this.Text = "PlayList";
            this.Load += new System.EventHandler(this.PlayList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridPlaylist)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DataGridPlaylist;
    }
}