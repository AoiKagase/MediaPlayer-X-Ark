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
    public partial class PlayListForm : Form
    {
        MainForm mainForm;
        public PlayListForm(MainForm main)
        {
            mainForm = main;
            InitializeComponent();
        }

        private void PlayList_Load(object sender, EventArgs e)
        {
            this.PlayListGrid.DataSource = MainForm.player.PlayList;

//        public string FileName { get; set; }
//        public FMOD.Sound Sound { get; set; }
//        public string Title { get; set; }
//        public string Artist { get; set; }
//        public string Album { get; set; }
//        public FMOD.SOUND_TYPE SoundType { get; set; }
//        public FMOD.SOUND_FORMAT Format { get; set; }
//        public int Bit { get; set; }
//        public uint length { get; set; }
            this.PlayListGrid.Columns[0].Visible = false;
            this.PlayListGrid.Columns[1].Visible = false;
            this.PlayListGrid.Columns[3].Visible = false;
            this.PlayListGrid.Columns[4].Visible = false;
            this.PlayListGrid.Columns[5].Visible = false;
            this.PlayListGrid.Columns[6].Visible = false;
            this.PlayListGrid.Columns[7].Visible = false;

            this.PlayListGrid.Columns[2].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PlayListGrid.Columns[8].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
        }

        private void PBtnOpen_MouseDown(object sender, MouseEventArgs e)
        {
            mainForm.BtnDownEvent(ref sender);
        }

        private void PBtnOpen_MouseUp(object sender, MouseEventArgs e)
        {
            mainForm.BtnUpEvent(ref sender);
        }

        private void PlayListGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                mainForm.PlayLoad(e.RowIndex);
        }
    }
}
