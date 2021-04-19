using System.Linq;
using System.Windows.Controls;
using IDPA_Steganographie.GUI.Controls;
using IDPA_Steganographie.GUI.MenuBar;
using IDPA_Steganographie.GUI.Settings;
using IDPA_Steganographie.HelperClasses;
using IDPA_Steganographie.HelperClasses.ErrorHelper;
using IDPA_Steganographie.Interfaces;
using Steganographie_Errors;

namespace IDPA_Steganographie.GUI.MainScreen
{
    /// <summary>
    /// Interaction logic for MainScreen.xaml
    /// </summary>
    public partial class MainScreen : IApplicationNavigator
    {
        public MainScreen()
        {
            InitializeComponent();
            _CreateMenuItems();
            RuntimeGlobals.Navigator = this;
            NavigateTo(new InfoPage());
        }

        /// <summary>
        /// Create menu items for the main menu
        /// </summary>
        private void _CreateMenuItems()
        {
            //--- Iconset at: http://www.iconarchive.com/show/farm-fresh-icons-by-fatcow.1.html for more icons 
            //--- Link to // http://www.doublejdesign.co.uk/, http://www.fatcow.com/free-icons // is required, TODO: Add link to the application
            var encryptUi = new EncryptUi.EncryptUi();
            var decryptUi = new DecryptUi.DecryptUi();
            MenuBar.AddMenuItem(new MenuItemControl("Info", () => RuntimeGlobals.Navigator.NavigateTo(new InfoPage()), WPFResourceHelper.ImageFromURL("IDPA_Steganographie", "Files\\Icons\\information.png")));
            MenuBar.AddMenuItem(new MenuItemControl("Encrypt", () => RuntimeGlobals.Navigator.NavigateTo(encryptUi), WPFResourceHelper.ImageFromURL("IDPA_Steganographie", "Files\\Icons\\encrypt.ico")));
            MenuBar.AddMenuItem(new MenuItemControl("Decrypt", () => RuntimeGlobals.Navigator.NavigateTo(decryptUi), WPFResourceHelper.ImageFromURL("IDPA_Steganographie", "Files\\Icons\\decrypt.ico")));
            MenuBar.AddMenuItem(new MenuItemControl("Settings", () => RuntimeGlobals.Navigator.NavigateTo(new SettingsPage()), WPFResourceHelper.ImageFromURL("IDPA_Steganographie", "Files\\Icons\\wrench.ico")));
            MenuBar.SelectFirst();
        }

        /// <summary>
        /// Navigates to a specific page
        /// </summary>
        /// <param name="content">The page that it navigates to</param>
        public void NavigateTo(UserControl content)
        {
            if (content != null)
                NavigatorFrame.Navigate(content);
            else
                ErrorHelper.Add(1);
        }

        /// <summary>
        /// Navigates back to the homescreen
        /// </summary>
        public void NavigateHome()
        {
            NavigateTo(this);
        }
    }
}
