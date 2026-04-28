using System.Drawing;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark
{
	internal static class ApplicationIcon
	{
		public static void ApplyTo(Form form)
		{
			var icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
			if (icon == null)
				return;

			form.Icon = icon;
		}

		public static void ApplyTo(Form form, NotifyIcon notifyIcon)
		{
			ApplyTo(form);
			notifyIcon.Icon = form.Icon;
		}
	}
}
