using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaPlayer_X_Ark.Engine.Update;

namespace MediaPlayer_X_Ark
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            UpdateApplier.CleanupStagedUpdateArtifacts();
            //            Application.EnableVisualStyles();
            //            Application.SetCompatibleTextRenderingDefault(false);
            //            Application.Run(new MainForm());
            StartUp winAppBase = new StartUp();
            winAppBase.Run(args);
        }
    }
}
