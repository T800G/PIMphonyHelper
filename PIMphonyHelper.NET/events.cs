using System;
using PIMphonyHelper;
using System.Diagnostics;

namespace PIMphonyHelper
{
	static class DebugHelper
	{
		static public String DbgGetWinEventObjectName(long idObject)
		{
			switch ((ObjectIdentifiers)idObject)
			{
			case ObjectIdentifiers.OBJID_WINDOW: return "OBJID_WINDOW";
			case ObjectIdentifiers.OBJID_SYSMENU: return "OBJID_SYSMENU";
			case ObjectIdentifiers.OBJID_TITLEBAR: return "OBJID_TITLEBAR";
			case ObjectIdentifiers.OBJID_MENU: return "OBJID_MENU";
			case ObjectIdentifiers.OBJID_CLIENT: return "OBJID_CLIENT";
			case ObjectIdentifiers.OBJID_VSCROLL: return "OBJID_VSCROLL";
			case ObjectIdentifiers.OBJID_HSCROLL: return "OBJID_HSCROLL";
			case ObjectIdentifiers.OBJID_SIZEGRIP: return "OBJID_SIZEGRIP";
			case ObjectIdentifiers.OBJID_CARET: return "OBJID_CARET";
			case ObjectIdentifiers.OBJID_CURSOR: return "OBJID_CURSOR";
			case ObjectIdentifiers.OBJID_ALERT: return "OBJID_ALERT";
			case ObjectIdentifiers.OBJID_SOUND: return "OBJID_SOUND";
			case ObjectIdentifiers.OBJID_QUERYCLASSNAMEIDX: return "OBJID_QUERYCLASSNAMEIDX";
			case ObjectIdentifiers.OBJID_NATIVEOM: return "OBJID_NATIVEOM";
			}
		return "";
		}
		static public void DbgTraceWinEventObjectName(long idObject) {Debug.WriteLine(DbgGetWinEventObjectName(idObject));}

		static public String DbgGetWinEventName(long dwEvent)
		{
			switch((EventConstants)dwEvent)
			{
				case EventConstants.EVENT_SYSTEM_SWITCHSTART: return "EVENT_SYSTEM_SWITCHSTART";
				case EventConstants.EVENT_SYSTEM_SWITCHEND: return "EVENT_SYSTEM_SWITCHEND";
				case EventConstants.EVENT_SYSTEM_SOUND: return "EVENT_SYSTEM_SOUND";
				case EventConstants.EVENT_SYSTEM_SCROLLINGSTART: return "EVENT_SYSTEM_SCROLLINGSTART";
				case EventConstants.EVENT_SYSTEM_SCROLLINGEND: return "EVENT_SYSTEM_SCROLLINGEND";
				case EventConstants.EVENT_SYSTEM_MOVESIZESTART: return "EVENT_SYSTEM_MOVESIZESTART";
				case EventConstants.EVENT_SYSTEM_MOVESIZEEND: return "EVENT_SYSTEM_MOVESIZEEND";
				case EventConstants.EVENT_SYSTEM_MINIMIZESTART: return "EVENT_SYSTEM_MINIMIZESTART";
				case EventConstants.EVENT_SYSTEM_MINIMIZEEND: return "EVENT_SYSTEM_MINIMIZEEND";
				case EventConstants.EVENT_SYSTEM_MENUSTART: return "EVENT_SYSTEM_MENUSTART";
				case EventConstants.EVENT_SYSTEM_MENUPOPUPSTART: return "EVENT_SYSTEM_MENUPOPUPSTART";
				case EventConstants.EVENT_SYSTEM_MENUPOPUPEND: return "EVENT_SYSTEM_MENUPOPUPEND";
				case EventConstants.EVENT_SYSTEM_MENUEND: return "EVENT_SYSTEM_MENUEND";
				case EventConstants.EVENT_SYSTEM_FOREGROUND: return "EVENT_SYSTEM_FOREGROUND";
				case EventConstants.EVENT_SYSTEM_END: return "EVENT_SYSTEM_END";
				case EventConstants.EVENT_SYSTEM_DRAGDROPSTART: return "EVENT_SYSTEM_DRAGDROPSTART";
				case EventConstants.EVENT_SYSTEM_DRAGDROPEND: return "EVENT_SYSTEM_DRAGDROPEND";
				case EventConstants.EVENT_SYSTEM_DIALOGSTART: return "EVENT_SYSTEM_DIALOGSTART";
				case EventConstants.EVENT_SYSTEM_DIALOGEND: return "EVENT_SYSTEM_DIALOGEND";
				case EventConstants.EVENT_SYSTEM_DESKTOPSWITCH: return "EVENT_SYSTEM_DESKTOPSWITCH";
				case EventConstants.EVENT_SYSTEM_CONTEXTHELPSTART: return "EVENT_SYSTEM_CONTEXTHELPSTART";
				case EventConstants.EVENT_SYSTEM_CONTEXTHELPEND: return "EVENT_SYSTEM_CONTEXTHELPEND";
				case EventConstants.EVENT_SYSTEM_CAPTURESTART: return "EVENT_SYSTEM_CAPTURESTART";
				case EventConstants.EVENT_SYSTEM_CAPTUREEND: return "EVENT_SYSTEM_CAPTUREEND";
				case EventConstants.EVENT_SYSTEM_ARRANGMENTPREVIEW: return "EVENT_SYSTEM_ARRANGMENTPREVIEW";
				case EventConstants.EVENT_SYSTEM_ALERT: return "EVENT_SYSTEM_ALERT";
				case EventConstants.EVENT_OBJECT_VALUECHANGE: return "EVENT_OBJECT_VALUECHANGE";
				case EventConstants.EVENT_OBJECT_UNCLOAKED: return "EVENT_OBJECT_UNCLOAKED";
				case EventConstants.EVENT_OBJECT_TEXTSELECTIONCHANGED: return "EVENT_OBJECT_TEXTSELECTIONCHANGED";
				case EventConstants.EVENT_OBJECT_TEXTEDIT_CONVERSIONTARGETCHANGED: return "EVENT_OBJECT_TEXTEDIT_CONVERSIONTARGETCHANGED";
				case EventConstants.EVENT_OBJECT_STATECHANGE: return "EVENT_OBJECT_STATECHANGE";
				case EventConstants.EVENT_OBJECT_SHOW: return "EVENT_OBJECT_SHOW";
				case EventConstants.EVENT_OBJECT_SELECTIONWITHIN: return "EVENT_OBJECT_SELECTIONWITHIN";
				case EventConstants.EVENT_OBJECT_SELECTIONREMOVE: return "EVENT_OBJECT_SELECTIONREMOVE";
				case EventConstants.EVENT_OBJECT_SELECTIONADD: return "EVENT_OBJECT_SELECTIONADD";
				case EventConstants.EVENT_OBJECT_SELECTION: return "EVENT_OBJECT_SELECTION";
				case EventConstants.EVENT_OBJECT_REORDER: return "EVENT_OBJECT_REORDER";
				case EventConstants.EVENT_OBJECT_PARENTCHANGE: return "EVENT_OBJECT_PARENTCHANGE";
				case EventConstants.EVENT_OBJECT_NAMECHANGE: return "EVENT_OBJECT_NAMECHANGE";
				case EventConstants.EVENT_OBJECT_LOCATIONCHANGE: return "EVENT_OBJECT_LOCATIONCHANGE";
				case EventConstants.EVENT_OBJECT_LIVEREGIONCHANGED: return "EVENT_OBJECT_LIVEREGIONCHANGED";
				case EventConstants.EVENT_OBJECT_INVOKED: return "EVENT_OBJECT_INVOKED";
				case EventConstants.EVENT_OBJECT_IME_SHOW: return "EVENT_OBJECT_IME_SHOW";
				case EventConstants.EVENT_OBJECT_IME_HIDE: return "EVENT_OBJECT_IME_HIDE";
				case EventConstants.EVENT_OBJECT_IME_CHANGE: return "EVENT_OBJECT_IME_CHANGE";
				case EventConstants.EVENT_OBJECT_HOSTEDOBJECTSINVALIDATED: return "EVENT_OBJECT_HOSTEDOBJECTSINVALIDATED";
				case EventConstants.EVENT_OBJECT_HIDE: return "EVENT_OBJECT_HIDE";
				case EventConstants.EVENT_OBJECT_HELPCHANGE: return "EVENT_OBJECT_HELPCHANGE";
				case EventConstants.EVENT_OBJECT_FOCUS: return "EVENT_OBJECT_FOCUS";
				case EventConstants.EVENT_OBJECT_END: return "EVENT_OBJECT_END";
				case EventConstants.EVENT_OBJECT_DRAGSTART: return "EVENT_OBJECT_DRAGSTART";
				case EventConstants.EVENT_OBJECT_DRAGLEAVE: return "EVENT_OBJECT_DRAGLEAVE";
				case EventConstants.EVENT_OBJECT_DRAGENTER: return "EVENT_OBJECT_DRAGENTER";
				case EventConstants.EVENT_OBJECT_DRAGDROPPED: return "EVENT_OBJECT_DRAGDROPPED";
				case EventConstants.EVENT_OBJECT_DRAGCOMPLETE: return "EVENT_OBJECT_DRAGCOMPLETE";
				case EventConstants.EVENT_OBJECT_DRAGCANCEL: return "EVENT_OBJECT_DRAGCANCEL";
				case EventConstants.EVENT_OBJECT_DESTROY: return "EVENT_OBJECT_DESTROY";
				case EventConstants.EVENT_OBJECT_DESCRIPTIONCHANGE: return "EVENT_OBJECT_DESCRIPTIONCHANGE";
				case EventConstants.EVENT_OBJECT_DEFACTIONCHANGE: return "EVENT_OBJECT_DEFACTIONCHANGE";
				case EventConstants.EVENT_OBJECT_CREATE: return "EVENT_OBJECT_CREATE";
				case EventConstants.EVENT_OBJECT_CONTENTSCROLLED: return "EVENT_OBJECT_CONTENTSCROLLED";
				case EventConstants.EVENT_OBJECT_CLOAKED: return "EVENT_OBJECT_CLOAKED";
				case EventConstants.EVENT_OBJECT_ACCELERATORCHANGE: return "EVENT_OBJECT_ACCELERATORCHANGE";
				case EventConstants.EVENT_OEM_DEFINED_START: return "EVENT_OEM_DEFINED_START";
				case EventConstants.EVENT_OEM_DEFINED_END: return "EVENT_OEM_DEFINED_END";
		    }
			return "";
		}
		static public void DbgTraceWinEventName(long dwEvent) {Debug.WriteLine(DbgGetWinEventName(dwEvent));}
	};
};