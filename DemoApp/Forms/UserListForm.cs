using Dapper;
using DemoApp.Classes;
using DemoApp.Models;
using System;
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

        private void UserListForm_Load(object sender, EventArgs e)
        {
            Commands
                .Register(btnNew,
                    () => Globals.CurrentUser.HasRight(DbPermission.PERM_Users, Rights.Edit), 
                    () => Utils.NotImplemented() )
                ;
            Lookup.Populate(colSalutation, Salutations.ItemsWithoutCompany);
            Command_Load();
        }

        private void Command_Load()
        {
            using(var conn = DB.Connection)
            {
                var sql = "select rowid, * from Users";
                dbUserBindingSource.DataSource = conn.Query<DbUser>(sql).ToList();
            }
        }
    }
}
