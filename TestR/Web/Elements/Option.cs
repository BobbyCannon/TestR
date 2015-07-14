#region References

using Newtonsoft.Json.Linq;

#endregion

namespace TestR.Web.Elements
{
	/// <summary>
	/// Represents a browser option element.
	/// </summary>
	public class Option : Element
	{
		#region Constructors

		/// <summary>
		/// Initializes an instance of a browser element.
		/// </summary>
		/// <param name="element"> The browser element this is for. </param>
		/// <param name="browser"> The browser this element is associated with. </param>
		/// <param name="collection"> The collection this element is associated with. </param>
		public Option(JToken element, Browser browser, ElementCollection collection)
			: base(element, browser, collection)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the disabled attribute.
		/// </summary>
		/// <remarks>
		/// Specifies that an option should be disabled.
		/// </remarks>
		public string Disabled
		{
			get { return this["disabled"]; }
			set { this["disabled"] = value; }
		}

		/// <summary>
		/// Gets or sets the label attribute.
		/// </summary>
		/// <remarks>
		/// Specifies a shorter label for an option.
		/// </remarks>
		public string Label
		{
			get { return this["label"]; }
			set { this["label"] = value; }
		}

		/// <summary>
		/// Gets or sets the selected attribute.
		/// </summary>
		/// <remarks>
		/// Specifies that an option should be pre-selected when the page loads.
		/// </remarks>
		public string Selected
		{
			get { return this["selected"]; }
			set { this["selected"] = value; }
		}

		/// <summary>
		/// Gets or sets the value for this select.
		/// </summary>
		/// <remarks>
		/// Specifies the value to be sent to a server.
		/// </remarks>
		public string Value
		{
			get { return this["value"]; }
			set
			{
				this["value"] = value;
				TriggerElement();
			}
		}

		#endregion
	}
}