using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using PluginInterface.Classes;

namespace PluginInterface.PluginInterfaces
{
    public interface IGaleriePlugin : IStegoPlugin
    {
        void GalerieOpened();
        PluginReturnValue SelectedImageChanged(string path);
    }
}
