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
			cmbDrive = new System.Windows.Forms.ComboBox();
			lstTracks = new System.Windows.Forms.ListBox();
			lblStatus = new System.Windows.Forms.Label();
			lblDrive = new System.Windows.Forms.Label();
			BtnSelectAll = new System.Windows.Forms.Button();
			BtnDeselectAll = new System.Windows.Forms.Button();
			BtnAddPlaylist = new System.Windows.Forms.Button();
			BtnClearPlaylist = new System.Windows.Forms.Button();
			BtnRefresh = new System.Windows.Forms.Button();
			BtnEject = new System.Windows.Forms.Button();
			BtnClose = new System.Windows.Forms.Button();
			BtnCddb = new System.Windows.Forms.Button();
			lblFormat = new System.Windows.Forms.Label();
			cmbFormat = new System.Windows.Forms.ComboBox();
			BtnRip = new System.Windows.Forms.Button();
			prgRip = new System.Windows.Forms.ProgressBar();
			SuspendLayout();
			//
			// cmbDrive
			//
			cmbDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cmbDrive.FormattingEnabled = true;
			cmbDrive.Location = new System.Drawing.Point(65, 10);
			cmbDrive.Name = "cmbDrive";
			cmbDrive.Size = new System.Drawing.Size(80, 23);
			cmbDrive.TabIndex = 0;
			cmbDrive.SelectedIndexChanged += cmbDrive_SelectedIndexChanged;
			//
			// lstTracks
			//
			lstTracks.FormattingEnabled = true;
			lstTracks.Location = new System.Drawing.Point(12, 44);
			lstTracks.Name = "lstTracks";
			lstTracks.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			lstTracks.Size = new System.Drawing.Size(436, 244);
			lstTracks.TabIndex = 3;
			lstTracks.DoubleClick += lstTracks_DoubleClick;
			//
			// lblStatus
			//
			lblStatus.Location = new System.Drawing.Point(12, 297);
			lblStatus.Name = "lblStatus";
			lblStatus.Size = new System.Drawing.Size(436, 20);
			lblStatus.TabIndex = 4;
			lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// lblDrive
			//
			lblDrive.AutoSize = true;
			lblDrive.Location = new System.Drawing.Point(12, 14);
			lblDrive.Name = "lblDrive";
			lblDrive.Size = new System.Drawing.Size(44, 15);
			lblDrive.TabIndex = 0;
			lblDrive.Text = "ドライブ:";
			//
			// BtnSelectAll
			//
			BtnSelectAll.Location = new System.Drawing.Point(12, 325);
			BtnSelectAll.Name = "BtnSelectAll";
			BtnSelectAll.Size = new System.Drawing.Size(90, 30);
			BtnSelectAll.TabIndex = 4;
			BtnSelectAll.Text = "全選択";
			BtnSelectAll.UseVisualStyleBackColor = true;
			BtnSelectAll.Click += BtnSelectAll_Click;
			//
			// BtnDeselectAll
			//
			BtnDeselectAll.Location = new System.Drawing.Point(112, 325);
			BtnDeselectAll.Name = "BtnDeselectAll";
			BtnDeselectAll.Size = new System.Drawing.Size(90, 30);
			BtnDeselectAll.TabIndex = 5;
			BtnDeselectAll.Text = "全解除";
			BtnDeselectAll.UseVisualStyleBackColor = true;
			BtnDeselectAll.Click += BtnDeselectAll_Click;
			//
			// BtnAddPlaylist
			//
			BtnAddPlaylist.Location = new System.Drawing.Point(12, 365);
			BtnAddPlaylist.Name = "BtnAddPlaylist";
			BtnAddPlaylist.Size = new System.Drawing.Size(140, 30);
			BtnAddPlaylist.TabIndex = 6;
			BtnAddPlaylist.Text = "プレイリストに追加";
			BtnAddPlaylist.UseVisualStyleBackColor = true;
			BtnAddPlaylist.Click += BtnAddPlaylist_Click;
			//
			// BtnClearPlaylist
			//
			BtnClearPlaylist.Location = new System.Drawing.Point(162, 365);
			BtnClearPlaylist.Name = "BtnClearPlaylist";
			BtnClearPlaylist.Size = new System.Drawing.Size(286, 30);
			BtnClearPlaylist.TabIndex = 7;
			BtnClearPlaylist.Text = "プレイリスト全消去";
			BtnClearPlaylist.UseVisualStyleBackColor = true;
			BtnClearPlaylist.Click += BtnClearPlaylist_Click;
			//
			// BtnRefresh
			//
			BtnRefresh.Location = new System.Drawing.Point(155, 9);
			BtnRefresh.Name = "BtnRefresh";
			BtnRefresh.Size = new System.Drawing.Size(75, 25);
			BtnRefresh.TabIndex = 1;
			BtnRefresh.Text = "更新";
			BtnRefresh.UseVisualStyleBackColor = true;
			BtnRefresh.Click += BtnRefresh_Click;
			//
			// BtnEject
			//
			BtnEject.Location = new System.Drawing.Point(240, 9);
			BtnEject.Name = "BtnEject";
			BtnEject.Size = new System.Drawing.Size(75, 25);
			BtnEject.TabIndex = 2;
			BtnEject.Text = "取り出し";
			BtnEject.UseVisualStyleBackColor = true;
			BtnEject.Click += BtnEject_Click;
			//
			// BtnClose
			//
			BtnClose.Location = new System.Drawing.Point(373, 325);
			BtnClose.Name = "BtnClose";
			BtnClose.Size = new System.Drawing.Size(75, 30);
			BtnClose.TabIndex = 8;
			BtnClose.Text = "閉じる";
			BtnClose.UseVisualStyleBackColor = true;
			BtnClose.Click += BtnClose_Click;
			//
			// BtnCddb
			//
			BtnCddb.Location = new System.Drawing.Point(325, 9);
			BtnCddb.Name = "BtnCddb";
			BtnCddb.Size = new System.Drawing.Size(120, 25);
			BtnCddb.TabIndex = 9;
			BtnCddb.Text = "CDDB 問い合わせ";
			BtnCddb.Click += BtnCddb_Click;
			//
			// lblFormat
			//
			lblFormat.AutoSize = true;
			lblFormat.Location = new System.Drawing.Point(12, 410);
			lblFormat.Name = "lblFormat";
			lblFormat.TabIndex = 10;
			lblFormat.Text = "形式:";
			//
			// cmbFormat
			//
			cmbFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cmbFormat.FormattingEnabled = true;
			cmbFormat.Items.AddRange(new object[] { "WAV", "FLAC", "ALAC (.m4a)", "SRLA (.srl)" });
			cmbFormat.Location = new System.Drawing.Point(50, 406);
			cmbFormat.Name = "cmbFormat";
			cmbFormat.Size = new System.Drawing.Size(110, 23);
			cmbFormat.TabIndex = 11;
			cmbFormat.SelectedIndex = 1;
			//
			// BtnRip
			//
			BtnRip.Location = new System.Drawing.Point(172, 403);
			BtnRip.Name = "BtnRip";
			BtnRip.Size = new System.Drawing.Size(140, 30);
			BtnRip.TabIndex = 12;
			BtnRip.Text = "リップ保存";
			BtnRip.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
			BtnRip.ForeColor = System.Drawing.Color.White;
			BtnRip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnRip.Click += BtnRip_Click;
			//
			// prgRip
			//
			prgRip.Location = new System.Drawing.Point(12, 443);
			prgRip.Name = "prgRip";
			prgRip.Size = new System.Drawing.Size(436, 18);
			prgRip.TabIndex = 13;
			prgRip.Visible = false;
			//
			// CDForm
			//
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			ClientSize = new System.Drawing.Size(460, 472);
			Controls.Add(lblDrive);
			Controls.Add(cmbDrive);
			Controls.Add(BtnRefresh);
			Controls.Add(BtnEject);
			Controls.Add(lstTracks);
			Controls.Add(lblStatus);
			Controls.Add(BtnSelectAll);
			Controls.Add(BtnDeselectAll);
			Controls.Add(BtnAddPlaylist);
			Controls.Add(BtnClearPlaylist);
			Controls.Add(BtnClose);
			Controls.Add(BtnCddb);
			Controls.Add(lblFormat);
			Controls.Add(cmbFormat);
			Controls.Add(BtnRip);
			Controls.Add(prgRip);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "CDForm";
			ShowInTaskbar = false;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "CD Player";
			FormClosing += CDForm_FormClosing;
			Load += CDForm_Load;
			ResumeLayout(false);
			PerformLayout();
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
		private System.Windows.Forms.Label lblFormat;
		private System.Windows.Forms.ComboBox cmbFormat;
		private System.Windows.Forms.Button BtnRip;
		private System.Windows.Forms.ProgressBar prgRip;
	}
}
