using Dapper;
using DemoApp.Models;
using System;
using Wisej.Web;
using WisejLib;
using static WisejLib.Utils;

namespace DemoApp.Classes
{
    /// <summary>
    /// The mother of all forms. It implements CommandHandler logic, stores and restores 
    /// the column configuration of all DataGridViews that reside on the form and maintains
    /// a Modified flag that changes to true as soon the user changes the content of any 
    /// control on the form
    /// </summary>
    public partial class BaseForm : Form
    {
        /// <summary>
        /// Create a new BaseForm
        /// </summary>
        public BaseForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// CommandHandler instance.
        /// Not instantiated before its is used for the 1st time (lazy loading)
        /// </summary>
        protected CommandHandler Commands
        {
            get
            {
                if (_Commands == null)
                    _Commands = new CommandHandler();
                return _Commands;
            }
        }

        /// <summary>
        /// Backup field for Commands property
        /// </summary>
        private CommandHandler _Commands;

        /// <summary>
        /// Set to true after the Load event finished.
        /// This way controls can check in their event handlers, if initialization has 
        /// already completed before they do things that cannot be done before 
        /// initialization.
        /// THis applies for example to the SelectionChanged event of comboboxes. They
        /// are typically initialized during the Load event and when the combobox' 
        /// DataSource property is assigned, the SelectionChanged event is fired. There
        /// you can check if CreateCompleted is true before you do anything in the 
        /// combobox' SelectionChanged event
        /// </summary>
        protected bool CreateCompleted { get; private set; }

        /// <summary>
        /// Modified is set to true as soon as the user changes something in any control 
        /// of the form. BaseForm uses the ModifiedChanged event to do this
        /// </summary>
        public bool Modified { get; set; }

        /// <summary>
        /// Loads settings from the database
        /// </summary>
        /// <param name="grid"></param>
        protected void LoadGridSettings(DataGridView grid)
        {
            using (var conn = DB.Connection)
            {
                var setting = DbSetting.GetByName(conn, GetGridSettingsName(grid), Globals.CurrentUser.RowId);
                if (setting != null)
                    grid.ApplyLayout(setting.SettingsData);
            }
        }

        /// <summary>
        /// Saves settings to the database
        /// </summary>
        /// <param name="grid"></param>
        protected void SaveGridSettings(DataGridView grid)
        {
            using (var conn = DB.Connection)
            using (var tx = conn.BeginTransaction())
            {
                var setting = new DbSetting
                {
                    UserId = Globals.CurrentUser.RowId,
                    SettingsName = GetGridSettingsName(grid),
                    SettingsData = grid.GetLayoutString(),
                };

                var sql =
                    "delete from settings where UserId = @UserId and SettingsName = @SettingsName; \n" +
                    "insert into settings (UserId, SettingsName, SettingsData) \n" +
                    "values (@UserId, @SettingsName, @SettingsData) \n" +
                    "returning rowid";

                conn.Execute(sql, setting, tx);
                tx.Commit();
            }
        }

        /// <summary>
        /// Create a settings name
        /// </summary>
        private string GetGridSettingsName(DataGridView grid)
        {
            return $"LAYOUT_{Name}_{grid.Name}";
        }

        /// <summary>
        ///  Applies the grid settings to all grids
        /// </summary>
        protected virtual void LoadSettings()
        {
            ForEachControl(this, ctrl =>
            {
                if (ctrl is DataGridView grid)
                    LoadGridSettings(grid);
            });
        }

        /// <summary>
        ///  Saves the grid settings of all grids to the database
        /// </summary>
        protected virtual void SaveSettings()
        {
            ForEachControl(this, ctrl =>
            {
                if (ctrl is DataGridView grid)
                    SaveGridSettings(grid);
            });
        }

        protected void SetReadOnly()
        {
            Utils.ForEachControl(this, ctrl =>
            {
                if (ctrl is IReadOnly roControl)
                    roControl.ReadOnly = true;
            });
        }

        /// <summary>
        /// Load is overriden to set CreateCompleted to true after the event is finished
        /// </summary>
        /// <param name="e">Not of interest</param>
        protected override void OnLoad(EventArgs e)
        {
            LoadSettings();
            HookupModifiedHandler();
            base.OnLoad(e);
            Modified = false;
            CreateCompleted = true;
        }

        private void HookupModifiedHandler()
        {
            Utils.ForEachControl(this, ctrl =>
            {
                if (ctrl is IModified)
                    (ctrl as IModified).ModifiedChanged += Shared_ModifiedChanged;
                else if (ctrl is CheckBox checkBox)
                    checkBox.CheckedChanged += Shared_ModifiedChanged;
            });
        }

        private void Shared_ModifiedChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        /// <summary>
        /// When the form closes, dsave the settings of all grids
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveSettings();
            base.OnFormClosing(e);
        }
    }
}
