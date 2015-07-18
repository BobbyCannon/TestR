#region References

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

#endregion

namespace TestR.Native
{
	public static class Keyboard
	{
		#region Constants

		private const int KeyboardLowLevel = 13;
		private const int KeyDown = 0x0100;

		#endregion

		#region Fields

		private static readonly NativeMethods.LowLevelKeyboardProc _hook;
		private static IntPtr _hookId;

		#endregion

		#region Constructors

		static Keyboard()
		{
			_hookId = IntPtr.Zero;
			_hook = HookCallback;
		}

		#endregion

		#region Methods

		public static void StartMonitoring()
		{
			using (var curProcess = Process.GetCurrentProcess())
			{
				using (var curModule = curProcess.MainModule)
				{
					_hookId = NativeMethods.SetWindowsHookEx(KeyboardLowLevel, _hook, NativeMethods.GetModuleHandle(curModule.ModuleName), 0);
				}
			}
		}

		public static void StopMonitoring()
		{
			NativeMethods.UnhookWindowsHookEx(_hookId);
		}

		private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode < 0 || wParam != (IntPtr) KeyDown)
			{
				return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
			}

			var vkCode = Marshal.ReadInt32(lParam);
			var keyPressed = KeyInterop.KeyFromVirtualKey(vkCode);
			Trace.WriteLine(keyPressed);

			OnKeyPressed(keyPressed);

			return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
		}

		private static void OnKeyPressed(Key obj)
		{
			var handler = KeyPressed;
			if (handler != null)
			{
				handler(obj);
			}
		}

		#endregion

		#region Events

		public static event Action<Key> KeyPressed;

		#endregion
	}
}