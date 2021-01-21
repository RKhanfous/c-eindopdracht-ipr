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
            byte[] testArray = DataParser.GetClearLinesMessage();
            string testIdentifier;

            // Act
            bool testResult = DataParser.getJsonIdentifier(testArray, out testIdentifier);

            // Assert
            Assert.IsTrue(testResult);
            Assert.AreEqual(DataParser.CLEAR_LINES, testIdentifier);
        }

        [TestMethod]
        public void testGetLogOnJsonMessage()
        {
            //arrange
            string username = "username";
            string roomCode = "roomCode";
            string testIdentifier;

            //act
            byte[] message = DataParser.GetLogOnJsonMessage(username, roomCode);

            int expectedMessageLength = BitConverter.ToInt32(message, 0);

            DataParser.getJsonIdentifier(message, out testIdentifier);

            byte[] payload = message.Skip(5).ToArray();

            //assert
            Assert.AreEqual(message.Length, expectedMessageLength);
            Assert.AreEqual(0x02, message[4]);
            Assert.AreEqual(username, DataParser.GetUsernameFromLogOnjson(payload));
            Assert.AreEqual(roomCode, DataParser.GetRoomCodeFromLogOnjson(payload));
            Assert.AreEqual(DataParser.LOG_ON, testIdentifier);
        }

        [TestMethod]
        public void testGoToRoomMessage()
        {
            //arrange
            string roomCode = "roomCode";
            bool running = true;
            string testIdentifier;

            //act
            byte[] message = DataParser.GetGoToRoomMessage(roomCode, running);

            int expectedMessageLength = BitConverter.ToInt32(message, 0);

            DataParser.getJsonIdentifier(message, out testIdentifier);

            byte[] payload = message.Skip(5).ToArray();

            //assert
            Assert.AreEqual(message.Length, expectedMessageLength);
            Assert.AreEqual(0x02, message[4]);
            Assert.AreEqual((roomCode, running), DataParser.GetRoomDataFromGoToRoomjson(payload));
            Assert.AreEqual(DataParser.GO_TO_ROOM, testIdentifier);
        }

        [TestMethod]
        public void testGetOwnDataMessage()
        {
            //arrange
            string username = "username";
            uint i = 13;
            string testIdentifier;

            //act
            byte[] message = DataParser.GetOwnDataMessage(username, i);

            int expectedMessageLength = BitConverter.ToInt32(message, 0);

            DataParser.getJsonIdentifier(message, out testIdentifier);

            byte[] testPayload = message.Skip(5).ToArray();

            //assert
            Assert.AreEqual(message.Length, expectedMessageLength);
            Assert.AreEqual(0x02, message[4]);
            Assert.AreEqual((username, i), DataParser.GetUsernameIdFromJsonMessage(testPayload));
            Assert.AreEqual(DataParser.OWN_DATA, testIdentifier);
        }

        [TestMethod]
        public void testGetStartMessage()
        {
            //arrange
            string testIdentifier;

            //act
            byte[] message = DataParser.GetStartMessage();

            DataParser.getJsonIdentifier(message, out testIdentifier);

            int expectedMessageLength = BitConverter.ToInt32(message, 0);

            //assert
            Assert.AreEqual(message.Length, expectedMessageLength);
            Assert.AreEqual(DataParser.START_GAME, testIdentifier);
        }

        [TestMethod]
        public void testGetDrawerMessage()
        {
            //arrange
            uint id = 13;
            string testIdentifier;

            //act
            byte[] message = DataParser.GetDrawerMessage(id);

            int expectedMessageLength = BitConverter.ToInt32(message, 0);

            DataParser.getJsonIdentifier(message, out testIdentifier);

            byte[] testPayload = message.Skip(5).ToArray();

            //assert
            Assert.AreEqual(message.Length, expectedMessageLength);
            Assert.AreEqual(0x02, message[4]);
            Assert.AreEqual(id, DataParser.GetDrawerId(testPayload));
            Assert.AreEqual(DataParser.SET_DRAWER, testIdentifier);
        }

        [TestMethod]
        public void testGetWordMessage()
        {
            //arrange
            string word = "word";
            string testIdentifier;

            //act
            byte[] message = DataParser.GetWordMessage(word);

            int expectedMessageLength = BitConverter.ToInt32(message, 0);

            DataParser.getJsonIdentifier(message, out testIdentifier);

            byte[] testPayload = message.Skip(5).ToArray();

            //assert
            Assert.AreEqual(message.Length, expectedMessageLength);
            Assert.AreEqual(0x02, message[4]);
            Assert.AreEqual(word, DataParser.GetWordFromjsonMessage(testPayload));
            Assert.AreEqual(DataParser.WORD, testIdentifier);
        }

        [TestMethod]
        public void testGetClearLinesMessage()
        {
            //arrange
            string testIdentifier;


            //act
            byte[] message = DataParser.GetClearLinesMessage();

            DataParser.getJsonIdentifier(message, out testIdentifier);

            int expectedMessageLength = BitConverter.ToInt32(message, 0);

            //assert
            Assert.AreEqual(message.Length, expectedMessageLength);
            Assert.AreEqual(DataParser.CLEAR_LINES, testIdentifier);
        }

        [TestMethod]
        public void testGetGuessMessage()
        {
            //arrange
            string guess = "guess";
            string testIdentifier;

            //act
            byte[] message = DataParser.GetGuessMessage(guess);

            int expectedMessageLength = BitConverter.ToInt32(message, 0);

            DataParser.getJsonIdentifier(message, out testIdentifier);

            byte[] testPayload = message.Skip(5).ToArray();

            //assert
            Assert.AreEqual(message.Length, expectedMessageLength);
            Assert.AreEqual(0x02, message[4]);
            Assert.AreEqual(guess, DataParser.GetGuessFromjsonMessage(testPayload));
            Assert.AreEqual(DataParser.GUESS, testIdentifier);
        }

        [TestMethod]
        public void testGetGuessScoreMessage()
        {
            //arrange
            uint clientID = 100;
            int score = 99;
            string testIdentifier;

            //act
            byte[] message = DataParser.GetGuessScoreMessage(clientID, score);

            int expectedMessageLength = BitConverter.ToInt32(message, 0);

            DataParser.getJsonIdentifier(message, out testIdentifier);

            byte[] testPayload = message.Skip(5).ToArray();

            //assert
            Assert.AreEqual(message.Length, expectedMessageLength);
            Assert.AreEqual(0x02, message[4]);
            Assert.AreEqual((clientID, score), DataParser.GetGuessScoreFromjsonMessage(testPayload));
            Assert.AreEqual(DataParser.GUESS, testIdentifier);
        }

        [TestMethod]
        public void testGetTurnOverMessage()
        {
            //arrange
            string word = "word";
            string testIdentifier;

            //act
            byte[] message = DataParser.GetTurnOverMessage(word);

            int expectedMessageLength = BitConverter.ToInt32(message, 0);

            DataParser.getJsonIdentifier(message, out testIdentifier);

            byte[] testPayload = message.Skip(5).ToArray();

            //assert
            Assert.AreEqual(message.Length, expectedMessageLength);
            Assert.AreEqual(0x02, message[4]);
            Assert.AreEqual(word, DataParser.GetWordFromjsonTurnOverMessage(testPayload));
            Assert.AreEqual(DataParser.TURN_OVER, testIdentifier);
        }

        [TestMethod]
        public void testGetGameOverMessage()
        {
            //arrange
            string testIdentifier;

            //act
            byte[] message = DataParser.GetGameOverMessage();

            DataParser.getJsonIdentifier(message, out testIdentifier);

            int expectedMessageLength = BitConverter.ToInt32(message, 0);

            //assert
            Assert.AreEqual(message.Length, expectedMessageLength);
            Assert.AreEqual(DataParser.GAME_OVER, testIdentifier);
        }
    }
}
