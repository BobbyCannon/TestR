#region References

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using TestR.Desktop.Automation;
using TestR.Desktop.Automation.Patterns;
using TestR.Exceptions;
using TestR.Extensions;
using TestR.Native;
using Utility = TestR.Helpers.Utility;

#endregion

namespace TestR.Desktop
{
	/// <summary>
	/// Base element for desktop automation.
	/// </summary>
	public class Element : IElementParent
	{
		#region Constructors

		/// <summary>
		/// Creates an instance of an element.
		/// </summary>
		/// <param name="element"> The automation element for this element. </param>
		/// <param name="parent"> The parent element for this element. </param>
		public Element(AutomationElement element, IElementParent parent)
		{
			Automation = element;
			Children = new ElementCollection<Element>(this);
			Parent = parent;
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
					builder.Insert(0, new []{ element.Id, element.Name, " "}.FirstValue());
					element = element.Parent as Element;
				} while (element != null);
				return builder.ToString();
			}
		}

		/// <summary>
		/// Gets or sets the automation element for this element.
		/// </summary>
		public AutomationElement Automation { get; private set; }

		/// <summary>
		/// Gets the children for this element.
		/// </summary>
		public ElementCollection<Element> Children { get; private set; }

		/// <summary>
		/// Gets a value that indicates whether the element is enabled.
		/// </summary>
		public bool Enabled => Automation.Current.IsEnabled;

		/// <summary>
		/// Gets the height of the element.
		/// </summary>
		public double Height
		{
			get
			{
				var area = Automation.Current.BoundingRectangle;
				return area.Bottom - area.Top;
			}
		}

		/// <summary>
		/// Gets the ID of this element.
		/// </summary>
		public string Id => Automation.Current.AutomationId;

		/// <summary>
		/// Gets a value that indicates whether the element can be use by the keyboard.
		/// </summary>
		public bool KeyboardFocusable => Automation.Current.IsKeyboardFocusable;

		/// <summary>
		/// Gets the location of the element.
		/// </summary>
		public Rectangle Location => new Rectangle(Automation.Current.BoundingRectangle.Location.ToPoint(), Automation.Current.BoundingRectangle.Size.ToSize());

		/// <summary>
		/// Gets the name of this element.
		/// </summary>
		public string Name => Automation.Current.Name;

		/// <summary>
		/// The parent element of this element.
		/// </summary>
		public IElementParent Parent { get; private set; }

		/// <summary>
		/// Gets or sets the time out for delay request.
		/// </summary>
		public TimeSpan Timeout => Parent.Timeout;

		/// <summary>
		/// Gets the type ID of this element.
		/// </summary>
		public int TypeId => Automation.Current.ControlType.Id;

		/// <summary>
		/// Gets the name of the control type.
		/// </summary>
		public string TypeName => Automation.Current.LocalizedControlType;

		/// <summary>
		/// Gets a value that indicates whether the element is visible.
		/// </summary>
		public bool Visible => !Automation.Current.IsOffscreen;

		/// <summary>
		/// Gets the width of the element.
		/// </summary>
		public double Width
		{
			get
			{
				var area = Automation.Current.BoundingRectangle;
				return area.Right - area.Left;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Performs mouse click at the center of the element.
		/// </summary>
		/// <param name="x"> Optional X offset when clicking. </param>
		/// <param name="y"> Optional Y offset when clicking. </param>
		public virtual void Click(int x = 0, int y = 0)
		{
			var point = GetClickablePoint(x, y);
			Mouse.LeftClick(point);
		}

		/// <summary>
		/// Temporary string to debug base controls.
		/// </summary>
		/// <returns> </returns>
		public string DebugString()
		{
			var items = new Dictionary<string, string>
			{
				{ "Id", Id },
				{ "Name", Name },
				//{ "Handle", Automation.Current.NativeWindowHandle.ToString() },
				//{ "Enabled", Enabled },
				//{ "ParentId", Parent == null ? string.Empty : Parent.Id },
				//{ "TypeId", TypeId },
				{ "TypeName", TypeName },
				//{ "Type", GetType().Name },
				{ "X", Automation.Current.BoundingRectangle.X.ToString() },
				{ "Y", Automation.Current.BoundingRectangle.Y.ToString() }
			};

			return string.Join(" : ", items.Where(x => !string.IsNullOrWhiteSpace(x.Value)).Select(x => x.Key + " - " + x.Value));
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
		/// Set focus on the element.
		/// </summary>
		public void Focus()
		{
			Automation.SetFocus();
		}

		public static Element FromCursor()
		{
			var point = Mouse.GetCursorPosition();
			var element = new Element(AutomationElement.FromPoint(new System.Windows.Point(point.X, point.Y)), null);

			if (!string.IsNullOrWhiteSpace(element.Id) || !string.IsNullOrWhiteSpace(element.Name))
			{
				return element;
			}

			// if element has invalid id then try and go up until we get an id?
			var parent = new Element(TreeWalker.RawViewWalker.GetParent(element.Automation), null);
			while (parent.Automation != null && string.IsNullOrWhiteSpace(parent.Id) && string.IsNullOrWhiteSpace(parent.Name))
			{
				parent = new Element(TreeWalker.RawViewWalker.GetParent(parent.Automation), null);
			}

			return parent.Automation != null ? parent : element;
		}

		/// <summary>
		/// Get a child of a certain type and key.
		/// </summary>
		/// <typeparam name="T"> The type of the child. </typeparam>
		/// <param name="key"> The key of the child. </param>
		/// <param name="includeDescendance"> Flag to determine to include descendance or not. </param>
		/// <returns> The child if found or null if otherwise. </returns>
		public T GetChild<T>(string key, bool includeDescendance = true)
			where T : Element, IElementParent
		{
			return (T) Children.GetChild(key, includeDescendance);
		}

		public static Element GetFirstParentWithId(Element element)
		{
			return !string.IsNullOrEmpty(element.Id) ? element : GetFirstParentWithId(element.Parent as Element);
		}

		public static Element GetFocusedElement()
		{
			return new Element(AutomationElement.FocusedElement, null);
		}

		public string GetText()
		{
			object pattern;
			if (Automation.TryGetCurrentPattern(ValuePattern.Pattern, out pattern))
			{
				// Control supports the ValuePattern pattern so we can use the SetValue method to insert content. 
				return ((ValuePattern) pattern).Current.Value;
			}

			throw new NotSupportedException();
		}

		/// <summary>
		/// Moves mouse cursor to the center of the element.
		/// </summary>
		public void MoveMouseTo()
		{
			var point = GetClickablePoint();
			Mouse.MoveTo(point);
		}

		/// <summary>
		/// Performs mouse right click at the center of the element.
		/// </summary>
		/// <param name="x"> Optional X offset when clicking. </param>
		/// <param name="y"> Optional Y offset when clicking. </param>
		public virtual void RightClick(int x = 0, int y = 0)
		{
			var point = GetClickablePoint(x, y);
			Mouse.RightClick(point);
		}

		/// <summary>
		/// Sets the text value of the element.
		/// </summary>
		/// <param name="value"> The text to set the element to. </param>
		public void SetText(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value), "String parameter must not be null.");
			}

			if (!Enabled)
			{
				throw new InvalidOperationException("The element is not enabled.");
			}

			object pattern;
			if (Automation.TryGetCurrentPattern(ValuePattern.Pattern, out pattern))
			{
				// Control supports the ValuePattern pattern so we can use the SetValue method to insert content. 
				((ValuePattern) pattern).SetValue(value);
				return;
			}

			if (!KeyboardFocusable)
			{
				throw new NotKeyboardFocusableException("The element is read-only.");
			}

			// Set focus for input functionality and begin.
			Automation.SetFocus();

			// Pause before sending keyboard input.
			Thread.Sleep(100);
			Keyboard.TypeText(value);
		}

		public string ToDetailString()
		{
			var items = new Dictionary<string, string>
			{
				{ "Id", Id },
				{ "Name", Name },
				{ "TypeId", TypeId.ToString() },
				{ "TypeName", TypeName },
				{ "Type", GetType().Name },
				{ "X", Automation.Current.BoundingRectangle.X.ToString() },
				{ "Y", Automation.Current.BoundingRectangle.Y.ToString() },
				{ "Height", Automation.Current.BoundingRectangle.Height.ToString() },
				{ "Weight", Automation.Current.BoundingRectangle.Width.ToString() }
			};

			return string.Join(", ", items.Select(x => x.Key + " - " + x.Value));
		}

		/// <summary>
		/// Focus the element then type the text via the keyboard.
		/// </summary>
		/// <param name="value"> The value to type. </param>
		public void TypeText(string value)
		{
			Focus();
			Keyboard.TypeText(value);
		}

		/// <summary>
		/// Update the children for this element.
		/// </summary>
		public void UpdateChildren()
		{
			UpdateChildren(this);
			OnChildrenUpdated();
		}

		/// <summary>
		/// Update the parents for this element.
		/// </summary>
		public void UpdateParents()
		{
			UpdateParent(this);
		}

		/// <summary>
		/// Wait for the child to be available then return it.
		/// </summary>
		/// <param name="id"> The ID of the child to wait for. </param>
		/// <param name="includeDescendance"> Flag to determine to include descendance or not. </param>
		/// <returns> The child element for the ID. </returns>
		public Element WaitForChild(string id, bool includeDescendance = true)
		{
			return WaitForChild<Element>(id, includeDescendance);
		}

		/// <summary>
		/// Wait for the child to be available then return it.
		/// </summary>
		/// <param name="id"> The ID of the child to wait for. </param>
		/// <param name="includeDescendance"> Flag to determine to include descendance or not. </param>
		/// <returns> The child element for the ID. </returns>
		public T WaitForChild<T>(string id, bool includeDescendance = true)
			where T : Element
		{
			Element response = null;

			Utility.Wait(() =>
			{
				response = GetChild<T>(id, includeDescendance);
				if (response != null)
				{
					return true;
				}

				UpdateChildren();
				return false;
			}, Timeout.TotalMilliseconds, 100);

			if (response == null)
			{
				throw new ArgumentException("Failed to find the child by ID.");
			}

			return (T) response;
		}

		/// <summary>
		/// Gets the provided pattern from the automation element.
		/// </summary>
		/// <typeparam name="T"> The pattern to get. </typeparam>
		/// <returns> The pattern requested. </returns>
		protected T GetPattern<T>(int id)
			where T : BasePattern
		{
			var patterns = Automation.GetSupportedPatterns();
			var pattern = patterns.FirstOrDefault(x => x.Id == id);
			return pattern == null ? null : (T) Automation.GetCurrentPattern(pattern);
		}

		/// <summary>
		/// Handles the ChildrenUpdated event.
		/// </summary>
		protected void OnChildrenUpdated()
		{
			var handler = ChildrenUpdated;
			if (handler != null)
			{
				handler();
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
			System.Windows.Point point;
			if (Automation.TryGetClickablePoint(out point))
			{
				return new Point((int) (point.X + x), (int) (point.Y + y));
			}

			var tagRect = Automation.Current.BoundingRectangle;
			var width = tagRect.Right - tagRect.Left;
			var height = tagRect.Bottom - tagRect.Top;

			return new Point((int) (tagRect.Left + (width / 2) + x), (int) (tagRect.Top + (height / 2) + y));
		}

		/// <summary>
		/// Updates the children for the provided element.
		/// </summary>
		/// <param name="element"> The element to update. </param>
		private static void UpdateChildren(Element element)
		{
			element.Children.Clear();
			ElementWalker.GetChildren(element.Automation).ForEach(x => element.Children.Add(x));
			element.Children.ForEach(x => x.UpdateChildren());
		}

		/// <summary>
		/// Update the parent for the provided element.
		/// </summary>
		/// <param name="element"> The element to update. </param>
		private static void UpdateParent(Element element)
		{
			if (element == null)
			{
				return;
			}

			var parent = TreeWalker.RawViewWalker.GetParent(element.Automation);
			if (parent == null || parent.Current.ProcessId != element.Automation.Current.ProcessId)
			{
				return;
			}

			element.Parent = new Element(parent, null);
			UpdateParent((Element) element.Parent);
		}

		#endregion

		#region Indexers

		/// <summary>
		/// Get a child using a provided key.
		/// </summary>
		/// <param name="key"> The key of the child. </param>
		/// <returns> The child if found or null if otherwise. </returns>
		public Element this[string key]
		{
			get { return Children[key]; }
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the children are updated.
		/// </summary>
		public event Action ChildrenUpdated;

		#endregion
	}
}