#region References

using Newtonsoft.Json.Linq;

#endregion

namespace TestR.Web.Elements
{
	/// <summary>
	/// Represent a browser line break element.
	/// </summary>
	public class LineBreak : Element
	{
		#region Constructors

		/// <summary>
		/// Initializes an instance of a line break browser element.
		/// </summary>
		/// <param name="element"> The browser element this is for. </param>
		/// <param name="browser"> The browser this element is associated with. </param>
		/// <param name="collection"> The collection this element is associated with. </param>
		public LineBreak(JToken element, Browser browser, ElementCollection collection)
			: base(element, browser, collection)
		{
		}

		#endregion
	}
}