#region References

using System.Collections.Generic;
using TestR.Desktop.Automation;

#endregion

namespace TestR.Desktop
{
	/// <summary>
	/// Provides a way to walk automation elements.
	/// </summary>
	public static class ElementWalker
	{
		#region Methods

		/// <summary>
		/// Gets all the direct children of an element.
		/// </summary>
		/// <param name="element"> The element to get the children of. </param>
		/// <returns> The list of children for the element. </returns>
		public static IEnumerable<AutomationElement> GetChildren(AutomationElement element)
		{
			var walker = new TreeWalker(Automation.Automation.RawViewCondition);
			var child = walker.GetFirstChild(element, CacheRequest.Current);

			while (child != null)
			{
				yield return child;
				child = walker.GetNextSibling(child);
			}
		}

		/// <summary>
		/// Gets a list of window elements for a process.
		/// </summary>
		/// <param name="id"> The ID of the process. </param>
		/// <returns> The list of automation elements for the process. </returns>
		public static IEnumerable<AutomationElement> GetWindowsForProcess(int id)
		{
			var condition1 = new PropertyCondition(AutomationElement.ProcessIdProperty, id);
			var condition2 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window);
			var conditions = new AndCondition(Automation.Automation.RawViewCondition, condition1, condition2);
			var walker = new TreeWalker(conditions);
			var root = AutomationElement.RootElement;
			var element = walker.GetFirstChild(root);

			while (element != null)
			{
				yield return element;
				element = walker.GetNextSibling(element);
			}
		}

		#endregion
	}
}