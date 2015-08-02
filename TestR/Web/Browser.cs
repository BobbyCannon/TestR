#region References

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestR.Desktop;
using TestR.Extensions;
using TestR.Helpers;
using TestR.Logging;
using TestR.Web.Browsers;

#endregion

namespace TestR.Web
{
	/// <summary>
	/// This is the base class for browsers.
	/// </summary>
	/// <exclude> </exclude>
	public abstract class Browser : IDisposable
	{
		#region Fields

		private string _lastUri;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the Browser class.
		/// </summary>
		protected Browser(Application application)
		{
			Application = application;
			AutoClose = false;
			AutoRefresh = true;
			Elements = new ElementCollection();
			JavascriptLibraries = new JavaScriptLibrary[0];
			Timeout = TimeSpan.FromSeconds(10);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the current active element.
		/// </summary>
		public Element ActiveElement
		{
			get
			{
				var id = ExecuteScript("document.activeElement.id");
				if (string.IsNullOrWhiteSpace(id) || !Elements.ContainsKey(id))
				{
					return null;
				}

				return Elements[id];
			}
		}

		/// <summary>
		/// Gets or sets the application for this browser.
		/// </summary>
		public Application Application { get; protected set; }

		/// <summary>
		/// Gets or sets a flag to auto close the browser when disposed of. Defaults to false.
		/// </summary>
		public bool AutoClose { get; set; }

		/// <summary>
		/// Gets or sets a flag that allows elements to refresh when reading properties. Defaults to true.
		/// </summary>
		public bool AutoRefresh { get; set; }

		/// <summary>
		/// Gets the type of the browser.
		/// </summary>
		public abstract BrowserType BrowserType { get; }

		/// <summary>
		/// Gets a list of all elements on the current page.
		/// </summary>
		public ElementCollection Elements { get; }

		/// <summary>
		/// Gets the ID of the browser.
		/// </summary>
		public abstract int Id { get; }

		/// <summary>
		/// Gets a list of JavaScript libraries that were detected on the page.
		/// </summary>
		public IEnumerable<JavaScriptLibrary> JavascriptLibraries { get; set; }

		/// <summary>
		/// Gets the raw HTML of the page.
		/// </summary>
		public virtual string RawHtml
		{
			get { return ExecuteScript("document.getElementsByTagName('body')[0].innerHTML"); }
		}

		/// <summary>
		/// Gets or sets a flag to tell the browser to act slower. Defaults to false.
		/// </summary>
		public bool SlowMotion { get; set; }

		/// <summary>
		/// Gets or sets the time out for delay request. Defaults to 5 seconds.
		/// </summary>
		public TimeSpan Timeout { get; set; }

		/// <summary>
		/// Gets the URI of the current page.
		/// </summary>
		public string Uri
		{
			get { return GetBrowserUri(); }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Attach browsers for each type provided.
		/// </summary>
		/// <param name="type"> The type of the browser to attach to. </param>
		public static IEnumerable<Browser> AttachBrowsers(BrowserType type = BrowserType.All)
		{
			var response = new List<Browser>();

			if ((type & BrowserType.Chrome) == BrowserType.Chrome)
			{
				response.Add(Chrome.Attach());
			}

			if ((type & BrowserType.InternetExplorer) == BrowserType.InternetExplorer)
			{
				response.Add(InternetExplorer.Attach());
			}

			if ((type & BrowserType.Firefox) == BrowserType.Firefox)
			{
				response.Add(Firefox.Attach());
			}

			return response;
		}

		/// <summary>
		/// Attach or create browsers for each type provided.
		/// </summary>
		/// <param name="type"> The type of the browser to attach to or create. </param>
		public static IEnumerable<Browser> AttachOrCreate(BrowserType type = BrowserType.All)
		{
			var response = new List<Browser>();

			if ((type & BrowserType.Chrome) == BrowserType.Chrome)
			{
				response.Add(Chrome.AttachOrCreate());
			}

			if ((type & BrowserType.InternetExplorer) == BrowserType.InternetExplorer)
			{
				response.Add(InternetExplorer.AttachOrCreate());
			}

			if ((type & BrowserType.Firefox) == BrowserType.Firefox)
			{
				response.Add(Firefox.AttachOrCreate());
			}

			return response;
		}

		/// <summary>
		/// Closes all browsers of the provided type.
		/// </summary>
		/// <param name="type"> The type of the browser to close. </param>
		public static void CloseBrowsers(BrowserType type = BrowserType.All)
		{
			if ((type & BrowserType.Chrome) == BrowserType.Chrome)
			{
				Application.CloseAll(Chrome.Name);
			}

			if ((type & BrowserType.InternetExplorer) == BrowserType.InternetExplorer)
			{
				Application.CloseAll(InternetExplorer.Name);
			}

			if ((type & BrowserType.Firefox) == BrowserType.Firefox)
			{
				Application.CloseAll(Firefox.Name);
			}
		}

		/// <summary>
		/// Create browsers for each type provided.
		/// </summary>
		/// <param name="type"> The type of the browser to create. </param>
		public static IEnumerable<Browser> CreateBrowsers(BrowserType type = BrowserType.All)
		{
			var response = new List<Browser>();

			if ((type & BrowserType.Chrome) == BrowserType.Chrome)
			{
				response.Add(Chrome.Create());
			}

			if ((type & BrowserType.InternetExplorer) == BrowserType.InternetExplorer)
			{
				response.Add(InternetExplorer.Create());
			}

			if ((type & BrowserType.Firefox) == BrowserType.Firefox)
			{
				response.Add(Firefox.Create());
			}

			return response;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Execute JavaScript code in the current document.
		/// </summary>
		/// <param name="script"> The script to run. </param>
		/// <param name="expectResponse"> The script will return response. </param>
		/// <returns> The response from the script. </returns>
		public string ExecuteScript(string script, bool expectResponse = true)
		{
			var response = ExecuteJavaScript(script, expectResponse);

			// Check the response to see if the TestR script has been injected.
			if (response.Contains("TestR is not defined"))
			{
				InjectTestScript();
				return ExecuteJavaScript(script, expectResponse);
			}

			return response;
		}

		/// <summary>
		/// Process an action against a new instance of each browser type provided.
		/// </summary>
		/// <param name="action"> The action to perform against each browser. </param>
		/// <param name="type"> The type of the browser to process against. </param>
		public static void ForEachBrowser(Action<Browser> action, BrowserType type = BrowserType.All)
		{
			var browsers = AttachOrCreate(type);
			foreach (var browser in browsers)
			{
				using (browser)
				{
					action(browser);
				}
			}
		}

		/// <summary>
		/// Navigates the browser to the provided URI.
		/// </summary>
		/// <param name="uri"> The URI to navigate to. </param>
		/// <param name="expectedUri"> The expected URI to navigate to. </param>
		public void NavigateTo(string uri, string expectedUri = null)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}

			LogManager.Write("Navigating to " + uri + ".", LogLevel.Verbose);
			BrowserNavigateTo(uri);
			WaitForNavigation(expectedUri);
		}

		/// <summary>
		/// Refresh the state because the browser page may have changed state.
		/// </summary>
		public void Refresh()
		{
			LogManager.Write("Refreshing the page.", LogLevel.Verbose);
			WaitForComplete();
			InjectTestScript();
			DetectJavascriptLibraries();
			RefreshElements();
		}

		/// <summary>
		/// Removes the element from the page. * Experimental
		/// </summary>
		/// <param name="element"> The element to remove. </param>
		public void RemoveElement(Element element)
		{
			LogManager.Write("Removing element with ID of " + element.Id + ".", LogLevel.Verbose);
			ExecuteJavaScript("TestR.removeElement('" + element.Id + "');", false);
			Elements.Remove(element);
		}

		/// <summary>
		/// Removes an attribute from an element.
		/// </summary>
		/// <param name="element"> The element to remove the attribute from. </param>
		/// <param name="name"> The name of the attribute to remove. </param>
		public void RemoveElementAttribute(Element element, string name)
		{
			LogManager.Write("Removing element attribute with ID of " + element.Id + ".", LogLevel.Verbose);
			ExecuteJavaScript("TestR.removeElementAttribute('" + element.Id + "', '" + name + "');", false);
		}

		/// <summary>
		/// Wait for the browser page to redirect to a provided URI.
		/// </summary>
		/// <param name="uri"> The expected URI to land on. Defaults to empty string if not provided. </param>
		/// <param name="timeout"> The timeout before giving up on the redirect. Defaults to Timeout if not provided. </param>
		public void WaitForNavigation(string uri = null, TimeSpan? timeout = null)
		{
			if (timeout == null)
			{
				timeout = Timeout;
			}

			if (uri == null)
			{
				LogManager.Write("Waiting for navigation with timeout of " + timeout.Value + ".", LogLevel.Verbose);
				if (!Utility.Wait(() => Uri != _lastUri, (int) timeout.Value.TotalMilliseconds))
				{
					throw new Exception("Browser never completed navigated away from " + _lastUri + ".");
				}
			}
			else
			{
				LogManager.Write("Waiting for navigation to " + uri + " with timeout of " + timeout.Value + ".", LogLevel.Verbose);
				if (!Utility.Wait(() => Uri.Contains(uri, StringComparison.OrdinalIgnoreCase), (int) timeout.Value.TotalMilliseconds))
				{
					throw new Exception("Browser never completed navigation to " + uri + ". Current URI is " + Uri + ".");
				}
			}

			Refresh();
			_lastUri = Uri;
		}

		/// <summary>
		/// Browser implementation of navigate to
		/// </summary>
		/// <param name="uri"> The URI to navigate to. </param>
		protected abstract void BrowserNavigateTo(string uri);

		/// <summary>
		/// Creates a new process.
		/// </summary>
		/// <param name="fileName"> The filename of the browser. </param>
		/// <param name="arguments"> The arguments for the browser. </param>
		/// <returns> The new process for the browser. </returns>
		/// <exception cref="Exception"> Failed to start the process. </exception>
		protected static Process CreateInstance(string fileName, string arguments = "")
		{
			var info = new ProcessStartInfo(fileName);
			info.Arguments = arguments;
			info.WindowStyle = ProcessWindowStyle.Normal;
			info.UseShellExecute = true;

			var process = new Process();
			process.StartInfo = info;

			if (!process.Start())
			{
				throw new Exception("Failed to start the process. ExitCode: " + process.ExitCode);
			}

			Utility.Wait(process, p => p.Handle != IntPtr.Zero);
			Thread.Sleep(250);

			return process;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing"> True if disposing and false if otherwise. </param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}

			if (AutoClose && Application != null)
			{
				Application.Close();
			}

			if (Application != null)
			{
				Application.Dispose();
				Application = null;
			}
		}

		/// <summary>
		/// Execute JavaScript code in the current document.
		/// </summary>
		/// <param name="script"> The code script to execute. </param>
		/// <param name="expectResponse"> The script will return response. </param>
		/// <returns> The response from the execution. </returns>
		protected abstract string ExecuteJavaScript(string script, bool expectResponse = true);

		/// <summary>
		/// Reads the current URI directly from the browser.
		/// </summary>
		/// <returns> The current URI that was read from the browser. </returns>
		protected abstract string GetBrowserUri();

		/// <summary>
		/// Inserts the test script into the current page.
		/// </summary>
		protected static string GetTestScript()
		{
			var assembly = Assembly.GetExecutingAssembly();

			using (var stream = assembly.GetManifestResourceStream("TestR.TestR.min.js"))
			{
				if (stream != null)
				{
					using (var reader = new StreamReader(stream))
					{
						return reader.ReadToEnd();
					}
				}
			}

			return string.Empty;
		}

		/// <summary>
		/// Refreshes the element collection for the current page.
		/// </summary>
		protected void RefreshElements()
		{
			LogManager.Write("Refresh the elements.", LogLevel.Verbose);

			Elements.Clear();

			var data = ExecuteScript("JSON.stringify(TestR.getElements())");
			if (JavascriptLibraries.Contains(JavaScriptLibrary.Angular) && data.Contains("ng-view ng-cloak"))
			{
				throw new Exception("JavaScript not completed?");
			}

			var elements = JsonConvert.DeserializeObject<JArray>(data);
			if (elements == null)
			{
				return;
			}

			Elements.AddRange(elements, this);
		}

		/// <summary>
		/// Waits until the browser to complete any outstanding operations.
		/// </summary>
		/// <summary>
		/// Waits until the browser to complete any outstanding operations.
		/// </summary>
		protected virtual void WaitForComplete()
		{
			Utility.Wait(() => ExecuteJavaScript("document.readyState === 'complete'").Equals("true", StringComparison.OrdinalIgnoreCase));
			Application.WaitWhileBusy();
		}

		/// <summary>
		/// Runs script to detect specific libraries.
		/// </summary>
		private void DetectJavascriptLibraries()
		{
			LogManager.Write("Detecting JavaScript libraries.", LogLevel.Verbose);

			var uri = GetBrowserUri();
			if (uri.Length <= 0 || uri.Equals("about:tabs"))
			{
				return;
			}

			var libraries = new List<JavaScriptLibrary>();
			var hasLibrary = ExecuteScript("typeof jQuery !== 'undefined'");
			if (hasLibrary.Equals("true", StringComparison.OrdinalIgnoreCase))
			{
				libraries.Add(JavaScriptLibrary.JQuery);
			}

			hasLibrary = ExecuteScript("typeof angular !== 'undefined'");
			if (hasLibrary.Equals("true", StringComparison.OrdinalIgnoreCase))
			{
				libraries.Add(JavaScriptLibrary.Angular);
			}

			hasLibrary = ExecuteScript("typeof moment !== 'undefined'");
			if (hasLibrary.Equals("true", StringComparison.OrdinalIgnoreCase))
			{
				libraries.Add(JavaScriptLibrary.Moment);
			}

			//neither of the bootstrap tests are guaranteed since it's possible to customize 
			//bootstrap to not include some plugins. Bootstrap doesn't provide a namespace to
			//test against, so there is a small change of a false positive
			hasLibrary = ExecuteScript("typeof $().emulateTransitionEnd == 'function'");
			if (hasLibrary.Equals("true", StringComparison.OrdinalIgnoreCase))
			{
				libraries.Add(JavaScriptLibrary.Bootstrap3);
			}

			//both bs 2 and 3 have popover but only 3+ uses emulateTransitionEnd
			if (!libraries.Contains(JavaScriptLibrary.Bootstrap3))
			{
				hasLibrary = ExecuteScript("typeof($.fn.popover) !== 'undefined'");
				if (hasLibrary.Equals("true", StringComparison.OrdinalIgnoreCase))
				{
					libraries.Add(JavaScriptLibrary.Bootstrap2);
				}
			}

			JavascriptLibraries = libraries;
		}

		/// <summary>
		/// Injects the test script into the browser.
		/// </summary>
		private void InjectTestScript()
		{
			ExecuteJavaScript(GetTestScript());

			var test = ExecuteJavaScript("typeof TestR");
			if (test.Equals("undefined"))
			{
				throw new Exception("Failed to inject the TestR JavaScript.");
			}
		}

		#endregion
	}
}