#region References

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TestR.Desktop.Elements;
using TestR.Extensions;

#endregion

namespace TestR.Desktop
{
	/// <summary>
	/// Represents a collection of elements.
	/// </summary>
	public class ElementCollection<T> : ObservableCollection<T>
		where T : Element
	{
		#region Constructors

		/// <summary>
		/// Initializes an instance of the ElementCollection class.
		/// </summary>
		public ElementCollection()
		{
		}
		
		/// <summary>
		/// Initializes an instance of the ElementCollection class.
		/// </summary>
		/// <param name="collection"> The collection of elements to add to the new collection. </param>
		public ElementCollection(IEnumerable<T> collection)
		{
			this.AddRange(collection);
		}
		
		#endregion

		#region Properties

		/// <summary>
		/// Gets a list of all button elements.
		/// </summary>
		public ElementCollection<Button> Buttons => OfType<Button>();

		/// <summary>
		/// Get a list of all check box elements.
		/// </summary>
		public ElementCollection<CheckBox> CheckBoxes => OfType<CheckBox>();

		/// <summary>
		/// Get a list of all combo box elements.
		/// </summary>
		public ElementCollection<ComboBox> ComboBoxes => OfType<ComboBox>();

		/// <summary>
		/// Get a list of all custom elements.
		/// </summary>
		public ElementCollection<Custom> Customs => OfType<Custom>();

		/// <summary>
		/// Gets a list of all document elements.
		/// </summary>
		public ElementCollection<Document> Documents => OfType<Document>();

		/// <summary>
		/// Gets a list of all edit elements.
		/// </summary>
		public ElementCollection<Edit> Edits => OfType<Edit>();

		/// <summary>
		/// Gets a list of all group elements.
		/// </summary>
		public ElementCollection<Group> Groups => OfType<Group>();

		/// <summary>
		/// Gets a list of all hyperlink elements.
		/// </summary>
		public ElementCollection<Hyperlink> HyperLinks => OfType<Hyperlink>();

		/// <summary>
		/// Gets a list of all list item elements.
		/// </summary>
		public ElementCollection<ListItem> ListItems => OfType<ListItem>();

		/// <summary>
		/// Gets a list of all list elements.
		/// </summary>
		public ElementCollection<List> Lists => OfType<List>();

		/// <summary>
		/// Gets a list of all menu bar elements.
		/// </summary>
		public ElementCollection<MenuBar> MenuBars => OfType<MenuBar>();

		/// <summary>
		/// Gets a list of all menu item elements.
		/// </summary>
		public ElementCollection<MenuItem> MenuItems => OfType<MenuItem>();

		/// <summary>
		/// Gets a list of all pane elements.
		/// </summary>
		public ElementCollection<Pane> Panes => OfType<Pane>();

		/// <summary>
		/// Gets the parent for this collection of elements.
		/// </summary>
		public Element Parent { get; }

		/// <summary>
		/// Gets a list of all scroll bar elements.
		/// </summary>
		public ElementCollection<ScrollBar> ScrollBars => OfType<ScrollBar>();

		/// <summary>
		/// Gets a list of all split button elements.
		/// </summary>
		public ElementCollection<SplitButton> SplitButtons => OfType<SplitButton>();

		/// <summary>
		/// Gets a list of all status bar elements.
		/// </summary>
		public ElementCollection<StatusBar> StatusBars => OfType<StatusBar>();

		/// <summary>
		/// Gets a list of all table elements.
		/// </summary>
		public ElementCollection<Table> Tables => OfType<Table>();

		/// <summary>
		/// Gets a list of all thumb elements.
		/// </summary>
		public ElementCollection<Thumb> Thumbs => OfType<Thumb>();

		/// <summary>
		/// Gets a list of all title bar elements.
		/// </summary>
		public ElementCollection<TitleBar> TitleBars => OfType<TitleBar>();

		/// <summary>
		/// Gets a list of all tool bar elements.
		/// </summary>
		public ElementCollection<ToolBar> ToolBars => OfType<ToolBar>();

		/// <summary>
		/// Gets a list of all tree elements.
		/// </summary>
		public ElementCollection<Tree> Trees => OfType<Tree>();

		/// <summary>
		/// Get a list of all window elements.
		/// </summary>
		public ElementCollection<Window> Windows => OfType<Window>();

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
		public T1 GetChild<T1>(Func<T1, bool> condition, bool includeDescendants = true) where T1 : Element
		{
			var children = this.OfType<T1>().ToList();
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
		/// Prints out all children as a debug string.
		/// </summary>
		/// <param name="prefix"> Prefix to the debug information. </param>
		public ElementCollection<T> PrintDebug(string prefix = "")
		{
			foreach (var item in this)
			{
				Console.WriteLine(prefix + item.ToDetailString().Replace(Environment.NewLine, ", "));
				item.Children.PrintDebug(prefix + "    ");
			}

			return this;
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

		/// <summary>
		/// Gets a collection of element of the provided type.
		/// </summary>
		/// <typeparam name="T1"> The type of the element for the collection. </typeparam>
		/// <returns> The collection of elements of the provided type. </returns>
		public ElementCollection<T1> OfType<T1>() where T1 : Element
		{
			return new ElementCollection<T1>(this.Where(x => x.GetType() == typeof(T1)).Cast<T1>());
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