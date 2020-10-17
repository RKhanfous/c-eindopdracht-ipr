using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class NetworkHandler
    {
        private TcpListener listener;
        private List<Client> clients;
        public NetworkHandler()
        {
            clients = new List<Client>();
            listener = new TcpListener(IPAddress.Any, 15243);
            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }

        private void OnConnect(IAsyncResult ar)
        {
            var tcpClient = listener.EndAcceptTcpClient(ar);
            Console.WriteLine($"Client connected from {tcpClient.Client.RemoteEndPoint}");
            clients.Add(new Client(TcpClient, );
            listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }
    }

    class DataParser
    {
        internal void TellNewTurn(Player currentPlayer, string currentWord, List<Player> players)
        {
            throw new NotImplementedException();
        }

        internal void TellTurnOver(List<Player> players, string currentWord)
        {
            throw new NotImplementedException();
        }

        internal void TellGameReset(List<Player> players)
        {
            throw new NotImplementedException();
        }

        internal void TellGameOver(List<Player> players)
        {
            throw new NotImplementedException();
        }
    }
}
