#region References

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Automation.Patterns
{
	public class InvokePattern : BasePattern
	{
		#region Fields

		public static readonly AutomationEvent InvokedEvent = InvokePatternIdentifiers.InvokedEvent;
		public static readonly AutomationPattern Pattern = InvokePatternIdentifiers.Pattern;
		private readonly IUIAutomationInvokePattern _pattern;

		#endregion

		#region Constructors

		private InvokePattern(AutomationElement el, IUIAutomationInvokePattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Methods

		public void Invoke()
		{
			try
			{
				_pattern.Invoke();
			}
			catch (COMException e)
			{
				Exception newEx;
				if (Utility.ConvertException(e, out newEx))
				{
					throw newEx;
				}
				throw;
			}
		}

		internal static object Wrap(AutomationElement el, object pattern, bool cached)
		{
			return (pattern == null) ? null : new InvokePattern(el, (IUIAutomationInvokePattern) pattern, cached);
		}

		#endregion
	}
}