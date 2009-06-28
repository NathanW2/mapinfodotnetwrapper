namespace Wrapper.Example.Forms
{
	partial class MapForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWorldTableLINQExampleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBoxMapTool = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.QueryTextbox = new System.Windows.Forms.TextBox();
            this.RunQueryButton = new System.Windows.Forms.Button();
            this.MapInformation = new System.Windows.Forms.PropertyGrid();
            this.mapPanel = new System.Windows.Forms.PictureBox();
            this.AddNewButton = new System.Windows.Forms.Button();
            this.TableInfo = new System.Windows.Forms.GroupBox();
            this.TabInfoGrid = new System.Windows.Forms.PropertyGrid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapPanel)).BeginInit();
            this.TableInfo.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1142, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openWorldTableLINQExampleToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openWorldTableLINQExampleToolStripMenuItem
            // 
            this.openWorldTableLINQExampleToolStripMenuItem.Name = "openWorldTableLINQExampleToolStripMenuItem";
            this.openWorldTableLINQExampleToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.openWorldTableLINQExampleToolStripMenuItem.Text = "Open World Table LINQ Example";
            this.openWorldTableLINQExampleToolStripMenuItem.Click += new System.EventHandler(this.openWorldTableLINQExampleToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // comboBoxMapTool
            // 
            this.comboBoxMapTool.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMapTool.Enabled = false;
            this.comboBoxMapTool.FormattingEnabled = true;
            this.comboBoxMapTool.Location = new System.Drawing.Point(111, 56);
            this.comboBoxMapTool.Name = "comboBoxMapTool";
            this.comboBoxMapTool.Size = new System.Drawing.Size(121, 21);
            this.comboBoxMapTool.TabIndex = 0;
            this.comboBoxMapTool.SelectionChangeCommitted += new System.EventHandler(this.comboBoxMapTool_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Active Map Tool:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 515);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1142, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel1.Text = " ";
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.Enabled = false;
            this.buttonZoomIn.Location = new System.Drawing.Point(248, 56);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(75, 23);
            this.buttonZoomIn.TabIndex = 3;
            this.buttonZoomIn.Text = "Zoom In";
            this.buttonZoomIn.UseVisualStyleBackColor = true;
            this.buttonZoomIn.Click += new System.EventHandler(this.buttonZoomIn_Click);
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.Enabled = false;
            this.buttonZoomOut.Location = new System.Drawing.Point(342, 56);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(75, 23);
            this.buttonZoomOut.TabIndex = 4;
            this.buttonZoomOut.Text = "Zoom Out";
            this.buttonZoomOut.UseVisualStyleBackColor = true;
            this.buttonZoomOut.Click += new System.EventHandler(this.buttonZoomOut_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Select all where Continent is equal to North America",
            "Select all where Pop_1994 is greater then 4,000,000",
            "Select Name, Pop_1994, obj from world."});
            this.comboBox1.Location = new System.Drawing.Point(623, 60);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(412, 21);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.Text = "SELECT A QUERY TO RUN ON THE WORLD TABLE.";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(623, 143);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(504, 329);
            this.dataGridView1.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(623, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(271, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Open World Table for LINQ-To-Mapinfo example.";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(900, 26);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(233, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Open World workspace";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // QueryTextbox
            // 
            this.QueryTextbox.Location = new System.Drawing.Point(626, 87);
            this.QueryTextbox.Multiline = true;
            this.QueryTextbox.Name = "QueryTextbox";
            this.QueryTextbox.Size = new System.Drawing.Size(504, 49);
            this.QueryTextbox.TabIndex = 9;
            // 
            // RunQueryButton
            // 
            this.RunQueryButton.Location = new System.Drawing.Point(1042, 60);
            this.RunQueryButton.Name = "RunQueryButton";
            this.RunQueryButton.Size = new System.Drawing.Size(88, 23);
            this.RunQueryButton.TabIndex = 10;
            this.RunQueryButton.Text = "Run Query";
            this.RunQueryButton.UseVisualStyleBackColor = true;
            this.RunQueryButton.Click += new System.EventHandler(this.RunQueryButton_Click);
            // 
            // MapInformation
            // 
            this.MapInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapInformation.HelpVisible = false;
            this.MapInformation.Location = new System.Drawing.Point(3, 3);
            this.MapInformation.Name = "MapInformation";
            this.MapInformation.Size = new System.Drawing.Size(249, 245);
            this.MapInformation.TabIndex = 0;
            this.MapInformation.ToolbarVisible = false;
            // 
            // mapPanel
            // 
            this.mapPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.mapPanel.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.mapPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mapPanel.Location = new System.Drawing.Point(20, 83);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(597, 426);
            this.mapPanel.TabIndex = 11;
            this.mapPanel.TabStop = false;
            // 
            // AddNewButton
            // 
            this.AddNewButton.Location = new System.Drawing.Point(623, 478);
            this.AddNewButton.Name = "AddNewButton";
            this.AddNewButton.Size = new System.Drawing.Size(162, 31);
            this.AddNewButton.TabIndex = 12;
            this.AddNewButton.Text = "Add new record to world table";
            this.AddNewButton.UseVisualStyleBackColor = true;
            this.AddNewButton.Click += new System.EventHandler(this.AddNewButton_Click);
            // 
            // TableInfo
            // 
            this.TableInfo.Controls.Add(this.tabControl1);
            this.TableInfo.Location = new System.Drawing.Point(342, 207);
            this.TableInfo.Name = "TableInfo";
            this.TableInfo.Size = new System.Drawing.Size(275, 302);
            this.TableInfo.TabIndex = 14;
            this.TableInfo.TabStop = false;
            this.TableInfo.Text = "Information";
            // 
            // TabInfoGrid
            // 
            this.TabInfoGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabInfoGrid.HelpVisible = false;
            this.TabInfoGrid.Location = new System.Drawing.Point(3, 3);
            this.TabInfoGrid.Name = "TabInfoGrid";
            this.TabInfoGrid.Size = new System.Drawing.Size(249, 157);
            this.TabInfoGrid.TabIndex = 0;
            this.TabInfoGrid.ToolbarVisible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(6, 19);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(263, 277);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.MapInformation);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(255, 251);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Map Info";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.TabInfoGrid);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(255, 163);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Table Info";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // MapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 537);
            this.Controls.Add(this.TableInfo);
            this.Controls.Add(this.AddNewButton);
            this.Controls.Add(this.mapPanel);
            this.Controls.Add(this.RunQueryButton);
            this.Controls.Add(this.QueryTextbox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.buttonZoomOut);
            this.Controls.Add(this.buttonZoomIn);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.comboBoxMapTool);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(450, 450);
            this.Name = "MapForm";
            this.Text = "Map Viewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapPanel)).EndInit();
            this.TableInfo.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ComboBox comboBoxMapTool;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button buttonZoomIn;
        private System.Windows.Forms.Button buttonZoomOut;
        private System.Windows.Forms.ToolStripMenuItem openWorldTableLINQExampleToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox QueryTextbox;
        private System.Windows.Forms.Button RunQueryButton;
        private System.Windows.Forms.PropertyGrid MapInformation;
        private System.Windows.Forms.PictureBox mapPanel;
        private System.Windows.Forms.Button AddNewButton;
        private System.Windows.Forms.GroupBox TableInfo;
        private System.Windows.Forms.PropertyGrid TabInfoGrid;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
	}
}

