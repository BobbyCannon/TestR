#region References

using System.Linq;
using System.Management.Automation;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Logging;
using TestR.PowerShell;
using TestR.Web;
using TestR.Web.Elements;

#endregion

namespace TestR.AutomationTests.Web
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "Browsers")]
	public class BrowserTests : BrowserTestCmdlet
	{
		#region Constants

		public const string TestSite = "http://localhost:8080";

		#endregion

		#region Constructors

		public BrowserTests()
		{
			BrowserType = BrowserType.All;
			//TestHelper.AddConsoleLogger();
		}

		#endregion

		#region Methods

		[TestMethod]
		public void AngularInputTrigger()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "AngularInputTrigger");
				browser.NavigateTo(TestSite + "/Angular.html#/");

				var email = browser.Elements.TextInputs["email"];
				email.TypeText("user", true);

				var expected = "ng-untouched ng-scope ng-invalid ng-not-empty ng-dirty ng-invalid-email ng-valid-required".Split(' ');
				var actual = email.GetAttributeValue("class", true).Split(' ');
				TestHelper.AreEqual("user", email.Text);
				TestHelper.AreEqual(expected, actual);

				email.TypeText("@domain.com");
				expected = "ng-untouched ng-scope ng-not-empty ng-dirty ng-valid-required ng-valid ng-valid-email".Split(' ');
				actual = email.GetAttributeValue("class", true).Split(' ');
				TestHelper.AreEqual("user@domain.com", email.Text);
				TestHelper.AreEqual(expected, actual);
			});
		}

		[TestMethod]
		public void AngularNewElements()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "AngularNewElements");
				browser.NavigateTo(TestSite + "/Angular.html#/");
				var elementCount = browser.Elements.Count;

				var button = browser.Elements.Buttons["addItem"];
				button.Click();
				browser.Refresh();

				Assert.AreEqual(elementCount + 1, browser.Elements.Count);
				elementCount = browser.Elements.Count;

				button.Click();
				browser.Refresh();
				Assert.AreEqual(elementCount + 1, browser.Elements.Count);
			});
		}

		[TestMethod]
		public void AngularSetTextInputs()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "AngularSetTextInputs");
				browser.NavigateTo(TestSite + "/Angular.html#/form");
				Assert.AreEqual("true", browser.Elements.Buttons["saveButton"].Disabled);
				browser.Elements.TextInputs["pageTitle"].Text = "Hello World";
				Assert.AreEqual("Hello World", browser.Elements.TextInputs["pageTitle"].Text);
				browser.Elements.TextArea["pageText"].Text = "The quick brown fox jumps over the lazy dog's back.";
				Assert.AreEqual("The quick brown fox jumps over the lazy dog's back.", browser.Elements.TextArea["pageText"].Text);
				Assert.AreEqual("false", browser.Elements.Buttons["saveButton"].Disabled);
			});
		}

		[TestMethod]
		public void AngularSwitchPageByNavigateTo()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "AngularInputTrigger");
				browser.NavigateTo(TestSite + "/Angular.html#/");
				Assert.AreEqual(TestSite + "/Angular.html#/", browser.Uri);

				Assert.IsTrue(browser.Elements.ContainsKey("addItem"));
				Assert.IsTrue(browser.Elements.ContainsKey("anotherPageLink"));

				browser.NavigateTo(TestSite + "/Angular.html#/anotherPage");
				Assert.AreEqual(TestSite + "/Angular.html#/anotherPage", browser.Uri);

				Assert.IsFalse(browser.Elements.ContainsKey("addItem"));
				Assert.IsTrue(browser.Elements.ContainsKey("pageLink"));

				browser.NavigateTo(TestSite + "/Angular.html#/");
				Assert.AreEqual(TestSite + "/Angular.html#/", browser.Uri);

				Assert.IsTrue(browser.Elements.ContainsKey("addItem"));
				Assert.IsTrue(browser.Elements.ContainsKey("anotherPageLink"));
			});
		}

		[TestMethod]
		public void ClickButton()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "ClickButton");
				browser.NavigateTo(TestSite + "/index.html");
				browser.Elements["button"].Click();

				var actual = browser.Elements.TextArea["textarea"].Text;
				Assert.AreEqual("button", actual);
			});
		}

		[TestMethod]
		public void ClickButtonByName()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "ClickButtonByName");
				browser.NavigateTo(TestSite + "/index.html");
				browser.Elements.First(x => x.Name == "buttonByName").Click();

				var textArea = browser.Elements.TextArea["textarea"];
				Assert.AreEqual("buttonByName", textArea.Text);
			});
		}

		[TestMethod]
		public void ClickCheckbox()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "ClickCheckbox");
				browser.NavigateTo(TestSite + "/inputs.html");

				foreach (var element in browser.Elements.Checkboxes)
				{
					element.Click();
					Assert.IsTrue(element.Checked);
					element.Click();
					Assert.IsFalse(element.Checked);
				}
			});
		}

		[TestMethod]
		public void ClickInputButton()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "ClickInputButton");
				browser.NavigateTo(TestSite + "/index.html");
				browser.Elements.Buttons["inputButton"].Click();

				var textArea = browser.Elements.TextArea["textarea"];
				Assert.AreEqual("inputButton", textArea.Text);
			});
		}

		[TestMethod]
		public void ClickLink()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "ClickLink");
				browser.NavigateTo(TestSite + "/index.html");
				browser.Elements["link"].Click();

				var textArea = browser.Elements.TextArea["textarea"];
				Assert.AreEqual("link", textArea.Text);
			});
		}

		[TestMethod]
		public void ClickRadioButton()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "ClickCheckbox");
				browser.NavigateTo(TestSite + "/inputs.html");

				Assert.IsFalse(browser.Elements.RadioButtons.Any(x => x.Checked));

				foreach (var element in browser.Elements.RadioButtons)
				{
					element.Click();
					Assert.IsTrue(element.Checked);
					Assert.IsTrue(browser.Elements.RadioButtons.Where(x => x.Id == element.Id).All(x => x.Checked));
					Assert.IsTrue(browser.Elements.RadioButtons.Where(x => x.Id != element.Id).All(x => !x.Checked));
				}
			});
		}

		[TestMethod]
		public void DetectAngularJavaScriptLibrary()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "DetectAngularJavaScriptLibrary");
				browser.NavigateTo(TestSite + "/Angular.html#/");
				Assert.IsTrue(browser.JavascriptLibraries.Contains(JavaScriptLibrary.Angular));
			});
		}

		[TestMethod]
		public void DetectBootstrap2JavaScriptLibrary()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "DetectBootstrap2JavaScriptLibrary");
				browser.NavigateTo(TestSite + "/Bootstrap2.html");
				Assert.IsTrue(browser.JavascriptLibraries.Contains(JavaScriptLibrary.Bootstrap2));
			});
		}

		[TestMethod]
		public void DetectBootstrap3JavaScriptLibrary()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "DetectBootstrap3JavaScriptLibrary");
				browser.NavigateTo(TestSite + "/Bootstrap3.html");
				Assert.IsTrue(browser.JavascriptLibraries.Contains(JavaScriptLibrary.Bootstrap3));
			});
		}

		[TestMethod]
		public void DetectJQueryJavaScriptLibrary()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "DetectJQueryJavaScriptLibrary");
				browser.NavigateTo(TestSite + "/JQuery.html");
				Assert.IsTrue(browser.JavascriptLibraries.Contains(JavaScriptLibrary.JQuery));
			});
		}

		[TestMethod]
		public void DetectMomentJavaScriptLibrary()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "DetectMomentJavaScriptLibrary");
				browser.NavigateTo(TestSite + "/Moment.html");
				Assert.IsTrue(browser.JavascriptLibraries.Contains(JavaScriptLibrary.Moment));
			});
		}

		[TestMethod]
		public void DetectNoJavaScriptLibrary()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "DetectNoJavaScriptLibrary");
				browser.NavigateTo(TestSite + "/Inputs.html");
				Assert.AreEqual(0, browser.JavascriptLibraries.Count());
			});
		}

		[TestMethod]
		public void ElementChildren()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "ElementChildren");
				browser.NavigateTo(TestSite + "/relationships.html");
				var children = browser.Elements["parent1div"].Children;

				var expected = new[] { "child1div", "child2span", "child3br", "child4input" };
				Assert.AreEqual(4, children.Count);
				TestHelper.AreEqual(expected.ToList(), children.Select(x => x.Id).ToList());
			});
		}

		[TestMethod]
		public void ElementParent()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "ElementParent");
				browser.NavigateTo(TestSite + "/relationships.html");
				var element = browser.Elements["child1div"].Parent;
				Assert.AreEqual("parent1div", element.Id);
			});
		}

		[TestMethod]
		public void EnumerateDivisions()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "EnumerateDivisions");
				browser.NavigateTo(TestSite + "/index.html");
				var elements = browser.Elements.Divisions;
				Assert.AreEqual(2, elements.Count());

				foreach (var division in elements)
				{
					division.Highlight(true);
				}
			});
		}

		[TestMethod]
		public void EnumerateHeaders()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "EnumerateHeaders");
				browser.NavigateTo(TestSite + "/index.html");
				var elements = browser.Elements.Headers;
				Assert.AreEqual(6, elements.Count());

				foreach (var header in elements)
				{
					header.Text = "Header!";
				}
			});
		}

		[TestMethod]
		public void FilterElementByTextElements()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "FilterElementByTextElements");
				browser.NavigateTo(TestSite + "/inputs.html");
				var inputs = browser.Elements.TextInputs.ToList();
				// Old IE treats input types of Date, Month, Week as "Text" which increases input count.
				//var expected = browser.BrowserType == BrowserType.InternetExplorer ? 11 : 8;
				Assert.AreEqual(8, inputs.Count);
			});
		}

		[TestMethod]
		public void FindElementByClass()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "FindElementByClass");
				browser.NavigateTo(TestSite + "/index.html");
				var elements = browser.Elements.Where(x => x.Class.Contains("red"));
				Assert.AreEqual(1, elements.Count());
			});
		}

		[TestMethod]
		public void FindElementByClassByValueAccessor()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "FindElementByClassByValueAccessor");
				browser.NavigateTo(TestSite + "/index.html");
				var elements = browser.Elements.Where(x => x["class"].Contains("red"));
				Assert.AreEqual(1, elements.Count());
			});
		}

		[TestMethod]
		public void FindElementByClassProperty()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "FindElementByClassProperty");
				browser.NavigateTo(TestSite + "/index.html");
				var elements = browser.Elements.Links.Where(x => x.Class == "bold");
				Assert.AreEqual(1, elements.Count());
			});
		}

		[TestMethod]
		public void FindElementByDataAttribute()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "FindElementByDataAttribute");
				browser.NavigateTo(TestSite + "/Index.html");

				var link = browser.Elements.FirstOrDefault(x => x["data-test"] == "testAnchor");

				Assert.IsNotNull(link, "Failed to find the link by data attribute.");
				Assert.AreEqual(link.Id, "a1");
			});
		}

		[TestMethod]
		public void FindHeadersByText()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "FindHeadersByText");
				browser.NavigateTo(TestSite + "/index.html");
				var elements = browser.Elements.OfType<Header>().Where(x => x.Text.Contains("Header"));
				Assert.AreEqual(6, elements.Count());
			});
		}

		[TestMethod]
		public void FindSpanElementByText()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "FindSpanElementByText");
				browser.NavigateTo(TestSite + "/index.html");
				var elements = browser.Elements.Spans.Where(x => x.Text == "SPAN with ID of 1");
				Assert.AreEqual(1, elements.Count());
			});
		}

		[TestMethod]
		public void FindTextInputsByText()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "FindTextInputsByText");
				browser.NavigateTo(TestSite + "/index.html");
				var elements = browser.Elements.OfType<TextInput>().Where(x => x.Text == "Hello World");
				Assert.AreEqual(1, elements.Count());
			});
		}

		[TestMethod]
		public void Focus()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "Focus");
				browser.NavigateTo(TestSite + "/inputs.html");

				var expected = browser.Elements.TextInputs.Last();
				Assert.IsNull(browser.ActiveElement, "There should not be an active element.");

				expected.Focus();
				Assert.IsNotNull(browser.ActiveElement, "There should be an active element.");
				Assert.AreEqual(expected.Id, browser.ActiveElement.Id);
			});
		}

		[TestMethod]
		public void GetElementByNameIndex()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "GetElementByNameIndex");
				browser.NavigateTo(TestSite + "/index.html");
				var actual = browser.Elements.First(x => x.Name == "inputName").Name;
				Assert.AreEqual("inputName", actual);
			});
		}

		[TestMethod]
		public void HighlightAllElements()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "HighlightAllElements");
				browser.NavigateTo(TestSite + "/index.html");
				var elements = browser.Elements;

				foreach (var element in elements)
				{
					element.Highlight(true);
					element.Highlight(false);
				}
			});
		}

		[TestMethod]
		public void HighlightElement()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "HighlightElement");
				browser.NavigateTo(TestSite + "/inputs.html");

				var inputElements = browser.Elements.Where(t => t.TagName == "input").ToList();
				foreach (var element in inputElements)
				{
					var originalColor = element.GetStyleAttributeValue("background-color");
					element.Highlight(true);
					Assert.AreEqual("yellow", element.GetStyleAttributeValue("background-color"));
					element.Highlight(false);
					Assert.AreEqual(originalColor, element.GetStyleAttributeValue("background-color"));
				}
			});
		}

		[TestMethod]
		public void NavigateTo()
		{
			ForEachBrowser(browser =>
			{
				var expected = TestSite + "/index.html";
				browser.NavigateTo(expected);
				Assert.AreEqual(expected, browser.Uri.ToLower());
			});
		}

		[TestMethod]
		public void NavigateToWithDifferentFinalUri()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "NavigateToWithDifferentFinalUri");
				browser.NavigateTo(TestSite + "/Angular.html");
				Assert.AreEqual(TestSite + "/Angular.html#/", browser.Uri);
			});
		}

		[TestMethod]
		public void RedirectByLink()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "RedirectByLink");
				var expected = TestSite + "/index.html";
				browser.NavigateTo(expected);
				Assert.AreEqual(expected, browser.Uri.ToLower());

				// Redirect by the link.
				browser.Elements.Links["redirectLink"].Click();
				browser.WaitForNavigation(TestSite + "/Inputs.html");

				expected = TestSite + "/inputs.html";
				Assert.AreEqual(expected, browser.Uri.ToLower());
				browser.Elements["submit"].Click();
			});
		}

		[TestMethod]
		public void RedirectByScript()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "RedirectByScript");
				var expected = TestSite + "/index.html";
				browser.NavigateTo(expected);
				Assert.AreEqual(expected, browser.Uri.ToLower());
				Assert.IsNotNull(browser.Elements["link"], "Failed to find the link element.");

				// Redirect by a script.
				browser.ExecuteScript("window.location.href = 'inputs.html'");
				browser.WaitForNavigation(TestSite + "/inputs.html");

				expected = TestSite + "/inputs.html";
				Assert.AreEqual(expected, browser.Uri.ToLower());
				browser.Elements["submit"].Click();
			});
		}

		[TestMethod]
		public void Refresh()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "Refresh");
				var expected = new[] { TestSite + "/index.html", TestSite + "/inputs.html" };

				for (var i = 0; i < 10; i++)
				{
					browser.NavigateTo(expected[i % 2]);
					Assert.AreEqual(expected[i % 2], browser.Uri.ToLower());
					browser.Refresh();
					Thread.Sleep(10);
				}
			});
		}

		[TestMethod]
		public void SelectSelectedOption()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "SelectSelectedOption");
				browser.NavigateTo(TestSite + "/index.html");
				var select = browser.Elements.Selects["select"];
				Assert.AreEqual("2", select.Value);
				Assert.AreEqual("2", select.SelectedOption.Value);
				Assert.AreEqual("Two", select.SelectedOption.Text);
			});
		}

		[TestMethod]
		public void SetButtonText()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "SetButtonText");
				browser.NavigateTo(TestSite + "/index.html");
				browser.Elements["button"].Text = "Hello";

				var actual = browser.Elements["button"].GetAttributeValue("textContent", true);
				Assert.AreEqual("Hello", actual);
			});
		}

		[TestMethod]
		public void SetInputButtonText()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "SetButtonText");
				browser.NavigateTo(TestSite + "/index.html");
				browser.Elements["inputButton"].Text = "Hello";

				var actual = browser.Elements["inputButton"].GetAttributeValue("value", true);
				Assert.AreEqual("Hello", actual);
			});
		}

		[TestMethod]
		public void SetTextAllInputs()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "SetTextAllInputs");
				browser.NavigateTo(TestSite + "/inputs.html");

				foreach (var input in browser.Elements.TextInputs)
				{
					if (input.Id == "number")
					{
						input.Text = "100";
						Assert.AreEqual("100", input.Text);
					}
					else
					{
						input.Text = input.Id;
						Assert.AreEqual(input.Id, input.Text);
					}
				}

				foreach (var input in browser.Elements.TextArea)
				{
					input.Text = input.Id;
					Assert.AreEqual(input.Id, input.Text);
				}
			});
		}

		[TestMethod]
		public void TestContentForInputText()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "TestContentForInputText");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.Elements.TextInputs["inputText1"];
				Assert.AreEqual(element.Text, "inputText1");
			});
		}

		[TestMethod]
		public void TextAreaValue()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "TextAreaValue");
				browser.NavigateTo(TestSite + "/");

				var element = browser.Elements.TextArea["textarea"];
				Assert.AreEqual(element.Text, "Text Area's \"Quotes\" Data");

				element.Text = "\"Text Area's \"Quote's\" Data\"";
				Assert.AreEqual(element.Text, "\"Text Area's \"Quote's\" Data\"");
			});
		}

		[TestMethod]
		public void TextContentForDiv()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "TestContentForDiv");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.Elements.Divisions["div1"];
				Assert.AreEqual(element.Text, "\n\t\t\tDiv - Span\n\t\t\tOther Text\n\t\t");
			});
		}

		[TestMethod]
		public void TextContentForDivWithChildren()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "TestContentForHeader1");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.Elements.Divisions["div2"];
				Assert.AreEqual(element.Text, "\n\t\t\tHeader One\n\t\t\tHeader Two\n\t\t\tHeader Three\n\t\t\tHeader Four\n\t\t\tHeader Five\n\t\t");
			});
		}

		[TestMethod]
		public void TextContentForHeader1()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "TestContentForHeader1");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.Elements.Headers["h1"];
				Assert.AreEqual(element.Text, "Header One");
			});
		}

		[TestMethod]
		public void TextContentForHeader2()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "TestContentForHeader2");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.Elements.Headers["h2"];
				Assert.AreEqual(element.Text, "Header Two");
			});
		}

		[TestMethod]
		public void TextContentForHeader3()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "TestContentForHeader3");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.Elements.Headers["h3"];
				Assert.AreEqual(element.Text, "Header Three");
			});
		}

		[TestMethod]
		public void TextContentForHeader4()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "TestContentForHeader4");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.Elements.Headers["h4"];
				Assert.AreEqual(element.Text, "Header Four");
			});
		}

		[TestMethod]
		public void TextContentForHeader5()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "TestContentForHeader5");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.Elements.Headers["h5"];
				Assert.AreEqual(element.Text, "Header Five");
			});
		}

		[TestMethod]
		public void TypeTextAllInputs()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "TypeTextAllInputs");
				browser.NavigateTo(TestSite + "/inputs.html");

				foreach (var input in browser.Elements.TextInputs)
				{
					if (input.Id == "number")
					{
						input.TypeText("100");
						Assert.AreEqual("100", input.Text);
					}
					else
					{
						input.TypeText(input.Id);
						Assert.AreEqual(input.Id, input.Text);
					}
				}

				foreach (var input in browser.Elements.TextArea)
				{
					input.TypeText(input.Id);
					Assert.AreEqual(input.Id, input.Text);
				}
			});
		}

		[TestMethod]
		public void TypeTextAppendInput()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "TypeTextAppendInput");
				browser.NavigateTo(TestSite + "/inputs.html");
				var input = browser.Elements.TextInputs["text"];
				input.Value = "foo";
				input.TypeText("bar");
				Assert.AreEqual("foobar", input.Value);
			});
		}

		[TestMethod]
		public void TypeTextPasswordInput()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "TypeTextPasswordInput");
				browser.NavigateTo(TestSite + "/inputs.html");
				var input = browser.Elements.TextInputs["password"];
				input.TypeText("password", true);
				Assert.AreEqual("password", input.Value);
			});
		}

		[TestMethod]
		public void TypeTextSetInput()
		{
			ForEachBrowser(browser =>
			{
				LogManager.UpdateReferenceId(browser, "TypeTextSetInput");
				browser.NavigateTo(TestSite + "/inputs.html");
				var input = browser.Elements.TextInputs["text"];
				input.Value = "foo";
				input.TypeText("bar", true);
				Assert.AreEqual("bar", input.Value);
			});
		}

		#endregion
	}
}