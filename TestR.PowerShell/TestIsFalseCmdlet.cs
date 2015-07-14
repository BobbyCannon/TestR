#region References

using System;
using System.Management.Automation;

#endregion

namespace TestR.PowerShell
{
	[Cmdlet(VerbsDiagnostic.Test, "IsFalse")]
	public class TestIsFalseCmdlet : Cmdlet
	{
		#region Properties

		[Parameter(Mandatory = true)]
		public bool Condition { get; set; }

		[Parameter(Mandatory = true)]
		public string Message { get; set; }

		#endregion

		#region Methods

		protected override void ProcessRecord()
		{
			if (Condition)
			{
				WriteError(new ErrorRecord(new Exception(Message), "-1", ErrorCategory.InvalidResult, this));
			}

			base.ProcessRecord();
		}

		#endregion
	}
}