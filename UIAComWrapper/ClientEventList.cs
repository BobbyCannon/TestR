// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

#region References

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UIAutomationClient;

#endregion

namespace UIAComWrapper
{
	internal class EventListener
	{
		#region Constructors

		public EventListener(int eventId, int[] runtimeId, Delegate handler)
		{
			Debug.Assert(handler != null);

			EventId = eventId;
			RuntimeId = runtimeId;
			Handler = handler;
		}

		#endregion

		#region Properties

		public int EventId { get; private set; }
		public Delegate Handler { get; private set; }
		public int[] RuntimeId { get; private set; }

		#endregion

		#region Methods

		public override bool Equals(object obj)
		{
			var listener = obj as EventListener;
			return (listener != null &&
				EventId == listener.EventId &&
				Handler == listener.Handler &&
				Automation.Compare(RuntimeId, listener.RuntimeId));
		}

		public override int GetHashCode()
		{
			return Handler.GetHashCode();
		}

		#endregion
	}

	internal class FocusEventListener : EventListener, IUIAutomationFocusChangedEventHandler
	{
		#region Fields

		private readonly AutomationFocusChangedEventHandler _focusHandler;

		#endregion

		#region Constructors

		public FocusEventListener(AutomationFocusChangedEventHandler handler) :
			base(AutomationElement.AutomationFocusChangedEvent.Id, null, handler)
		{
			Debug.Assert(handler != null);
			_focusHandler = handler;
		}

		#endregion

		#region Methods

		void IUIAutomationFocusChangedEventHandler.HandleFocusChangedEvent(
			IUIAutomationElement sender)
		{
			// Can't set the arguments -- they come from a WinEvent handler.
			var args = new AutomationFocusChangedEventArgs(0, 0);
			_focusHandler(AutomationElement.Wrap(sender), args);
		}

		#endregion
	}

	internal class BasicEventListener : EventListener, IUIAutomationEventHandler
	{
		#region Fields

		private readonly AutomationEventHandler _basicHandler;

		#endregion

		#region Constructors

		public BasicEventListener(AutomationEvent eventKind, AutomationElement element, AutomationEventHandler handler) :
			base(eventKind.Id, element.GetRuntimeId(), handler)
		{
			Debug.Assert(handler != null);
			_basicHandler = handler;
		}

		#endregion

		#region Methods

		void IUIAutomationEventHandler.HandleAutomationEvent(
			IUIAutomationElement sender, int eventId)
		{
			AutomationEventArgs args;
			if (eventId != WindowPatternIdentifiers.WindowClosedEvent.Id)
			{
				args = new AutomationEventArgs(AutomationEvent.LookupById(eventId));
			}
			else
			{
				args = new WindowClosedEventArgs((int[]) sender.GetRuntimeId());
			}
			_basicHandler(AutomationElement.Wrap(sender), args);
		}

		#endregion
	}

	internal class PropertyEventListener : EventListener, IUIAutomationPropertyChangedEventHandler
	{
		#region Fields

		private readonly AutomationPropertyChangedEventHandler _propChangeHandler;

		#endregion

		#region Constructors

		public PropertyEventListener(AutomationEvent eventKind, AutomationElement element, AutomationPropertyChangedEventHandler handler) :
			base(AutomationElement.AutomationPropertyChangedEvent.Id, element.GetRuntimeId(), handler)
		{
			Debug.Assert(handler != null);
			_propChangeHandler = handler;
		}

		#endregion

		#region Methods

		void IUIAutomationPropertyChangedEventHandler.HandlePropertyChangedEvent(
			IUIAutomationElement sender,
			int propertyId,
			object newValue)
		{
			var property = AutomationProperty.LookupById(propertyId);
			var wrappedObj = Utility.WrapObjectAsProperty(property, newValue);
			var args = new AutomationPropertyChangedEventArgs(
				property,
				null,
				wrappedObj);
			_propChangeHandler(AutomationElement.Wrap(sender), args);
		}

		#endregion
	}

	internal class StructureEventListener : EventListener, IUIAutomationStructureChangedEventHandler
	{
		#region Fields

		private readonly StructureChangedEventHandler _structureChangeHandler;

		#endregion

		#region Constructors

		public StructureEventListener(AutomationEvent eventKind, AutomationElement element, StructureChangedEventHandler handler) :
			base(AutomationElement.StructureChangedEvent.Id, element.GetRuntimeId(), handler)
		{
			Debug.Assert(handler != null);
			_structureChangeHandler = handler;
		}

		#endregion

		#region Methods

		void IUIAutomationStructureChangedEventHandler.HandleStructureChangedEvent(IUIAutomationElement sender, UIAutomationClient.StructureChangeType changeType, int[] runtimeId)
		{
			var args = new StructureChangedEventArgs(
				(StructureChangeType) changeType,
				(int[]) runtimeId);
			_structureChangeHandler(AutomationElement.Wrap(sender), args);
		}

		#endregion
	}

	internal class ClientEventList
	{
		#region Fields

		private static readonly LinkedList<EventListener> _events = new LinkedList<EventListener>();

		#endregion

		#region Methods

		public static void Add(EventListener listener)
		{
			lock (_events)
			{
				_events.AddLast(listener);
			}
		}

		public static void Clear()
		{
			lock (_events)
			{
				_events.Clear();
			}
		}

		public static EventListener Remove(AutomationEvent eventId, AutomationElement element, Delegate handler)
		{
			// Create a prototype to seek
			var runtimeId = (element == null) ? null : element.GetRuntimeId();
			var prototype = new EventListener(eventId.Id, runtimeId, handler);
			lock (_events)
			{
				var node = _events.Find(prototype);
				if (node == null)
				{
					throw new ArgumentException("event handler not found");
				}
				var result = node.Value;
				_events.Remove(node);
				return result;
			}
		}

		#endregion
	}
}