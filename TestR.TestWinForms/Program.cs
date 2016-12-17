#region References

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TestR.Desktop;
using TestR.Native;
using FormApplication = System.Windows.Forms.Application;

#endregion

namespace TestR.TestWinForms
{
	internal static class Program
	{
		#region Fields

		private static ParentForm _parentForm;

		#endregion

		#region Methods

		private static string GetParentWindowsTitle(DesktopElement element)
		{
			var parent = element;

			while (parent != null && parent.TypeId != 50032)
			{
				parent = parent.Parent as DesktopElement;
			}

			return parent == null ? string.Empty : parent.Name;
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			Mouse.Clicked += MouseClick;
			Mouse.StartMonitoring();

			FormApplication.EnableVisualStyles();
			FormApplication.SetCompatibleTextRenderingDefault(false);
			FormApplication.Run(_parentForm = new ParentForm());
		}

		private static void MouseClick(object sender, MouseEventArgs e)
		{
			var point = new Point(e.X, e.Y);
			var element = DesktopElement.FromPoint(point);
			element.UpdateParents();

			var message = $"{element.FullId} : {element.TypeName} @ {point.X}:{point.Y} on {GetParentWindowsTitle(element)} with {e.Button} button";
			Debug.WriteLine(message);

			if (_parentForm != null)
			{
				_parentForm.StatusText = message;
			}
		}

		#endregion
	}
}