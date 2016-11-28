#region References

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;

#endregion

namespace TestR.Web
{
	/// <summary>
	/// Represents an element for a browser.
	/// </summary>
	public class WebElement : Element
	{
		#region Fields

		private readonly dynamic _element;
		private readonly string _highlightColor;
		private readonly string _originalColor;

		/// <summary>
		/// Properties that need to be renamed when requested.
		/// </summary>
		private static readonly Dictionary<string, string> _propertiesToRename;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes an instance of a browser element.
		/// </summary>
		/// <param name="element"> The browser element this is for. </param>
		/// <param name="browser"> The browser this element is associated with. </param>
		/// <param name="parent"> </param>
		public WebElement(JToken element, Browser browser, ElementHost parent)
			: base(browser.Application, parent)
		{
			Browser = browser;
			_element = element;
			_originalColor = GetStyleAttributeValue("backgroundColor", false) ?? "";
			_highlightColor = "yellow";
		}

		static WebElement()
		{
			_propertiesToRename = new Dictionary<string, string> { { "class", "className" } };
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the browser this element is currently associated with.
		/// </summary>
		public Browser Browser { get; }

		/// <inheritdoc />
		public override bool Focused => false;

		/// <inheritdoc />
		public override Element FocusedElement => null;

		/// <inheritdoc />
		public override int Height => this["offsetHeight"].ToInt();

		/// <inheritdoc />
		public override string Id => _element.id;

		/// <inheritdoc />
		public override string this[string name]
		{
			get { return GetAttributeValue(name, Browser.AutoRefresh); }
			set { SetAttributeValue(name, value); }
		}

		/// <inheritdoc />
		public override Point Location => new Point(this["offsetLeft"].ToInt(), this["offsetTop"].ToInt());

		/// <inheritdoc />
		public override string Name => _element.name;

		/// <summary>
		/// Gets the tag element name.
		/// </summary>
		public string TagName => _element.tagName;

		/// <summary>
		/// Gets or sets the text content.
		/// </summary>
		public virtual string Text
		{
			get { return this["textContent"]; }
			set { this["textContent"] = value; }
		}

		/// <summary>
		/// Gets or sets the title attribute.
		/// </summary>
		/// <remarks>
		/// Specifies extra information about an element.
		/// </remarks>
		public string Title
		{
			get { return this["title"]; }
			set { this["title"] = value; }
		}

		/// <inheritdoc />
		public override int Width => this["offsetWidth"].ToInt();

		#endregion

		#region Methods

		/// <inheritdoc />
		public override Element CaptureSnippet(string filePath)
		{
			// todo: Implement this :)
			return this;
		}

		/// <inheritdoc />
		public override Element Click(int x = 0, int y = 0)
		{
			Browser.ExecuteScript("document.getElementById('" + Id + "').click()");
			return this;
		}

		/// <summary>
		/// Fires an event on the element.
		/// </summary>
		/// <param name="eventName"> The events name to fire. </param>
		/// <param name="eventProperties"> The properties for the event. </param>
		public void FireEvent(string eventName, Dictionary<string, string> eventProperties)
		{
			var values = eventProperties.Aggregate("", (current, item) => current + "{ key: '" + item.Key + "', value: '" + item.Value + "'},");
			if (values.Length > 0)
			{
				values = values.Remove(values.Length - 1, 1);
			}

			var script = "TestR.triggerEvent(document.getElementById('" + Id + "'), '" + eventName.ToLower() + "', [" + values + "]);";
			Browser.ExecuteScript(script);
		}

		/// <summary>
		/// Focuses on the element.
		/// </summary>
		public override Element Focus()
		{
			Browser.ExecuteScript("document.getElementById('" + Id + "').focus()");
			return this;
		}

		/// <summary>
		/// Gets an attribute value by the provided name.
		/// </summary>
		/// <param name="name"> The name of the attribute to read. </param>
		/// <returns> The attribute value. </returns>
		public string GetAttributeValue(string name)
		{
			return GetAttributeValue(name, Browser.AutoRefresh);
		}

		/// <summary>
		/// Gets an attribute value by the provided name.
		/// </summary>
		/// <param name="name"> The name of the attribute to read. </param>
		/// <param name="refresh"> A flag to force the element to refresh. </param>
		/// <returns> The attribute value. </returns>
		public string GetAttributeValue(string name, bool refresh)
		{
			string value;

			if (refresh)
			{
				name = _propertiesToRename.ContainsKey(name) ? _propertiesToRename[name] : name;
				var script = "TestR.getElementValue('" + Id + "','" + name + "')";
				value = Browser.ExecuteScript(script);
			}
			else
			{
				value = GetCachedAttribute(name);
			}

			if (string.IsNullOrWhiteSpace(value))
			{
				return string.Empty;
			}

			SetElementAttributeValue(name, value);
			return value;
		}

		/// <summary>
		/// Gets an attribute style value by the provided name.
		/// </summary>
		/// <param name="name"> The name of the attribute style to read. </param>
		/// <returns> The attribute style value. </returns>
		public string GetStyleAttributeValue(string name)
		{
			return GetStyleAttributeValue(name, Browser.AutoRefresh);
		}

		/// <summary>
		/// Gets an attribute style value by the provided name.
		/// </summary>
		/// <param name="name"> The name of the attribute style to read. </param>
		/// <param name="forceRefresh"> A flag to force the element to refresh. </param>
		/// <returns> The attribute style value. </returns>
		public string GetStyleAttributeValue(string name, bool forceRefresh)
		{
			var styleValue = GetAttributeValue("style", forceRefresh);
			if (styleValue == null)
			{
				return string.Empty;
			}

			var styleValues = styleValue.Split(';')
				.Select(x => x.Split(':'))
				.Where(x => x.Length == 2)
				.Select(x => new KeyValuePair<string, string>(x[0].Trim(), x[1].Trim()))
				.ToList()
				.ToDictionary(x => x.Key, x => x.Value);

			return styleValues.ContainsKey(name) ? styleValues[name] : string.Empty;
		}

		/// <summary>
		/// Highlight or resets the element.
		/// </summary>
		/// <param name="highlight">
		/// If true the element is highlight yellow. If false the element is returned to its original
		/// color.
		/// </param>
		public void Highlight(bool highlight)
		{
			//LogManager.Write(highlight ? "Adding highlight to element " + Id + "." : "Removing highlight from element " + Id + ".", LogLevel.Verbose);
			SetStyleAttributeValue("background-color", highlight ? _highlightColor : _originalColor);

			if (Browser.Application.SlowMotion && highlight)
			{
				Thread.Sleep(150);
			}
		}

		/// <inheritdoc />
		public override Element MoveMouseTo(int x = 0, int y = 0)
		{
			return this;
		}

		/// <inheritdoc />
		public override ElementHost Refresh()
		{
			return this;
		}

		/// <inheritdoc />
		public override Element RightClick(int x = 0, int y = 0)
		{
			return this;
		}

		/// <summary>
		/// Sets an attribute value by the provided name.
		/// </summary>
		/// <param name="name"> The name of the attribute to write. </param>
		/// <param name="value"> The value to be written. </param>
		public void SetAttributeValue(string name, string value)
		{
			name = _propertiesToRename.ContainsKey(name) ? _propertiesToRename[name] : name;
			value = value.Replace("\r", "\\r")
				.Replace("\n", "\\n")
				.Replace("\'", "\\\'")
				.Replace("\"", "\\\"");

			var script = "TestR.setElementValue('" + Id + "','" + name + "','" + value + "')";
			Browser.ExecuteScript(script);
			AddOrUpdateElementAttribute(name, value);
			TriggerElement();
		}

		/// <summary>
		/// Sets an attribute style value by the provided name.
		/// </summary>
		/// <param name="name"> The name of the attribute style to write. </param>
		/// <param name="value"> The style value to be written. </param>
		public void SetStyleAttributeValue(string name, string value)
		{
			var styleValue = GetCachedAttribute("style") ?? string.Empty;
			var styleValues = styleValue
				.Split(';')
				.Select(x => x.Split(':'))
				.Where(x => x.Length == 2)
				.Select(x => new KeyValuePair<string, string>(x[0], x[1]))
				.ToList()
				.ToDictionary(x => x.Key, x => x.Value);

			if (!styleValues.ContainsKey(name))
			{
				styleValues.Add(name, value);
			}

			if (string.IsNullOrWhiteSpace(value))
			{
				styleValues.Remove(name);
			}
			else
			{
				styleValues[name] = value;
			}

			styleValue = string.Join(";", styleValues.Select(x => x.Key + ":" + x.Value));
			SetAttributeValue("style", styleValue);
		}

		/// <summary>
		/// Provides a string of details for the element.
		/// </summary>
		/// <returns> The string of element details. </returns>
		public override string ToDetailString()
		{
			var builder = new StringBuilder();

			builder.AppendLine("id : " + Id);
			builder.AppendLine("name : " + Name);
			builder.AppendLine("type : " + GetType().Name);

			for (var i = 0; i < _element.attributes.Count; i++)
			{
				string attributeName = _element.attributes[i++].ToString();
				builder.AppendLine($"{attributeName} : {_element.attributes[i]}");
			}

			return builder.ToString();
		}

		/// <inheritdoc />
		public override ElementHost WaitForComplete(int minimumDelay = 0)
		{
			return Browser.WaitForComplete(minimumDelay);
		}

		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
		}

		/// <summary>
		/// First the key code event properties for the character.
		/// </summary>
		/// <param name="character"> The character to get the event properties for. </param>
		/// <returns> An event properties for the character. </returns>
		protected Dictionary<string, string> GetKeyCodeEventProperty(char character)
		{
			return new Dictionary<string, string>
			{
				{ "keyCode", ((int) character).ToString() },
				{ "charCode", ((int) character).ToString() },
				{ "which", ((int) character).ToString() }
			};
		}

		/// <summary>
		/// Triggers the element via the Angular function "trigger".
		/// </summary>
		protected void TriggerElement()
		{
			if (Browser.JavascriptLibraries.Contains(JavaScriptLibrary.Angular))
			{
				Browser.ExecuteScript("angular.element(document.querySelector('#" + Id + "')).triggerHandler('input');", false);
				Browser.ExecuteScript("angular.element(document.querySelector('#" + Id + "')).trigger('input');", false);
				Browser.ExecuteScript("angular.element(document.querySelector('#" + Id + "')).trigger('change');", false);
			}
		}

		/// <summary>
		/// Add or updates the cached attributes for this element.
		/// </summary>
		/// <param name="name"> </param>
		/// <param name="value"> </param>
		private void AddOrUpdateElementAttribute(string name, string value)
		{
			for (var i = 0; i < _element.attributes.Count; i++)
			{
				string attributeName = _element.attributes[i++].ToString();
				if (attributeName == name)
				{
					_element.attributes[i] = value;
					return;
				}
			}

			_element.attributes.Add(name);
			_element.attributes.Add(value);
		}

		/// <summary>
		/// Gets the attribute from the local cache.
		/// </summary>
		/// <param name="name"> The name of the attribute. </param>
		/// <returns> Returns the value or null if the attribute was not found. </returns>
		private string GetCachedAttribute(string name)
		{
			for (var i = 0; i < _element.attributes.Count; i++)
			{
				string attributeName = _element.attributes[i++].ToString();
				if (attributeName == name)
				{
					return _element.attributes[i].ToString();
				}
			}

			return null;
		}

		/// <summary>
		/// Sets the element attribute value. If the attribute is not found we'll add it.
		/// </summary>
		/// <param name="name"> </param>
		/// <param name="value"> </param>
		private void SetElementAttributeValue(string name, string value)
		{
			for (var i = 0; i < _element.attributes.Count; i++)
			{
				string attributeName = _element.attributes[i++].ToString();
				if (attributeName != name)
				{
					continue;
				}

				_element.attributes[i] = value;
				return;
			}

			_element.attributes.Add(name);
			_element.attributes.Add(value);
		}

		#endregion
	}
}