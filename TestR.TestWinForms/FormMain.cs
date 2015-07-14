#region References

using System;
using System.Windows.Forms;

#endregion

namespace TestR.TestWinForms
{
	public partial class FormMain : Form
	{
		#region Constructors

		public FormMain()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		#endregion
	}
}