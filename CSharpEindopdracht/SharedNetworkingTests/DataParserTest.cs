using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedNetworking.Utils;
using System;
using System.Linq;

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
            string username = "username";
            string roomCode = "roomCode";

            //act
            byte[] message = DataParser.GetLogOnJsonMessage(username, roomCode);

            int expectedMessageLength = BitConverter.ToInt32(message, 0);

            byte[] payload = message.Skip(5).ToArray();

            //assert
            Assert.AreEqual(message.Length, expectedMessageLength);
            Assert.AreEqual(0x02, message[4]);
            Assert.AreEqual(username, DataParser.GetUsernameFromLogOnjson(payload));
            Assert.AreEqual(roomCode, DataParser.GetRoomCodeFromLogOnjson(payload));
        }

        [TestMethod]
        public void GoToRoomMessage()
        {
            //arrange
            string roomCode = "roomCode";
            bool running = true;

            //act
            byte[] message = DataParser.GetGoToRoomMessage(roomCode, running);

            int expectedMessageLength = BitConverter.ToInt32(message, 0);

            byte[] payload = message.Skip(5).ToArray();

            //assert
            Assert.AreEqual(message.Length, expectedMessageLength);
            Assert.AreEqual(0x02, message[4]);
            Assert.AreEqual((roomCode, running), DataParser.GetRoomDataFromGoToRoomjson(payload));
        }
    }
}
