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
	public sealed class CacheRequest
	{
		#region Fields

		internal static readonly CacheRequest DefaultCacheRequest = new CacheRequest();

		[ThreadStatic]
		private static Stack _cacheStack;

		private int _cRef;
		private readonly object _lock;

		#endregion

		#region Constructors

		public CacheRequest()
		{
			NativeCacheRequest = Automation.Factory.CreateCacheRequest();
			_lock = new object();
		}

		internal CacheRequest(IUIAutomationCacheRequest obj)
		{
			Debug.Assert(obj != null);
			NativeCacheRequest = obj;
			_lock = new object();
		}

		#endregion

		#region Properties

		public AutomationElementMode AutomationElementMode
		{
			get { return (AutomationElementMode) NativeCacheRequest.AutomationElementMode; }
			set
			{
				lock (_lock)
				{
					CheckAccess();
					NativeCacheRequest.AutomationElementMode = (UIAutomationClient.AutomationElementMode) value;
				}
			}
		}

		public static CacheRequest Current
		{
			get
			{
				if ((_cacheStack != null) && (_cacheStack.Count != 0))
				{
					return (CacheRequest) _cacheStack.Peek();
				}
				return DefaultCacheRequest;
			}
		}

		public Condition TreeFilter
		{
			get { return Condition.Wrap(NativeCacheRequest.TreeFilter); }
			set
			{
				Utility.ValidateArgumentNonNull(value, "TreeFilter");
				lock (_lock)
				{
					CheckAccess();
					NativeCacheRequest.TreeFilter = value.NativeCondition;
				}
			}
		}

		public TreeScope TreeScope
		{
			get { return (TreeScope) NativeCacheRequest.TreeScope; }
			set
			{
				lock (_lock)
				{
					CheckAccess();
					NativeCacheRequest.TreeScope = (UIAutomationClient.TreeScope) value;
				}
			}
		}

		internal static IUIAutomationCacheRequest CurrentNativeCacheRequest
		{
			get { return Current.NativeCacheRequest; }
		}

		internal IUIAutomationCacheRequest NativeCacheRequest { get; private set; }

		#endregion

		#region Methods

		public IDisposable Activate()
		{
			Push();
			return new CacheRequestActivation(this);
		}

		public void Add(AutomationPattern pattern)
		{
			Utility.ValidateArgumentNonNull(pattern, "pattern");
			lock (_lock)
			{
				CheckAccess();
				NativeCacheRequest.AddPattern(pattern.Id);
			}
		}

		public void Add(AutomationProperty property)
		{
			Utility.ValidateArgumentNonNull(property, "property");
			lock (_lock)
			{
				CheckAccess();
				NativeCacheRequest.AddProperty(property.Id);
			}
		}

		public CacheRequest Clone()
		{
			return new CacheRequest(NativeCacheRequest.Clone());
		}

		public void Pop()
		{
			if (((_cacheStack == null) || (_cacheStack.Count == 0)) || (_cacheStack.Peek() != this))
			{
				throw new InvalidOperationException("Only the top cache request can be popped");
			}
			_cacheStack.Pop();
			lock (_lock)
			{
				_cRef--;
			}
		}

		public void Push()
		{
			if (_cacheStack == null)
			{
				_cacheStack = new Stack();
			}
			_cacheStack.Push(this);
			lock (_lock)
			{
				_cRef++;
			}
		}

		private void CheckAccess()
		{
			if ((_cRef != 0) || (this == DefaultCacheRequest))
			{
				throw new InvalidOperationException("Can't modify an active cache request");
			}
		}

		#endregion
	}

	internal class CacheRequestActivation : IDisposable
	{
		#region Fields

		private CacheRequest _request;

		#endregion

		#region Constructors

		internal CacheRequestActivation(CacheRequest request)
		{
			_request = request;
		}

		#endregion

		#region Methods

		public void Dispose()
		{
			if (_request != null)
			{
				_request.Pop();
				_request = null;
			}
		}

		#endregion
	}
}