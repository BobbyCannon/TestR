#region References

using System;
using System.Collections.Generic;
using System.Linq;

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

		#endregion
	}
}