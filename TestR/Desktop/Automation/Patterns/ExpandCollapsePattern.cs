#region References

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Automation.Patterns
{
	public class ExpandCollapsePattern : BasePattern
	{
		#region Fields

		public static readonly AutomationProperty ExpandCollapseStateProperty = ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty;
		public static readonly AutomationPattern Pattern = ExpandCollapsePatternIdentifiers.Pattern;
		private readonly IUIAutomationExpandCollapsePattern _pattern;

		#endregion

		#region Constructors

		private ExpandCollapsePattern(AutomationElement el, IUIAutomationExpandCollapsePattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Properties

		public ExpandCollapsePatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new ExpandCollapsePatternInformation(_el, true);
			}
		}

		public ExpandCollapsePatternInformation Current
		{
			get { return new ExpandCollapsePatternInformation(_el, false); }
		}

		#endregion

		#region Methods

		public void Collapse()
		{
			try
			{
				_pattern.Collapse();
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

		public void Expand()
		{
			try
			{
				_pattern.Expand();
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
			return (pattern == null) ? null : new ExpandCollapsePattern(el, (IUIAutomationExpandCollapsePattern) pattern, cached);
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct ExpandCollapsePatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal ExpandCollapsePatternInformation(AutomationElement element, bool isCached)
			{
				_el = element;
				_isCached = isCached;
			}

			#endregion

			#region Properties

			public ExpandCollapseState ExpandCollapseState
			{
				get { return (ExpandCollapseState) _el.GetPropertyValue(ExpandCollapseStateProperty, _isCached); }
			}

			#endregion
		}

		#endregion
	}
}