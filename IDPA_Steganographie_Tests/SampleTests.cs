using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IDPA_Steganographie_Tests
{
    [TestClass] // Marks this class as a test class for MSTest
    public class SampleTests
    {
        [TestMethod] // Marks this method as a test method
        public void Test_Addition()
        {
            // Arrange
            int a = 2;
            int b = 3;

            // Act
            int result = a + b;

            // Assert
            Assert.AreEqual(5, result, "Addition result is not correct!");
        }

        [TestMethod]
        public void Test_Subtraction()
        {
            // Arrange
            int a = 5;
            int b = 3;

            // Act
            int result = a - b;

            // Assert
            Assert.AreEqual(2, result, "Subtraction result is not correct!");
        }
    }
}