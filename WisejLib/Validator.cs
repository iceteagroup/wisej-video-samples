using System;
using Wisej.Web;

namespace WisejLib
{
    /// <summary>
    /// Validator is a helper class for checking what the user entered into form fields.
    /// If a condition is not met the Validator throws an exception and puts the focus on the 
    /// offending control (if possible).
    /// Of course I do know that there are other ways to validate input but hey, this is one 
    /// way to do it and it worked well for me so far :-)
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Checks if a condition is true
        /// </summary>
        /// <param name="trueCondition">If false, an exception is thrown</param>
        /// <param name="control">Control that shoud receive the focus (can be null)</param>
        /// <param name="errorText">ErrorText to display. If null, a defualt message is used</param>
        public static void CheckTrue(bool trueCondition, Control control, string errorText)
        {
            if (!trueCondition)
                InternalCheck(false, control, errorText);
        }

        /// <summary>
        /// Checks if a condition is false
        /// </summary>
        /// <param name="falseCondition">If true, an exception is thrown</param>
        /// <param name="control">Control that shoud receive the focus (can be null)</param>
        /// <param name="errorText">ErrorText to display. If null, a defualt message is used</param>
        public static void CheckFalse(bool falseCondition, Control control, string errorText)
        {
            if (!falseCondition)
                InternalCheck(false, control, errorText);
        }


        /// <summary>
        /// Throws an exception if the given string is null or empty
        /// </summary>
        /// <param name="value">String value to check</param>
        /// <param name="errorText">ErrorText to display. If null, a defualt message is used</param>
        public static void CheckNotEmpty(string value, string errorText)
        {
            InternalCheck(!string.IsNullOrWhiteSpace(value), null, errorText);
        }

        public static void CheckEmpty(string value, string errorText)
        {
            InternalCheck(string.IsNullOrWhiteSpace(value), null, errorText);
        }

        /// <summary>
        /// Throws an exception if the Text property of the given control is null or empty.
        /// If error, the control receives the focus
        /// </summary>
        /// <param name="control">Control whose text is checked. The control receives the focus on error</param>
        /// <param name="label">The labels text is inserted into the error message</param>
        public static void CheckNotEmpty(Control control, Label label)
        {
            InternalCheck(!string.IsNullOrEmpty(GetControlText(control)), control, $"Field '{label.Text}' cannot be empty");
        }

        public static void CheckEmpty(Control control, Label label)
        {
            InternalCheck(string.IsNullOrEmpty(GetControlText(control)), control, $"Field '{label.Text}' must be empty");
        }

        /// <summary>
        /// Throws an exception if the Text property of the given control is null or empty.
        /// If error, the control receives the focus
        /// </summary>
        /// <param name="control">Control whose text is checked. The control receives the focus on error</param>
        /// <param name="errorText">The plain errortext</param>
        public static void CheckNotEmpty(Control control, string errorText)
        {
            InternalCheck(!string.IsNullOrEmpty(GetControlText(control)), control, errorText);
        }

        public static void CheckEmpty(Control control, string errorText)
        {
            InternalCheck(string.IsNullOrEmpty(GetControlText(control)), control, errorText);
        }

        /// <summary>
        /// Throws an exception if the control's Text property is not a valid phone number
        /// </summary>
        /// <param name="control">The control to check and that receives the focus on error</param>
        /// <param name="label">The label whose text goes into the error message</param>
        public static void CheckPhoneNumber(Control control, Label label)
        {
            CheckPhoneNumber(control, $"{label.Text} is not a valid phone number");
        }

        /// <summary>
        /// Throws an exception if the control's Text property is not a valid phone number
        /// </summary>
        /// <param name="control">The control to check and that receives the focus on error</param>
        /// <param name="errorText">The plain errortext</param>
        public static void CheckPhoneNumber(Control control, string errorText = "Invalid phone number")
        {
            string value = GetControlText(control);
            InternalCheck(string.IsNullOrEmpty(value) || value.IsValidPhoneNumber(), control, errorText);
        }

        /// <summary>
        /// Throws an exception if the control's Text property is not a valid url
        /// </summary>
        /// <param name="control">The control to check and that receives the focus on error</param>
        /// <param name="label">The label whose text goes into the error message</param>
        public static void CheckUrl(Control control, Label label)
        {
            CheckUrl(control, $"{label.Text} is not a valid internet address");
        }

        /// <summary>
        /// Throws an exception if the control's Text property is not a valid url
        /// </summary>
        /// <param name="control">The control to check and that receives the focus on error</param>
        /// <param name="errorText">The plain errortext</param>
        public static void CheckUrl(Control control, string errorText = "Invalid internet address")
        {
            string value = GetControlText(control);
            InternalCheck(string.IsNullOrEmpty(value) || value.IsValidUrl(), control, errorText);
        }

        /// <summary>
        /// Throws an exception if the control's Text property is not a valid IBA
        /// </summary>
        /// <param name="control">The control to check and that receives the focus on error</param>
        /// <param name="label">The label whose text goes into the error message</param>
        public static void CheckIBAN(Control control, Label label)
        {
            CheckIBAN(control, $"{label.Text} is not a valid IBAN");
        }

        /// <summary>
        /// Throws an exception if the control's Text property is not a valid IBAN
        /// </summary>
        /// <param name="control">The control to check and that receives the focus on error</param>
        /// <param name="errorText">The plain errortext</param>
        public static void CheckIBAN(Control control, string errorText = "Invalid IBAN")
        {
            string value = GetControlText(control);
            InternalCheck(string.IsNullOrEmpty(value) || value.IsValidIBAN(), control, errorText);
        }

        /// <summary>
        /// returns the Text property of a control
        /// </summary>
        /// <param name="control">Must be TextBox, ComboBox or DateTimePicker</param>
        private static string GetControlText(Control control)
        {
            switch (control)
            {
                case TextBox textBox:
                    return string.IsNullOrEmpty(textBox.Text) ? string.Empty : textBox.Text;
                case ComboBox comboBox:
                    return string.IsNullOrEmpty(comboBox.Text) ? string.Empty : comboBox.Text;
                case DateTimePicker dateTimePicker:
                    return string.IsNullOrEmpty(dateTimePicker.Text) ? string.Empty : dateTimePicker.Text;
                default:
                    throw new Exception($"Cannot validate controls of type {control.GetType().Name}");
            }
        }

        /// <summary>
        /// This is called by the public Check* methods.
        /// </summary>
        /// <param name="okCondition">If false an exception is thrown. If true, nothing happens</param>
        /// <param name="control">The control that receives the focus when error. Can be null</param>
        /// <param name="errorText">The text for the exception. If null or empty a default text is used</param>
        /// <exception cref="Exception">Throws an Exception with the header "Eingabefehler"</exception>
        private static void InternalCheck(bool okCondition, Control control, string errorText)
        {
            if (okCondition)
                return;
            if (control != null && control.CanFocus)
                control.Focus();
            if (string.IsNullOrEmpty(errorText))
                throw new Exception($"Unexpected errorwhile validating {control.Name}");
            throw new Exception($"Failed|{errorText}");
        }
    }
}
