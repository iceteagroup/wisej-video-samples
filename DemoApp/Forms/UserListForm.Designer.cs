namespace DemoApp.Forms
{
    partial class UserListForm
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
            this.components = new System.ComponentModel.Container();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            this.panel1 = new Wisej.Web.Panel();
            this.btnEdit = new Wisej.Web.Button();
            this.btnNew = new Wisej.Web.Button();
            this.Grid = new Wisej.Web.DataGridView();
            this.colLoginname = new Wisej.Web.DataGridViewTextBoxColumn();
            this.colSalutation = new Wisej.Web.DataGridViewComboBoxColumn();
            this.colName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.colIssysadmin = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.colPassword = new Wisej.Web.DataGridViewTextBoxColumn();
            this.colPasswordenc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.colFirstname = new Wisej.Web.DataGridViewTextBoxColumn();
            this.colLastname = new Wisej.Web.DataGridViewTextBoxColumn();
            this.colJobtitle = new Wisej.Web.DataGridViewTextBoxColumn();
            this.colHired = new Wisej.Web.DataGridViewDateTimePickerColumn();
            this.colRetired = new Wisej.Web.DataGridViewDateTimePickerColumn();
            this.colIsadmin = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.colIsactive = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.colRowid = new Wisej.Web.DataGridViewTextBoxColumn();
            this.colState = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dbUserBindingSource = new Wisej.Web.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbUserBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnEdit);
            this.panel1.Controls.Add(this.btnNew);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(634, 48);
            this.panel1.TabIndex = 0;
            // 
            // btnEdit
            // 
            this.btnEdit.ImageSource = "Images\\Edit.svg";
            this.btnEdit.Location = new System.Drawing.Point(50, 8);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(32, 32);
            this.btnEdit.TabIndex = 0;
            this.btnEdit.ToolTipText = "Edit user";
            // 
            // btnNew
            // 
            this.btnNew.ImageSource = "Images\\New.svg";
            this.btnNew.Location = new System.Drawing.Point(12, 8);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(32, 32);
            this.btnNew.TabIndex = 0;
            this.btnNew.ToolTipText = "New user";
            // 
            // Grid
            // 
            this.Grid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.colLoginname,
            this.colSalutation,
            this.colName,
            this.colIssysadmin,
            this.colPassword,
            this.colPasswordenc,
            this.colFirstname,
            this.colLastname,
            this.colJobtitle,
            this.colHired,
            this.colRetired,
            this.colIsadmin,
            this.colIsactive,
            this.colRowid,
            this.colState});
            this.Grid.DataSource = this.dbUserBindingSource;
            this.Grid.Dock = Wisej.Web.DockStyle.Fill;
            this.Grid.Location = new System.Drawing.Point(0, 48);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(634, 283);
            this.Grid.TabIndex = 1;
            this.Grid.CellFormatting += new Wisej.Web.DataGridViewCellFormattingEventHandler(this.Grid_CellFormatting);
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            // 
            // colLoginname
            // 
            this.colLoginname.DataPropertyName = "Loginname";
            this.colLoginname.HeaderText = "Loginname";
            this.colLoginname.Name = "colLoginname";
            this.colLoginname.Width = 150;
            // 
            // colSalutation
            // 
            this.colSalutation.DataPropertyName = "Salutation";
            this.colSalutation.HeaderText = "Salut.";
            this.colSalutation.Name = "colSalutation";
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 250;
            // 
            // colIssysadmin
            // 
            this.colIssysadmin.DataPropertyName = "IsSysAdmin";
            this.colIssysadmin.HeaderText = "System Admin";
            this.colIssysadmin.Name = "colIssysadmin";
            this.colIssysadmin.ReadOnly = true;
            this.colIssysadmin.Visible = false;
            // 
            // colPassword
            // 
            this.colPassword.DataPropertyName = "Password";
            this.colPassword.HeaderText = "Password";
            this.colPassword.Name = "colPassword";
            this.colPassword.ShowInVisibilityMenu = false;
            this.colPassword.Visible = false;
            // 
            // colPasswordenc
            // 
            this.colPasswordenc.DataPropertyName = "PasswordEnc";
            this.colPasswordenc.HeaderText = "PasswordEnc";
            this.colPasswordenc.Name = "colPasswordenc";
            this.colPasswordenc.ShowInVisibilityMenu = false;
            this.colPasswordenc.Visible = false;
            // 
            // colFirstname
            // 
            this.colFirstname.DataPropertyName = "Firstname";
            this.colFirstname.HeaderText = "Firstname";
            this.colFirstname.Name = "colFirstname";
            this.colFirstname.ShowInVisibilityMenu = false;
            this.colFirstname.Visible = false;
            // 
            // colLastname
            // 
            this.colLastname.DataPropertyName = "Lastname";
            this.colLastname.HeaderText = "Lastname";
            this.colLastname.Name = "colLastname";
            this.colLastname.ShowInVisibilityMenu = false;
            this.colLastname.Visible = false;
            // 
            // colJobtitle
            // 
            this.colJobtitle.DataPropertyName = "JobTitle";
            this.colJobtitle.HeaderText = "Job Title";
            this.colJobtitle.Name = "colJobtitle";
            this.colJobtitle.Visible = false;
            // 
            // colHired
            // 
            this.colHired.DataPropertyName = "Hired";
            dataGridViewCellStyle3.Format = "d";
            dataGridViewCellStyle3.NullValue = null;
            this.colHired.DefaultCellStyle = dataGridViewCellStyle3;
            this.colHired.HeaderText = "Hired";
            this.colHired.Name = "colHired";
            this.colHired.Visible = false;
            // 
            // colRetired
            // 
            this.colRetired.DataPropertyName = "Retired";
            dataGridViewCellStyle4.Format = "d";
            this.colRetired.DefaultCellStyle = dataGridViewCellStyle4;
            this.colRetired.HeaderText = "Retired";
            this.colRetired.Name = "colRetired";
            this.colRetired.Visible = false;
            // 
            // colIsadmin
            // 
            this.colIsadmin.DataPropertyName = "IsAdmin";
            this.colIsadmin.HeaderText = "Admin";
            this.colIsadmin.Name = "colIsadmin";
            this.colIsadmin.Visible = false;
            // 
            // colIsactive
            // 
            this.colIsactive.DataPropertyName = "IsActive";
            this.colIsactive.HeaderText = "Active";
            this.colIsactive.Name = "colIsactive";
            this.colIsactive.Visible = false;
            // 
            // colRowid
            // 
            this.colRowid.DataPropertyName = "RowId";
            this.colRowid.HeaderText = "RowId";
            this.colRowid.Name = "colRowid";
            this.colRowid.ShowInVisibilityMenu = false;
            this.colRowid.Visible = false;
            // 
            // colState
            // 
            this.colState.DataPropertyName = "State";
            this.colState.HeaderText = "State";
            this.colState.Name = "colState";
            this.colState.ShowInVisibilityMenu = false;
            this.colState.Visible = false;
            // 
            // dbUserBindingSource
            // 
            this.dbUserBindingSource.DataSource = typeof(DemoApp.Models.DbUser);
            // 
            // UserListForm
            // 
            this.ClientSize = new System.Drawing.Size(634, 331);
            this.Controls.Add(this.Grid);
            this.Controls.Add(this.panel1);
            this.IconSource = "Images\\Mandanten.svg";
            this.Name = "UserListForm";
            this.Text = "Userlist";
            this.Load += new System.EventHandler(this.UserListForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbUserBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel panel1;
        private Wisej.Web.Button btnNew;
        private Wisej.Web.Button btnEdit;
        private Wisej.Web.DataGridView Grid;
        private Wisej.Web.DataGridViewTextBoxColumn colName;
        private Wisej.Web.DataGridViewCheckBoxColumn colIssysadmin;
        private Wisej.Web.DataGridViewTextBoxColumn colPassword;
        private Wisej.Web.DataGridViewTextBoxColumn colLoginname;
        private Wisej.Web.DataGridViewTextBoxColumn colPasswordenc;
        private Wisej.Web.DataGridViewTextBoxColumn colFirstname;
        private Wisej.Web.DataGridViewTextBoxColumn colLastname;
        private Wisej.Web.DataGridViewTextBoxColumn colJobtitle;
        private Wisej.Web.DataGridViewDateTimePickerColumn colHired;
        private Wisej.Web.DataGridViewDateTimePickerColumn colRetired;
        private Wisej.Web.DataGridViewCheckBoxColumn colIsadmin;
        private Wisej.Web.DataGridViewCheckBoxColumn colIsactive;
        private Wisej.Web.DataGridViewTextBoxColumn colRowid;
        private Wisej.Web.DataGridViewTextBoxColumn colState;
        private Wisej.Web.BindingSource dbUserBindingSource;
        private Wisej.Web.DataGridViewComboBoxColumn colSalutation;
    }
}