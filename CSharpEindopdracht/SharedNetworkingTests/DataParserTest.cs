using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharedNetworkingTests
{
    [TestClass]
    public class DataParserTest
    {
        [TestMethod]
        public void testGetJsonIdentifier_returnsBool_whereIdentifierIsCLEARLINES()
        {
            // Arrange
            byte[] testArray = SharedNetworking.Utils.DataParser.GetClearLinesMessage();
            string testIdentifier;

            // Act
            bool testResult = SharedNetworking.Utils.DataParser.getJsonIdentifier(testArray, out testIdentifier);

            // Assert
            Assert.IsTrue(testResult);
            Assert.AreEqual(SharedNetworking.Utils.DataParser.CLEAR_LINES, testIdentifier);
        }

        [TestMethod]
        public void testGetLogOnJsonMessage()
        {
            // Arrange
            byte[] testArray;
            // Act
            // Assert

        }
    }
}
