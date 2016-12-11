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
using TestR.Native;
using UIAutomationClient;
using Image = TestR.Desktop.Elements.Image;

#endregion

namespace TestR.Desktop
{
	/// <summary>
	/// Base element for desktop automation.
	/// </summary>
	public class DesktopElement : Element
	{
		#region Fields

		/// <summary>
		/// Properties that should not be included in <see cref="ToDetailString" />.
		/// </summary>
		private static readonly string[] _excludedProperties;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of a desktop element.
		/// </summary>
		/// <param name="element"> The automation element for this element. </param>
		/// <param name="application"> The application parent for this element. </param>
		/// <param name="parent"> The parent element for this element. </param>
		protected DesktopElement(IUIAutomationElement element, Application application, ElementHost parent)
			: base(application, parent)
		{
			NativeElement = element;
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static DesktopElement()
		{
			_excludedProperties = new[] { nameof(Parent), nameof(Children), nameof(NativeElement), "Item", nameof(FocusedElement) };
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a value that indicates whether the element is enabled.
		/// </summary>
		public override bool Enabled => NativeElement.CurrentIsEnabled == 1;

		/// <inheritdoc />
		public override bool Focused => NativeElement.CurrentHasKeyboardFocus == 1;

		/// <inheritdoc />
		public override Element FocusedElement => FirstOrDefault(x => x.Focused);

		/// <inheritdoc />
		public override int Height
		{
			get
			{
				var rectangle = NativeElement.CurrentBoundingRectangle;
				return rectangle.bottom - rectangle.top;
			}
		}

		/// <inheritdoc />
		public override string Id => NativeElement.CurrentAutomationId;

		/// <inheritdoc />
		public override string this[string id]
		{
			get { return NativeElement.GetCachedPropertyValue(int.Parse(id)).ToString(); }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets a value that indicates whether the element can be use by the keyboard.
		/// </summary>
		public bool KeyboardFocusable => (NativeElement.CurrentIsKeyboardFocusable == 1) && Enabled;

		/// <inheritdoc />
		public override Point Location
		{
			get
			{
				var rectangle = NativeElement.CurrentBoundingRectangle;
				return new Point(rectangle.left, rectangle.top);
			}
		}

		/// <inheritdoc />
		public override string Name => NativeElement.CurrentName;

		/// <summary>
		/// Gets the native element for the desktop element.
		/// </summary>
		public IUIAutomationElement NativeElement { get; }

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
				var clickable = TryGetClickablePoint(out point) && (point.Y != 0) && (point.Y != 0);
				var focused = Focused || Children.Any(x => x.Focused);
				return (NativeElement.CurrentIsOffscreen == 0) && (clickable || focused);
			}
		}

		/// <inheritdoc />
		public override int Width
		{
			get
			{
				var rectangle = NativeElement.CurrentBoundingRectangle;
				return rectangle.right - rectangle.left;
			}
		}

		#endregion

		#region Methods

		/// <inheritdoc />
		public override Element CaptureSnippet(string filePath)
		{
			var result = new Bitmap(Width, Height);

			using (var graphics = Graphics.FromImage(result))
			{
				graphics.CopyFromScreen(Location, Point.Empty, Size);
			}

			result.Save(filePath, ImageFormat.Png);
			return this;
		}

		/// <inheritdoc />
		public override Element Click(int x = 0, int y = 0)
		{
			var point = GetClickablePoint(x, y);
			Mouse.LeftClick(point);
			Thread.Sleep(100);
			return this;
		}

		/// <inheritdoc />
		public override Element Focus()
		{
			//NativeElement.SetFocus();
			NativeMethods.SetFocus(NativeElement.CurrentNativeWindowHandle);
			return this;
		}

		/// <summary>
		/// Gets the element that is currently under the cursor.
		/// </summary>
		/// <returns> The element if found or null if not found. </returns>
		public static DesktopElement FromCursor()
		{
			var point = Mouse.GetCursorPosition();
			return FromPoint(point);
		}

		/// <summary>
		/// Gets the element that is currently focused.
		/// </summary>
		/// <returns> The element if found or null if not found. </returns>
		public static DesktopElement FromFocus()
		{
			try
			{
				var automation = new CUIAutomationClass();
				var element = automation.GetFocusedElement();

				return element == null ? null : new DesktopElement(element, null, null);
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
		public static DesktopElement FromPoint(Point point)
		{
			try
			{
				var automation = new CUIAutomationClass();
				var element = automation.ElementFromPoint(new tagPOINT { x = point.X, y = point.Y });
				return element == null ? null : new DesktopElement(element, null, null);
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Gets the text value of the element.
		/// </summary>
		/// <returns> The value of the element. </returns>
		public string GetText()
		{
			return ValuePattern.Create(this)?.Value ?? string.Empty;
		}

		/// <inheritdoc />
		public override Element MiddleClick(int x = 0, int y = 0)
		{
			var point = GetClickablePoint(x, y);
			Mouse.MiddleClick(point);
			Thread.Sleep(100);
			return this;
		}

		/// <inheritdoc />
		public override Element MoveMouseTo(int x = 0, int y = 0)
		{
			var point = GetClickablePoint(x, y);
			Mouse.MoveTo(point);
			Thread.Sleep(100);
			return this;
		}

		/// <inheritdoc />
		public override ElementHost Refresh()
		{
			Children.Clear();
			var test = GetChildren(this).Select(x => Create(x, Application, this)).ToList();
			Children.AddRange(test);
			Children.ForEach(x => x.Refresh());
			return this;
		}

		/// <inheritdoc />
		public override Element RightClick(int x = 0, int y = 0)
		{
			var point = GetClickablePoint(x, y);
			Mouse.RightClick(point);
			Thread.Sleep(100);
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
				throw new Exception("The element is read-only.");
			}

			// Set focus for input functionality and begin.
			NativeElement.SetFocus();

			// Pause before sending keyboard input.
			Thread.Sleep(100);
			Keyboard.TypeText(value);
			return this;
		}

		/// <summary>
		/// Provides a string of details for the element.
		/// </summary>
		/// <returns> The string of element details. </returns>
		public override string ToDetailString()
		{
			var type = GetType();
			var properties = type.GetProperties()
				.Where(x => !_excludedProperties.Contains(x.Name))
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
		/// Update the parents for this element.
		/// </summary>
		public DesktopElement UpdateParents()
		{
			UpdateParent();
			((DesktopElement) Parent)?.UpdateParents();
			return this;
		}

		/// <inheritdoc />
		public override ElementHost WaitForComplete(int minimumDelay = 0)
		{
			Thread.Sleep(minimumDelay);
			return this;
		}

		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
		}

		/// <summary>
		/// Creates an element from the automation element.
		/// </summary>
		/// <param name="element"> The element to create. </param>
		/// <param name="application"> The application parent for this element. </param>
		/// <param name="parent"> The parent of the element to create. </param>
		internal static DesktopElement Create(IUIAutomationElement element, Application application, DesktopElement parent)
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
					return new DesktopElement(element, application, parent);
			}
		}

		/// <summary>
		/// Gets all the direct children of an element.
		/// </summary>
		/// <param name="element"> The element to get the children of. </param>
		/// <returns> The list of children for the element. </returns>
		private static IEnumerable<IUIAutomationElement> GetChildren(DesktopElement element)
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
			return new Point(location.X + size.Width / 2 + x, location.Y + Size.Height / 2 + y);
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
		/// Update the parent for the provided element.
		/// </summary>
		private DesktopElement UpdateParent()
		{
			var parent = NativeElement.GetCachedParent() ?? NativeElement.GetCurrentParent();
			if ((parent == null) || (parent.CurrentProcessId != NativeElement.CurrentProcessId))
			{
				Parent = null;
				return this;
			}

			Parent = new DesktopElement(parent, Application, null);
			Debug.WriteLine("P: {0},{1},{2},{3}",
				Parent.Id,
				parent.CurrentName,
				parent.CurrentAutomationId,
				parent.CurrentFrameworkId);

			return this;
		}

		#endregion
	}
}