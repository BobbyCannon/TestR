﻿#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a spinner element.
	/// </summary>
	public class Spinner : DesktopElement
	{
		#region Constructors

		internal Spinner(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}