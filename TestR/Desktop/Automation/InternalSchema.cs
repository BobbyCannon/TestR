#region References

using System;
using System.Globalization;
using System.Windows;
using TestR.Desktop.Automation.Patterns;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Automation
{
	internal delegate object PropertyConverter(object valueAsObject);

	internal delegate object PatternWrapper(AutomationElement el, object pattern, bool cached);

	internal class PropertyTypeInfo
	{
		#region Constructors

		internal PropertyTypeInfo(PropertyConverter converter, AutomationIdentifier id, Type type)
		{
			ID = id;
			Type = type;
			ObjectConverter = converter;
		}

		#endregion

		#region Properties

		internal AutomationIdentifier ID { get; private set; }
		internal PropertyConverter ObjectConverter { get; private set; }
		internal Type Type { get; private set; }

		#endregion
	}

	internal class PatternTypeInfo
	{
		#region Constructors

		public PatternTypeInfo(AutomationPattern id, PatternWrapper clientSideWrapper)
		{
			ID = id;
			ClientSideWrapper = clientSideWrapper;
		}

		#endregion

		#region Properties

		internal PatternWrapper ClientSideWrapper { get; private set; }
		internal AutomationPattern ID { get; private set; }

		#endregion
	}

	internal class Schema
	{
		#region Fields

		private static PropertyConverter convertToBool = ConvertToBool;
		private static readonly PropertyConverter convertToControlType = ConvertToControlType;
		private static readonly PropertyConverter convertToCultureInfo = ConvertToCultureInfo;
		private static readonly PropertyConverter convertToDockPosition = ConvertToDockPosition;
		private static PropertyConverter convertToElement = ConvertToElement;
		private static PropertyConverter convertToElementArray = ConvertToElementArray;
		private static readonly PropertyConverter convertToExpandCollapseState = ConvertToExpandCollapseState;
		private static readonly PropertyConverter convertToOrientationType = ConvertToOrientationType;
		private static readonly PropertyConverter convertToPoint = ConvertToPoint;
		private static readonly PropertyConverter convertToRect = ConvertToRect;
		private static readonly PropertyConverter convertToRowOrColumnMajor = ConvertToRowOrColumnMajor;
		private static readonly PropertyConverter convertToToggleState = ConvertToToggleState;
		private static readonly PropertyConverter convertToWindowInteractionState = ConvertToWindowInteractionState;
		private static readonly PropertyConverter convertToWindowVisualState = ConvertToWindowVisualState;

		private static readonly PatternTypeInfo[] _patternInfoTable =
		{
			new PatternTypeInfo(InvokePattern.Pattern, InvokePattern.Wrap),
			new PatternTypeInfo(SelectionPattern.Pattern, SelectionPattern.Wrap),
			new PatternTypeInfo(ValuePattern.Pattern, ValuePattern.Wrap),
			new PatternTypeInfo(RangeValuePattern.Pattern, RangeValuePattern.Wrap),
			new PatternTypeInfo(ScrollPattern.Pattern, ScrollPattern.Wrap),
			new PatternTypeInfo(ExpandCollapsePattern.Pattern, ExpandCollapsePattern.Wrap),
			new PatternTypeInfo(GridPattern.Pattern, GridPattern.Wrap),
			new PatternTypeInfo(GridItemPattern.Pattern, GridItemPattern.Wrap),
			new PatternTypeInfo(MultipleViewPattern.Pattern, MultipleViewPattern.Wrap),
			new PatternTypeInfo(WindowPattern.Pattern, WindowPattern.Wrap),
			new PatternTypeInfo(SelectionItemPattern.Pattern, SelectionItemPattern.Wrap),
			new PatternTypeInfo(DockPattern.Pattern, DockPattern.Wrap),
			new PatternTypeInfo(TablePattern.Pattern, TablePattern.Wrap),
			new PatternTypeInfo(TableItemPattern.Pattern, TableItemPattern.Wrap),
			new PatternTypeInfo(TextPattern.Pattern, TextPattern.Wrap),
			new PatternTypeInfo(TogglePattern.Pattern, TogglePattern.Wrap),
			new PatternTypeInfo(TransformPattern.Pattern, TransformPattern.Wrap),
			new PatternTypeInfo(ScrollItemPattern.Pattern, ScrollItemPattern.Wrap),
			new PatternTypeInfo(ItemContainerPattern.Pattern, ItemContainerPattern.Wrap),
			new PatternTypeInfo(VirtualizedItemPattern.Pattern, VirtualizedItemPattern.Wrap),
			new PatternTypeInfo(LegacyIAccessiblePattern.Pattern, LegacyIAccessiblePattern.Wrap),
			new PatternTypeInfo(SynchronizedInputPattern.Pattern, SynchronizedInputPattern.Wrap)
		};

		private static readonly PropertyTypeInfo[] _propertyInfoTable =
		{
			// Properties requiring conversion
			new PropertyTypeInfo(convertToRect, AutomationElement.BoundingRectangleProperty, typeof (Rect)),
			new PropertyTypeInfo(convertToControlType, AutomationElement.ControlTypeProperty, typeof (ControlType)),
			new PropertyTypeInfo(convertToPoint, AutomationElement.ClickablePointProperty, typeof (Point)),
			new PropertyTypeInfo(convertToCultureInfo, AutomationElement.CultureProperty, typeof (CultureInfo)),
			new PropertyTypeInfo(convertToOrientationType, AutomationElement.OrientationProperty, typeof (OrientationType)),
			new PropertyTypeInfo(convertToDockPosition, DockPattern.DockPositionProperty, typeof (DockPosition)),
			new PropertyTypeInfo(convertToExpandCollapseState, ExpandCollapsePattern.ExpandCollapseStateProperty, typeof (ExpandCollapseState)),
			new PropertyTypeInfo(convertToWindowVisualState, WindowPattern.WindowVisualStateProperty, typeof (WindowVisualState)),
			new PropertyTypeInfo(convertToWindowInteractionState, WindowPattern.WindowInteractionStateProperty, typeof (WindowInteractionState)),
			new PropertyTypeInfo(convertToRowOrColumnMajor, TablePattern.RowOrColumnMajorProperty, typeof (RowOrColumnMajor)),
			new PropertyTypeInfo(convertToToggleState, TogglePattern.ToggleStateProperty, typeof (ToggleState)),

			// Text attributes 
			new PropertyTypeInfo(null, TextPattern.AnimationStyleAttribute, typeof (AnimationStyle)),
			new PropertyTypeInfo(null, TextPattern.BackgroundColorAttribute, typeof (int)),
			new PropertyTypeInfo(null, TextPattern.BulletStyleAttribute, typeof (BulletStyle)),
			new PropertyTypeInfo(null, TextPattern.CapStyleAttribute, typeof (CapStyle)),
			new PropertyTypeInfo(convertToCultureInfo, TextPattern.CultureAttribute, typeof (CultureInfo)),
			new PropertyTypeInfo(null, TextPattern.FontNameAttribute, typeof (string)),
			new PropertyTypeInfo(null, TextPattern.FontSizeAttribute, typeof (double)),
			new PropertyTypeInfo(null, TextPattern.FontWeightAttribute, typeof (int)),
			new PropertyTypeInfo(null, TextPattern.ForegroundColorAttribute, typeof (int)),
			new PropertyTypeInfo(null, TextPattern.HorizontalTextAlignmentAttribute, typeof (HorizontalTextAlignment)),
			new PropertyTypeInfo(null, TextPattern.IndentationFirstLineAttribute, typeof (double)),
			new PropertyTypeInfo(null, TextPattern.IndentationLeadingAttribute, typeof (double)),
			new PropertyTypeInfo(null, TextPattern.IndentationTrailingAttribute, typeof (double)),
			new PropertyTypeInfo(null, TextPattern.IsHiddenAttribute, typeof (bool)), new PropertyTypeInfo(null, TextPattern.IsItalicAttribute, typeof (bool)),
			new PropertyTypeInfo(null, TextPattern.IsReadOnlyAttribute, typeof (bool)),
			new PropertyTypeInfo(null, TextPattern.IsSubscriptAttribute, typeof (bool)),
			new PropertyTypeInfo(null, TextPattern.IsSuperscriptAttribute, typeof (bool)),
			new PropertyTypeInfo(null, TextPattern.MarginBottomAttribute, typeof (double)),
			new PropertyTypeInfo(null, TextPattern.MarginLeadingAttribute, typeof (double)),
			new PropertyTypeInfo(null, TextPattern.MarginTopAttribute, typeof (double)),
			new PropertyTypeInfo(null, TextPattern.MarginTrailingAttribute, typeof (double)),
			new PropertyTypeInfo(null, TextPattern.OutlineStylesAttribute, typeof (OutlineStyles)),
			new PropertyTypeInfo(null, TextPattern.OverlineColorAttribute, typeof (int)),
			new PropertyTypeInfo(null, TextPattern.OverlineStyleAttribute, typeof (TextDecorationLineStyle)),
			new PropertyTypeInfo(null, TextPattern.StrikethroughColorAttribute, typeof (int)),
			new PropertyTypeInfo(null, TextPattern.StrikethroughStyleAttribute, typeof (TextDecorationLineStyle)),
			new PropertyTypeInfo(null, TextPattern.TabsAttribute, typeof (double[])),
			new PropertyTypeInfo(null, TextPattern.TextFlowDirectionsAttribute, typeof (FlowDirections)),
			new PropertyTypeInfo(null, TextPattern.UnderlineColorAttribute, typeof (int)),
			new PropertyTypeInfo(null, TextPattern.UnderlineStyleAttribute, typeof (TextDecorationLineStyle))
		};

		#endregion

		#region Constructors

		private Schema()
		{
		}

		#endregion

		#region Methods

		internal static object ConvertToElementArray(object value)
		{
			return Utility.ConvertToElementArray((IUIAutomationElementArray) value);
		}

		internal static bool GetPatternInfo(AutomationPattern id, out PatternTypeInfo info)
		{
			foreach (var info2 in _patternInfoTable)
			{
				if (info2.ID == id)
				{
					info = info2;
					return true;
				}
			}
			info = null;
			return false;
		}

		internal static bool GetPropertyTypeInfo(AutomationIdentifier id, out PropertyTypeInfo info)
		{
			foreach (var info2 in _propertyInfoTable)
			{
				if (info2.ID == id)
				{
					info = info2;
					return true;
				}
			}
			info = null;
			return false;
		}

		private static object ConvertToBool(object value)
		{
			return value;
		}

		private static object ConvertToControlType(object value)
		{
			if (value is ControlType)
			{
				return value;
			}
			return ControlType.LookupById((int) value);
		}

		private static object ConvertToCultureInfo(object value)
		{
			if (value is int)
			{
				if ((int) value == 0)
				{
					// Some providers return 0 to mean Invariant
					return CultureInfo.InvariantCulture;
				}
				return new CultureInfo((int) value);
			}
			return null;
		}

		private static object ConvertToDockPosition(object value)
		{
			return (DockPosition) value;
		}

		private static object ConvertToElement(object value)
		{
			return AutomationElement.Wrap((IUIAutomationElement) value);
		}

		private static object ConvertToExpandCollapseState(object value)
		{
			return (ExpandCollapseState) value;
		}

		private static object ConvertToOrientationType(object value)
		{
			return (OrientationType) value;
		}

		private static object ConvertToPoint(object value)
		{
			var numArray = (double[]) value;
			return new Point(numArray[0], numArray[1]);
		}

		private static object ConvertToRect(object value)
		{
			var numArray = (double[]) value;
			var x = numArray[0];
			var y = numArray[1];
			var width = numArray[2];
			return new Rect(x, y, width, numArray[3]);
		}

		private static object ConvertToRowOrColumnMajor(object value)
		{
			return (RowOrColumnMajor) value;
		}

		private static object ConvertToToggleState(object value)
		{
			return (ToggleState) value;
		}

		private static object ConvertToWindowInteractionState(object value)
		{
			return (WindowInteractionState) value;
		}

		private static object ConvertToWindowVisualState(object value)
		{
			return (WindowVisualState) value;
		}

		#endregion
	}
}