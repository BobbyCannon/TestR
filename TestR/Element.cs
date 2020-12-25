#region References

using System;
using System.Drawing;
using System.Text;
using TestR.Desktop;
using TestR.Internal;

#endregion

namespace TestR
{
	/// <summary>
	/// Represents an element.
	/// </summary>
	public abstract class Element : ElementHost
	{
		#region Constructors

		/// <summary>
		/// Instantiates an instance of an element.
		/// </summary>
		/// <param name="application"> The application this element exists in. </param>
		/// <param name="parent"> The parent of this element. </param>
		protected Element(Application application, ElementHost parent)
			: base(application, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the coordinates of the rectangle that completely encloses the element.
		/// </summary>
		public Rectangle BoundingRectangle => new Rectangle(Location.X, Location.Y, Width, Height);

		/// <summary>
		/// Gets a value that indicates whether the element is enabled.
		/// </summary>
		public abstract bool Enabled { get; }

		/// <summary>
		/// Gets a value that indicates whether the element is focused.
		/// </summary>
		public abstract bool Focused { get; }

		/// <summary>
		/// Gets the full id of the element which include all parent IDs prefixed to this element ID.
		/// </summary>
		/// <summary>
		/// Gets the ID of this element in the element host. Includes full host namespace. Ex. GrandParent,Parent,Element
		/// </summary>
		public string FullId
		{
			get
			{
				var builder = new StringBuilder();
				var element = (ElementHost) this;

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
		/// Gets the width of the element.
		/// </summary>
		public abstract int Height { get; }

		/// <summary>
		/// Gets or sets an attribute or property by name.
		/// </summary>
		/// <param name="id"> The ID of the attribute or property to read. </param>
		public abstract string this[string id] { get; set; }

		/// <summary>
		/// Gets the location of the element.
		/// </summary>
		public abstract Point Location { get; }

		/// <summary>
		/// Gets the size of the element.
		/// </summary>
		public Size Size => new Size(Width, Height);

		/// <summary>
		/// Gets the width of the element.
		/// </summary>
		public abstract int Width { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Performs mouse click at the center of the element.
		/// </summary>
		/// <param name="x"> Optional X offset when clicking. </param>
		/// <param name="y"> Optional Y offset when clicking. </param>
		/// <param name="refresh"> Optional value to refresh the element's children. </param>
		public abstract Element Click(int x = 0, int y = 0, bool refresh = true);

		/// <summary>
		/// Set focus on the element.
		/// </summary>
		public abstract Element Focus();

		/// <summary>
		/// Get next sibling. Returns null if no sibling is found.
		/// </summary>
		public Element GetNextSibling()
		{
			var index = Parent?.Children.IndexOf(this);
			if (index == null || index + 1 >= Parent?.Children.Count)
			{
				return null;
			}

			return Parent.Children[index.Value + 1];
		}

		/// <summary>
		/// Get previous sibling. Returns null if no sibling is found.
		/// </summary>
		public Element GetPreviousSibling()
		{
			var index = Parent?.Children.IndexOf(this);
			if (index == null || index < 1)
			{
				return null;
			}

			return Parent.Children[index.Value - 1];
		}

		/// <summary>
		/// Performs mouse left click at the center of the element.
		/// </summary>
		/// <param name="x"> Optional X offset when clicking. </param>
		/// <param name="y"> Optional Y offset when clicking. </param>
		public abstract Element LeftClick(int x = 0, int y = 0);

		/// <summary>
		/// Performs mouse middle click at the center of the element.
		/// </summary>
		/// <param name="x"> Optional X offset when clicking. </param>
		/// <param name="y"> Optional Y offset when clicking. </param>
		public abstract Element MiddleClick(int x = 0, int y = 0);

		/// <summary>
		/// Moves mouse cursor to the center of the element.
		/// </summary>
		/// <param name="x"> Optional X offset when clicking. </param>
		/// <param name="y"> Optional Y offset when clicking. </param>
		public abstract Element MoveMouseTo(int x = 0, int y = 0);

		/// <summary>
		/// Performs mouse right click at the center of the element.
		/// </summary>
		/// <param name="x"> Optional X offset when clicking. </param>
		/// <param name="y"> Optional Y offset when clicking. </param>
		public abstract Element RightClick(int x = 0, int y = 0);

		/// <summary>
		/// Provides a string of details for the element.
		/// </summary>
		/// <returns> The string of element details. </returns>
		public abstract string ToDetailString();

		/// <summary>
		/// Focus the element then type the text via the keyboard.
		/// </summary>
		/// <param name="value"> The value to type. </param>
		public virtual Element TypeText(string value)
		{
			Application.BringToFront();
			Focus();
			Input.Keyboard.TypeText(value);
			return this;
		}

		/// <summary>
		/// Focus the element then type the text via the keyboard.
		/// </summary>
		/// <param name="value"> The value to type. </param>
		/// <param name="keys"> An optional set of keyboard keys to press after typing the provided text. </param>
		public virtual Element TypeText(string value, params KeyboardKeys[] keys)
		{
			Application.BringToFront();
			Focus();
			Input.Keyboard.TypeText(value, TimeSpan.Zero, keys);
			return this;
		}

		/// <summary>
		/// Focus the element then type the text via the keyboard.
		/// </summary>
		/// <param name="value"> The value to type. </param>
		/// <param name="delay"> An optional delay before sending optional keys. </param>
		/// <param name="keys"> An optional set of keyboard keys to press after typing the provided text. </param>
		public virtual Element TypeText(string value, TimeSpan delay, params KeyboardKeys[] keys)
		{
			Application.BringToFront();
			Focus();
			Input.Keyboard.TypeText(value, delay, keys);
			return this;
		}

		#endregion
	}
}