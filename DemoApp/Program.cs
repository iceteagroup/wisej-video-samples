using DemoApp.Forms;
using System;
using System.Threading;
using Wisej.Web;
using WisejLib;

namespace DemoApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            // Unhandled exception display a lot of stuff nobody understands (except for the makers of C#?
            // I chose the write my on handler for unhandled exceptions but the event must be hooked up
            // before the main window is created
            Application.ThreadException += Application_ThreadException;

            MainForm window = new MainForm();
            window.Show();
        }

        /// <summary>
        /// Event handler that is invoked with every unhandled exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            // if the exception message doesn't have a title I add "Error" as a title
            var message = e.Exception.Message;
            if (message.IndexOf('|') < 0)
                message = "Error|" + message;

            // and finally display the eror message
            Utils.MsgBox(message, MessageBoxIcon.Error);
        }

        //
        // You can use the entry method below
        // to receive the parameters from the URL in the args collection.
        //
        //static void Main(NameValueCollection args)
        //{
        //}
    }
}