using System;
using System.Collections.Generic;
using System.Linq;
using Wisej.Web;

namespace WisejLib
{
    /// <summary>
    /// CommandHandler takes care of enabling/disabling buttons, menu items and component tools and 
    /// their click actions.
    /// Create 1 instance of CommandHandler on a form. For each Button, MenuItem and ComponentTool
    /// call one of the Register functions and pass delegates for enabling and executing. The rest
    /// is done by the CommandHandler.
    /// Typical code might look like this:
    ///     CommandHandler commands = new CommandHandler();
    ///     commands
    ///         .Register(btnOpen, null, () => Command_Open())
    ///         .Register(btnProcess, () => !Busy, () => Command_Process())
    ///         .Register(mnOpenMenuItem, btnOpen)
    ///         ;
    /// The code defines 2 Buttons. btnOpen is always enabled because null is passed to the
    /// enableCallback paraneter. If btnOpen is clicked, the Command_Open() function is invoked.
    /// The 2nd button is only enabled if the Busy property is false which means if you set Busy
    /// to false, btnProcess is automatically disabled. When clicked, Command_Process() is
    /// invoked.
    /// The 3rd registration creates a command item for the mnOpenMenuItem menu item. Instead of
    /// assigning enabledCallback and executeCallback, btnOpen is passed. enabledCallback and 
    /// executeCallback of btnOpen are copied to the mnOpenMenuItem menu item and it behaves
    /// simultaneously to the btnOpen button. This is useful when you have a context menu that
    /// duplicates the functionality of buttons on the form.
    /// </summary>
    public class CommandHandler
    {
        /// <summary>
        /// Constructor sets up Application.Idle event
        /// </summary>
        public CommandHandler()
        {
            Application.Idle += Application_Idle;
        }

        private readonly List<CommandItem> Items = new List<CommandItem>();

        /// <summary>
        /// Register a menu item by assigning the same actions as a specified button.
        /// This exists mainly because of context menus which perform the same actions
        /// as buttons.
        /// </summary>
        /// <param name="menuItem">The menu item to be registered</param>
        /// <param name="button">The button whose actions are copied for the menu item</param>
        public CommandHandler Register(MenuItem menuItem, Button button)
        {
            if (!TryGetCommandItem(button, out CommandItem item))
                throw new Exception($"Button {button.Name} is not registered");

            return Register(menuItem, item.EnabledCallback, item.ExecuteCallback);
        }

        /// <summary>
        /// Register a cotrol for some specific actions which ist always enabled
        /// </summary>
        /// <param name="control">The control to be registered</param>
        /// <param name="executeCallback">Delegate that is executed when the control's action has to be executed</param>
        public CommandHandler Register(object control, Action executeCallback)
        {
            return Register(control, null, executeCallback);
        }

        /// <summary>
        /// Register a cotrol for some specific actions
        /// </summary>
        /// <param name="control">The control to be registered</param>
        /// <param name="enabledCallback">Delegate that returns true if the control is enabled. You can use AlwaysEnabled() here</param>
        /// <param name="executeCallback">Delegate that is executed when the control's action has to be executed</param>
        public CommandHandler Register(object control, Func<bool> enabledCallback, Action executeCallback)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (!(control is Control) && !(control is MenuItem))
                throw new Exception($"Type {control.GetType().Name} is not supported in CommandHandler");

            if (executeCallback is null)
                throw new ArgumentNullException(nameof(executeCallback));

            // each control can only be registered once
            if (TryGetCommandItem(control, out _))
            {
                string name;
                switch (control)
                {
                    case Control ctrl:
                        name = ctrl.Name;
                        break;
                    case MenuItem menuItem:
                        name = menuItem.Name;
                        break;
                    default:
                        name = control.GetType().Name;
                        break;
                }
                throw new Exception($"Control {name} is already registered");
            }

            // create a new command item
            Items.Add(new CommandItem(control, enabledCallback ?? AlwaysEnabled, executeCallback));

            // hooking up a click event handler unfortunately requires to handle MenuItem differently
            // because MenuItem is not a control
            switch (control)
            {
                case Control ctrl:
                    ctrl.Click += Button_Click;
                    break;
                case MenuItem menuItem:
                    menuItem.Click += Button_Click;
                    break;
            }

            // return "this" for fluent interface
            return this;
        }

        /// <summary>
        /// Registers a control's tool
        /// </summary>
        /// <param name="control">The control that contains the tool</param>
        /// <param name="toolName">The name property of the tool</param>
        /// <param name="enabledCallback">Delegate that returns true if the control is enabled. You can use AlwaysEnabled() here</param>
        /// <param name="executeCallback">Delegate that is executed when the control's action has to be executed</param>
        public CommandHandler Register(object control, string toolName, Func<bool> enabledCallback, Action executeCallback)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (executeCallback is null)
                throw new ArgumentNullException(nameof(executeCallback));

            if (string.IsNullOrEmpty(toolName))
                throw new ArgumentException($"'{nameof(toolName)}' cannot be null or empty.");

            var tool = FindToolByName(control as Control, toolName) ?? throw new ArgumentException($"Tool '{nameof(toolName)}' not found.");

            // Tools are of type "ComponentTool" which is not a control so it has to handled differently.
            // Additionally the click event has to be handled differently, too

            // Can only register a tool once
            if (TryGetCommandItem(tool, out _))
                throw new Exception($"Tool is already registered");

            // create a command item from the ComponentTool
            Items.Add(new CommandItem(tool, enabledCallback ?? AlwaysEnabled, executeCallback));

            // The tool doesn't have a click event itself, instead the control containing the tool
            // has a ToolClick event and the clicked tool is passed in the EventArgs. This means
            // we have to hook up an individual click handler for tools.
            // A control can have more than 1 tool but has only 1 ToolClick event. This means that
            // we cannot add an event handler for every tool registration. Unfortunately there is
            // no easy way to check, if a certain event handler has been assigned the the ToolClick
            // event. To fix this problem I first remove the event handler and then add it again.
            // This makles sure that every control has only 1 ToolClick event handler assigned
            // regardless of the number of tools registered for the control. Coll, isn't it? If
            // anybody has a better solution I'd be happy for every idea
            switch (control)
            {
                case TextBoxBase textBoxBase:
                    textBoxBase.ToolClick -= Tool_Click;
                    textBoxBase.ToolClick += Tool_Click;
                    break;
                case ComboBox comboBox:
                    comboBox.ToolClick -= Tool_Click;
                    comboBox.ToolClick += Tool_Click;
                    break;
                case DateTimePicker dateTimePicker:
                    dateTimePicker.ToolClick -= Tool_Click;
                    dateTimePicker.ToolClick += Tool_Click;
                    break;
                case DataGridView dataGridView:
                    dataGridView.ToolClick -= Tool_Click;
                    dataGridView.ToolClick += Tool_Click;
                    break;
                case TreeView treeView:
                    treeView.ToolClick -= Tool_Click;
                    treeView.ToolClick += Tool_Click;
                    break;
                // add more controls here if needed
                default:
                    throw new Exception($"Unsupported object type: {control.GetType().Name}");
            }
            return this;
        }

        /// <summary>
        /// Execute a control's action if the control is enabled
        /// </summary>
        /// <param name="control">A registered control</param>
        public void Execute(object control)
        {
            // the app can safely call Execute to execute the command associated with the control
            // if the control hasn't been registered, nothing happens. If the control is registered
            // but not enablked, nothing happens either.
            if (TryGetCommandItem(control, out CommandItem item) && item.EnabledCallback())
                item.ExecuteCallback();
        }

        /// <summary>
        /// Delete all registrations. This will probably never be needed but anyway
        /// </summary>
        public void Clear()
        {
            Items.Clear();
        }

        /// <summary>
        /// Enabled/disables all registered controls
        /// </summary>
        public void Update()
        {
            // Actually I am not sure if this will ever be needed because the Idle event should
            // be calling EnableCommands() alrwady.
            EnableCommands();
        }

        /// <summary>
        /// Loops over all command items to find the control or ComponentTool
        /// </summary>
        /// <param name="control">The control to search for</param>
        /// <param name="commandItem">THe found CommandItem</param>
        /// <returns>Returns true if the command item was found</returns>
        private bool TryGetCommandItem(object control, out CommandItem commandItem)
        {
            commandItem = null;
            foreach (var item in Items)
                if (item.Control.Equals(control))
                {
                    commandItem = item;
                    return true;
                }
            return false;
        }

        /// <summary>
        /// Takes a tool name and returns the ComponentTool instance
        /// </summary>
        /// <param name="control">The control that contains the tool name</param>
        /// <param name="toolName">The name of the tool to search for</param>
        /// <returns>Returns the ComponentTool instance fpr the toll name</returns>
        private static ComponentTool FindToolByName(Control control, string toolName)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));
            if (string.IsNullOrEmpty(toolName))
                return null;

            // To search for the tool we must have the Tools collection. Unfortunately
            // I've found no easy way to get the Tools collection from a common class
            // from which Controls inherit. INstead the TOols collections ist defined
            // individually in each class. So the only way I see is to check the controls
            // that are frequently used in the app and retrieve the Tools collection
            // individually
            ComponentToolCollection tools;
            switch (control)
            {
                case TextBoxBase textBoxBase:
                    tools = textBoxBase.Tools;
                    break;
                case ComboBox comboBox:
                    tools = comboBox.Tools;
                    break;
                case DateTimePicker dateTimePicker:
                    tools = dateTimePicker.Tools;
                    break;
                case DataGridView dataGridView:
                    tools = dataGridView.Tools;
                    break;
                case TreeView treeView:
                    tools = treeView.Tools;
                    break;
                // add more controls as needed....
                default:
                    return null;
            }

            // Now that we have the Tools collection we return the ComponentTool instance
            // or null if not found
            return tools.SingleOrDefault(t => t.Name.Equals(toolName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Fires when the Click event of a registered control is performed
        /// </summary>
        private void Button_Click(object sender, EventArgs e)
        {
            Execute(sender);
        }

        /// <summary>
        /// Fires when the ToolClick event of a registered control is performed
        /// </summary>
        /// <param name="sender">The control containing the tool</param>
        /// <param name="e">These ToolClickEventArgs contain a reference to the tool</param>
        private void Tool_Click(object sender, ToolClickEventArgs e)
        {
            Execute(e.Tool);
        }

        /// <summary>
        /// The Idle event occurs when the application finished processing the message queue.
        /// THis comes in very handy because it basically means, the event fires very frequently
        /// anway, so we don't need a Timer to enable/disable commands. Using Idle to
        /// enable/disable commands has proven to work nicely. See also: https://t1p.de/l9lud
        /// </summary>
        private void Application_Idle(object sender, EventArgs e)
        {
            EnableCommands();
        }

        /// <summary>
        /// Loops over all command items and enables/disables them according to their Enabled
        /// callback result
        /// </summary>
        private void EnableCommands()
        {
            foreach (var commandItem in Items)
            {
                var enabled = commandItem.EnabledCallback();
                switch (commandItem.Control)
                {
                    case Button button:
                        button.Enabled = enabled;
                        break;
                    case MenuItem menuItem:
                        menuItem.Enabled = enabled;
                        break;
                    case ComponentTool tool:
                        tool.Enabled = enabled;
                        break;
                }
            }
        }

        /// <summary>
        /// This is a little helper that can be passed to the enabledCallback parameter
        /// of Register mnethods. It comes in handy if a command needs to be enabled
        /// all the time and never be disabled. If you pass null to the enabledCallback 
        /// parameter, AlwaysEnabled is internally used.
        /// </summary>
        /// <returns>Alweays returns true</returns>
        private bool AlwaysEnabled() => true;

        #region CommandItem ---------------------------------------------------------------

        /// <summary>
        /// Item of the Commandhandler that contains delegates for enabling and executing
        /// </summary>
        private class CommandItem
        {
            public CommandItem(object control, Func<bool> enabledCallback, Action executeCallback)
            {
                Control = control;
                EnabledCallback = enabledCallback;
                ExecuteCallback = executeCallback;
            }

            public object Control { get; set; }
            public Func<bool> EnabledCallback { get; }
            public Action ExecuteCallback { get; }
        }

        #endregion CommandItem
    }

}
