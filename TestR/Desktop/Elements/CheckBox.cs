#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a check box element.
	/// </summary>
	public class CheckBox : Element
	{
		#region Constructors

		internal CheckBox(IUIAutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a flag indicating if the checkbox is checked.
		/// </summary>
		public bool Checked
		{
			get
			{
				var pattern = NativeElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) as IUIAutomationTogglePattern;
				return pattern?.CurrentToggleState != UIAutomationClient.ToggleState.ToggleState_Off;
			}
		}

		/// <summary>
		/// Gets the state of the checkbox.
		/// </summary>
		public ToggleState CheckedState
		{
			get
			{
				var pattern = NativeElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) as IUIAutomationTogglePattern;
				return Convert(pattern?.CurrentToggleState);
			}
		}

		/// <summary>
		/// Gets the text value.
		/// </summary>
		public string Text => Name;

		#endregion

		#region Methods

		/// <summary>
		/// Toggle the checkbox.
		/// </summary>
		public void Toggle()
		{
			var pattern = NativeElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) as IUIAutomationTogglePattern;
			pattern?.Toggle();
		}

		private static ToggleState Convert(UIAutomationClient.ToggleState? state)
		{
			switch (state)
			{
				case UIAutomationClient.ToggleState.ToggleState_On:
					return ToggleState.On;

				case UIAutomationClient.ToggleState.ToggleState_Off:
					return ToggleState.Off;

				default:
					return ToggleState.Indeterminate;
			}
		}

		#endregion
	}
}