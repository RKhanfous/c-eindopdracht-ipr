using SharedNetworking.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace WpfClient.Utils
{
    class Client : SharedClient
    {
        private string username { get; set; }
        private IClientCallback clientCallback;


        public Client(TcpClient tcpClient, IClientCallback clientCallback) : base(tcpClient)
        {
            this.clientCallback = clientCallback;
        }


        protected override void HandleData(byte[] messageBytes)
        {
            throw new NotImplementedException();
        }



        public bool Connected()
        {
            return this.client.Client.Connected;
        }

        internal void LogOn(string username, string roomCode)
        {
            this.username = username;
            sendMessage(DataParser.GetLogOnJsonMessage(username, roomCode));
        }
    }
}
