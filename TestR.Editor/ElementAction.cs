#region References

using TestR.Desktop;

#endregion

namespace TestR.Editor
{
	public class ElementAction
	{
		#region Constructors

		public ElementAction()
		{
			ApplicationId = string.Empty;
			DisplayText = string.Empty;
			Input = string.Empty;
			Type = ElementActionType.TypeText;
		}

		public ElementAction(Element element, ElementActionType actionType)
		{
			ApplicationId = element.ApplicationId;
			DisplayText = element.Id + " : " + element.Name;
			Input = string.Empty;
			Type = actionType;
		}

		#endregion

		#region Properties

		public string ApplicationId { get; set; }
		public string DisplayText { get; set; }
		public string Input { get; set; }
		public ElementActionType Type { get; set; }

		#endregion
	}
}