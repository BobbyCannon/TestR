#region References

using System;
using System.Management.Automation;
using TestR.Web;

#endregion

namespace TestR.PowerShell
{
	[Cmdlet(VerbsDiagnostic.Test, "WaitForUriToEqual")]
	public class TestWaitForUriToEqual : Cmdlet
	{
		#region Constructors

		public TestWaitForUriToEqual()
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

		[Parameter(Mandatory = true)]
		public string ExpectedUri { get; set; }

		[Parameter(Mandatory = false)]
		public int Timeout { get; set; }

		#endregion

		#region Methods

		protected override void ProcessRecord()
		{
			if (Utility.Wait(() => Browser.Uri == ExpectedUri, Timeout, Delay))
			{
				WriteError(new ErrorRecord(new Exception("Failed to navigate to the expected URI."), "-1", ErrorCategory.InvalidResult, this));
			}

			base.ProcessRecord();
		}

		#endregion
	}
}