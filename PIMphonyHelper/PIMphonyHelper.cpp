#include "stdafx.h"
#include <shellapi.h>
#include "resource.h"
#include "multimon.h"
#include "events.h"

#ifdef _DEBUG
#define DBGTRACE(x) OutputDebugString(_T(x))
#define DBGTRACEW(x) OutputDebugStringW(x)
#else
#define DBGTRACE(x)
#define DBGTRACEW(x)
#endif

// Global Variables:
WNDCLASSEX g_wcex; //reuse g_wcex.hInstance and g_wcex.hIcon
#define DPS_WNDCLASS_NAME  _T("PIMphonyHelperWndClass")
HMENU g_htrayMenu;
NOTIFYICONDATA  g_trayicon;
#define WM_TRAY_NOTIFY (WM_APP+11)

HWND g_lastWnd;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Global variable.
HWINEVENTHOOK g_hWinEventHook;

#ifdef _DEBUG
#define PIMPHONYWNDCLASSNAME  _T("ThunderRT6FormDC")
#else
#define PIMPHONYWNDCLASSNAME  _T("ThunderRT6Form")
#endif

// Callback function that handles events.
//
void CALLBACK WinEventProcCallback(HWINEVENTHOOK hook, DWORD dwEvent, HWND hwnd, 
                             LONG idObject, LONG idChild, 
                             DWORD dwEventThread, DWORD dwmsEventTime)
{
	//DBGTRACEEVENT(dwEvent);

	if ((idObject == OBJID_WINDOW) && (idChild == CHILDID_SELF))
		switch (dwEvent)
		{
		case EVENT_SYSTEM_FOREGROUND:
			{
				//DBGTRACE("EVENT_SYSTEM_FOREGROUND\n");

		//Wnd class: ThunderRT6Form
		//Wnd caption: PIMphony Basic (**********, ***)
		//Process: C:\Program Files (x86)\Alcatel_PIMphony\aocphone.exe

				//https://support.microsoft.com/en-us/help/99456/win32-equivalents-for-c-run-time-functions

				//sizeof hardcoded string!!!  (wchar 2 bytes!)
				//https://stackoverflow.com/questions/1392200/sizeof-string-literal

				if ((GetClassName(hwnd, g_trayicon.szTip, 128)==(sizeof(PIMPHONYWNDCLASSNAME)/sizeof(TCHAR)-1) ) && (lstrcmp(g_trayicon.szTip, PIMPHONYWNDCLASSNAME)==0))
				{
					if (g_lastWnd == hwnd)
					{
						ClipOrCenterWindowToMonitor(hwnd, MONITOR_CLIP|MONITOR_AREA);
					}
					else
					{
						g_lastWnd = hwnd;
						ClipOrCenterWindowToMonitor(hwnd, MONITOR_CENTER|MONITOR_AREA);				
					}
				}
			}
			break;
		//case EVENT_OBJECT_HIDE:
		//	if (g_lastWnd == hwnd)
		//	{
		//		DBGTRACE("EVENT_OBJECT_HIDE\n");
		//	}
		//	break;
		case EVENT_OBJECT_DESTROY:
			if (g_lastWnd == hwnd)
			{
				DBGTRACE("EVENT_OBJECT_DESTROY\n");
				g_lastWnd = NULL;
			}
			break;
		}

}

// Initializes COM and sets up the event hook.
//
void InitializeMSAA()
{
	if (g_hWinEventHook==NULL)
	{
		g_hWinEventHook = SetWinEventHook(EVENT_MIN, EVENT_MAX /*EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND*/,
											NULL,  // handle to DLL
											WinEventProcCallback,
											0, 0, // process and thread IDs of interest (0 = all)
											WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS);
	}
}

// Unhooks the event and shuts down COM.
//
void ShutdownMSAA()
{
    if (g_hWinEventHook)
	{
		UnhookWinEvent(g_hWinEventHook);
	}
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////

//message-only window proc
LRESULT CALLBACK TrayWndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	switch (message)
	{
		case WM_COMMAND://menu Exit command
			//DBGTRACE("IDM_EXIT\n");
			//if (IDYES==::MessageBox(HWND_DESKTOP, _T("Exit?"), _T("PIMphony Window Helper"), MB_YESNO | MB_SYSTEMMODAL | MB_ICONQUESTION | MB_DEFBUTTON2))
				DestroyWindow(hWnd);
			//SetProcessWorkingSetSize(GetCurrentProcess(),(SIZE_T)-1,(SIZE_T)-1);
		break;

		case WM_TRAY_NOTIFY:
			if (LOWORD(lParam)==WM_RBUTTONUP)
			{
				//DBGTRACE("WM_RBUTTONUP\n");
				POINT mousepos;
				if (!GetCursorPos(&mousepos)) return 0;
				//MSDN Q135788 fix
				::SetForegroundWindow(hWnd);  
				BOOL bT=::TrackPopupMenuEx(GetSubMenu(g_htrayMenu,0),TPM_RIGHTALIGN,mousepos.x, mousepos.y, hWnd, NULL);
				::PostMessage(hWnd, WM_NULL, 0, 0);
				SetProcessWorkingSetSize(GetCurrentProcess(), (SIZE_T)-1, (SIZE_T)-1);
			}
		break;

		case WM_ENDSESSION:
			if (wParam==TRUE) { ShutdownMSAA(); DestroyWindow(hWnd);}
		break;

		case WM_DESTROY:
			PostQuitMessage(0);
		break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
	}
return 0;
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////

//main
#ifdef _ATL_MIN_CRT //release build
int WINAPI WinMainCRTStartup(void)//no crt
#else
int APIENTRY _tWinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPTSTR lpCmdLine, int nCmdShow)
#endif
{
	CoInitialize(NULL);
	
	g_lastWnd=NULL;

	//init structs
	SecureZeroMemory(&g_wcex, sizeof(WNDCLASSEX));
	SecureZeroMemory(&g_trayicon, sizeof(NOTIFYICONDATA));

	g_wcex.cbSize			= sizeof(WNDCLASSEX);
	g_wcex.hInstance		= GetModuleHandle(NULL);
	g_wcex.lpfnWndProc	= TrayWndProc;
	g_wcex.lpszMenuName	= MAKEINTRESOURCE(IDC_MAIN);
	g_wcex.lpszClassName	= DPS_WNDCLASS_NAME;
	g_wcex.hIconSm = (HICON)::LoadImage(g_wcex.hInstance, MAKEINTRESOURCE(IDI_MAIN),
					IMAGE_ICON, ::GetSystemMetrics(SM_CXSMICON), ::GetSystemMetrics(SM_CYSMICON), LR_DEFAULTCOLOR);

	if (!RegisterClassEx(&g_wcex)) return 0;


	//create message-only window
	g_trayicon.hWnd = CreateWindow(g_wcex.lpszClassName, NULL, WS_OVERLAPPEDWINDOW, CW_USEDEFAULT,
									0, CW_USEDEFAULT, 0, HWND_MESSAGE, NULL, g_wcex.hInstance, NULL);
	if (!g_trayicon.hWnd) return FALSE;


	g_htrayMenu=::GetMenu(g_trayicon.hWnd);
	
	//MessageBox(NULL, GetCommandLine(), NULL,MB_OK);
	if (_tcsstr(GetCommandLine(), _T("/notray"))==NULL)
	{
		//fill tray icon info
		g_trayicon.cbSize=sizeof(NOTIFYICONDATA);
		g_trayicon.uFlags=(NIF_ICON | NIF_MESSAGE | NIF_TIP);
		//g_trayicon.hWnd already set
		g_trayicon.hIcon=g_wcex.hIconSm;
		g_trayicon.dwInfoFlags=NIIF_USER;
		g_trayicon.uCallbackMessage=WM_TRAY_NOTIFY;
		g_trayicon.uTimeout=3000;//ms
		LoadString(GetModuleHandle(NULL), IDS_TRAYTOOLTIP, g_trayicon.szTip, sizeof(g_trayicon.szTip)/sizeof(TCHAR));
		//add tray icon
		Shell_NotifyIcon(NIM_ADD, &g_trayicon);	
	}

	InitializeMSAA();

	//minimize memory usage
	SetProcessWorkingSetSize(GetCurrentProcess(), (SIZE_T)-1, (SIZE_T)-1);

	// Main message loop:
	MSG msg;
	while (GetMessage(&msg, NULL, 0, 0))
	{
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}

	ShutdownMSAA();

	//cleanup
	if (g_trayicon.cbSize) Shell_NotifyIcon(NIM_DELETE, &g_trayicon);
	
	CoUninitialize();
#ifdef _ATL_MIN_CRT
	ExitProcess(0);
#endif
return 0;
}
