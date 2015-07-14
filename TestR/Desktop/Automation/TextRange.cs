#region References

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using TestR.Desktop.Automation.Patterns;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Automation
{
	public class TextPatternRange
	{
		#region Constructors

		internal TextPatternRange(IUIAutomationTextRange range, TextPattern pattern)
		{
			Debug.Assert(range != null);
			Debug.Assert(pattern != null);
			NativeRange = range;
			TextPattern = pattern;
		}

		#endregion

		#region Properties

		public TextPattern TextPattern { get; private set; }
		internal IUIAutomationTextRange NativeRange { get; private set; }

		#endregion

		#region Methods

		public void AddToSelection()
		{
			try
			{
				NativeRange.AddToSelection();
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

		public TextPatternRange Clone()
		{
			try
			{
				return Wrap(NativeRange.Clone(), TextPattern);
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

		public bool Compare(TextPatternRange range)
		{
			try
			{
				return 0 != NativeRange.Compare(range.NativeRange);
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

		public int CompareEndpoints(TextPatternRangeEndpoint endpoint, TextPatternRange targetRange, TextPatternRangeEndpoint targetEndpoint)
		{
			try
			{
				return NativeRange.CompareEndpoints(
					(UIAutomationClient.TextPatternRangeEndpoint) endpoint,
					targetRange.NativeRange,
					(UIAutomationClient.TextPatternRangeEndpoint) targetEndpoint);
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

		public void ExpandToEnclosingUnit(TextUnit unit)
		{
			try
			{
				NativeRange.ExpandToEnclosingUnit((UIAutomationClient.TextUnit) unit);
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

		public TextPatternRange FindAttribute(AutomationTextAttribute attribute, object value, bool backward)
		{
			Utility.ValidateArgumentNonNull(attribute, "attribute");
			Utility.ValidateArgumentNonNull(value, "value");
			if ((attribute == TextPattern.CultureAttribute) && (value is CultureInfo))
			{
				value = ((CultureInfo) value).LCID;
			}
			try
			{
				return Wrap(
					NativeRange.FindAttribute(attribute.Id, value, Utility.ConvertToInt(backward)), TextPattern);
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

		public TextPatternRange FindText(string text, bool backward, bool ignoreCase)
		{
			try
			{
				return Wrap(
					NativeRange.FindText(text, Utility.ConvertToInt(backward), Utility.ConvertToInt(ignoreCase)), TextPattern);
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

		public object GetAttributeValue(AutomationTextAttribute attribute)
		{
			Utility.ValidateArgumentNonNull(attribute, "attribute");
			try
			{
				PropertyTypeInfo info;
				if (!Schema.GetPropertyTypeInfo(attribute, out info))
				{
					throw new ArgumentException("Unsupported Attribute");
				}
				var valueAsObject = NativeRange.GetAttributeValue(attribute.Id);
				if (info.Type.IsEnum && (valueAsObject is int))
				{
					return Enum.ToObject(info.Type, (int) valueAsObject);
				}
				if ((valueAsObject != AutomationElement.NotSupported) && (info.ObjectConverter != null))
				{
					valueAsObject = info.ObjectConverter(valueAsObject);
				}
				return valueAsObject;
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

		public Rect[] GetBoundingRectangles()
		{
			try
			{
				var unrolledRects = NativeRange.GetBoundingRectangles();
				Rect[] result = null;
				if (unrolledRects != null)
				{
					Debug.Assert(unrolledRects.Length % 4 == 0);
					// If unrolledRects is somehow not a multiple of 4, we still will not 
					// overrun it, since (x / 4) * 4 <= x for C# integer math.
					result = new Rect[unrolledRects.Length / 4];
					for (var i = 0; i < result.Length; i++)
					{
						var j = i * 4;
						;
						result[i] = new Rect(unrolledRects[j], unrolledRects[j + 1], unrolledRects[j + 2], unrolledRects[j + 3]);
					}
				}
				return result;
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

		public AutomationElement[] GetChildren()
		{
			try
			{
				return Utility.ConvertToElementArray(NativeRange.GetChildren());
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

		public AutomationElement GetEnclosingElement()
		{
			try
			{
				return AutomationElement.Wrap(NativeRange.GetEnclosingElement());
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

		public string GetText(int maxLength)
		{
			try
			{
				return NativeRange.GetText(maxLength);
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

		public int Move(TextUnit unit, int count)
		{
			try
			{
				return NativeRange.Move((UIAutomationClient.TextUnit) unit, count);
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

		public void MoveEndpointByRange(TextPatternRangeEndpoint endpoint, TextPatternRange targetRange, TextPatternRangeEndpoint targetEndpoint)
		{
			try
			{
				NativeRange.MoveEndpointByRange(
					(UIAutomationClient.TextPatternRangeEndpoint) endpoint,
					targetRange.NativeRange,
					(UIAutomationClient.TextPatternRangeEndpoint) targetEndpoint);
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

		public int MoveEndpointByUnit(TextPatternRangeEndpoint endpoint, TextUnit unit, int count)
		{
			try
			{
				return NativeRange.MoveEndpointByUnit(
					(UIAutomationClient.TextPatternRangeEndpoint) endpoint,
					(UIAutomationClient.TextUnit) unit,
					count);
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

		public void RemoveFromSelection()
		{
			try
			{
				NativeRange.RemoveFromSelection();
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

		public void ScrollIntoView(bool alignToTop)
		{
			try
			{
				NativeRange.ScrollIntoView(Utility.ConvertToInt(alignToTop));
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

		public void Select()
		{
			try
			{
				NativeRange.Select();
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

		internal static TextPatternRange Wrap(IUIAutomationTextRange range, TextPattern pattern)
		{
			Debug.Assert(pattern != null);
			if (range == null)
			{
				return null;
			}
			return new TextPatternRange(range, pattern);
		}

		internal static TextPatternRange[] Wrap(IUIAutomationTextRangeArray ranges, TextPattern pattern)
		{
			if (ranges == null)
			{
				return null;
			}
			var rangeArray = new TextPatternRange[ranges.Length];
			for (var i = 0; i < ranges.Length; i++)
			{
				rangeArray[i] = new TextPatternRange(ranges.GetElement(i), pattern);
			}
			return rangeArray;
		}

		#endregion
	}
}