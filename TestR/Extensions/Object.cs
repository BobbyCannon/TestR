#region References

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

#endregion

namespace TestR.Extensions
{
	/// <summary>
	/// Represents a static helper class.
	/// </summary>
	public static partial class Helper
	{
		#region Fields

		internal static readonly JsonSerializerSettings DefaultSerializerSettings;

		#endregion

		#region Constructors

		static Helper()
		{
			// Setup the JSON formatter.
			DefaultSerializerSettings = GetSerializerSettings(true);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Dumps the object to Console.WriteLine.
		/// </summary>
		/// <param name="value"> The value to dump to the console. </param>
		/// <param name="label"> The label to prefix the value. </param>
		public static void Dump(this object value, string label = "")
		{
			Console.WriteLine(string.IsNullOrWhiteSpace(label) ? value.ToString() : label + ":" + value);
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