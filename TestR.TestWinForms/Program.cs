#region References

using System;
using FormApplication = System.Windows.Forms.Application;

#endregion

namespace TestR.TestWinForms
{
	internal static class Program
	{
		#region Fields

		private static ParentForm _parentForm;

		#endregion

		#region Methods

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			FormApplication.EnableVisualStyles();
			FormApplication.SetCompatibleTextRenderingDefault(false);
			FormApplication.Run(_parentForm = new ParentForm());
		}

		#endregion
	}
}