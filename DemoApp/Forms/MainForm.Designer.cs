namespace DemoApp.Forms
{
    partial class MainForm
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
            this.SuspendLayout();
            // 
            // Navigation
            // 
            this.Navigation.Logo = "Images\\Logo.svg";
            this.Navigation.ShowHeader = true;
            this.Navigation.Size = new System.Drawing.Size(282, 471);
            this.Navigation.Text = "Demo Application";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(799, 471);
            this.MdiTabProperties.AllowUserToMoveTabs = true;
            this.MdiTabProperties.BackColor = System.Drawing.Color.LightSteelBlue;
            this.MdiTabProperties.ShowMdiChildMenu = true;
            this.Name = "MainForm";
            this.Text = "Window1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

