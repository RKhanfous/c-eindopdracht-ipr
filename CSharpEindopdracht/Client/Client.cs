using Networking;
using SharedNetworking.Utils;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Client : SharedClient
    {

        private string username;
        private int clientID;

        public Client(TcpClient tcpClient, DataHandler dataHandler, int clientID) : base(tcpClient, dataHandler)
        {
            this.clientID = clientID;
        }



    }
}
