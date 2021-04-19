using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Steganographie_Errors;

namespace IDPA_Steganographie.HelperClasses.ErrorHelper
{
    public class ErrorHelper
    {
        /// <summary>
        /// Adds an error to the logfile and displays the error in the application
        /// </summary>
        /// <param name="error">Error holds all the error information</param>
        /// <param name="okAction">Specific action when ok is clicked. Default: Close the errorwindow and continue with the application</param>
        /// <param name="continueAction">Specific action when continue is clicked. Default: Button disabled</param>
        public static void Add(Error error, Action okAction = null, Action continueAction = null)
        {
            var errorControl = string.IsNullOrEmpty(error.ErrorMessage) ? 
                new ErrorMessageBox(error.Title, error.ErrorDescription, error.ErrorSolution, okAction, continueAction) :
                new ErrorMessageBox(error.Title, error.ErrorDescription, error.ErrorSolution, error.ErrorMessage, okAction, continueAction);
            _Show(errorControl);
            _WriteLog(error);
        }

        private static void _WriteLog(Error error)
        {
            //Todo Logger
        }

        public static void Add(int errornumber)
        {
            Add(ErrorCollection.GetError(errornumber));
        }

        internal static void Add(int errornumber, string exeptionmessage, string stacktrace)
        {
            var error = ErrorCollection.GetError(errornumber);
            if(error.IsErrorValid)
            {
                error.ErrorMessage = exeptionmessage;
                error.StackTrace = stacktrace;
            }
                
            Add(error);
        }

        /// <summary>
        /// Draws the errorwindow to the application
        /// </summary>
        /// <param name="error">Errorwindow that will be shown</param>
        private static void _Show(ErrorMessageBox error)
        {
            Window wnd = Application.Current.MainWindow; //Get current application mainwindow
            if (wnd == null)
                return;
            Grid g = null;
            if (wnd.Content == null) //Create a new control to show the errorwindow 
            {
                g = new Grid();
                wnd.Content = g;
            }
            else
            {
                if (wnd.Content is Grid)
                    g = (Grid)wnd.Content;
                else
                {
                    g = new Grid();
                    var c = wnd.Content as UIElement;
                    wnd.Content = null;
                    g.Children.Add(c);
                    wnd.Content = g;
                }
            }

            if (g.Children.OfType<ErrorMessageBox>().Any())
                return;
            g.Children.Add(error); //Add the errorwindow to the application mainwindow
        }
    }
}
