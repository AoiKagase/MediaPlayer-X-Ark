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
    public partial class OptionsForm : Form
    {
        private PlayerEngine _engine;
        public OptionsForm(ref PlayerEngine engine)
        {
            InitializeComponent();
            _engine = engine;
        }

        private void DistortionLevel_ValueChanged(object sender, EventArgs e)
        {
            _engine.effector.Distortion.Level = ((UI.Knob) sender).Value / 100F;
            lblValDistortionLevel.Text = _engine.effector.Distortion.Level.ToString("##0.00");
        }

        private void CheckDistortion_CheckedChanged(object sender, EventArgs e)
        {
            _engine.effector.Distortion.Switch(((CheckBox)sender).Checked);
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            KnobDistortionLevel.Value = (int)(_engine.effector.Distortion.Level * 100);
        }
    }
}
