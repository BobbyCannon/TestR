﻿#region References

using Interop.UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the menu bar for a window.
	/// </summary>
	public class MenuBar : DesktopElement
	{
		#region Constructors

		internal MenuBar(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}