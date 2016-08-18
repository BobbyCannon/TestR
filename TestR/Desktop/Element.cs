#region References

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using TestR.Desktop.Elements;
using TestR.Desktop.Pattern;
using TestR.Exceptions;
using TestR.Extensions;
using TestR.Helpers;
using TestR.Native;
using UIAutomationClient;
using Image = TestR.Desktop.Elements.Image;

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
		/// Static constructor.
		/// </summary>
		static Element()
		{
			ExcludedProperties = new[] { "Parent", "Children", "NativeElement", "Item" };
		}

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

		#endregion

		#region Properties

		/// <summary>
		/// Gets the application this element is hosted in.
		/// </summary>
		private Application Application { get; }

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
					builder.Insert(0, new[] { element.Id, element.Name, " " }.FirstValue() + ",");
					element = element.Parent;
				} while (element != null);

				builder.Remove(builder.Length - 1, 1);
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
		public virtual Point Location
		{
			get
			{
				var rectangle = BoundingRectangle;
				return new Point(rectangle.X, rectangle.Y);
			}
		}

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
		/// The all parent element of this element.
		/// </summary>
		public IEnumerable<Element> Parents
		{
			get
			{
				var parent = Parent;
				var response = new List<Element>();

				while (parent != null)
				{
					response.Add(parent);
					parent = parent.Parent;
				}

				return response;
			}
		}

		/// <summary>
		/// Gets the current process ID  of the element.
		/// </summary>
		public int ProcessId => NativeElement.CurrentProcessId;

		/// <summary>
		/// Gets the size of the element.
		/// </summary>
		public virtual Size Size
		{
			get
			{
				var rectangle = BoundingRectangle;
				return new Size(rectangle.Width, rectangle.Height);
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
				Point point;
				var clickable = TryGetClickablePoint(out point) && (point.Y != 0 && point.Y != 0);
				var focused = Focused || Children.Any(x => x.Focused);
				return NativeElement.CurrentIsOffscreen == 0 && (clickable || focused);
			}
		}

		/// <summary>
		/// Gets the width of the element;
		/// </summary>
		public int Width => Size.Width;

		#endregion

		#region Indexers

		/// <summary>
		/// Get a child using a provided key.
		/// </summary>
		/// <param name="id"> The ID of the child. </param>
		/// <returns> The child if found or null if otherwise. </returns>
		public Element this[string id] => Get(id, false);

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
			Thread.Sleep(100);
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
		/// Gets the element that is currently focused.
		/// </summary>
		/// <returns> The element if found or null if not found. </returns>
		public static Element FromFocusElement()
		{
			try
			{
				var automation = new CUIAutomationClass();
				var element = automation.GetFocusedElement();
				return element == null ? null : new Element(element, null, null);
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Gets the element that is currently at the point.
		/// </summary>
		/// <param name="point"> The point to try and detect at element at. </param>
		/// <returns> The element if found or null if not found. </returns>
		public static Element FromPoint(Point point)
		{
			try
			{
				var automation = new CUIAutomationClass();
				var element = automation.ElementFromPoint(new tagPOINT { x = point.X, y = point.Y });
				return element == null ? null : new Element(element, null, null);
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="id"> An ID of the element to get. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the ID. </returns>
		public Element Get(string id, bool recursive = true, bool wait = true)
		{
			return Get<Element>(id, recursive, wait);
		}

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="id"> An ID of the element to get. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the ID. </returns>
		public T Get<T>(string id, bool recursive = true, bool wait = true) where T : Element
		{
			return Get<T>(x => x.ApplicationId == id || x.Id == id || x.Name == id, recursive, wait);
		}

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="condition"> A function to test each element for a condition. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the condition. </returns>
		public Element Get(Func<Element, bool> condition, bool recursive = true, bool wait = true)
		{
			return Get<Element>(condition, recursive, wait);
		}

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="condition"> A function to test each element for a condition. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the condition. </returns>
		public T Get<T>(Func<T, bool> condition, bool recursive = true, bool wait = true) where T : Element
		{
			T response = null;

			Utility.Wait(() =>
			{
				try
				{
					response = Children.Get(condition, recursive);
					if (response != null || !wait)
					{
						return true;
					}

					UpdateChildren();
					return false;
				}
				catch (Exception)
				{
					return !wait;
				}
			}, Application?.Timeout.TotalMilliseconds ?? Application.DefaultTimeout);

			return response;
		}

		/// <summary>
		/// Gets the first parent that has an ID.
		/// </summary>
		/// <param name="element"> The element to iterate. </param>
		/// <returns> The parent if found or null otherwise. </returns>
		public static Element GetFirstParentWithId(Element element)
		{
			if (element == null)
			{
				return null;
			}

			return !string.IsNullOrEmpty(element.Id) ? element : GetFirstParentWithId(element.Parent);
		}

		/// <summary>
		/// Gets the text value of the element.
		/// </summary>
		/// <returns> The value of the element. </returns>
		public string GetText()
		{
			return ValuePattern.Create(this)?.Value ?? string.Empty;
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

			var builder = new StringBuilder();

			foreach (var property in properties)
			{
				try
				{
					var value = property.GetValue(this)?.ToString();
					if (value == null)
					{
						continue;
					}

					builder.AppendLine(property.Name + " - " + value);
				}
				catch (Exception ex)
				{
					builder.AppendLine(property.Name + " - " + ex.Message);
				}
			}

			builder.AppendLine("GetText() - " + GetText());

			return builder.ToString();
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
			return this;
		}

		/// <summary>
		/// Update the parent for the provided element.
		/// </summary>
		public Element UpdateParent()
		{
			var parent = NativeElement.GetCachedParent() ?? NativeElement.GetCurrentParent();
			if (parent == null || parent.CurrentProcessId != NativeElement.CurrentProcessId)
			{
				Parent = null;
				return this;
			}

			Parent = new Element(parent, Application, null);
			Debug.WriteLine("P: {0},{1},{2},{3}",
				Parent.ApplicationId,
				parent.CurrentName,
				parent.CurrentAutomationId,
				parent.CurrentFrameworkId);

			return this;
		}

		/// <summary>
		/// Update the parents for this element.
		/// </summary>
		public Element UpdateParents()
		{
			UpdateParent();
			Parent?.UpdateParents();
			return this;
		}

		/// <summary>
		/// Creates an element from the automation element.
		/// </summary>
		/// <param name="element"> The element to create. </param>
		/// <param name="application"> The application parent for this element. </param>
		/// <param name="parent"> The parent of the element to create. </param>
		internal static Element Create(IUIAutomationElement element, Application application, Element parent)
		{
			var itemType = element.CurrentControlType;

			switch (itemType)
			{
				case UIA_ControlTypeIds.UIA_ButtonControlTypeId:
					return new Button(element, application, parent);

				case UIA_ControlTypeIds.UIA_CalendarControlTypeId:
					return new Calendar(element, application, parent);

				case UIA_ControlTypeIds.UIA_CheckBoxControlTypeId:
					return new CheckBox(element, application, parent);

				case UIA_ControlTypeIds.UIA_ComboBoxControlTypeId:
					return new ComboBox(element, application, parent);

				case UIA_ControlTypeIds.UIA_CustomControlTypeId:
					return new Custom(element, application, parent);

				case UIA_ControlTypeIds.UIA_DataGridControlTypeId:
					return new DataGrid(element, application, parent);

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

				case UIA_ControlTypeIds.UIA_ImageControlTypeId:
					return new Image(element, application, parent);

				case UIA_ControlTypeIds.UIA_ListControlTypeId:
					return new List(element, application, parent);

				case UIA_ControlTypeIds.UIA_ListItemControlTypeId:
					return new ListItem(element, application, parent);

				case UIA_ControlTypeIds.UIA_MenuControlTypeId:
					return new Menu(element, application, parent);

				case UIA_ControlTypeIds.UIA_MenuBarControlTypeId:
					return new MenuBar(element, application, parent);

				case UIA_ControlTypeIds.UIA_MenuItemControlTypeId:
					return new MenuItem(element, application, parent);

				case UIA_ControlTypeIds.UIA_PaneControlTypeId:
					return new Pane(element, application, parent);

				case UIA_ControlTypeIds.UIA_ProgressBarControlTypeId:
					return new ProgressBar(element, application, parent);

				case UIA_ControlTypeIds.UIA_RadioButtonControlTypeId:
					return new RadioButton(element, application, parent);

				case UIA_ControlTypeIds.UIA_SeparatorControlTypeId:
					return new Separator(element, application, parent);

				case UIA_ControlTypeIds.UIA_ScrollBarControlTypeId:
					return new ScrollBar(element, application, parent);

				case UIA_ControlTypeIds.UIA_SemanticZoomControlTypeId:
					return new SemanticZoom(element, application, parent);

				case UIA_ControlTypeIds.UIA_SliderControlTypeId:
					return new Slider(element, application, parent);

				case UIA_ControlTypeIds.UIA_SpinnerControlTypeId:
					return new Spinner(element, application, parent);

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

				case UIA_ControlTypeIds.UIA_ToolTipControlTypeId:
					return new ToolTip(element, application, parent);

				case UIA_ControlTypeIds.UIA_TreeControlTypeId:
					return new Tree(element, application, parent);

				case UIA_ControlTypeIds.UIA_TreeItemControlTypeId:
					return new TreeItem(element, application, parent);

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
			var automation = new CUIAutomation8Class();
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
		/// Try to get a clickable point for the element.
		/// </summary>
		/// <param name="point"> The point value if call was successful. </param>
		/// <param name="x"> Optional X offset when calculating. </param>
		/// <param name="y"> Optional Y offset when calculating. </param>
		/// <returns> The clickable point for the element. </returns>
		private bool TryGetClickablePoint(out Point point, int x = 0, int y = 0)
		{
			try
			{
				tagPOINT point2;
				if (NativeElement.GetClickablePoint(out point2) == 1)
				{
					point = new Point(point2.x + x, point2.y + y);
					return true;
				}

				point = new Point(0, 0);
			}
			catch (Exception)
			{
				point = new Point(0, 0);
			}

			return false;
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

		#endregion
	}
}