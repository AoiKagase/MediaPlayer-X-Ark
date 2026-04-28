using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark
{
	public partial class PresetNameInputForm : Form
	{
		public PresetNameInputForm()
		{
			InitializeComponent();
			ApplicationIcon.ApplyTo(this);
		}

		private void BtnOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void PresetNameInputForm_Load(object sender, EventArgs e)
		{

		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}


	}
}
