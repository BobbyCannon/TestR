#region References

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

namespace TestR.Native
{
	internal static class NativeMethods
	{
		#region Methods

		[DllImport("user32.dll")]
		public static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool BringWindowToTop(IntPtr hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		internal static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "GetCursorPos", SetLastError = true)]
		internal static extern bool GetCursorPosition(out System.Drawing.Point lpMousePoint);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr GetForegroundWindow();

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr GetModuleHandle(string lpModuleName);

		internal static WindowPlacement GetWindowPlacement(IntPtr handle)
		{
			var placement = new WindowPlacement();
			placement.length = Marshal.SizeOf(placement);
			GetWindowPlacement(handle, ref placement);
			return placement;
		}

		[DllImport("User32.dll")]
		internal static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

		[DllImport("user32.dll")]
		internal static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("user32.dll", EntryPoint = "mouse_event")]
		internal static extern void MouseEvent(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

		[DllImport("user32.dll", EntryPoint = "SetCursorPos")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetCursorPosition(int x, int y);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		internal static extern int SetWindowsHookEx(int idHook, HookDelegate lpfn, IntPtr hMod, int dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		internal static extern int UnhookWindowsHookEx(int idHook);
		
		[DllImport("User32.dll")]
		private static extern bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);

		[DllImport("USER32.dll")]
		internal static extern int GetKeyState(VirtualKeyStates keyState);

		#endregion

		#region Delegates

		public delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

		public delegate int HookDelegate(int code, int wParam, IntPtr lParam);

		#endregion

		#region Enumerations

		internal enum VirtualKeyStates
		{
			VK_LSHIFT = 0xA0,
			VK_RSHIFT = 0xA1,
			VK_LCONTROL = 0xA2,
			VK_RCONTROL = 0xA3,
		}
		
		internal enum MouseMessages
		{
			WM_LBUTTONDOWN = 0x0201,
			WM_LBUTTONUP = 0x0202,
			WM_MOUSEMOVE = 0x0200,
			WM_MOUSEWHEEL = 0x020A,
			WM_RBUTTONDOWN = 0x0204,
			WM_RBUTTONUP = 0x0205
		}

		#endregion

		[StructLayout(LayoutKind.Sequential)]
		internal struct WindowPlacement
		{
			#region Fields

			public int length;
			public int flags;
			public int ShowState;
			public System.Drawing.Point ptMinPosition;
			public System.Drawing.Point ptMaxPosition;
			public System.Drawing.Rectangle rcNormalPosition;

			#endregion
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct Rect
		{
			#region Fields

			public int Left;        // x position of upper-left corner
			public int Top;         // y position of upper-left corner
			public int Right;       // x position of lower-right corner
			public int Bottom;      // y position of lower-right corner

			#endregion
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct MouseHook
		{
			#region Fields

			public Point pt;
			public uint mouseData;
			public uint flags;
			public uint time;
			public IntPtr dwExtraInfo;

			#endregion
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct Point
		{
			#region Fields

			public int x;
			public int y;

			#endregion
		}
	}
}