using System.Windows;
using System.Windows.Controls;

namespace IDPA_Steganographie.HelperClasses.WaitDialogHelper
{
    /// <summary>
    /// Shows a progress ring while the application is doing something in the background
    /// </summary>
    public partial class WaitDialog : UserControl
    {
        public WaitDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Adds the wait dialog helper on the top of the GUI
        /// </summary>
        /// <param name="infotext">Text that is shown while the progressring is visible</param>
        public void Show(string infotext)
        {
            Window wnd = Application.Current.MainWindow;
            if (wnd != null)
            {
                Grid g = null;
                if (wnd.Content == null)
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
                if (infotext != " ...")
                {
                    _ScreenText.Text = infotext;
                    _ScreenText.Visibility = Visibility.Visible;
                }

                g.Children.Add(this);
            }
        }

        /// <summary>
        /// Removes the progressring from the GUI
        /// </summary>
        public void Close()
        {
            _RemoveFromParent();
            _ScreenText.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Removes the progressring from the GUI
        /// </summary>
        private void _RemoveFromParent()
        {
            var p = Parent as Grid;
            if (p != null)
                p.Children.Remove(this);
        }
    }
}
