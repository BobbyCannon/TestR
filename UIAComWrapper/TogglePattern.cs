// (c) Copyright Microsoft Corporation.
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
	public class TogglePattern : BasePattern
	{
		#region Fields

		public static readonly AutomationPattern Pattern = TogglePatternIdentifiers.Pattern;
		public static readonly AutomationProperty ToggleStateProperty = TogglePatternIdentifiers.ToggleStateProperty;
		private readonly IUIAutomationTogglePattern _pattern;

		#endregion

		#region Constructors

		private TogglePattern(AutomationElement el, IUIAutomationTogglePattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Properties

		public TogglePatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new TogglePatternInformation(_el, true);
			}
		}

		public TogglePatternInformation Current
		{
			get { return new TogglePatternInformation(_el, false); }
		}

		#endregion

		#region Methods

		public void Toggle()
		{
			try
			{
				_pattern.Toggle();
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
			return (pattern == null) ? null : new TogglePattern(el, (IUIAutomationTogglePattern) pattern, cached);
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct TogglePatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal TogglePatternInformation(AutomationElement element, bool isCached)
			{
				_el = element;
				_isCached = isCached;
			}

			#endregion

			#region Properties

			public ToggleState ToggleState
			{
				get { return (ToggleState) _el.GetPropertyValue(ToggleStateProperty, _isCached); }
			}

			#endregion
		}

		#endregion
	}
}