using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IDPA_Steganographie.HelperClasses
{
    class WPFResourceHelper
    {
        public static ImageSource ImageFromURL(string assembly, string absolutepath)
        {
            var uri = new Uri("pack://application:,,,/" + assembly + ";component/" + absolutepath);
            return new BitmapImage(uri);
        }

        public static ImageSource ImageFromURL(Object obj, string absolutepath)
        {
            return ImageFromURL(obj.GetType().Assembly.GetName().Name, absolutepath);
        }

        public static bool ExtractFileFromResource(Object obj, string absoluteresourcepath, string pathToFile)
        {
            try
            {
                Assembly ass = obj.GetType().Assembly;
                string[] names = ass.GetManifestResourceNames();
                string spath = absoluteresourcepath.Replace("\\", ".").ToUpper();
                string path = names.SingleOrDefault(t => t.ToUpper().EndsWith(spath));
                Stream str = ass.GetManifestResourceStream(path);
                var output = new FileStream(pathToFile, FileMode.OpenOrCreate, FileAccess.Write);
                var buffer = new byte[32768];
                int read;
                while ((read = str.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, read);
                }
                output.Close();
                str.Close();
                return true;
            }
            catch (Exception)
            {
                Contract.Assert(false, "Resource file not found");
            }
            return false;
        }
    }
}
