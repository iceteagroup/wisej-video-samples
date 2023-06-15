namespace DemoApp.Classes
{
    partial class BaseMdiChildForm
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
            this.WindowMenu = new Wisej.Web.MainMenu(this.components);
            this.mnWindowClose = new Wisej.Web.MenuItem();
            this.mnWindowCloseAll = new Wisej.Web.MenuItem();
            this.mnWindowCloseOthers = new Wisej.Web.MenuItem();
            this.SuspendLayout();
            // 
            // WindowMenu
            // 
            this.WindowMenu.MenuItems.AddRange(new Wisej.Web.MenuItem[] {
            this.mnWindowClose,
            this.mnWindowCloseAll,
            this.mnWindowCloseOthers});
            this.WindowMenu.Name = "WindowMenu";
            // 
            // mnWindowClose
            // 
            this.mnWindowClose.Index = 0;
            this.mnWindowClose.Name = "mnWindowClose";
            this.mnWindowClose.Text = "Schliessen";
            // 
            // mnWindowCloseAll
            // 
            this.mnWindowCloseAll.Index = 1;
            this.mnWindowCloseAll.Name = "mnWindowCloseAll";
            this.mnWindowCloseAll.Text = "Alle schliessen";
            // 
            // mnWindowCloseOthers
            // 
            this.mnWindowCloseOthers.Index = 2;
            this.mnWindowCloseOthers.Name = "mnWindowCloseOthers";
            this.mnWindowCloseOthers.Text = "Alle anderen schliessen";
            // 
            // BaseMdiChildForm
            // 
            this.ClientSize = new System.Drawing.Size(500, 270);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.None;
            this.Menu = this.WindowMenu;
            this.Name = "BaseMdiChildForm";
            this.Text = "BaseMdiChildForm";
            this.WindowState = Wisej.Web.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.MainMenu WindowMenu;
        private Wisej.Web.MenuItem mnWindowClose;
        private Wisej.Web.MenuItem mnWindowCloseAll;
        private Wisej.Web.MenuItem mnWindowCloseOthers;
    }
}