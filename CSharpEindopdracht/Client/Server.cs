using SharedNetworking.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Server : IServer, SharedClient
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
<<<<<<< Updated upstream
        /// <summary>
        /// Makes a new room and adds it to the list with skribbleRooms
        /// </summary>
        /// <param name="roomcode"></param>
        /// <param name="numberOfRounds"></param>
        public void makeRoom(string roomcode, int numberOfRounds)
        {
            SkribbleRoom skribbleRoom = new SkribbleRoom(networkHandler, roomcode, numberOfRounds);
            skribblRooms.Add(skribbleRoom);
=======

        public void GiveBytes(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        protected override void HandleData(byte[] messageBytes)
        {
            throw new NotImplementedException();
>>>>>>> Stashed changes
        }
    }
}
