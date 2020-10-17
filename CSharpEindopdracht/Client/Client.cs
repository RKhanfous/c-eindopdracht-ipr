using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    internal class Client : SharedClient
    {

        private string username;
        public int clientID { get; set; }
        private TcpClient tcpClient;
        private NetworkStream stream;

        public Client(TcpClient tcpClient, DataHandler dataHandler, int clientID) : base(tcpClient, dataHandler)
        {
            this.clientID = clientID;
            this.tcpClient = tcpClient;
            this.stream = this.tcpClient.GetStream();
            Thread listernerThread = new Thread(()=>Listener());
            listernerThread.Start();
        }

        public Task Listener()
        {

        }

        public void Write()
        {

        }

        internal void Write(string packet)
        {
            throw new NotImplementedException();
        }
    }
}
