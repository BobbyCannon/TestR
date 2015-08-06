#region References

using System.Linq;
using TestR.Extensions;

#endregion

namespace TestR.Desktop
{
	/// <summary>
	/// Represents a collection of specific type of elements.
	/// </summary>
	/// <typeparam name="T"> The type of element. </typeparam>
	public class FilteredElementCollection<T> : ElementCollection<T>
		where T : Element
	{
		#region Constructors

		/// <summary>
		/// Initializes an instance of the ElementCollection class.
		/// </summary>
		private FilteredElementCollection(IElementParent parent)
			: base(parent)
		{
			parent.ChildrenUpdated += UpdateCollectionFromParent;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Creates a filtered element collection.
		/// </summary>
		/// <param name="parent"> The element parent for the filtered collection. </param>
		/// <returns> The filter element collections for the parent. </returns>
		public static FilteredElementCollection<T> Create(IElementParent parent)
		{
			var response = new FilteredElementCollection<T>(parent);
			response.UpdateCollectionFromParent();
			return response;
		}

		private void UpdateCollectionFromParent()
		{
			Clear();
			this.AddRange(Parent.Children.OfType<T>());
		}

		#endregion
	}
}