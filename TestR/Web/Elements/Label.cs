#region References

using Newtonsoft.Json.Linq;

#endregion

namespace TestR.Web.Elements
{
	/// <summary>
	/// Represents a browser label element.
	/// </summary>
	public class Label : Element
	{
		#region Constructors

		/// <summary>
		/// Initializes an instance of a browser element.
		/// </summary>
		/// <param name="element"> The browser element this is for. </param>
		/// <param name="browser"> The browser this element is associated with. </param>
		/// <param name="collection"> The collection this element is associated with. </param>
		public Label(JToken element, Browser browser, ElementCollection collection)
			: base(element, browser, collection)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the element id (for) attribute.
		/// </summary>
		/// <remarks>
		/// Specifies which form element a label is bound to.
		/// </remarks>
		public string For
		{
			get { return this["for"]; }
			set { this["for"] = value; }
		}

		/// <summary>
		/// Gets or sets the form id (form) attribute.
		/// </summary>
		/// <remarks>
		/// HTML5: Specifies one or more forms the label belongs to.
		/// </remarks>
		public string Form
		{
			get { return this["form"]; }
			set { this["form"] = value; }
		}

		#endregion
	}
}