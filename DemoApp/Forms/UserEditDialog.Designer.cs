namespace DemoApp.Forms
{
    partial class UserEditDialog
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
            this.panel1 = new Wisej.Web.Panel();
            this.btnCancel = new Wisej.Web.Button();
            this.btnOk = new Wisej.Web.Button();
            this.lblLoginname = new Wisej.Web.Label();
            this.dfLoginname = new Wisej.Web.TextBox();
            this.UserBindingSource = new Wisej.Web.BindingSource(this.components);
            this.label1 = new Wisej.Web.Label();
            this.dfSalutation = new Wisej.Web.ComboBox();
            this.lblLastname = new Wisej.Web.Label();
            this.dfLastname = new Wisej.Web.TextBox();
            this.label2 = new Wisej.Web.Label();
            this.dfFirstname = new Wisej.Web.TextBox();
            this.label3 = new Wisej.Web.Label();
            this.dfHired = new Wisej.Web.DateTimePicker();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UserBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 231);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(498, 70);
            this.panel1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((Wisej.Web.AnchorStyles)((Wisej.Web.AnchorStyles.Bottom | Wisej.Web.AnchorStyles.Right)));
            this.btnCancel.DialogResult = Wisej.Web.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(367, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 37);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((Wisej.Web.AnchorStyles)((Wisej.Web.AnchorStyles.Bottom | Wisej.Web.AnchorStyles.Right)));
            this.btnOk.DialogResult = Wisej.Web.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(239, 15);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(112, 37);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            // 
            // lblLoginname
            // 
            this.lblLoginname.AutoSize = true;
            this.lblLoginname.Location = new System.Drawing.Point(13, 14);
            this.lblLoginname.Name = "lblLoginname";
            this.lblLoginname.Size = new System.Drawing.Size(68, 18);
            this.lblLoginname.TabIndex = 1;
            this.lblLoginname.Text = "Loginname";
            // 
            // dfLoginname
            // 
            this.dfLoginname.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.dfLoginname.DataBindings.Add(new Wisej.Web.Binding("Text", this.UserBindingSource, "Loginname", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.dfLoginname.Location = new System.Drawing.Point(175, 8);
            this.dfLoginname.Name = "dfLoginname";
            this.dfLoginname.Size = new System.Drawing.Size(177, 30);
            this.dfLoginname.TabIndex = 2;
            // 
            // UserBindingSource
            // 
            this.UserBindingSource.DataSource = typeof(DemoApp.Models.DbUser);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Loginname";
            // 
            // dfSalutation
            // 
            this.dfSalutation.DataBindings.Add(new Wisej.Web.Binding("SelectedValue", this.UserBindingSource, "Salutation", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.dfSalutation.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.dfSalutation.Location = new System.Drawing.Point(175, 44);
            this.dfSalutation.Name = "dfSalutation";
            this.dfSalutation.Size = new System.Drawing.Size(120, 30);
            this.dfSalutation.TabIndex = 3;
            // 
            // lblLastname
            // 
            this.lblLastname.AutoSize = true;
            this.lblLastname.Location = new System.Drawing.Point(13, 86);
            this.lblLastname.Name = "lblLastname";
            this.lblLastname.Size = new System.Drawing.Size(59, 18);
            this.lblLastname.TabIndex = 1;
            this.lblLastname.Text = "Lastname";
            // 
            // dfLastname
            // 
            this.dfLastname.DataBindings.Add(new Wisej.Web.Binding("Text", this.UserBindingSource, "Lastname", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.dfLastname.Location = new System.Drawing.Point(175, 80);
            this.dfLastname.Name = "dfLastname";
            this.dfLastname.Size = new System.Drawing.Size(177, 30);
            this.dfLastname.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Firstname";
            // 
            // dfFirstname
            // 
            this.dfFirstname.DataBindings.Add(new Wisej.Web.Binding("Text", this.UserBindingSource, "Firstname", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.dfFirstname.Location = new System.Drawing.Point(175, 116);
            this.dfFirstname.Name = "dfFirstname";
            this.dfFirstname.Size = new System.Drawing.Size(177, 30);
            this.dfFirstname.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "Hired";
            // 
            // dfHired
            // 
            this.dfHired.Checked = false;
            this.dfHired.DataBindings.Add(new Wisej.Web.Binding("Value", this.UserBindingSource, "Hired", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.dfHired.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dfHired.Location = new System.Drawing.Point(175, 153);
            this.dfHired.Name = "dfHired";
            this.dfHired.Size = new System.Drawing.Size(120, 30);
            this.dfHired.TabIndex = 4;
            this.dfHired.Value = new System.DateTime(((long)(0)));
            // 
            // UserEditDialog
            // 
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.dfHired);
            this.Controls.Add(this.dfSalutation);
            this.Controls.Add(this.dfFirstname);
            this.Controls.Add(this.dfLastname);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dfLoginname);
            this.Controls.Add(this.lblLastname);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblLoginname);
            this.Controls.Add(this.panel1);
            this.IconSource = "Images\\Mandant.svg";
            this.Name = "UserEditDialog";
            this.Text = "User";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UserBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Wisej.Web.Panel panel1;
        private Wisej.Web.Button btnCancel;
        private Wisej.Web.Button btnOk;
        private Wisej.Web.Label lblLoginname;
        private Wisej.Web.TextBox dfLoginname;
        private Wisej.Web.BindingSource UserBindingSource;
        private Wisej.Web.Label label1;
        private Wisej.Web.ComboBox dfSalutation;
        private Wisej.Web.Label lblLastname;
        private Wisej.Web.TextBox dfLastname;
        private Wisej.Web.Label label2;
        private Wisej.Web.TextBox dfFirstname;
        private Wisej.Web.Label label3;
        private Wisej.Web.DateTimePicker dfHired;
    }
}