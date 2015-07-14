#region References

using Newtonsoft.Json.Linq;

#endregion

namespace TestR.Web.Elements
{
	/// <summary>
	/// Represents a browser list item element.
	/// </summary>
	public class ListItem : Element
	{
		#region Constructors

		/// <summary>
		/// Initializes an instance of a browser element.
		/// </summary>
		/// <param name="element"> The browser element this is for. </param>
		/// <param name="browser"> The browser this element is associated with. </param>
		/// <param name="collection"> The collection this element is associated with. </param>
		public ListItem(JToken element, Browser browser, ElementCollection collection)
			: base(element, browser, collection)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the value attribute.
		/// </summary>
		/// <remarks>
		/// Specifies the value of a list item. The following list items will increment from that number (only for ordered lists).
		/// </remarks>
		public string Value
		{
			get { return this["value"]; }
			set { this["value"] = value; }
		}

		#endregion
	}
}