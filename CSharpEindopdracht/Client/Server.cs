using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Server
    {
        private NetworkHandler networkHandler;
        private List<SkribbleRoom> skribblRooms;
        private List<Player> players;

        public Server()
        {
            networkHandler = new NetworkHandler();
            skribblRooms = new List<SkribbleRoom>();
            players = new List<Player>();
        }
        /// <summary>
        /// Makes a new room and adds it to the list with skribbleRooms
        /// </summary>
        /// <param name="roomcode"></param>
        /// <param name="numberOfRounds"></param>
        public void makeRoom(string roomcode, int numberOfRounds)
        {
            SkribbleRoom skribbleRoom = new SkribbleRoom(networkHandler, roomcode, numberOfRounds);
            skribblRooms.Add(skribbleRoom);
        }
    }
}
