#region References

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Windows.Forms;
using TestR.Extensions;
using TestR.Web;
using TestR.Web.Browsers;

#endregion

namespace TestR.PowerShell
{
	public abstract class BrowserTestCmdlet : TestCmdlet
	{
		#region Constructors

		protected BrowserTestCmdlet()
		{
			BrowserType = BrowserType.All;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the browser type to run each test with. You can specify a single browser, combination, or all.
		/// </summary>
		[Parameter]
		public BrowserType BrowserType { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Run a test against each browser. BrowserType property will determine which browsers to run the test against.
		/// </summary>
		/// <param name="action"> The action to run each browser against. </param>
		/// <param name="useSecondaryMonitor"> The flag to determine to attempt to use secondary monitor. </param>
		/// <seealso cref="BrowserType" />
		public void ForEachBrowser(Action<Browser> action, bool useSecondaryMonitor = true)
		{
			var screen = Screen.AllScreens.FirstOrDefault(x => x.Primary == false) ?? Screen.AllScreens.First(x => x.Primary);
			var browserOffset = 0;
			var browserWidth = screen.WorkingArea.Width / BrowserType.Count();
			
			Browser.ForEachBrowser(x =>
			{
				try
				{
					x.MoveWindow(screen.WorkingArea.Left + (browserOffset++ * browserWidth), 0, browserWidth, browserWidth * 2);
					x.BringToFront();
					action(x);
				}
				catch (Exception ex)
				{
					throw new Exception("Test failed using " + x.GetType().Name + ".", ex);
				}
			}, BrowserType);
		}
		
		#endregion
	}
}