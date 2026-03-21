using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace MediaPlayer_X_Ark
{
    partial class CDForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.cmbDrive        = new System.Windows.Forms.ComboBox();
            this.lstTracks       = new System.Windows.Forms.ListBox();
            this.lblStatus       = new System.Windows.Forms.Label();
            this.lblDrive        = new System.Windows.Forms.Label();
            this.BtnSelectAll    = new System.Windows.Forms.Button();
            this.BtnDeselectAll  = new System.Windows.Forms.Button();
            this.BtnAddPlaylist  = new System.Windows.Forms.Button();
            this.BtnClearPlaylist = new System.Windows.Forms.Button();
            this.BtnRefresh      = new System.Windows.Forms.Button();
            this.BtnEject        = new System.Windows.Forms.Button();
            this.BtnClose        = new System.Windows.Forms.Button();
			this.BtnCddb         = new System.Windows.Forms.Button();

			this.SuspendLayout();

            // lblDrive
            this.lblDrive.AutoSize = true;
            this.lblDrive.Location = new System.Drawing.Point(12, 14);
            this.lblDrive.Name     = "lblDrive";
            this.lblDrive.Size     = new System.Drawing.Size(44, 15);
            this.lblDrive.Text     = "ドライブ:";

            // cmbDrive
            this.cmbDrive.DropDownStyle    = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDrive.FormattingEnabled = true;
            this.cmbDrive.Location = new System.Drawing.Point(65, 10);
            this.cmbDrive.Name     = "cmbDrive";
            this.cmbDrive.Size     = new System.Drawing.Size(80, 23);
            this.cmbDrive.TabIndex = 0;
            this.cmbDrive.SelectedIndexChanged += new System.EventHandler(this.cmbDrive_SelectedIndexChanged);

            // BtnRefresh
            this.BtnRefresh.Location  = new System.Drawing.Point(155, 9);
            this.BtnRefresh.Name      = "BtnRefresh";
            this.BtnRefresh.Size      = new System.Drawing.Size(75, 25);
            this.BtnRefresh.TabIndex  = 1;
            this.BtnRefresh.Text      = "更新";
            this.BtnRefresh.UseVisualStyleBackColor = true;
            this.BtnRefresh.Click    += new System.EventHandler(this.BtnRefresh_Click);

            // BtnEject
            this.BtnEject.Location  = new System.Drawing.Point(240, 9);
            this.BtnEject.Name      = "BtnEject";
            this.BtnEject.Size      = new System.Drawing.Size(75, 25);
            this.BtnEject.TabIndex  = 2;
            this.BtnEject.Text      = "取り出し";
            this.BtnEject.UseVisualStyleBackColor = true;
            this.BtnEject.Click    += new System.EventHandler(this.BtnEject_Click);

            // lstTracks
            this.lstTracks.FormattingEnabled = true;
            this.lstTracks.ItemHeight        = 15;
            this.lstTracks.Location          = new System.Drawing.Point(12, 44);
            this.lstTracks.Name              = "lstTracks";
            this.lstTracks.SelectionMode     = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstTracks.Size              = new System.Drawing.Size(436, 244);
			this.lstTracks.TabIndex          = 3;
            this.lstTracks.DoubleClick      += new System.EventHandler(this.lstTracks_DoubleClick);
			
            // lblStatus
			this.lblStatus.AutoSize  = false;
            this.lblStatus.Location  = new System.Drawing.Point(12, 297);
            this.lblStatus.Name      = "lblStatus";
            this.lblStatus.Size      = new System.Drawing.Size(436, 20);
			this.lblStatus.Text      = "";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // BtnSelectAll
            this.BtnSelectAll.Location  = new System.Drawing.Point(12, 325);
            this.BtnSelectAll.Name      = "BtnSelectAll";
            this.BtnSelectAll.Size      = new System.Drawing.Size(90, 30);
            this.BtnSelectAll.TabIndex  = 4;
            this.BtnSelectAll.Text      = "全選択";
            this.BtnSelectAll.UseVisualStyleBackColor = true;
            this.BtnSelectAll.Click    += new System.EventHandler(this.BtnSelectAll_Click);

            // BtnDeselectAll
            this.BtnDeselectAll.Location  = new System.Drawing.Point(112, 325);
            this.BtnDeselectAll.Name      = "BtnDeselectAll";
            this.BtnDeselectAll.Size      = new System.Drawing.Size(90, 30);
            this.BtnDeselectAll.TabIndex  = 5;
            this.BtnDeselectAll.Text      = "全解除";
            this.BtnDeselectAll.UseVisualStyleBackColor = true;
            this.BtnDeselectAll.Click    += new System.EventHandler(this.BtnDeselectAll_Click);

            // BtnAddPlaylist
            this.BtnAddPlaylist.Location  = new System.Drawing.Point(12, 365);
            this.BtnAddPlaylist.Name      = "BtnAddPlaylist";
            this.BtnAddPlaylist.Size      = new System.Drawing.Size(140, 30);
            this.BtnAddPlaylist.TabIndex  = 6;
            this.BtnAddPlaylist.Text      = "プレイリストに追加";
            this.BtnAddPlaylist.UseVisualStyleBackColor = true;
            this.BtnAddPlaylist.Click    += new System.EventHandler(this.BtnAddPlaylist_Click);

            // BtnClearPlaylist
            this.BtnClearPlaylist.Location  = new System.Drawing.Point(162, 365);
            this.BtnClearPlaylist.Name      = "BtnClearPlaylist";
 			this.BtnClearPlaylist.Size      = new System.Drawing.Size(286, 30);
			this.BtnClearPlaylist.TabIndex  = 7;
            this.BtnClearPlaylist.Text      = "プレイリスト全消去";
            this.BtnClearPlaylist.UseVisualStyleBackColor = true;
            this.BtnClearPlaylist.Click    += new System.EventHandler(this.BtnClearPlaylist_Click);

            // BtnClose
            this.BtnClose.Location  = new System.Drawing.Point(240, 325);
            this.BtnClose.Name      = "BtnClose";
            this.BtnClose.Size      = new System.Drawing.Size(75, 30);
            this.BtnClose.TabIndex  = 8;
            this.BtnClose.Text      = "閉じる";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click    += new System.EventHandler(this.BtnClose_Click);

			this.BtnCddb.Text     = "CDDB 問い合わせ";
			this.BtnCddb.Location = new System.Drawing.Point(325, 9);
			this.BtnCddb.Size = new System.Drawing.Size(120, 25);
			this.BtnCddb.Click   += new System.EventHandler(this.BtnCddb_Click);
			
            // CDForm
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize          = new System.Drawing.Size(460, 407);
			this.Controls.Add(this.lblDrive);
            this.Controls.Add(this.cmbDrive);
            this.Controls.Add(this.BtnRefresh);
            this.Controls.Add(this.BtnEject);
            this.Controls.Add(this.lstTracks);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.BtnSelectAll);
            this.Controls.Add(this.BtnDeselectAll);
            this.Controls.Add(this.BtnAddPlaylist);
            this.Controls.Add(this.BtnClearPlaylist);
            this.Controls.Add(this.BtnClose);
			this.Controls.Add(this.BtnCddb);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.Name            = "CDForm";
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text            = "CD Player";
            this.Load           += new System.EventHandler(this.CDForm_Load);
            this.FormClosing    += new System.Windows.Forms.FormClosingEventHandler(this.CDForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox cmbDrive;
        private System.Windows.Forms.ListBox lstTracks;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblDrive;
        private System.Windows.Forms.Button BtnSelectAll;
        private System.Windows.Forms.Button BtnDeselectAll;
        private System.Windows.Forms.Button BtnAddPlaylist;
        private System.Windows.Forms.Button BtnClearPlaylist;
        private System.Windows.Forms.Button BtnRefresh;
        private System.Windows.Forms.Button BtnEject;
        private System.Windows.Forms.Button BtnClose;
		private System.Windows.Forms.Button BtnCddb;
	}
}
