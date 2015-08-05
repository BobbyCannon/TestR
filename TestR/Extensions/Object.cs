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
		/// Dumps the object to Console.WriteLine.
		/// </summary>
		/// <param name="value"> </param>
		public static void Dump(this object value)
		{
			Console.WriteLine(value.ToString());
		}

		#endregion
	}
}