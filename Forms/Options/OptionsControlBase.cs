using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
	public abstract class OptionsControlBase : UserControl
	{
		protected IPlayerEngine Engine { get; }
		protected IConfigService Config { get; }
		protected OptionsControlBase(IPlayerEngine engine, IConfigService config)
		{
			Engine = engine;
			Config = config;
			Dock = DockStyle.Fill;
		}

		public abstract void LoadSettings();
		public abstract void SaveSettings();
	}
}
