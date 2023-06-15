﻿using System;
using System.IO;
using Wisej.Web.Ext.NavigationBar;
using WisejLib;

namespace DemoApp.Classes
{
    /// <summary>
    /// The main form's generic parent including the Navigation Bar component and help funktions to add 
    /// items to the Navigation Bar
    /// </summary>
    public partial class BaseMainForm : BaseForm
    {
        /// <summary>
        /// Create a BaseMainForm
        /// </summary>
        public BaseMainForm() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// You can pass this constant to the 1st parameter of CreateMdiChildForm to indicate that forms
        /// of the specified type can only exist once. Passing UNIQUE_FORM_NAME is identical to passing
        /// the form's type name
        /// </summary>
        public const string UNIQUE_FORM_NAME = "*";

        /// <summary>
        /// Since the NavigationBar is in BaseMainForm.Designer.cs and it is private this property makes it accessible.
        /// Changing the Modifier property of the NavigationBar control to protected or public would be possible but 
        /// then the XML documentation requests a comment and commenting something in the BaseMainForm.Designer.cs file 
        /// is a really bad idea because the file is generated by the designer.
        /// </summary>
        protected NavigationBar NavBar => Navigation;

        /// <summary>
        /// The folder where the images of the application are located. The default is "Images"
        /// </summary>
        protected string NavBarImageFolder { get; set; } = "Images";

        /// <summary>
        /// The standard image for parent items of the NavigationBar. The default is "Folder.svg"
        /// </summary>
        protected string NavBarGroupImage { get; set; } = "Folder.svg";

        /// <summary>
        /// NavBarGroup defines the text of the navigation item when NavBarGroupItem is accessed the next time.
        /// NavBarGroup must be set before accessing NavBarGroupItem.
        /// The navigation group item is not being created before it is accessed for the first time. So as long as there are no children
        /// added to the NavBarGroupItem the NavBarGroupItem doesn't appear at all.
        /// If you add NavigationBarItems depending on whether the current user has access rights or not, it may happen that the user
        /// doesn't have rights to see any of the NavigationBarItems under a NavBarGroupItem. In other words, the NavBarGroupItem isn't 
        /// needed at all. Not creating the NavBarGroupItem until the first child item is added to it, makes sure there are no empty 
        /// navigation bar groups
        /// 
        /// Example:
        ///     // add toplevel modules
        ///     if (user.HasRight(DbPermission.PERM_Addresses))
        ///         AddNavItem(null, "Addresses", "Address.svg", item => Utils.NotImplemented());
        ///         
        ///     // add a sub level but only if there is at least 1 navigation item underneath
        ///     NavBarGroup = "Administration";
        ///     if (user.HasRight(DbPermission.PERM_Addresses))
        ///         AddNavItem(NavBarGroupItem, "Users", "Users.svg", item => Utils.NotImplemented());
        ///         
        /// </summary>
        protected string NavBarGroup { get; set; }

        /// <summary>
        /// Returns a NavBarGroupItem. Child items can be created under this item.
        /// It is not created until it is accessed for the first time. For more explanation see NavBarGroup.
        /// </summary>
        protected NavigationBarItem NavBarGroupItem
        {
            get
            {
                if (_NavBarGroupItem == null)
                {
                    if (string.IsNullOrEmpty(NavBarGroup))
                        throw new Exception("NavBarGroup must be set before accessing NavBarGroupItem");
                    _NavBarGroupItem = AddNavItem(null, NavBarGroup, NavBarGroupImage, null);
                }
                return _NavBarGroupItem;
            }
        }
        private NavigationBarItem _NavBarGroupItem;

        /// <summary>
        /// Use this method to create mdi child forms. For example:
        /// 
        /// CreateMdiChildForm with type AddressListForm and no parameters
        /// 
        ///     This creates a form of type AddressListForm as a mdi child. Calling this multiple 
        ///     times results in multiple mdi children
        ///     
        /// CreateMdiChildForm with type AddressEditForm and with uniqueFormId = "AddrId=42" and with the 2 parameters ("AddrId", 42) and ("Junk", "n/a")).
        /// 
        ///     This creates a form of type AddressEditForm as a mdi child. Calling this multiple 
        ///     times does not result in multiple mdi children because you would call 
        ///     CreateMdiChildForm with different uniqueFormId values. Instead the mdi child is 
        ///     only created once for each uniqueFormId and when CreateMdiChildForm is called 
        ///     again with the same uniqueFormId, the already created mdi child with this 
        ///     uniqueFormId is pulled to the front and made the active window
        ///     
        /// </summary>
        /// <typeparam name="T">The type of the form to be created as a mdi child form</typeparam>
        /// <param name="uniqueFormId">(optional)A unique value for the form to be created.
        /// If uniqueFormId is not specified each call to CreateMdiChildForm creates a new 
        /// mdi child form. If uniqueFormId is specified, this mdi child form can only exist once.
        /// Subsequent calls with the same uniqueFormId simply pull the earlier create mdi child 
        /// form to the from and actiuvates it
        /// </param>
        /// <param name="parameters">(otional)You can pass parameters to the mdi chidl form. 
        /// The parameters ar Tuples of type (string, object), similar to Dictionary's KeyValuePair.
        /// Name is the name of teh parameter and Value is it's value. 
        /// See also: BaseMdiChildForm.GetParameter which retrieves a typed parameter value
        /// </param>
        /// <returns>The newly created mdi child form or the form that was brought to the front</returns>
        public BaseMdiChildForm CreateMdiChildForm<T>(string uniqueFormId = null, params (string Name, object Value)[] parameters)
        {
            if (uniqueFormId == UNIQUE_FORM_NAME)
                uniqueFormId = Name;

            var child = FindChildForm<T>(uniqueFormId);
            if (child != null)
            {
                child.Activate();
                child.BringToFront();
                return child;
            }

            child = Activator.CreateInstance<T>() as BaseMdiChildForm;
            child.MdiParent = this;
            child.UniqueFormId = uniqueFormId;
            foreach (var param in parameters)
                child.Parameters.Add(param.Name, param.Value);
            child.Show();
            return child;
        }

        /// <summary>
        /// Loops over mdi child forms to find one that is of the same type.
        /// If it finds one it returns the form.
        /// If uniqueFormId is specified, the found form must also have the 
        /// same uniqueFormId.
        /// </summary>
        /// <typeparam name="T">The type of the form</typeparam>
        /// <param name="uniqueFormId">(optional) the unique identifier for a specific form</param>
        /// <returns></returns>
        private BaseMdiChildForm FindChildForm<T>(string uniqueFormId)
        {
            foreach (var mdiChild in MdiChildren)
            {
                var form = mdiChild as BaseMdiChildForm;
                if (form.GetType() == typeof(T) && !string.IsNullOrEmpty(uniqueFormId) && form.UniqueFormId.SameText(uniqueFormId))
                    return form;
            }

            return null;
        }

        /// <summary>
        /// This is the universal method to create navigation bar items
        /// </summary>
        /// <param name="parent">(otional) If the item is to be created as a child, specify its parent here. If its going to be 
        /// top level item, specify null.</param>
        /// <param name="text">(required) The text of the item</param>
        /// <param name="imageSource">(optional) Specify the item's image here. If you don't, NavBarGroupImage is used. 
        /// If the image is located in the NavBarImageFolder you can omit the path name</param>
        /// <param name="executionHandler"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        protected NavigationBarItem AddNavItem(NavigationBarItem parent, string text, string imageSource, Action<NavigationBarItem> executionHandler)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException($"'{nameof(text)}' cannot be null or empty.", nameof(text));

            if (string.IsNullOrEmpty(imageSource))
                imageSource = NavBarGroupImage;
            if (!imageSource.Contains("\\"))
                imageSource = Path.Combine(NavBarImageFolder, imageSource);

            var navItem = new NavigationBarItem
            {
                AllowHtml = true,
                Icon = imageSource,
                Text = text,
            };

            if (executionHandler != null)
                navItem.Tag = executionHandler;

            if (parent == null)
                NavBar.Items.Add(navItem);
            else
                parent.Items.Add(navItem);

            return navItem;
        }

        /// <summary>
        /// This event handler is hooked up to the NavigationBar ItemClick event and fires when an item in the NavigationBar is clicked.
        /// </summary>
        /// <param name="sender">The navigation bar</param>
        /// <param name="e">The args contain the clicked item: e.Item</param>
        private void NavBar_ItemClick(object sender, NavigationBarItemClickEventArgs e)
        {
            var item = e.Item;
            if (item != null && item.Tag is Action<NavigationBarItem> action)
                action(item);
        }
    }
}
