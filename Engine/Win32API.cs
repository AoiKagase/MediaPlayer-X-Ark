using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace MediaPlayer_X_Ark
{
	public static class Win32API
    {
		public enum TernaryRasterOperations : uint
		{
			SRCCOPY = 0x00CC0020,
			SRCPAINT = 0x00EE0086,
			SRCAND = 0x008800C6,
			SRCINVERT = 0x00660046,
			SRCERASE = 0x00440328,
			NOTSRCCOPY = 0x00330008,
			NOTSRCERASE = 0x001100A6,
			MERGECOPY = 0x00C000CA,
			MERGEPAINT = 0x00BB0226,
			PATCOPY = 0x00F00021,
			PATPAINT = 0x00FB0A09,
			PATINVERT = 0x005A0049,
			DSTINVERT = 0x00550009,
			BLACKNESS = 0x00000042,
			WHITENESS = 0x00FF0062,
			CAPTUREBLT = 0x40000000
		}
		[DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string path);
		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight,
		  IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);
		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PatBlt(IntPtr hdc, int x, int y, int width, int height, int rop);
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowDC(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern IntPtr GetDC(IntPtr hwnd);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool DeleteDC(IntPtr hdc);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		[DllImport("gdi32.dll")]
		public static extern IntPtr DeleteObject(IntPtr hObject);

		[DllImport("user32.dll")]
		public static extern int FillRect(IntPtr hDC, [In] ref RECT lprc, IntPtr hbr);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateSolidBrush(uint crColor);

		[DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileStringW", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpresulturnedString, uint nSize, string lpFileName);
		[DllImport("kernel32.DLL")]
		public static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool SetDllDirectory(string lpPathName);

		public static readonly IntPtr HWND_TOP = IntPtr.Zero;
		public const uint SWP_NOSIZE = 0x0001;
		public const uint SWP_NOMOVE = 0x0002;
		public const uint SWP_NOACTIVATE = 0x0010;
		public const uint SWP_SHOWWINDOW = 0x0040;

		[DllImport("user32.dll")]
		public static extern bool SetWindowPos(
			IntPtr hWnd,
			IntPtr hWndInsertAfter,
			int X,
			int Y,
			int cx,
			int cy,
			uint uFlags);

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);
		public const int WM_SETREDRAW = 0x000B;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int Left, Top, Right, Bottom;

		public RECT(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

		public int X
		{
			get { return Left; }
			set { Right -= (Left - value); Left = value; }
		}

		public int Y
		{
			get { return Top; }
			set { Bottom -= (Top - value); Top = value; }
		}

		public int Height
		{
			get { return Bottom - Top; }
			set { Bottom = value + Top; }
		}

		public int Width
		{
			get { return Right - Left; }
			set { Right = value + Left; }
		}

		public System.Drawing.Point Location
		{
			get { return new System.Drawing.Point(Left, Top); }
			set { X = value.X; Y = value.Y; }
		}

		public System.Drawing.Size Size
		{
			get { return new System.Drawing.Size(Width, Height); }
			set { Width = value.Width; Height = value.Height; }
		}

		public static implicit operator System.Drawing.Rectangle(RECT r)
		{
			return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
		}

		public static implicit operator RECT(System.Drawing.Rectangle r)
		{
			return new RECT(r);
		}

		public static bool operator ==(RECT r1, RECT r2)
		{
			return r1.Equals(r2);
		}

		public static bool operator !=(RECT r1, RECT r2)
		{
			return !r1.Equals(r2);
		}

		public bool Equals(RECT r)
		{
			return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
		}

		public override bool Equals(object obj)
		{
			if (obj is RECT)
				return Equals((RECT)obj);
			else if (obj is System.Drawing.Rectangle)
				return Equals(new RECT((System.Drawing.Rectangle)obj));
			return false;
		}

		public override int GetHashCode()
		{
			return ((System.Drawing.Rectangle)this).GetHashCode();
		}

		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
		}
	}
}
