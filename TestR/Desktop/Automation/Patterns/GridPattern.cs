#region References

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Automation.Patterns
{
	public class GridPattern : BasePattern
	{
		#region Fields

		public static readonly AutomationProperty ColumnCountProperty = GridPatternIdentifiers.ColumnCountProperty;
		public static readonly AutomationPattern Pattern = GridPatternIdentifiers.Pattern;
		public static readonly AutomationProperty RowCountProperty = GridPatternIdentifiers.RowCountProperty;
		private readonly IUIAutomationGridPattern _pattern;

		#endregion

		#region Constructors

		protected GridPattern(AutomationElement el, IUIAutomationGridPattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Properties

		public GridPatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new GridPatternInformation(_el, true);
			}
		}

		public GridPatternInformation Current
		{
			get { return new GridPatternInformation(_el, false); }
		}

		#endregion

		#region Methods

		public AutomationElement GetItem(int row, int column)
		{
			try
			{
				// Looks like we have to cache explicitly here, since GetItem doesn't
				// take a cache request.
				return AutomationElement.Wrap(_pattern.GetItem(row, column)).GetUpdatedCache(CacheRequest.Current);
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
			return (pattern == null) ? null : new GridPattern(el, (IUIAutomationGridPattern) pattern, cached);
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct GridPatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal GridPatternInformation(AutomationElement element, bool isCached)
			{
				_el = element;
				_isCached = isCached;
			}

			#endregion

			#region Properties

			public int ColumnCount
			{
				get { return (int) _el.GetPropertyValue(ColumnCountProperty, _isCached); }
			}

			public int RowCount
			{
				get { return (int) _el.GetPropertyValue(RowCountProperty, _isCached); }
			}

			#endregion
		}

		#endregion
	}

	public class GridItemPattern : BasePattern
	{
		#region Fields

		public static readonly AutomationProperty ColumnProperty = GridItemPatternIdentifiers.ColumnProperty;
		public static readonly AutomationProperty ColumnSpanProperty = GridItemPatternIdentifiers.ColumnSpanProperty;
		public static readonly AutomationProperty ContainingGridProperty = GridItemPatternIdentifiers.ContainingGridProperty;
		public static readonly AutomationPattern Pattern = GridItemPatternIdentifiers.Pattern;
		public static readonly AutomationProperty RowProperty = GridItemPatternIdentifiers.RowProperty;
		public static readonly AutomationProperty RowSpanProperty = GridItemPatternIdentifiers.RowSpanProperty;
		private IUIAutomationGridItemPattern _pattern;

		#endregion

		#region Constructors

		protected GridItemPattern(AutomationElement el, IUIAutomationGridItemPattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Properties

		public GridItemPatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new GridItemPatternInformation(_el, true);
			}
		}

		public GridItemPatternInformation Current
		{
			get { return new GridItemPatternInformation(_el, false); }
		}

		#endregion

		#region Methods

		internal static object Wrap(AutomationElement el, object pattern, bool cached)
		{
			return (pattern == null) ? null : new GridItemPattern(el, (IUIAutomationGridItemPattern) pattern, cached);
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct GridItemPatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal GridItemPatternInformation(AutomationElement element, bool isCached)
			{
				_el = element;
				_isCached = isCached;
			}

			#endregion

			#region Properties

			public int Column
			{
				get { return (int) _el.GetPropertyValue(ColumnProperty, _isCached); }
			}

			public int ColumnSpan
			{
				get { return (int) _el.GetPropertyValue(ColumnSpanProperty, _isCached); }
			}

			public AutomationElement ContainingGrid
			{
				get { return (AutomationElement) _el.GetPropertyValue(ContainingGridProperty, _isCached); }
			}

			public int Row
			{
				get { return (int) _el.GetPropertyValue(RowProperty, _isCached); }
			}

			public int RowSpan
			{
				get { return (int) _el.GetPropertyValue(RowSpanProperty, _isCached); }
			}

			#endregion
		}

		#endregion
	}
}