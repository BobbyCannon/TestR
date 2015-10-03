#region References

using System.Linq;
using System.Text;
using TestR.Desktop;
using TestR.Editor.Extensions;

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
			DisplayText = GetDisplayName(element);
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

		private string GetDisplayName(Element element)
		{
			var builder = new StringBuilder(128);
			builder.AppendFirst(element.Id, element.Name);
			builder.AppendIf(" -> ", builder.Length > 0);
			builder.Append(element.ApplicationId);

			return builder.ToString();
		}

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