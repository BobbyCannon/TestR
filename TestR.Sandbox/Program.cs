#region References

using System;
using TestR.Extension;

#endregion

namespace TestR.Sandbox
{
	internal class Program
	{
		#region Methods

		[STAThread]
		private static void Main(string[] args)
		{
			ExtensionWindow.TestWindow();
			//Console.WriteLine("Press any key to exit...");
			//Console.ReadKey();
		}

		#endregion
	}
}