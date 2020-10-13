using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    /// <summary>
    /// skribbl room
    /// 
    /// </summary>
    class SkribbleRoom
    {
        #region private Members

        private NetworkHandler networkHandler;
        private string roomCode;
        private List<Player> players;
        private int currentRound;
        private int numRounds;
        private List<string> words;
        private List<string> usedWords;
        private string currentWord;
        private Player currentPlayer;
        private List<Player> drawingPlayers;
        private bool running;

        #endregion

        #region constructors

        public SkribbleRoom(NetworkHandler networkHandler)
        {

        }

        public SkribbleRoom(NetworkHandler networkHandler, string roomCode)
        {

        }

        public SkribbleRoom(NetworkHandler networkHandler, string roomCode, int numberOfRounds)
        {
            this.networkHandler = networkHandler;
            this.roomCode = roomCode;
            this.players = new List<Player>();
            this.currentRound = 0;
            this.numRounds = numberOfRounds;
        }

        #endregion

        /// <summary>
        /// main loop for the room
        /// </summary>
        /// <param name="objectState"></param>
        public void Run(object objectState)
        {
            while (running)
            {

            }
        }

        #region Getters and setters

        public void AddPlayer(Player player)
        {
            lock (this.players)
            {
                this.players.Add(player);
            }
        }

        public void RemovePlayer(Player player)
        {
            lock (this.players)
            {
                this.players.Remove(player);
            }
        }

        #endregion
    }
}
