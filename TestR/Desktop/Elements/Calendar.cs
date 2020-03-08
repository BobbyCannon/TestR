﻿#region References

using Interop.UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a calendar element.
	/// </summary>
	public class Calendar : DesktopElement
	{
		#region Constructors

		internal Calendar(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}