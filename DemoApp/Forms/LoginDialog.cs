using DemoApp.Classes;
using System;
using Wisej.Web;
using WisejLib;

namespace DemoApp.Forms
{
    /// <summary>
    /// The dialog to enter loginname and password
    /// </summary>
    public partial class LoginDialog : BaseDialog
    {
        public LoginDialog() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Static method to launch the dialog
        /// </summary>
        /// <param name="loginname">Returns the entered loginname</param>
        /// <param name="password">Returns the entered password or null</param>
        /// <returns>True if the user hit Ok and false if the user hit Cancel</returns>
        public static bool Execute(out string loginname, out string password)
        {
            loginname = null;
            password = null;
            using (var dlg = new LoginDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK)
                    return false;

                loginname = dlg.dfLoginname.Text;
                password = dlg.dfPassword.Text;
                return true;
            }
        }

        /// <summary>
        /// Validate the user's input and close the dialog
        /// </summary>
        private void Command_Ok()
        {
            Validator.CheckNotEmpty(dfLoginname, "Please provide a username");
            Close();
        }

        /// <summary>
        /// Set up Commands
        /// </summary>
        private void LoginDialog_Load(object sender, EventArgs e)
        {
            // passing null as the 2nd parameter means that the button is always enabled
            // the 3rd parameter defines which action is to be performed when the user clicks the button
            Commands
                .Register(btnOk, null, () => Command_Ok())
                .Register(btnCancel, null, () => Close())
                ;
        }
    }
}
