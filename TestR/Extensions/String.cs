#region References

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

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