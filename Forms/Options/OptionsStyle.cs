using System.Drawing;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
	internal static class OptionsStyle
	{
		public static readonly Color PrimaryBlue = Color.FromArgb(0, 120, 215);
		public static readonly Size SaveButtonSize = new Size(80, 28);
		public const int ContentPadding = 16;

		public static void ApplyPrimaryButton(Button btn)
		{
			btn.Size = SaveButtonSize;
			btn.BackColor = PrimaryBlue;
			btn.ForeColor = Color.White;
			btn.FlatStyle = FlatStyle.Flat;
		}
	}
}