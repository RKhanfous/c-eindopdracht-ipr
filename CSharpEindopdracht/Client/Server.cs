using SharedNetworking.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Server : IServer
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

        public void GiveBytes(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        protected override void HandleData(byte[] messageBytes)
        {
            throw new NotImplementedException();
        }
    }
}
