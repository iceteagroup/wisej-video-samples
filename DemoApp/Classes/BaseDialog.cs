using System;
using Wisej.Web;
using WisejLib;

namespace DemoApp.Classes
{
    /// <summary>
    /// The BaseDialog is the base for all dialog forms. It doesn't implement any new 
    /// funtionality, just the properties are set to make it a dialog with fixed border
    /// and CenterParent
    /// </summary>
    public partial class BaseDialog : BaseForm
    {
        /// <summary>
        /// Creates a new BaseDialog
        /// </summary>
        public BaseDialog() : base()
        {
            InitializeComponent();
        }
    }
}
