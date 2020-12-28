#region References

using System;
using System.Windows.Forms;
using TestR.Desktop;

#endregion

namespace TestR.TestWinForms
{
	public partial class FormMain : Form
	{
		#region Constructors

		public FormMain()
		{
			InitializeComponent();

			Input.Keyboard.KeyPressed += KeyboardOnKeyPressed;
			Input.Keyboard.StartMonitoring();
		}

		#endregion

		#region Methods

		private void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}

		private void KeyboardOnKeyPressed(object sender, KeyboardState state)
		{
			if (!state.IsPressed)
			{
				return;
			}

			keyPress.Text += state;
		}

		#endregion
	}
}