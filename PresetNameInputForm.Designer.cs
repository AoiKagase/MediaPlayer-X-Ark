using System.Windows.Forms;

namespace MediaPlayer_X_Ark
{
	partial class PresetNameInputForm : Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		public string PresetNameValue
		{
			get { return PresetName.Text; }
			set { PresetName.Text = value; }
		}
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
			PresetName = new TextBox();
			BtnOK = new Button();
			BtnCancel = new Button();
			SuspendLayout();
			// 
			// PresetName
			// 
			PresetName.Location = new System.Drawing.Point(12, 12);
			PresetName.Name = "PresetName";
			PresetName.Size = new System.Drawing.Size(169, 23);
			PresetName.TabIndex = 0;
			// 
			// BtnOK
			// 
			BtnOK.Location = new System.Drawing.Point(12, 44);
			BtnOK.Name = "BtnOK";
			BtnOK.Size = new System.Drawing.Size(64, 22);
			BtnOK.TabIndex = 1;
			BtnOK.Text = "OK";
			BtnOK.UseVisualStyleBackColor = true;
			BtnOK.Click += BtnOK_Click;
			// 
			// BtnCancel
			// 
			BtnCancel.Location = new System.Drawing.Point(117, 44);
			BtnCancel.Name = "BtnCancel";
			BtnCancel.Size = new System.Drawing.Size(64, 22);
			BtnCancel.TabIndex = 2;
			BtnCancel.Text = "Cancel";
			BtnCancel.UseVisualStyleBackColor = true;
			BtnCancel.Click += BtnCancel_Click;
			// 
			// PresetNameInputForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(197, 82);
			ControlBox = false;
			Controls.Add(BtnCancel);
			Controls.Add(BtnOK);
			Controls.Add(PresetName);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "PresetNameInputForm";
			ShowIcon = false;
			ShowInTaskbar = false;
			SizeGripStyle = SizeGripStyle.Hide;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Preset Name";
			TopMost = true;
			Load += PresetNameInputForm_Load;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.TextBox PresetName;
		private System.Windows.Forms.Button BtnOK;
		private System.Windows.Forms.Button BtnCancel;
	}
}