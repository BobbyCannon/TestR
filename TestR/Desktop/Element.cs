#region References

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
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
		public bool Enabled
		{
			get { return Automation.Current.IsEnabled; }
		}

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
		public string Id
		{
			get { return Automation.Current.AutomationId; }
		}

		/// <summary>
		/// Gets a value that indicates whether the element can be use by the keyboard.
		/// </summary>
		public bool KeyboardFocusable
		{
			get { return Automation.Current.IsKeyboardFocusable; }
		}

		/// <summary>
		/// Gets the name of this element.
		/// </summary>
		public string Name
		{
			get { return Automation.Current.Name; }
		}

		/// <summary>
		/// The parent element of this element.
		/// </summary>
		public IElementParent Parent { get; private set; }

		/// <summary>
		/// Gets or sets the time out for delay request.
		/// </summary>
		public TimeSpan Timeout
		{
			get { return Parent.Timeout; }
		}

		/// <summary>
		/// Gets the type ID of this element.
		/// </summary>
		public int TypeId
		{
			get { return Automation.Current.ControlType.Id; }
		}

		/// <summary>
		/// Gets the name of the control type.
		/// </summary>
		public string TypeName
		{
			get { return Automation.Current.LocalizedControlType; }
		}

		/// <summary>
		/// Gets a value that indicates whether the element is visible.
		/// </summary>
		public bool Visible
		{
			get { return !Automation.Current.IsOffscreen; }
		}

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
			if (!Visible)
			{
				throw new Exception("Could not click item because it's not visible.");
			}

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
		/// Get a child of a certain type and key.
		/// </summary>
		/// <typeparam name="T"> The type of the child. </typeparam>
		/// <param name="key"> The key of the child. </param>
		/// <param name="includeDescendance"> Flag to determine to include descendance or not. </param>
		/// <returns> The child if found or null if otherwise. </returns>
		public T GetChild<T>(string key, bool includeDescendance = true)
			where T : Element, IElementParent
		{
			T child = null;

			if (Children.ContainsKey(key))
			{
				child = Children[key] as T;
			}

			if (!includeDescendance)
			{
				return child;
			}

			return child ?? Children.Select(x => x.GetChild<T>(key, true)).FirstOrDefault(x => x != null);
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
			if (!Visible)
			{
				throw new Exception("Could not click item because it's not visible.");
			}

			var point = GetClickablePoint(0, 0);
			Mouse.MoveTo(point);
		}

		/// <summary>
		/// Sets the text value of the element.
		/// </summary>
		/// <param name="value"> The text to set the element to. </param>
		public void SetText(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value", "String parameter must not be null.");
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

			// Control does not support the ValuePattern pattern so use keyboard input to insert content. 
			// 
			// NOTE: Elements that support TextPattern do not support ValuePattern and TextPattern 
			// does not support setting the text of multi-line edit or document controls. For this reason, 
			// text input must be simulated using one of the following methods. 

			// Delete existing content in the control and insert new content.
			SendKeys.SendWait("^{HOME}"); // Move to start of control
			SendKeys.SendWait("^+{END}"); // Select everything
			SendKeys.SendWait("{DEL}"); // Delete selection

			value = value.Replace("+", "{add}");

			SendKeys.SendWait(value);
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
		/// <param name="element"> </param>
		private static void UpdateChildren(Element element)
		{
			element.Children.Clear();
			ElementWalker.GetChildren(element.Automation).ForEach(x => element.Children.Add(x));
			element.Children.ForEach(x => x.UpdateChildren());
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