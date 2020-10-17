using System.Net.Sockets;

namespace Server
{
    internal class Client : SharedClient
    {

        private string username;
        private int clientID;

        public Client(TcpClient tcpClient, int clientID) : base(tcpClient)
        {
            this.clientID = clientID;
        }

        protected override void HandleData(byte[] messageBytes)
        {
            throw new NotImplementedException();
        }
    }
}
