#region References

using System.IO;
using System.Reflection;

#endregion

namespace TestR.AutomationTests.Native
{
	public abstract class BaseTest
	{
		#region Constructors

		protected BaseTest()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var path = Path.GetDirectoryName(assembly.Location);
			var info = new DirectoryInfo(path ?? "/");

			SolutionPath = info.Parent?.Parent?.Parent?.FullName;
		}

		#endregion

		#region Properties

		public string SolutionPath { get; }

		#endregion
	}
}