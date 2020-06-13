using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Linq;

namespace PIMphonyHelper
{
	public sealed class WinEventHook
	{	
		#if DEBUG
		public const String APPWNDCLASSNAME = "ThunderRT6FormDC";
		#else
		public const String APPWNDCLASSNAME =  "ThunderRT6Form";
		#endif
		
		[DllImport("user32.dll")]
			public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess,uint idThread, uint dwFlags);
		[DllImport("user32.dll")]
			public static extern bool UnhookWinEvent(IntPtr hWinEventHook);
		[DllImport("kernel32.dll", SetLastError = true)]
  			static extern uint GetCurrentThreadId();
  			
//[DllImport("user32.dll")]
//	public static extern IntPtr GetForegroundWindow();


 		EventConstants[] arrevt = {
	
//	 		EventConstants.EVENT_OBJECT_CREATE,
//			EventConstants.EVENT_OBJECT_DESTROY,
//			EventConstants.EVENT_OBJECT_SHOW,
//			EventConstants.EVENT_OBJECT_HIDE,
	
 			EventConstants.EVENT_SYSTEM_FOREGROUND,
 			EventConstants.EVENT_OBJECT_DESTROY
	 		};
		
		public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
		static WinEventDelegate procDelegate = new WinEventDelegate(WinEventProc);	
   

		private static IntPtr m_lastWnd;
		private static IntPtr m_hook;
		
		public WinEventHook()
		{
			m_lastWnd = IntPtr.Zero;
			m_hook = IntPtr.Zero;
			//TODO find last event with old foreground window
			m_hook = SetWinEventHook((uint)arrevt.Min(), (uint)arrevt.Max(), IntPtr.Zero, procDelegate, 0, 0, (uint)(EventHookFlags.WINEVENT_OUTOFCONTEXT | EventHookFlags.WINEVENT_SKIPOWNPROCESS));

		}
		~WinEventHook()
		{
			UnhookWinEvent(m_hook);
		}

	
		static void	WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
		{
			// Did this event happen on the same thread we are on...Could be some issues if the CLR thread does not match the native thread.
			if (GetCurrentThreadId() == dwEventThread) return;

			if ((idObject == (int)ObjectIdentifiers.OBJID_WINDOW) && (idChild == (int)CommonObjectIDs.CHILDID_SELF))
			{
				
				//DebugHelper.DbgTraceWinEventName(eventType);

				switch ((EventConstants)eventType)
				{
					case EventConstants.EVENT_SYSTEM_FOREGROUND:
						if (WindowHelper.IsWindowClass(hwnd, APPWNDCLASSNAME))
						{
							if (m_lastWnd == hwnd)
							{
								MultiMon.ClipOrCenterWindowToMonitor(hwnd, ClipOrCenterFlags.MONITOR_CLIP | ClipOrCenterFlags.MONITOR_WORKAREA);
							}
							else
							{
								m_lastWnd = hwnd;
								MultiMon.ClipOrCenterWindowToMonitor(hwnd, (uint)(ClipOrCenterFlags.MONITOR_CENTER | ClipOrCenterFlags.MONITOR_WORKAREA), (uint)SetWindowPosFlags.SWP_SHOWWINDOW);
							}
						}
					break;
					case EventConstants.EVENT_OBJECT_DESTROY:
						if (m_lastWnd == hwnd)
						{
							//Debug.WriteLine("EVENT_OBJECT_DESTROY=" + hwnd.ToString("X4"));
							m_lastWnd = IntPtr.Zero;
						}
					break;
				};
			}
		}
	}
}
