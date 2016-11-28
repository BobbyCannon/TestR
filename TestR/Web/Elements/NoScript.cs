#region References

using Newtonsoft.Json.Linq;

#endregion

namespace TestR.Web.Elements
{
	/// <summary>
	/// Represents a browser NoScript element.
	/// </summary>
	public class NoScript : WebElement
	{
		#region Constructors

		/// <summary>
		/// Initializes an instance of a browser element.
		/// </summary>
		/// <param name="element"> The browser element this is for. </param>
		/// <param name="browser"> The browser this element is associated with. </param>
		/// <param name="parent"> The parent host for this element. </param>
		public NoScript(JToken element, Browser browser, ElementHost parent)
			: base(element, browser, parent)
		{
		}

		#endregion
	}
}