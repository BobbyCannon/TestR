#region References

using System;

#endregion

namespace TestR.Extensions
{
	/// <summary>
	/// Represents a static helper class.
	/// </summary>
	public static partial class Helper
	{
		#region Methods

		/// <summary>
		/// Dumps the object to the console using the ToString method.
		/// </summary>
		/// <param name="value"> The value to output to the console. </param>
		public static void Dump(this object value)
		{
			Console.WriteLine(value.ToString());
		}

		#endregion
	}
}