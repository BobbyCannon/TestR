#region References

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Automation.Patterns
{
	public class ScrollPattern : BasePattern
	{
		#region Constants

		public const double NoScroll = -1.0;

		#endregion

		#region Fields

		public static readonly AutomationProperty HorizontallyScrollableProperty = ScrollPatternIdentifiers.HorizontallyScrollableProperty;
		public static readonly AutomationProperty HorizontalScrollPercentProperty = ScrollPatternIdentifiers.HorizontalScrollPercentProperty;
		public static readonly AutomationProperty HorizontalViewSizeProperty = ScrollPatternIdentifiers.HorizontalViewSizeProperty;
		public static readonly AutomationPattern Pattern = ScrollPatternIdentifiers.Pattern;
		public static readonly AutomationProperty VerticallyScrollableProperty = ScrollPatternIdentifiers.VerticallyScrollableProperty;
		public static readonly AutomationProperty VerticalScrollPercentProperty = ScrollPatternIdentifiers.VerticalScrollPercentProperty;
		public static readonly AutomationProperty VerticalViewSizeProperty = ScrollPatternIdentifiers.VerticalViewSizeProperty;
		private readonly IUIAutomationScrollPattern _pattern;

		#endregion

		#region Constructors

		private ScrollPattern(AutomationElement el, IUIAutomationScrollPattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Properties

		public ScrollPatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new ScrollPatternInformation(_el, true);
			}
		}

		public ScrollPatternInformation Current
		{
			get { return new ScrollPatternInformation(_el, false); }
		}

		#endregion

		#region Methods

		public void Scroll(ScrollAmount horizontalAmount, ScrollAmount verticalAmount)
		{
			try
			{
				_pattern.Scroll((UIAutomationClient.ScrollAmount) horizontalAmount, (UIAutomationClient.ScrollAmount) verticalAmount);
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

		public void ScrollHorizontal(ScrollAmount amount)
		{
			try
			{
				_pattern.Scroll((UIAutomationClient.ScrollAmount) amount, UIAutomationClient.ScrollAmount.ScrollAmount_NoAmount);
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

		public void ScrollVertical(ScrollAmount amount)
		{
			try
			{
				_pattern.Scroll(UIAutomationClient.ScrollAmount.ScrollAmount_NoAmount, (UIAutomationClient.ScrollAmount) amount);
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

		public void SetScrollPercent(double horizontalPercent, double verticalPercent)
		{
			try
			{
				_pattern.SetScrollPercent(horizontalPercent, verticalPercent);
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
			return (pattern == null) ? null : new ScrollPattern(el, (IUIAutomationScrollPattern) pattern, cached);
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct ScrollPatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal ScrollPatternInformation(AutomationElement element, bool isCached)
			{
				_el = element;
				_isCached = isCached;
			}

			#endregion

			#region Properties

			public bool HorizontallyScrollable
			{
				get { return (bool) _el.GetPropertyValue(HorizontallyScrollableProperty, _isCached); }
			}

			public double HorizontalScrollPercent
			{
				get { return (double) _el.GetPropertyValue(HorizontalScrollPercentProperty, _isCached); }
			}

			public double HorizontalViewSize
			{
				get { return (double) _el.GetPropertyValue(HorizontalViewSizeProperty, _isCached); }
			}

			public bool VerticallyScrollable
			{
				get { return (bool) _el.GetPropertyValue(VerticallyScrollableProperty, _isCached); }
			}

			public double VerticalScrollPercent
			{
				get { return (double) _el.GetPropertyValue(VerticalScrollPercentProperty, _isCached); }
			}

			public double VerticalViewSize
			{
				get { return (double) _el.GetPropertyValue(VerticalViewSizeProperty, _isCached); }
			}

			#endregion
		}

		#endregion
	}

	public class ScrollItemPattern : BasePattern
	{
		#region Fields

		public static readonly AutomationPattern Pattern = ScrollItemPatternIdentifiers.Pattern;
		private readonly IUIAutomationScrollItemPattern _pattern;

		#endregion

		#region Constructors

		private ScrollItemPattern(AutomationElement el, IUIAutomationScrollItemPattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Methods

		public void ScrollIntoView()
		{
			try
			{
				_pattern.ScrollIntoView();
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
			return (pattern == null) ? null : new ScrollItemPattern(el, (IUIAutomationScrollItemPattern) pattern, cached);
		}

		#endregion
	}
}