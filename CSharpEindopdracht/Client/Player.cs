using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class Player
    {
        #region public properties

        public string username { get; }
        public int score { get; private set; }
        public uint clientID { get; }
        public SkribblRoom playingInRoom { get; set; }

        #endregion

        public Player(string username, int score, uint clientID)
        {
            this.username = username;
            this.score = score;
            this.clientID = clientID;
        }

        public void AddScore(int score)
        {
            if (score < 0)
            {
                throw new ArgumentOutOfRangeException("cannot add negative scores");
            }
            this.score += score;
        }

        public SharedNetworking.Utils.Player GetDataPlayer()
        {
            return new SharedNetworking.Utils.Player() { Username = username, Id = (uint)clientID, Score = (uint)score };
        }
    }
}
