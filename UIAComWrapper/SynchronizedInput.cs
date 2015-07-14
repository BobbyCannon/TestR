// (c) Copyright Microsoft Corporation, 2010.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

#region References

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UIAutomationClient;

#endregion

namespace UIAComWrapper
{
	public class SynchronizedInputPattern : BasePattern
	{
		#region Fields

		public static readonly AutomationEvent InputDiscardedEvent = SynchronizedInputPatternIdentifiers.InputDiscardedEvent;
		public static readonly AutomationEvent InputReachedOtherElementEvent = SynchronizedInputPatternIdentifiers.InputReachedOtherElementEvent;
		public static readonly AutomationEvent InputReachedTargetEvent = SynchronizedInputPatternIdentifiers.InputReachedTargetEvent;
		public static readonly AutomationPattern Pattern = SynchronizedInputPatternIdentifiers.Pattern;
		private readonly IUIAutomationSynchronizedInputPattern _pattern;

		#endregion

		#region Constructors

		private SynchronizedInputPattern(AutomationElement el, IUIAutomationSynchronizedInputPattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Methods

		public void Cancel()
		{
			try
			{
				_pattern.Cancel();
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

		public void StartListening(SynchronizedInputType type)
		{
			try
			{
				_pattern.StartListening((UIAutomationClient.SynchronizedInputType) type);
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
			return (pattern == null) ? null : new SynchronizedInputPattern(el, (IUIAutomationSynchronizedInputPattern) pattern, cached);
		}

		#endregion
	}
}