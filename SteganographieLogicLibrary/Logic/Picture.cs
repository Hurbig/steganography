using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Steganographie.Logic.Image;

namespace Steganographie.Logic.Logic
{
    internal class Picture : IDisposable
    {
        private Bitmap _image;
        private BitmapData _imageData;
        private byte[] _bytes;
        private int[] _order;
        private readonly IntPtr _scan0;

        public int Width;
        public int Height;

        public int Size => Width * Height;

        internal Picture(int sizeX, int sizeY, int seed = 0) : this(new Bitmap(sizeX, sizeY),seed) { }

        internal Picture(Bitmap image, int seed= 0)
        {
            _image = image;
            Width = _image.Width;
            Height = _image.Height;

            _imageData = _image.LockBits(
                new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            _bytes = new byte[Math.Abs(_imageData.Stride) * Height];
            _scan0 = _imageData.Scan0;
            Marshal.Copy(_scan0, _bytes, 0, _bytes.Length);

            CreateOrderedList();
            if (seed != 0)
                ShuffleOrder(seed);
        }

        internal Picture(byte[] bytes, int seed, int width, int height)
        {
            _bytes = bytes;
            Width = width;
            Height = height;

            CreateOrderedList();
            if (seed != 0)
                ShuffleOrder(seed);
        }

        internal byte[] GetRawData()
        {
            return _bytes;
        }

        internal Bitmap GetImage()
        {
            if (_image == null)
            {
                _image = new Bitmap(Width,Height);
                _imageData = _image.LockBits(
                    new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            }

            Marshal.Copy(_bytes, 0, _scan0, _bytes.Length);
            _image.UnlockBits(_imageData);
            return _image;
        }

        internal byte GetByte(int position)
        {
            return _bytes[_order[position]];
        }

        internal byte GetPixelChannel(int pixel, Channel channel)
        {
            return _bytes[_order[pixel] * 4 + (int) channel];
        }

        internal void SetByte(int position, byte data)
        {
            _bytes[_order[position]] = data;
        }

        internal void SetPixelChannel(int pixel, Channel channel, byte data)
        {
            _bytes[_order[pixel] * 4 + (int) channel] = data;
        }

        #region Order
        private void CreateOrderedList()
        {
            _order = new int[_bytes.Length / 4];
            for (var i = 0; i < _order.Length; i++)
                _order[i] = i;
        }

        //Pixel as well as their channels will be mixed up
        private void ShuffleOrder(int seed)
        {
            var rnd = new Random(seed);
            var n = _order.Length;
            while (n > 1)
            {
                n--;
                var k = rnd.Next(n + 1);
                var value = _order[k];
                _order[k] = _order[n];
                _order[n] = value;
            }
        }
        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            _image.Dispose();
            _bytes = null;
            _order = null;
        }

        #endregion
    }
}
