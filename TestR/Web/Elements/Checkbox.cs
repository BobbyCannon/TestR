﻿#region References

using Newtonsoft.Json.Linq;

#endregion

namespace TestR.Web.Elements
{
	/// <summary>
	/// Represent a browser input checkbox element.
	/// </summary>
	public class CheckBox : WebElement
	{
		#region Constructors

		/// <summary>
		/// Initializes an instance of a check box browser element.
		/// </summary>
		/// <param name="element"> The browser element this is for. </param>
		/// <param name="browser"> The browser this element is associated with. </param>
		/// <param name="parent"> The parent host for this element. </param>
		public CheckBox(JToken element, Browser browser, ElementHost parent)
			: base(element, browser, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the checked attribute.
		/// </summary>
		/// <remarks>
		/// Specifies that an element should be pre-selected when the page loads (for type="checkbox" or type="radio").
		/// </remarks>
		public bool Checked
		{
			get { return this["checked"] == "true"; }
			set { this["checked"] = value.ToString(); }
		}

		/// <summary>
		/// Gets or sets the disabled attribute.
		/// </summary>
		/// <remarks>
		/// Specifies that a button should be disabled.
		/// </remarks>
		public string Disabled
		{
			get { return this["disabled"]; }
			set { this["disabled"] = value; }
		}

		/// <summary>
		/// Gets or sets the form attribute.
		/// </summary>
		/// <remarks>
		/// HTML5: Specifies one or more forms the button belongs to.
		/// </remarks>
		public string Form
		{
			get { return this["form"]; }
			set { this["form"] = value; }
		}

		/// <summary>
		/// Gets or sets the form no validate attribute.
		/// </summary>
		/// <remarks>
		/// HTML5: Specifies that the form-data should not be validated on submission. Only for type="submit".
		/// </remarks>
		public string FormNoValidate
		{
			get { return this["formnovalidate"]; }
			set { this["formnovalidate"] = value; }
		}

		/// <summary>
		/// Gets or sets the value attribute.
		/// </summary>
		public override string Text
		{
			get { return TagName == "button" ? this["value"] : this["textContent"]; }
			set { this[TagName == "button" ? "value" : "textContent"] = value; }
		}

		/// <summary>
		/// Gets or sets the value attribute.
		/// </summary>
		public string Value
		{
			get { return this["value"]; }
			set { this["value"] = value; }
		}

		#endregion
	}
}