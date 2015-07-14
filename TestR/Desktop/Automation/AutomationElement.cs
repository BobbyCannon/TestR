#region References

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using UIAutomationClient;
using IAccessible = Accessibility.IAccessible;

#endregion

namespace TestR.Desktop.Automation
{
	public sealed class AutomationElement
	{
		#region Fields

		public static readonly AutomationProperty AcceleratorKeyProperty = AutomationElementIdentifiers.AcceleratorKeyProperty;
		public static readonly AutomationProperty AccessKeyProperty = AutomationElementIdentifiers.AccessKeyProperty;
		public static readonly AutomationProperty AriaPropertiesProperty = AutomationElementIdentifiers.AriaPropertiesProperty;
		public static readonly AutomationProperty AriaRoleProperty = AutomationElementIdentifiers.AriaRoleProperty;
		public static readonly AutomationEvent AsyncContentLoadedEvent = AutomationElementIdentifiers.AsyncContentLoadedEvent;
		public static readonly AutomationEvent AutomationFocusChangedEvent = AutomationElementIdentifiers.AutomationFocusChangedEvent;
		public static readonly AutomationProperty AutomationIdProperty = AutomationElementIdentifiers.AutomationIdProperty;
		public static readonly AutomationEvent AutomationPropertyChangedEvent = AutomationElementIdentifiers.AutomationPropertyChangedEvent;
		public static readonly AutomationProperty BoundingRectangleProperty = AutomationElementIdentifiers.BoundingRectangleProperty;
		public static readonly AutomationProperty ClassNameProperty = AutomationElementIdentifiers.ClassNameProperty;
		public static readonly AutomationProperty ClickablePointProperty = AutomationElementIdentifiers.ClickablePointProperty;
		public static readonly AutomationProperty ControllerForProperty = AutomationElementIdentifiers.ControllerForProperty;
		public static readonly AutomationProperty ControlTypeProperty = AutomationElementIdentifiers.ControlTypeProperty;
		public static readonly AutomationProperty CultureProperty = AutomationElementIdentifiers.CultureProperty;
		public static readonly AutomationProperty DescribedByProperty = AutomationElementIdentifiers.DescribedByProperty;
		public static readonly AutomationProperty FlowsToProperty = AutomationElementIdentifiers.FlowsToProperty;
		public static readonly AutomationProperty FrameworkIdProperty = AutomationElementIdentifiers.FrameworkIdProperty;
		public static readonly AutomationProperty HasKeyboardFocusProperty = AutomationElementIdentifiers.HasKeyboardFocusProperty;
		public static readonly AutomationProperty HelpTextProperty = AutomationElementIdentifiers.HelpTextProperty;
		public static readonly AutomationProperty IsContentElementProperty = AutomationElementIdentifiers.IsContentElementProperty;
		public static readonly AutomationProperty IsControlElementProperty = AutomationElementIdentifiers.IsControlElementProperty;
		public static readonly AutomationProperty IsDataValidForFormProperty = AutomationElementIdentifiers.IsDataValidForFormProperty;
		public static readonly AutomationProperty IsDockPatternAvailableProperty = AutomationElementIdentifiers.IsDockPatternAvailableProperty;
		public static readonly AutomationProperty IsEnabledProperty = AutomationElementIdentifiers.IsEnabledProperty;
		public static readonly AutomationProperty IsExpandCollapsePatternAvailableProperty = AutomationElementIdentifiers.IsExpandCollapsePatternAvailableProperty;
		public static readonly AutomationProperty IsGridItemPatternAvailableProperty = AutomationElementIdentifiers.IsGridItemPatternAvailableProperty;
		public static readonly AutomationProperty IsGridPatternAvailableProperty = AutomationElementIdentifiers.IsGridPatternAvailableProperty;
		public static readonly AutomationProperty IsInvokePatternAvailableProperty = AutomationElementIdentifiers.IsInvokePatternAvailableProperty;
		public static readonly AutomationProperty IsItemContainerPatternAvailableProperty = AutomationElementIdentifiers.IsItemContainerPatternAvailableProperty;
		public static readonly AutomationProperty IsKeyboardFocusableProperty = AutomationElementIdentifiers.IsKeyboardFocusableProperty;
		public static readonly AutomationProperty IsLegacyIAccessiblePatternAvailableProperty = AutomationElementIdentifiers.IsLegacyIAccessiblePatternAvailableProperty;
		public static readonly AutomationProperty IsMultipleViewPatternAvailableProperty = AutomationElementIdentifiers.IsMultipleViewPatternAvailableProperty;
		public static readonly AutomationProperty IsOffscreenProperty = AutomationElementIdentifiers.IsOffscreenProperty;
		public static readonly AutomationProperty IsPasswordProperty = AutomationElementIdentifiers.IsPasswordProperty;
		public static readonly AutomationProperty IsRangeValuePatternAvailableProperty = AutomationElementIdentifiers.IsRangeValuePatternAvailableProperty;
		public static readonly AutomationProperty IsRequiredForFormProperty = AutomationElementIdentifiers.IsRequiredForFormProperty;
		public static readonly AutomationProperty IsScrollItemPatternAvailableProperty = AutomationElementIdentifiers.IsScrollItemPatternAvailableProperty;
		public static readonly AutomationProperty IsScrollPatternAvailableProperty = AutomationElementIdentifiers.IsScrollPatternAvailableProperty;
		public static readonly AutomationProperty IsSelectionItemPatternAvailableProperty = AutomationElementIdentifiers.IsSelectionItemPatternAvailableProperty;
		public static readonly AutomationProperty IsSelectionPatternAvailableProperty = AutomationElementIdentifiers.IsSelectionPatternAvailableProperty;
		public static readonly AutomationProperty IsSynchronizedInputPatternAvailableProperty = AutomationElementIdentifiers.IsSynchronizedInputPatternAvailableProperty;
		public static readonly AutomationProperty IsTableItemPatternAvailableProperty = AutomationElementIdentifiers.IsTableItemPatternAvailableProperty;
		public static readonly AutomationProperty IsTablePatternAvailableProperty = AutomationElementIdentifiers.IsTablePatternAvailableProperty;
		public static readonly AutomationProperty IsTextPatternAvailableProperty = AutomationElementIdentifiers.IsTextPatternAvailableProperty;
		public static readonly AutomationProperty IsTogglePatternAvailableProperty = AutomationElementIdentifiers.IsTogglePatternAvailableProperty;
		public static readonly AutomationProperty IsTransformPatternAvailableProperty = AutomationElementIdentifiers.IsTransformPatternAvailableProperty;
		public static readonly AutomationProperty IsValuePatternAvailableProperty = AutomationElementIdentifiers.IsValuePatternAvailableProperty;
		public static readonly AutomationProperty IsVirtualizedItemPatternAvailableProperty = AutomationElementIdentifiers.IsVirtualizedItemPatternAvailableProperty;
		public static readonly AutomationProperty IsWindowPatternAvailableProperty = AutomationElementIdentifiers.IsWindowPatternAvailableProperty;
		public static readonly AutomationProperty ItemStatusProperty = AutomationElementIdentifiers.ItemStatusProperty;
		public static readonly AutomationProperty ItemTypeProperty = AutomationElementIdentifiers.ItemTypeProperty;
		public static readonly AutomationProperty LabeledByProperty = AutomationElementIdentifiers.LabeledByProperty;
		public static readonly AutomationEvent LayoutInvalidatedEvent = AutomationElementIdentifiers.LayoutInvalidatedEvent;
		public static readonly AutomationProperty LocalizedControlTypeProperty = AutomationElementIdentifiers.LocalizedControlTypeProperty;
		public static readonly AutomationEvent MenuClosedEvent = AutomationElementIdentifiers.MenuClosedEvent;
		public static readonly AutomationEvent MenuModeEndEvent = AutomationElementIdentifiers.MenuModeEndEvent;
		public static readonly AutomationEvent MenuModeStartEvent = AutomationElementIdentifiers.MenuModeStartEvent;
		public static readonly AutomationEvent MenuOpenedEvent = AutomationElementIdentifiers.MenuOpenedEvent;
		public static readonly AutomationProperty NameProperty = AutomationElementIdentifiers.NameProperty;
		public static readonly AutomationProperty NativeWindowHandleProperty = AutomationElementIdentifiers.NativeWindowHandleProperty;
		public static readonly object NotSupported = AutomationElementIdentifiers.NotSupported;
		public static readonly AutomationProperty OrientationProperty = AutomationElementIdentifiers.OrientationProperty;
		public static readonly AutomationProperty ProcessIdProperty = AutomationElementIdentifiers.ProcessIdProperty;
		public static readonly AutomationProperty ProviderDescriptionProperty = AutomationElementIdentifiers.ProviderDescriptionProperty;
		public static readonly AutomationProperty RuntimeIdProperty = AutomationElementIdentifiers.RuntimeIdProperty;
		public static readonly AutomationEvent StructureChangedEvent = AutomationElementIdentifiers.StructureChangedEvent;
		public static readonly AutomationEvent ToolTipClosedEvent = AutomationElementIdentifiers.ToolTipClosedEvent;
		public static readonly AutomationEvent ToolTipOpenedEvent = AutomationElementIdentifiers.ToolTipOpenedEvent;

		#endregion

		#region Constructors

		public AutomationElement(IUIAutomationElement obj)
		{
			Debug.Assert(obj != null);
			NativeElement = obj;
		}

		#endregion

		#region Properties

		public AutomationElementInformation Current
		{
			get { return new AutomationElementInformation(this, false); }
		}

		public static AutomationElement FocusedElement
		{
			get { return Wrap(Automation.Factory.GetFocusedElement()); }
		}

		public IUIAutomationElement NativeElement { get; private set; }

		public static AutomationElement RootElement
		{
			get
			{
				var element = Automation.Factory.GetRootElementBuildCache(CacheRequest.CurrentNativeCacheRequest);
				return Wrap(element);
			}
		}

		#endregion

		#region Methods

		public override bool Equals(object obj)
		{
			var element = obj as AutomationElement;
			return (((obj != null) && (element != null)) && (Automation.Factory.CompareElements(NativeElement, element.NativeElement) != 0));
		}

		public AutomationElementCollection FindAll(TreeScope scope, Condition condition)
		{
			Utility.ValidateArgumentNonNull(condition, "condition");

			var elemArray = NativeElement.FindAllBuildCache((UIAutomationClient.TreeScope) scope, condition.NativeCondition, CacheRequest.CurrentNativeCacheRequest);
			return AutomationElementCollection.Wrap(elemArray);
		}

		public AutomationElement FindFirst(TreeScope scope, Condition condition)
		{
			Utility.ValidateArgumentNonNull(condition, "condition");
			var elem = NativeElement.FindFirstBuildCache((UIAutomationClient.TreeScope) scope, condition.NativeCondition, CacheRequest.CurrentNativeCacheRequest);
			return Wrap(elem);
		}

		public static AutomationElement FromHandle(IntPtr hwnd)
		{
			Utility.ValidateArgument(hwnd != IntPtr.Zero, "Hwnd cannot be null");

			var element = Automation.Factory.ElementFromHandleBuildCache(hwnd, CacheRequest.CurrentNativeCacheRequest);
			return Wrap(element);
		}

		public static AutomationElement FromIAccessible(IAccessible acc, int childId)
		{
			Utility.ValidateArgumentNonNull(acc, "acc");

			var element = Automation.Factory.ElementFromIAccessibleBuildCache((UIAutomationClient.IAccessible) acc, childId, CacheRequest.CurrentNativeCacheRequest);
			return Wrap(element);
		}

		public static AutomationElement FromLocalProvider(object /* IRawElementProviderSimple */ localImpl)
		{
			Utility.ValidateArgumentNonNull(localImpl, "localImpl");

			// It's not clear how you'd do this -- COM API doesn't give you the chance to wrap a provider like this.
			throw new NotImplementedException();
		}

		public static AutomationElement FromPoint(Point pt)
		{
			var element = Automation.Factory.ElementFromPointBuildCache(Utility.PointManagedToNative(pt), CacheRequest.CurrentNativeCacheRequest);
			return Wrap(element);
		}

		public object GetCachedPattern(AutomationPattern pattern)
		{
			object patternObj;
			if (!TryGetCachedPattern(pattern, out patternObj))
			{
				throw new InvalidOperationException("Unsupported pattern");
			}
			return patternObj;
		}

		public object GetCachedPropertyValue(AutomationProperty property)
		{
			return GetCachedPropertyValue(property, false);
		}

		public object GetCachedPropertyValue(AutomationProperty property, bool ignoreDefaultValue)
		{
			Utility.ValidateArgumentNonNull(property, "property");

			var obj = NativeElement.GetCachedPropertyValueEx(property.Id, (ignoreDefaultValue) ? 1 : 0);
			return Utility.WrapObjectAsProperty(property, obj);
		}

		public Point GetClickablePoint()
		{
			Point point;
			if (!TryGetClickablePoint(out point))
			{
				throw new NoClickablePointException();
			}
			return point;
		}

		public object GetCurrentPattern(AutomationPattern pattern)
		{
			object patternObj;
			if (!TryGetCurrentPattern(pattern, out patternObj))
			{
				throw new InvalidOperationException("Unsupported pattern");
			}
			return patternObj;
		}

		public object GetCurrentPropertyValue(AutomationProperty property)
		{
			return GetCurrentPropertyValue(property, false);
		}

		public object GetCurrentPropertyValue(AutomationProperty property, bool ignoreDefaultValue)
		{
			Utility.ValidateArgumentNonNull(property, "property");

			var obj = NativeElement.GetCurrentPropertyValueEx(property.Id, (ignoreDefaultValue) ? 1 : 0);
			return Utility.WrapObjectAsProperty(property, obj);
		}

		public override int GetHashCode()
		{
			var runtimeId = GetRuntimeId();
			var num = 0;
			if (runtimeId == null)
			{
				throw new InvalidOperationException("Operation cannot be performed");
			}
			foreach (var i in runtimeId)
			{
				num = (num * 4) ^ i;
			}
			return num;
		}

		public int[] GetRuntimeId()
		{
			return NativeElement.GetRuntimeId();
		}

		public AutomationPattern[] GetSupportedPatterns()
		{
			int[] rawPatternIds;
			string[] rawPatternNames;

			Automation.Factory.PollForPotentialSupportedPatterns(NativeElement, out rawPatternIds, out rawPatternNames);

			var patternIds = rawPatternIds;

			// This element may support patterns that are not registered for this 
			// client.  Filter them out.
			var patterns = new List<AutomationPattern>();
			foreach (var patternId in patternIds)
			{
				var pattern = AutomationPattern.LookupById(patternId);
				if (pattern != null)
				{
					patterns.Add(pattern);
				}
			}
			return patterns.ToArray();
		}

		public AutomationProperty[] GetSupportedProperties()
		{
			int[] rawPropertyIds;
			string[] rawPropertyNames;

			Automation.Factory.PollForPotentialSupportedProperties(NativeElement, out rawPropertyIds, out rawPropertyNames);

			var propertyIds = rawPropertyIds;

			// This element may support properties that are not registered for this client. Filter them out.
			var properties = new List<AutomationProperty>();
			foreach (var propertyId in propertyIds)
			{
				var property = AutomationProperty.LookupById(propertyId);
				if (property != null)
				{
					properties.Add(property);
				}
			}
			return properties.ToArray();
		}

		public AutomationElement GetUpdatedCache(CacheRequest request)
		{
			return Wrap(NativeElement.BuildUpdatedCache(request.NativeCacheRequest));
		}

		public static bool operator ==(AutomationElement left, AutomationElement right)
		{
			if (Equals(left, null))
			{
				return (Equals(right, null));
			}
			if (Equals(right, null))
			{
				return (Equals(left, null));
			}
			return left.Equals(right);
		}

		public static bool operator !=(AutomationElement left, AutomationElement right)
		{
			return !(left == right);
		}

		public void SetFocus()
		{
			try
			{
				NativeElement.SetFocus();
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

		public bool TryGetCachedPattern(AutomationPattern pattern, out object patternObject)
		{
			patternObject = null;
			Utility.ValidateArgumentNonNull(pattern, "pattern");
			try
			{
				var nativePattern = NativeElement.GetCachedPattern(pattern.Id);
				patternObject = Utility.WrapObjectAsPattern(this, nativePattern, pattern, true /* cached */);
				return (patternObject != null);
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

		public bool TryGetClickablePoint(out Point pt)
		{
			pt = new Point(0.0, 0.0);
			var nativePoint = new tagPOINT();
			try
			{
				var success = NativeElement.GetClickablePoint(out nativePoint) != 0;
				if (success)
				{
					pt.X = nativePoint.x;
					pt.Y = nativePoint.y;
				}
				return success;
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

		public bool TryGetCurrentPattern(AutomationPattern pattern, out object patternObject)
		{
			patternObject = null;
			Utility.ValidateArgumentNonNull(pattern, "pattern");
			try
			{
				var nativePattern = NativeElement.GetCurrentPattern(pattern.Id);
				patternObject = Utility.WrapObjectAsPattern(this, nativePattern, pattern, false /* cached */);
				return (patternObject != null);
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

		public static AutomationElement Wrap(IUIAutomationElement obj)
		{
			return (obj == null) ? null : new AutomationElement(obj);
		}

		internal object GetPropertyValue(AutomationProperty property, bool cached)
		{
			if (cached)
			{
				return GetCachedPropertyValue(property);
			}
			return GetCurrentPropertyValue(property);
		}

		internal object GetRawPattern(AutomationPattern pattern, bool isCached)
		{
			try
			{
				if (isCached)
				{
					return NativeElement.GetCachedPattern(pattern.Id);
				}
				return NativeElement.GetCurrentPattern(pattern.Id);
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

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct AutomationElementInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal AutomationElementInformation(AutomationElement el, bool isCached)
			{
				_el = el;
				_isCached = isCached;
			}

			#endregion

			#region Properties

			public string AcceleratorKey
			{
				get { return (string) _el.GetPropertyValue(AcceleratorKeyProperty, _isCached); }
			}

			public string AccessKey
			{
				get { return (string) _el.GetPropertyValue(AccessKeyProperty, _isCached); }
			}

			public string AriaProperties
			{
				get { return (string) _el.GetPropertyValue(AriaPropertiesProperty, _isCached); }
			}

			public string AriaRole
			{
				get { return (string) _el.GetPropertyValue(AriaRoleProperty, _isCached); }
			}

			public string AutomationId
			{
				get { return (string) _el.GetPropertyValue(AutomationIdProperty, _isCached); }
			}

			public Rect BoundingRectangle
			{
				get { return (Rect) _el.GetPropertyValue(BoundingRectangleProperty, _isCached); }
			}

			public string ClassName
			{
				get { return (string) _el.GetPropertyValue(ClassNameProperty, _isCached); }
			}

			public AutomationElement ControllerFor
			{
				get { return (AutomationElement) _el.GetPropertyValue(ControllerForProperty, _isCached); }
			}

			public ControlType ControlType
			{
				get { return (ControlType) _el.GetPropertyValue(ControlTypeProperty, _isCached); }
			}

			public AutomationElement DescribedBy
			{
				get { return (AutomationElement) _el.GetPropertyValue(DescribedByProperty, _isCached); }
			}

			public AutomationElement FlowsTo
			{
				get { return (AutomationElement) _el.GetPropertyValue(FlowsToProperty, _isCached); }
			}

			public string FrameworkId
			{
				get { return (string) _el.GetPropertyValue(FrameworkIdProperty, _isCached); }
			}

			public bool HasKeyboardFocus
			{
				get { return (bool) _el.GetPropertyValue(HasKeyboardFocusProperty, _isCached); }
			}

			public string HelpText
			{
				get { return (string) _el.GetPropertyValue(HelpTextProperty, _isCached); }
			}

			public bool IsContentElement
			{
				get { return (bool) _el.GetPropertyValue(IsContentElementProperty, _isCached); }
			}

			public bool IsControlElement
			{
				get { return (bool) _el.GetPropertyValue(IsControlElementProperty, _isCached); }
			}

			public bool IsDataValidForForm
			{
				get { return (bool) _el.GetPropertyValue(IsDataValidForFormProperty, _isCached); }
			}

			public bool IsEnabled
			{
				get { return (bool) _el.GetPropertyValue(IsEnabledProperty, _isCached); }
			}

			public bool IsKeyboardFocusable
			{
				get { return (bool) _el.GetPropertyValue(IsKeyboardFocusableProperty, _isCached); }
			}

			public bool IsOffscreen
			{
				get { return (bool) _el.GetPropertyValue(IsOffscreenProperty, _isCached); }
			}

			public bool IsPassword
			{
				get { return (bool) _el.GetPropertyValue(IsPasswordProperty, _isCached); }
			}

			public bool IsRequiredForForm
			{
				get { return (bool) _el.GetPropertyValue(IsRequiredForFormProperty, _isCached); }
			}

			public string ItemStatus
			{
				get { return (string) _el.GetPropertyValue(ItemStatusProperty, _isCached); }
			}

			public string ItemType
			{
				get { return (string) _el.GetPropertyValue(ItemTypeProperty, _isCached); }
			}

			public AutomationElement LabeledBy
			{
				get { return (AutomationElement) _el.GetPropertyValue(LabeledByProperty, _isCached); }
			}

			public string LocalizedControlType
			{
				get { return (string) _el.GetPropertyValue(LocalizedControlTypeProperty, _isCached); }
			}

			public string Name
			{
				get { return (string) _el.GetPropertyValue(NameProperty, _isCached); }
			}

			public int NativeWindowHandle
			{
				get { return (int) _el.GetPropertyValue(NativeWindowHandleProperty, _isCached); }
			}

			public OrientationType Orientation
			{
				get { return (OrientationType) _el.GetPropertyValue(OrientationProperty, _isCached); }
			}

			public int ProcessId
			{
				get { return (int) _el.GetPropertyValue(ProcessIdProperty, _isCached); }
			}

			public string ProviderDescription
			{
				get { return (string) _el.GetPropertyValue(ProviderDescriptionProperty, _isCached); }
			}

			#endregion
		}

		#endregion
	}
}