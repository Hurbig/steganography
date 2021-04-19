using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Steganographie.Logic.Helper
{
    /// <summary>
    /// Saves and reads a picture to or from the computer
    /// </summary>
    public static class InputOutput
    {
        /// <summary>
        /// Reads in an image
        /// </summary>
        /// <param name="filePath">Filepath to were the file is</param>
        /// <returns>Bitmap image</returns>
        public static async Task<Bitmap> ReadImageFromFileAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                Bitmap bmp;
                using (var bmpTemp = new Bitmap(filePath))
                {
                    bmp = new Bitmap(bmpTemp);
                }
                return bmp;
            });
        }

        /// <summary>
        /// Saves an image to the harddisk
        /// </summary>
        /// <param name="filePath">Filepath to were the file is</param>
        /// <param name="file">Bitmap image data</param>
        public static void SaveImageToFile(string filePath, BitmapImage file) 
        {
            _SaveImageToFile(filePath, file);
        }

        private static void _SaveImageToFile(string filePath, BitmapImage file)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(file));

            using(var fileStream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(fileStream);
                file = null;
            }
            GC.Collect();
        }

        public static bool CheckPathIsValidImage(string path)
        {
            return path.ToLower().EndsWith(".png") || path.ToLower().EndsWith(".bmp") || path.ToLower().EndsWith(".jpg") || path.ToLower().EndsWith(".jpeg");
        }
    }
}
