#region References

using System;

#endregion

namespace TestR.Desktop
{
	/// <summary>
	/// Represents an element parent.
	/// </summary>
	public interface IElementParent
	{
		#region Properties

		/// <summary>
		/// Gets the children for this element.
		/// </summary>
		ElementCollection<Element> Children { get; }

		/// <summary>
		/// Gets the ID of this element.
		/// </summary>
		string Id { get; }

		/// <summary>
		/// Gets the name of this element.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets or sets the time out for delay request.
		/// </summary>
		TimeSpan Timeout { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Get a child of a certain type and key.
		/// </summary>
		/// <typeparam name="T"> The type of the child. </typeparam>
		/// <param name="key"> The key of the child. </param>
		/// <param name="includeDescendance"> Flag to determine to include descendance or not. </param>
		/// <returns> The child if found or null if otherwise. </returns>
		T GetChild<T>(string key, bool includeDescendance) where T : Element, IElementParent;

		/// <summary>
		/// Update the children for this element.
		/// </summary>
		void UpdateChildren();

		/// <summary>
		/// Wait for the child to be available then return it.
		/// </summary>
		/// <param name="id"> The ID of the child to wait for. </param>
		/// <param name="includeDescendance"> Flag to determine to include descendance or not. </param>
		/// <returns> The child element for the ID. </returns>
		Element WaitForChild(string id, bool includeDescendance);

		/// <summary>
		/// Wait for the child to be available then return it.
		/// </summary>
		/// <param name="id"> The ID of the child to wait for. </param>
		/// <param name="includeDescendance"> Flag to determine to include descendance or not. </param>
		/// <returns> The child element for the ID. </returns>
		T WaitForChild<T>(string id, bool includeDescendance) where T : Element;

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the children are updated.
		/// </summary>
		event Action ChildrenUpdated;

		#endregion
	}
}