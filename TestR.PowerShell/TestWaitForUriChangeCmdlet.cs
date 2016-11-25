#region References

using System;
using System.Management.Automation;
using TestR.Web;

#endregion

namespace TestR.PowerShell
{
	[Cmdlet(VerbsDiagnostic.Test, "WaitForUriChange")]
	public class TestWaitForUriChangeCmdlet : Cmdlet
	{
		#region Constructors

		public TestWaitForUriChangeCmdlet()
		{
			Timeout = 1000;
			Delay = 25;
		}

		#endregion

		#region Properties

		[Parameter(Mandatory = true)]
		public Browser Browser { get; set; }

		[Parameter(Mandatory = false)]
		public int Delay { get; set; }

		[Parameter(Mandatory = false)]
		public string OriginalUri { get; set; }

		[Parameter(Mandatory = false)]
		public int Timeout { get; set; }

		#endregion

		#region Methods

		protected override void ProcessRecord()
		{
			var originalUri = OriginalUri ?? Browser.Uri;
			if (Utility.Wait(() => Browser.Uri != originalUri, Timeout, Delay))
			{
				WriteError(new ErrorRecord(new Exception("The browser failed to change its URI."), "-1", ErrorCategory.InvalidResult, this));
			}

			base.ProcessRecord();
		}

		#endregion
	}
}