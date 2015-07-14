#region References

using System;
using System.Runtime.InteropServices;
using System.Text;
using mshtml;
using SHDocVw;

#endregion

namespace TestR.Native
{
	internal static class NativeMethods
	{
		#region Fields

		private static readonly uint _gwOwner = 4;
		private static Guid _sidSTopLevelBrowser = new Guid(0x4C96BE40, 0x915C, 0x11CF, 0x99, 0xD3, 0x00, 0xAA, 0x00, 0x4A, 0xE8, 0x37);
		private static Guid _sidSWebBrowserApp = new Guid(0x0002DF05, 0x0000, 0x0000, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46);

		#endregion

		#region Methods

		public static IWebBrowser2 GetWebBrowserFromHtmlWindow(IHTMLWindow2 htmlWindow)
		{
			var guidIServiceProvider = typeof (IServiceProvider).GUID;
			var serviceProvider = htmlWindow as IServiceProvider;
			if (serviceProvider == null)
			{
				return null;
			}

			object objIServiceProvider;
			serviceProvider.QueryService(ref _sidSTopLevelBrowser, ref guidIServiceProvider, out objIServiceProvider);
			serviceProvider = objIServiceProvider as IServiceProvider;

			if (serviceProvider == null)
			{
				return null;
			}

			object objIWebBrowser;
			var guidIWebBrowser = typeof (IWebBrowser2).GUID;
			serviceProvider.QueryService(ref _sidSWebBrowserApp, ref guidIWebBrowser, out objIWebBrowser);
			var webBrowser = objIWebBrowser as IWebBrowser2;

			return webBrowser;
		}

		public static IntPtr GetWindowHandle(int processId)
		{
			var response = IntPtr.Zero;

			EnumWindows((handle, lParam) =>
			{
				uint windowsProcessId;
				GetWindowThreadProcessId(handle, out windowsProcessId);

				if (windowsProcessId != processId)
				{
					return true;
				}

				if (GetWindow(handle, _gwOwner) != IntPtr.Zero || !IsWindowVisible(handle))
				{
					return true;
				}

				response = handle;
				return false;
			}, IntPtr.Zero);

			if (response == IntPtr.Zero)
			{
				throw new Exception("Failed to find the window handle for the process ID.");
			}

			return response;
		}

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool BringWindowToTop(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildWindowProc lpEnumFunc, IntPtr lParam);

		[DllImport("user32", SetLastError = true)]
		internal static extern int GetClassName(IntPtr handleToWindow, StringBuilder className, int maxClassNameLength);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

		[DllImport("oleacc", SetLastError = true)]
		internal static extern int ObjectFromLresult(int lResult, ref Guid riid, int wParam, ref IHTMLDocument2 ppvObject);

		[DllImport("user32", SetLastError = true)]
		internal static extern int RegisterWindowMessage(string lpString);

		[DllImport("user32", SetLastError = true)]
		internal static extern int SendMessageTimeout(IntPtr hWnd, int msg, int wParam, int lParam, int fuFlags, int uTimeout, ref int lpdwResult);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr SetFocus(IntPtr hWnd);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool EnumWindows(EnumWindowProc lpEnumFunc, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		#endregion

		#region Delegates

		internal delegate bool EnumChildWindowProc(IntPtr hWnd, IntPtr lParam);

		private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

		#endregion

		#region Interfaces

		[ComImport]
		[Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IServiceProvider
		{
			#region Methods

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			uint QueryService(ref Guid guidService, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);

			#endregion
		}

		#endregion
	}
}