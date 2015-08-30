#region References

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

namespace TestR.Native
{
	public class MouseMonitor : IDisposable
	{
		#region Constants

		private const int MouseLowLevel = 14;

		#endregion

		#region Fields

		private readonly NativeMethods.LowLevelKeyboardProc _hook;
		private IntPtr _hookId;
		private readonly int _processId;

		#endregion

		#region Constructors

		public MouseMonitor(int processId)
		{
			_processId = processId;
			_hookId = IntPtr.Zero;
			_hook = HookCallback;
		}

		#endregion

		#region Methods

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Start monitoring the mouse for changes.
		/// </summary>
		public void StartMonitoring()
		{
			var process = Process.GetProcessById(_processId);
			using (var curModule = process.MainModule)
			{
				_hookId = NativeMethods.SetWindowsHookEx(MouseLowLevel, _hook, NativeMethods.GetModuleHandle(curModule.ModuleName), 0);
			}
		}

		/// <summary>
		/// Stops monitoring the mouse for changes.
		/// </summary>
		public void StopMonitoring()
		{
			NativeMethods.UnhookWindowsHookEx(_hookId);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				StopMonitoring();
			}
		}

		private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode < 0)
			{
				return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
			}

			var message = (NativeMethods.MouseMessages) wParam;
			var hook = (NativeMethods.MouseHook) Marshal.PtrToStructure(lParam, typeof (NativeMethods.MouseHook));

			if (message != NativeMethods.MouseMessages.WM_MOUSEMOVE)
			{
				OnMouseChanged(Mouse.GetEvent(message), new Point(hook.pt.x, hook.pt.y));
			}

			return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
		}

		private void OnMouseChanged(Mouse.MouseEvent mouseEvent, Point point)
		{
			MouseChanged?.Invoke(mouseEvent, point);
		}

		#endregion

		#region Events

		/// <summary>
		/// Event for when the mouse changes during monitoring.
		/// </summary>
		public event Action<Mouse.MouseEvent, Point> MouseChanged;

		#endregion
	}
}