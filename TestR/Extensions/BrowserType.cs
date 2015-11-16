#region References

using TestR.Web;

#endregion

namespace TestR.Extensions
{
	public static partial class Helper
	{
		#region Methods

		/// <summary>
		/// Returns the number of browsers for this type.
		/// </summary>
		/// <param name="type"> The browser type that contains the configuration. </param>
		/// <returns> The number of browsers configured in the type. </returns>
		public static int Count(this BrowserType type)
		{
			var response = 0;

			if ((type & BrowserType.Chrome) == BrowserType.Chrome)
			{
				response++;
			}

			//if ((type & BrowserType.Edge) == BrowserType.Edge)
			//{
			//	response++;
			//}

			if ((type & BrowserType.InternetExplorer) == BrowserType.InternetExplorer)
			{
				response++;
			}

			if ((type & BrowserType.Firefox) == BrowserType.Firefox)
			{
				response++;
			}

			return response;
		}

		#endregion
	}
}