namespace Schematix
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.Images32 = new System.Windows.Forms.ImageList(this.components);
            this.btnSaveClass = new System.Windows.Forms.Button();
            this.btnSaveAllClasses = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbTables = new System.Windows.Forms.ListBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.SqlTabs = new System.Windows.Forms.TabControl();
            this.tabSql = new System.Windows.Forms.TabPage();
            this.dfSql = new System.Windows.Forms.TextBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.lvColumns = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel4 = new System.Windows.Forms.Panel();
            this.dfColumnsPerLine = new System.Windows.Forms.NumericUpDown();
            this.cbColumnAlias = new System.Windows.Forms.CheckBox();
            this.cbFullyQualifiedColumns = new System.Windows.Forms.CheckBox();
            this.dfTableAlias = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabClasses = new System.Windows.Forms.TabPage();
            this.dfClass = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cbINotifyPropertyChanged = new System.Windows.Forms.CheckBox();
            this.dfNamespace = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SqliteOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.FolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.lblITG = new System.Windows.Forms.LinkLabel();
            this.SqliteLogo = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SqlTabs.SuspendLayout();
            this.tabSql.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dfColumnsPerLine)).BeginInit();
            this.tabClasses.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SqliteLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.SqliteLogo);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnSaveClass);
            this.panel1.Controls.Add(this.btnSaveAllClasses);
            this.panel1.Controls.Add(this.btnOpen);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(833, 70);
            this.panel1.TabIndex = 0;
            // 
            // btnExit
            // 
            this.btnExit.ImageKey = "Power.png";
            this.btnExit.ImageList = this.Images32;
            this.btnExit.Location = new System.Drawing.Point(12, 5);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(48, 54);
            this.btnExit.TabIndex = 0;
            this.toolTips.SetToolTip(this.btnExit, "Exit program");
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.Shared_ButtonClick);
            // 
            // Images32
            // 
            this.Images32.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Images32.ImageStream")));
            this.Images32.TransparentColor = System.Drawing.Color.Transparent;
            this.Images32.Images.SetKeyName(0, "Open32.png");
            this.Images32.Images.SetKeyName(1, "Power.png");
            this.Images32.Images.SetKeyName(2, "Diskette32.png");
            this.Images32.Images.SetKeyName(3, "DisketteMany32.png");
            // 
            // btnSaveClass
            // 
            this.btnSaveClass.ImageKey = "Diskette32.png";
            this.btnSaveClass.ImageList = this.Images32;
            this.btnSaveClass.Location = new System.Drawing.Point(199, 8);
            this.btnSaveClass.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSaveClass.Name = "btnSaveClass";
            this.btnSaveClass.Size = new System.Drawing.Size(48, 54);
            this.btnSaveClass.TabIndex = 0;
            this.toolTips.SetToolTip(this.btnSaveClass, "Save class for selected table");
            this.btnSaveClass.UseVisualStyleBackColor = true;
            this.btnSaveClass.Click += new System.EventHandler(this.Shared_ButtonClick);
            // 
            // btnSaveAllClasses
            // 
            this.btnSaveAllClasses.ImageKey = "DisketteMany32.png";
            this.btnSaveAllClasses.ImageList = this.Images32;
            this.btnSaveAllClasses.Location = new System.Drawing.Point(142, 8);
            this.btnSaveAllClasses.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSaveAllClasses.Name = "btnSaveAllClasses";
            this.btnSaveAllClasses.Size = new System.Drawing.Size(48, 54);
            this.btnSaveAllClasses.TabIndex = 0;
            this.toolTips.SetToolTip(this.btnSaveAllClasses, "Save classes for all tables");
            this.btnSaveAllClasses.UseVisualStyleBackColor = true;
            this.btnSaveAllClasses.Click += new System.EventHandler(this.Shared_ButtonClick);
            // 
            // btnOpen
            // 
            this.btnOpen.ImageKey = "Open32.png";
            this.btnOpen.ImageList = this.Images32;
            this.btnOpen.Location = new System.Drawing.Point(86, 8);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(48, 54);
            this.btnOpen.TabIndex = 0;
            this.toolTips.SetToolTip(this.btnOpen, "Open Sqlite database");
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.Shared_ButtonClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbTables);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 94);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 370);
            this.panel2.TabIndex = 1;
            // 
            // lbTables
            // 
            this.lbTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTables.FormattingEnabled = true;
            this.lbTables.ItemHeight = 17;
            this.lbTables.Location = new System.Drawing.Point(0, 0);
            this.lbTables.Name = "lbTables";
            this.lbTables.Size = new System.Drawing.Size(200, 370);
            this.lbTables.TabIndex = 0;
            this.lbTables.SelectedIndexChanged += new System.EventHandler(this.lbTables_SelectedIndexChanged);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(200, 94);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(8, 370);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.SqlTabs);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(208, 94);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(625, 370);
            this.panel3.TabIndex = 3;
            // 
            // SqlTabs
            // 
            this.SqlTabs.Controls.Add(this.tabSql);
            this.SqlTabs.Controls.Add(this.tabClasses);
            this.SqlTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SqlTabs.Location = new System.Drawing.Point(0, 0);
            this.SqlTabs.Name = "SqlTabs";
            this.SqlTabs.SelectedIndex = 0;
            this.SqlTabs.Size = new System.Drawing.Size(625, 370);
            this.SqlTabs.TabIndex = 0;
            // 
            // tabSql
            // 
            this.tabSql.Controls.Add(this.dfSql);
            this.tabSql.Controls.Add(this.splitter2);
            this.tabSql.Controls.Add(this.lvColumns);
            this.tabSql.Controls.Add(this.panel4);
            this.tabSql.Location = new System.Drawing.Point(4, 26);
            this.tabSql.Name = "tabSql";
            this.tabSql.Padding = new System.Windows.Forms.Padding(3);
            this.tabSql.Size = new System.Drawing.Size(617, 340);
            this.tabSql.TabIndex = 0;
            this.tabSql.Text = "SQL";
            this.tabSql.UseVisualStyleBackColor = true;
            // 
            // dfSql
            // 
            this.dfSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dfSql.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfSql.Location = new System.Drawing.Point(312, 75);
            this.dfSql.Multiline = true;
            this.dfSql.Name = "dfSql";
            this.dfSql.ReadOnly = true;
            this.dfSql.Size = new System.Drawing.Size(302, 262);
            this.dfSql.TabIndex = 2;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(304, 75);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(8, 262);
            this.splitter2.TabIndex = 1;
            this.splitter2.TabStop = false;
            // 
            // lvColumns
            // 
            this.lvColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colType});
            this.lvColumns.Dock = System.Windows.Forms.DockStyle.Left;
            this.lvColumns.FullRowSelect = true;
            this.lvColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvColumns.HideSelection = false;
            this.lvColumns.Location = new System.Drawing.Point(3, 75);
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new System.Drawing.Size(301, 262);
            this.lvColumns.TabIndex = 0;
            this.lvColumns.UseCompatibleStateImageBehavior = false;
            this.lvColumns.View = System.Windows.Forms.View.Details;
            this.lvColumns.SelectedIndexChanged += new System.EventHandler(this.lvColumns_SelectedIndexChanged);
            // 
            // colName
            // 
            this.colName.Text = "Column";
            this.colName.Width = 170;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            this.colType.Width = 100;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dfColumnsPerLine);
            this.panel4.Controls.Add(this.cbColumnAlias);
            this.panel4.Controls.Add(this.cbFullyQualifiedColumns);
            this.panel4.Controls.Add(this.dfTableAlias);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(611, 72);
            this.panel4.TabIndex = 1;
            // 
            // dfColumnsPerLine
            // 
            this.dfColumnsPerLine.Location = new System.Drawing.Point(139, 37);
            this.dfColumnsPerLine.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.dfColumnsPerLine.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dfColumnsPerLine.Name = "dfColumnsPerLine";
            this.dfColumnsPerLine.Size = new System.Drawing.Size(57, 25);
            this.dfColumnsPerLine.TabIndex = 3;
            this.dfColumnsPerLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.dfColumnsPerLine.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.dfColumnsPerLine.ValueChanged += new System.EventHandler(this.dfColumnsPerLine_ValueChanged);
            // 
            // cbColumnAlias
            // 
            this.cbColumnAlias.AutoSize = true;
            this.cbColumnAlias.Location = new System.Drawing.Point(293, 39);
            this.cbColumnAlias.Name = "cbColumnAlias";
            this.cbColumnAlias.Size = new System.Drawing.Size(102, 21);
            this.cbColumnAlias.TabIndex = 2;
            this.cbColumnAlias.Text = "Column Alias";
            this.cbColumnAlias.UseVisualStyleBackColor = true;
            this.cbColumnAlias.CheckedChanged += new System.EventHandler(this.Shared_CheckedChanged);
            // 
            // cbFullyQualifiedColumns
            // 
            this.cbFullyQualifiedColumns.AutoSize = true;
            this.cbFullyQualifiedColumns.Location = new System.Drawing.Point(293, 8);
            this.cbFullyQualifiedColumns.Name = "cbFullyQualifiedColumns";
            this.cbFullyQualifiedColumns.Size = new System.Drawing.Size(160, 21);
            this.cbFullyQualifiedColumns.TabIndex = 2;
            this.cbFullyQualifiedColumns.Text = "Fully qualified Columns";
            this.cbFullyQualifiedColumns.UseVisualStyleBackColor = true;
            this.cbFullyQualifiedColumns.CheckedChanged += new System.EventHandler(this.Shared_CheckedChanged);
            // 
            // dfTableAlias
            // 
            this.dfTableAlias.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.dfTableAlias.Location = new System.Drawing.Point(139, 6);
            this.dfTableAlias.Name = "dfTableAlias";
            this.dfTableAlias.Size = new System.Drawing.Size(123, 25);
            this.dfTableAlias.TabIndex = 1;
            this.dfTableAlias.TextChanged += new System.EventHandler(this.dfTableAlias_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Columns per line";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Table Alias";
            // 
            // tabClasses
            // 
            this.tabClasses.Controls.Add(this.dfClass);
            this.tabClasses.Controls.Add(this.panel5);
            this.tabClasses.Location = new System.Drawing.Point(4, 26);
            this.tabClasses.Name = "tabClasses";
            this.tabClasses.Padding = new System.Windows.Forms.Padding(3);
            this.tabClasses.Size = new System.Drawing.Size(617, 340);
            this.tabClasses.TabIndex = 1;
            this.tabClasses.Text = "Classes";
            this.tabClasses.UseVisualStyleBackColor = true;
            // 
            // dfClass
            // 
            this.dfClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dfClass.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfClass.Location = new System.Drawing.Point(3, 43);
            this.dfClass.Multiline = true;
            this.dfClass.Name = "dfClass";
            this.dfClass.ReadOnly = true;
            this.dfClass.Size = new System.Drawing.Size(611, 294);
            this.dfClass.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.cbINotifyPropertyChanged);
            this.panel5.Controls.Add(this.dfNamespace);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(611, 40);
            this.panel5.TabIndex = 0;
            // 
            // cbINotifyPropertyChanged
            // 
            this.cbINotifyPropertyChanged.AutoSize = true;
            this.cbINotifyPropertyChanged.Checked = true;
            this.cbINotifyPropertyChanged.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbINotifyPropertyChanged.Location = new System.Drawing.Point(367, 8);
            this.cbINotifyPropertyChanged.Name = "cbINotifyPropertyChanged";
            this.cbINotifyPropertyChanged.Size = new System.Drawing.Size(218, 21);
            this.cbINotifyPropertyChanged.TabIndex = 4;
            this.cbINotifyPropertyChanged.Text = "Support INotifyPropertyChanged";
            this.cbINotifyPropertyChanged.UseVisualStyleBackColor = true;
            this.cbINotifyPropertyChanged.CheckedChanged += new System.EventHandler(this.cbINotifyPropertyChanged_CheckedChanged);
            // 
            // dfNamespace
            // 
            this.dfNamespace.Location = new System.Drawing.Point(139, 6);
            this.dfNamespace.Name = "dfNamespace";
            this.dfNamespace.Size = new System.Drawing.Size(187, 25);
            this.dfNamespace.TabIndex = 3;
            this.dfNamespace.TextChanged += new System.EventHandler(this.dfNamespace_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Namespace";
            // 
            // SqliteOpenDialog
            // 
            this.SqliteOpenDialog.DefaultExt = "sqlite";
            this.SqliteOpenDialog.Filter = "Databases|*.sqlite;*.db|Alle Files|*.*";
            this.SqliteOpenDialog.Title = "Select database";
            // 
            // FolderDialog
            // 
            this.FolderDialog.RootFolder = System.Environment.SpecialFolder.Windows;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.lblDatabase);
            this.panel6.Controls.Add(this.lblITG);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 70);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(833, 24);
            this.panel6.TabIndex = 1;
            // 
            // lblDatabase
            // 
            this.lblDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDatabase.Location = new System.Drawing.Point(0, 0);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblDatabase.Size = new System.Drawing.Size(665, 22);
            this.lblDatabase.TabIndex = 0;
            this.lblDatabase.Text = "No database file selected";
            this.lblDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblITG
            // 
            this.lblITG.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblITG.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblITG.Location = new System.Drawing.Point(665, 0);
            this.lblITG.Name = "lblITG";
            this.lblITG.Size = new System.Drawing.Size(166, 22);
            this.lblITG.TabIndex = 1;
            this.lblITG.TabStop = true;
            this.lblITG.Text = "Copyright© 2023 by ITG";
            this.lblITG.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTips.SetToolTip(this.lblITG, "Go to the website...");
            this.lblITG.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblITG_LinkClicked);
            // 
            // SqliteLogo
            // 
            this.SqliteLogo.Dock = System.Windows.Forms.DockStyle.Right;
            this.SqliteLogo.Image = ((System.Drawing.Image)(resources.GetObject("SqliteLogo.Image")));
            this.SqliteLogo.Location = new System.Drawing.Point(757, 0);
            this.SqliteLogo.Name = "SqliteLogo";
            this.SqliteLogo.Size = new System.Drawing.Size(74, 68);
            this.SqliteLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.SqliteLogo.TabIndex = 1;
            this.SqliteLogo.TabStop = false;
            this.SqliteLogo.Click += new System.EventHandler(this.SqliteLogo_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 464);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            this.Text = "Schematix";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.SqlTabs.ResumeLayout(false);
            this.tabSql.ResumeLayout(false);
            this.tabSql.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dfColumnsPerLine)).EndInit();
            this.tabClasses.ResumeLayout(false);
            this.tabClasses.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SqliteLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ImageList Images32;
        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ListBox lbTables;
        private System.Windows.Forms.TabControl SqlTabs;
        private System.Windows.Forms.TabPage tabSql;
        private System.Windows.Forms.OpenFileDialog SqliteOpenDialog;
        private System.Windows.Forms.TextBox dfSql;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.ListView lvColumns;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbColumnAlias;
        private System.Windows.Forms.CheckBox cbFullyQualifiedColumns;
        private System.Windows.Forms.TextBox dfTableAlias;
        private System.Windows.Forms.NumericUpDown dfColumnsPerLine;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabClasses;
        private System.Windows.Forms.TextBox dfClass;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox dfNamespace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSaveClass;
        private System.Windows.Forms.Button btnSaveAllClasses;
        private System.Windows.Forms.FolderBrowserDialog FolderDialog;
        private System.Windows.Forms.CheckBox cbINotifyPropertyChanged;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.LinkLabel lblITG;
        private System.Windows.Forms.PictureBox SqliteLogo;
    }
}

