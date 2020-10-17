using System.Net.Sockets;

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
