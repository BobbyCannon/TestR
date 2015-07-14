#region References

using System.Diagnostics;
using System.Runtime.InteropServices;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Automation.Patterns
{
	public class TablePattern : GridPattern
	{
		#region Fields

		public static readonly AutomationProperty ColumnHeadersProperty = TablePatternIdentifiers.ColumnHeadersProperty;
		public new static readonly AutomationPattern Pattern = TablePatternIdentifiers.Pattern;
		public static readonly AutomationProperty RowHeadersProperty = TablePatternIdentifiers.RowHeadersProperty;
		public static readonly AutomationProperty RowOrColumnMajorProperty = TablePatternIdentifiers.RowOrColumnMajorProperty;
		private IUIAutomationTablePattern _pattern;

		#endregion

		#region Constructors

		private TablePattern(AutomationElement el, IUIAutomationTablePattern tablePattern, IUIAutomationGridPattern gridPattern, bool cached)
			: base(el, gridPattern, cached)
		{
			Debug.Assert(tablePattern != null);
			_pattern = tablePattern;
		}

		#endregion

		#region Properties

		public new TablePatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new TablePatternInformation(_el, true);
			}
		}

		public new TablePatternInformation Current
		{
			get { return new TablePatternInformation(_el, false); }
		}

		#endregion

		#region Methods

		internal new static object Wrap(AutomationElement el, object pattern, bool cached)
		{
			TablePattern result = null;
			if (pattern != null)
			{
				var gridPattern =
					(IUIAutomationGridPattern) el.GetRawPattern(GridPattern.Pattern, cached);
				if (gridPattern != null)
				{
					result = new TablePattern(el, (IUIAutomationTablePattern) pattern,
						gridPattern, cached);
				}
			}
			return result;
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct TablePatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal TablePatternInformation(AutomationElement element, bool isCached)
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

			public RowOrColumnMajor RowOrColumnMajor
			{
				get { return (RowOrColumnMajor) _el.GetPropertyValue(RowOrColumnMajorProperty, _isCached); }
			}

			#endregion

			#region Methods

			public AutomationElement[] GetColumnHeaders()
			{
				return (AutomationElement[]) _el.GetPropertyValue(ColumnHeadersProperty, _isCached);
			}

			public AutomationElement[] GetRowHeaders()
			{
				return (AutomationElement[]) _el.GetPropertyValue(RowHeadersProperty, _isCached);
			}

			#endregion
		}

		#endregion
	}

	public class TableItemPattern : GridItemPattern
	{
		#region Fields

		public static readonly AutomationProperty ColumnHeaderItemsProperty = TableItemPatternIdentifiers.ColumnHeaderItemsProperty;
		public new static readonly AutomationPattern Pattern = TableItemPatternIdentifiers.Pattern;
		public static readonly AutomationProperty RowHeaderItemsProperty = TableItemPatternIdentifiers.RowHeaderItemsProperty;
		private IUIAutomationTableItemPattern _pattern;

		#endregion

		#region Constructors

		private TableItemPattern(AutomationElement el, IUIAutomationTableItemPattern tablePattern, IUIAutomationGridItemPattern gridPattern, bool cached)
			: base(el, gridPattern, cached)
		{
			Debug.Assert(tablePattern != null);
			_pattern = tablePattern;
		}

		#endregion

		#region Properties

		public new TableItemPatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new TableItemPatternInformation(_el, true);
			}
		}

		public new TableItemPatternInformation Current
		{
			get { return new TableItemPatternInformation(_el, false); }
		}

		#endregion

		#region Methods

		internal new static object Wrap(AutomationElement el, object pattern, bool cached)
		{
			TableItemPattern result = null;
			if (pattern != null)
			{
				var gridPattern =
					(IUIAutomationGridItemPattern) el.GetRawPattern(GridItemPattern.Pattern, cached);
				if (gridPattern != null)
				{
					result = new TableItemPattern(el, (IUIAutomationTableItemPattern) pattern,
						gridPattern, cached);
				}
			}
			return result;
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct TableItemPatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal TableItemPatternInformation(AutomationElement element, bool isCached)
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

			#region Methods

			public AutomationElement[] GetColumnHeaderItems()
			{
				return (AutomationElement[]) _el.GetPropertyValue(ColumnHeaderItemsProperty, _isCached);
			}

			public AutomationElement[] GetRowHeaderItems()
			{
				return (AutomationElement[]) _el.GetPropertyValue(RowHeaderItemsProperty, _isCached);
			}

			#endregion
		}

		#endregion
	}
}