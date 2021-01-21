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
            byte[] testArray = new byte[9];
            for (int i = 0; i < 10; i++)
            {
                testArray[i] = 0x02;
            }

            string testIdentifier = "START";
            // Act
            bool testResult = SharedNetworking.Utils.DataParser.getJsonIdentifier(testArray, out testIdentifier);

            // Assert
            Assert.AreEqual(true, testResult);
        }
    }
}
