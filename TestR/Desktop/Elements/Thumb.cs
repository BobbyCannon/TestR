﻿#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the thumb for a window.
	/// </summary>
	public class Thumb : DesktopElement
	{
		#region Constructors

		internal Thumb(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}