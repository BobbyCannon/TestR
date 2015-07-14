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
	public class MultipleViewPattern : BasePattern
	{
		#region Fields

		public static readonly AutomationProperty CurrentViewProperty = MultipleViewPatternIdentifiers.CurrentViewProperty;
		public static readonly AutomationPattern Pattern = MultipleViewPatternIdentifiers.Pattern;
		public static readonly AutomationProperty SupportedViewsProperty = MultipleViewPatternIdentifiers.SupportedViewsProperty;
		private readonly IUIAutomationMultipleViewPattern _pattern;

		#endregion

		#region Constructors

		private MultipleViewPattern(AutomationElement el, IUIAutomationMultipleViewPattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Properties

		public MultipleViewPatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new MultipleViewPatternInformation(_el, true);
			}
		}

		public MultipleViewPatternInformation Current
		{
			get { return new MultipleViewPatternInformation(_el, false); }
		}

		#endregion

		#region Methods

		public string GetViewName(int viewId)
		{
			try
			{
				return _pattern.GetViewName(viewId);
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

		public void SetCurrentView(int viewId)
		{
			try
			{
				_pattern.SetCurrentView(viewId);
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
			return (pattern == null) ? null : new MultipleViewPattern(el, (IUIAutomationMultipleViewPattern) pattern, cached);
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct MultipleViewPatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal MultipleViewPatternInformation(AutomationElement element, bool isCached)
			{
				_el = element;
				_isCached = isCached;
			}

			#endregion

			#region Properties

			public int CurrentView
			{
				get { return (int) _el.GetPropertyValue(CurrentViewProperty, _isCached); }
			}

			#endregion

			#region Methods

			public int[] GetSupportedViews()
			{
				return (int[]) _el.GetPropertyValue(SupportedViewsProperty, _isCached);
			}

			#endregion
		}

		#endregion
	}
}