using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerTests
{
    [TestClass]
    class SkribbleRoomTest
    {
        [TestMethod]
        public void simpleTest()
        {
            TestNetworHandler networHandler = new TestNetworHandler();
            SkribblRoom skribblRoom = new SkribblRoom(networHandler);

            Assert.AreEqual("", skribblRoom.roomCode);
            Assert.AreEqual(false, skribblRoom.running);

            Assert.AreEqual(0, networHandler.TellAboutNewPlayerCount);
            Assert.AreEqual(0, networHandler.TellGameOverCount);
            Assert.AreEqual(0, networHandler.TellGameResetCount);
            Assert.AreEqual(0, networHandler.TellGameStartCount);
            Assert.AreEqual(0, networHandler.TellNewTurnCount);
            Assert.AreEqual(0, networHandler.TellTurnOverCount);
        }

        [TestMethod]
        public void test2()
        {
            TestNetworHandler networHandler = new TestNetworHandler();
            SkribblRoom skribblRoom = new SkribblRoom(networHandler);

            skribblRoom.lines

        }

    }

    class TestNetworHandler : INetworkHandler
    {
        public int TellAboutNewPlayerCount { get; private set; }
        public int TellGameOverCount { get; private set; }
        public int TellGameResetCount { get; private set; }
        public int TellGameStartCount { get; private set; }
        public int TellNewTurnCount { get; private set; }
        public int TellTurnOverCount { get; private set; }
        public void TellAboutNewPlayer(List<Player> lists, Player player)
        {
            TellAboutNewPlayerCount++;
        }

        public void TellGameOver(List<Player> players)
        {
            TellGameOverCount++;
        }

        public void TellGameReset(List<Player> players)
        {
            TellGameResetCount++;
        }

        public void TellGameStart(List<Player> players)
        {
            throw new NotImplementedException();
        }

        public void TellNewTurn(Player currentPlayer, string currentWord, List<Player> players)
        {
            throw new NotImplementedException();
        }

        public void TellTurnOver(List<Player> players, string currentWord)
        {
            throw new NotImplementedException();
        }
    }
}
