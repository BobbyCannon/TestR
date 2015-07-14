// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

#region References

using System;
using System.Runtime.InteropServices;
using UIAutomationClient;

#endregion

namespace UIAComWrapper
{
	public static class Automation
	{
		#region Fields

		private static readonly IUIAutomation factory = new CUIAutomationClass();
		public static readonly Condition ContentViewCondition = Condition.Wrap(Factory.ContentViewCondition);
		public static readonly Condition ControlViewCondition = Condition.Wrap(Factory.ControlViewCondition);
		public static readonly Condition RawViewCondition = Condition.Wrap(Factory.RawViewCondition);

		#endregion

		#region Constructors

		// Explicit static constructor to tell C# compiler
		// not to mark type as beforefieldinit
		static Automation()
		{
		}

		#endregion

		#region Properties

		internal static IUIAutomation Factory
		{
			get { return factory; }
		}

		#endregion

		#region Methods

		public static void AddAutomationEventHandler(AutomationEvent eventId, AutomationElement element, TreeScope scope, AutomationEventHandler eventHandler)
		{
			Utility.ValidateArgumentNonNull(element, "element");
			Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");
			Utility.ValidateArgument(eventId != AutomationElement.AutomationFocusChangedEvent, "Use FocusChange notification instead");
			Utility.ValidateArgument(eventId != AutomationElement.StructureChangedEvent, "Use StructureChange notification instead");
			Utility.ValidateArgument(eventId != AutomationElement.AutomationPropertyChangedEvent, "Use PropertyChange notification instead");

			try
			{
				var listener = new BasicEventListener(eventId, element, eventHandler);
				Factory.AddAutomationEventHandler(
					eventId.Id,
					element.NativeElement,
					(UIAutomationClient.TreeScope) scope,
					CacheRequest.CurrentNativeCacheRequest,
					listener);
				ClientEventList.Add(listener);
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

		public static void AddAutomationFocusChangedEventHandler(AutomationFocusChangedEventHandler eventHandler)
		{
			Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");

			try
			{
				var listener = new FocusEventListener(eventHandler);
				Factory.AddFocusChangedEventHandler(CacheRequest.CurrentNativeCacheRequest, listener);
				ClientEventList.Add(listener);
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

		public static void AddAutomationPropertyChangedEventHandler(AutomationElement element, TreeScope scope, AutomationPropertyChangedEventHandler eventHandler, params AutomationProperty[] properties)
		{
			Utility.ValidateArgumentNonNull(element, "element");
			Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");
			Utility.ValidateArgumentNonNull(properties, "properties");
			if (properties.Length == 0)
			{
				throw new ArgumentException("AtLeastOnePropertyMustBeSpecified");
			}
			var propertyIdArray = new int[properties.Length];
			for (var i = 0; i < properties.Length; ++i)
			{
				Utility.ValidateArgumentNonNull(properties[i], "properties");
				propertyIdArray[i] = properties[i].Id;
			}

			try
			{
				var listener = new PropertyEventListener(AutomationElement.StructureChangedEvent, element, eventHandler);
				Factory.AddPropertyChangedEventHandler(
					element.NativeElement,
					(UIAutomationClient.TreeScope) scope,
					CacheRequest.CurrentNativeCacheRequest,
					listener,
					propertyIdArray);
				ClientEventList.Add(listener);
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

		public static void AddStructureChangedEventHandler(AutomationElement element, TreeScope scope, StructureChangedEventHandler eventHandler)
		{
			Utility.ValidateArgumentNonNull(element, "element");
			Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");
			Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");

			try
			{
				var listener = new StructureEventListener(AutomationElement.StructureChangedEvent, element, eventHandler);
				Factory.AddStructureChangedEventHandler(
					element.NativeElement,
					(UIAutomationClient.TreeScope) scope,
					CacheRequest.CurrentNativeCacheRequest,
					listener);
				ClientEventList.Add(listener);
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

		public static bool Compare(int[] runtimeId1, int[] runtimeId2)
		{
			if (runtimeId1 == null && runtimeId2 == null)
			{
				return true;
			}
			if (runtimeId1 == null || runtimeId2 == null)
			{
				return false;
			}
			try
			{
				return Factory.CompareRuntimeIds(runtimeId1, runtimeId2) != 0;
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

		public static bool Compare(AutomationElement el1, AutomationElement el2)
		{
			if (el1 == null && el2 == null)
			{
				return true;
			}
			if (el1 == null || el2 == null)
			{
				return false;
			}
			try
			{
				return Factory.CompareElements(el1.NativeElement, el2.NativeElement) != 0;
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

		public static string PatternName(AutomationPattern pattern)
		{
			Utility.ValidateArgumentNonNull(pattern, "pattern");
			return Factory.GetPatternProgrammaticName(pattern.Id);
		}

		public static string PropertyName(AutomationProperty property)
		{
			Utility.ValidateArgumentNonNull(property, "property");
			return Factory.GetPropertyProgrammaticName(property.Id);
		}

		public static void RemoveAllEventHandlers()
		{
			try
			{
				Factory.RemoveAllEventHandlers();
				ClientEventList.Clear();
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

		public static void RemoveAutomationEventHandler(AutomationEvent eventId, AutomationElement element, AutomationEventHandler eventHandler)
		{
			Utility.ValidateArgumentNonNull(element, "element");
			Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");
			Utility.ValidateArgument(eventId != AutomationElement.AutomationFocusChangedEvent, "Use FocusChange notification instead");
			Utility.ValidateArgument(eventId != AutomationElement.StructureChangedEvent, "Use StructureChange notification instead");
			Utility.ValidateArgument(eventId != AutomationElement.AutomationPropertyChangedEvent, "Use PropertyChange notification instead");

			try
			{
				var listener = (BasicEventListener) ClientEventList.Remove(eventId, element, eventHandler);
				Factory.RemoveAutomationEventHandler(eventId.Id, element.NativeElement, listener);
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

		public static void RemoveAutomationFocusChangedEventHandler(AutomationFocusChangedEventHandler eventHandler)
		{
			Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");

			try
			{
				var listener = (FocusEventListener) ClientEventList.Remove(AutomationElement.AutomationFocusChangedEvent, null, eventHandler);
				Factory.RemoveFocusChangedEventHandler(listener);
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

		public static void RemoveAutomationPropertyChangedEventHandler(AutomationElement element, AutomationPropertyChangedEventHandler eventHandler)
		{
			Utility.ValidateArgumentNonNull(element, "element");
			Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");

			try
			{
				var listener = (PropertyEventListener) ClientEventList.Remove(AutomationElement.AutomationPropertyChangedEvent, element, eventHandler);
				Factory.RemovePropertyChangedEventHandler(element.NativeElement, listener);
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

		public static void RemoveStructureChangedEventHandler(AutomationElement element, StructureChangedEventHandler eventHandler)
		{
			Utility.ValidateArgumentNonNull(element, "element");
			Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");

			try
			{
				var listener = (StructureEventListener) ClientEventList.Remove(AutomationElement.StructureChangedEvent, element, eventHandler);
				Factory.RemoveStructureChangedEventHandler(element.NativeElement, listener);
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
	}
}