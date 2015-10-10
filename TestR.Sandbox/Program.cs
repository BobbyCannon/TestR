#region References

using System;
using System.Linq;
using TestR.Desktop;
using TestR.Web;

#endregion

namespace TestR.Sandbox
{
	internal class Program
	{
		#region Methods

		[STAThread]
		private static void Main(string[] args)
		{
			foreach (var browser in Browser.AttachOrCreate(BrowserType.Chrome))
			{
				using (browser)
				{
					browser.NavigateTo("http://google.com");
					
					var input = browser.Elements["lst-ib"];
					input.Text = "Bobby Cannon";

					var button = browser.Elements.First(x => x.Text == "Google Search");
					button["style"] = "color: red;";
					button.Click();
				}
			}

			//Console.WriteLine("Press any key to exit...");
			//Console.ReadKey();
		}

		#endregion
	}
}