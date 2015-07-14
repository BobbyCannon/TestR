// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

#region References

using System;
using System.Collections;
using System.Diagnostics;
using UIAutomationClient;

#endregion

namespace UIAComWrapper
{
	public class AutomationElementCollection : ICollection, IEnumerable
	{
		#region Fields

		private readonly IUIAutomationElementArray _obj;

		#endregion

		#region Constructors

		internal AutomationElementCollection(IUIAutomationElementArray obj)
		{
			Debug.Assert(obj != null);
			_obj = obj;
		}

		#endregion

		#region Properties

		public int Count
		{
			get { return _obj.Length; }
		}

		public virtual bool IsSynchronized
		{
			get { return false; }
		}

		public virtual object SyncRoot
		{
			get { return this; }
		}

		#endregion

		#region Methods

		public virtual void CopyTo(Array array, int index)
		{
			var cElem = _obj.Length;
			for (var i = 0; i < cElem; ++i)
			{
				array.SetValue(this[i], i + index);
			}
		}

		public void CopyTo(AutomationElement[] array, int index)
		{
			var cElem = _obj.Length;
			for (var i = 0; i < cElem; ++i)
			{
				array.SetValue(this[i], i + index);
			}
		}

		public IEnumerator GetEnumerator()
		{
			return new AutomationElementCollectionEnumerator(_obj);
		}

		internal static AutomationElementCollection Wrap(IUIAutomationElementArray obj)
		{
			return (obj == null) ? null : new AutomationElementCollection(obj);
		}

		#endregion

		#region Indexers

		public AutomationElement this[int index]
		{
			get { return AutomationElement.Wrap(_obj.GetElement(index)); }
		}

		#endregion
	}

	public class AutomationElementCollectionEnumerator : IEnumerator
	{
		#region Fields

		private readonly int _cElem;
		private int _index;
		private readonly IUIAutomationElementArray _obj;

		#endregion

		#region Constructors

		internal AutomationElementCollectionEnumerator(IUIAutomationElementArray obj)
		{
			Debug.Assert(obj != null);
			_obj = obj;
			_cElem = obj.Length;
		}

		#endregion

		#region Properties

		public object Current
		{
			get { return AutomationElement.Wrap(_obj.GetElement(_index)); }
		}

		#endregion

		#region Methods

		public bool MoveNext()
		{
			if (_index < (_cElem - 1))
			{
				++_index;
				return true;
			}
			return false;
		}

		public void Reset()
		{
			_index = 0;
		}

		#endregion
	}
}