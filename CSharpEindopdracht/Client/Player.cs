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

        public Player(string username, int score, int clientID)
        {
            this.username = username;
            this.score = score;
            this.clientID = clientID;
        }
    }
}
