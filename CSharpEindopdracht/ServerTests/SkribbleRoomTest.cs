using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ServerTests
{
    [TestClass]
    public class SkribbleRoomTest
    {
        [TestMethod]
        public void SimpleTest()
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
        public void Test1()
        {
            //arrange
            TestNetworHandler networHandler = new TestNetworHandler();
            SkribblRoom skribblRoom = new SkribblRoom(networHandler);

            Player player = new Player("super awsome username", 10000, 0);

            //act
            skribblRoom.AddPlayer(player);

            //assert
            Assert.AreEqual(1, skribblRoom.GetPlayers().Count);

            foreach (Player p in skribblRoom.GetPlayers())
            {
                Assert.AreEqual(player.username, p.username);
                Assert.AreEqual(skribblRoom, p.playingInRoom);
            }

        }

        [TestMethod]
        public void Test2()
        {
            //arrange
            TestNetworHandler networHandler = new TestNetworHandler();
            SkribblRoom skribblRoom = new SkribblRoom(networHandler);

            Player player = new Player("super awsome username", 10000, 0);

            //act
            skribblRoom.AddPlayer(player);

            //assert
            Assert.AreEqual(1, skribblRoom.GetPlayers().Count);

            foreach (Player p in skribblRoom.GetPlayers())
            {
                Assert.AreEqual(player.username, p.username);
                Assert.AreEqual(skribblRoom, p.playingInRoom);
            }

            //act
            skribblRoom.RemovePlayer(player);

            //assert
            Assert.AreEqual(0, skribblRoom.GetPlayers().Count);
        }

        [TestMethod]
        public void Test3()
        {
            //arrange
            TestNetworHandler networHandler = new TestNetworHandler();
            SkribblRoom skribblRoom = new SkribblRoom(networHandler);

            Player player1 = new Player("super awsome username2", 10000, 0);
            Player player2 = new Player("super awsome username2", 20000, 1);

            skribblRoom.AddPlayer(player1);

            Assert.IsFalse(skribblRoom.Start());

            Assert.IsTrue(skribblRoom.TryAddPlayer(player2));

            Assert.AreEqual(2, skribblRoom.GetPlayers().Count);

            Assert.IsTrue(skribblRoom.Start());

            Assert.AreEqual(2, networHandler.TellAboutNewPlayerCount);
            Assert.AreEqual(0, networHandler.TellGameOverCount);
            Assert.AreEqual(0, networHandler.TellGameResetCount);
            Assert.AreEqual(1, networHandler.TellGameStartCount);
            Assert.AreEqual(1, networHandler.TellNewTurnCount);
            Assert.AreEqual(0, networHandler.TellTurnOverCount);
        }

        [TestMethod]
        public void Test4()
        {
            //arrange
            TestNetworHandler networHandler = new TestNetworHandler();
            SkribblRoom skribblRoom = new SkribblRoom(networHandler);

            Player player1 = new Player("super awsome username2", 10000, 0);
            Player player2 = new Player("super awsome username2", 20000, 1);

            skribblRoom.AddPlayer(player1);
            skribblRoom.AddPlayer(player2);

            Assert.IsTrue(skribblRoom.Start());

            Thread waitingThread = new Thread(() =>
            {
                int waited = 0;
                int waitIncrements = 10;
                while (networHandler.TellTurnOverCount <= 0 && waited < 31000)// turn takes a maximum of 30 sec
                {
                    Thread.Sleep(waitIncrements);
                    waited += waitIncrements;
                }
            });

            waitingThread.Start();

            Assert.AreEqual(2, networHandler.TellAboutNewPlayerCount);
            Assert.AreEqual(0, networHandler.TellGameOverCount);
            Assert.AreEqual(0, networHandler.TellGameResetCount);
            Assert.AreEqual(1, networHandler.TellGameStartCount);
            Assert.AreEqual(1, networHandler.TellNewTurnCount);
            Assert.AreEqual(0, networHandler.TellTurnOverCount);

            waitingThread.Join();
            Assert.AreEqual(1, networHandler.TellTurnOverCount);
            Assert.AreEqual(2, networHandler.TellNewTurnCount);


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
            TellGameStartCount++;
        }

        public void TellNewTurn(Player currentPlayer, string currentWord, List<Player> players)
        {
            TellNewTurnCount++;
        }

        public void TellTurnOver(List<Player> players, string currentWord)
        {
            TellTurnOverCount++;
        }
    }
}
