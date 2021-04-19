using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Steganographie.Logic.Helper
{
    internal static class BitmapConverter
    {
        public static byte[] ToByteArray(this BitmapSource imageSource)
        {
            var height = imageSource.PixelHeight;
            var width = imageSource.PixelWidth;
            var nStride = (width*imageSource.Format.BitsPerPixel + 7)/8;
            var pixelArray = new byte[height * nStride];
            imageSource.CopyPixels(pixelArray,nStride,0);
            
            return pixelArray;;
        }

        public static Bitmap ToWinFormsBitmap(this BitmapSource bitmapsource)
        {
            using (var stream = new MemoryStream())
            {
                BitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(stream);

                using (var tempBitmap = new Bitmap(stream))
                {
                    var pixel = tempBitmap.GetPixel(0, 0);
                    // According to MSDN, one "must keep the stream open for the lifetime of the Bitmap."
                    // So we return a copy of the new bitmap, allowing us to dispose both the bitmap and the stream.
                    return new Bitmap(tempBitmap);
                }
            }
        }

        public static BitmapSource ToWpfBitmap(this Bitmap bitmap)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);

                stream.Position = 0;
                var result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }
    }
}
