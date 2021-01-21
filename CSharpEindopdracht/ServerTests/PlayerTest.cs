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
            Player testplayer = new Player("testPerson", 0, (uint)100);
            int testScore = 100;

            // Act
            Player.AddScore(testScore);

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
    }
}
