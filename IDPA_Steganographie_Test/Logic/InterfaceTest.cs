using System.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Steganographie.Logic;
using Steganographie.Logic.Image;
using Steganographie.Logic.Logic;
using Steganographie.Tests.Helper;

namespace Steganographie.Tests.Logic
{
    [TestClass]
    public class InterfaceTest
    {
        [TestMethod]
        public void TestSingleCharTagless()
        {
            const string text = "s";
            var image = ColoredBitmap.CreateBitmap(1, 1);
            var usage = new ChannelUsage(2);

            var encrypter = new Encrypter(new Picture(image));
            encrypter.InsertText(text, usage, false);

            var resImage = encrypter.GetPictureData().GetImage();
            encrypter.Dispose();

            var decrypter = new Decrypter(new Picture(resImage));
            var resText = decrypter.ReadText(usage, false);

            Assert.AreEqual(text, resText);
        }

        [TestMethod]
        public void TestSingleChar()
        {
            const string text = "s";
            var image = ColoredBitmap.CreateBitmap(13, 1);
            var usage = new ChannelUsage(2);

            var encrypter = new Encrypter(new Picture(image));
            encrypter.InsertText(text, usage);

            var resImage = encrypter.GetPictureData().GetImage();
            encrypter.Dispose();
            var pixel = resImage.GetPixel(0, 0);

            var decrypter = new Decrypter(new Picture(resImage));
            var resText = decrypter.ReadText(usage);

            Assert.AreEqual(text, resText);
        }

        [TestMethod]
        public void TestEncrypt()
        {
            const string text = "Sample text to hide";
            var image = ColoredBitmap.CreateBitmap(100, 100);
            var usage = new ChannelUsage(2);
            var picture = Interface.Encrypt(image, usage, 0, text);

            var decrypter = new Decrypter(new Picture(picture));
            var resText = decrypter.ReadText(usage);

            Assert.AreEqual(text, resText);
        }

        [TestMethod]
        public void TestDecrypt()
        {
            const string text = "Sample text to hide";
            var image = ColoredBitmap.CreateBitmap(100, 100);
            var usage = new ChannelUsage(2);
            var picture = new Encrypter(new Picture(image));
            picture.InsertText(text, usage);

            var endImage = picture.GetPictureData().GetImage();
            endImage.Save("image.png", ImageFormat.Png);

            var resText = Interface.Decrypt(endImage, usage, 0);

            Assert.AreEqual(text, resText);
        }

        [TestMethod]
        public void TestProcess()
        {
            const string text = "Pretty long text just to test if everything is working properly!";
            var image = ColoredBitmap.CreateBitmap(100, 100);
            var usage = new ChannelUsage(2);

            var pic = Interface.Encrypt(image, usage, 0, text);
            var resText = Interface.Decrypt(pic, usage, 0);

            Assert.AreEqual(text, resText);
        }

        [TestMethod]
        public void TestProcessSeed()
        {
            const string text = "Pretty long text just to test if everything is working properly!";
            const int seed = 13;
            var image = ColoredBitmap.CreateBitmap(100, 100);
            var usage = new ChannelUsage(2);

            var pic = Interface.Encrypt(image, usage, seed, text);
            var resText = Interface.Decrypt(pic, usage, seed);

            Assert.AreEqual(text, resText);
        }

        [TestMethod]
        public void TestProcessLarge()
        {
            const string text = "Pretty long text just to test if everything is working properly! I'll therefore add some more text so the whole process takes some more time which makes it more suitable to compare the performance as well as changes to it.";
            var image = ColoredBitmap.CreateBitmap(1920, 1080);
            var usage = new ChannelUsage(2);

            var pic = Interface.Encrypt(image, usage, 0, text);
            var resText = Interface.Decrypt(pic, usage,0);

            Assert.AreEqual(text, resText);
        }

        [TestMethod]
        public void TestProcessHuge()
        {
            var text = "Pretty long text just to test if everything is working properly! I'll therefore add some more text so the whole process takes some more time which makes it more suitable to compare the performance as well as changes to it. This is a 4k picture. This could theoretically take some time but we'll manage to process this extremely fast";
            var image = ColoredBitmap.CreateBitmap(3840, 2160);
            var usage = new ChannelUsage(2);

            var pic = Interface.Encrypt(image, usage, 0, text);
            var resText = Interface.Decrypt(pic, usage, 0);

            Assert.AreEqual(text, resText);
        }
    }
}
