using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Client
    {
        private string username;
        private int clientID;

        private TcpClient tcpClient;
        private NetworkStream networkStream;

        public Client(TcpClient tcpClient, int clientID)
        {
            this.tcpClient = tcpClient;
            this.networkStream = this.tcpClient.GetStream();
        }
    }
}
