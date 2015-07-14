#region References

using Newtonsoft.Json.Linq;

#endregion

namespace TestR.Web.Elements
{
	/// <summary>
	/// Represents a browser Applet element.
	/// </summary>
	public class Applet : Element
	{
		#region Constructors

		/// <summary>
		/// Initializes an instance of a browser element.
		/// </summary>
		/// <param name="element"> The browser element this is for. </param>
		/// <param name="browser"> The browser this element is associated with. </param>
		/// <param name="collection"> The collection this element is associated with. </param>
		public Applet(JToken element, Browser browser, ElementCollection collection)
			: base(element, browser, collection)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the code attribute. This should be a URL to the applet class file.
		/// </summary>
		/// <remarks>
		/// Specifies the file name of a Java applet.
		/// </remarks>
		public string Code
		{
			get { return this["code"]; }
			set { this["code"] = value; }
		}

		/// <summary>
		/// Gets or sets the height attribute.
		/// </summary>
		public string Height
		{
			get { return this["height"]; }
			set { this["height"] = value; }
		}

		/// <summary>
		/// Gets or sets the object attribute.
		/// </summary>
		/// <remarks>
		/// Specifies a reference to a serialized representation of an applet.
		/// </remarks>
		public string Object
		{
			get { return this["object"]; }
			set { this["object"] = value; }
		}

		/// <summary>
		/// Gets or sets the width attribute.
		/// </summary>
		public string Width
		{
			get { return this["width"]; }
			set { this["width"] = value; }
		}

		#endregion
	}
}