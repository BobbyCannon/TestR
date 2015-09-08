﻿#region References

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Threading;
using TestR.Desktop;
using Application = TestR.Desktop.Application;

#endregion

namespace TestR.TestWinForms
{
	public partial class ParentForm : Form
	{
		#region Fields

		private readonly Dispatcher _dispatcher;
		private int childFormNumber;

		#endregion

		#region Constructors

		public ParentForm()
		{
			InitializeComponent();
			_dispatcher = Dispatcher.CurrentDispatcher;
		}

		#endregion

		#region Properties

		public string StatusText
		{
			get { return toolStripStatusLabel.Text; }
			set { toolStripStatusLabel.Text = value; }
		}

		#endregion

		#region Methods

		
		private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.ArrangeIcons);
		}

		private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.Cascade);
		}

		private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (var childForm in MdiChildren)
			{
				childForm.Close();
			}
		}

		private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void CutToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void OpenFile(object sender, EventArgs e)
		{
			var openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				var FileName = openFileDialog.FileName;
			}
		}

		private void ParentForm_Load(object sender, EventArgs e)
		{
			var main = new FormMain();
			main.MdiParent = this;
			//main.WindowState = FormWindowState.Maximized;
			main.Show();

			
		}

		private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var saveFileDialog = new SaveFileDialog();
			saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				var FileName = saveFileDialog.FileName;
			}
		}

		private void ShowNewForm(object sender, EventArgs e)
		{
			var childForm = new Form();
			childForm.MdiParent = this;
			childForm.Text = "Window " + childFormNumber++;
			childForm.Show();
		}

		private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			statusStrip.Visible = statusBarToolStripMenuItem.Checked;
		}

		private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileHorizontal);
		}

		private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileVertical);
		}

		private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			toolStrip.Visible = toolBarToolStripMenuItem.Checked;
		}

		#endregion
	}
}