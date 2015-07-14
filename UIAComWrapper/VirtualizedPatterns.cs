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
	public class ItemContainerPattern : BasePattern
	{
		#region Fields

		public static readonly AutomationPattern Pattern = ItemContainerPatternIdentifiers.Pattern;
		private readonly IUIAutomationItemContainerPattern _pattern;

		#endregion

		#region Constructors

		private ItemContainerPattern(AutomationElement el, IUIAutomationItemContainerPattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Methods

		public AutomationElement FindItemByProperty(AutomationElement startAfter, AutomationProperty property, object value)
		{
			try
			{
				return AutomationElement.Wrap(
					_pattern.FindItemByProperty(
						(startAfter == null) ? null : startAfter.NativeElement,
						(property == null) ? 0 : property.Id,
						Utility.UnwrapObject(value)));
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
			return (pattern == null) ? null : new ItemContainerPattern(el, (IUIAutomationItemContainerPattern) pattern, cached);
		}

		#endregion
	}

	public class VirtualizedItemPattern : BasePattern
	{
		#region Fields

		public static readonly AutomationPattern Pattern = VirtualizedItemPatternIdentifiers.Pattern;
		private readonly IUIAutomationVirtualizedItemPattern _pattern;

		#endregion

		#region Constructors

		private VirtualizedItemPattern(AutomationElement el, IUIAutomationVirtualizedItemPattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Methods

		public void Realize()
		{
			try
			{
				_pattern.Realize();
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
			return (pattern == null) ? null : new VirtualizedItemPattern(el, (IUIAutomationVirtualizedItemPattern) pattern, cached);
		}

		#endregion
	}
}