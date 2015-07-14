#region References

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Automation.Patterns
{
	public class DockPattern : BasePattern
	{
		#region Fields

		public static readonly AutomationProperty DockPositionProperty = DockPatternIdentifiers.DockPositionProperty;
		public static readonly AutomationPattern Pattern = DockPatternIdentifiers.Pattern;
		private readonly IUIAutomationDockPattern _pattern;

		#endregion

		#region Constructors

		private DockPattern(AutomationElement el, IUIAutomationDockPattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Properties

		public DockPatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new DockPatternInformation(_el, true);
			}
		}

		public DockPatternInformation Current
		{
			get { return new DockPatternInformation(_el, false); }
		}

		#endregion

		#region Methods

		public void SetDockPosition(DockPosition dockPosition)
		{
			try
			{
				_pattern.SetDockPosition((UIAutomationClient.DockPosition) dockPosition);
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
			return (pattern == null) ? null : new DockPattern(el, (IUIAutomationDockPattern) pattern, cached);
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct DockPatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal DockPatternInformation(AutomationElement element, bool isCached)
			{
				_el = element;
				_isCached = isCached;
			}

			#endregion

			#region Properties

			public DockPosition DockPosition
			{
				get { return (DockPosition) _el.GetPropertyValue(DockPositionProperty, _isCached); }
			}

			#endregion
		}

		#endregion
	}
}