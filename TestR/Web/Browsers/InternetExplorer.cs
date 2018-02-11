#region References

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using mshtml;
using SHDocVw;
using TestR.Native;

#endregion

namespace TestR.Web.Browsers
{
	/// <summary>
	/// Represents an Internet Explorer browser.
	/// </summary>
	public class InternetExplorer : Browser
	{
		#region Constants

		/// <summary>
		/// The name of the browser.
		/// </summary>
		public const string BrowserName = "iexplore";

		#endregion

		#region Fields

		private SHDocVw.InternetExplorer _browser;

		#endregion

		#region Constructors

		private InternetExplorer(SHDocVw.InternetExplorer browser, ICollection<IntPtr> windowsToIgnore = null, bool bringToFront = true)
			: base(Application.Attach(ProcessService.Where(BrowserName).First(x => x.MainWindowHandle == new IntPtr(browser.HWND)), false, bringToFront), windowsToIgnore)
		{
			_browser = browser;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the type of the browser.
		/// </summary>
		public override BrowserType BrowserType => BrowserType.InternetExplorer;

		/// <summary>
		/// Gets the raw HTML of the page.
		/// </summary>
		public override string RawHtml => ((HTMLDocument) _browser.Document).documentElement.outerHTML;

		#endregion

		#region Methods

		/// <summary>
		/// Attempts to attach to an existing browser.
		/// </summary>
		/// <param name="bringToFront"> The option to bring the application to the front. This argment is optional and defaults to true. </param>
		/// <returns> An instance of an Internet Explorer browser. </returns>
		public static Browser Attach(bool bringToFront = true)
		{
			var browser = GetBrowserToAttachTo(0, bringToFront);
			if (browser == null)
			{
				return null;
			}

			//browser.NavigateTo("about:blank");
			browser.Refresh();
			return browser;
		}

		/// <summary>
		/// Attempts to attach to an existing browser.
		/// </summary>
		/// <param name="process"> The process to attach to. </param>
		/// <param name="bringToFront"> The option to bring the application to the front. This argment is optional and defaults to true. </param>
		/// <returns> The browser instance or null if not found. </returns>
		public static Browser Attach(Process process, bool bringToFront = true)
		{
			if (process.ProcessName != BrowserName)
			{
				return null;
			}

			var browser = GetBrowserToAttachTo(process.Id, bringToFront);
			if (browser == null)
			{
				return null;
			}

			//browser.NavigateTo("about:blank");
			browser.Refresh();
			return browser;
		}

		/// <summary>
		/// Attempts to attach to an existing browser. If one is not found then create and return a new one.
		/// </summary>
		/// <param name="bringToFront"> The option to bring the application to the front. This argment is optional and defaults to true. </param>
		/// <returns> An instance of an Internet Explorer browser. </returns>
		public static Browser AttachOrCreate(bool bringToFront = true)
		{
			return Attach(bringToFront) ?? Create(bringToFront);
		}

		/// <summary>
		/// Creates a new instance of an Internet Explorer browser.
		/// </summary>
		/// <param name="bringToFront"> The option to bring the application to the front. This argment is optional and defaults to true. </param>
		/// <returns> An instance of an Internet Explorer browser. </returns>
		public static Browser Create(bool bringToFront = true)
		{
			// See if chrome is already running
			InternetExplorer browser;
			var foundBrowser = GetBrowserToAttachTo();

			if (foundBrowser != null)
			{
				// Start process but connect to current process and new window.
				var existing = foundBrowser.Application.GetWindows().Select(x =>
				{
					x.Dispose();
					return x.Handle;
				}).ToList();

				browser = new InternetExplorer(CreateInternetExplorerClass(), existing, bringToFront);
			}
			else
			{
				browser = new InternetExplorer(CreateInternetExplorerClass(), null, bringToFront);
			}

			browser.NavigateTo("about:blank");
			browser.Refresh();
			return browser;
		}

		/// <inheritdoc />
		public override ElementHost WaitForComplete(int minimumDelay = 0)
		{
			//LogManager.Write("InternetExploreBrowser.WaitForComplete", LogLevel.Verbose);

			// If the URL is empty and is not initialized means browser is no page is loaded.
			if (_browser.LocationURL == string.Empty && _browser.ReadyState == tagREADYSTATE.READYSTATE_UNINITIALIZED)
			{
				return this;
			}

			var states = new[] { tagREADYSTATE.READYSTATE_COMPLETE, tagREADYSTATE.READYSTATE_INTERACTIVE };
			var readyStates = new[] { "complete", "interactive" };

			// Wait for browser to completely load or become interactive.
			if (!Wait(x => states.Contains(_browser.ReadyState)))
			{
				throw new TestRException("The browser never finished loading...");
			}

			// Wait for browser document to completely load or become interactive.
			if (!Wait(x => readyStates.Contains(((IHTMLDocument2) _browser.Document).readyState)))
			{
				throw new TestRException("The browser document never finished loading...");
			}

			// Wait while the browser is busy and not complete.
			if (!Wait(x => !(_browser.Busy && _browser.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE)))
			{
				throw new TestRException("The browser is currently busy.");
			}

			return this;
		}

		/// <summary>
		/// Navigates the browser to the provided URI.
		/// </summary>
		/// <param name="uri"> The URI to navigate to. </param>
		protected override void BrowserNavigateTo(string uri)
		{
			try
			{
				//LogManager.Write("InternetExplorer navigated to " + uri + ".", LogLevel.Verbose);
				if (_browser.LocationURL == uri)
				{
					//_browser.Refresh2(3);
					_browser.Refresh();
				}
				else
				{
					//object nil = null;
					//object absoluteUri = uri;
					//_browser.Navigate2(ref absoluteUri, ref nil, ref nil, ref nil, ref nil);
					_browser.Navigate(uri);
				}

				WaitForComplete();

				var htmlDocument = _browser.Document as IHTMLDocument2;
				if (htmlDocument == null)
				{
					throw new TestRException("Failed to run script because no document is loaded.");
				}

				Thread.Sleep(50);
			}
			catch (Exception)
			{
				// A COMException with "RPC_E_DISCONNECTED" happens when the browser switches security context.
				Thread.Sleep(150);
				ReinitializeBrowser();
				BrowserNavigateTo(uri);
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing"> True if disposing and false if otherwise. </param>
		protected override void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}

			try
			{
				if (_browser == null || !AutoClose)
				{
					return;
				}

				WaitForComplete();

				// We cannot allow the browser to close within a second.
				// I assume that add-on(s) need time to start before closing the browser.
				var timeout = TimeSpan.FromMilliseconds(1000);
				var watch = Stopwatch.StartNew();

				while (watch.Elapsed <= timeout)
				{
					Thread.Sleep(50);
				}

				_browser.Quit();
			}
			catch
			{
				_browser = null;
			}
		}

		/// <summary>
		/// Execute JavaScript code in the current document.
		/// </summary>
		/// <param name="script"> The code script to execute. </param>
		/// <param name="expectResponse"> The script will return response. </param>
		/// <returns> The response from the execution. </returns>
		protected override string ExecuteJavaScript(string script, bool expectResponse = true)
		{
			//LogManager.Write("Request: " + script, LogLevel.Verbose);

			// If the URL is empty and is not initialized means browser is no page is loaded.
			if (!(_browser.LocationURL.StartsWith("http", StringComparison.OrdinalIgnoreCase) || _browser.LocationURL.StartsWith("about:", StringComparison.OrdinalIgnoreCase)) 
				|| _browser.ReadyState == tagREADYSTATE.READYSTATE_UNINITIALIZED)
			{
				return string.Empty;
			}

			var document = _browser.Document as IHTMLDocument2;
			if (document == null)
			{
				throw new TestRException("Failed to run script because no document is loaded.");
			}

			try
			{
				// See if we are injecting the test script.
				if (script.Contains("var TestR=TestR") || script.Contains("var TestR = TestR"))
				{
					document.parentWindow.execScript(script, "javascript");
					return "Injected TestR Script";
				}

				script = script
					//.Replace("\r", "\\r")
					//.Replace("\n", "\\n")
					//.Replace("\'", "\\\'")
					//.Replace("\"", "\\\"")
					.Replace("\\", "\\\\");

				// Run the script using TestR.
				script = HttpUtility.HtmlEncode(script);
				var wrappedScript = $"TestR.runScript('{script}');";
				document.parentWindow.execScript(wrappedScript, "javascript");

				return GetJavascriptResult((IHTMLDocument3) document);
			}
			catch (Exception ex)
			{
				if (ex.HResult == -2147352319)
				{
					return TestrNotDefinedMessage;
				}

				throw;
			}
		}

		/// <summary>
		/// Reads the current URI directly from the browser.
		/// </summary>
		/// <returns> The current URI that was read from the browser. </returns>
		protected override string GetBrowserUri()
		{
			return _browser.LocationURL;
		}

		/// <summary>
		/// Creates an instance of the InternetExplorer.
		/// </summary>
		/// <returns> An instance of Internet Explorer. </returns>
		private static SHDocVw.InternetExplorer CreateInternetExplorerClass()
		{
			try
			{
				var explorer = new InternetExplorerClass
				{
					Visible = true,
					RegisterAsBrowser = true,
					RegisterAsDropTarget = true,
					IWebBrowserApp_Visible = true
				};

				Thread.Sleep(100);

				return explorer;
			}
			catch (Exception)
			{
				return CreateInternetExplorerClass();
			}
		}

		private static InternetExplorer GetBrowserToAttachTo(int processId = 0, bool bringToFront = true)
		{
			var explorers = new ShellWindowsClass()
				.Cast<SHDocVw.InternetExplorer>()
				.Where(x => x.FullName.ToLower().Contains("iexplore.exe"))
				.ToList();

			foreach (var explorer in explorers)
			{
				try
				{
					if (processId > 0)
					{
						if (!NativeMethods.GetWindowThreadProcessId(new IntPtr(explorer.HWND), out uint foundProcessId) || foundProcessId != processId)
						{
							continue;
						}
					}

					return new InternetExplorer(explorer, null, bringToFront);
				}
				catch (Exception)
				{
					// Ignore this browser and move to the next one.
					//LogManager.Write($"Error with finding browser to attach to. Exception: {ex.Message}.", LogLevel.Verbose);
				}
			}

			return null;
		}

		private string GetJavascriptResult(IHTMLDocument3 document)
		{
			try
			{
				var resultElement = document.getElementById("testrResult");
				var result = resultElement.getAttribute("value");

				//LogManager.Write("Response: " + result, LogLevel.Verbose);
				return result ?? string.Empty;
			}
			catch
			{
				// The document may have been redirected which means the member will not be there.
				return string.Empty;
			}
		}

		/// <summary>
		/// Disconnects from the current browser and finds the new instance.
		/// </summary>
		private string ReinitializeBrowser()
		{
			Application.Dispose();

			using (var app = GetBrowserToAttachTo())
			{
				_browser = app?._browser ?? CreateInternetExplorerClass();
			}

			Application = Application.Attach(ProcessService.Where(BrowserName).First(x => x.MainWindowHandle == new IntPtr(_browser.HWND)), false);
			//_zoneId = NativeMethods.GetZoneId(_browser.LocationURL);
			return _browser.LocationURL;
		}

		#endregion
	}
}