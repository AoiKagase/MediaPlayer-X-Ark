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
            this.PlayListGrid.Columns.Clear();
            this.PlayListGrid.Columns.Add("Title", "Title");
            this.PlayListGrid.Columns.Add("Artist", "Artist");
            this.PlayListGrid.Columns.Add("Album", "Album");
            this.PlayListGrid.Columns.Add("length", "Length");
        }

        private void PBtnOpen_MouseDown(object sender, MouseEventArgs e)
        {
            mainForm.BtnDownEvent(ref sender);
        }

        private void PBtnOpen_MouseUp(object sender, MouseEventArgs e)
        {
            mainForm.BtnUpEvent(ref sender);
        }
    }
}
