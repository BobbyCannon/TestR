#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Pattern
{
	/// <summary>
	/// Represents the value pattern.
	/// </summary>
	public class ValuePattern : BasePattern
	{
		#region Constructors

		private ValuePattern(Element element)
			: base(element)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a value indicating if the element is read only.
		/// </summary>
		public bool IsReadOnly => GetPattern<IUIAutomationValuePattern>()?.CurrentIsReadOnly == 1;

		/// <summary>
		/// Gets the current value of the element.
		/// </summary>
		public string Value => GetPattern<IUIAutomationValuePattern>()?.CurrentValue ?? string.Empty;

		#endregion

		#region Methods

		/// <summary>
		/// Creates a new instance of this pattern.
		/// </summary>
		/// <param name="element"> The element this pattern is for. </param>
		/// <returns> The instance of the pattern. </returns>
		public static ValuePattern New(Element element)
		{
			return new ValuePattern(element);
		}

		/// <summary>
		/// Sets the value of the element.
		/// </summary>
		/// <param name="value"> </param>
		public void SetValue(string value)
		{
			GetPattern<IUIAutomationValuePattern>()?.SetValue(value);
		}

		#endregion
	}
}