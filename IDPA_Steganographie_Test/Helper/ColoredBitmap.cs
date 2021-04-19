using System.Drawing;

namespace Steganographie.Tests.Helper
{
    internal static class ColoredBitmap
    {
        internal static Bitmap CreateBitmap(int sizeX, int sizeY)
        {
            return CreateBitmap(sizeX,sizeY,Color.BlueViolet);
        }

        internal static Bitmap CreateBitmap(int sizeX, int sizeY, Color color)
        {
            var image = new Bitmap(sizeX,sizeY);
            using (var graph = Graphics.FromImage(image))
            {
                var rect = new Rectangle(0,0,sizeX,sizeY);
                graph.FillRectangle(new SolidBrush(color), rect);
            }
            return image;
        }
    }
}
