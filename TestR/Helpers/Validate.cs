#region References

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TestR.Helpers
{
	/// <summary>
	/// The validate class contains methods to help validate your test.
	/// </summary>
	public static class Validate
	{
		#region Methods

		/// <summary>
		/// Checks to see if both list contain the same items but ignores order.
		/// </summary>
		/// <param name="expected"> The list of expected items. </param>
		/// <param name="actual"> The list of actual items. </param>
		/// <typeparam name="T"> The type of the item in the lists. </typeparam>
		public static void AllExists<T>(IList<T> expected, IList<T> actual)
		{
			var builder = new StringBuilder();
			foreach (var item in expected.Except(actual))
			{
				builder.AppendLine("Missing [" + item + "] in actual collection.");
			}

			if (builder.Length > 0)
			{
				Fail(builder.ToString());
			}
		}

		/// <summary>
		/// Checks to see if two items are equal. If not then throws a failure exception.
		/// </summary>
		/// <param name="item1"> The first item to test. </param>
		/// <param name="item2"> The second item to test. </param>
		/// <param name="message"> The failure message. </param>
		public static void Equals<T>(T item1, T item2, string message)
		{
			if (!item1.Equals(item2))
			{
				// todo: Create custom exception type...
				throw new Exception(message);
			}
		}

		/// <summary>
		/// Ensures the correct exception is thrown.
		/// </summary>
		/// <param name="action"> The action that should throw the expected exception. </param>
		/// <param name="message"> The message to be contained in the exception. </param>
		/// <typeparam name="T"> The type of the exception to be thrown. </typeparam>
		public static void ExpectedException<T>(Action action, string message = "")
			where T : Exception
		{
			try
			{
				action();
				Fail("The expected exception was not thrown.");
			}
			catch (T ex)
			{
				if (!ex.Message.Contains(message))
				{
					var error = "The expected exception was thrown but did not contain the expected message.";
					Fail("{0}{1}Expected: {2}{1}Actual: {3}", error, Environment.NewLine, message, ex.Message);
				}
			}
		}

		/// <summary>
		/// Fail with an exception.
		/// </summary>
		/// <param name="message"> The message for the failure exception. </param>
		public static void Fail(string message)
		{
			throw new Exception(message);
		}

		/// <summary>
		/// Fail with a formatted exception.
		/// </summary>
		/// <param name="format"> The formatted message. </param>
		/// <param name="parameters"> The parameters for the message. </param>
		public static void Fail(string format, params object[] parameters)
		{
			throw new Exception(string.Format(format, parameters));
		}

		/// <summary>
		/// Checks to see if an item is null. If it's null then throws a failure exception.
		/// </summary>
		/// <param name="item"> The item to test. </param>
		/// <param name="message"> The failure message. </param>
		public static void IsNotNull(object item, string message)
		{
			if (item == null)
			{
				// todo: Create custom exception type...
				throw new Exception(message);
			}
		}

		#endregion
	}
}