#region References

using System;
using System.Collections.Generic;
using System.Linq;
using Interop.UIAutomationClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TestR.Web;
using ExpandCollapseState = TestR.Desktop.Pattern.ExpandCollapseState;
using ToggleState = TestR.Desktop.Pattern.ToggleState;

#endregion

namespace TestR.Internal
{
	internal static class Extensions
	{
		#region Methods

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
			var types = new[] { BrowserType.Chrome, BrowserType.Edge, BrowserType.Firefox };
			return types.Where(type => (browserType & type) == type).ToArray();
		}

		internal static ToggleState Convert(this Interop.UIAutomationClient.ToggleState state)
		{
			switch (state)
			{
				case Interop.UIAutomationClient.ToggleState.ToggleState_Off:
					return ToggleState.Off;

				case Interop.UIAutomationClient.ToggleState.ToggleState_On:
					return ToggleState.On;

				case Interop.UIAutomationClient.ToggleState.ToggleState_Indeterminate:
					return ToggleState.Indeterminate;

				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}
		}

		internal static ExpandCollapseState Convert(this Interop.UIAutomationClient.ExpandCollapseState state)
		{
			switch (state)
			{
				case Interop.UIAutomationClient.ExpandCollapseState.ExpandCollapseState_Collapsed:
					return ExpandCollapseState.Collapsed;

				case Interop.UIAutomationClient.ExpandCollapseState.ExpandCollapseState_Expanded:
					return ExpandCollapseState.Expanded;

				case Interop.UIAutomationClient.ExpandCollapseState.ExpandCollapseState_PartiallyExpanded:
					return ExpandCollapseState.PartiallyExpanded;

				case Interop.UIAutomationClient.ExpandCollapseState.ExpandCollapseState_LeafNode:
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

		#endregion
	}
}