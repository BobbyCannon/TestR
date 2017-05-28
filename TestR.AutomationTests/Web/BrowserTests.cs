#region References

using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop;
using TestR.Native;
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
		#region Constructors

		static BrowserTests()
		{
			TestSite = "https://testr.local";
		}

		#endregion

		#region Properties

		public static string TestSite { get; }

		#endregion

		#region Methods

		[TestMethod]
		public void AngularInputTrigger()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "AngularInputTrigger");
				browser.NavigateTo(TestSite + "/Angular.html");

				var email = browser.First<TextInput>("email");
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
		public void AngularNewElementCount()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "AngularNewElements");
				browser.NavigateTo(TestSite + "/Angular.html");

				var span = browser.First<Span>("items-length");
				var button = browser.First<Button>("addItem");
				button.Click();

				var count = 0;
				var result = span.Wait(x =>
				{
					count++;
					var text = ((Span) x).Text;
					return text == "1";
				}, 2000);

				Assert.IsTrue(count > 1, "We should have tested text more than once.");
				Assert.IsTrue(result, "The count never incremented to 1.");
			});
		}

		[TestMethod]
		public void AngularNewElements()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "AngularNewElements");
				browser.NavigateTo(TestSite + "/Angular.html");
				var elementCount = browser.Descendants().Count();

				var button = browser.First<Button>("addItem");
				button.Click();

				var newItem = browser.FirstOrDefault("items-0");
				Assert.IsNotNull(newItem, "Never found the new item.");

				Assert.AreEqual(elementCount + 1, browser.Descendants().Count());
				elementCount = browser.Descendants().Count();

				button.Click();

				newItem = browser.FirstOrDefault("items-1");
				Assert.IsNotNull(newItem, "Never found the new item.");

				Assert.AreEqual(elementCount + 1, browser.Descendants().Count());
			});
		}

		[TestMethod]
		public void AngularSetTextInputs()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "AngularSetTextInputs");
				browser.NavigateTo(TestSite + "/Angular.html#!/form");
				Assert.AreEqual("true", browser.First<Button>("saveButton").Disabled);
				browser.First<TextInput>("pageTitle").Text = "Hello World";
				Assert.AreEqual("Hello World", browser.First<TextInput>("pageTitle").Text);
				browser.First<TextArea>("pageText").Text = "The quick brown fox jumps over the lazy dog's back.";
				Assert.AreEqual("The quick brown fox jumps over the lazy dog's back.", browser.First<TextArea>("pageText").Text);
				Assert.AreEqual("false", browser.First<Button>("saveButton").Disabled);
			});
		}

		[TestMethod]
		public void AngularSwitchPageByNavigateTo()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "AngularInputTrigger");
				browser.NavigateTo(TestSite + "/Angular.html#!/");
				Assert.AreEqual(TestSite + "/Angular.html#!/", browser.Uri);

				Assert.IsTrue(browser.Contains("addItem"));
				Assert.IsTrue(browser.Contains("anotherPageLink"));

				browser.NavigateTo(TestSite + "/Angular.html#!/anotherPage");
				Assert.AreEqual(TestSite + "/Angular.html#!/anotherPage", browser.Uri);

				Assert.IsFalse(browser.Contains("addItem"));
				Assert.IsTrue(browser.Contains("pageLink"));

				browser.NavigateTo(TestSite + "/Angular.html#!/");
				Assert.AreEqual(TestSite + "/Angular.html#!/", browser.Uri);

				Assert.IsTrue(browser.Contains("addItem"));
				Assert.IsTrue(browser.Contains("anotherPageLink"));
			});
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			Browser.CloseBrowsers();
		}

		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			Browser.CloseBrowsers();
		}

		[TestMethod]
		public void ClickButton()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "ClickButton");
				browser.NavigateTo(TestSite + "/main.html");
				Assert.AreEqual("Text Area's \"Quotes\" Data", browser.First<TextArea>("textarea").Text);
				browser.First("button").Click();
				Assert.AreEqual("button", browser.First<TextArea>("textarea").Text);
			});
		}

		[TestMethod]
		public void ClickButtonByName()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "ClickButtonByName");
				browser.NavigateTo(TestSite + "/main.html");
				browser.First(x => x.Name == "buttonByName").Click();

				var textArea = browser.First<TextArea>("textarea");
				Assert.AreEqual("buttonByName", textArea.Text);
			});
		}

		[TestMethod]
		public void ClickCheckbox()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "ClickCheckbox");
				browser.NavigateTo(TestSite + "/inputs.html");

				foreach (var element in browser.Descendants<CheckBox>())
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
				//LogManager.UpdateReferenceId(browser, "ClickInputButton");
				browser.NavigateTo(TestSite + "/main.html");
				browser.First<Button>("inputButton").Click();

				var textArea = browser.First<TextArea>("textarea");
				Assert.AreEqual("inputButton", textArea.Text);
			});
		}

		[TestMethod]
		public void ClickLink()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "ClickLink");
				browser.NavigateTo(TestSite + "/main.html");
				browser.First("link").Click();

				var textArea = browser.First<TextArea>("textarea");
				Assert.AreEqual("link", textArea.Text);
			});
		}

		[TestMethod]
		public void ClickRadioButton()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "ClickCheckbox");
				browser.NavigateTo(TestSite + "/inputs.html");

				Assert.IsFalse(browser.Descendants<RadioButton>().Any(x => x.Checked));

				foreach (var element in browser.Descendants<RadioButton>())
				{
					element.Click();
					Assert.IsTrue(element.Checked);
					Assert.IsTrue(browser.Descendants<RadioButton>().Where(x => x.Id == element.Id).All(x => x.Checked));
					Assert.IsTrue(browser.Descendants<RadioButton>().Where(x => x.Id != element.Id).All(x => !x.Checked));
				}
			});
		}

		[TestMethod]
		public void DetectAngularJavaScriptLibrary()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "DetectAngularJavaScriptLibrary");
				browser.NavigateTo(TestSite + "/Angular.html#!/");
				Assert.IsTrue(browser.JavascriptLibraries.Contains(JavaScriptLibrary.Angular));
			});
		}

		[TestMethod]
		public void DetectBootstrap2JavaScriptLibrary()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "DetectBootstrap2JavaScriptLibrary");
				browser.NavigateTo(TestSite + "/Bootstrap2.html");
				Assert.IsTrue(browser.JavascriptLibraries.Contains(JavaScriptLibrary.Bootstrap2));
			});
		}

		[TestMethod]
		public void DetectBootstrap3JavaScriptLibrary()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "DetectBootstrap3JavaScriptLibrary");
				browser.NavigateTo(TestSite + "/Bootstrap3.html");
				Assert.IsTrue(browser.JavascriptLibraries.Contains(JavaScriptLibrary.Bootstrap3));
			});
		}

		[TestMethod]
		public void DetectJQueryJavaScriptLibrary()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "DetectJQueryJavaScriptLibrary");
				browser.NavigateTo(TestSite + "/JQuery.html");
				Assert.IsTrue(browser.JavascriptLibraries.Contains(JavaScriptLibrary.JQuery));
			});
		}

		[TestMethod]
		public void DetectMomentJavaScriptLibrary()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "DetectMomentJavaScriptLibrary");
				browser.NavigateTo(TestSite + "/Moment.html");
				Assert.IsTrue(browser.JavascriptLibraries.Contains(JavaScriptLibrary.Moment));
			});
		}

		[TestMethod]
		public void DetectNoJavaScriptLibrary()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "DetectNoJavaScriptLibrary");
				browser.NavigateTo(TestSite + "/Inputs.html");
				Assert.AreEqual(0, browser.JavascriptLibraries.Count());
			});
		}


		[TestMethod]
		public void DeepDescendants()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "ElementChildren");
				browser.NavigateTo(TestSite + "/relationships.html");
				Assert.AreEqual(4043, browser.Descendants().Count());

				foreach (var e in browser.Descendants())
				{
					Console.WriteLine(e.Id);
				}
			});
		}

		[TestMethod]
		public void ElementChildren()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "ElementChildren");
				browser.NavigateTo(TestSite + "/relationships.html");
				Assert.AreEqual(4043, browser.Descendants().Count());
				var children = browser.First("parent1div").Children;

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
				//LogManager.UpdateReferenceId(browser, "ElementParent");
				browser.NavigateTo(TestSite + "/relationships.html");
				var element = browser.First("child1div").Parent;
				Assert.AreEqual("parent1div", element.Id);
			});
		}

		[TestMethod]
		public void Enabled()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "ClickInputButton");
				browser.NavigateTo(TestSite + "/inputs.html");
				Assert.AreEqual(true, browser.First<Button>("button").Enabled);
				Assert.AreEqual(false, browser.First<Button>("button2").Enabled);
			});
		}

		[TestMethod]
		public void EnumerateDivisions()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "EnumerateDivisions");
				browser.NavigateTo(TestSite + "/main.html");
				var elements = browser.Descendants<Division>().ToList();
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
				//LogManager.UpdateReferenceId(browser, "EnumerateHeaders");
				browser.NavigateTo(TestSite + "/main.html");
				var elements = browser.Descendants<Header>().ToList();
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
				//LogManager.UpdateReferenceId(browser, "FilterElementByTextElements");
				browser.NavigateTo(TestSite + "/inputs.html");
				var inputs = browser.Descendants<TextInput>().ToList();
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
				//LogManager.UpdateReferenceId(browser, "FindElementByClass");
				browser.NavigateTo(TestSite + "/main.html");
				var elements = browser.Descendants()
					.Cast<WebElement>()
					.Where(x => x.Class.Contains("red"));
				Assert.AreEqual(1, elements.Count());
			});
		}

		[TestMethod]
		public void FindElementByClassByValueAccessor()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "FindElementByClassByValueAccessor");
				browser.NavigateTo(TestSite + "/main.html");
				var elements = browser.Descendants()
					.Cast<WebElement>().Where(x => x["class"].Contains("red"));
				Assert.AreEqual(1, elements.Count());
			});
		}

		[TestMethod]
		public void FindElementByClassProperty()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "FindElementByClassProperty");
				browser.NavigateTo(TestSite + "/main.html");
				var elements = browser.Descendants<Link>().Where(x => x.Class == "bold");
				Assert.AreEqual(1, elements.Count());
			});
		}

		[TestMethod]
		public void FindElementByDataAttribute()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "FindElementByDataAttribute");
				browser.NavigateTo(TestSite + "/main.html");

				var link = browser.Descendants(x => x["data-test"] == "testAnchor").FirstOrDefault();

				Assert.IsNotNull(link, "Failed to find the link by data attribute.");
				Assert.AreEqual(link.Id, "a1");
			});
		}

		[TestMethod]
		public void FindHeadersByText()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "FindHeadersByText");
				browser.NavigateTo(TestSite + "/main.html");
				var elements = browser.Descendants<Header>().Where(x => x.Text.Contains("Header"));
				Assert.AreEqual(6, elements.Count());
			});
		}

		[TestMethod]
		public void FindSpanElementByText()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "FindSpanElementByText");
				browser.NavigateTo(TestSite + "/main.html");
				var elements = browser.Descendants<Span>(x => x.Text == "SPAN with ID of 1");
				Assert.AreEqual(1, elements.Count());
			});
		}

		[TestMethod]
		public void FindTextInputsByText()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "FindTextInputsByText");
				browser.NavigateTo(TestSite + "/main.html");
				var elements = browser.Descendants<TextInput>().Where(x => x.Text == "Hello World");
				Assert.AreEqual(1, elements.Count());
			});
		}

		[TestMethod]
		public void Focus()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "Focus");
				browser.NavigateTo(TestSite + "/inputs.html");

				var expected = browser.Descendants<TextInput>().Last();
				expected.Focus();
				Assert.IsNotNull(browser.ActiveElement, "There should be an active element.");
				Assert.AreEqual(expected.Id, browser.ActiveElement.Id);
			});
		}

		[TestMethod]
		public void FocusEvent()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "Focus");
				browser.NavigateTo(TestSite + "/main.html");

				var expected = browser.First<TextInput>("text");
				expected.Focus();
				Assert.IsNotNull(browser.ActiveElement, "There should be an active element.");
				Assert.AreEqual(expected.Id, browser.ActiveElement.Id);
				Assert.AreEqual("focused", expected.Text);
			});
		}

		[TestMethod]
		public void FormWithSubInputsWithNamesOfFormAttributeNames()
		{
			ForEachBrowser(browser =>
			{
				browser.NavigateTo(TestSite + "/forms.html");
				Assert.AreEqual(10, browser.Descendants().Count());
				Assert.AreEqual(10, browser.Descendants<WebElement>().Count());

				var form = browser.First<Form>("form");
				Assert.AreEqual("form", form.Id);
				Assert.AreEqual("form", form.Name);
				Assert.AreEqual("form", form.TagName);
				Assert.AreEqual("form", form.GetAttributeValue("id", true));

				var input = browser.First<TextInput>("id");
				Assert.AreEqual("id", input.Id);
				Assert.AreEqual("id", input.Name);
				Assert.AreEqual("input", input.TagName);
				Assert.AreEqual("id", input.GetAttributeValue("id", true));

				input = browser.First<TextInput>("name");
				Assert.AreEqual("name", input.Id);
				Assert.AreEqual("name", input.Name);
				Assert.AreEqual("input", input.TagName);
				Assert.AreEqual("name", input.GetAttributeValue("id", true));
			});
		}

		[TestMethod]
		public void FormWithSubInputsWithNamesOfFormAttributeNamesButMainFormHasNoId()
		{
			ForEachBrowser(browser =>
			{
				browser.NavigateTo(TestSite + "/forms2.html");
				Assert.AreEqual(10, browser.Descendants().Count());
				Assert.AreEqual(10, browser.Descendants<WebElement>().Count());

				var form = browser.First<Form>("form");
				Assert.AreEqual("form", form.Name);
				Assert.AreEqual("form", form.TagName);

				var input = browser.First<TextInput>("id");
				Assert.AreEqual("id", input.Id);
				Assert.AreEqual("id", input.Name);
				Assert.AreEqual("input", input.TagName);
				Assert.AreEqual("id", input.GetAttributeValue("id", true));

				input = browser.First<TextInput>("name");
				Assert.AreEqual("name", input.Id);
				Assert.AreEqual("name", input.Name);
				Assert.AreEqual("input", input.TagName);
				Assert.AreEqual("name", input.GetAttributeValue("id", true));
			});
		}

		[TestMethod]
		public void GetElementByNameIndex()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "GetElementByNameIndex");
				browser.NavigateTo(TestSite + "/main.html");
				var actual = browser.First(x => x.Name == "inputName").Name;
				Assert.AreEqual("inputName", actual);
			});
		}

		[TestMethod]
		public void GetNextSibling()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "AngularNewElements");
				browser.NavigateTo(TestSite + "/Angular.html#!/");

				var items = browser.First<Division>("items");
				Assert.AreEqual(0, items.Children.Count);

				browser.First<Button>("addItem").Click().Click();
				var result = items.Wait(x => x.Refresh().Children.Count >= 2);
				Assert.IsTrue(result, "Should have had two items.");
				Assert.AreEqual(2, items.Children.Count);

				var item0 = items.First("items-0");
				var item1 = item0.GetNextSibling();

				Assert.IsNotNull(item1, "Failed to find sibling.");
				Assert.AreEqual("items-1", item1.Id);
			});
		}

		[TestMethod]
		public void GetPreviousSibling()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "AngularNewElements");
				browser.NavigateTo(TestSite + "/Angular.html#!/");

				var items = browser.First<Division>("items");
				Assert.AreEqual(0, items.Children.Count);

				browser.First<Button>("addItem").Click().Click();
				var result = items.Wait(x => x.Refresh().Children.Count >= 2);
				Assert.IsTrue(result, "Should have had two items.");
				Assert.AreEqual(2, items.Children.Count);

				var item1 = items.First("items-1");
				var item0 = item1.GetPreviousSibling();

				Assert.AreEqual("items-0", item0.Id);
			});
		}

		[TestMethod]
		public void HighlightAllElements()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "HighlightAllElements");
				browser.NavigateTo(TestSite + "/main.html");
				var elements = browser.OfType<WebElement>();

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
				//LogManager.UpdateReferenceId(browser, "HighlightElement");
				browser.NavigateTo(TestSite + "/inputs.html");

				var inputElements = browser.Descendants<WebElement>(t => t.TagName == "input").ToList();
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
		public void Location()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "ClickButton");
				browser.NavigateTo(TestSite + "/main.html");
				var button = browser.First("button");
				button.Location.Dump();
				button.LeftClick();
				browser.WaitForComplete(100);

				Assert.AreEqual("button", browser.First<TextArea>("textarea").Text);
			});
		}

		[TestMethod]
		public void MiddleClickButton()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "ClickButton");
				browser.NavigateTo(TestSite + "/main.html");
				var button = browser.First("button");
				button.MiddleClick();
				browser.WaitForComplete(150);

				// Middle click may not click but does set focus.
				Assert.IsTrue(button.Focused);
				Mouse.LeftClick(button.Location);
			});
		}

		[TestMethod]
		public void NavigateThenRunJavascript()
		{
			ForEachBrowser(browser =>
			{
				browser.NavigateTo(TestSite + "/main.html");

				var actual = browser.ExecuteScript("TestR.runScript('document.toString()')");
				Assert.IsTrue(actual.Contains("undefined"));

				actual = browser.ExecuteScript("document.title");
				Assert.AreEqual("Index", actual);
			});
		}

		[TestMethod]
		public void NavigateTo()
		{
			ForEachBrowser(browser =>
			{
				var expected = TestSite + "/main.html";
				browser.NavigateTo(expected);
				Assert.AreEqual(expected, browser.Uri.ToLower());
			});
		}

		[TestMethod]
		public void NavigateToFromHttpToHttps()
		{
			ForEachBrowser(browser =>
			{
				var expected = TestSite.Replace("https://", "http://") + "/main.html";
				Assert.IsFalse(expected.StartsWith("https://"));
				browser.NavigateTo(expected);
				Assert.IsTrue(browser.Uri.StartsWith("https://"));
				Assert.AreEqual($"{TestSite}/main.html", browser.Uri.ToLower());
			});
		}

		[TestMethod]
		public void NavigateToFromHttpToHttpsWhenAlreadyOnExpectedUri()
		{
			ForEachBrowser(browser =>
			{
				browser.NavigateTo($"{TestSite}/main.html");
				var expected = TestSite.Replace("https://", "http://") + "/main.html";
				Assert.IsFalse(expected.StartsWith("https://"));
				browser.NavigateTo(expected);
				Assert.IsTrue(browser.Uri.StartsWith("https://"));
				Assert.AreEqual($"{TestSite}/main.html", browser.Uri.ToLower());
			});
		}

		[TestMethod]
		public void NavigateToSameUri()
		{
			ForEachBrowser(browser =>
			{
				var expected = TestSite + "/main.html";
				browser.NavigateTo(expected);
				browser.NavigateTo(expected);
				browser.NavigateTo(expected);
				Assert.AreEqual(expected, browser.Uri.ToLower());
			});
		}

		[TestMethod]
		public void NavigateToSameUriWithEndingForwardSlash()
		{
			ForEachBrowser(browser =>
			{
				var expected = TestSite + "/";
				browser.NavigateTo(expected);
				browser.NavigateTo(expected);
				browser.NavigateTo(expected);
				Assert.AreEqual($"{expected}", browser.Uri.ToLower());
			});
		}

		[TestMethod]
		public void NavigateToWithDifferentFinalUri()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "NavigateToWithDifferentFinalUri");
				browser.NavigateTo(TestSite + "/Angular.html");
				Assert.AreEqual(TestSite + "/Angular.html#!/", browser.Uri);
			});
		}

		[TestMethod]
		public void RawHtml()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "RawHtml");
				browser.NavigateTo(TestSite + "/Jquery.html");
				var test = browser.RawHtml.Trim();
				Assert.IsTrue(test.StartsWith("<html"));
				Assert.IsTrue(test.EndsWith("</html>"));
			});
		}

		[TestMethod]
		public void RedirectByLink()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "RedirectByLink");
				var expected = TestSite + "/main.html";
				browser.NavigateTo(expected);
				Assert.AreEqual(expected, browser.Uri.ToLower());

				// Redirect by the link.
				browser.First<Link>("redirectLink").Click();
				browser.WaitForNavigation(TestSite + "/Inputs.html");

				expected = TestSite + "/inputs.html";
				Assert.AreEqual(expected, browser.Uri.ToLower());
			});
		}

		[TestMethod]
		public void RedirectByLinkWithoutProvidingExpectedUri()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "RedirectByLink");
				var expected = TestSite + "/main.html";
				browser.NavigateTo(expected);
				Assert.AreEqual(expected, browser.Uri.ToLower());

				// Redirect by the link.
				browser.First<Link>("redirectLink").Click();
				browser.WaitForNavigation();

				expected = TestSite + "/inputs.html";
				Assert.AreEqual(expected, browser.Uri.ToLower());
			});
		}

		[TestMethod]
		public void RedirectByScript()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "RedirectByScript");
				var expected = TestSite + "/main.html";
				browser.NavigateTo(expected);
				Assert.AreEqual(expected, browser.Uri.ToLower());
				Assert.IsNotNull(browser.First("link"), "Failed to find the link element.");

				// Redirect by a script.
				browser.ExecuteScript("window.location.href = 'inputs.html'");
				browser.WaitForNavigation(TestSite + "/inputs.html");

				expected = TestSite + "/inputs.html";
				Assert.AreEqual(expected, browser.Uri.ToLower());
			});
		}

		[TestMethod]
		public void RedirectByScriptWithoutProvidedExpectedUri()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "RedirectByScript");
				var expected = TestSite + "/main.html";
				browser.NavigateTo(expected);
				Assert.AreEqual(expected, browser.Uri.ToLower());
				Assert.IsNotNull(browser.First("link"), "Failed to find the link element.");

				// Redirect by a script.
				browser.ExecuteScript("setTimeout(function() {window.location.href = 'inputs.html'}, 1000)");
				browser.WaitForNavigation();

				expected = TestSite + "/inputs.html";
				Assert.AreEqual(expected, browser.Uri.ToLower());
			});
		}

		[TestMethod]
		public void Refresh()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "Refresh");
				var expected = TestSite + "/angular.html";
				browser.NavigateTo(expected);
				Assert.AreEqual(33, browser.Descendants().Count());

				browser.First("addItem").Click();
				Assert.AreEqual(33, browser.Descendants().Count());

				var result = browser.Wait(x =>
				{
					x.Refresh();
					return x.Descendants().Count() == 34;
				});
				Assert.IsTrue(result, "The count never incremented.");
			});
		}

		[TestMethod]
		public void RightClickButton()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "ClickButton");
				browser.NavigateTo(TestSite + "/main.html");
				var button = browser.First("button");
				button.RightClick();

				var actual = browser.First<TextArea>("textarea").Text;
				Assert.AreEqual("Text Area's \"Quotes\" Data", actual);

				var location = button.Location;
				Mouse.MoveTo(location.X + 60, location.Y + 22);

				var result = Utility.Wait(() => DesktopElement.FromCursor()?.TypeName == "menu item");
				Assert.IsTrue(result, "Failed to find menu.");

				Mouse.LeftClick(button.Location);
			});
		}

		[TestMethod]
		public void ScrollIntoView()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "ClickButton");
				browser.NavigateTo(TestSite + "/main.html");
				browser.MoveWindow(0, 0, 300, 300);
				browser.BringToFront();

				var button = browser.First<WebElement>("button");
				button.ScrollIntoView();
				Assert.IsTrue(button.Location.X < 100);
				Assert.IsTrue(button.Location.Y < 100);
			});
		}

		[TestMethod]
		public void SelectSelectedOption()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "SelectSelectedOption");
				browser.NavigateTo(TestSite + "/main.html");
				var select = browser.First<Select>("select");
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
				//LogManager.UpdateReferenceId(browser, "SetButtonText");
				browser.NavigateTo(TestSite + "/main.html");
				browser.First<Button>("button").Text = "Hello";

				var actual = browser.First<Button>("button").GetAttributeValue("textContent", true);
				Assert.AreEqual("Hello", actual);
			});
		}

		[TestMethod]
		public void SetInputButtonText()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "SetButtonText");
				browser.NavigateTo(TestSite + "/main.html");
				browser.First<Button>("inputButton").Text = "Hello";

				var actual = browser.First<Button>("inputButton").GetAttributeValue("value", true);
				Assert.AreEqual("Hello", actual);
			});
		}

		[TestMethod]
		public void SetSelectText()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "TypeTextPasswordInput");
				browser.NavigateTo(TestSite + "/main.html");
				var select = browser.First<Select>("select");
				select.Text = "One";
				Assert.AreEqual("One", select.Text);
				Assert.AreEqual("1", select.Value);

				var text = browser.First<TextInput>("text");
				Assert.AreEqual("1", text.Value);
			});
		}

		[TestMethod]
		public void SetTextAllInputs()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "SetTextAllInputs");
				browser.NavigateTo(TestSite + "/inputs.html");

				foreach (var input in browser.Descendants<TextInput>())
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

				foreach (var input in browser.Descendants<TextArea>())
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
				//LogManager.UpdateReferenceId(browser, "TestContentForInputText");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.First<TextInput>("inputText1");
				Assert.AreEqual(element.Text, "inputText1");
			});
		}

		[TestMethod]
		public void TextAreaValue()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "TextAreaValue");
				browser.NavigateTo(TestSite + "/main.html");

				var element = browser.First<TextArea>("textarea");
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
				//LogManager.UpdateReferenceId(browser, "TestContentForDiv");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.First<Division>("div1");
				Assert.AreEqual(element.Text, "\n\t\t\tDiv - Span\n\t\t\tOther Text\n\t\t");
			});
		}

		[TestMethod]
		public void TextContentForDivWithChildren()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "TestContentForHeader1");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.First<Division>("div2");
				Assert.AreEqual(element.Text, "\n\t\t\tHeader One\n\t\t\tHeader Two\n\t\t\tHeader Three\n\t\t\tHeader Four\n\t\t\tHeader Five\n\t\t");
			});
		}

		[TestMethod]
		public void TextContentForHeader1()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "TestContentForHeader1");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.First<Header>("h1");
				Assert.AreEqual(element.Text, "Header One");
			});
		}

		[TestMethod]
		public void TextContentForHeader2()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "TestContentForHeader2");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.First<Header>("h2");
				Assert.AreEqual(element.Text, "Header Two");
			});
		}

		[TestMethod]
		public void TextContentForHeader3()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "TestContentForHeader3");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.First<Header>("h3");
				Assert.AreEqual(element.Text, "Header Three");
			});
		}

		[TestMethod]
		public void TextContentForHeader4()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "TestContentForHeader4");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.First<Header>("h4");
				Assert.AreEqual(element.Text, "Header Four");
			});
		}

		[TestMethod]
		public void TextContentForHeader5()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "TestContentForHeader5");
				browser.NavigateTo(TestSite + "/TextContent.html");

				var element = browser.First<Header>("h5");
				Assert.AreEqual(element.Text, "Header Five");
			});
		}

		[TestMethod]
		public void TypeTextAllInputs()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "TypeTextAllInputs");
				browser.NavigateTo(TestSite + "/inputs.html");

				foreach (var input in browser.Descendants<TextInput>())
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

				foreach (var input in browser.Descendants<TextArea>())
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
				//LogManager.UpdateReferenceId(browser, "TypeTextAppendInput");
				browser.NavigateTo(TestSite + "/inputs.html");
				var input = browser.First<TextInput>("text");
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
				//LogManager.UpdateReferenceId(browser, "TypeTextPasswordInput");
				browser.NavigateTo(TestSite + "/inputs.html");
				var input = browser.First<TextInput>("password");
				input.TypeText("password", true);
				Assert.AreEqual("password", input.Value);
			});
		}

		[TestMethod]
		public void TypeTextSelectInput()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "TypeTextSelectInput");
				browser.NavigateTo(TestSite + "/main.html");
				var select = browser.First<Select>("select");
				select.TypeText("O");
				Assert.AreEqual("One", select.Text);
				Assert.AreEqual("1", select.Value);
			});
		}

		[TestMethod]
		public void TypeTextSetInput()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "TypeTextSetInput");
				browser.NavigateTo(TestSite + "/inputs.html");
				var input = browser.First<TextInput>("text");
				input.Value = "foo";
				input.TypeText("bar", true);
				Assert.AreEqual("bar", input.Value);
			});
		}

		[TestMethod]
		public void TypeTextUsingKeyboard()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "TypeTextSetInput");
				browser.NavigateTo(TestSite + "/inputs.html");
				var input = browser.First("text");
				input.TypeText("bar");
				Assert.AreEqual("bar", ((TextInput) input).Value);
			});
		}

		[TestMethod]
		public void WebElementRefresh()
		{
			ForEachBrowser(browser =>
			{
				//LogManager.UpdateReferenceId(browser, "AngularNewElements");
				browser.NavigateTo(TestSite + "/Angular.html#!/");

				var items = browser.First<Division>("items");
				Assert.AreEqual(0, items.Children.Count);

				browser.First<Button>("addItem").Click();

				var result = items.Wait(x => x.Refresh().Children.Count == 1);
				Assert.IsTrue(result, "Items was not added");
				Assert.AreEqual(1, items.Children.Count);
				Assert.AreEqual("items-0", items.Children[0].Id);
			});
		}

		private void ForEachBrowser(Action<Browser> action)
		{
			BrowserType = BrowserType.All;
			base.ForEachBrowser(action);
		}

		#endregion
	}
}