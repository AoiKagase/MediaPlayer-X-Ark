namespace MediaPlayer_X_Ark.Forms
{
	partial class OptionsForm
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
				components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			SuspendLayout();
			// 
			// OptionsForm
			// 
			ClientSize = new System.Drawing.Size(800, 609);
			Name = "OptionsForm";
			Text = "Options";
			FormClosing += OptionsForm_FormClosing;
			Load += OptionsForm_Load;
			ResumeLayout(false);
		}
	}
}