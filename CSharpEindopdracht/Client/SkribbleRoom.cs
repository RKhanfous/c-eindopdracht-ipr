using System.Collections.Generic;
using System.Timers;

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
        private Timer timer;

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
            this.timer = new Timer();
        }

        #endregion


        #region main logic
        /// <summary>
        /// main loop for the room
        /// </summary>
        /// <param name="objectState"></param>
        public void Start(object objectState)
        {
            //start of room
            //checks
            if (Check())
            {
                Stop();
                return;
            }

            //setup round
            SetNewRound();

            while (running)
            {

            }
        }

        /// <summary>
        /// checks if room can continue running
        /// </summary>
        private bool Check()
        {
            lock (this.players)
            {
                if (this.players.Count < 2)
                {
                    return false;
                }

                if (this.words.Count < (this.numRounds = this.currentRound) * 8)
                {
                    //stop();
                    //return;
                }
            }
            return true;
        }

        /// <summary>
        /// sets up new round
        /// </summary>
        private void SetNewRound()
        {
            this.currentRound++;
            this.drawingPlayers.Clear();
            this.players.ForEach((player) => { this.drawingPlayers.Add(player); });
        }

        #endregion

        #region timers callbacks



        #endregion

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
        private void Stop()
        {
            //TODO tell all players to stop

            this.running = false;
        }
    }
}
