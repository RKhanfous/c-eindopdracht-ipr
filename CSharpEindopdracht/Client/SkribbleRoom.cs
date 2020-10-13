using System.Collections.Generic;

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

        #endregion

        #region Propertys

        public bool running { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="networkHandler"></param>
        public SkribbleRoom(NetworkHandler networkHandler) : this(networkHandler, "") { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="networkHandler"></param>
        /// <param name="roomCode"></param>
        public SkribbleRoom(NetworkHandler networkHandler, string roomCode) : this(networkHandler, roomCode, 3) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="networkHandler"></param>
        /// <param name="roomCode"></param>
        /// <param name="numberOfRounds"></param>
        public SkribbleRoom(NetworkHandler networkHandler, string roomCode, int numberOfRounds)
        {
            this.networkHandler = networkHandler;
            this.roomCode = roomCode;
            this.players = new List<Player>();
            this.currentRound = 0;
            this.numRounds = numberOfRounds;
            this.words = new List<string> { "Boot", "Zon", "Mens", "Gras", "Water", "Sneeuw", "Kerk", "Concert", "Slang", "Huis", "Computer", "Klok", "vlees", "Tong", "Mug", "Soldaat" };
        }

        #endregion

        /// <summary>
        /// main loop for the room
        /// </summary>
        /// <param name="objectState"></param>
        public void Run(object objectState)
        {
            //start of room
            //checks
            doChecks();

            //setup round
            setNewRound();

            while (running)
            {

            }
        }

        /// <summary>
        /// checks if room can continue running
        /// </summary>
        private void doChecks()
        {
            lock (this.players)
            {
                if (this.players.Count < 2)
                {

                }

                //current round is 0 because this is the beginning
                if (this.words.Count < (this.numRounds = this.currentRound) * 8)
                {
                    //stop();
                    //return;
                }
            }
        }

        /// <summary>
        /// sets up new round
        /// </summary>
        private void setNewRound()
        {
            this.currentRound++;
            this.drawingPlayers.Clear();
            this.players.ForEach((player) => { this.drawingPlayers.Add(player); });
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

        /// <summary>
        /// things that need to be done to stop
        /// </summary>
        private void stop()
        {
            //TODO tell all players to stop

            this.running = false;
        }
    }
}
