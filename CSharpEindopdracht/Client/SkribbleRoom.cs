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

        private string RoomCode;
        private List<Player> players;
        private int currentRound;
        private int maxNumRounds;
        private List<string> words;
        private List<string> usedWords;
        private string currentWord;

        #endregion

        #region constructors

        public SkribbleRoom()
        {
            this.players = new List<Player>();
        }

        #endregion

        /// <summary>
        /// main loop for the room
        /// </summary>
        /// <param name="objectState"></param>
        public void Run(object objectState)
        {

        }
    }
}
