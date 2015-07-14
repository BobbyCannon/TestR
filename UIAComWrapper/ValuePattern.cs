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
	public class RangeValuePattern : BasePattern
	{
		#region Fields

		public static readonly AutomationProperty IsReadOnlyProperty = RangeValuePatternIdentifiers.IsReadOnlyProperty;
		public static readonly AutomationProperty LargeChangeProperty = RangeValuePatternIdentifiers.LargeChangeProperty;
		public static readonly AutomationProperty MaximumProperty = RangeValuePatternIdentifiers.MaximumProperty;
		public static readonly AutomationProperty MinimumProperty = RangeValuePatternIdentifiers.MinimumProperty;
		public static readonly AutomationPattern Pattern = RangeValuePatternIdentifiers.Pattern;
		public static readonly AutomationProperty SmallChangeProperty = RangeValuePatternIdentifiers.SmallChangeProperty;
		public static readonly AutomationProperty ValueProperty = RangeValuePatternIdentifiers.ValueProperty;
		private readonly IUIAutomationRangeValuePattern _pattern;

		#endregion

		#region Constructors

		private RangeValuePattern(AutomationElement el, IUIAutomationRangeValuePattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Properties

		public RangeValuePatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new RangeValuePatternInformation(_el, true);
			}
		}

		public RangeValuePatternInformation Current
		{
			get { return new RangeValuePatternInformation(_el, false); }
		}

		#endregion

		#region Methods

		public void SetValue(double value)
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
			return (pattern == null) ? null : new RangeValuePattern(el, (IUIAutomationRangeValuePattern) pattern, cached);
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct RangeValuePatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal RangeValuePatternInformation(AutomationElement element, bool isCached)
			{
				_el = element;
				_isCached = isCached;
			}

			#endregion

			#region Properties

			public bool IsReadOnly
			{
				get { return (bool) _el.GetPropertyValue(IsReadOnlyProperty, _isCached); }
			}

			public double LargeChange
			{
				get { return (double) _el.GetPropertyValue(LargeChangeProperty, _isCached); }
			}

			public double Maximum
			{
				get { return (double) _el.GetPropertyValue(MaximumProperty, _isCached); }
			}

			public double Minimum
			{
				get { return (double) _el.GetPropertyValue(MinimumProperty, _isCached); }
			}

			public double SmallChange
			{
				get { return (double) _el.GetPropertyValue(SmallChangeProperty, _isCached); }
			}

			public double Value
			{
				get { return (double) _el.GetPropertyValue(ValueProperty, _isCached); }
			}

			#endregion
		}

		#endregion
	}

	public class ValuePattern : BasePattern
	{
		#region Fields

		public static readonly AutomationProperty IsReadOnlyProperty = ValuePatternIdentifiers.IsReadOnlyProperty;
		public static readonly AutomationPattern Pattern = ValuePatternIdentifiers.Pattern;
		public static readonly AutomationProperty ValueProperty = ValuePatternIdentifiers.ValueProperty;
		private readonly IUIAutomationValuePattern _pattern;

		#endregion

		#region Constructors

		private ValuePattern(AutomationElement el, IUIAutomationValuePattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Properties

		public ValuePatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new ValuePatternInformation(_el, true);
			}
		}

		public ValuePatternInformation Current
		{
			get { return new ValuePatternInformation(_el, false); }
		}

		#endregion

		#region Methods

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
			return (pattern == null) ? null : new ValuePattern(el, (IUIAutomationValuePattern) pattern, cached);
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct ValuePatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal ValuePatternInformation(AutomationElement element, bool isCached)
			{
				_el = element;
				_isCached = isCached;
			}

			#endregion

			#region Properties

			public bool IsReadOnly
			{
				get { return (bool) _el.GetPropertyValue(IsReadOnlyProperty, _isCached); }
			}

			public string Value
			{
				get { return (string) _el.GetPropertyValue(ValueProperty, _isCached); }
			}

			#endregion
		}

		#endregion
	}
}