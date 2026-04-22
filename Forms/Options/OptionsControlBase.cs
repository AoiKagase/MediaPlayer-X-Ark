using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
	public abstract class OptionsControlBase : UserControl
	{
		private float _appliedDpiScale = 1f;

		protected IPlayerEngine Engine { get; }
		protected IConfigService Config { get; }
		protected OptionsControlBase(IPlayerEngine engine, IConfigService config)
		{
			Engine = engine;
			Config = config;
			Dock = DockStyle.Fill;
			AutoScaleMode = AutoScaleMode.None;
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			ApplyDpiScaleIfNeeded();
		}

		public abstract void LoadSettings();
		public abstract void SaveSettings();

		private void ApplyDpiScaleIfNeeded()
		{
			float scale = DeviceDpi > 0 ? DeviceDpi / 96f : 1f;
			if (Math.Abs(scale - _appliedDpiScale) < 0.001f)
				return;

			float delta = scale / _appliedDpiScale;
			SuspendLayout();
			Scale(new SizeF(delta, delta));
			if (AutoScrollMinSize.Width > 0 || AutoScrollMinSize.Height > 0)
			{
				AutoScrollMinSize = new Size(
					ScaleLength(AutoScrollMinSize.Width, delta),
					ScaleLength(AutoScrollMinSize.Height, delta));
			}
			_appliedDpiScale = scale;
			ResumeLayout(true);
		}

		private static int ScaleLength(int value, float scale)
		{
			if (value <= 0)
				return value;

			return Math.Max(1, (int)Math.Round(value * scale, MidpointRounding.AwayFromZero));
		}
	}
}
