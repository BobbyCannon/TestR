#region References

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TestR.Web;
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
			var response = 0;

			if ((type & BrowserType.Chrome) == BrowserType.Chrome)
			{
				response++;
			}

			//if ((type & BrowserType.Edge) == BrowserType.Edge)
			//{
			//	response++;
			//}

			if ((type & BrowserType.InternetExplorer) == BrowserType.InternetExplorer)
			{
				response++;
			}

			if ((type & BrowserType.Firefox) == BrowserType.Firefox)
			{
				response++;
			}

			return response;
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
		/// Formats the string to be able to include inside inner string.
		/// </summary>
		/// <param name="source"> The source string value. </param>
		/// <returns> The string formatted to be place inside inner string. </returns>
		public static string FormatForInnerString(this string source)
		{
			return source.Replace("\\", "\\\\");
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
			return int.TryParse(item, out int response) ? response : 0;
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