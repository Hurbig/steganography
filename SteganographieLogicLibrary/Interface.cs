/*--------------------------------------------------------------------//
 * Project: IDPA Steganographie Logic
 * FileName: Interface.cs
 * 
 * This provides the interface to call the functions from the outside
 * --------------------------------------------------------------------*/

using System.Drawing;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using Steganographie.Logic.Helper;
using Steganographie.Logic.Image;
using Steganographie.Logic.Logic;

namespace Steganographie.Logic
{
    public static class Interface
    {
        /// <summary>
        /// Encrypts a text into a BitmapImage
        /// </summary>
        /// <param name="picture">The picture the text will be encrypted into</param>
        /// <param name="usage">How many bits will be used in each channel</param>
        /// <param name="seed">The seed used in the generator for the order of pixels</param>
        /// <param name="text">Text that should be inserted</param>
        /// <returns>BitmapImage with slightly changed pixel colors</returns>
        public static BitmapImage Encrypt(BitmapImage picture, ChannelUsage usage, int seed, string text)
        {
            return (BitmapImage) Encrypt(picture.ToWinFormsBitmap(),usage,seed,text).ToWpfBitmap();
        }
        
        public static Bitmap Encrypt(Bitmap picture, ChannelUsage usage, int seed, string text)
        {
            using (var encrypter = new Encrypter(new Picture(picture, seed)))
            {
                encrypter.InsertText(text, usage);
                return encrypter.GetPictureData().GetImage();
            }
        }

        /// <summary>
        /// Decrypts a text from a BitmapImage
        /// </summary>
        /// <param name="picture">The picture in which a text will be searched for</param>
        /// <param name="usage">How many bits will be used in each channel</param>
        /// <param name="seed">The seed used in the generator for the order of pixels</param>
        /// <returns>Text found in the picture</returns>
        public static string Decrypt(BitmapImage picture, ChannelUsage usage, int seed)
        {
            using (var decrypter = new Decrypter(new Picture(picture.ToByteArray(), seed, picture.PixelHeight, picture.PixelWidth)))
            {
                var text = decrypter.ReadText(usage);
                return (text == "") ? "No text found." : text;
            }
        }

        public static string Decrypt(Bitmap encryptedPicture, ChannelUsage usage, int seed)
        {
            using (encryptedPicture)
            using (var decrypter = new Decrypter(new Picture(encryptedPicture, seed)))
            {
                var text = decrypter.ReadText(usage);
                return (text == "") ? "No text found." : text;
            }
        }

        public static int GetMaxCharCount(int width, int height, ChannelUsage usage)
        {
            const int startAndEndLength = 12;
            var pixCount = height * width;
            var channelUsage = usage.GetTotaleUsage();
            return pixCount * channelUsage - startAndEndLength;
        }

    }
}
