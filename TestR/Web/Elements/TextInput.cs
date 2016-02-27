#region References

using System.Threading;
using Newtonsoft.Json.Linq;

#endregion

namespace TestR.Web.Elements
{
	/// <summary>
	/// Represents a browser text input element.
	/// </summary>
	public class TextInput : Element
	{
		#region Constructors

		/// <summary>
		/// Initializes an instance of a TextInput browser element.
		/// </summary>
		/// <param name="element"> The browser element this is for. </param>
		/// <param name="browser"> The browser this element is associated with. </param>
		/// <param name="collection"> The collection this element is associated with. </param>
		public TextInput(JToken element, Browser browser, ElementCollection collection)
			: base(element, browser, collection)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the autofocus attribute.
		/// </summary>
		/// <remarks>
		/// HTML5: Specifies that an input element should automatically get focus when the page loads.
		/// </remarks>
		public string AutoFocus
		{
			get { return this["autofocus"]; }
			set { this["autofocus"] = value; }
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
		/// Gets or sets the pattern attribute.
		/// </summary>
		/// <remarks>
		/// HTML5: Specifies a regular expression that an input element's value is checked against.
		/// </remarks>
		public string Pattern
		{
			get { return this["pattern"]; }
			set { this["pattern"] = value; }
		}

		/// <summary>
		/// Gets or sets the place holder attribute.
		/// </summary>
		/// <remarks>
		/// HTML5: Specifies a short hint that describes the expected value of an input element.
		/// </remarks>
		public string PlaceHolder
		{
			get { return this["placeholder"]; }
			set { this["placeholder"] = value; }
		}

		/// <summary>
		/// Gets or sets the read only attribute.
		/// </summary>
		/// <remarks>
		/// Specifies that an input field is read-only
		/// </remarks>
		public string ReadOnly
		{
			get { return this["readonly"]; }
			set { this["readonly"] = value; }
		}

		/// <summary>
		/// Gets or sets the step attribute.
		/// </summary>
		/// <remarks>
		/// HTML5: Specifies the legal number intervals for an input field.
		/// </remarks>
		public string Step
		{
			get { return this["step"]; }
			set { this["step"] = value; }
		}

		/// <summary>
		/// Gets or sets the value attribute.
		/// </summary>
		/// <remarks>
		/// Specifies the value of an input element.
		/// </remarks>
		public override string Text
		{
			get { return this["value"]; }
			set { TypeText(value, true); }
		}

		/// <summary>
		/// Gets the delay (in milliseconds) between each character.
		/// </summary>
		public int TypingDelay => Browser.SlowMotion ? 50 : 10;

		/// <summary>
		/// Gets or sets the value attribute.
		/// </summary>
		/// <remarks>
		/// Specifies the value of an input element.
		/// </remarks>
		public string Value
		{
			get { return this["value"]; }
			set { TypeText(value, true); }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Type text into the element.
		/// </summary>
		/// <param name="value"> The value to be typed. </param>
		/// <param name="reset"> Clear the input before typing the text. </param>
		public void TypeText(string value, bool reset = false)
		{
			Focus();
			Highlight(true);

			var newValue = reset ? string.Empty : Text;

			foreach (var character in value)
			{
				var eventProperty = GetKeyCodeEventProperty(character);
				newValue += character;
				FireEvent("keyDown", eventProperty);
				SetAttributeValue("value", newValue);
				FireEvent("keyPress", eventProperty);
				FireEvent("keyUp", eventProperty);
				Thread.Sleep(TypingDelay);
			}

			Thread.Sleep(TypingDelay);
			Highlight(false);
			TriggerElement();
		}

		#endregion
	}
}