using Microsoft.WindowsAPICodePack.Dialogs;
using Schematix.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Schematix
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Application.Idle += Application_Idle;
        }

        public bool Busy
        {
            get => _Busy;
            set
            {
                _Busy = value;
                Cursor = _Busy ? Cursors.WaitCursor : Cursors.Default;
            }
        }
        private bool _Busy;

        private Options Options;

        private void Command_Open()
        {
            if (SqliteOpenDialog.ShowDialog() == DialogResult.OK)
                OpenDatabase(SqliteOpenDialog.FileName);
        }

        private void OpenDatabase(string filename)
        {
            Busy = true;
            try
            {
                DB.Open(filename);
                Options.DatabaseName = filename;
                lblDatabase.Text = filename;
                dfNamespace.Text = Path.GetFileNameWithoutExtension(filename) + ".Schema";
                LoadTables();
            }
            finally
            {
                Busy = false;
            }
        }

        private void Command_SaveClass()
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    SaveClass(dialog.FileName, lbTables.Text);
            }
        }

        private void Command_SaveAllClasses()
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    foreach (string table in lbTables.Items)
                        SaveClass(dialog.FileName, table);
                }
            }
        }

        private void SaveClass(string directoryName, string tableName)
        {
            CollectOptions();

            var columns = DB.GetColumns(tableName);
            var code = ClassGenerator.Generate(tableName, columns, Options);
            
            var filename = Path.Combine(directoryName, tableName + ".cs");
            File.WriteAllText(filename, code);
        }

        private void LoadTables()
        {
            dfClass.Clear();
            lbTables.Items.Clear();
            foreach (var table in DB.Tables)
                lbTables.Items.Add(table);
            if (lbTables.Items.Count > 0)
                lbTables.SelectedIndex = 0;
        }

        private void LoadColumns()
        {
            dfSql.Clear();
            lvColumns.Items.Clear();

            var tablename = lbTables.Text;
            if (string.IsNullOrEmpty(tablename))
                return;

            var columns = DB.GetColumns(tablename);
            foreach (var column in columns)
            {
                var item = new ListViewItem(column.name);
                item.SubItems.Add(column.CSharpType);
                item.Tag = column;
                lvColumns.Items.Add(item);
            }
        }

        private void GenerateSqlStatements()
        {
            CollectOptions();
            dfSql.Text = SqlGenerator.Generate(lbTables.Text, GetSelectedColumns(), Options);
            GenerateClass();
        }

        private void GenerateClass()
        {
            CollectOptions();
            dfClass.Text = ClassGenerator.Generate(lbTables.Text, GetAllColumns(), Options);
        }

        private List<TableColumn> GetSelectedColumns()
        {
            List<TableColumn> result = new List<TableColumn>();
            foreach (ListViewItem item in lvColumns.SelectedItems)
                result.Add(item.Tag as TableColumn);
            return result;
        }

        private List<TableColumn> GetAllColumns()
        {
            List<TableColumn> result = new List<TableColumn>();
            foreach (ListViewItem item in lvColumns.Items)
                result.Add(item.Tag as TableColumn);
            return result;
        }

        private void CollectOptions()
        {
            Options.TableAlias = dfTableAlias.Text;
            Options.ColumnAlias = cbColumnAlias.Checked;
            Options.FullyQualifiedColumns = cbFullyQualifiedColumns.Checked;
            Options.ColumnsPerLine = (int)dfColumnsPerLine.Value;
            Options.NameSpace = dfNamespace.Text;
            Options.SupportINotifyPropertyChanged = cbINotifyPropertyChanged.Checked;
        }

        private void Initialize()
        {
            Options = Options.Load();
            lblITG.Text = $"Copyright© {DateTime.Now.Year} by ITG";
            dfTableAlias.Text = Options.TableAlias;
            dfColumnsPerLine.Value = (Options.ColumnsPerLine > dfColumnsPerLine.Minimum ? Options.ColumnsPerLine : dfColumnsPerLine.Minimum);
            cbColumnAlias.Checked = Options.ColumnAlias;
            cbFullyQualifiedColumns.Checked = Options.FullyQualifiedColumns;
            dfNamespace.Text = Options.NameSpace;
            cbINotifyPropertyChanged.Checked = Options.SupportINotifyPropertyChanged;
            if (!string.IsNullOrEmpty(Options.DatabaseName) && File.Exists(Options.DatabaseName))
                OpenDatabase(Options.DatabaseName);
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            btnOpen.Enabled = !Busy;
            btnExit.Enabled = !Busy;
            btnSaveClass.Enabled = !Busy && lbTables.SelectedIndex >= 0 && !string.IsNullOrEmpty(dfNamespace.Text);
            btnSaveAllClasses.Enabled = !Busy && lbTables.SelectedIndex >= 0 && !string.IsNullOrEmpty(dfNamespace.Text);
        }

        private void Shared_ButtonClick(object sender, EventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case nameof(btnOpen):
                    Command_Open();
                    break;
                case nameof(btnExit):
                    Application.Exit();
                    break;
                case nameof(btnSaveClass):
                    Command_SaveClass();
                    break;
                case nameof(btnSaveAllClasses):
                    Command_SaveAllClasses();
                    break;
            }
        }

        private void lbTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadColumns();
            GenerateSqlStatements();
            GenerateClass();
        }

        private void lvColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateSqlStatements();
        }

        private void dfTableAlias_TextChanged(object sender, EventArgs e)
        {
            GenerateSqlStatements();
        }

        private void Shared_CheckedChanged(object sender, EventArgs e)
        {
            GenerateSqlStatements();
        }

        private void dfColumnsPerLine_ValueChanged(object sender, EventArgs e)
        {
            GenerateSqlStatements();
        }

        private void dfNamespace_TextChanged(object sender, EventArgs e)
        {
            GenerateClass();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Options.Save();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void cbINotifyPropertyChanged_CheckedChanged(object sender, EventArgs e)
        {
            GenerateClass();
        }

        private void lblITG_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.wisej.net");
        }

        private void SqliteLogo_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.sqlite.org");
        }
    }
}
