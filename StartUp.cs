using MediaPlayer_X_Ark.Engine.CD;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

			// Libs フォルダのネイティブ DLL を init() より前に明示的にロードする
			string libsPath = Path.Combine(
				AppDomain.CurrentDomain.BaseDirectory, "Libs");
			Win32API.SetDllDirectory(libsPath);
			Win32API.LoadLibrary(Path.Combine(libsPath, "fmod.dll"));
			Win32API.LoadLibrary(Path.Combine(libsPath, "fluidsynth.dll"));
			TryLoadOptionalLibrary(libsPath, "bass.dll");
			TryLoadOptionalLibrary(libsPath, "bassmidi.dll");
			Win32API.LoadLibrary(Path.Combine(libsPath, "AlacEncoder.dll"));
			Win32API.LoadLibrary(Path.Combine(libsPath, "FlacEncoder.dll"));
            System.Diagnostics.Debug.WriteLine(AlacEncoder.GetLoadedBuildId());
            System.Diagnostics.Debug.WriteLine(FlacEncoder.GetLoadedBuildId());
			this.EnableVisualStyles = true;
            this.IsSingleInstance = true;
            this.MainForm = new MainForm();
            this.StartupNextInstance += new StartupNextInstanceEventHandler(StartUp_StartupNextInstance);
        }

		private static void TryLoadOptionalLibrary(string libsPath, string fileName)
		{
			string fullPath = Path.Combine(libsPath, fileName);
			if (File.Exists(fullPath))
				Win32API.LoadLibrary(fullPath);
		}

        /// <summary>
        /// 多重起動処理
        /// 1度起動した後の多重起動時のみ実行される
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StartUp_StartupNextInstance(object sender, StartupNextInstanceEventArgs e)
        {
            string parameters = e.CommandLine[0];
            if (File.Exists(parameters))
            {
                ((MainForm)this.MainForm).Controller.OpenAndPlay(parameters);
            }
        }
    }
}
