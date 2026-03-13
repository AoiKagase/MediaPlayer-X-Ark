using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;
using System.IO;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark
{
    class StartUp : WindowsFormsApplicationBase
    {
        public StartUp() : base()
        {
            this.EnableVisualStyles = true;
            this.IsSingleInstance = true;
            this.MainForm = new MainForm();
            this.StartupNextInstance += new StartupNextInstanceEventHandler(StartUp_StartupNextInstance);
        }

        /// <summary>
        /// 多重起動処理
        /// 1度起動した後の多重起動時のみ実行される
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StartUp_StartupNextInstance(object sender, StartupNextInstanceEventArgs e)
        {
            // パラメータを取得してOpen関数へ引き渡し
            string parameters = e.CommandLine[0];
            if (File.Exists(parameters))
            {
                ((MainForm)this.MainForm).OpenFile(parameters);
            }
        }
    }
}
