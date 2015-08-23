#region References

using TestR.Extensions;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Pattern
{
	/// <summary>
	/// Represents the toggle pattern.
	/// </summary>
	public class TogglePattern : BasePattern
	{
		#region Constructors

		private TogglePattern(Element element)
			: base(element)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a Boolean representation of the toggle state.
		/// </summary>
		public bool Toggled => GetPattern<IUIAutomationTogglePattern>()?.CurrentToggleState.Convert() != ToggleState.Off;

		/// <summary>
		/// Gets the toggle state of the element.
		/// </summary>
		public ToggleState ToggleState => GetPattern<IUIAutomationTogglePattern>()?.CurrentToggleState.Convert() ?? ToggleState.Off;

		#endregion

		#region Methods

		/// <summary>
		/// Creates a new instance of this pattern.
		/// </summary>
		/// <param name="element"> The element this pattern is for. </param>
		/// <returns> The instance of the pattern. </returns>
		public static TogglePattern New(Element element)
		{
			return new TogglePattern(element);
		}

		/// <summary>
		/// Toggles the element.
		/// </summary>
		public void Toggle()
		{
			GetPattern<IUIAutomationTogglePattern>()?.Toggle();
		}

		#endregion
	}
}