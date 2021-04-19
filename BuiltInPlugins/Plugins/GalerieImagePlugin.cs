using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PluginInterface.Attributes;
using PluginInterface.Classes;
using PluginInterface.PluginInterfaces;

namespace BuiltInPlugins.Plugins
{
    class GalerieImagePlugin : IGaleriePlugin
    {
        public string Name { get; } = "Galerie Image Plugin";
        public string Description { get; } = "Shows the image as a thumbnail in the plugin bar.";
        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public void GalerieOpened()
        {
        }

        public PluginReturnValue SelectedImageChanged(string path)
        {
            var source = new BitmapImage(new Uri(path));
            return new PluginReturnValue(source, ReturnTypes.Image);
        }
    }
}
