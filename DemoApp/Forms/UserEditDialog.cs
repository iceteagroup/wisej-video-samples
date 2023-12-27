using DemoApp.Classes;
using DemoApp.Models;
using System;
using Wisej.Web;
using WisejLib;

namespace DemoApp.Forms
{
    public partial class UserEditDialog : BaseDialog
    {
        public UserEditDialog()
        {
            InitializeComponent();
        }

        public static bool Execute (DbUser user)
        {
            using(var dlg = new  UserEditDialog())
            {
                dlg.Preset(user);
                if (dlg.ShowDialog() != DialogResult.OK)
                    return false;

                return true;
            }
        }

        private void Preset(DbUser user)
        {
            UserBindingSource.DataSource = user;
            Commands
                .Register(btnCancel, () => Close())
                .Register(btnOk, () => Command_Close())
                ;
            Lookup.Populate(dfSalutation, Salutations.ItemsWithoutCompany);
        }

        private void Command_Close()
        {
            DbUser user = (DbUser)UserBindingSource.DataSource;
            if (user.Hired > DateTime.Today)
                throw new Exception("Invalid hiring date");

            Validator.CheckNotEmpty(dfLoginname, lblLoginname);
            Validator.CheckNotEmpty(dfLastname, lblLastname);

            Close();
        }
    }
}
