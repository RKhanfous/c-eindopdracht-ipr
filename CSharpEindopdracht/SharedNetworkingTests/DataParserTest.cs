using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharedNetworkingTests
{
    [TestClass]
    public class DataParserTest
    {
        [TestMethod]
        public void testGetJsonIdentifier_returnsBool_where()
        {
            // Arrange
            byte[] testArray = new byte[9];
            for(int i = 0; i < 10; i++)
            {
                testArray[i] = 0x02;
            }

            string testIdentifier = "START";

            // Act
            bool testResult = DataParser.getJsonIdentifier(testArray, testIdentifier);

            // Assert
            Assert.AreEqual(true, testResult);
        }
    }
}
