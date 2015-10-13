#region References

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using TestR.Desktop;
using TestR.Extensions;
using TestR.Helpers;
using TestR.Logging;

#endregion

namespace TestR.Web.Browsers
{
	/// <summary>
	/// Represents an Edge browser.
	/// </summary>
	public class Edge : Browser
	{
		#region Constants

		/// <summary>
		/// The name of the browser.
		/// </summary>
		public const string Name = "MicrosoftEdge";

		#endregion

		#region Fields

		private readonly string _sessionId;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the Browser class.
		/// </summary>
		public Edge(Application application, string sessionId)
			: base(application)
		{
			_sessionId = sessionId;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the type of the browser.
		/// </summary>
		public override BrowserType BrowserType => BrowserType.Edge;

		#endregion

		#region Methods

		/// <summary>
		/// Attempts to attach to an existing browser.
		/// </summary>
		/// <returns> An instance of an Internet Explorer browser. </returns>
		public static Browser Attach()
		{
			InitializeDriver();
			var session = GetSession() ?? StartSession();
			if (string.IsNullOrWhiteSpace(session))
			{
				return null;
			}

			var application = Application.Attach(Name, null, false);
			var browser = new Edge(application, session);
			browser.Refresh();
			return browser;
		}

		/// <summary>
		/// Attempts to attach to an existing browser.
		/// </summary>
		/// <returns> The browser instance or null if not found. </returns>
		public static Browser Attach(Process process)
		{
			if (process.ProcessName != Name)
			{
				return null;
			}

			var session = GetSession() ?? StartSession();
			var application = Application.Attach(process, false);
			var browser = new Edge(application, session);
			browser.Refresh();
			return browser;
		}

		/// <summary>
		/// Attempts to attach to an existing browser. If one is not found then create and return a new one.
		/// </summary>
		/// <returns> An instance of an Internet Explorer browser. </returns>
		public static Browser AttachOrCreate()
		{
			return Attach() ?? Create();
		}

		/// <summary>
		/// Creates a new instance of an Edge browser.
		/// </summary>
		/// <returns> An instance of an Edge browser. </returns>
		public static Browser Create()
		{
			InitializeDriver();
			var session = GetSession() ?? StartSession();
			var application = Application.Attach(Name, null, false);
			var browser = new Edge(application, session);
			browser.Refresh();
			return browser;
		}

		public static void EndSession(string sessionId)
		{
			Request("DELETE", "http://localhost:17556/session/" + sessionId, null);
		}

		public static Process InitializeDriver()
		{
			var path = @"C:\Program Files (x86)\Microsoft Web Driver\MicrosoftWebDriver.exe";
			if (!File.Exists(path))
			{
				throw new Exception("Please install the Microsoft web driver.");
			}

			var process = Process.GetProcessesByName("MicrosoftWebDriver").FirstOrDefault();
			if (process != null)
			{
				return process;
			}

			var startInfo = new ProcessStartInfo
			{
				FileName = path,
				CreateNoWindow = true,
				UseShellExecute = false,
				WindowStyle = ProcessWindowStyle.Hidden,
				RedirectStandardError = true,
				RedirectStandardInput = true,
				RedirectStandardOutput = true
			};

			return Process.Start(startInfo);
		}

		/// <summary>
		/// Move the window and resize it.
		/// </summary>
		/// <param name="x"> The x coordinate to move to. </param>
		/// <param name="y"> The y coordinate to move to. </param>
		/// <param name="width"> The width of the window. </param>
		/// <param name="height"> The height of the window. </param>
		public override Browser MoveWindow(int x, int y, int width, int height)
		{
			// todo: figure out how to really move the window. Windows store apps do not play nice with old api.
			return this;
		}

		/// <summary>
		/// Browser implementation of navigate to
		/// </summary>
		/// <param name="uri"> The URI to navigate to. </param>
		protected override void BrowserNavigateTo(string uri)
		{
			var postData = (new { url = uri }).ToJson();
			Request("POST", $"http://localhost:17556/session/{_sessionId}/url", postData, (int) Timeout.TotalMilliseconds);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing"> True if disposing and false if otherwise. </param>
		protected override void Dispose(bool disposing)
		{
			if (AutoClose)
			{
				EndSession(_sessionId);
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Execute JavaScript code in the current document.
		/// </summary>
		/// <param name="script"> The code script to execute. </param>
		/// <param name="expectResponse"> The script will return response. </param>
		/// <returns> The response from the execution. </returns>
		protected override string ExecuteJavaScript(string script, bool expectResponse = true)
		{
			if (script.Contains("var TestR=TestR") || script.Contains("var TestR = TestR"))
			{
				script = script.Replace("var TestR = TestR || {", "TestR = {");
				return Request("POST", $"http://localhost:17556/session/{_sessionId}/execute", "{\"script\": \"" + script + "\", \"args\": []}", (int) Timeout.TotalMilliseconds);
			}

			//script = script.Replace("'", "\\'");

			var postData = (new { script = "TestR.runScript(arguments[0])", args = new[] { script } }).ToJson();
			var data = Request("POST", $"http://localhost:17556/session/{_sessionId}/execute", postData, (int) Timeout.TotalMilliseconds);
			var response = JsonConvert.DeserializeObject<dynamic>(data);
			return response.status != 0 ? "TestR is not defined" : GetScriptResults();
		}

		/// <summary>
		/// Reads the current URI directly from the browser.
		/// </summary>
		/// <returns> The current URI that was read from the browser. </returns>
		protected override string GetBrowserUri()
		{
			LogManager.Write("Get browser's URI.", LogLevel.Verbose);
			return ExecuteScript("window.location.href");
		}

		private string GetScriptResults()
		{
			var postData = (new { Using = "id", Value = "testrResult" }).ToJson();
			var data = Request("POST", $"http://localhost:17556/session/{_sessionId}/element", postData, (int) Timeout.TotalMilliseconds);
			var response = JsonConvert.DeserializeObject<dynamic>(data);
			if (response.status == 7)
			{
				return "TestR is not defined";
			}

			var elementId = ((object) response.value.ELEMENT).ToString();
			data = Request("GET", $"http://localhost:17556/session/{_sessionId}/element/{elementId}/attribute/value", null, (int) Timeout.TotalMilliseconds);
			response = JsonConvert.DeserializeObject<dynamic>(data);
			return ((object) response.value).ToString();
		}

		private static string GetSession()
		{
			var data = Request("GET", "http://localhost:17556/sessions", null);
			var response = JsonConvert.DeserializeObject<dynamic>(data);
			return response.status.ToString() != "success" ? null : response.value[0].id.ToString();
		}

		private static string Request(string method, string location, string data, int timeout = 1500)
		{
			var request = (HttpWebRequest) WebRequest.Create(location);
			request.Method = method;
			request.Timeout = timeout;

			if (data != null)
			{
				request.ContentType = "Content-Type: text/plain; charset=UTF-8";
				request.ContentLength = data.Length;

				using (var stream = request.GetRequestStream())
				{
					var buffer = Encoding.UTF8.GetBytes(data);
					stream.Write(buffer, 0, buffer.Length);
				}
			}

			using (var response = request.GetResponse())
			{
				var stream = response.GetResponseStream();
				if (stream == null)
				{
					throw new Exception("Failed to get a response.");
				}

				using (var reader = new StreamReader(stream))
				{
					return reader.ReadToEnd();
				}
			}
		}

		private static string StartSession()
		{
			string response = null;

			Utility.Wait(() =>
			{
				var data = Request("POST", "http://localhost:17556/session", "{\"desiredCapabilities\": {},\"requiredCapabilities\": {}}");
				var item = JsonConvert.DeserializeObject<dynamic>(data);

				if (item.status != 0)
				{
					return false;
				}

				response = item.sessionId.ToString();
				return true;
			});

			return response;
		}

		#endregion
	}
}