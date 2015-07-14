#region References

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Automation.Patterns
{
	public class TransformPattern : BasePattern
	{
		#region Fields

		public static readonly AutomationProperty CanMoveProperty = TransformPatternIdentifiers.CanMoveProperty;
		public static readonly AutomationProperty CanResizeProperty = TransformPatternIdentifiers.CanResizeProperty;
		public static readonly AutomationProperty CanRotateProperty = TransformPatternIdentifiers.CanRotateProperty;
		public static readonly AutomationPattern Pattern = TransformPatternIdentifiers.Pattern;
		private readonly IUIAutomationTransformPattern _pattern;

		#endregion

		#region Constructors

		private TransformPattern(AutomationElement el, IUIAutomationTransformPattern pattern, bool cached)
			: base(el, cached, Pattern.Id, Pattern.Guid, Pattern.ProgrammaticName)
		{
			Debug.Assert(pattern != null);
			_pattern = pattern;
		}

		#endregion

		#region Properties

		public TransformPatternInformation Cached
		{
			get
			{
				Utility.ValidateCached(_cached);
				return new TransformPatternInformation(_el, true);
			}
		}

		public TransformPatternInformation Current
		{
			get { return new TransformPatternInformation(_el, false); }
		}

		#endregion

		#region Methods

		public void Move(double x, double y)
		{
			try
			{
				_pattern.Move(x, y);
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

		public void Resize(double width, double height)
		{
			try
			{
				_pattern.Resize(width, height);
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

		public void Rotate(double degrees)
		{
			try
			{
				_pattern.Rotate(degrees);
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

		internal static object Wrap(AutomationElement el, object pattern, bool cached)
		{
			return (pattern == null) ? null : new TransformPattern(el, (IUIAutomationTransformPattern) pattern, cached);
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct TransformPatternInformation
		{
			#region Fields

			private readonly AutomationElement _el;
			private readonly bool _isCached;

			#endregion

			#region Constructors

			internal TransformPatternInformation(AutomationElement element, bool isCached)
			{
				_el = element;
				_isCached = isCached;
			}

			#endregion

			#region Properties

			public bool CanMove
			{
				get { return (bool) _el.GetPropertyValue(CanMoveProperty, _isCached); }
			}

			public bool CanResize
			{
				get { return (bool) _el.GetPropertyValue(CanResizeProperty, _isCached); }
			}

			public bool CanRotate
			{
				get { return (bool) _el.GetPropertyValue(CanRotateProperty, _isCached); }
			}

			#endregion
		}

		#endregion
	}
}