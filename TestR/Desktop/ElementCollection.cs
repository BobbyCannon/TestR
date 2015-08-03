#region References

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TestR.Desktop.Elements;

#endregion

namespace TestR.Desktop
{
	/// <summary>
	/// Represents a collection of elements.
	/// </summary>
	public class ElementCollection<T> : ObservableCollection<T>
		where T : Element, IElementParent
	{
		#region Constructors

		/// <summary>
		/// Initializes an instance of the ElementCollection class.
		/// </summary>
		/// <param name="parent"> The parent element for this collection. </param>
		public ElementCollection(IElementParent parent)
		{
			Parent = parent;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a list of all button elements.
		/// </summary>
		public ElementCollection<Button> Buttons => FilteredElementCollection<Button>.Create(Parent);

		/// <summary>
		/// Get a list of all check box elements.
		/// </summary>
		public ElementCollection<CheckBox> CheckBoxes => FilteredElementCollection<CheckBox>.Create(Parent);

		/// <summary>
		/// Get a list of all combo box elements.
		/// </summary>
		public ElementCollection<ComboBox> ComboBoxes => FilteredElementCollection<ComboBox>.Create(Parent);

		/// <summary>
		/// Get a list of all custom elements.
		/// </summary>
		public ElementCollection<Custom> Customs => FilteredElementCollection<Custom>.Create(Parent);

		/// <summary>
		/// Gets a list of all document elements.
		/// </summary>
		public ElementCollection<Document> Documents => FilteredElementCollection<Document>.Create(Parent);

		/// <summary>
		/// Gets a list of all edit elements.
		/// </summary>
		public ElementCollection<Edit> Edits => FilteredElementCollection<Edit>.Create(Parent);

		/// <summary>
		/// Gets a list of all group elements.
		/// </summary>
		public ElementCollection<Group> Groups => FilteredElementCollection<Group>.Create(Parent);

		/// <summary>
		/// Gets a list of all hyperlink elements.
		/// </summary>
		public ElementCollection<Hyperlink> HyperLinks => FilteredElementCollection<Hyperlink>.Create(Parent);

		/// <summary>
		/// Gets a list of all list item elements.
		/// </summary>
		public ElementCollection<ListItem> ListItems => FilteredElementCollection<ListItem>.Create(Parent);

		/// <summary>
		/// Gets a list of all list elements.
		/// </summary>
		public ElementCollection<List> Lists => FilteredElementCollection<List>.Create(Parent);

		/// <summary>
		/// Gets a list of all menu bar elements.
		/// </summary>
		public ElementCollection<MenuBar> MenuBars => FilteredElementCollection<MenuBar>.Create(Parent);

		/// <summary>
		/// Gets a list of all menu item elements.
		/// </summary>
		public ElementCollection<MenuItem> MenuItems => FilteredElementCollection<MenuItem>.Create(Parent);

		/// <summary>
		/// Gets a list of all pane elements.
		/// </summary>
		public ElementCollection<Pane> Panes => FilteredElementCollection<Pane>.Create(Parent);

		/// <summary>
		/// Gets the parent for this collection of elements.
		/// </summary>
		public IElementParent Parent { get; }

		/// <summary>
		/// Gets a list of all scroll bar elements.
		/// </summary>
		public ElementCollection<ScrollBar> ScrollBars => FilteredElementCollection<ScrollBar>.Create(Parent);

		/// <summary>
		/// Gets a list of all split button elements.
		/// </summary>
		public ElementCollection<SplitButton> SplitButtons => FilteredElementCollection<SplitButton>.Create(Parent);

		/// <summary>
		/// Gets a list of all status bar elements.
		/// </summary>
		public ElementCollection<StatusBar> StatusBars => FilteredElementCollection<StatusBar>.Create(Parent);

		/// <summary>
		/// Gets a list of all table elements.
		/// </summary>
		public ElementCollection<Table> Tables => FilteredElementCollection<Table>.Create(Parent);

		/// <summary>
		/// Gets a list of all thumb elements.
		/// </summary>
		public ElementCollection<Thumb> Thumbs => FilteredElementCollection<Thumb>.Create(Parent);

		/// <summary>
		/// Gets a list of all title bar elements.
		/// </summary>
		public ElementCollection<TitleBar> TitleBars => FilteredElementCollection<TitleBar>.Create(Parent);

		/// <summary>
		/// Gets a list of all tool bar elements.
		/// </summary>
		public ElementCollection<ToolBar> ToolBars => FilteredElementCollection<ToolBar>.Create(Parent);

		/// <summary>
		/// Gets a list of all tree elements.
		/// </summary>
		public ElementCollection<Tree> Trees => FilteredElementCollection<Tree>.Create(Parent);

		/// <summary>
		/// Get a list of all window elements.
		/// </summary>
		public ElementCollection<Window> Windows => FilteredElementCollection<Window>.Create(Parent);

		#endregion

		#region Methods

		/// <summary>
		/// Check to see if this collection contains an element.
		/// </summary>
		/// <param name="key"> The key to search for. </param>
		/// <returns> True if the key is found, false if otherwise. </returns>
		public bool ContainsKey(string key)
		{
			return this[key] != null;
		}

		/// <summary>
		/// Get a child of a certain type and key.
		/// </summary>
		/// <param name="key"> The key of the child. </param>
		/// <param name="includeDescendants"> Flag to determine to include descendants or not. </param>
		/// <returns> The child if found or null if otherwise. </returns>
		public T GetChild(string key, bool includeDescendants = true)
		{
			var response = this[key];
			if (!includeDescendants)
			{
				return response;
			}

			if (response != null)
			{
				return response;
			}

			foreach (var child in this)
			{
				response = child.GetChild<T>(key);
				if (response != null)
				{
					return response;
				}
			}

			return null;
		}

		/// <summary>
		/// Get a child of a certain type and meets the condition.
		/// </summary>
		/// <typeparam name="T1"> The type of the child. </typeparam>
		/// <param name="condition"> A function to test each element for a condition. </param>
		/// <param name="includeDescendants"> Flag to determine to include descendants or not. </param>
		/// <returns> The child if found or null if otherwise. </returns>
		public T1 GetChild<T1>(Func<T1, bool> condition, bool includeDescendants = true) where T1 : Element, IElementParent
		{
			var children = OfType<T1>().ToList();
			var response = children.FirstOrDefault(condition);
			if (!includeDescendants)
			{
				return response;
			}

			if (response != null)
			{
				return response;
			}

			foreach (var child in this)
			{
				response = child.GetChild(condition);
				if (response != null)
				{
					return response;
				}
			}

			return null;
		}

		/// <summary>
		/// Gets a collection of element of the provided type.
		/// </summary>
		/// <typeparam name="TChild"> The type of the element for the collection. </typeparam>
		/// <returns> The collection of elements of the provided type. </returns>
		public IEnumerable<TChild> OfType<TChild>() where TChild : IElementParent
		{
			return this.Where(x => x.GetType() == typeof (TChild)).Cast<TChild>().ToList();
		}

		/// <summary>
		/// Prints out all children as a debug string.
		/// </summary>
		/// <param name="prefix"> Prefix to the debug information. </param>
		public void PrintDebug(string prefix = "")
		{
			foreach (var item in this)
			{
				Console.WriteLine(prefix + item.ToDetailString().Replace(Environment.NewLine, ", "));
				item.Children.PrintDebug(prefix + "    ");
			}
		}

		/// <summary>
		/// Add the element to the collection.
		/// </summary>
		/// <param name="element"> The type of the element. </param>
		/// <returns> Returns the element that was added. </returns>
		private T AddElement(T element)
		{
			Add(element);
			return element;
		}

		#endregion

		#region Indexers

		/// <summary>
		/// Access an element by the ID or by Name.
		/// </summary>
		/// <param name="id"> The ID or Name of the element. </param>
		/// <returns> The element if found or null if not found. </returns>
		public T this[string id]
		{
			get
			{
				// Try to use the full application ID first.
				return this.FirstOrDefault(x => x.ApplicationId == id) ?? this.FirstOrDefault(x => x.Id == id);
			}
		}

		#endregion
	}
}