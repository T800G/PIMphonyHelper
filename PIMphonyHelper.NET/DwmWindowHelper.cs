using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Text;
using System.Globalization;


namespace PIMphonyHelper
{
    public static class WindowHelper
    {
    	[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName,int nMaxCount);

		public static bool IsWindowClass(IntPtr hWnd, String className)
		{
		    int nRet;
		    StringBuilder cn = new StringBuilder(256);
		    nRet = GetClassName(hWnd, cn, cn.Capacity);
		    if(nRet != 0)
		    {
		    	return (string.Compare(cn.ToString(), className, true, CultureInfo.InvariantCulture) == 0);
		    }
		    return false;
		}

		
        [DllImport(@"dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out Rect pvAttribute, int cbAttribute);

        [Serializable, StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public Rectangle ToRectangle()
            {
                return Rectangle.FromLTRB(Left, Top, Right, Bottom);
            }
        }

        [DllImport(@"user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        private static Rectangle GetWindowRect(IntPtr handle)
        {
            Rect rect;
            GetWindowRect(handle, out rect);
            return rect.ToRectangle();
        }
        
        /// <summary>
        /// Get real window size, no matter whether Win XP, Win Vista, 7 or 8.
        /// </summary>
        public static Rectangle GetWindowRectangle(IntPtr handle)
        {
            if (Environment.OSVersion.Version.Major < 6)
            {
                return GetWindowRect(handle);
            }
            else
            {
                Rectangle rectangle;
                return DWMWA_EXTENDED_FRAME_BOUNDS(handle, out rectangle) ? rectangle : GetWindowRect(handle);
            }
        }

        private enum Dwmwindowattribute
        {
            DwmwaExtendedFrameBounds = 9
        }

        private static bool DWMWA_EXTENDED_FRAME_BOUNDS(IntPtr handle, out Rectangle rectangle)
        {
            Rect rect;
            var result = DwmGetWindowAttribute(handle, (int)Dwmwindowattribute.DwmwaExtendedFrameBounds, out rect, Marshal.SizeOf(typeof(Rect)));
            rectangle = rect.ToRectangle();
            return result >= 0;
        }
    }
}
