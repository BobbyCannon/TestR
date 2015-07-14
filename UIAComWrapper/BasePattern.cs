// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

#region References

using System;
using System.Diagnostics;

#endregion

namespace UIAComWrapper
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