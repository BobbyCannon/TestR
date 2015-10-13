#region References

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.VisualStudio.Shell;

#endregion

namespace TestR.Extension
{
	/// <summary>
	/// This class implements the tool window exposed by this package and hosts a user control.
	/// </summary>
	/// <remarks>
	/// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
	/// usually implemented by the package implementer.
	/// <para>
	/// This class derives from the ToolWindowPane class provided from the MPF in order to use its
	/// implementation of the IVsUIElementPane interface.
	/// </para>
	/// </remarks>
	[Guid("80a3545f-fb1e-47ff-8947-dfd7bd4a6558")]
	public class ExtensionWindow : ToolWindowPane
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtensionWindow" /> class.
		/// </summary>
		[SuppressMessage("ReSharper", "DoNotCallOverridableMethodsInConstructor")]
		public ExtensionWindow() : base(null)
		{
			Caption = "TestR Extension";

			// This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
			// we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
			// the object returned by the Content property.
			Content = new ExtensionWindowControl();
		}

		#endregion

		#region Methods

		public static void TestWindow()
		{
			var window = new Window();
			var control = new ExtensionWindowControl();
			control.HorizontalContentAlignment = HorizontalAlignment.Stretch;
			control.VerticalContentAlignment = VerticalAlignment.Stretch;
			window.Content = control;
			window.Width = 450;
			window.Height = 600;
			window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			window.ShowDialog();
		}

		#endregion
	}
}