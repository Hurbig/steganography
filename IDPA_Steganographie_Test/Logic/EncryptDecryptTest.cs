using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Steganographie.Logic.Image;
using Steganographie.Logic.Logic;
using Steganographie.Tests.Helper;

namespace Steganographie.Tests.Logic
{
    [TestClass]
    public class EncryptDecryptTest
    {
        [TestMethod]
        public void TestShort()
        {
            const string text = "Sample Text to hide";
            var usage = new ChannelUsage(2);
            var encrypter = new Encrypter(new Picture(ColoredBitmap.CreateBitmap(10, 10, Color.Aqua), 0));
            encrypter.InsertText(text, usage);
            var picture = encrypter.GetPictureData();

            var decrypter = new Decrypter(picture);
            var res = decrypter.ReadText(usage);

            Assert.AreEqual(text, res);
        }

        [TestMethod]
        public void TestLong()
        {
            const string text = "Pretty long text just to test if everything is working properly!";
            var usage = new ChannelUsage(2);
            var encrypter = new Encrypter(new Picture(ColoredBitmap.CreateBitmap(20, 20, Color.Aqua), 0));
            encrypter.InsertText(text, usage);
            var picture = encrypter.GetPictureData();

            var decrypter = new Decrypter(picture);
            var res = decrypter.ReadText(usage);

            Assert.AreEqual(text, res);
        }

        [TestMethod]
        public void TestSeed()
        {
            const int seed = 23;
            const string text = "Sample Text to hide";
            var usage = new ChannelUsage(2);
            var encrypter = new Encrypter(new Picture(new Bitmap(20, 20), seed));
            encrypter.InsertText(text, usage);
            var picture = encrypter.GetPictureData();

            var decrypter = new Decrypter(picture);
            var res = decrypter.ReadText(usage);

            Assert.AreEqual(text, res);
        }

        [TestMethod]
        public void TestWrongSeed()
        {
            const int seed = 26;
            const string text = "This won't work because we use another seed for decrypting our image";
            var usage = new ChannelUsage(2);
            var encrypter = new Encrypter(new Picture(new Bitmap(20, 20), seed));
            encrypter.InsertText(text, usage);
            var picture = encrypter.GetPictureData();

            var decrypter = new Decrypter(new Picture(picture.GetImage(), 0));
            var res = decrypter.ReadText(usage);

            Assert.AreNotEqual(text, res);
        }

        [TestMethod]
        public void TestChnUsage()
        {
            const string text = "Sample Text written with 3 bits per channel which gives the possibility to store more text";
            var usage = new ChannelUsage(3);
            var encrypter = new Encrypter(new Picture(new Bitmap(20, 20)));
            encrypter.InsertText(text, usage);
            var picture = encrypter.GetPictureData();

            var decrypter = new Decrypter(picture);
            var res = decrypter.ReadText(usage);

            Assert.AreEqual(text, res);
        }

        [TestMethod]
        public void TestNoAlpha()
        {
            const string text = "Sample Text written with 2 bits per channel, but not using the alpha channel";
            var usage = new ChannelUsage(2, 2, 2);
            var encrypter = new Encrypter(new Picture(new Bitmap(20, 20)));
            encrypter.InsertText(text, usage);
            var picture = encrypter.GetPictureData();

            var decrypter = new Decrypter(picture);
            var res = decrypter.ReadText(usage);

            Assert.AreEqual(text, res);
        }

        [TestMethod]
        public void TestDifferentChnUsage()
        {
            const string text = "Sample Text written with different bits used per channel, but not using the alpha channel";
            var usage = new ChannelUsage(2, 3, 0, 1);
            var encrypter = new Encrypter(new Picture(100, 100));
            encrypter.InsertText(text, usage);
            var picture = encrypter.GetPictureData();

            var decrypter = new Decrypter(picture);
            var res = decrypter.ReadText(usage);

            Assert.AreEqual(text, res);
        }

        [TestMethod]
        public void TestWrongChnUsage()
        {
            const string text = "Sample Text written with 2 bits per channel, but not using the alpha channel while encrypting but using it when decrypting";
            var usage = new ChannelUsage(2, 3, 3, 2);
            var encrypter = new Encrypter(new Picture(100, 100));
            encrypter.InsertText(text, usage);
            var picture = encrypter.GetPictureData();

            var decrypter = new Decrypter(picture);
            usage = new ChannelUsage(3, 2, 2, 3);
            var res = decrypter.ReadText(usage);

            Assert.AreNotEqual(text, res);
        }
    }
}
