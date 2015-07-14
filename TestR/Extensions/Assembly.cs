#region References

using System;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace TestR.Extensions
{
	public static partial class Helper
	{
		#region Methods

		/// <summary>
		/// Checks to see if the assembly passed in is a debug build.
		/// </summary>
		/// <param name="assembly"> The assembly to test. </param>
		/// <returns> True if is a debug build and false if a release build. </returns>
		public static bool IsAssemblyDebugBuild(this Assembly assembly)
		{
			var retVal = false;

			foreach (var att in assembly.GetCustomAttributes(false))
			{
				if (att.GetType() == Type.GetType("System.Diagnostics.DebuggableAttribute"))
				{
					retVal = ((DebuggableAttribute) att).IsJITTrackingEnabled;
				}
			}

			return retVal;
		}

		#endregion
	}
}