#region References

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

#endregion

namespace TestR.Web.Browsers
{
	/// <summary>
	/// Represents an Chrome browser.
	/// </summary>
	public class Chrome : Browser
	{
		#region Constants

		/// <summary>
		/// The name of the browser.
		/// </summary>
		public const string BrowserName = "chrome";

		/// <summary>
		/// The debugging argument for starting the browser.
		/// </summary>
		public const string DebugArgument = "--remote-debugging-port=9222 --profile-directory=\"Default\"";

		#endregion

		#region Fields

		private readonly JsonSerializerSettings _jsonSerializerSettings;
		private int _requestId;
		private ClientWebSocket _socket;
		private readonly ConcurrentDictionary<string, dynamic> _socketResponses;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the Chrome class.
		/// </summary>
		/// <param name="application"> The window of the existing browser. </param>
		private Chrome(Application application)
			: base(application)
		{
			_jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
			_requestId = 0;
			_socketResponses = new ConcurrentDictionary<string, dynamic>();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the type of the browser.
		/// </summary>
		public override BrowserType BrowserType => BrowserType.Chrome;

		#endregion

		#region Methods

		/// <summary>
		/// Attempts to attach to an existing browser.
		/// </summary>
		/// <returns> The browser instance or null if not found. </returns>
		public static Browser Attach(bool bringToFront = true)
		{
			var application = Application.Attach(BrowserName, DebugArgument, false, bringToFront);
			if (application == null)
			{
				return null;
			}

			var browser = new Chrome(application);
			browser.Connect();
			browser.Refresh();
			return browser;
		}

		/// <summary>
		/// Attempts to attach to an existing browser.
		/// </summary>
		/// <returns> The browser instance or null if not found. </returns>
		public static Browser Attach(Process process, bool bringToFront = true)
		{
			if (process.ProcessName != BrowserName)
			{
				return null;
			}

			if (!Application.Exists(BrowserName, DebugArgument))
			{
				throw new ArgumentException("The process was not started with the debug arguments.", nameof(process));
			}

			var application = Application.Attach(process, false, bringToFront);
			var browser = new Chrome(application);
			browser.Connect();
			browser.Refresh();
			return browser;
		}

		/// <summary>
		/// Attempts to attach to an existing browser. If one is not found then create and return a new one.
		/// </summary>
		/// <returns> The browser instance. </returns>
		public static Browser AttachOrCreate(bool bringToFront = true)
		{
			return Attach(bringToFront) ?? Create(bringToFront);
		}

		/// <summary>
		/// Attempts to create a new browser. If one is not found then we'll make sure it was started with the
		/// remote debugger argument. If not an exception will be thrown.
		/// </summary>
		/// <returns> The browser instance. </returns>
		public static Browser Create(bool bringToFront = true)
		{
			if (Application.Exists(BrowserName) && !Application.Exists(BrowserName, DebugArgument))
			{
				throw new TestRException("The first instance of Chrome was not started with the remote debugger enabled.");
			}

			// Create a new instance and return it.
			var application = Application.Create($"{BrowserName}.exe", DebugArgument, false, bringToFront);
			var browser = new Chrome(application);
			browser.Connect();
			browser.Refresh();
			return browser;
		}

		/// <summary>
		/// Navigates the browser to the provided URI.
		/// </summary>
		/// <param name="uri"> The URI to navigate to. </param>
		protected override void BrowserNavigateTo(string uri)
		{
			var request = new Request
			{
				Id = _requestId++,
				Method = "Page.navigate",
				Params = new
				{
					Url = uri
				}
			};

			SendRequestAndReadResponse(request, x => x.id == request.Id);

			// todo: There must be a better way to determine when Chrome and Firefox is done processing.
			Thread.Sleep(250);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing"> True if disposing and false if otherwise. </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && _socket != null)
			{
				_socket.Dispose();
				_socket = null;
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
			var request = new Request
			{
				Id = _requestId++,
				Method = "Runtime.evaluate",
				Params = new
				{
					DoNotPauseOnExceptions = true,
					Expression = script,
					ObjectGroup = "console",
					IncludeCommandLineAPI = true,
					ReturnByValue = expectResponse
				}
			};

			var data = SendRequestAndReadResponse(request, x => x.id == request.Id);
			if (data.Contains(TestrNotDefinedMessage))
			{
				return TestrNotDefinedMessage;
			}

			var response = data.AsJToken() as dynamic;
			if (response == null || response.result == null || response.result.result == null)
			{
				return data;
			}

			var result = response.result.result;
			if (result.value != null && result.value.GetType().Name == "JValue")
			{
				return result.value;
			}

			var typeName = result.GetType().Name;
			return typeName != "JValue"
				? JsonConvert.SerializeObject(result, Formatting.Indented)
				: (string) result.value;
		}

		/// <summary>
		/// Reads the current URI directly from the browser.
		/// </summary>
		/// <returns> The current URI that was read from the browser. </returns>
		protected override string GetBrowserUri()
		{
			//LogManager.Write("First browser's URI.", LogLevel.Verbose);

			var request = new Request
			{
				Id = _requestId++,
				Method = "DOM.getDocument"
			};

			var response = SendRequestAndReadResponse(request, x => x.id == request.Id);
			var document = response.AsJToken() as dynamic;
			if (document == null || document.result == null || document.result.root == null || document.result.root.documentURL == null)
			{
				throw new TestRException("Failed to get the URI.");
			}

			return document.result.root.documentURL;
		}

		/// <summary>
		/// Connect to the Chrome browser debugger port.
		/// </summary>
		/// <exception cref="Exception"> All debugging sessions are taken. </exception>
		private void Connect()
		{
			var sessions = new List<RemoteSessionsResponse>();

			Utility.Wait(() =>
			{
				try
				{
					sessions.AddRange(GetAvailableSessions());
					return true;
				}
				catch (WebException)
				{
					return false;
				}
			}, Application.Timeout.TotalMilliseconds, 250);

			if (sessions.Count == 0)
			{
				throw new TestRException("All debugging sessions are taken.");
			}

			var session = sessions.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.WebSocketDebuggerUrl));
			if (session == null)
			{
				throw new TestRException("Could not find a valid debugger enabled page. Make sure you close the debugger tools.");
			}

			var sessionWsEndpoint = new Uri(session.WebSocketDebuggerUrl);
			_socket = new ClientWebSocket();

			if (!_socket.ConnectAsync(sessionWsEndpoint, CancellationToken.None).Wait(Application.Timeout))
			{
				throw new TestRException("Failed to connect to the server.");
			}

			Task.Run(() =>
			{
				while (ReadResponseAsync())
				{
					Thread.Sleep(1);
				}
			});
		}

		private List<RemoteSessionsResponse> GetAvailableSessions()
		{
			var request = (HttpWebRequest) WebRequest.Create("http://localhost:9222/json");

			using (var response = request.GetResponse())
			{
				var stream = response.GetResponseStream();
				if (stream == null)
				{
					throw new TestRException("Failed to get a response.");
				}

				using (var reader = new StreamReader(stream))
				{
					var sessions = JsonConvert.DeserializeObject<List<RemoteSessionsResponse>>(reader.ReadToEnd());
					sessions.RemoveAll(x => x.Url.StartsWith("chrome-extension"));
					sessions.RemoveAll(x => x.Url.StartsWith("chrome-devtools"));
					return sessions;
				}
			}
		}

		private bool ReadResponse(int id)
		{
			return Utility.Wait(() => _socketResponses.ContainsKey(id.ToString()), Application.Timeout.TotalMilliseconds, 1);
		}

		private bool ReadResponseAsync()
		{
			var buffer = new ArraySegment<byte>(new byte[131072]);
			var builder = new StringBuilder();

			try
			{
				WebSocketReceiveResult result;

				if (_socket.State == WebSocketState.Aborted || _socket.State == WebSocketState.Closed)
				{
					return false;
				}

				do
				{
					result = _socket.ReceiveAsync(buffer, CancellationToken.None).Result;
					var data = new byte[result.Count];
					Array.Copy(buffer.Array, 0, data, 0, data.Length);
					builder.Append(Encoding.UTF8.GetString(data));
				} while (!result.EndOfMessage);

				var response = (dynamic) builder.ToString().AsJToken();
				//LogManager.Write("Debugger Response: " + response, LogLevel.Verbose);

				if (response.id != null)
				{
					_socketResponses.TryAdd(response.id.ToString(), response);
				}

				return true;
			}
			catch (ObjectDisposedException)
			{
				return false;
			}
			catch (AggregateException)
			{
				return false;
			}
			catch
			{
				return false;
			}
		}

		private bool SendRequest<T>(T request)
		{
			var json = JsonConvert.SerializeObject(request, _jsonSerializerSettings);
			//LogManager.Write("Debugger Request: " + json, LogLevel.Verbose);
			var jsonBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(json));
			return _socket.SendAsync(jsonBuffer, WebSocketMessageType.Text, true, CancellationToken.None).Wait(Application.Timeout);
		}

		private string SendRequestAndReadResponse(Request request, Func<dynamic, bool> action)
		{
			if (SendRequest(request) && ReadResponse(request.Id))
			{
				dynamic response;
				_socketResponses.TryRemove(request.Id.ToString(), out response);
				return response == null ? string.Empty : response.ToString();
			}

			request.Id++;
			return SendRequestAndReadResponse(request, action);
		}

		#endregion

		#region Classes

		[Serializable]
		[DataContract]
		internal class RemoteSessionsResponse
		{
			#region Properties

			[DataMember]
			public string DevtoolsFrontendUrl { get; set; }

			[DataMember]
			public string FaviconUrl { get; set; }

			[DataMember]
			public string ThumbnailUrl { get; set; }

			[DataMember]
			public string Title { get; set; }

			[DataMember]
			public string Url { get; set; }

			[DataMember]
			public string WebSocketDebuggerUrl { get; set; }

			#endregion
		}

		private class Request
		{
			#region Properties

			public int Id { get; set; }
			public string Method { get; set; }
			public dynamic Params { get; set; }

			#endregion
		}

		#endregion
	}
}