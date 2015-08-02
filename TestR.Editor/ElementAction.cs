#region References

using System.Linq;
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
			Properties = new string[0];
			Property = string.Empty;
		}

		public ElementAction(Element element, ElementActionType actionType)
		{
			ApplicationId = element.ApplicationId;
			DisplayText = element.Id + " : " + element.Name;
			Input = string.Empty;
			Type = actionType;
			Properties = GetProperties(element);
			Property = Properties.FirstOrDefault() ?? string.Empty;
		}

		#endregion

		#region Properties

		public string ApplicationId { get; set; }
		public string DisplayText { get; set; }
		public string Input { get; set; }
		public string[] Properties { get; set; }
		public string Property { get; set; }
		public ElementActionType Type { get; set; }

		#endregion

		#region Methods

		private static string[] GetProperties(Element element)
		{
			var type = element.GetType();
			return type.GetProperties()
				.Where(x => !Element.ExcludedProperties.Contains(x.Name))
				.Select(x => x.Name)
				.OrderBy(x => x)
				.ToArray();
		}

		#endregion
	}
}