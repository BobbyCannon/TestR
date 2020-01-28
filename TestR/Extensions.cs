﻿#region References

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TestR.Web;
using UIAutomationClient;
using ExpandCollapseState = TestR.Desktop.Pattern.ExpandCollapseState;
using ToggleState = TestR.Desktop.Pattern.ToggleState;

#pragma warning disable 618

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
		/// Returns the number of browsers for this type.
		/// </summary>
		/// <param name="type"> The browser type that contains the configuration. </param>
		/// <returns> The number of browsers configured in the type. </returns>
		public static int Count(this BrowserType type)
		{
			return type.GetTypeArray().Length;
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
		/// Run a test against each browser. BrowserType property will determine which browsers to run the test against.
		/// </summary>
		/// <param name="browserType"> The browser types to run the action against. </param>
		/// <param name="action"> The action to run each browser against. </param>
		/// <param name="useSecondaryMonitor"> The flag to determine to attempt to use secondary monitor. </param>
		/// <param name="resizeBrowsers"> The flag to determine to resize the browsers. </param>
		/// <param name="timeout"> The timeout in milliseconds. </param>
		/// <param name="resizeType"> The resize type if browser resizing is enabled. </param>
		/// <seealso cref="BrowserType" />
		/// <seealso cref="BrowserResizeType" />
		public static void ForAllBrowsers(this BrowserType browserType, Action<Browser> action, bool useSecondaryMonitor = true, bool resizeBrowsers = true, int timeout = 30000, BrowserResizeType resizeType = BrowserResizeType.SideBySide)
		{
			var browserTypes = browserType.GetTypeArray();
			var size = CalculateBrowserSize(useSecondaryMonitor, browserTypes, resizeType, out var leftOffset, out var topOffset);

			Browser.ForAllBrowsers(browser =>
			{
				try
				{
					if (resizeBrowsers)
					{
						var offset = CalculateStart(resizeType, browserTypes, browser, leftOffset, size, topOffset);
						//Debug.WriteLine($"{browser.BrowserType} X: {offset.X} Y: {offset.Y} W: {size.Width} H: {size.Height}");
						browser.MoveWindow(offset.X, offset.Y, size.Width, size.Height);
						browser.Application.MoveWindow(offset.X, offset.Y, size.Width, size.Height);
					}

					browser.BringToFront();
					browser.NavigateTo("about:blank");
					action(browser);
				}
				catch (Exception ex)
				{
					throw new Exception($"Test failed using {browser.BrowserType}.", ex);
				}
			}, browserType, TimeSpan.FromMilliseconds(timeout));
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
		/// Run a test against each browser. BrowserType property will determine which browsers to run the test against.
		/// </summary>
		/// <param name="browserType"> The browser types to run the action against. </param>
		/// <param name="action"> The action to run each browser against. </param>
		/// <param name="useSecondaryMonitor"> The flag to determine to attempt to use secondary monitor. </param>
		/// <param name="resizeBrowsers"> The flag to determine to resize the browsers. </param>
		/// <param name="resizeType"> The resize type if browser resizing is enabled. </param>
		/// <seealso cref="BrowserType" />
		/// <seealso cref="BrowserResizeType" />
		public static void ForEachBrowser(this BrowserType browserType, Action<Browser> action, bool useSecondaryMonitor = true, bool resizeBrowsers = true, BrowserResizeType resizeType = BrowserResizeType.SideBySide)
		{
			var screen = useSecondaryMonitor ? Screen.AllScreens.FirstOrDefault(x => x.Primary == false) ?? Screen.AllScreens.First(x => x.Primary) : Screen.AllScreens.First(x => x.Primary);
			var browserTypes = browserType.GetTypeArray();
			var size = CalculateBrowserSize(useSecondaryMonitor, browserTypes, resizeType, out var leftOffset, out var topOffset);

			Browser.ForEachBrowser(browser =>
			{
				try
				{
					if (resizeBrowsers)
					{
						var offset = CalculateStart(resizeType, browserTypes, browser, leftOffset, size, topOffset);
						//Debug.WriteLine($"{browser.BrowserType} X: {offset.X} Y: {offset.Y} W: {size.Width} H: {size.Height}");
						browser.MoveWindow(offset.X, offset.Y, size.Width, size.Height);
						browser.Application.MoveWindow(offset.X, offset.Y, size.Width, size.Height);
					}

					browser.BringToFront();
					action(browser);
				}
				catch (Exception ex)
				{
					throw new Exception($"Test failed using {browser.BrowserType}.", ex);
				}
			}, browserType);
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
		/// Formats the string to be able to include inside inner string.
		/// </summary>
		/// <param name="source"> The source string value. </param>
		/// <returns> The string formatted to be place inside inner string. </returns>
		public static string FormatForInnerString(this string source)
		{
			return source.Replace("\\", "\\\\");
		}

		/// <summary>
		/// Converts a browser type enum into an array of only individual browser types.
		/// </summary>
		/// <param name="browserType"> The browser type to convert. </param>
		/// <returns> The individual browser type values in the provided type. </returns>
		public static BrowserType[] GetTypeArray(this BrowserType browserType)
		{
			var types = new[] { BrowserType.Chrome, BrowserType.Edge, BrowserType.Firefox, BrowserType.InternetExplorer };
			return types.Where(type => (browserType & type) == type).ToArray();
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
			return int.TryParse(item, out var response) ? response : 0;
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

		private static Size CalculateBrowserSize(bool useSecondaryMonitor, BrowserType[] browserTypes, BrowserResizeType resizeType, out int leftOffset, out int topOffset)
		{
			var screen = useSecondaryMonitor
				? Screen.AllScreens.FirstOrDefault(x => x.Primary == false) ?? Screen.AllScreens.First(x => x.Primary)
				: Screen.AllScreens.First(x => x.Primary);

			topOffset = screen.WorkingArea.Top;

			switch (resizeType)
			{
				case BrowserResizeType.LeftSideBySide:
				case BrowserResizeType.RightSideBySide:
				{
					var horizontalCount = browserTypes.Length > 2 ? 2 : browserTypes.Length;
					var verticalCount = browserTypes.Length > 2 ? 2 : 1;
					var browserHeight = screen.WorkingArea.Height / verticalCount;
					var browserWidth = screen.WorkingArea.Width / 2 / horizontalCount;
					leftOffset = resizeType == BrowserResizeType.RightSideBySide
						? screen.WorkingArea.Left + screen.WorkingArea.Width / 2
						: screen.WorkingArea.Left;
					return new Size(browserWidth, browserHeight);
				}
				case BrowserResizeType.SideBySide:
				default:
				{
					var browserWidth = screen.WorkingArea.Width / browserTypes.Length;
					leftOffset = screen.WorkingArea.Left;
					return new Size(browserWidth, screen.WorkingArea.Height);
				}
			}
		}

		private static Point CalculateStart(BrowserResizeType resizeType, BrowserType[] browserTypes, Browser browser, int leftOffset, Size size, int topOffset)
		{
			var xOffset = Array.IndexOf(browserTypes, browser.BrowserType);
			var yOffset = 0;

			if (resizeType != BrowserResizeType.SideBySide)
			{
				yOffset = xOffset / 2;
			}

			if (resizeType != BrowserResizeType.SideBySide && xOffset >= 2)
			{
				xOffset -= 2;
			}

			var x = leftOffset + xOffset * size.Width;
			var y = topOffset + yOffset * size.Height;
			return new Point(x, y);
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