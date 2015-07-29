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
			var child = walker.GetFirstChild(element);

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
			var walker = new TreeWalker(Automation.Automation.RawViewCondition);
			var child = walker.GetFirstChild(AutomationElement.RootElement);

			while (child != null)
			{
				if (child.Current.ProcessId == id && child.Current.ControlType.ProgrammaticName == ControlType.Window.ProgrammaticName)
				{
					yield return child;
				}
				child = walker.GetNextSibling(child);
			}
		}

		#endregion
	}
}