namespace DemoApp.Forms
{
    partial class LoginDialog
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

        #region Wisej Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginDialog));
            this.btnCancel = new Wisej.Web.Button();
            this.btnOk = new Wisej.Web.Button();
            this.dfPassword = new Wisej.Web.TextBox();
            this.label2 = new Wisej.Web.Label();
            this.dfLoginname = new Wisej.Web.TextBox();
            this.lblLoginname = new Wisej.Web.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((Wisej.Web.AnchorStyles)((Wisej.Web.AnchorStyles.Bottom | Wisej.Web.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.FromName("@buttonFace");
            this.btnCancel.DialogResult = Wisej.Web.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(360, 252);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 32);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((Wisej.Web.AnchorStyles)((Wisej.Web.AnchorStyles.Bottom | Wisej.Web.AnchorStyles.Right)));
            this.btnOk.BackColor = System.Drawing.Color.FromName("@buttonFace");
            this.btnOk.DialogResult = Wisej.Web.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(218, 252);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(120, 32);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Login";
            // 
            // dfPassword
            // 
            this.dfPassword.Anchor = ((Wisej.Web.AnchorStyles)(((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.dfPassword.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.dfPassword.InputType.Type = Wisej.Web.TextBoxType.Password;
            this.dfPassword.Location = new System.Drawing.Point(218, 124);
            this.dfPassword.MaxLength = 50;
            this.dfPassword.Name = "dfPassword";
            this.dfPassword.PasswordChar = '*';
            this.dfPassword.Size = new System.Drawing.Size(215, 30);
            this.dfPassword.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(68, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password";
            // 
            // dfLoginname
            // 
            this.dfLoginname.Anchor = ((Wisej.Web.AnchorStyles)(((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.dfLoginname.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.dfLoginname.Location = new System.Drawing.Point(218, 79);
            this.dfLoginname.MaxLength = 50;
            this.dfLoginname.Name = "dfLoginname";
            this.dfLoginname.Size = new System.Drawing.Size(215, 30);
            this.dfLoginname.TabIndex = 1;
            // 
            // lblLoginname
            // 
            this.lblLoginname.AutoSize = true;
            this.lblLoginname.BackColor = System.Drawing.Color.Transparent;
            this.lblLoginname.ForeColor = System.Drawing.Color.White;
            this.lblLoginname.Location = new System.Drawing.Point(68, 85);
            this.lblLoginname.Name = "lblLoginname";
            this.lblLoginname.Size = new System.Drawing.Size(74, 18);
            this.lblLoginname.TabIndex = 0;
            this.lblLoginname.Text = "Login Name";
            // 
            // LoginDialog
            // 
            this.AcceptButton = this.btnOk;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(500, 300);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.dfPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dfLoginname);
            this.Controls.Add(this.lblLoginname);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.None;
            this.Name = "LoginDialog";
            this.Text = "Anmeldung";
            this.Load += new System.EventHandler(this.LoginDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Wisej.Web.Button btnCancel;
        private Wisej.Web.Button btnOk;
        private Wisej.Web.TextBox dfPassword;
        private Wisej.Web.Label label2;
        private Wisej.Web.TextBox dfLoginname;
        private Wisej.Web.Label lblLoginname;
    }
}