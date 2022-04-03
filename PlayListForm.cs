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
        public PlayListForm()
        {
            InitializeComponent();
        }

        private void PlayList_Load(object sender, EventArgs e)
        {
            this.DataGridPlaylist.DataSource = MainForm.player.PlayList;
            this.DataGridPlaylist.Columns.Clear();
            this.DataGridPlaylist.Columns.Add("Title", "Title");
            this.DataGridPlaylist.Columns.Add("Artist", "Artist");
            this.DataGridPlaylist.Columns.Add("Album", "Album");
            this.DataGridPlaylist.Columns.Add("length", "Length");
        }
    }
}
