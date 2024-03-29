﻿namespace TestR.TestWinForms
{
	partial class FormMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed otherwise false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("One");
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Two");
			System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Three");
			this.MenuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.StatusStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabelReady = new System.Windows.Forms.ToolStripStatusLabel();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.comboBox3 = new System.Windows.Forms.ComboBox();
			this.comboBox4 = new System.Windows.Forms.ComboBox();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
			this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.linkLabel3 = new System.Windows.Forms.LinkLabel();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.listView1 = new System.Windows.Forms.ListView();
			this.keyPress = new System.Windows.Forms.TextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.textBox6 = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.checkBoxKeyPressDetails = new System.Windows.Forms.CheckBox();
			this.MenuStrip.SuspendLayout();
			this.StatusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// MenuStrip
			// 
			this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.MenuStrip.Location = new System.Drawing.Point(0, 0);
			this.MenuStrip.Name = "MenuStrip";
			this.MenuStrip.Size = new System.Drawing.Size(639, 24);
			this.MenuStrip.TabIndex = 0;
			this.MenuStrip.Text = "mainMenuStrip";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// StatusStrip
			// 
			this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelReady});
			this.StatusStrip.Location = new System.Drawing.Point(0, 535);
			this.StatusStrip.Name = "StatusStrip";
			this.StatusStrip.Size = new System.Drawing.Size(639, 22);
			this.StatusStrip.TabIndex = 9;
			this.StatusStrip.Text = "statusStrip1";
			// 
			// toolStripStatusLabelReady
			// 
			this.toolStripStatusLabelReady.Name = "toolStripStatusLabelReady";
			this.toolStripStatusLabelReady.Size = new System.Drawing.Size(39, 17);
			this.toolStripStatusLabelReady.Text = "Ready";
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(97, 33);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(82, 17);
			this.checkBox1.TabIndex = 4;
			this.checkBox1.Text = "Unchecked";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Checked = true;
			this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox2.Location = new System.Drawing.Point(97, 56);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(69, 17);
			this.checkBox2.TabIndex = 5;
			this.checkBox2.Text = "Checked";
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// checkBox3
			// 
			this.checkBox3.AutoSize = true;
			this.checkBox3.Checked = true;
			this.checkBox3.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.checkBox3.Location = new System.Drawing.Point(97, 79);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(90, 17);
			this.checkBox3.TabIndex = 6;
			this.checkBox3.Text = "Indeterminate";
			this.checkBox3.ThreeState = true;
			this.checkBox3.UseVisualStyleBackColor = true;
			// 
			// checkBox4
			// 
			this.checkBox4.AutoCheck = false;
			this.checkBox4.AutoSize = true;
			this.checkBox4.Enabled = false;
			this.checkBox4.Location = new System.Drawing.Point(97, 102);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(67, 17);
			this.checkBox4.TabIndex = 7;
			this.checkBox4.Text = "Disabled";
			this.checkBox4.UseVisualStyleBackColor = true;
			// 
			// checkBox5
			// 
			this.checkBox5.AutoSize = true;
			this.checkBox5.Location = new System.Drawing.Point(97, 125);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(76, 17);
			this.checkBox5.TabIndex = 8;
			this.checkBox5.Text = "Not Visible";
			this.checkBox5.UseVisualStyleBackColor = true;
			this.checkBox5.Visible = false;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 33);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Button";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Enabled = false;
			this.button2.Location = new System.Drawing.Point(12, 62);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "Disabled";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(12, 91);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 3;
			this.button3.Text = "Not Visible";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Visible = false;
			// 
			// checkedListBox1
			// 
			this.checkedListBox1.FormattingEnabled = true;
			this.checkedListBox1.Items.AddRange(new object[] {
            "One",
            "Two",
            "Three",
            "Four",
            "Five"});
			this.checkedListBox1.Location = new System.Drawing.Point(193, 33);
			this.checkedListBox1.Name = "checkedListBox1";
			this.checkedListBox1.Size = new System.Drawing.Size(97, 109);
			this.checkedListBox1.TabIndex = 10;
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            "One",
            "Two",
            "Three"});
			this.comboBox1.Location = new System.Drawing.Point(296, 33);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.TabIndex = 11;
			// 
			// comboBox2
			// 
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Items.AddRange(new object[] {
            "Dog",
            "Cat",
            "Bird",
            "Fish"});
			this.comboBox2.Location = new System.Drawing.Point(296, 60);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(121, 21);
			this.comboBox2.TabIndex = 12;
			// 
			// comboBox3
			// 
			this.comboBox3.Enabled = false;
			this.comboBox3.FormattingEnabled = true;
			this.comboBox3.Items.AddRange(new object[] {
            "South Carolina",
            "Georgia",
            "Florida"});
			this.comboBox3.Location = new System.Drawing.Point(296, 87);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new System.Drawing.Size(121, 21);
			this.comboBox3.TabIndex = 13;
			// 
			// comboBox4
			// 
			this.comboBox4.FormattingEnabled = true;
			this.comboBox4.Items.AddRange(new object[] {
            "Not Visible",
            "So You Should",
            "Not See This"});
			this.comboBox4.Location = new System.Drawing.Point(296, 114);
			this.comboBox4.Name = "comboBox4";
			this.comboBox4.Size = new System.Drawing.Size(121, 21);
			this.comboBox4.TabIndex = 14;
			this.comboBox4.Visible = false;
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Location = new System.Drawing.Point(423, 33);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
			this.dateTimePicker1.TabIndex = 15;
			// 
			// dateTimePicker2
			// 
			this.dateTimePicker2.Enabled = false;
			this.dateTimePicker2.Location = new System.Drawing.Point(423, 59);
			this.dateTimePicker2.Name = "dateTimePicker2";
			this.dateTimePicker2.Size = new System.Drawing.Size(200, 20);
			this.dateTimePicker2.TabIndex = 16;
			// 
			// dateTimePicker3
			// 
			this.dateTimePicker3.Location = new System.Drawing.Point(423, 85);
			this.dateTimePicker3.Name = "dateTimePicker3";
			this.dateTimePicker3.Size = new System.Drawing.Size(200, 20);
			this.dateTimePicker3.TabIndex = 17;
			this.dateTimePicker3.Visible = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 158);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 13);
			this.label1.TabIndex = 18;
			this.label1.Text = "Label";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 178);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(77, 13);
			this.label2.TabIndex = 19;
			this.label2.Text = "Disabled Label";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 198);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(86, 13);
			this.label3.TabIndex = 20;
			this.label3.Text = "Not Visible Label";
			this.label3.Visible = false;
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(12, 220);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(56, 13);
			this.linkLabel1.TabIndex = 21;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Link Label";
			// 
			// linkLabel2
			// 
			this.linkLabel2.AutoSize = true;
			this.linkLabel2.Enabled = false;
			this.linkLabel2.Location = new System.Drawing.Point(12, 240);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(100, 13);
			this.linkLabel2.TabIndex = 22;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "Disabled Link Label";
			// 
			// linkLabel3
			// 
			this.linkLabel3.AutoSize = true;
			this.linkLabel3.Location = new System.Drawing.Point(12, 260);
			this.linkLabel3.Name = "linkLabel3";
			this.linkLabel3.Size = new System.Drawing.Size(109, 13);
			this.linkLabel3.TabIndex = 23;
			this.linkLabel3.TabStop = true;
			this.linkLabel3.Text = "Not Visible Link Label";
			this.linkLabel3.Visible = false;
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Items.AddRange(new object[] {
            "One",
            "Two",
            "Three",
            "Four"});
			this.listBox1.Location = new System.Drawing.Point(193, 158);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(97, 95);
			this.listBox1.TabIndex = 24;
			// 
			// listView1
			// 
			this.listView1.HideSelection = false;
			this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
			this.listView1.Location = new System.Drawing.Point(296, 158);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(121, 95);
			this.listView1.TabIndex = 25;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.List;
			// 
			// keyPress
			// 
			this.keyPress.Location = new System.Drawing.Point(12, 314);
			this.keyPress.Multiline = true;
			this.keyPress.Name = "keyPress";
			this.keyPress.Size = new System.Drawing.Size(611, 204);
			this.keyPress.TabIndex = 26;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(423, 158);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(200, 20);
			this.textBox1.TabIndex = 27;
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(423, 184);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(200, 20);
			this.textBox2.TabIndex = 28;
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(423, 210);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(200, 20);
			this.textBox3.TabIndex = 29;
			// 
			// textBox4
			// 
			this.textBox4.Location = new System.Drawing.Point(423, 236);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(200, 20);
			this.textBox4.TabIndex = 30;
			// 
			// textBox5
			// 
			this.textBox5.Location = new System.Drawing.Point(423, 262);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(200, 20);
			this.textBox5.TabIndex = 31;
			// 
			// textBox6
			// 
			this.textBox6.Location = new System.Drawing.Point(423, 288);
			this.textBox6.Name = "textBox6";
			this.textBox6.Size = new System.Drawing.Size(200, 20);
			this.textBox6.TabIndex = 32;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 298);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(89, 13);
			this.label4.TabIndex = 33;
			this.label4.Text = "Key Press History";
			// 
			// checkBoxKeyPressDetails
			// 
			this.checkBoxKeyPressDetails.AutoSize = true;
			this.checkBoxKeyPressDetails.Location = new System.Drawing.Point(107, 294);
			this.checkBoxKeyPressDetails.Name = "checkBoxKeyPressDetails";
			this.checkBoxKeyPressDetails.Size = new System.Drawing.Size(58, 17);
			this.checkBoxKeyPressDetails.TabIndex = 34;
			this.checkBoxKeyPressDetails.Text = "Details";
			this.checkBoxKeyPressDetails.UseVisualStyleBackColor = true;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(639, 557);
			this.Controls.Add(this.checkBoxKeyPressDetails);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBox6);
			this.Controls.Add(this.textBox5);
			this.Controls.Add(this.textBox4);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.keyPress);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.linkLabel3);
			this.Controls.Add(this.linkLabel2);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dateTimePicker3);
			this.Controls.Add(this.dateTimePicker2);
			this.Controls.Add(this.dateTimePicker1);
			this.Controls.Add(this.comboBox4);
			this.Controls.Add(this.comboBox3);
			this.Controls.Add(this.comboBox2);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.checkedListBox1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.checkBox5);
			this.Controls.Add(this.checkBox4);
			this.Controls.Add(this.checkBox3);
			this.Controls.Add(this.checkBox2);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.StatusStrip);
			this.Controls.Add(this.MenuStrip);
			this.KeyPreview = true;
			this.Name = "FormMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "TestR Test WinForm";
			this.MenuStrip.ResumeLayout(false);
			this.MenuStrip.PerformLayout();
			this.StatusStrip.ResumeLayout(false);
			this.StatusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip MenuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.StatusStrip StatusStrip;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelReady;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.CheckBox checkBox5;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.CheckedListBox checkedListBox1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.ComboBox comboBox3;
		private System.Windows.Forms.ComboBox comboBox4;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.DateTimePicker dateTimePicker2;
		private System.Windows.Forms.DateTimePicker dateTimePicker3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.LinkLabel linkLabel2;
		private System.Windows.Forms.LinkLabel linkLabel3;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.TextBox keyPress;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.TextBox textBox6;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox checkBoxKeyPressDetails;
	}
}

