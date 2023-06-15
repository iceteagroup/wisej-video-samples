using DemoApp.Models;
using System.Drawing;
using Wisej.Web;

namespace DemoApp.Classes
{
    /// <summary>
    /// The static class Globals holds things that are needed all over the place, for 
    /// example the current user or some fonts
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// The currently authenticated user. It is stored in the Session because 
        /// otherwise all clients would share the same CUrrentUser property
        /// </summary>
        public static DbUser CurrentUser
        {
            get => Application.Session[nameof(CurrentUser)];
            set => Application.Session[nameof(CurrentUser)] = value;
        }

        /// <summary>
        /// The standard font but in bold. It's created here in this class once so you 
        /// don't have to create it everywhere the app needs a bold font
        /// </summary>
        public static Font FONT_Bold { get; set; }

        /// <summary>
        /// The standard font but stroke out. It's created here in this class once so you 
        /// don't have to create it everywhere the app needs a bold font
        /// </summary>
        public static Font FONT_StrikeOut { get; set; }

        /// <summary>
        /// This method creates the bold and strikeout fonts. This is not done in a 
        /// static constructor because we need to pass a prototype font. So I simply
        /// defined this method and call it from MainForm
        /// </summary>
        /// <param name="prototype"></param>
        public static void InitFonts(Font prototype)
        {
            FONT_Bold = new Font(prototype, FontStyle.Bold);
            FONT_StrikeOut = new Font(prototype, FontStyle.Strikeout);
        }

    }
}