#region References

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;
using TestR.Helpers;
using TestR.Logging;

#endregion

namespace TestR.Web
{
	/// <summary>
	/// Represents an element for a browser.
	/// </summary>
	public class Element
	{
		#region Fields

		private readonly ElementCollection _collection;
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
		/// <param name="collection"> The collection this element is associated with. </param>
		public Element(JToken element, Browser browser, ElementCollection collection)
		{
			_element = element;
			Browser = browser;
			_collection = collection;
			_originalColor = GetStyleAttributeValue("backgroundColor", false) ?? "";
			_highlightColor = "yellow";
		}

		static Element()
		{
			_propertiesToRename = new Dictionary<string, string> { { "class", "className" } };
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the access key attribute.
		/// </summary>
		/// <remarks>
		/// Specifies a shortcut key to activate/focus an element.
		/// </remarks>
		public string AccessKey
		{
			get { return this["accesskey"]; }
			set { this["accesskey"] = value; }
		}

		/// <summary>
		/// Gets the browser this element is currently associated with.
		/// </summary>
		public Browser Browser { get; }

		/// <summary>
		/// Gets the children for this element.
		/// </summary>
		public ElementCollection Children => new ElementCollection(_collection.Where(x => x.ParentId == Id));

		/// <summary>
		/// Gets or sets the class attribute.
		/// </summary>
		/// <remarks>
		/// Specifies one or more class names for an element (refers to a class in a style sheet).
		/// </remarks>
		public string Class
		{
			get { return this["class"]; }
			set { this["class"] = value; }
		}

		/// <summary>
		/// Gets the content editable (contenteditable) attribute.
		/// </summary>
		/// <remarks>
		/// HTML5: Specifies whether the content of an element is editable or not.
		/// </remarks>
		public string ContentEditable
		{
			get { return this["contenteditable"]; }
			set { this["contenteditable"] = value; }
		}

		/// <summary>
		/// Gets or sets the context menu attribute.
		/// </summary>
		/// <remarks>
		/// HTML5: Specifies a context menu for an element. The context menu appears when a user Right-clicks on the element.
		/// </remarks>
		public string ContextMenu
		{
			get { return this["contextmenu"]; }
			set { this["contextmenu"] = value; }
		}

		/// <summary>
		/// Gets or sets the draggable attribute.
		/// </summary>
		/// <remarks>
		/// HTML5: Specifies whether an element is draggable or not.
		/// </remarks>
		public string Draggable
		{
			get { return this["draggable"]; }
			set { this["draggable"] = value; }
		}

		/// <summary>
		/// Gets or sets the drop zone (dropzone) attribute.
		/// </summary>
		/// <remarks>
		/// HTML5: Specifies whether the dragged data is copied, moved, or linked, when dropped.
		/// </remarks>
		public string DropZone
		{
			get { return this["dropzone"]; }
			set { this["dropzone"] = value; }
		}

		/// <summary>
		/// Gets or sets the hidden attribute.
		/// </summary>
		/// <remarks>
		/// HTML5: Specifies that an element is not yet, or is no longer, relevant.
		/// </remarks>
		public string Hidden
		{
			get { return this["hidden"]; }
			set { this["hidden"] = value; }
		}

		/// <summary>
		/// Gets the ID attribute.
		/// </summary>
		/// <remarks>
		/// Specifies a unique id for an element.
		/// </remarks>
		public string Id => _element.id;

		/// <summary>
		/// Gets or sets the language (lang) attribute.
		/// </summary>
		/// <remarks>
		/// Specifies the language of the element's content.
		/// </remarks>
		public string Language
		{
			get { return this["lang"]; }
			set { this["lang"] = value; }
		}

		/// <summary>
		/// Gets or sets the name attribute.
		/// </summary>
		public string Name
		{
			get { return _element.name; }
			set { _element.name = value; }
		}

		/// <summary>
		/// The parent element of this element. Returns null if there is no parent.
		/// </summary>
		public Element Parent
		{
			get
			{
				if (string.IsNullOrWhiteSpace(ParentId) || !_collection.ContainsKey(ParentId))
				{
					return null;
				}

				return _collection[ParentId];
			}
		}

		/// <summary>
		/// Gets the ID of the element's parent.
		/// </summary>
		public string ParentId => _element.parentId;

		/// <summary>
		/// Gets or sets the spell check (spellcheck) attribute.
		/// </summary>
		/// <remarks>
		/// HTML5: Specifies whether the element is to have its spelling and grammar checked or not.
		/// </remarks>
		public string SpellCheck
		{
			get { return this["spellcheck"]; }
			set { this["spellcheck"] = value; }
		}

		/// <summary>
		/// Gets or sets the style attribute.
		/// </summary>
		/// <remarks>
		/// Specifies an inline CSS style for an element.
		/// </remarks>
		public string Style
		{
			get { return this["style"]; }
			set { this["style"] = value; }
		}

		/// <summary>
		/// Gets or sets the tab index (tabindex) attribute.
		/// </summary>
		/// <remarks>
		/// Specifies the tabbing order of the element.
		/// </remarks>
		public string TabIndex
		{
			get { return this["tabindex"]; }
			set { this["tabindex"] = value; }
		}

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
		/// Gets the text direction (dir) attribute.
		/// </summary>
		/// <remarks>
		/// Specifies the text direction for the content in an element.
		/// </remarks>
		public string TextDirection
		{
			get { return this["dir"]; }
			set { this["dir"] = value; }
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

		/// <summary>
		/// Gets or sets the translate attribute.
		/// </summary>
		/// <remarks>
		/// HTML5: Specifies whether the content of an element should be translated or not.
		/// </remarks>
		public string Translate
		{
			get { return this["translate"]; }
			set { this["translate"] = value; }
		}

		#endregion

		#region Indexers

		/// <summary>
		/// Gets or sets an attribute or property by name.
		/// </summary>
		/// <param name="name"> The name of the attribute or property to read. </param>
		public string this[string name]
		{
			get { return GetAttributeValue(name, Browser.AutoRefresh); }
			set { SetAttributeValue(name, value); }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Clicks the element.
		/// </summary>
		public void Click()
		{
			Browser.ExecuteScript("document.getElementById('" + Id + "').click()");
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
		public void Focus()
		{
			Browser.ExecuteScript("document.getElementById('" + Id + "').focus()");
		}

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="id"> An ID of the element to get. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the ID. </returns>
		public Element Get(string id, bool recursive = true, bool wait = true)
		{
			return Get<Element>(id, recursive, wait);
		}

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="id"> An ID of the element to get. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the ID. </returns>
		public T Get<T>(string id, bool recursive = true, bool wait = true) where T : Element
		{
			return Get<T>(x => (x.Id == id) || (x.Name == id), recursive, wait);
		}

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="condition"> A function to test each element for a condition. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the condition. </returns>
		public T Get<T>(Func<T, bool> condition, bool recursive = true, bool wait = true) where T : Element
		{
			T response = null;

			Utility.Wait(() =>
			{
				try
				{
					response = Children.Get(condition, recursive);
					if ((response != null) || !wait)
					{
						return true;
					}

					Browser.Refresh();
					return false;
				}
				catch (Exception)
				{
					return !wait;
				}
			}, Browser?.Timeout.TotalMilliseconds ?? Browser.DefaultTimeout);

			return response;
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
			LogManager.Write(highlight ? "Adding highlight to element " + Id + "." : "Removing highlight from element " + Id + ".", LogLevel.Verbose);
			SetStyleAttributeValue("background-color", highlight ? _highlightColor : _originalColor);

			if (Browser.SlowMotion && highlight)
			{
				Thread.Sleep(150);
			}
		}

		/// <summary>
		/// Gets Raw HTML.
		/// </summary>
		public string Html()
		{
			return Browser.ExecuteScript("document.getElementById('" + Id + "').innerHTML");
		}

		/// <summary>
		/// Remove the element. * Experimental
		/// </summary>
		public void Remove()
		{
			Browser.RemoveElement(this);
		}

		/// <summary>
		/// Remove the element attribute. * Experimental
		/// </summary>
		/// <param name="name"> The name of the attribute. </param>
		public void RemoveAttribute(string name)
		{
			Browser.RemoveElementAttribute(this, name);
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
		public string ToDetailString()
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

		/// <summary>
		/// Get the key code event properties for the character.
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