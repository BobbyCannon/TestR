#region References

using TestR.Extensions;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Pattern
{
	public class TogglePattern : BasePattern
	{
		#region Constructors

		private TogglePattern(Element element)
			: base(element)
		{
		}

		#endregion

		#region Properties

		public bool Toggled => GetPattern<IUIAutomationTogglePattern>()?.CurrentToggleState.Convert() != ToggleState.Off;
		public ToggleState ToggleState => GetPattern<IUIAutomationTogglePattern>()?.CurrentToggleState.Convert() ?? ToggleState.Off;

		#endregion

		#region Methods

		public static TogglePattern New(Element element)
		{
			return new TogglePattern(element);
		}

		public void Toggle()
		{
			GetPattern<IUIAutomationTogglePattern>()?.Toggle();
		}

		#endregion
	}
}