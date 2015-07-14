// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

#region References

using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using UIAutomationClient;

#endregion

namespace UIAComWrapper
{
	internal class Utility
	{
		#region Methods

		internal static Array CombineArrays(IEnumerable arrays, Type t)
		{
			var length = 0;
			foreach (Array array in arrays)
			{
				length += array.Length;
			}
			var destinationArray = Array.CreateInstance(t, length);
			var destinationIndex = 0;
			foreach (Array array3 in arrays)
			{
				var num3 = array3.Length;
				Array.Copy(array3, 0, destinationArray, destinationIndex, num3);
				destinationIndex += num3;
			}
			return destinationArray;
		}

		internal static bool ConvertException(COMException e, out Exception uiaException)
		{
			var handled = true;
			switch (e.ErrorCode)
			{
				case UiaCoreIds.UIA_E_ELEMENTNOTAVAILABLE:
					uiaException = new ElementNotAvailableException(e);
					break;

				case UiaCoreIds.UIA_E_ELEMENTNOTENABLED:
					uiaException = new ElementNotEnabledException(e);
					break;

				case UiaCoreIds.UIA_E_NOCLICKABLEPOINT:
					uiaException = new NoClickablePointException(e);
					break;

				case UiaCoreIds.UIA_E_PROXYASSEMBLYNOTLOADED:
					uiaException = new ProxyAssemblyNotLoadedException(e);
					break;

				default:
					uiaException = null;
					handled = false;
					break;
			}
			return handled;
		}

		internal static ControlType ConvertToControlType(int id)
		{
			return ControlType.LookupById(id);
		}

		internal static AutomationElement[] ConvertToElementArray(IUIAutomationElementArray array)
		{
			AutomationElement[] elementArray;
			if (array != null)
			{
				elementArray = new AutomationElement[array.Length];
				for (var i = 0; i < array.Length; i++)
				{
					elementArray[i] = AutomationElement.Wrap(array.GetElement(i));
				}
			}
			else
			{
				elementArray = null;
			}
			return elementArray;
		}

		internal static int ConvertToInt(bool b)
		{
			return (b) ? 1 : 0;
		}

		internal static Rect ConvertToRect(tagRECT rc)
		{
			return new Rect(rc.left, rc.top, rc.right - rc.left, rc.bottom - rc.top);
		}

		internal static tagPOINT PointManagedToNative(Point pt)
		{
			var nativePoint = new tagPOINT();
			nativePoint.x = (int) pt.X;
			nativePoint.y = (int) pt.Y;
			return nativePoint;
		}

		internal static Array RemoveDuplicates(Array a, Type t)
		{
			if (a.Length == 0)
			{
				return a;
			}
			Array.Sort(a);
			var index = 0;
			for (var i = 1; i < a.Length; i++)
			{
				if (!a.GetValue(i).Equals(a.GetValue(index)))
				{
					index++;
					a.SetValue(a.GetValue(i), index);
				}
			}
			var length = index + 1;
			if (length == a.Length)
			{
				return a;
			}
			var destinationArray = Array.CreateInstance(t, length);
			Array.Copy(a, 0, destinationArray, 0, length);
			return destinationArray;
		}

		// Unwrap an object from API representationt to what the native client will expect
		internal static object UnwrapObject(object val)
		{
			if (val != null)
			{
				if (val is ControlType)
				{
					val = ((ControlType) val).Id;
				}
				else if (val is Rect)
				{
					var rect = (Rect) val;
					val = new[] { rect.Left, rect.Top, rect.Width, rect.Height };
				}
				else if (val is Point)
				{
					var point = (Point) val;
					val = new[] { point.X, point.Y };
				}
				else if (val is CultureInfo)
				{
					val = ((CultureInfo) val).LCID;
				}
				else if (val is AutomationElement)
				{
					val = ((AutomationElement) val).NativeElement;
				}
			}
			return val;
		}

		internal static void ValidateArgument(bool cond, string reason)
		{
			if (!cond)
			{
				throw new ArgumentException(reason);
			}
		}

		internal static void ValidateArgumentNonNull(object obj, string argName)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(argName);
			}
		}

		internal static void ValidateCached(bool cached)
		{
			if (!cached)
			{
				throw new InvalidOperationException("Cache Request Needs Cache");
			}
		}

		internal static object WrapObjectAsPattern(AutomationElement el, object nativePattern, AutomationPattern pattern, bool cached)
		{
			PatternTypeInfo info;
			if (!Schema.GetPatternInfo(pattern, out info))
			{
				throw new ArgumentException("Unsupported pattern");
			}
			if (info.ClientSideWrapper == null)
			{
				return null;
			}
			return info.ClientSideWrapper(el, nativePattern, cached);
		}

		internal static object WrapObjectAsProperty(AutomationProperty property, object obj)
		{
			PropertyTypeInfo info;

			// Handle the cases that we know.
			if (obj == AutomationElement.NotSupported)
			{
				// No-op
			}
			else if (obj is IUIAutomationElement)
			{
				obj = AutomationElement.Wrap((IUIAutomationElement) obj);
			}
			else if (obj is IUIAutomationElementArray)
			{
				obj = ConvertToElementArray((IUIAutomationElementArray) obj);
			}
			else if (Schema.GetPropertyTypeInfo(property, out info))
			{
				// Well known properties
				if ((obj != null) && (info.ObjectConverter != null))
				{
					obj = info.ObjectConverter(obj);
				}
			}

			return obj;
		}

		private static void CheckNonNull(object el1, object el2)
		{
			if (el1 == null)
			{
				throw new ArgumentNullException("el1");
			}
			if (el2 == null)
			{
				throw new ArgumentNullException("el2");
			}
		}

		#endregion
	}
}