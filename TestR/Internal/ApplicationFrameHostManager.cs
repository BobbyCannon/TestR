#region References

using System;
using System.Diagnostics;
using TestR.Desktop;
using TestR.Desktop.Elements;

#endregion

namespace TestR.Internal
{
	internal static class ApplicationFrameHostManager
	{
		#region Methods

		public static IntPtr Refresh(SafeProcess process)
		{
			var frameHosts = Process.GetProcessesByName("ApplicationFrameHost");

			foreach (var host in frameHosts)
			{
				using var application = new Application(host);
				application.Refresh();

				foreach (var c in application.Children)
				{
					if (!(c is Window window))
					{
						continue;
					}

					foreach (var cc in c.Children)
					{
						if (!(cc is Window ww))
						{
							continue;
						}

						if (ww.NativeElement.CurrentProcessId != process.Id)
						{
							continue;
						}
						
						ww.Dispose();

						return window.Handle;
					}
				}
			}

			return IntPtr.Zero;
		}

		#endregion
	}
}