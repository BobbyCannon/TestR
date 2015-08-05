#region References

using System;

#endregion

namespace TestR.Web
{
	/// <summary>
	/// The type of the browser.
	/// </summary>
	[Flags]
	public enum BrowserType
	{
		/// <summary>
		/// Represents a Chrome browser.
		/// </summary>
		Chrome = 0x01,

		/// <summary>
		/// Represents an Internet Explorer browser.
		/// </summary>
		InternetExplorer = 0x02,

		/// <summary>
		/// Represents a Firefox browser.
		/// </summary>
		Firefox = 0x04,

		/// <summary>
		/// Represents an Edge browser.
		/// </summary>
		Edge = 0x08,

		/// <summary>
		/// Represents all browser types.
		/// </summary>
		All = 0xFF
	}
}