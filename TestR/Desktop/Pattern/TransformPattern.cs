#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Pattern
{
	/// <summary>
	/// Represents the Windows transform pattern.
	/// </summary>
	public class TransformPattern
	{
		#region Fields

		private readonly IUIAutomationTransformPattern _pattern;

		#endregion

		#region Constructors

		private TransformPattern(IUIAutomationTransformPattern pattern)
		{
			_pattern = pattern;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Create a new pattern for the provided element.
		/// </summary>
		/// <param name="element"> The element that supports the pattern. </param>
		/// <returns> The pattern if we could find one else null will be returned. </returns>
		public static TransformPattern Create(Element element)
		{
			var pattern = element.NativeElement.GetCurrentPattern(UIA_PatternIds.UIA_TransformPatternId) as IUIAutomationTransformPattern;
			return pattern == null ? null : new TransformPattern(pattern);
		}

		/// <summary>
		/// Move the element.
		/// </summary>
		/// <param name="x"> The x value of the position to move to. </param>
		/// <param name="y"> The y value of the position to move to. </param>
		public void Move(int x, int y)
		{
			_pattern.Move(x, y);
		}

		/// <summary>
		/// Resize the element.
		/// </summary>
		/// <param name="width"> The width to set. </param>
		/// <param name="height"> The height to set. </param>
		public void Resize(int width, int height)
		{
			_pattern.Resize(width, height);
		}

		#endregion
	}
}