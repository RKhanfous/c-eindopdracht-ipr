using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Player
    {
        private string username;
        private int score;
        private int clientID;
        public SkribbleRoom playingInRoom { get; set; }
        public bool active { get; set; }

        public Player(string username, int score, int clientID)
        {
            this.username = username;
            this.score = score;
            this.clientID = clientID;
        }
    }
}
