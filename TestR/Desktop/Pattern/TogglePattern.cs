#region References

using System;
using TestR.Extensions;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Pattern
{
	/// <summary>
	/// Represents the Windows toggle pattern.
	/// </summary>
	public class TogglePattern
	{
		private readonly IUIAutomationTogglePattern _pattern;

		#region Constructors

		private TogglePattern(IUIAutomationTogglePattern pattern)
		{
			_pattern = pattern;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the toggled value.
		/// </summary>
		public bool Toggled => _pattern.CurrentToggleState.Convert() != ToggleState.Off;

		/// <summary>
		/// Gets the toggle state of the element.
		/// </summary>
		public ToggleState ToggleState => _pattern.CurrentToggleState.Convert();

		#endregion

		#region Methods

		/// <summary>
		/// Create a new pattern for the provided element.
		/// </summary>
		/// <param name="element"> The element that supports the pattern. </param>
		/// <returns> The pattern if we could find one else null will be returned. </returns>
		public static TogglePattern Create(Element element)
		{
			var pattern = element.NativeElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) as IUIAutomationTogglePattern;
			return pattern == null ? null : new TogglePattern(pattern);
		}

		/// <summary>
		/// Toggle the element.
		/// </summary>
		public void Toggle()
		{
			_pattern.Toggle();
		}

		#endregion
	}
}