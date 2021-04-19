using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace IDPA_Steganographie
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            _CheckStartUp();
        }

        private void _CheckStartUp()
        {
            if(string.IsNullOrEmpty(IDPA_Steganographie.Properties.Settings.Default.SavePath))
                IDPA_Steganographie.Properties.Settings.Default.SavePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (string.IsNullOrEmpty(IDPA_Steganographie.Properties.Settings.Default.OpenPath))
                IDPA_Steganographie.Properties.Settings.Default.OpenPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }
    }
}
