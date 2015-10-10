#region References

using System;
using System.Management.Automation;
using TestR.Web;

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
		/// <seealso cref="BrowserType" />
		public void ForEachBrowser(Action<Browser> action)
		{
			var browserOffset = 0;
			var browserWidth = 500;

			Browser.ForEachBrowser(x =>
			{
				try
				{
					x.Application.MoveWindow((browserOffset++ * browserWidth), 0, browserWidth, browserWidth * 2);
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