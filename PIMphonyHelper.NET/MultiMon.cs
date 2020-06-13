using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace PIMphonyHelper
{
	public static class ClipOrCenterFlags
	{
	    public const uint MONITOR_CENTER   = 0x0001; // center rect to monitor
		public const uint MONITOR_CLIP     = 0x0000; // clip rect to monitor
		public const uint MONITOR_WORKAREA = 0x0002; // use monitor work area
		public const uint MONITOR_AREA     = 0x0000; // use monitor entire area
	}

	public static class MultiMon
	{
		[DllImport("user32.dll", SetLastError=true)]
		static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
		
		
		public static void ClipOrCenterRectToMonitor(ref Rectangle prc, uint flags)
		{
			int w = prc.Right  - prc.Left;
			int h = prc.Bottom - prc.Top;
			
			Rectangle rc;
			if ((flags & (uint)ClipOrCenterFlags.MONITOR_WORKAREA) != 0)
				rc = System.Windows.Forms.Screen.FromRectangle(prc).WorkingArea;
			else 
				rc = System.Windows.Forms.Screen.FromRectangle(prc).Bounds;
			
			int l,t,r,b;
			
		    if ((flags & (uint)ClipOrCenterFlags.MONITOR_CENTER) != 0)
		    {
		        l = rc.Left + (rc.Width - w) / 2;
		        t = rc.Top  + (rc.Height - h) / 2;
		        r = prc.Left + w;
		        b = prc.Top  + h;
		    }
		    else
		    {
		        l = Math.Max(rc.Left, Math.Min(rc.Right - w,  prc.Left));
		        t = Math.Max(rc.Top,  Math.Min(rc.Bottom - h, prc.Top));
		        r = prc.Left + w;
		        b = prc.Top  + h;		        
		    }
		    
		    prc = new Rectangle(l, t, (r-l), (b-t));
		}
		
		public static void ClipOrCenterWindowToMonitor(IntPtr hwnd, uint flags)
		{
			Rectangle rc = WindowHelper.GetWindowRectangle(hwnd);
			ClipOrCenterRectToMonitor(ref rc, flags);
			SetWindowPos(hwnd, IntPtr.Zero, rc.Left, rc.Top, 0, 0, (uint)(SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOACTIVATE));
		}

		public static void ClipOrCenterWindowToMonitor(IntPtr hwnd, uint flags, uint swpFlags)
		{
			Rectangle rc = WindowHelper.GetWindowRectangle(hwnd);
			ClipOrCenterRectToMonitor(ref rc, flags);
		    SetWindowPos(hwnd, IntPtr.Zero, rc.Left, rc.Top, 0, 0, (uint)(SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOACTIVATE) | swpFlags);
		}
	}
}
