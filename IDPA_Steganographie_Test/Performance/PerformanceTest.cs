using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Steganographie.Logic;
using Steganographie.Logic.Image;

namespace Steganographie.Tests.Performance
{
    [TestClass]
    public class PerformanceTests
    {
        [TestMethod]
        public void EncryptPerformance_Bait()
        {
            var text = "A bit longer text to actually be able to test the performance. More text should have an heavy impact on performance";
            var image = new Bitmap(1920, 1080);
            var time = StopwatchUtil.Time(
                () =>
                {
                    Interface.Encrypt(image, new ChannelUsage(2), 0, text);
                });

            Assert.IsTrue(time.Milliseconds <= 0, "Actual time was: " + time.Milliseconds + "ms");
        }

        [TestMethod]
        public void DecryptPerformance_Bait()
        {
            var text = "A bit longer text to actually be able to test the performance. More text should have an heavy impact on performance";
            var image = Interface.Encrypt(new Bitmap(1920, 1080), new ChannelUsage(2), 0, text);
            var time = StopwatchUtil.Time(
                () =>
                {
                    Interface.Decrypt(image, new ChannelUsage(2),0);
                });

            Assert.IsTrue(time.Milliseconds <= 0, "Actual time was: " + time.Milliseconds + "ms");
        }
    }
}
