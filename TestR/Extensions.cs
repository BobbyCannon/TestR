#region References

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TestR.Desktop;
using TestR.Desktop.Elements;
using TestR.Native;
using UIAutomationClient;
using ExpandCollapseState = TestR.Desktop.Pattern.ExpandCollapseState;
using ToggleState = TestR.Desktop.Pattern.ToggleState;

#endregion

namespace TestR
{
	/// <summary>
	/// Container for all extension methods.
	/// </summary>
	public static class Extensions
	{
		#region Methods

		/// <summary>
		/// Adds a range of items to the collection.
		/// </summary>
		/// <param name="collection"> The collection to add the items to. </param>
		/// <param name="items"> The items to add to the collection. </param>
		/// <typeparam name="T"> The type of the item in the collections. </typeparam>
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				collection.Add(item);
			}
		}

		/// <summary>
		/// Deserialize JSON data into a JToken class.
		/// </summary>
		/// <param name="data"> The JSON data to deserialize. </param>
		/// <returns> The JToken class of the data. </returns>
		public static JToken AsJToken(this string data)
		{
			var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
			return (JToken) JsonConvert.DeserializeObject(data, jsonSerializerSettings);
		}

		/// <summary>
		/// Check to see if the string contains the value.
		/// </summary>
		/// <param name="source"> The source string value. </param>
		/// <param name="value"> The value to search for. </param>
		/// <param name="comparisonType"> The type of comparison to use when searching. </param>
		/// <returns> True if the value is found and false if otherwise. </returns>
		public static bool Contains(this string source, string value, StringComparison comparisonType)
		{
			return source.IndexOf(value, comparisonType) >= 0;
		}

		/// <summary>
		/// Return the first string that is not null or empty.
		/// </summary>
		/// <param name="collection"> The collection of string to parse. </param>
		public static string FirstValue(this IEnumerable<string> collection)
		{
			return collection.FirstOrDefault(item => !string.IsNullOrEmpty(item));
		}

		/// <summary>
		/// Performs an action on each item in the collection.
		/// </summary>
		/// <param name="collection"> The collection of items to run the action with. </param>
		/// <param name="action"> The action to run against each item in the collection. </param>
		/// <typeparam name="T"> The type of the item in the collection. </typeparam>
		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			foreach (var item in collection)
			{
				action(item);
			}
		}

		/// <summary>
		/// Performs an action on each item in the collection.
		/// </summary>
		/// <param name="collection"> The collection of items to run the action with. </param>
		/// <param name="action"> The action to run against each item in the collection. </param>
		/// <typeparam name="T"> The type of the item in the collection. </typeparam>
		public static void ForEachDisposable<T>(this IEnumerable<T> collection, Action<T> action)
			where T : IDisposable
		{
			foreach (var item in collection)
			{
				using (item)
				{
					action(item);
				}
			}
		}

		/// <summary>
		/// First the main window location for the process.
		/// </summary>
		/// <param name="process"> The process that contains the window. </param>
		/// <returns> The location of the window. </returns>
		public static Point GetWindowLocation(this Process process)
		{
			var p = NativeMethods.GetWindowPlacement(process.MainWindowHandle);
			var location = p.rcNormalPosition.Location;

			if ((p.ShowState == 2) || (p.ShowState == 3))
			{
				NativeMethods.Rect windowsRect;
				NativeMethods.GetWindowRect(process.MainWindowHandle, out windowsRect);
				location = new Point(windowsRect.Left + 8, windowsRect.Top + 8);
			}

			return location;
		}

		/// <summary>
		/// Gets all windows for the process.
		/// </summary>
		/// <param name="process"> The process to get windows for. </param>
		/// <param name="application"> The application the elements are for. </param>
		/// <returns> The array of windows. </returns>
		public static IEnumerable<Window> GetWindows(this Process process, Application application)
		{
			// There is a issue in Windows 10 and Cortana (or modern apps) where there is a 60 second delay when walking the root element.
			// When you hit the last sibling it delays. For now we are simply going to return the main window and we'll roll this code
			// back once the Windows 10 issue has been resolved.

			process.Refresh();
			var response = new List<Window>();
			var automation = new CUIAutomationClass();

			foreach (var handle in EnumerateProcessWindowHandles(process.Id))
			{
				var automationElement = automation.ElementFromHandle(handle);
				var element = DesktopElement.Create(automationElement, application, null) as Window;
				if (element != null)
				{
					response.Add(element);
				}
			}

			return response;
		}

		/// <summary>
		/// Gets the size of the main window for the process.
		/// </summary>
		/// <param name="process"> The process to size. </param>
		/// <returns> The size of the main window. </returns>
		public static Size GetWindowSize(this Process process)
		{
			NativeMethods.Rect data;
			NativeMethods.GetWindowRect(process.MainWindowHandle, out data);
			return new Size(data.Right - data.Left, data.Bottom - data.Top);
		}

		/// <summary>
		/// Checks to see if the assembly passed in is a debug build.
		/// </summary>
		/// <param name="assembly"> The assembly to test. </param>
		/// <returns> True if is a debug build and false if a release build. </returns>
		public static bool IsAssemblyDebugBuild(this Assembly assembly)
		{
			var retVal = false;

			foreach (var att in assembly.GetCustomAttributes(false))
			{
				if (att.GetType() == Type.GetType("System.Diagnostics.DebuggableAttribute"))
				{
					retVal = ((DebuggableAttribute) att).IsJITTrackingEnabled;
				}
			}

			return retVal;
		}

		/// <summary>
		/// Splits a by a single separator.
		/// </summary>
		/// <param name="value"> The string to be split. </param>
		/// <param name="separator"> The character to deliminate the string. </param>
		/// <param name="options"> The options to use when splitting. </param>
		/// <returns> The array of strings. </returns>
		public static string[] Split(this string value, string separator, StringSplitOptions options = StringSplitOptions.None)
		{
			return value.Split(new[] { separator }, options);
		}

		/// <summary>
		/// Converts the string to an integer.
		/// </summary>
		/// <param name="item"> The item to convert to an integer. </param>
		/// <returns> The JSON data of the object. </returns>
		public static int ToInt(this string item)
		{
			int response;
			return int.TryParse(item, out response) ? response : 0;
		}

		/// <summary>
		/// Serializes the object to JSON.
		/// </summary>
		/// <typeparam name="T"> The type of the item. </typeparam>
		/// <param name="item"> The item to serialize. </param>
		/// <param name="camelCase"> The flag to use camel case. If true then camel case else pascel case. </param>
		/// <returns> The JSON data of the object. </returns>
		public static string ToJson<T>(this T item, bool camelCase = true)
		{
			return JsonConvert.SerializeObject(item, Formatting.None, GetSerializerSettings(camelCase));
		}

		internal static ToggleState Convert(this UIAutomationClient.ToggleState state)
		{
			switch (state)
			{
				case UIAutomationClient.ToggleState.ToggleState_Off:
					return ToggleState.Off;

				case UIAutomationClient.ToggleState.ToggleState_On:
					return ToggleState.On;

				case UIAutomationClient.ToggleState.ToggleState_Indeterminate:
					return ToggleState.Indeterminate;

				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}
		}

		internal static ExpandCollapseState Convert(this UIAutomationClient.ExpandCollapseState state)
		{
			switch (state)
			{
				case UIAutomationClient.ExpandCollapseState.ExpandCollapseState_Collapsed:
					return ExpandCollapseState.Collapsed;

				case UIAutomationClient.ExpandCollapseState.ExpandCollapseState_Expanded:
					return ExpandCollapseState.Expanded;

				case UIAutomationClient.ExpandCollapseState.ExpandCollapseState_PartiallyExpanded:
					return ExpandCollapseState.PartiallyExpanded;

				case UIAutomationClient.ExpandCollapseState.ExpandCollapseState_LeafNode:
					return ExpandCollapseState.LeafNode;

				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}
		}

		internal static IUIAutomationElement GetCurrentParent(this IUIAutomationElement element)
		{
			var automation = new CUIAutomationClass();
			var walker = automation.CreateTreeWalker(automation.RawViewCondition);
			return walker.GetParentElement(element);
		}

		private static IEnumerable<IntPtr> EnumerateProcessWindowHandles(int processId)
		{
			var handles = new List<IntPtr>();

			foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
			{
				NativeMethods.EnumThreadWindows(thread.Id, (hWnd, lParam) =>
				{
					handles.Add(hWnd);
					return true;
				}, IntPtr.Zero);
			}

			return handles;
		}

		private static JsonSerializerSettings GetSerializerSettings(bool camelCase = false)
		{
			var response = new JsonSerializerSettings();
			response.Converters.Add(new IsoDateTimeConverter());
			response.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;

			if (camelCase)
			{
				response.Converters.Add(new StringEnumConverter { CamelCaseText = true });
				response.ContractResolver = new CamelCasePropertyNamesContractResolver();
			}

			return response;
		}

		#endregion
	}
}