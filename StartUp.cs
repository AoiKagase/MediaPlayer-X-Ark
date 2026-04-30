using MediaPlayer_X_Ark.Engine.CD;
using MediaPlayer_X_Ark.Forms;
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
			this.MinimumSplashScreenDisplayTime = 3000;
			this.SplashScreen = new SplashForm();
            var mainForm = new MainForm();
			mainForm.StartupReady += MainForm_StartupReady;
            this.MainForm = mainForm;
            this.StartupNextInstance += new StartupNextInstanceEventHandler(StartUp_StartupNextInstance);
        }

		private void MainForm_StartupReady(object sender, EventArgs e)
		{
			if (this.SplashScreen is SplashForm splash && !splash.IsDisposed)
				splash.CloseAfterMinimumDisplay(this.MinimumSplashScreenDisplayTime);
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
			this.MainForm.Show();
			this.MainForm.Activate();
			if (e.CommandLine.Count > 0)
			{
				string parameters = e.CommandLine[0];
				if (File.Exists(parameters))
				{
					((MainForm)this.MainForm).Controller.OpenAndPlay(parameters);
				}
			}
        }
    }
}
