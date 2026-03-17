using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            //            Application.EnableVisualStyles();
            //            Application.SetCompatibleTextRenderingDefault(false);
            //            Application.Run(new MainForm());
            StartUp winAppBase = new StartUp();
            // initialize configuration and expose as IConfigService for DI migration
            var cfg = new Engine.Configration(Engine.PlayerEngineStaticHolder.EngineInstance);
            var configService = cfg.AsService();

            // The temporary holder provides the PlayerEngine instance used across the app until DI is introduced
            Engine.PlayerEngineStaticHolder.ConfigService = configService;

            winAppBase.Run(args);
        }
    }
}
