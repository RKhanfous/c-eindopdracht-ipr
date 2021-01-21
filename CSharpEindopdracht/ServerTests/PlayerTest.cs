using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerTests
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void testAddScore_addsScoreToPlayer_whereScoreGreaterThanZero()
        {
            // Arrange
            Server.Player testplayer = new Player("testPerson", 0, (uint)100);
            int testScore = 100;

            // Act
            testplayer.AddScore(testScore);

            // Assert
            Assert.AreEqual(testScore, testplayer.score);
        }

        [TestMethod]
        public void testAddScore_addsScoreToPlayer_whereScoreLessThanZero()
        {
            // Arrange
            Player testplayer = new Player("testPerson", 0, (uint)100);
            int testScore = -100;

            // Act
            testplayer.AddScore(testScore);

            // Assert
            Assert.AreNotEqual(testScore, testplayer.score);
        }

        [TestMethod]
        public void testGetJsonIdentifier_returnsBool_where()
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
    }
}
