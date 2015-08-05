#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Pattern
{
	public class ValuePattern
	{
		#region Fields

		private readonly IUIAutomationValuePattern _pattern;

		#endregion

		#region Constructors

		private ValuePattern(IUIAutomationValuePattern pattern)
		{
			_pattern = pattern;
		}

		#endregion

		#region Properties

		public bool IsReadOnly => _pattern.CurrentIsReadOnly == 1;
		public string Value => _pattern.CurrentValue;

		#endregion

		#region Methods

		public static ValuePattern New(Element element)
		{
			var pattern = element.NativeElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
			return pattern == null ? null : new ValuePattern(pattern);
		}

		public void SetValue(string value)
		{
			_pattern.SetValue(value);
		}

		#endregion
	}
}