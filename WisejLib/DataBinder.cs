using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Wisej.Web;

namespace WisejLib
{
    /// <summary>
    /// I dislike the BindingSource because I find it very tedious to select the bound field from the drop 
    /// down dialog each time I add an entry field. For lists of data in conjunction with a DataGridView 
    /// it comes pretty handy but for windows with many entry fields the BindingSource can become a pain to use.
    /// A 2nd argument to not user BindingSource was that I was never able to make it work with DateTimePicker 
    /// controls that are bound to a nullable DateTime property.The code that is generated in the forms 
    /// designer.cs file is simply wrong.
    /// This made me look into it and write my own binding class. On creation it scans all controls on a form 
    /// and finds properties of the model class that have the same name.If so, it registers the control together 
    /// with the associated property's PropertyInfo and attaches a Validating event handler to the control. 
    /// Whenever the focus leaves the bound control, the associated data class ist updated as well.
    /// Because all model classes are derived from DbEntity and DbEntity implements INotifyPropertyChanged, the 
    /// DataBinder also attaches to the PropertyChanged event and gets notified when the data changes and the 
    /// control has to be updated.
    /// </summary>
    /// <typeparam name="T">A model class that is derived from DbEntity</typeparam>
    public class DataBinder<T> where T : DbEntity
    {
        /// <summary>
        /// parentForm is the cotaining form window. It is required so that DataBinder knows where to search for controls to bind
        /// </summary>
        public DataBinder(Form parentForm)
        {
            ParentForm = parentForm ?? throw new ArgumentNullException(nameof(parentForm));
        }

        /// <summary>
        /// If you like to bind Label controls to data properties, set this to true
        /// </summary>
        public bool BindLabels { get; set; } = false;

        /// <summary>
        /// Set this property to an instance of your data record
        /// </summary>
        public T DataSource {
            get => _DataSource;
            set
            {
                if (_DataSource != null)
                    _DataSource.PropertyChanged -= Data_PropertyChanged;
                
                _DataSource = value;
                if (_DataSource == null)
                    return;

                _DataSource = value ?? throw new ArgumentNullException(nameof(value));
                DataSource.PropertyChanged += Data_PropertyChanged;
                AutoDiscover();
            }
        }
        private T _DataSource = null;

        private Form ParentForm { get; }
        private List<DataBinderItem> Bindings { get; } = new List<DataBinderItem>();
        private PropertyInfo[] Properties { get; set; } = typeof(T).GetProperties();


        // public -----------------------------------------------------------

        /// <summary>
        /// AutoDiscover is automatically executed when you set the DataSource property. It scans all 
        /// controls on parentForm and tries to find DataSource properties that match by name. If 
        /// found, the control and the DataSource properties are registered. This basically means
        /// that all controls are automagically bound to the DataSource as long as the names match
        /// </summary>
        private void AutoDiscover()
        {
            Utils.ForEachControl(ParentForm, ctrl => { Register(ctrl); });
            RefreshControls();
        }

        /// <summary>
        /// Called after AutoDiscover has finished. This method loops over all controls and refreshes
        /// their values from their assigned DataSource properties
        /// </summary>
        public void RefreshControls()
        {
            foreach (var binding in Bindings)
                Refresh(binding.BoundProperty.Name);
        }

        /// <summary>
        /// Refreshes the value of the control from its assigned DataSource property
        /// </summary>
        public void RefreshControl(Control control)
        {
            var item = Bindings.SingleOrDefault(x => x.BoundControl == control);
            item?.UpdateControl(DataSource);
        }

        /// <summary>
        /// This method loops over all controls and refreshes their assigned DataSource property
        /// </summary>
        public void RefreshData()
        {
            foreach (var binding in Bindings)
                Refresh(binding.BoundControl);
        }

        /// <summary>
        /// Resets the color of all controls to their standard theme color
        /// </summary>
        public void ColorizeControls()
        {
            ColorizeControls(Application.Theme.GetColor("window"));
        }

        /// <summary>
        /// Sets the color of all controls to a specific color. During testing you can add a call 
        /// to this method and set the color to something prominent, like pink for example. That's
        /// a good way to check which controls are bound to the DataSource and, even more importantly,
        /// which contzrols are NOT bound to any DataSource property. The method was created because
        /// it makes identifying unbound controls a breeze. It sometimes happens that I forget to set
        /// the control's Name to match the DataSource property or there's a typo in the name. In 
        /// this case the control wouldn't be bound and wouldn't display any data. To identify such 
        /// controls use this method.
        /// </summary>
        /// <param name="color">The color to set the control's background to</param>
        public void ColorizeControls(Color color)
        {
            foreach (var binding in Bindings)
                binding.BoundControl.BackColor = color;
        }

        // private -----------------------------------------------------------

        /// <summary>
        /// This method can be used to register a control manually to any of the DataSource's properties, even
        /// if the names don't match. A possible use case for this would be when you want to bind 2 controls
        /// to the same property.
        /// </summary>
        /// <param name="control">The control to bind</param>
        /// <param name="propertyName">The name of the DataSource property that the control has to be bound to</param>
        /// <param name="refreshControl">Set this to true to fill the control with data right away</param>
        /// <returns>Returns the DataBinder itself which allows a fluent interface</returns>
        public DataBinder<T> Register(Control control, string propertyName, bool refreshControl = false)
        {
            if (control is Label && !BindLabels)
                return this;

            PropertyInfo prop = Properties.SingleOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                ?? throw new ArgumentException($"Property {propertyName} is not found");

            _ = AddItem(control, prop);
            if (refreshControl)
                RefreshControl(control);

            return this;
        }

        /// <summary>
        /// This registers a control by finding a DataSource property with a matching name. If a matching property 
        /// is not found this methode doesn nothing
        /// </summary>
        /// <param name="control">The control to bind. It must have a name that matches one of the DataSource properties.
        /// Any lowercase characters at the beginning of the name are ignored. That makes control names like txtLastname 
        /// or cbIsAdmin possible</param>
        /// <returns>Returns the DataBinder itself which allows a fluent interface</returns>
        private DataBinder<T> Register(Control control)
        {
            if (control is Label && !BindLabels)
                return this;

            // Used by AutoDiscover()
            // GetPropertyInfo returns the property with a matching name or null if not found
            var prop = GetPropertyInfo(control);
            if (prop != null)
                _ = AddItem(control, prop);
            return this;
        }

        /// <summary>
        /// This private method adds the control and the PropertyInfo of the DataSource property to the internal 
        /// list of bindings.
        /// </summary>
        /// <param name="control">The control to bind</param>
        /// <param name="prop">The PropertyInfo of a DataSource property</param>
        /// <returns>Returns the bind item</returns>
        private DataBinderItem AddItem(Control control, PropertyInfo prop)
        {
            var item = new DataBinderItem(control, prop);
            Bindings.Add(item);
            control.Validating += Control_Validating;
            return item;
        }

        /// <summary>
        /// This updates all controls the are bound to a DataSource property. A property can be bound to multiple 
        /// controls
        /// </summary>
        /// <param name="propertyName">THis property is used to update the associated control</param>
        private void Refresh(string propertyName)
        {
            // A property can be bound to multiple controls
            foreach (var binding in Bindings)
            {
                if (binding.BoundProperty.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                    binding.UpdateControl(DataSource);
            }
        }

        /// <summary>
        /// This updates the DataSource property that is bound to the specified control
        /// </summary>
        /// <param name="control">This control is user to update the associated DataSource property</param>
        private void Refresh(Control control)
        {
            // update data
            // A control can only be bound to 1 property
            var binding = Bindings.Find(x => x.BoundControl.Equals(control));
            binding?.UpdateProperty(DataSource);
        }

        /// <summary>
        /// This event handler is hooked to the PropertyChanged event of DbEntity. Whenever a property of
        /// the DataSource is changed, the event fires and this handler refreshes the control with the new 
        /// data
        /// </summary>
        private void Data_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Refresh(e.PropertyName);
        }

        /// <summary>
        /// This event handler is hooked to each bound control. As soon as the control's Validating event 
        /// fires, this event handler refreshes the DataSource property, Because C# supports multi-cast
        /// events you can still add an event handler to the control's Validating event.
        /// </summary>
        private void Control_Validating(object sender, CancelEventArgs e)
        {
            Refresh(sender as Control);
        }

        /// <summary>
        /// This method retrieves the PropertyInfo of the DataSource property whose name matches the 
        /// control's name. GetCleanName is used to remove leading lowercase characters so that control names 
        /// like txtLastname or cbIsAdmin are possible
        /// </summary>
        /// <param name="control">A property that matches this control's name is retrieved</param>
        /// <returns>Returns the PropertyInfo or null if not found</returns>
        private PropertyInfo GetPropertyInfo(Control control)
        {
            string cleanName = GetCleanName(control);
            foreach (PropertyInfo prop in Properties)
            {
                if (prop.Name.Equals(cleanName, StringComparison.OrdinalIgnoreCase))
                    return prop;
            }
            return null;
        }

        /// <summary>
        /// Takes the control's name and returns all leading lowercase characters
        /// </summary>
        private string GetCleanName(Control control)
        {
            for (int i = 0; i < control.Name.Length; i++)
            {
                if (char.IsUpper(control.Name[i]))
                    return control.Name.Substring(i);
            }
            return control.Name;
        }

        // DataBinderItem -----------------------------------------------------------

        /// <summary>
        /// This is a class that holds a control and  the PropertyInfo of the DataSource property 
        /// that the control is bound to. It also provides methods to tansfer data back and forth
        /// between control and property
        /// </summary>
        private class DataBinderItem
        {
            /// <summary>
            /// Creates a new binding
            /// </summary>
            /// <param name="boundControl"></param>
            /// <param name="boundProperty"></param>
            public DataBinderItem(Control boundControl, PropertyInfo boundProperty)
            {
                BoundControl = boundControl;
                BoundProperty = boundProperty;
            }

            public Control BoundControl { get; set; }
            public PropertyInfo BoundProperty { get; set; }

            /// <summary>
            /// Takes the data instance and transfers the data value from the property to the control
            /// The actual process is kind of laborious because it deals with different control types 
            /// bound to different property types.
            /// </summary>
            public void UpdateControl(T data)
            {
                object value = BoundProperty.GetValue(data);

                switch (BoundControl)
                {
                    case TypedTextBox typedTextBox:
                        typedTextBox.Text = value != null ? Convert.ToString(value) : null;
                        break;
                    case TextBox textBox:
                        textBox.Text = value != null ? Convert.ToString(value) : null;
                        break;
                    case CheckBox checkBox:
                        if (value != null)
                            checkBox.Checked = Convert.ToBoolean(value);
                        else
                            checkBox.CheckState = CheckState.Indeterminate;
                        break;
                    case ComboBox comboBox:
                        if (comboBox.DataSource != null)
                            comboBox.SelectedValue = Convert.ToInt32(value);
                        else
                            comboBox.SelectedText = Convert.ToString(value);
                        break;
                    case DateTimePicker dateTimePicker:
                        if (value != null)
                            dateTimePicker.Value = Convert.ToDateTime(value);
                        else
                            dateTimePicker.Text = null;
                        break;
                    case NumericUpDown numericUpDown:
                        if (value != null)
                            numericUpDown.Value = Convert.ToDecimal(value);
                        else
                            numericUpDown.Text = null;
                        break;
                    default:
                        // you may want to extend this switch statement to deal with more control types as necessary
                        throw new NotSupportedException($"DataBinder does not support controls of type {BoundControl.GetType().Name}");
                }
            }

            /// <summary>
            /// Takes the data instance and transfers the data value from the control to the property.
            /// The actual process is kind of laborious because it deals with different control types 
            /// bound to different property types.
            /// </summary>
            public void UpdateProperty(T data)
            {
                switch (BoundControl)
                {
                    case TypedTextBox typedTextBox:
                        if (!string.IsNullOrEmpty(typedTextBox.Text))
                            BoundProperty.SetValue(data, typedTextBox.Text);
                        else
                            BoundProperty.SetValue(data, null);
                        break;
                    case TextBox textBox:
                        if (!string.IsNullOrEmpty(textBox.Text))
                        {
                            if (BoundProperty.PropertyType == typeof(string))
                                BoundProperty.SetValue(data, textBox.Text);
                            else if (BoundProperty.PropertyType == typeof(int) || BoundProperty.PropertyType == typeof(int?))
                            {
                                if (!int.TryParse(textBox.Text, out var value))
                                {
                                    if (BoundControl.CanFocus)
                                        BoundControl.Focus();
                                    throw new Exception("The value must be an integer number");
                                }
                                BoundProperty.SetValue(data, value);
                            }
                            else if (BoundProperty.PropertyType == typeof(decimal) || BoundProperty.PropertyType == typeof(decimal?))
                            {
                                if (!decimal.TryParse(textBox.Text, out var value))
                                {
                                    if (BoundControl.CanFocus)
                                        BoundControl.Focus();
                                    throw new Exception("The value must be a decimal number");
                                }
                                BoundProperty.SetValue(data, value);
                            }
                            else if (BoundProperty.PropertyType == typeof(double) || BoundProperty.PropertyType == typeof(double?))
                            {
                                if (!double.TryParse(textBox.Text, out var value))
                                {
                                    if (BoundControl.CanFocus)
                                        BoundControl.Focus();
                                    throw new Exception("The value must be a decimal number");
                                }
                                BoundProperty.SetValue(data, value);
                            }
                            else if (BoundProperty.PropertyType == typeof(float) || BoundProperty.PropertyType == typeof(float?))
                            {
                                if (!float.TryParse(textBox.Text, out var value))
                                {
                                    if (BoundControl.CanFocus)
                                        BoundControl.Focus();
                                    throw new Exception("The value must be a decimal number");
                                }
                                BoundProperty.SetValue(data, value);
                            }
                            else if (BoundProperty.PropertyType == typeof(DateTime) || BoundProperty.PropertyType == typeof(DateTime?))
                            {
                                if (!DateTime.TryParse(textBox.Text, out var value))
                                {
                                    if (BoundControl.CanFocus)
                                        BoundControl.Focus();
                                    throw new Exception("The value must be a date");
                                }
                                BoundProperty.SetValue(data, value);
                            }
                            else
                                throw new NotSupportedException($"DataBinder does not support controls of type {BoundControl.GetType().Name} with {BoundProperty.PropertyType.Name} properties");
                        }
                        else
                            BoundProperty.SetValue(data, null);
                        break;
                    case CheckBox checkBox:
                        switch (checkBox.CheckState)
                        {
                            case CheckState.Unchecked:
                                BoundProperty.SetValue(data, false);
                                break;
                            case CheckState.Checked:
                                BoundProperty.SetValue(data, true);
                                break;
                            case CheckState.Indeterminate:
                                BoundProperty.SetValue(data, null);
                                break;
                        }
                        break;
                    case ComboBox comboBox:
                        if (comboBox.DataSource != null)
                            BoundProperty.SetValue(data, comboBox.SelectedValue);
                        else
                            BoundProperty.SetValue(data, comboBox.Text);
                        break;
                    case DateTimePicker dateTimePicker:
                        if (!string.IsNullOrEmpty(dateTimePicker.Text))
                            BoundProperty.SetValue(data, dateTimePicker.Value);
                        else
                            BoundProperty.SetValue(data, null);
                        break;
                    case NumericUpDown numericUpDown:
                        if (!string.IsNullOrEmpty(numericUpDown.Text))
                            BoundProperty.SetValue(data, numericUpDown.Value);
                        else
                            BoundProperty.SetValue(data, null);
                        break;
                    default:
                        // you may want to extend this switch statement to deal with more control types as necessary
                        throw new NotSupportedException($"DataBinder does not support controls of type {BoundControl.GetType().Name}");
                }
            }

        }
    }
}
