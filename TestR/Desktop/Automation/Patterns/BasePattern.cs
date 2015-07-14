#region References

using System;
using System.Diagnostics;

#endregion

namespace TestR.Desktop.Automation.Patterns
{
	public abstract class BasePattern : AutomationPattern
	{
		#region Fields

		internal bool _cached;
		internal AutomationElement _el;

		#endregion

		#region Constructors

		internal BasePattern(AutomationElement el, bool cached, int id, Guid guid, string programmaticName)
			: base(id, guid, programmaticName)
		{
			Debug.Assert(el != null);
			_el = el;
			_cached = cached;
		}

		#endregion
	}
}