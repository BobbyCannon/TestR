#region References

using Newtonsoft.Json.Linq;

#endregion

namespace TestR.Web.Elements
{
	/// <summary>
	/// Represents a browser Directory (dir) element.
	/// </summary>
	/// <remarks>
	/// Not supported in HTML5 so use CSS instead.
	/// </remarks>
	public class Directory : Element
	{
		#region Constructors

		/// <summary>
		/// Initializes an instance of a browser element.
		/// </summary>
		/// <param name="element"> The browser element this is for. </param>
		/// <param name="browser"> The browser this element is associated with. </param>
		/// <param name="collection"> The collection this element is associated with. </param>
		public Directory(JToken element, Browser browser, ElementCollection collection)
			: base(element, browser, collection)
		{
		}

		#endregion
	}
}