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

        private void sendMessage(byte[] message)
        {
            this.stream.BeginWrite(message, 0, message.Length, new AsyncCallback(OnWrite), null);
        }

        private void OnWrite(IAsyncResult ar)
        {
            this.stream.EndWrite(ar);
        }

        internal void LogOn(string username)
        {
            this.username = username;
            Debug.WriteLine("sending username " + username);
            sendMessage(DataParser.getLogOnJsonMessage(username));
        }
    }
}
