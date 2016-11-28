#region References

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;

#endregion

namespace TestR.Native
{
	/// <summary>
	/// Represents the keyboard and allows for simulated input.
	/// </summary>
	public static class Keyboard
	{
		#region Constants

		private const int KeyboardLowLevel = 13;
		private const int KeyDown = 0x0100;

		#endregion

		#region Fields

		private static readonly NativeMethods.HookDelegate _hook;
		private static int _hookId;

		#endregion

		#region Constructors

		static Keyboard()
		{
			_hookId = 0;
			_hook = HookCallback;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Determinse if the control key is pressed.
		/// </summary>
		/// <returns>
		/// True if either control key is pressed.
		/// </returns>
		public static bool IsControlPressed()
		{
			var result1 = NativeMethods.GetKeyState(NativeMethods.VirtualKeyStates.VK_LCONTROL);
			var result2 = NativeMethods.GetKeyState(NativeMethods.VirtualKeyStates.VK_RCONTROL);
			return ((result1 & 0x8000) == 0x8000) || ((result2 & 0x8000) == 0x8000);
		}

		/// <summary>
		/// Start monitoring the keyboard for keystrokes.
		/// </summary>
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

		/// <summary>
		/// Stop monitoring the keyboard for keystrokes.
		/// </summary>
		public static void StopMonitoring()
		{
			NativeMethods.UnhookWindowsHookEx(_hookId);
		}

		/// <summary>
		/// Types text as keyboard input.
		/// </summary>
		/// <param name="value"> </param>
		public static void TypeText(string value)
		{
			SendKeys.SendWait(value);
		}

		private static int HookCallback(int nCode, int wParam, IntPtr lParam)
		{
			if ((nCode < 0) || (wParam != KeyDown))
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
			handler?.Invoke(obj);
		}

		#endregion

		#region Events

		/// <summary>
		/// Event for key press events when monitoring the keyboard.
		/// </summary>
		public static event Action<Key> KeyPressed;

		#endregion
	}
}