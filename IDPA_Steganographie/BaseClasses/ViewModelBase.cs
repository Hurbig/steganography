using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using IDPA_Steganographie.GUI.Settings;
using IDPA_Steganographie.HelperClasses;
using IDPA_Steganographie.Interfaces;

namespace IDPA_Steganographie.BaseClasses
{
    /// <summary>
    /// Base logic class that provides several functions every logic class needs
    /// </summary>
    public abstract class ViewModelBase : IViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public SettingsViewModel Settings { get; private set; }

        protected ViewModelBase()
        {
            Settings = RuntimeGlobals.Settings;
        }
        /// <summary>
        /// Tells the GUI that a property value changed
        /// </summary>
        /// <param name="propertyName">Name of the changed property</param>
        protected virtual void _OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Tells the GUI that a property value changed
        /// </summary>
        /// <param name="propertyName">Name of the changed property</param>
        protected virtual void _OnPropertyChanged(MethodBase propertyName)
        {
            _OnPropertyChanged(propertyName.Name.Replace("set_", ""));
        }
    }
}
