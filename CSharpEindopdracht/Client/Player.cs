using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Player
    {
        #region public properties

        public string username { get; }
        public int score { get; private set; }
        public int clientID { get; }
        public SkribblRoom playingInRoom { get; set; }

        #endregion

        public Player(string username, int score, int clientID)
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
    }
}
