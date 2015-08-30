#region References

using System;
using System.Diagnostics;
using System.Windows.Forms;
using TestR.Desktop;
using Application = TestR.Desktop.Application;

#endregion

namespace TestR.TestWinForms
{
	public partial class FormMain : Form
	{
		#region Fields

		private readonly Application _application;

		#endregion

		#region Constructors

		public FormMain()
		{
			InitializeComponent();
			_application = Application.Attach(Process.GetCurrentProcess().MainWindowHandle, false);
			_application.ElementClicked += ApplicationOnElementClicked;
		}

		#endregion

		#region Methods

		private void ApplicationOnElementClicked(Element element)
		{
			toolStripStatusLabelReady.Text = $"{element.Id}/{element.Name} : {element.TypeName}";
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		#endregion
	}
}