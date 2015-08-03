#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Pattern
{
	public class ValuePattern : BasePattern
	{
		#region Constructors

		private ValuePattern(Element element)
			: base(element)
		{
		}

		#endregion

		#region Properties

		public bool IsReadOnly => GetPattern<IUIAutomationValuePattern>()?.CurrentIsReadOnly == 1;
		public string Value => GetPattern<IUIAutomationValuePattern>()?.CurrentValue ?? string.Empty;

		#endregion

		#region Methods

		public static ValuePattern New(Element element)
		{
			return new ValuePattern(element);
		}

		public void SetValue(string value)
		{
			GetPattern<IUIAutomationValuePattern>()?.SetValue(value);
		}

		#endregion
	}
}