// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

#region References

using System;
using System.Diagnostics;
using UIAutomationClient;

#endregion

namespace UIAComWrapper
{
	public abstract class Condition
	{
		#region Fields

		public static readonly Condition FalseCondition = BoolCondition.Wrap(false);
		public static readonly Condition TrueCondition = BoolCondition.Wrap(true);

		#endregion

		#region Properties

		internal abstract IUIAutomationCondition NativeCondition { get; }

		#endregion

		#region Methods

		internal static IUIAutomationCondition[] ConditionArrayManagedToNative(
			Condition[] conditions)
		{
			var unwrappedConditions =
				new IUIAutomationCondition[conditions.Length];
			for (var i = 0; i < conditions.Length; ++i)
			{
				unwrappedConditions[i] = ConditionManagedToNative(conditions[i]);
			}
			return unwrappedConditions;
		}

		internal static Condition[] ConditionArrayNativeToManaged(
			Array conditions)
		{
			var wrappedConditions = new Condition[conditions.Length];
			for (var i = 0; i < conditions.Length; ++i)
			{
				wrappedConditions[i] = Wrap((IUIAutomationCondition) conditions.GetValue(i));
			}
			return wrappedConditions;
		}

		internal static IUIAutomationCondition ConditionManagedToNative(
			Condition condition)
		{
			return (condition == null) ? null : condition.NativeCondition;
		}

		internal static Condition Wrap(IUIAutomationCondition obj)
		{
			if (obj is IUIAutomationBoolCondition)
			{
				return new BoolCondition((IUIAutomationBoolCondition) obj);
			}
			if (obj is IUIAutomationAndCondition)
			{
				return new AndCondition((IUIAutomationAndCondition) obj);
			}
			if (obj is IUIAutomationOrCondition)
			{
				return new OrCondition((IUIAutomationOrCondition) obj);
			}
			if (obj is IUIAutomationNotCondition)
			{
				return new NotCondition((IUIAutomationNotCondition) obj);
			}
			if (obj is IUIAutomationPropertyCondition)
			{
				return new PropertyCondition((IUIAutomationPropertyCondition) obj);
			}
			throw new ArgumentException("obj");
		}

		#endregion

		#region Classes

		private class BoolCondition : Condition
		{
			#region Fields

			internal readonly IUIAutomationBoolCondition _obj;

			#endregion

			#region Constructors

			internal BoolCondition(IUIAutomationBoolCondition obj)
			{
				Debug.Assert(obj != null);
				_obj = obj;
			}

			#endregion

			#region Properties

			internal override IUIAutomationCondition NativeCondition
			{
				get { return _obj; }
			}

			#endregion

			#region Methods

			internal static BoolCondition Wrap(bool b)
			{
				var obj = (IUIAutomationBoolCondition) ((b) ?
					Automation.Factory.CreateTrueCondition() :
					Automation.Factory.CreateFalseCondition());
				return new BoolCondition(obj);
			}

			#endregion
		}

		#endregion
	}

	public class NotCondition : Condition
	{
		#region Fields

		internal IUIAutomationNotCondition _obj;

		#endregion

		#region Constructors

		public NotCondition(Condition condition)
		{
			_obj = (IUIAutomationNotCondition)
				Automation.Factory.CreateNotCondition(
					ConditionManagedToNative(condition));
		}

		internal NotCondition(IUIAutomationNotCondition obj)
		{
			Debug.Assert(obj != null);
			_obj = obj;
		}

		#endregion

		#region Properties

		public Condition Condition
		{
			get { return Wrap(_obj.GetChild()); }
		}

		internal override IUIAutomationCondition NativeCondition
		{
			get { return _obj; }
		}

		#endregion
	}

	public class AndCondition : Condition
	{
		#region Fields

		internal IUIAutomationAndCondition _obj;

		#endregion

		#region Constructors

		public AndCondition(params Condition[] conditions)
		{
			_obj = (IUIAutomationAndCondition)
				Automation.Factory.CreateAndConditionFromArray(
					ConditionArrayManagedToNative(conditions));
		}

		internal AndCondition(IUIAutomationAndCondition obj)
		{
			Debug.Assert(obj != null);
			_obj = obj;
		}

		#endregion

		#region Properties

		internal override IUIAutomationCondition NativeCondition
		{
			get { return _obj; }
		}

		#endregion

		#region Methods

		public Condition[] GetConditions()
		{
			return ConditionArrayNativeToManaged(_obj.GetChildren());
		}

		#endregion
	}

	public class OrCondition : Condition
	{
		#region Fields

		internal IUIAutomationOrCondition _obj;

		#endregion

		#region Constructors

		public OrCondition(params Condition[] conditions)
		{
			_obj = (IUIAutomationOrCondition)
				Automation.Factory.CreateOrConditionFromArray(
					ConditionArrayManagedToNative(conditions));
		}

		internal OrCondition(IUIAutomationOrCondition obj)
		{
			Debug.Assert(obj != null);
			_obj = obj;
		}

		#endregion

		#region Properties

		internal override IUIAutomationCondition NativeCondition
		{
			get { return _obj; }
		}

		#endregion

		#region Methods

		public Condition[] GetConditions()
		{
			return ConditionArrayNativeToManaged(_obj.GetChildren());
		}

		#endregion
	}

	[Flags]
	public enum PropertyConditionFlags
	{
		None,
		IgnoreCase
	}

	public class PropertyCondition : Condition
	{
		#region Fields

		internal IUIAutomationPropertyCondition _obj;

		#endregion

		#region Constructors

		public PropertyCondition(AutomationProperty property, object value)
		{
			Init(property, value, PropertyConditionFlags.None);
		}

		public PropertyCondition(AutomationProperty property, object value, PropertyConditionFlags flags)
		{
			Init(property, value, flags);
		}

		internal PropertyCondition(IUIAutomationPropertyCondition obj)
		{
			Debug.Assert(obj != null);
			_obj = obj;
		}

		#endregion

		#region Properties

		public PropertyConditionFlags Flags
		{
			get { return (PropertyConditionFlags) _obj.PropertyConditionFlags; }
		}

		public AutomationProperty Property
		{
			get { return AutomationProperty.LookupById(_obj.propertyId); }
		}

		public object Value
		{
			get { return _obj.PropertyValue; }
		}

		internal override IUIAutomationCondition NativeCondition
		{
			get { return _obj; }
		}

		#endregion

		#region Methods

		private void Init(AutomationProperty property, object val, PropertyConditionFlags flags)
		{
			Utility.ValidateArgumentNonNull(property, "property");

			_obj = (IUIAutomationPropertyCondition)
				Automation.Factory.CreatePropertyConditionEx(
					property.Id,
					Utility.UnwrapObject(val),
					(UIAutomationClient.PropertyConditionFlags) flags);
		}

		#endregion
	}
}