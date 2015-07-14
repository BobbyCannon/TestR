// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

#region References

using System;
using System.Runtime.InteropServices;

#endregion

namespace UIAComWrapper
{
	public class AutomationEventArgs : EventArgs
	{
		#region Constructors

		public AutomationEventArgs(AutomationEvent eventId)
		{
			EventId = eventId;
		}

		#endregion

		#region Properties

		public AutomationEvent EventId { get; private set; }

		#endregion
	}

	public delegate void AutomationEventHandler(object sender, AutomationEventArgs e);

	public sealed class WindowClosedEventArgs : AutomationEventArgs
	{
		#region Fields

		private readonly int[] _runtimeId;

		#endregion

		#region Constructors

		public WindowClosedEventArgs(int[] runtimeId)
			: base(WindowPatternIdentifiers.WindowClosedEvent)
		{
			if (runtimeId == null)
			{
				throw new ArgumentNullException("runtimeId");
			}
			_runtimeId = (int[]) runtimeId.Clone();
		}

		#endregion

		#region Methods

		public int[] GetRuntimeId()
		{
			return (int[]) _runtimeId.Clone();
		}

		#endregion
	}

	[Guid("d8e55844-7043-4edc-979d-593cc6b4775e")]
	[ComVisible(true)]
	public enum AsyncContentLoadedState
	{
		Beginning,
		Progress,
		Completed
	}

	public sealed class AsyncContentLoadedEventArgs : AutomationEventArgs
	{
		#region Constructors

		public AsyncContentLoadedEventArgs(AsyncContentLoadedState asyncContentState, double percentComplete)
			: base(AutomationElementIdentifiers.AsyncContentLoadedEvent)
		{
			AsyncContentLoadedState = asyncContentState;
			PercentComplete = percentComplete;
		}

		#endregion

		#region Properties

		public AsyncContentLoadedState AsyncContentLoadedState { get; private set; }
		public double PercentComplete { get; private set; }

		#endregion
	}

	public sealed class AutomationPropertyChangedEventArgs : AutomationEventArgs
	{
		#region Constructors

		public AutomationPropertyChangedEventArgs(AutomationProperty property, object oldValue, object newValue)
			: base(AutomationElementIdentifiers.AutomationPropertyChangedEvent)
		{
			OldValue = oldValue;
			NewValue = newValue;
			Property = property;
		}

		#endregion

		#region Properties

		public object NewValue { get; private set; }
		public object OldValue { get; private set; }
		public AutomationProperty Property { get; private set; }

		#endregion
	}

	public delegate void AutomationPropertyChangedEventHandler(object sender, AutomationPropertyChangedEventArgs e);

	public class AutomationFocusChangedEventArgs : AutomationEventArgs
	{
		#region Constructors

		public AutomationFocusChangedEventArgs(int idObject, int idChild)
			: base(AutomationElement.AutomationFocusChangedEvent)
		{
			ObjectId = idObject;
			ChildId = idChild;
		}

		#endregion

		#region Properties

		public int ChildId { get; private set; }
		public int ObjectId { get; private set; }

		#endregion
	}

	public delegate void AutomationFocusChangedEventHandler(object sender, AutomationFocusChangedEventArgs e);

	[Guid("e4cfef41-071d-472c-a65c-c14f59ea81eb")]
	[ComVisible(true)]
	public enum StructureChangeType
	{
		ChildAdded,
		ChildRemoved,
		ChildrenInvalidated,
		ChildrenBulkAdded,
		ChildrenBulkRemoved,
		ChildrenReordered
	}

	public sealed class StructureChangedEventArgs : AutomationEventArgs
	{
		#region Fields

		private readonly int[] _runtimeID;

		#endregion

		#region Constructors

		public StructureChangedEventArgs(StructureChangeType structureChangeType, int[] runtimeId)
			: base(AutomationElementIdentifiers.StructureChangedEvent)
		{
			if (runtimeId == null)
			{
				throw new ArgumentNullException("runtimeId");
			}
			StructureChangeType = structureChangeType;
			_runtimeID = (int[]) runtimeId.Clone();
		}

		#endregion

		#region Properties

		public StructureChangeType StructureChangeType { get; private set; }

		#endregion

		#region Methods

		public int[] GetRuntimeId()
		{
			return (int[]) _runtimeID.Clone();
		}

		#endregion
	}

	public delegate void StructureChangedEventHandler(object sender, StructureChangedEventArgs e);
}