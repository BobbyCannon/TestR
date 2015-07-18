#region References

using TestR.Desktop;

#endregion

namespace TestR.Editor
{
	public class ElementAction
	{
		#region Constructors

		public ElementAction(IElementParent element, ElementActionType actionType)
		{
			ElementId = SelectId(element);
			Input = string.Empty;
			Type = actionType;
		}

		#endregion

		#region Properties

		public string ElementId { get; set; }
		public string Input { get; set; }
		public ElementActionType Type { get; set; }

		#endregion

		#region Methods

		private static string SelectId(IElementParent element)
		{
			if (!string.IsNullOrWhiteSpace(element.Id))
			{
				return element.Id;
			}

			if (!string.IsNullOrWhiteSpace(element.Name))
			{
				return element.Name;
			}

			return string.Empty;
		}

		#endregion
	}
}