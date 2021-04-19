using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PluginInterface.Attributes;
using PluginInterface.Classes;
using PluginInterface.PluginInterfaces;

namespace BuiltInPlugins.Plugins
{
    class GaleriePlugin : IGaleriePlugin
    {
        public string Name { get; } = "Galerie Plugin";
        public string Description { get; } = "Adds extended information about images like resolution or size to the galerie";

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public void GalerieOpened()
        {
        }

        [ReturnType(ReturnTypes.List)]
        public PluginReturnValue SelectedImageChanged(string path)
        {
            BitmapMetadata meta;
            using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BitmapSource img = BitmapFrame.Create(fs);
                BitmapMetadata md = (BitmapMetadata) img.Metadata;
                meta = md;
            }
            if(meta != null)
            {
                return new PluginReturnValue(new List<string>()
                                             {
                                                 meta.Format,
                                                 meta.DateTaken,
                                                 path
                                             }, ReturnTypes.List);
            }

            return null;
        }
    }
}
