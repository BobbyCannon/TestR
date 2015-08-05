#region References

using TestR.Extensions;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Pattern
{
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

		public bool Toggled => _pattern.CurrentToggleState.Convert() != ToggleState.Off;
		public ToggleState ToggleState => _pattern.CurrentToggleState.Convert();

		#endregion

		#region Methods

		public static TogglePattern New(Element element)
		{
			var pattern = element.NativeElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) as IUIAutomationTogglePattern;
			return pattern == null ? null : new TogglePattern(pattern);
		}

		public void Toggle()
		{
			_pattern.Toggle();
		}

		#endregion
	}
}