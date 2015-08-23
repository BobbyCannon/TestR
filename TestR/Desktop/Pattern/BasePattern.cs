#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Pattern
{
	/// <summary>
	/// Represent the base pattern for all patterns.
	/// </summary>
	public abstract class BasePattern
	{
		#region Fields

		private readonly Element _element;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of the base pattern.
		/// </summary>
		/// <param name="element"> The element this pattern is for. </param>
		protected BasePattern(Element element)
		{
			_element = element;
		}

		#endregion

		#region Methods

		internal T GetPattern<T>() where T : class
		{
			return _element.NativeElement?.GetCurrentPattern(GetPatternId<T>()) as T;
		}

		private int GetPatternId<T>()
		{
			var type = typeof (T);
			switch (type.Name)
			{
				case "IUIAutomationExpandCollapsePattern":
					return UIA_PatternIds.UIA_ExpandCollapsePatternId;

				case "IUIAutomationTogglePattern":
					return UIA_PatternIds.UIA_TogglePatternId;

				case "IUIAutomationValuePattern":
					return UIA_PatternIds.UIA_ValuePatternId;

				default:
					return -1;
			}
		}

		#endregion
	}
}