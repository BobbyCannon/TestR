#region References

using System.Collections.ObjectModel;
using TestR.Desktop;
using TestR.Extensions;

#endregion

namespace TestR.Editor
{
	public class ElementReference
	{
		#region Constructors

		public ElementReference(Element element)
		{
			Display = new[] { element.Id, element.Name }.FirstValue();
			ApplicationId = element.ApplicationId;
			Children = new ObservableCollection<ElementReference>();
		}

		#endregion

		#region Properties

		public string ApplicationId { get; set; }
		public ObservableCollection<ElementReference> Children { get; set; }
		public string Display { get; set; }

		#endregion
	}
}