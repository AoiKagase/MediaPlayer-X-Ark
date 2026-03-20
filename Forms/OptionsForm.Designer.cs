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
			this.SuspendLayout();
			this.ClientSize = new System.Drawing.Size(800, 520);
			this.Name = "OptionsForm";
			this.Text = "Options";
			this.FormClosing += OptionsForm_FormClosing;
			this.Load += OptionsForm_Load;
			this.ResumeLayout(false);
		}
	}
}