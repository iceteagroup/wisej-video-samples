using System;
using System.Collections.Generic;
using System.Linq;
using Wisej.Web;

namespace DemoApp.Classes
{
    /// <summary>
    /// The mother of all mdi child forms. It contains a context menu to close one or child 
    /// forms, it can be accessed by right-clicking on the form's tab
    /// </summary>
    public partial class BaseMdiChildForm : BaseForm
    {
        /// <summary>
        /// Creates a new BaseMdiChildForm
        /// </summary>
        public BaseMdiChildForm() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The identifier that makes this mdi child form unique
        /// </summary>
        public string UniqueFormId { get; set; }

        /// <summary>
        /// The list of parameters that has been passed during creation of the mdi child form.
        /// Paramneters are stored as KeyValuePair of types string and object. This enables
        /// the Parameters list to store values of any kind but comes with the disadvantage
        /// of having to cast the value to it's specific type each time it is read.
        /// See also: BaseMdiChildForm.GetParameter
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Retrieves a parameter value that has been passed during creation of the mdi child form.
        /// If the parameter name does not exist, the default value of the type is returned. 
        /// </summary>
        /// <typeparam name="T">The type of the parameter value</typeparam>
        /// <param name="name">The name of the parameter. If the parameter name does not exist, the default value of the type is returned.</param>
        /// <returns>The type parameter value or its defualt value if it doesn't exist</returns>
        protected T GetParameter<T>(string name) => Parameters.ContainsKey(name) ? (T)Parameters[name] : default;

        /// <summary>
        /// THis overriden method register the commands of the mdi child window menu before the load event is fired
        /// </summary>
        /// <param name="e">No useful information in here</param>
        protected override void OnLoad(EventArgs e)
        {
            Commands
                .Register(mnWindowClose, null, () => Close())
                .Register(mnWindowCloseAll, null, () => Command_WindowCloseAll())
                .Register(mnWindowCloseOthers, () => MdiParent.MdiChildren.Length > 1, () => Command_WindowCloseOthers())
                ;

            base.OnLoad(e);
        }

        /// <summary>
        /// This method calls the same method in BaseMainForm. See BaseMainForm.CreatMdiChildForm
        /// </summary>
        /// <typeparam name="T">The type of the form to be created as a mdi child form. See BaseMainForm.CreatMdiChildForm</typeparam>
        /// <param name="uniqueFormId">(optional)A unique value for the form to be created. See BaseMainForm.CreatMdiChildForm</param>
        /// <param name="parameters">(otional)You can pass parameters to the mdi child form. See BaseMainForm.CreatMdiChildForm</param>
        /// <returns>The newly created mdi child form See BaseMainForm.CreatMdiChildForm</returns>
        protected BaseMdiChildForm CreateMdiChildForm<T>(string uniqueFormId = null, params (string Name, object Value)[] parameters)
        {
            return (MdiParent as BaseMainForm).CreateMdiChildForm<T>(uniqueFormId, parameters);
        }

        /// <summary>
        /// Command execution handler for menu item mnWindowCloseAll
        /// Closes all mdi child windows.
        /// </summary>
        private void Command_WindowCloseAll()
        {
            for (int i = MdiParent.MdiChildren.Length - 1; i >= 0; i--)
                MdiParent.MdiChildren[i].Close();
        }

        /// <summary>
        /// Command execution handler for menu item mnWindowCloseOthers.
        /// Closes all mdi child windows except the current one
        /// </summary>
        private void Command_WindowCloseOthers()
        {
            for (int i = MdiParent.MdiChildren.Length - 1; i >= 0; i--)
                if (MdiParent.MdiChildren[i] != this)
                    MdiParent.MdiChildren[i].Close();
        }
    }
}
