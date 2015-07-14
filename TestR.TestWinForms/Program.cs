#region References

using System;
using System.Windows.Forms;

#endregion

namespace TestR.TestWinForms
{
	internal static class Program
	{
		#region Methods

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FormMain());
		}

		#endregion
	}
}