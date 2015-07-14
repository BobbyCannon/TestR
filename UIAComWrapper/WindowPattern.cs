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
	public class WindowPattern : BasePattern
	{
		#region Fields

		public static readonly AutomationProperty CanMaximizeProperty = WindowPatternIdentifiers.CanMaximizeProperty;
		public static readonly AutomationProperty CanMinimizeProperty = WindowPatternIdentifiers.CanMinimizeProperty;
		public static readonly AutomationProperty IsModalProperty = WindowPatternIdentifiers.IsModalProperty;
		public static readonly AutomationProperty IsTopmostProperty = WindowPatternIdentifiers.IsTopmostProperty;
		public static readonly AutomationPattern Pattern = WindowPatternIdentifiers.Pattern;
		public static readonly AutomationEvent WindowClosedEvent = WindowPatternIdentifiers.WindowClosedEvent;
		public static readonly AutomationProperty WindowInteractionStateProperty = WindowPatternIdentifiers.WindowInteractionStateProperty;
		public static readonly AutomationEvent WindowOpenedEvent = WindowPatternIdentifiers.WindowOpenedEvent;
		public static readonly AutomationProperty WindowVisualStateProperty = WindowPatternIdentifiers.WindowVisualStateProperty;
		private readonly IUIAutomationWindowPattern _pattern;

		#endregion

		#region Constructors

		private WindowPattern(AutomationElement el, IUIAutomationWindowPattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Properties

		public WindowPatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new WindowPatternInformation(_el, true);
			}
		}

		public WindowPatternInformation Current
		{
			get { return new WindowPatternInformation(_el, false); }
		}

		#endregion

		#region Methods

		public void Close()
		{
			try
			{
				_pattern.Close();
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

		public void SetWindowVisualState(WindowVisualState state)
		{
			try
			{
				_pattern.SetWindowVisualState((UIAutomationClient.WindowVisualState) state);
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

		public bool WaitForInputIdle(int milliseconds)
		{
			try
			{
				return (0 != _pattern.WaitForInputIdle(milliseconds));
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
			return (pattern == null) ? null : new WindowPattern(el, (IUIAutomationWindowPattern) pattern, cached);
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct WindowPatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal WindowPatternInformation(AutomationElement element, bool isCached)
			{
				_el = element;
				_isCached = isCached;
			}

			#endregion

			#region Properties

			public bool CanMaximize
			{
				get { return (bool) _el.GetPropertyValue(CanMaximizeProperty, _isCached); }
			}

			public bool CanMinimize
			{
				get { return (bool) _el.GetPropertyValue(CanMinimizeProperty, _isCached); }
			}

			public bool IsModal
			{
				get { return (bool) _el.GetPropertyValue(IsModalProperty, _isCached); }
			}

			public bool IsTopmost
			{
				get { return (bool) _el.GetPropertyValue(IsTopmostProperty, _isCached); }
			}

			public WindowInteractionState WindowInteractionState
			{
				get { return (WindowInteractionState) _el.GetPropertyValue(WindowInteractionStateProperty, _isCached); }
			}

			public WindowVisualState WindowVisualState
			{
				get { return (WindowVisualState) _el.GetPropertyValue(WindowVisualStateProperty, _isCached); }
			}

			#endregion
		}

		#endregion
	}
}