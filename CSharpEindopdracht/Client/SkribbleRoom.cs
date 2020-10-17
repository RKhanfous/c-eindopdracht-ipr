using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using System.Linq;
using System.Diagnostics;

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
        private List<Player> correctlyGuessedPlayers;
        private int currentRound;
        private int numRounds;
        private List<string> words;
        private List<string> usedWords;
        private string currentWord;
        private Player currentPlayer;
        private List<Player> drawingPlayers;
        private Timer timer;
        private const int maxNumPlayers = 8;
        private const int guessTimeMills = 30000;
        private Stopwatch stopwatch;

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
            this.correctlyGuessedPlayers = new List<Player>();
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

            ////setup round
            //SetNextRound();


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

                if (this.words.Count < (this.numRounds = this.currentRound) * maxNumPlayers)
                {
                    //stop();
                    //return;
                }
            }
            return true;
        }

        /// <summary>
        /// things that need to be done to stop
        /// </summary>
        private void Stop()
        {
            //TODO tell all players to stop

            this.running = false;
        }

        #endregion

        #region skribbl logic

        /// <summary>
        /// sets up new round
        /// </summary>
        private void SetNextRound()
        {
            this.currentRound++;
            this.drawingPlayers.Clear();
            this.players.ForEach((player) => { this.drawingPlayers.Add(player); });//better way?
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetNextTurn()
        {
            //clear correctly guessed players
            this.correctlyGuessedPlayers.Clear();

            //get a new player to draw
            this.currentPlayer = getrandom<Player>(this.drawingPlayers);

            //add current player to correctlyGuessedPlayers
            this.correctlyGuessedPlayers.Add(this.currentPlayer);

            //remove the player from the que of players to draw
            this.drawingPlayers.Remove(this.currentPlayer);

            //get new word to draw
            this.currentWord = getrandom<string>(this.words);

            //remove word so it is only used once in a game
            this.words.Remove(this.currentWord);

            //add the word to used words idk why
            this.usedWords.Add(currentWord);

            //tell everyone about the new turn!
            this.networkHandler.TellNewTurn(this.currentPlayer, this.currentWord, this.players);

            //reset timer and stopwatch
            this.stopwatch.Restart();
            this.timer.Start();
        }

        #endregion

        #region timers callbacks

        private void OnEndTurn(object sender, ElapsedEventArgs e)
        {
            this.networkHandler.TellTurnOver(this.players, this.currentWord);

            if (this.drawingPlayers.Count == 0)
                if (this.currentRound < 3)
                    SetNextRound();
                else
                {
                    endOfGame();
                    return;
                }

            SetNextTurn();
        }

        private void endOfGame()
        {
            //stop room or reset room
        }


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

        #region public skribblmethods

        /// <summary>
        /// returns how much points a player recieves for their guess if it is correct.
        /// returns -1 is incorrect guess
        /// updates internal data
        /// </summary>
        /// <param name="player">player who guesses</param>
        /// <param name="guess">the guessed string</param>
        /// <returns></returns>
        public int guess(Player player, string guess)
        {
            //check if guess is correct
            if (guess != this.currentWord)
                return -1;

            //calculate score
            int score = (int)((guessTimeMills - this.stopwatch.ElapsedMilliseconds) / 100);

            //check for negative numbers (because out of sync?)
            if (score < 0)
            {
                Debug.WriteLine($"player {player.username}'s guess was to late");
                return -1;
            }

            //add score
            player.AddScore(score);

            //return score
            return score;
        }

        #endregion

        #region helpers

        private T getrandom<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return default;
            }
            if (list.Count == 1)
            {
                return list[0];
            }
            Random random = new Random();
            return list[random.Next(list.Count)];
        }

        #endregion

    }
}
