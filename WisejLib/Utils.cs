using Newtonsoft.Json;
using System;
using System.Linq;
using Wisej.Web;

namespace WisejLib
{

    /// <summary>
    /// Static class with various helpers
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Create a IBAN from account number and BIC/SWIFT (D/A/CH only)
        /// </summary>
        /// <param name="blz">German "Bankleitzahl" aka BIC/SWIFT</param>
        /// <param name="accountNumber">THe number of the account</param>
        /// <param name="grouped">If true the resulting IBAN is grouped into pieces of 4 digits separated by a space</param>
        /// <param name="countryCode">The country code. Valid codes are D, A and CH</param>
        public static string CreateIBAN(string blz, string accountNumber, bool grouped = true, string countryCode = "D")
        {
            string bban = string.Empty;

            countryCode = countryCode.ToUpper();
            switch (countryCode)
            {
                case "D":
                    bban = blz.PadLeft(8, '0') + accountNumber.PadLeft(10, '0');
                    break;
                case "A":
                    bban = blz.PadLeft(5, '0') + accountNumber.PadLeft(11, '0');
                    break;
                case "CH":
                    bban = blz.PadLeft(5, '0') + accountNumber.PadLeft(12, '0');
                    break;
                default:
                    return string.Empty;
            }
            string sum = bban + countryCode.Aggregate("", (current, c) => current + (c - 55).ToString()) + "00";

            var d = decimal.Parse(sum);
            var checksum = 98 - (d % 97);
            string iban = countryCode + checksum.ToString().PadLeft(2, '0') + bban;
            return grouped ? iban.Select((c, i) => (i % 4 == 3) ? c + " " : c + "").Aggregate("", (current, c) => current + c) : iban;
        }

        /// <summary>
        /// Method to loop over all controls of a form recursively and perform 
        /// an action on each control
        /// </summary>
        /// <param name="parent">The parent control, usually a form. Cannot be null</param>
        /// <param name="action">The action delegate to invoke with each control</param>
        public static void ForEachControl(Control parent, Action<Control> action)
        {
            if (parent is null)
                throw new ArgumentNullException(nameof(parent));

            if (action is null)
                throw new ArgumentNullException(nameof(action));

            foreach (Control control in parent.Controls.Cast<Control>())
            {
                action(control);
                ForEachControl(control, action);
            }
        }

        /// <summary>
        /// This is just a short cut for MsgBox where the icon is always MessageBoxIcon.Question
        /// </summary>
        /// <param name="titleAndMessage">The Question to ask. It may contain a title (see MsgBox for details)</param>
        /// <returns>True for yes and false for no</returns>
        public static bool Confirm(string titleAndMessage)
        {
            return MsgBox(titleAndMessage, MessageBoxIcon.Question, MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        /// <summary>
        /// This is just a short cut for MsgBox where the icon is always MessageBoxIcon.Information and there is only a Ok button.
        /// The method doies not return a DialogResult because its purpose is just to show some info and there is no response needed.
        /// </summary>
        /// <param name="captionTitleText">This contains the MessageBox caption, a title before the 
        /// text and the text itself, all separated by a pipe symbol (|)</param>
        public static void InfoBox(string captionTitleText)
        {
            MsgBox(captionTitleText, MessageBoxIcon.Information);
        }

        /// <summary>
        /// This is a convenient shortcut for MessageBox
        /// </summary>
        /// <param name="captionTitleText">This contains the MessageBox caption, a title before the 
        /// text and the text itself, all separated by a pipe symbol (|).
        /// Example: "Error|Damn|An error occurred" makes "Error" the message box caption, 
        /// "Damn" the title in bold followed by "An error occurred". Title and Text allow HTML tags</param>
        /// <param name="icon">The icon to use. This also determines which Caption the window will have if no caption is provided in captionTitleText</param>
        /// <param name="buttons">The usual MessageBoxButtons</param>
        /// <returns>A DialogResult</returns>
        public static DialogResult MsgBox(string captionTitleText, MessageBoxIcon icon = MessageBoxIcon.None, MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            string IconToCaption(MessageBoxIcon anIcon)
            {
                switch (anIcon)
                {
                    case MessageBoxIcon.Stop:
                        return "Stop";
                    case MessageBoxIcon.Hand:
                        return "Attention";
                    case MessageBoxIcon.Error:
                        return "Error";
                    case MessageBoxIcon.Question:
                        return "Confirmation";
                    case MessageBoxIcon.Warning:
                        return "Warning";
                    case MessageBoxIcon.Information:
                        return "Information";
                    default:
                        return Application.Title;
                }
            }

            string caption, title, text;
            var parts = captionTitleText.Split('|');
            switch (parts.Length)
            {
                case 3:
                    caption = parts[0];
                    title = parts[1];
                    text = parts[2];
                    break;
                case 2:
                    caption = IconToCaption(icon);
                    title = parts[0];
                    text = parts[1];
                    break;
                case 1:
                    caption = IconToCaption(icon);
                    title = null;
                    text = parts[0];
                    break;
                default:
                    throw new ArgumentException("Too many parts in parameter captionTitleText", nameof(captionTitleText));
            }

            if (!string.IsNullOrEmpty(title))
                text = $"<p style=\"font-weight: bold; font-size: 1.25em\">{title}</p><p>{text}</p>";

            return MessageBox.Show(text, IconToCaption(icon), buttons, icon);
        }
 
        /// <summary>
        /// Helper method to display a "not implemented" message
        /// </summary>
        /// <param name = "functionality" > (optional)Specify the name of a functionality or module</param>
        public static void NotImplemented(string functionality = null)
        {
            string message = "This functionality is not implemented yet.";
            if (!string.IsNullOrEmpty(functionality))
                message += $":<br><br><b>{functionality}</b>";
            MsgBox(message, MessageBoxIcon.Hand);
        }


        /// <summary>
        /// Taken from https://stackoverflow.com/questions/78536/deep-cloning-objects
        /// Perform a deep Copy of the object, using Json as a serialization method. NOTE: Private members are not cloned using this method.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (source == null) 
                return default;

            // initialize inner objects individually
            var deserializeSettings = new JsonSerializerSettings 
            { 
                ObjectCreationHandling = ObjectCreationHandling.Replace,
            };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }
    }
}
