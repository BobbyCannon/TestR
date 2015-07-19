#region References

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SHDocVw;

#endregion

namespace TestR.Extensions
{
	/// <summary>
	/// Represents a static helper class.
	/// </summary>
	public static partial class Helper
	{
		#region Methods

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

		#endregion
	}
}