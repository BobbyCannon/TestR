// (c) Copyright Microsoft Corporation, 2010.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

#region References

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UIAutomationClient;
using IAccessible = Accessibility.IAccessible;

#endregion

namespace UIAComWrapper
{
	public class LegacyIAccessiblePattern : BasePattern
	{
		#region Fields

		public static readonly AutomationProperty ChildIdProperty = LegacyIAccessiblePatternIdentifiers.ChildIdProperty;
		public static readonly AutomationProperty DefaultActionProperty = LegacyIAccessiblePatternIdentifiers.DefaultActionProperty;
		public static readonly AutomationProperty DescriptionProperty = LegacyIAccessiblePatternIdentifiers.DescriptionProperty;
		public static readonly AutomationProperty HelpProperty = LegacyIAccessiblePatternIdentifiers.HelpProperty;
		public static readonly AutomationProperty KeyboardShortcutProperty = LegacyIAccessiblePatternIdentifiers.KeyboardShortcutProperty;
		public static readonly AutomationProperty NameProperty = LegacyIAccessiblePatternIdentifiers.NameProperty;
		public static readonly AutomationPattern Pattern = LegacyIAccessiblePatternIdentifiers.Pattern;
		public static readonly AutomationProperty RoleProperty = LegacyIAccessiblePatternIdentifiers.RoleProperty;
		public static readonly AutomationProperty SelectionProperty = LegacyIAccessiblePatternIdentifiers.SelectionProperty;
		public static readonly AutomationProperty StateProperty = LegacyIAccessiblePatternIdentifiers.StateProperty;
		public static readonly AutomationProperty ValueProperty = LegacyIAccessiblePatternIdentifiers.ValueProperty;
		private readonly IUIAutomationLegacyIAccessiblePattern _pattern;

		#endregion

		#region Constructors

		private LegacyIAccessiblePattern(AutomationElement el, IUIAutomationLegacyIAccessiblePattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Properties

		public LegacyIAccessiblePatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new LegacyIAccessiblePatternInformation(_el, true);
			}
		}

		public LegacyIAccessiblePatternInformation Current
		{
			get { return new LegacyIAccessiblePatternInformation(_el, false); }
		}

		#endregion

		#region Methods

		public void DoDefaultAction()
		{
			try
			{
				_pattern.DoDefaultAction();
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

		public IAccessible GetIAccessible()
		{
			try
			{
				return (IAccessible) _pattern.GetIAccessible();
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

		public void Select(int flagsSelect)
		{
			try
			{
				_pattern.Select(flagsSelect);
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

		public void SetValue(string value)
		{
			try
			{
				_pattern.SetValue(value);
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
			return (pattern == null) ? null : new LegacyIAccessiblePattern(el, (IUIAutomationLegacyIAccessiblePattern) pattern, cached);
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct LegacyIAccessiblePatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal LegacyIAccessiblePatternInformation(AutomationElement element, bool isCached)
			{
				_el = element;
				_isCached = isCached;
			}

			#endregion

			#region Properties

			public int ChildId
			{
				get { return (int) _el.GetPropertyValue(ChildIdProperty, _isCached); }
			}

			public string DefaultAction
			{
				get { return (string) _el.GetPropertyValue(DefaultActionProperty, _isCached); }
			}

			public string Description
			{
				get { return (string) _el.GetPropertyValue(DescriptionProperty, _isCached); }
			}

			public string Help
			{
				get { return (string) _el.GetPropertyValue(HelpProperty, _isCached); }
			}

			public string KeyboardShortcut
			{
				get { return (string) _el.GetPropertyValue(KeyboardShortcutProperty, _isCached); }
			}

			public string Name
			{
				get { return (string) _el.GetPropertyValue(NameProperty, _isCached); }
			}

			public uint Role
			{
				get { return Convert.ToUInt32(_el.GetPropertyValue(RoleProperty, _isCached)); }
			}

			public uint State
			{
				get { return Convert.ToUInt32(_el.GetPropertyValue(StateProperty, _isCached)); }
			}

			public string Value
			{
				get { return (string) _el.GetPropertyValue(ValueProperty, _isCached); }
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
}