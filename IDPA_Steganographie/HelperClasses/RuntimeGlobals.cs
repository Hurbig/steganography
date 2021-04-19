using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using IDPA_Steganographie.GUI.Settings;
using IDPA_Steganographie.Interfaces;
using Microsoft.Win32;

namespace IDPA_Steganographie.HelperClasses
{
    /// <summary>
    /// Static class that provides objects that every class can use
    /// </summary>
    public static class RuntimeGlobals
    {
        static RuntimeGlobals()
        {
            Settings = new SettingsViewModel();
        }
        /// <summary>
        /// Navigate to a UserControl
        /// </summary>
        public static IApplicationNavigator Navigator { get; set; }
        public static SettingsViewModel Settings { get; set; }

        public static string ShowSaveFileDialog(string filter, string initialDirectory)
        {
            string saveFilePath = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = filter,
                InitialDirectory = initialDirectory
            };
            if (saveFileDialog.ShowDialog() == true)
                saveFilePath = saveFileDialog.FileName;
            return saveFilePath;
        }
    }
}
