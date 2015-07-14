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
	public sealed class TreeWalker
	{
		#region Fields

		public static readonly TreeWalker ContentViewWalker = new TreeWalker(Automation.ContentViewCondition);
		public static readonly TreeWalker ControlViewWalker = new TreeWalker(Automation.ControlViewCondition);
		public static readonly TreeWalker RawViewWalker = new TreeWalker(Automation.RawViewCondition);
		private readonly IUIAutomationTreeWalker _obj;

		#endregion

		#region Constructors

		public TreeWalker(Condition condition)
		{
			// This is an unusual situation - a direct constructor.
			// We have to go create the native tree walker, which might throw.
			Utility.ValidateArgumentNonNull(condition, "condition");
			try
			{
				_obj = Automation.Factory.CreateTreeWalker(condition.NativeCondition);
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

		internal TreeWalker(IUIAutomationTreeWalker obj)
		{
			Debug.Assert(obj != null);
			_obj = obj;
		}

		#endregion

		#region Properties

		public Condition Condition
		{
			get
			{
				try
				{
					return Condition.Wrap(_obj.condition);
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
		}

		#endregion

		#region Methods

		public AutomationElement GetFirstChild(AutomationElement element)
		{
			return GetFirstChild(element, CacheRequest.Current);
		}

		public AutomationElement GetFirstChild(AutomationElement element, CacheRequest request)
		{
			Utility.ValidateArgumentNonNull(element, "element");
			Utility.ValidateArgumentNonNull(request, "request");
			try
			{
				return AutomationElement.Wrap(_obj.GetFirstChildElementBuildCache(
					element.NativeElement,
					request.NativeCacheRequest));
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

		public AutomationElement GetLastChild(AutomationElement element)
		{
			return GetLastChild(element, CacheRequest.Current);
		}

		public AutomationElement GetLastChild(AutomationElement element, CacheRequest request)
		{
			Utility.ValidateArgumentNonNull(element, "element");
			Utility.ValidateArgumentNonNull(request, "request");
			try
			{
				return AutomationElement.Wrap(_obj.GetLastChildElementBuildCache(
					element.NativeElement,
					request.NativeCacheRequest));
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

		public AutomationElement GetNextSibling(AutomationElement element)
		{
			return GetNextSibling(element, CacheRequest.Current);
		}

		public AutomationElement GetNextSibling(AutomationElement element, CacheRequest request)
		{
			Utility.ValidateArgumentNonNull(element, "element");
			Utility.ValidateArgumentNonNull(request, "request");
			try
			{
				return AutomationElement.Wrap(_obj.GetNextSiblingElementBuildCache(
					element.NativeElement,
					request.NativeCacheRequest));
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

		public AutomationElement GetParent(AutomationElement element)
		{
			return GetParent(element, CacheRequest.Current);
		}

		public AutomationElement GetParent(AutomationElement element, CacheRequest request)
		{
			Utility.ValidateArgumentNonNull(element, "element");
			Utility.ValidateArgumentNonNull(request, "request");
			try
			{
				return AutomationElement.Wrap(_obj.GetParentElementBuildCache(
					element.NativeElement,
					request.NativeCacheRequest));
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

		public AutomationElement GetPreviousSibling(AutomationElement element)
		{
			return GetPreviousSibling(element, CacheRequest.Current);
		}

		public AutomationElement GetPreviousSibling(AutomationElement element, CacheRequest request)
		{
			Utility.ValidateArgumentNonNull(element, "element");
			Utility.ValidateArgumentNonNull(request, "request");
			try
			{
				return AutomationElement.Wrap(_obj.GetPreviousSiblingElementBuildCache(
					element.NativeElement,
					request.NativeCacheRequest));
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

		public AutomationElement Normalize(AutomationElement element)
		{
			return Normalize(element, CacheRequest.Current);
		}

		public AutomationElement Normalize(AutomationElement element, CacheRequest request)
		{
			Utility.ValidateArgumentNonNull(element, "element");
			Utility.ValidateArgumentNonNull(request, "request");
			try
			{
				return AutomationElement.Wrap(_obj.NormalizeElementBuildCache(
					element.NativeElement,
					request.NativeCacheRequest));
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

		internal TreeWalker Wrap(IUIAutomationTreeWalker obj)
		{
			return (obj == null) ? null : Wrap(obj);
		}

		#endregion
	}
}