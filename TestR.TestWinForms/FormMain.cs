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

			//Keyboard.KeyPressed += key => keyPress.Text = KeyConverter.KeyToAsciiValue(key, Keyboard.IsShiftPressed()).ToString("X2") + " - " + key.ToString();
			//Keyboard.StartMonitoring();
		}

		#endregion

		#region Methods

		private void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}

		private void keyPress_KeyDown(object sender, KeyEventArgs e)
		{
			e.SuppressKeyPress = true;
			e.Handled = true;
		}

		private void keyPress_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
		}

		#endregion
	}
}