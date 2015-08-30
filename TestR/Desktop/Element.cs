#region References

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using TestR.Desktop.Elements;
using TestR.Desktop.Pattern;
using TestR.Exceptions;
using TestR.Extensions;
using TestR.Helpers;
using TestR.Native;
using UIAutomationClient;

#endregion

namespace TestR.Desktop
{
	/// <summary>
	/// Base element for desktop automation.
	/// </summary>
	public class Element
	{
		#region Fields

		/// <summary>
		/// Properties that should not be included in UI elements or the detail string.
		/// </summary>
		public static readonly string[] ExcludedProperties;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of an element.
		/// </summary>
		/// <param name="element"> The automation element for this element. </param>
		/// <param name="application"> The application parent for this element. </param>
		/// <param name="parent"> The parent element for this element. </param>
		internal Element(IUIAutomationElement element, Application application, Element parent)
		{
			Application = application;
			Children = new ElementCollection<Element>();
			NativeElement = element;
			Parent = parent;
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static Element()
		{
			ExcludedProperties = new[] { "Parent", "Children", "NativeElement", "Item" };
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the ID of this element in the application. Includes full application namespace.
		/// </summary>
		public string ApplicationId
		{
			get
			{
				var builder = new StringBuilder();
				var element = this;
				do
				{
					builder.Insert(0, new[] { element.Id, element.Name, " " }.FirstValue());
					element = element.Parent;
				} while (element != null);
				return builder.ToString();
			}
		}

		/// <summary>
		/// Gets the bounding rectangle of the element.
		/// </summary>
		public Rectangle BoundingRectangle
		{
			get
			{
				var rectangle = NativeElement.CurrentBoundingRectangle;
				return new Rectangle(rectangle.left, rectangle.top, rectangle.right - rectangle.left, rectangle.bottom - rectangle.top);
			}
		}

		/// <summary>
		/// Gets the children for this element.
		/// </summary>
		public ElementCollection<Element> Children { get; }

		/// <summary>
		/// Gets a value that indicates whether the element is enabled.
		/// </summary>
		public bool Enabled => NativeElement.CurrentIsEnabled == 1;

		/// <summary>
		/// Gets a value that indicates whether the element is focused.
		/// </summary>
		public bool Focused => FromFocusElement()?.Id == Id;

		/// <summary>
		/// Gets the height of the element.
		/// </summary>
		public int Height => Size.Height;

		/// <summary>
		/// Gets the ID of this element.
		/// </summary>
		public string Id => NativeElement.CurrentAutomationId;

		/// <summary>
		/// Gets a value that indicates whether the element can be use by the keyboard.
		/// </summary>
		public bool KeyboardFocusable => NativeElement.CurrentIsKeyboardFocusable == 1 && Enabled;

		/// <summary>
		/// Gets the location of the element.
		/// </summary>
		public Point Location => new Point(BoundingRectangle.X, BoundingRectangle.Y);

		/// <summary>
		/// Gets the name of this element.
		/// </summary>
		public string Name => NativeElement.CurrentName;

		/// <summary>
		/// Gets or sets the automation element for this element.
		/// </summary>
		public IUIAutomationElement NativeElement { get; }

		/// <summary>
		/// The parent element of this element.
		/// </summary>
		public Element Parent { get; private set; }

		/// <summary>
		/// Gets the current process ID  of the element.
		/// </summary>
		public int ProcessId => NativeElement.CurrentProcessId;

		/// <summary>
		/// Gets the size of the element.
		/// </summary>
		public Size Size
		{
			get
			{
				var test = NativeElement.CurrentBoundingRectangle;
				return new Size(test.right - test.left, test.bottom - test.top);
			}
		}

		/// <summary>
		/// Gets the type ID of this element.
		/// </summary>
		public int TypeId => NativeElement.CurrentControlType;

		/// <summary>
		/// Gets the name of the control type.
		/// </summary>
		public string TypeName => NativeElement.CurrentLocalizedControlType;

		/// <summary>
		/// Gets a value that indicates whether the element is visible.
		/// </summary>
		public bool Visible
		{
			get
			{
				tagPOINT point;
				var clickable = NativeElement.GetClickablePoint(out point) > 0 && (point.x != 0 && point.y != 0);
				var focused = Focused || Children.Any(x => x.Focused);
				return NativeElement.CurrentIsOffscreen == 0 && (clickable || focused);
			}
		}

		/// <summary>
		/// Gets the width of the element;
		/// </summary>
		public int Width => Size.Width;

		internal Application Application { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Performs mouse click at the center of the element.
		/// </summary>
		/// <param name="x"> Optional X offset when clicking. </param>
		/// <param name="y"> Optional Y offset when clicking. </param>
		public virtual Element Click(int x = 0, int y = 0)
		{
			var point = GetClickablePoint(x, y);
			Mouse.LeftClick(point);
			Thread.Sleep(1);
			return this;
		}

		/// <summary>
		/// Gets a list of structure elements into a single collection.
		/// </summary>
		/// <returns> A collection of the items. </returns>
		public IEnumerable<Element> Descendants()
		{
			var nodes = new Stack<Element>(new[] { this });
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

		/// <summary>
		/// Check to see if the element still exists.
		/// </summary>
		/// <returns> True if the element still exists and false if otherwise. </returns>
		public bool Exists()
		{
			try
			{
				NativeElement.GetRuntimeId();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Set focus on the element.
		/// </summary>
		public Element Focus()
		{
			NativeElement.SetFocus();
			return this;
		}

		/// <summary>
		/// Gets the element that is currently under the cursor.
		/// </summary>
		/// <returns> The element if found or null if not found. </returns>
		public static Element FromCursor()
		{
			var point = Mouse.GetCursorPosition();
			return FromPoint(point);
		}

		/// <summary>
		/// Gets the element that is currently at the point.
		/// </summary>
		/// <param name="point"> The point to try and detect at element at. </param>
		/// <returns> The element if found or null if not found. </returns>
		public static Element FromPoint(Point point)
		{
			var automation = new CUIAutomationClass();
			var element = automation.ElementFromPoint(new tagPOINT { x = point.X, y = point.Y });
			return element == null ? null : new Element(element, null, null);
		}

		/// <summary>
		/// Gets the element that is currently focused.
		/// </summary>
		/// <returns> The element if found or null if not found. </returns>
		public static Element FromFocusElement()
		{
			var automation = new CUIAutomationClass();
			var element = automation.GetFocusedElement();
			return element == null ? null : new Element(element, null, null);
		}

		/// <summary>
		/// Get a child of a certain type and key.
		/// </summary>
		/// <typeparam name="T"> The type of the child. </typeparam>
		/// <param name="key"> The key of the child. </param>
		/// <param name="includeDescendants"> Flag to determine to include descendants or not. </param>
		/// <returns> The child if found or null if otherwise. </returns>
		public T GetChild<T>(string key, bool includeDescendants = true)
			where T : Element
		{
			return (T) Children.GetChild(key, includeDescendants);
		}

		/// <summary>
		/// Get a child of a certain type that meets the condition.
		/// </summary>
		/// <typeparam name="T"> The type of the child. </typeparam>
		/// <param name="condition"> A function to test each element for a condition. </param>
		/// <param name="includeDescendants"> Flag to determine to include descendants or not. </param>
		/// <returns> The child if found or null if otherwise. </returns>
		public T GetChild<T>(Func<T, bool> condition, bool includeDescendants = true) where T : Element
		{
			return Children.GetChild(condition, includeDescendants);
		}

		/// <summary>
		/// Gets the first parent that has an ID.
		/// </summary>
		/// <param name="element"> The element to iterate. </param>
		/// <returns> The parent if found or null otherwise. </returns>
		public static Element GetFirstParentWithId(Element element)
		{
			return !string.IsNullOrEmpty(element.Id) ? element : GetFirstParentWithId(element.Parent);
		}

		/// <summary>
		/// Gets the text value of the element.
		/// </summary>
		/// <returns> The value of the element. </returns>
		public string GetText()
		{
			return ValuePattern.New(this)?.Value ?? string.Empty;
		}

		/// <summary>
		/// Moves mouse cursor to the center of the element.
		/// </summary>
		/// <param name="x"> Optional X offset when clicking. </param>
		/// <param name="y"> Optional Y offset when clicking. </param>
		public Element MoveMouseTo(int x = 0, int y = 0)
		{
			var point = GetClickablePoint(x, y);
			Mouse.MoveTo(point);
			return this;
		}

		/// <summary>
		/// Performs mouse right click at the center of the element.
		/// </summary>
		/// <param name="x"> Optional X offset when clicking. </param>
		/// <param name="y"> Optional Y offset when clicking. </param>
		public Element RightClick(int x = 0, int y = 0)
		{
			var point = GetClickablePoint(x, y);
			Mouse.RightClick(point);
			return this;
		}

		/// <summary>
		/// Sets the text value of the element.
		/// </summary>
		/// <param name="value"> The text to set the element to. </param>
		public Element SetText(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value), "String parameter must not be null.");
			}

			if (!Enabled)
			{
				throw new InvalidOperationException("The element is not enabled.");
			}

			var pattern = NativeElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
			if (pattern != null)
			{
				// Control supports the ValuePattern pattern so we can use the SetValue method to insert content. 
				pattern.SetValue(value);
				return this;
			}

			if (!KeyboardFocusable)
			{
				throw new NotKeyboardFocusableException("The element is read-only.");
			}

			// Set focus for input functionality and begin.
			NativeElement.SetFocus();

			// Pause before sending keyboard input.
			Thread.Sleep(100);
			Keyboard.TypeText(value);
			return this;
		}

		/// <summary>
		/// Takes a screenshot of the element.
		/// </summary>
		public Element TakeScreenshot(string filePath)
		{
			var result = new Bitmap(Width, Height);

			using (var graphics = Graphics.FromImage(result))
			{
				graphics.CopyFromScreen(Location, Point.Empty, Size);
			}

			result.Save(filePath, ImageFormat.Png);
			return this;
		}

		/// <summary>
		/// Provides a string of details for the element.
		/// </summary>
		/// <returns> The string of element details. </returns>
		public string ToDetailString()
		{
			var type = GetType();
			var properties = type.GetProperties()
				.Where(x => !ExcludedProperties.Contains(x.Name))
				.OrderBy(x => x.Name).ToList();
			var items = new Dictionary<string, string>(properties.Count);

			foreach (var property in properties)
			{
				var value = property.GetValue(this);
				if (value == null)
				{
					continue;
				}

				items.Add(property.Name, value.ToString());
			}

			return string.Join(Environment.NewLine, items.Where(x => !string.IsNullOrWhiteSpace(x.Value)).Select(x => x.Key + " - " + x.Value));
		}

		/// <summary>
		/// Focus the element then type the text via the keyboard.
		/// </summary>
		/// <param name="value"> The value to type. </param>
		public Element TypeText(string value)
		{
			Focus();
			Keyboard.TypeText(value);
			return this;
		}

		/// <summary>
		/// Update the children for this element.
		/// </summary>
		public Element UpdateChildren()
		{
			UpdateChildren(this);
			OnChildrenUpdated();
			return this;
		}

		/// <summary>
		/// Update the parents for this element.
		/// </summary>
		public Element UpdateParents()
		{
			UpdateParent(this);
			return this;
		}

		/// <summary>
		/// Wait for the child to be available then return it.
		/// </summary>
		/// <param name="id"> The ID of the child to wait for. </param>
		/// <param name="includeDescendants"> Flag to determine to include descendants or not. </param>
		/// <returns> The child element for the ID. </returns>
		public Element WaitForChild(string id, bool includeDescendants = true)
		{
			return WaitForChild<Element>(id, includeDescendants);
		}

		/// <summary>
		/// Wait for the child to be available then return it.
		/// </summary>
		/// <param name="id"> The ID of the child to wait for. </param>
		/// <param name="includeDescendants"> Flag to determine to include descendants or not. </param>
		/// <returns> The child element for the ID. </returns>
		public T WaitForChild<T>(string id, bool includeDescendants = true)
			where T : Element
		{
			T response = null;

			Utility.Wait(() =>
			{
				try
				{
					response = GetChild<T>(id, includeDescendants);
					if (response != null)
					{
						return true;
					}

					UpdateChildren();
					return false;
				}
				catch (COMException)
				{
					return false;
				}
			}, Application?.Timeout.TotalMilliseconds ?? 5000, 10);

			if (response == null)
			{
				throw new ArgumentException("Failed to find the child by ID.");
			}

			return response;
		}

		/// <summary>
		/// Wait for the child to be available and meet the condition then return it.
		/// </summary>
		/// <param name="condition"> A function to test each element for a condition. </param>
		/// <param name="includeDescendants"> Flag to determine to include descendants or not. </param>
		/// <returns> The child element for the ID. </returns>
		public T WaitForChild<T>(Func<T, bool> condition, bool includeDescendants = true) where T : Element
		{
			T response = null;

			Utility.Wait(() =>
			{
				try
				{
					response = GetChild(condition, includeDescendants);
					if (response != null)
					{
						return true;
					}

					UpdateChildren();
					return false;
				}
				catch (COMException)
				{
					return false;
				}
			}, Application?.Timeout.TotalMilliseconds ?? 5000, 10);

			if (response == null)
			{
				throw new ArgumentException("Failed to find the child by ID.");
			}

			return response;
		}

		/// <summary>
		/// Handles the ChildrenUpdated event.
		/// </summary>
		protected void OnChildrenUpdated()
		{
			ChildrenUpdated?.Invoke();
		}

		/// <summary>
		/// Creates an element from the automation element.
		/// </summary>
		/// <param name="element"> The element to create. </param>
		/// <param name="application"> The application parent for this element. </param>
		/// <param name="parent"> The parent of the element to create. </param>
		private static Element Create(IUIAutomationElement element, Application application, Element parent)
		{
			var itemType = element.CurrentControlType;

			switch (itemType)
			{
				case UIA_ControlTypeIds.UIA_ButtonControlTypeId:
					return new Button(element, application, parent);

				case UIA_ControlTypeIds.UIA_CheckBoxControlTypeId:
					return new CheckBox(element, application, parent);

				case UIA_ControlTypeIds.UIA_ComboBoxControlTypeId:
					return new ComboBox(element, application, parent);

				case UIA_ControlTypeIds.UIA_CustomControlTypeId:
					return new Custom(element, application, parent);

				case UIA_ControlTypeIds.UIA_DataItemControlTypeId:
					return new DataItem(element, application, parent);

				case UIA_ControlTypeIds.UIA_DocumentControlTypeId:
					return new Document(element, application, parent);

				case UIA_ControlTypeIds.UIA_EditControlTypeId:
					return new Edit(element, application, parent);

				case UIA_ControlTypeIds.UIA_GroupControlTypeId:
					return new Group(element, application, parent);

				case UIA_ControlTypeIds.UIA_HeaderControlTypeId:
					return new Header(element, application, parent);

				case UIA_ControlTypeIds.UIA_HeaderItemControlTypeId:
					return new HeaderItem(element, application, parent);

				case UIA_ControlTypeIds.UIA_HyperlinkControlTypeId:
					return new Hyperlink(element, application, parent);

				case UIA_ControlTypeIds.UIA_ListControlTypeId:
					return new List(element, application, parent);

				case UIA_ControlTypeIds.UIA_ListItemControlTypeId:
					return new ListItem(element, application, parent);

				case UIA_ControlTypeIds.UIA_MenuBarControlTypeId:
					return new MenuBar(element, application, parent);

				case UIA_ControlTypeIds.UIA_MenuItemControlTypeId:
					return new MenuItem(element, application, parent);

				case UIA_ControlTypeIds.UIA_PaneControlTypeId:
					return new Pane(element, application, parent);

				case UIA_ControlTypeIds.UIA_ScrollBarControlTypeId:
					return new ScrollBar(element, application, parent);

				case UIA_ControlTypeIds.UIA_SplitButtonControlTypeId:
					return new SplitButton(element, application, parent);

				case UIA_ControlTypeIds.UIA_StatusBarControlTypeId:
					return new StatusBar(element, application, parent);

				case UIA_ControlTypeIds.UIA_TabControlTypeId:
					return new TabControl(element, application, parent);

				case UIA_ControlTypeIds.UIA_TabItemControlTypeId:
					return new TabItem(element, application, parent);

				case UIA_ControlTypeIds.UIA_TableControlTypeId:
					return new Table(element, application, parent);

				case UIA_ControlTypeIds.UIA_TextControlTypeId:
					return new Text(element, application, parent);

				case UIA_ControlTypeIds.UIA_ThumbControlTypeId:
					return new Thumb(element, application, parent);

				case UIA_ControlTypeIds.UIA_TitleBarControlTypeId:
					return new TitleBar(element, application, parent);

				case UIA_ControlTypeIds.UIA_ToolBarControlTypeId:
					return new ToolBar(element, application, parent);

				case UIA_ControlTypeIds.UIA_TreeControlTypeId:
					return new Tree(element, application, parent);

				case UIA_ControlTypeIds.UIA_WindowControlTypeId:
					return new Window(element, application, parent);

				default:
					Debug.WriteLine("Need to add support for [" + itemType + "] element.");
					return new Element(element, application, parent);
			}
		}

		/// <summary>
		/// Gets all the direct children of an element.
		/// </summary>
		/// <param name="element"> The element to get the children of. </param>
		/// <returns> The list of children for the element. </returns>
		private static IEnumerable<IUIAutomationElement> GetChildren(Element element)
		{
			var automation = new CUIAutomationClass();
			var walker = automation.CreateTreeWalker(automation.RawViewCondition);
			var child = walker.GetFirstChildElement(element.NativeElement);

			while (child != null)
			{
				yield return child;
				child = walker.GetNextSiblingElement(child);
			}
		}

		/// <summary>
		/// Gets the clickable point for the element.
		/// </summary>
		/// <param name="x"> Optional X offset when calculating. </param>
		/// <param name="y"> Optional Y offset when calculating. </param>
		/// <returns> The clickable point for the element. </returns>
		private Point GetClickablePoint(int x = 0, int y = 0)
		{
			tagPOINT point;
			if (NativeElement.GetClickablePoint(out point) == 1)
			{
				return new Point(point.x + x, point.y + y);
			}

			var location = BoundingRectangle;
			var size = Size;
			return new Point(location.X + (size.Width / 2) + x, location.Y + (Size.Height / 2) + y);
		}

		/// <summary>
		/// Updates the children for the provided element.
		/// </summary>
		/// <param name="element"> The element to update. </param>
		private static void UpdateChildren(Element element)
		{
			element.Children.Clear();
			GetChildren(element).ForEach(x => element.Children.Add(Create(x, element.Application, element)));
			element.Children.ForEach(x => x.UpdateChildren());
		}

		/// <summary>
		/// Update the parent for the provided element.
		/// </summary>
		/// <param name="element"> The element to update. </param>
		private static void UpdateParent(Element element)
		{
			var parent = element?.NativeElement.GetCurrentParent();
			if (parent == null || parent.CurrentProcessId != element.NativeElement.CurrentProcessId)
			{
				return;
			}

			element.Parent = new Element(parent, element.Application, null);
			UpdateParent(element.Parent);
		}

		#endregion

		#region Indexers

		/// <summary>
		/// Get a child using a provided key.
		/// </summary>
		/// <param name="key"> The key of the child. </param>
		/// <returns> The child if found or null if otherwise. </returns>
		public Element this[string key] => Children[key];

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the children are updated.
		/// </summary>
		public event Action ChildrenUpdated;

		#endregion
	}
}