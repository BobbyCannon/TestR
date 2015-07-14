#region References

using Newtonsoft.Json.Linq;

#endregion

namespace TestR.Web.Elements
{
	/// <summary>
	/// Represents a browser table (table) element.
	/// </summary>
	public class Table : Element
	{
		#region Constructors

		/// <summary>
		/// Initializes an instance of a browser element.
		/// </summary>
		/// <param name="element"> The browser element this is for. </param>
		/// <param name="browser"> The browser this element is associated with. </param>
		/// <param name="collection"> The collection this element is associated with. </param>
		public Table(JToken element, Browser browser, ElementCollection collection)
			: base(element, browser, collection)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the sortable attribute.
		/// </summary>
		/// <remarks>
		/// Specifies that the table should be sortable.
		/// </remarks>
		public string Sortable
		{
			get { return this["sortable"]; }
			set { this["sortable"] = value; }
		}

		#endregion
	}
}