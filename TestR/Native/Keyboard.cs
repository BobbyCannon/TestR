﻿#region References

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		public static string[] Codes;

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

			Codes = new[]
			{
				"{BACKSPACE}", "{BS}", "{BKSP}", "{BREAK}", "{CAPSLOCK}", "{DELETE}", "{DEL}", "{DOWN}", "{END}", "{ENTER}", "{ESC}", "{HELP}", "{HOME}", "{INSERT}", "{INS}", "{LEFT}", "{NUMLOCK}", "{PGDN}", "{PGUP}",
				"{PRTSC}", "{RIGHT}", "{SCROLLLOCK}", "{TAB}", "{UP}", "{F1}", "{F2}", "{F3}", "{F4}", "{F5}", "{F6}", "{F7}", "{F8}", "{F9}", "{F10}", "{F11}", "{F12}", "{F13}", "{F14}", "{F15}", "{F16}", "{ADD}",
				"{SUBTRACT}", "{MULTIPLY}", "{DIVIDE}", "{~}", "{(}", "{)}", "{+}", "{^}", "{%}", "{[}", "{}}", "{{}", "{}}"
			};
		}

		#endregion

		#region Methods

		/// <summary>
		/// Updates the text in preparation for TypeText method.
		/// </summary>
		/// <param name="value"> The value to cleanup. </param>
		/// <returns> The value that is ready to pass to SendKeys. </returns>
		public static string FormatTextForTypeText(string value)
		{
			var builder = new StringBuilder(value.Length * 2);
			var formatBuilder = new StringBuilder(value.Length);

			for (var i = 0; i < value.Length; i++)
			{
				var c = value[i];

				switch (c)
				{
					case '\x08':
						builder.Append("{BS}");
						continue;

					case '\x13':
						builder.Append("{BREAK}");
						continue;

					case '\x10':
						builder.Append("{CAPSLOCK}");
						continue;
					
					case '\x7F':
						builder.Append("{DEL}");
						continue;

					case '~':
					case '(':
					case ')':
					case '+':
					case '^':
					case '%':
					case '[':
					case ']':
					case '}':
						builder.Append($"{{{c}}}");
						continue;
					
					case '{':
						formatBuilder.Clear();

						// look ahead to see if we are an existing format
						for (var j = i; j < value.Length; j++)
						{
							formatBuilder.Append(value[j]);

							if (value[j] == '}' && formatBuilder.Length > 2)
							{
								break;
							}
						}

						var format = formatBuilder.ToString();
						if (Codes.Contains(format))
						{
							builder.Append(format);
							i += format.Length - 1;
							continue;
						}

						builder.Append($"{{{c}}}");
						continue;
				}

				builder.Append(c);
			}

			return builder.ToString();
		}

		/// <summary>
		/// Determines if the control key is pressed.
		/// </summary>
		/// <returns>
		/// True if either control key is pressed and false if otherwise.
		/// </returns>
		public static bool IsControlPressed()
		{
			var result1 = NativeMethods.GetKeyState(NativeMethods.VirtualKeyStates.VK_LCONTROL);
			var result2 = NativeMethods.GetKeyState(NativeMethods.VirtualKeyStates.VK_RCONTROL);
			return (result1 & 0x8000) == 0x8000 || (result2 & 0x8000) == 0x8000;
		}

		/// <summary>
		/// Determines if the shift key is pressed.
		/// </summary>
		/// <returns>
		/// True if either shift key is pressed and false if otherwise.
		/// </returns>
		public static bool IsShiftPressed()
		{
			var result1 = NativeMethods.GetKeyState(NativeMethods.VirtualKeyStates.VK_LSHIFT);
			var result2 = NativeMethods.GetKeyState(NativeMethods.VirtualKeyStates.VK_RSHIFT);
			return (result1 & 0x8000) == 0x8000 || (result2 & 0x8000) == 0x8000;
		}

		/// <summary>
		/// Start monitoring the keyboard for keystrokes.
		/// </summary>
		public static void StartMonitoring()
		{
			using (var curProcess = ProcessService.GetCurrentProcess())
			{
				using (var curModule = curProcess.Process.MainModule)
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

		private static int HookCallback(int nCode, int wParam, ref NativeMethods.KeyboardHookStruct lParam)
		{
			if (nCode >= 0)
			{
				var key = (Keys) lParam.vkCode;
				var kea = new KeyEventArgs(key);
				var keyPressed = KeyInterop.KeyFromVirtualKey(kea.KeyValue);

				KeyPressed?.Invoke(keyPressed);
			}

			return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, ref lParam);
		}

		#endregion

		#region Events

		/// <summary>
		/// Event for key press events when monitoring the keyboard.
		/// </summary>
		public static event Action<Key> KeyPressed;

		#endregion

		#region Classes

		/// <summary>
		/// Represents specials keys to send when typing text.
		/// </summary>
		public static class SpecialKeys
		{
			#region Constants

			/// <summary>
			/// Represents an CTRL key press.
			/// </summary>
			public const string Alt = "%";

			/// <summary>
			/// Represents an BACKSPACE key press.
			/// </summary>
			public const string Backspace = "{BACKSPACE}";

			/// <summary>
			/// Represents an BREAK key press.
			/// </summary>
			public const string Break = "{BREAK}";

			/// <summary>
			/// Represents an CAPS LOCK key press.
			/// </summary>
			public const string CapsLock = "{CAPSLOCK}";

			/// <summary>
			/// Represents an CTRL key press.
			/// </summary>
			public const string Control = "^";

			/// <summary>
			/// Represents an DELETE key press.
			/// </summary>
			public const string Delete = "{DELETE}";

			/// <summary>
			/// Represents an DOWN ARROW key press.
			/// </summary>
			public const string DownArrow = "{DOWN}";

			/// <summary>
			/// Represents an END key press.
			/// </summary>
			public const string End = "{END}";

			/// <summary>
			/// Represents an ENTER key press.
			/// </summary>
			public const string Enter = "{ENTER}";

			/// <summary>
			/// Represents an ESC key press.
			/// </summary>
			public const string Esc = "{ESC}";

			/// <summary>
			/// Represents an F1 key press.
			/// </summary>
			public const string F1 = "{F1}";

			/// <summary>
			/// Represents an F10 key press.
			/// </summary>
			public const string F10 = "{F10}";

			/// <summary>
			/// Represents an F11 key press.
			/// </summary>
			public const string F11 = "{F11}";

			/// <summary>
			/// Represents an F12 key press.
			/// </summary>
			public const string F12 = "{F12}";

			/// <summary>
			/// Represents an F13 key press.
			/// </summary>
			public const string F13 = "{F13}";

			/// <summary>
			/// Represents an F14 key press.
			/// </summary>
			public const string F14 = "{F14}";

			/// <summary>
			/// Represents an F15 key press.
			/// </summary>
			public const string F15 = "{F15}";

			/// <summary>
			/// Represents an F16 key press.
			/// </summary>
			public const string F16 = "{F16}";

			/// <summary>
			/// Represents an F2 key press.
			/// </summary>
			public const string F2 = "{F2}";

			/// <summary>
			/// Represents an F3 key press.
			/// </summary>
			public const string F3 = "{F3}";

			/// <summary>
			/// Represents an F4 key press.
			/// </summary>
			public const string F4 = "{F4}";

			/// <summary>
			/// Represents an F5 key press.
			/// </summary>
			public const string F5 = "{F5}";

			/// <summary>
			/// Represents an F6 key press.
			/// </summary>
			public const string F6 = "{F6}";

			/// <summary>
			/// Represents an F7 key press.
			/// </summary>
			public const string F7 = "{F7}";

			/// <summary>
			/// Represents an F8 key press.
			/// </summary>
			public const string F8 = "{F8}";

			/// <summary>
			/// Represents an F9 key press.
			/// </summary>
			public const string F9 = "{F9}";

			/// <summary>
			/// Represents an HELP key press.
			/// </summary>
			public const string Help = "{HELP}";

			/// <summary>
			/// Represents an HOME key press.
			/// </summary>
			public const string Home = "{HOME}";

			/// <summary>
			/// Represents an INSERT key press.
			/// </summary>
			public const string Insert = "{INSERT}";

			/// <summary>
			/// Represents an Keypad add key press.
			/// </summary>
			public const string KeypadAdd = "{ADD}";

			/// <summary>
			/// Represents an Keypad divide key press.
			/// </summary>
			public const string KeypadDivide = "{DIVIDE}";

			/// <summary>
			/// Represents an Keypad multiply key press.
			/// </summary>
			public const string KeypadMultiply = "{MULTIPLY}";

			/// <summary>
			/// Represents an Keypad subtract key press.
			/// </summary>
			public const string KeypadSubtract = "{SUBTRACT}";

			/// <summary>
			/// Represents an LEFT ARROW key press.
			/// </summary>
			public const string LeftArrow = "{LEFT}";

			/// <summary>
			/// Represents an NUM LOCK key press.
			/// </summary>
			public const string NumLock = "{NUMLOCK}";

			/// <summary>
			/// Represents an PAGE DOWN key press.
			/// </summary>
			public const string PageDown = "{PGDN}";

			/// <summary>
			/// Represents an PAGE UP key press.
			/// </summary>
			public const string PageUp = "{PGUP}";

			/// <summary>
			/// Represents an PRINT SCREEN key press.
			/// </summary>
			public const string PrintScreen = "{PRTSC}";

			/// <summary>
			/// Represents an RIGHT ARROW key press.
			/// </summary>
			public const string RightArrow = "{RIGHT}";

			/// <summary>
			/// Represents an SCROLL LOCK key press.
			/// </summary>
			public const string ScrollLock = "{SCROLLLOCK}";

			/// <summary>
			/// Represents an SHIFT key press.
			/// </summary>
			public const string Shift = "+";

			/// <summary>
			/// Represents an TAB key press.
			/// </summary>
			public const string Tab = "{TAB}";

			/// <summary>
			/// Represents an UP ARROW key press.
			/// </summary>
			public const string UpArrow = "{UP}";

			#endregion
		}

		#endregion
	}
}