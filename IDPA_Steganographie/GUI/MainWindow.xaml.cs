using System.Windows;
using System.Windows.Controls;
using IDPA_Steganographie.HelperClasses;
using IDPA_Steganographie.Interfaces;

namespace IDPA_Steganographie.GUI
{
    /// <summary>
    /// Start window of the application, just references to the main application-GUI
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentHolder.Content = new MainScreen.MainScreen();
        }
    }
}
