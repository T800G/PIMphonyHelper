#ifndef _MULTIMON_67D8181A_3E50_4D18_B406_34D6DB6113DE_
#define _MULTIMON_67D8181A_3E50_4D18_B406_34D6DB6113DE_

#define MONITOR_CENTER   0x0001        // center rect to monitor 
#define MONITOR_CLIP     0x0000        // clip rect to monitor 
#define MONITOR_WORKAREA 0x0002        // use monitor work area 
#define MONITOR_AREA     0x0000        // use monitor entire area 

// 
//  ClipOrCenterRectToMonitor 
// 
//  The most common problem apps have when running on a 
//  multimonitor system is that they "clip" or "pin" windows 
//  based on the SM_CXSCREEN and SM_CYSCREEN system metrics. 
//  Because of app compatibility reasons these system metrics 
//  return the size of the primary monitor. 
// 
//  This shows how you use the multi-monitor functions 
//  to do the same thing. 
// 
BOOL ClipOrCenterRectToMonitor(LPRECT prc, UINT flags)
{
    HMONITOR hMonitor;
    MONITORINFO mi;
    RECT        rc;
    int         w = prc->right  - prc->left;
    int         h = prc->bottom - prc->top;

    // 
    // get the nearest monitor to the passed rect. 
    // 
    hMonitor = MonitorFromRect(prc, MONITOR_DEFAULTTONEAREST);
	if (hMonitor==NULL) return FALSE;

    // 
    // get the work area or entire monitor rect. 
    // 
    mi.cbSize = sizeof(mi);
    if (!GetMonitorInfo(hMonitor, &mi)) return FALSE;

    if (flags & MONITOR_WORKAREA)
        rc = mi.rcWork;
    else
        rc = mi.rcMonitor;

    // 
    // center or clip the passed rect to the monitor rect 
    // 
    if (flags & MONITOR_CENTER)
    {
        prc->left   = rc.left + (rc.right  - rc.left - w) / 2;
        prc->top    = rc.top  + (rc.bottom - rc.top  - h) / 2;
        prc->right  = prc->left + w;
        prc->bottom = prc->top  + h;
    }
    else
    {
        prc->left   = max(rc.left, min(rc.right-w,  prc->left));
        prc->top    = max(rc.top,  min(rc.bottom-h, prc->top));
        prc->right  = prc->left + w;
        prc->bottom = prc->top  + h;
    }
	return TRUE;
}

#ifndef _DWMAPI_H_
typedef enum _DWMWINDOWATTRIBUTE { 
  DWMWA_NCRENDERING_ENABLED          = 1,
  DWMWA_NCRENDERING_POLICY,
  DWMWA_TRANSITIONS_FORCEDISABLED,
  DWMWA_ALLOW_NCPAINT,
  DWMWA_CAPTION_BUTTON_BOUNDS,
  DWMWA_NONCLIENT_RTL_LAYOUT,
  DWMWA_FORCE_ICONIC_REPRESENTATION,
  DWMWA_FLIP3D_POLICY,
  DWMWA_EXTENDED_FRAME_BOUNDS,
  DWMWA_HAS_ICONIC_BITMAP,
  DWMWA_DISALLOW_PEEK,
  DWMWA_EXCLUDED_FROM_PEEK,
  DWMWA_CLOAK,
  DWMWA_CLOAKED,
  DWMWA_FREEZE_REPRESENTATION,
  DWMWA_LAST
} DWMWINDOWATTRIBUTE;
#endif

BOOL GetDWMWindowRect(HWND hWnd, LPRECT lpRect)
{
	BOOL bRet=FALSE;
	HMODULE hDll = LoadLibrary(_T("Dwmapi.dll"));
	if (hDll == NULL) return bRet;
	typedef HRESULT (WINAPI* pfnDwmGWA)(HWND, DWORD, PVOID, DWORD);
	pfnDwmGWA pfnDwmGetWindowAttribute = (pfnDwmGWA)GetProcAddress(hDll, "DwmGetWindowAttribute");
	if (pfnDwmGetWindowAttribute)
	{
		if SUCCEEDED(pfnDwmGetWindowAttribute(hWnd, DWMWA_EXTENDED_FRAME_BOUNDS, lpRect, sizeof(RECT))) bRet=TRUE;
	}
	FreeLibrary(hDll);
return bRet;
}

BOOL ClipOrCenterWindowToMonitor(HWND hwnd, UINT flags)
{
    RECT rc;
    if ((GetDWMWindowRect(hwnd, &rc) || GetWindowRect(hwnd, &rc)) && ClipOrCenterRectToMonitor(&rc, flags))
		return SetWindowPos(hwnd, NULL, rc.left, rc.top, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE);
	else return FALSE;
}

#endif//_MULTIMON_67D8181A_3E50_4D18_B406_34D6DB6113DE_
