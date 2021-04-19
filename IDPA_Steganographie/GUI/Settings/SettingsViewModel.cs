using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDPA_Steganographie.BaseClasses;

namespace IDPA_Steganographie.GUI.Settings
{
    public class SettingsViewModel : ViewModelBase
    {
        public string SavePath
        {
            get
            {
                return Properties.Settings.Default.SavePath;
            }
            set
            {
                Properties.Settings.Default.SavePath = value;
                Properties.Settings.Default.Save();
                _OnPropertyChanged();
            }
        }

        public string OpenPath {
            get
            {
                return Properties.Settings.Default.OpenPath;
            }
            set
            {
                Properties.Settings.Default.OpenPath = value;
                Properties.Settings.Default.Save();
                _OnPropertyChanged();
            }
        }
        public int ImageFormat { get; set; }
        public string ImageSize { get; set; }
        public bool UseAlphaChannel {
            get
            {
                return Properties.Settings.Default.UseAlpha;
            }
            set
            {
                Properties.Settings.Default.UseAlpha = value;
                Properties.Settings.Default.Save();
                _OnPropertyChanged();
            }
        }
    }
}
