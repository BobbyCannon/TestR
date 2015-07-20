#region References

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

namespace TestR.Editor.Extensions
{
	public static class EnumerableExtensions
	{
		#region Methods

		/// <summary>
		/// Gets a list of structure elements into a single collection.
		/// </summary>
		/// <returns> A collection of the items. </returns>
		public static IEnumerable<ElementReference> Descendants(this ObservableCollection<ElementReference> collection)
		{
			var nodes = new Stack<ElementReference>(collection);
			while (nodes.Any())
			{
				var node = nodes.Pop();
				yield return node;
				foreach (var n in node.Children)
				{
					nodes.Push(n);
				}
			}
		}

		#endregion
	}
}