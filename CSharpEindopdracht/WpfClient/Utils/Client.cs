using SharedNetworking.Utils;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace WpfClient.Utils
{
    class Client : SharedClient
    {
        private string username { get; set; }
        private IClientCallback clientCallback;


        public Client(TcpClient tcpClient, string username, IClientCallback clientCallback) : base(tcpClient)
        {
            this.username = username;
            this.clientCallback = clientCallback;
        }


        protected override void HandleData(byte[] messageBytes)
        {
            throw new NotImplementedException();
        }
    }
}
