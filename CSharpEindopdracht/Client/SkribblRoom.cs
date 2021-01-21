using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using SharedSkribbl;
using System.Collections.ObjectModel;
using System.IO;

namespace Server
{
    /// <summary>
    /// skribbl room
    /// 
    /// </summary>
    public class SkribblRoom
    {
        #region private Members

        private NetworkHandler networkHandler;
        private List<Player> players;
        private List<Player> correctlyGuessedPlayers;
        private int currentRound;
        private int numRounds;
        private HashSet<string> words;
        private HashSet<string> usedWords = new HashSet<string>();
        private string currentWord;
        private Player currentPlayer;
        private List<Player> drawingPlayers = new List<Player>();
        private Timer timer;
        private const int maxNumPlayers = 8;
        public const int guessTimeMills = 30000;
        private Stopwatch stopwatch;
        private string filePath;

        #endregion

        #region Propertys

        public bool running { get; set; }
        public string roomCode { get; set; }
        public List<Line> lines { get; set; } = new List<Line>();

        #endregion

        #region constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="networkHandler"></param>
        public SkribblRoom(NetworkHandler networkHandler) : this(networkHandler, "") { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="networkHandler"></param>
        /// <param name="roomCode"></param>
        public SkribblRoom(NetworkHandler networkHandler, string roomCode) : this(networkHandler, roomCode, 3) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="networkHandler"></param>
        /// <param name="roomCode"></param>
        /// <param name="numberOfRounds"></param>
        public SkribblRoom(NetworkHandler networkHandler, string roomCode, int numberOfRounds)
        {
            // Filepath for FileIO
            this.filePath = @"Words\GameWords.txt";
            this.words = new HashSet<string>();
            // Reading lines from File and adding them to the HashSet
            foreach (string word in File.ReadAllLines(filePath))
            {
                this.words.Add(word);
            }
            this.networkHandler = networkHandler;
            this.roomCode = roomCode;
            this.players = new List<Player>();
            this.correctlyGuessedPlayers = new List<Player>();
            this.currentRound = 0;
            if (numRounds > 0)
                this.numRounds = numberOfRounds;
            else
                this.numRounds = 1;
            this.timer = new Timer();
            this.stopwatch = new Stopwatch();
        }



        #endregion

        #region main logic
        /// <summary>
        /// main loop for the room
        /// </summary>
        /// <param name="objectState"></param>
        public bool Start()
        {
            //start of room
            //checks
            if (!Check())
            {
                return false;
            }

            if (running)
            {
                return false;
            }

            this.networkHandler.TellGameStart(this.players);

            SetNextRound();

            //setup timer
            this.timer.AutoReset = false;
            this.timer.Elapsed += OnEndTurn;
            this.timer.Enabled = true;
            this.timer.Interval = guessTimeMills;

            this.running = true;

            SetNextTurn();

            return true;
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
                    Debug.WriteLine("not enough players");
                    return false;
                }

                if (this.words.Count < (this.numRounds - this.currentRound) * maxNumPlayers)
                {
                    Debugger.Break();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// kill room and release all recourses
        /// </summary>
        private void Stop()
        {
            this.running = false;
            this.networkHandler.TellGameOver(this.players);
            this.timer.Dispose();
            this.stopwatch.Stop();
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
            if (!Check())
                Debug.WriteLine("did not pass check");

            this.networkHandler.TellTurnOver(this.players, this.currentWord);

            Sleep(5000);//idk why

            if (this.drawingPlayers.Count == 0)
                if (this.currentRound < numRounds)
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
            if (!Check())
            {
                this.Stop();
                return;
            }

            //reset room
            this.words.UnionWith(this.usedWords);
            this.usedWords.Clear();

            this.networkHandler.TellGameReset(this.players);

            Sleep(5000);

            Start();
        }


        #endregion

        #region Getters and setters

        public void AddPlayer(Player player)
        {
            lock (this.players)
            {
                this.players.Add(player);
                player.playingInRoom = this;
            }
            this.networkHandler.TellAboutNewPlayer(this.players.Take(this.players.Count - 1).ToList(), player); ;
        }

        public void RemovePlayer(Player player)
        {
            lock (this.players)
            {
                this.players.Remove(player);
                player.playingInRoom = null;
            }
        }

        public bool TryAddPlayer(Player player)
        {
            lock (this.players)
            {
                if (this.players.Count >= maxNumPlayers)
                    return false;
                this.players.Add(player);
                player.playingInRoom = this;
            }
            this.networkHandler.TellAboutNewPlayer(this.players.Take(this.players.Count - 1).ToList(), player); ;
            return true;
        }

        public ReadOnlyCollection<Player> GetPlayers()
        {
            lock (this.players)
            {
                return new ReadOnlyCollection<Player>(this.players);
            }
        }

        #endregion

        #region public skribbl methods

        /// <summary>
        /// returns how much points a player recieves for their guess if it is correct.
        /// returns -1 is incorrect guess
        /// returns -1 is not allowed to guess
        /// updates internal data
        /// </summary>
        /// <param name="player">player who guesses</param>
        /// <param name="guess">the guessed string</param>
        /// <returns></returns>
        public int guess(Player player, string guess)
        {
            //check if guess is correct
            if (guess.ToLower() != this.currentWord.ToLower())
                return -1;

            //check if player already guessed correctly
            if (correctlyGuessedPlayers.Contains(player))
                return -2;
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

            //add player to list of players that already guessed correcly
            correctlyGuessedPlayers.Add(player);

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

        private T getrandom<T>(HashSet<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return default;
            }
            Random random = new Random();
            int randomPos = random.Next(list.Count);// 0 >= randomInt < list.Count   ?
            int current = 0;
            foreach (T t in list)
            {
                if (current == randomPos)
                    return t;
                current++;
            }
            return default;
        }

        private async void Sleep(int millis)
        {
            await Task.Delay(millis);
        }

        #endregion

    }
}
