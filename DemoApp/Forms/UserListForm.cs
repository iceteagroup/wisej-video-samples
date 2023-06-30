using Dapper;
using DemoApp.Classes;
using DemoApp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Wisej.Web;
using WisejLib;

namespace DemoApp.Forms
{
    public partial class UserListForm : BaseMdiChildForm
    {
        public UserListForm() : base()
        {
            InitializeComponent();
        }

        private List<DbUser> DataList => (List<DbUser>)dbUserBindingSource.DataSource;

        private void UserListForm_Load(object sender, EventArgs e)
        {
            Grid.SetDefaults();
            Commands
                .Register(btnNew, () => true, () => Utils.NotImplemented())
                .Register(btnEdit, ()=> Grid.SelectedCount() == 1, () => Command_Edit())
                ;

            Lookup.Populate(colSalutation, Salutations.Items);

            Command_Load();
        }

        private void Command_Load()
        {
            using(var conn = DB.Connection)
            {
                var sql = "select rowid, * from Users order by Lastname, Firstname";
                dbUserBindingSource.DataSource = conn.Query<DbUser>(sql).ToList();
            }
        }

        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            Command_Edit();
        }

        private void Command_Edit()
        {
            Utils.NotImplemented();
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var data = DataList[e.RowIndex];

            if(data.IsAdmin)
                e.CellStyle.Font = Globals.FONT_Bold;

            if (data.IsSysAdmin)
                e.CellStyle.ForeColor = Color.Red;
        }
    }
}
