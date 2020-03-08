#region References

using System.Drawing;
using System.Runtime.InteropServices;

#endregion

namespace TestR.Internal.Native
{
	internal class NativeInput
	{
		#region Methods

		[DllImport("user32.dll", SetLastError = true)]
		public static extern short GetAsyncKeyState(ushort virtualKeyCode);

		[DllImport("user32.dll", EntryPoint = "GetCursorPos", SetLastError = true)]
		public static extern bool GetCursorPosition(out Point lpMousePoint);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern short GetKeyState(ushort virtualKeyCode);

		[DllImport("user32.dll")]
		public static extern uint MapVirtualKey(uint uCode, uint uMapType);

		[DllImport("user32.dll", SetLastError = true, EntryPoint = "SetCursorPos")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetCursorPosition(int x, int y);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern uint SendInput(uint numberOfInputs, Inputs.Input[] inputs, int sizeOfInputStructure);

		#endregion
	}
}