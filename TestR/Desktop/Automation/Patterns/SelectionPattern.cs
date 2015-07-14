#region References

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Automation.Patterns
{
	public class SelectionPattern : BasePattern
	{
		#region Fields

		public static readonly AutomationProperty CanSelectMultipleProperty = SelectionPatternIdentifiers.CanSelectMultipleProperty;
		public static readonly AutomationEvent InvalidatedEvent = SelectionPatternIdentifiers.InvalidatedEvent;
		public static readonly AutomationProperty IsSelectionRequiredProperty = SelectionPatternIdentifiers.IsSelectionRequiredProperty;
		public static readonly AutomationPattern Pattern = SelectionPatternIdentifiers.Pattern;
		public static readonly AutomationProperty SelectionProperty = SelectionPatternIdentifiers.SelectionProperty;
		private IUIAutomationSelectionPattern _pattern;

		#endregion

		#region Constructors

		private SelectionPattern(AutomationElement el, IUIAutomationSelectionPattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Properties

		public SelectionPatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new SelectionPatternInformation(_el, true);
			}
		}

		public SelectionPatternInformation Current
		{
			get { return new SelectionPatternInformation(_el, false); }
		}

		#endregion

		#region Methods

		internal static object Wrap(AutomationElement el, object pattern, bool cached)
		{
			return (pattern == null) ? null : new SelectionPattern(el, (IUIAutomationSelectionPattern) pattern, cached);
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct SelectionPatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal SelectionPatternInformation(AutomationElement element, bool isCached)
			{
				_el = element;
				_isCached = isCached;
			}

			#endregion

			#region Properties

			public bool CanSelectMultiple
			{
				get { return (bool) _el.GetPropertyValue(CanSelectMultipleProperty, _isCached); }
			}

			public bool IsSelectionRequired
			{
				get { return (bool) _el.GetPropertyValue(IsSelectionRequiredProperty, _isCached); }
			}

			#endregion

			#region Methods

			public AutomationElement[] GetSelection()
			{
				return (AutomationElement[]) _el.GetPropertyValue(SelectionProperty, _isCached);
			}

			#endregion
		}

		#endregion
	}

	public class SelectionItemPattern : BasePattern
	{
		#region Fields

		public static readonly AutomationEvent ElementAddedToSelectionEvent = SelectionItemPatternIdentifiers.ElementAddedToSelectionEvent;
		public static readonly AutomationEvent ElementRemovedFromSelectionEvent = SelectionItemPatternIdentifiers.ElementRemovedFromSelectionEvent;
		public static readonly AutomationEvent ElementSelectedEvent = SelectionItemPatternIdentifiers.ElementSelectedEvent;
		public static readonly AutomationProperty IsSelectedProperty = SelectionItemPatternIdentifiers.IsSelectedProperty;
		public static readonly AutomationPattern Pattern = SelectionItemPatternIdentifiers.Pattern;
		public static readonly AutomationProperty SelectionContainerProperty = SelectionItemPatternIdentifiers.SelectionContainerProperty;
		private readonly IUIAutomationSelectionItemPattern _pattern;

		#endregion

		#region Constructors

		private SelectionItemPattern(AutomationElement el, IUIAutomationSelectionItemPattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Properties

		public SelectionItemPatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new SelectionItemPatternInformation(_el, true);
			}
		}

		public SelectionItemPatternInformation Current
		{
			get { return new SelectionItemPatternInformation(_el, false); }
		}

		#endregion

		#region Methods

		public void AddToSelection()
		{
			try
			{
				_pattern.AddToSelection();
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

		public void RemoveFromSelection()
		{
			try
			{
				_pattern.RemoveFromSelection();
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

		public void Select()
		{
			try
			{
				_pattern.Select();
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
			return (pattern == null) ? null : new SelectionItemPattern(el, (IUIAutomationSelectionItemPattern) pattern, cached);
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct SelectionItemPatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal SelectionItemPatternInformation(AutomationElement element, bool isCached)
			{
				_el = element;
				_isCached = isCached;
			}

			#endregion

			#region Properties

			public bool IsSelected
			{
				get { return (bool) _el.GetPropertyValue(IsSelectedProperty, _isCached); }
			}

			public AutomationElement SelectionContainer
			{
				get { return (AutomationElement) _el.GetPropertyValue(SelectionContainerProperty, _isCached); }
			}

			#endregion
		}

		#endregion
	}
}