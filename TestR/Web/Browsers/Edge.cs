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
using TestR.Logging;

#endregion

namespace TestR.Web.Browsers
{
	public class Edge : Browser
	{
		#region Constants

		/// <summary>
		/// The name of the browser.
		/// </summary>
		public const string Name = "MicrosoftEdge";

		#endregion

		#region Fields

		private readonly Process _driver;
		private readonly string _sessionId;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the Browser class.
		/// </summary>
		public Edge(Application application, Process driver, string sessionId)
			: base(application)
		{
			_driver = driver;
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
		/// Creates a new instance of an Edge browser.
		/// </summary>
		/// <returns> An instance of an Edge browser. </returns>
		public static Edge Create()
		{
			Application.CloseAll(Name);
			var driver = GetDriver();
			var data = Request("POST", "http://localhost:17556/session", "{\"desiredCapabilities\": {},\"requiredCapabilities\": {}}");
			var response = JsonConvert.DeserializeObject<dynamic>(data);
			var sessionId = response.sessionId.ToString();
			var application = Application.Attach(Name);
			return new Edge(application, driver, sessionId);
		}

		/// <summary>
		/// Browser implementation of navigate to
		/// </summary>
		/// <param name="uri"> The URI to navigate to. </param>
		protected override void BrowserNavigateTo(string uri)
		{
			Request("POST", "http://localhost:17556/session/" + _sessionId + "/url", "{\"url\": \"" + uri + "\"}");
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
				return Request("POST", "http://localhost:17556/session/" + _sessionId + "/execute", "{\"script\": \"" + script + "\", \"args\": []}");
			}

			var wrappedScript = "TestR.runScript('" + script + "');";
			var data = Request("POST", "http://localhost:17556/session/" + _sessionId + "/execute", "{\"script\": \"" + wrappedScript + "\", \"args\": []}");
			var response = JsonConvert.DeserializeObject<dynamic>(data);
			if (response.status != 0)
			{
				return "TestR is not defined";
			}

			return GetScriptResults();
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
			var data = Request("POST", "http://localhost:17556/session/" + _sessionId + "/element", "{\"using\":\"id\",\"value\":\"testrResult\"}");
			var response = JsonConvert.DeserializeObject<dynamic>(data);
			if (response.status == 7)
			{
				return "TestR is not defined";
			}

			var elementId = ((object)response.value.ELEMENT).ToString();
			data = Request("GET", "http://localhost:17556/session/" + _sessionId + "/element/" + elementId + "/attribute/value", null);
			response = JsonConvert.DeserializeObject<dynamic>(data);
			return ((object) response.value).ToString();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing"> True if disposing and false if otherwise. </param>
		protected override void Dispose(bool disposing)
		{
			//Request("DELETE", "http://localhost:17556/session/" + _sessionId, null);
			base.Dispose(disposing);
		}

		private static Process GetDriver()
		{
			var path = @"C:\Program Files (x86)\Microsoft Web Driver\MicrosoftWebDriver.exe";
			var process = Process.GetProcessesByName("MicrosoftWebDriver").FirstOrDefault();
			if (process != null)
			{
				process.Kill();
				process.Dispose();
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
		
		private static string Request(string method, string location, string data)
		{
			var request = (HttpWebRequest) WebRequest.Create(location);
			request.Method = method;
			request.Timeout = 30000;

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

		#endregion
	}
}