#region References

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using TestR.Desktop.Automation;
using TestR.Desktop.Elements;

#endregion

namespace TestR.Desktop
{
	/// <summary>
	/// Represents a collection of elements.
	/// </summary>
	public class ElementCollection<T> : Collection<T>
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
		public ElementCollection<Button> Buttons
		{
			get { return FilteredElementCollection<Button>.Create(Parent); }
		}

		/// <summary>
		/// Get a list of all check box elements.
		/// </summary>
		public ElementCollection<CheckBox> CheckBoxes
		{
			get { return FilteredElementCollection<CheckBox>.Create(Parent); }
		}

		/// <summary>
		/// Get a list of all custom elements.
		/// </summary>
		public ElementCollection<Custom> Customs
		{
			get { return FilteredElementCollection<Custom>.Create(Parent); }
		}

		/// <summary>
		/// Gets a list of all document elements.
		/// </summary>
		public ElementCollection<Document> Documents
		{
			get { return FilteredElementCollection<Document>.Create(Parent); }
		}

		/// <summary>
		/// Gets a list of all edit elements.
		/// </summary>
		public ElementCollection<Edit> Edits
		{
			get { return FilteredElementCollection<Edit>.Create(Parent); }
		}

		/// <summary>
		/// Gets a list of all group elements.
		/// </summary>
		public ElementCollection<Group> Groups
		{
			get { return FilteredElementCollection<Group>.Create(Parent); }
		}

		/// <summary>
		/// Gets a list of all list elements.
		/// </summary>
		public ElementCollection<List> Lists
		{
			get { return FilteredElementCollection<List>.Create(Parent); }
		}

		/// <summary>
		/// Gets a list of all menu bar elements.
		/// </summary>
		public ElementCollection<MenuBar> MenuBars
		{
			get { return FilteredElementCollection<MenuBar>.Create(Parent); }
		}

		/// <summary>
		/// Gets a list of all menu item elements.
		/// </summary>
		public ElementCollection<MenuItem> MenuItems
		{
			get { return FilteredElementCollection<MenuItem>.Create(Parent); }
		}

		/// <summary>
		/// Gets a list of all pane elements.
		/// </summary>
		public ElementCollection<Pane> Panes
		{
			get { return FilteredElementCollection<Pane>.Create(Parent); }
		}

		/// <summary>
		/// Gets the parent for this collection of elements.
		/// </summary>
		public IElementParent Parent { get; private set; }

		/// <summary>
		/// Gets a list of all scroll bar elements.
		/// </summary>
		public ElementCollection<ScrollBar> ScrollBars
		{
			get { return FilteredElementCollection<ScrollBar>.Create(Parent); }
		}

		/// <summary>
		/// Get a list of all split button elements.
		/// </summary>
		public ElementCollection<SplitButton> SplitButtons
		{
			get { return FilteredElementCollection<SplitButton>.Create(Parent); }
		}

		/// <summary>
		/// Gets a list of all status bar elements.
		/// </summary>
		public ElementCollection<StatusBar> StatusBars
		{
			get { return FilteredElementCollection<StatusBar>.Create(Parent); }
		}

		/// <summary>
		/// Gets a list of all table elements.
		/// </summary>
		public ElementCollection<Table> Tables
		{
			get { return FilteredElementCollection<Table>.Create(Parent); }
		}

		/// <summary>
		/// Gets a list of all thumb elements.
		/// </summary>
		public ElementCollection<Thumb> Thumbs
		{
			get { return FilteredElementCollection<Thumb>.Create(Parent); }
		}

		/// <summary>
		/// Gets a list of all title bar elements.
		/// </summary>
		public ElementCollection<TitleBar> TitleBars
		{
			get { return FilteredElementCollection<TitleBar>.Create(Parent); }
		}

		/// <summary>
		/// Gets a list of all tool bar elements.
		/// </summary>
		public ElementCollection<ToolBar> ToolBars
		{
			get { return FilteredElementCollection<ToolBar>.Create(Parent); }
		}

		/// <summary>
		/// Gets a list of all tree elements.
		/// </summary>
		public ElementCollection<Tree> Trees
		{
			get { return FilteredElementCollection<Tree>.Create(Parent); }
		}

		/// <summary>
		/// Get a list of all window elements.
		/// </summary>
		public ElementCollection<Window> Windows
		{
			get { return FilteredElementCollection<Window>.Create(Parent); }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Add an element to this collection.
		/// </summary>
		/// <param name="element"> The element to add. </param>
		public T Add(AutomationElement element)
		{
			var itemType = element.Current.ControlType.ProgrammaticName;

			switch (itemType)
			{
				case "ControlType.Button":
					return AddElement(new Button(element, Parent) as T);

				case "ControlType.CheckBox":
					return AddElement(new CheckBox(element, Parent) as T);

				case "ControlType.ComboBox":
					return AddElement(new ComboBox(element, Parent) as T);

				case "ControlType.Custom":
					return AddElement(new Custom(element, Parent) as T);

				case "ControlType.Document":
					return AddElement(new Document(element, Parent) as T);

				case "ControlType.Group":
					return AddElement(new Group(element, Parent) as T);

				case "ControlType.Pane":
					return AddElement(new Pane(element, Parent) as T);

				case "ControlType.Edit":
					return AddElement(new Edit(element, Parent) as T);

				case "ControlType.List":
					return AddElement(new List(element, Parent) as T);

				case "ControlType.ListItem":
					return AddElement(new ListItem(element, Parent) as T);

				case "ControlType.Hyperlink":
					return AddElement(new Hyperlink(element, Parent) as T);

				case "ControlType.MenuBar":
					return AddElement(new MenuBar(element, Parent) as T);

				case "ControlType.MenuItem":
					return AddElement(new MenuItem(element, Parent) as T);

				case "ControlType.ScrollBar":
					return AddElement(new ScrollBar(element, Parent) as T);

				case "ControlType.SplitButton":
					return AddElement(new SplitButton(element, Parent) as T);

				case "ControlType.StatusBar":
					return AddElement(new StatusBar(element, Parent) as T);

				case "ControlType.Table":
					return AddElement(new Table(element, Parent) as T);

				case "ControlType.Thumb":
					return AddElement(new Thumb(element, Parent) as T);

				case "ControlType.Text":
					return AddElement(new Text(element, Parent) as T);

				case "ControlType.Tree":
					return AddElement(new Tree(element, Parent) as T);

				case "ControlType.TitleBar":
					return AddElement(new TitleBar(element, Parent) as T);

				case "ControlType.ToolBar":
					return AddElement(new ToolBar(element, Parent) as T);

				case "ControlType.Window":
					return AddElement(new Window(element, Parent) as T);

				default:
					Debug.WriteLine("Need to add support for [" + itemType + "] element.");
					return AddElement(new Element(element, Parent) as T);
			}
		}

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
				Console.WriteLine(prefix + item.DebugString());
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
			get { return this.FirstOrDefault(x => x.Id == id) ?? this.FirstOrDefault(x => x.Name == id); }
		}

		#endregion
	}
}