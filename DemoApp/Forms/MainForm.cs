
using DemoApp.Classes;
using DemoApp.Models;
using System;
using System.IO;
using Wisej.Web;
using WisejLib;

namespace DemoApp.Forms
{
    public partial class MainForm : BaseMainForm
    {
        public MainForm() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Add all items and their commands to the navigation bar, depending on the current user's permissions
        /// </summary>
        /// <param name="user"></param>
        private void PopulateNavigationBar(DbUser user)
        {
            NavBar.Items.Clear();

            // add toplevel modules
            if (user.HasRight(DbPermission.PERM_Addresses))
                AddNavItem(null, "Addresses", "Address.svg", item => Utils.NotImplemented());

            // add a sub level but only if there is at least 1 navigation item underneath
            NavBarGroup = "Administration";
            if (user.HasRight(DbPermission.PERM_Addresses))
                AddNavItem(NavBarGroupItem, "Users", "Users.svg", item => Utils.NotImplemented());
        }

        /// <summary>
        /// Calls the LoginDialog, authenticates the user from the Users database table and fills the navigation bar
        /// </summary>
        private void LoginUser()
        {
            // the user has 3 attempts to enter credentials until the app terminates
            int tryCount = 3;

            // if Autologin is true, loginname and password must have reasonable content
            bool AutoLogin = false;
            string loginname = "JOE";
            string password = null;

            // don't stop until 3 failed attempts or the user hits Cancel
            while (tryCount > 0)
            {
                // asking user to enter credentials. Exeute returns false when the user hits cancel
                if (AutoLogin || LoginDialog.Execute(out loginname, out password))
                {
                    // read the user data from the database. If null, the loginname doesn't exist or the user is inactive
                    var user = DbUser.GetByLoginname(loginname);
                    if (user != null)
                    {
                        // check password
                        var passwordOk =
                            (string.IsNullOrEmpty(user.PasswordEnc) && string.IsNullOrEmpty(password)) ||
                            (user.PasswordEnc == password.Encrypt());
                        if (passwordOk)
                        {
                            // the current user is stored in Globals because it is used in many forms
                            Globals.CurrentUser = user;
                            
                            // display user name in the navigation bar
                            NavBar.UserName = user.Name;
                            
                            // fill the navigation bar with items
                            PopulateNavigationBar(user);

                            break;
                        }
                        else
                            // wrong password
                            Utils.MsgBox("Zugriff verweigert|Falsches Kennwort", MessageBoxIcon.Stop);
                    }
                    else
                        // loginname doesn't exist or user is inactive
                        Utils.MsgBox("Zugriff verweigert|Benutzer existiert nicht oder ist nicht aktiv", MessageBoxIcon.Stop);
                }
                else
                {
                    // user hit cancel
                    Application.Exit();
                    return;
                }

                // if the allowed number of tries has been reached stop the application
                tryCount--;
                if (tryCount < 1)
                {
                    Application.Exit();
                    return;
                }

                // make sure that the login dialog appears when auto-login of ADMIN failed
                AutoLogin = false;
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            // Fonts are created only once and can be used everywhere
            Globals.InitFonts(this.Font);

            Utils.MsgBox("This app|Hint|Brush teeth twice a day", MessageBoxIcon.Hand);

            var AppName = Application.Title;

            // it is assumned that the sqlite database is in the same directory as the executable
            DB.ConnectionString = $"Data Source={AppName}.sqlite;";

            // initialize the database with default content
            Initializer.Execute();

            // and last but not least let the user log in
            LoginUser();
        }
    }
}
