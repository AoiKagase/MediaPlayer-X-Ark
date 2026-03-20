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
			AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
			{
				string libsPath = Path.Combine(
					AppDomain.CurrentDomain.BaseDirectory, "Libs");
				string assemblyName = new System.Reflection.AssemblyName(args.Name).Name;
				string assemblyPath = Path.Combine(libsPath, assemblyName + ".dll");

				if (File.Exists(assemblyPath))
					return System.Reflection.Assembly.LoadFrom(assemblyPath);

				return null;
			};

			// ★Libs フォルダのネイティブDLLを先にロード
			string libsPath = Path.Combine(
				AppDomain.CurrentDomain.BaseDirectory, "Libs");
            Win32API.SetDllDirectory(libsPath);
			Win32API.LoadLibrary(Path.Combine(libsPath, "fmod.dll"));
			// fmod.dllの次に追加
			Win32API.LoadLibrary(Path.Combine(libsPath, "fluidsynth.dll"));
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
