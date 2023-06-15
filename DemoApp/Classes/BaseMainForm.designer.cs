namespace DemoApp.Classes
{
    partial class BaseMainForm
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
            this.Navigation = new Wisej.Web.Ext.NavigationBar.NavigationBar();
            this.SuspendLayout();
            // 
            // Navigation
            // 
            this.Navigation.Dock = Wisej.Web.DockStyle.Left;
            this.Navigation.Indentation = 16;
            this.Navigation.Name = "Navigation";
            this.Navigation.ShowUser = false;
            this.Navigation.Size = new System.Drawing.Size(282, 650);
            this.Navigation.TabIndex = 1;
            this.Navigation.Text = "NavBar";
            this.Navigation.ItemClick += new Wisej.Web.Ext.NavigationBar.NavigationBarItemClickEventHandler(this.NavBar_ItemClick);
            // 
            // BaseMainForm
            // 
            this.ClientSize = new System.Drawing.Size(1200, 650);
            this.Controls.Add(this.Navigation);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.MdiTabProperties.AllowUserToMoveTabs = true;
            this.MdiTabProperties.BackColor = System.Drawing.Color.LightSteelBlue;
            this.MdiTabProperties.ShowMdiChildMenu = true;
            this.Name = "BaseMainForm";
            this.Text = "BaseMainForm";
            this.WindowState = Wisej.Web.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        protected Wisej.Web.Ext.NavigationBar.NavigationBar Navigation;
    }
}