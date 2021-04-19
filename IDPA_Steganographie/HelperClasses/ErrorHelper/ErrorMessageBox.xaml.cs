using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace IDPA_Steganographie.HelperClasses.ErrorHelper
{
    /// <summary>
    /// A little errorwindow that shows the information of an error.cs object
    /// </summary>
    public partial class ErrorMessageBox : UserControl
    {
        private readonly Action _OkAction;
        private readonly Action _ContinueAction;

        /// <summary>
        /// Create a new instance of ErrorMessageBox
        /// </summary>
        /// <param name="title">Meaningful title on top of the errorwindow</param>
        /// <param name="errorDescription">Describes the error a bit</param>
        /// <param name="errorSolution">Solution to fix the error</param>
        /// <param name="okAction">Specific action when ok is clicked. Default: Close the errorwindow and continue with the application</param>
        /// <param name="continueAction">Specific action when continue is clicked. Default: Button disabled</param>
        public ErrorMessageBox(string title, string errorDescription, string errorSolution, Action okAction = null, Action continueAction = null)
        {
            InitializeComponent();
            ErrorMessageExpander.Visibility = Visibility.Collapsed;
            _ErrorDescriptionBlock.Text = errorDescription;
            _TitleBlock.Text = title;
            _ErrorSolutionBlock.Text = errorSolution;
            _OkAction = okAction;
            _ContinueAction = continueAction;

            _OkButton.Click += _OkButton_Click;
            _ContinueButton.Click += _ContinueButton_Click;

            _ContinueButton.Visibility = continueAction != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public ErrorMessageBox(string title, string errorDescription, string errorSolution, string errorText, Action okAction = null, Action continueAction = null)
            : this(title, errorDescription, errorSolution, okAction, continueAction)
        {
            ErrorMessage.Text = errorText;
            ErrorMessageExpander.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Runs the continue button action if it's set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _ContinueButton_Click(object sender, RoutedEventArgs e)
        {    
            if(_ContinueAction != null)
                _ContinueAction();
            _Close();
        }

        /// <summary>
        /// Runs the ok button action if it's set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _OkButton_Click(object sender, RoutedEventArgs e)
        {
            _Close();
            if(_OkAction != null)
                _OkAction();
        }

        /// <summary>
        /// Removes the errorwindow from the application window
        /// </summary>
        private void _Close()
        {
            _RemoveFromParent();
            _OkButton.Click -= _OkButton_Click;
            _ContinueButton.Click -= _ContinueButton_Click;
        }

        /// <summary>
        /// Removes the errorwindow from the application window
        /// </summary>
        private void _RemoveFromParent()
        {
            var p = Parent as Grid;
            if (p != null)
                p.Children.Remove(this);
        }
    }
}
