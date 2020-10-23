using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class NetworkHandler
    {
        private TcpListener listener;
        public List<Client> clients { get; set; }
        public NetworkHandler()
        {
            clients = new List<Client>();
            listener = new TcpListener(IPAddress.Any, 5555);
            listener.Start();
            Console.WriteLine($"==========================================================================\n" +
                                $"\tstarted accepting clients at {DateTime.Now}\n" +
                            $"==========================================================================");
            listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }

        private void OnConnect(IAsyncResult ar)
        {
            Random r = new Random();
            int randomClientID = r.Next();
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].clientID == randomClientID)
                {
                    randomClientID = r.Next();
                    i = 0;
                }
            }
            var tcpClient = listener.EndAcceptTcpClient(ar);
            Console.WriteLine($"Client connected from {tcpClient.Client.RemoteEndPoint}");
            clients.Add(new Client(tcpClient, this, randomClientID));
            listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }
        internal void TellNewTurn(Player currentPlayer, string currentWord, List<Player> players)
        {
            clients = new List<Client>();
            listener = new TcpListener(IPAddress.Any, 15243);
            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }

        internal void Disconnect(Client client)
        {
            clients.Remove(client);
            Console.WriteLine("Client disconnected");
        }

        internal void SendToUser(int clientID, string packet)
        {
            foreach (var client in clients.Where(c => c.clientID == clientID))
            {
                client.Write(0x02, packet);
            }
        }

        public bool checkClientsForUser(int clientID)
        {
            foreach (Client client in clients)
            {
                if (client.clientID == clientID)
                {
                    return true;
                }
            }
            return false;
        }

        public Client getClientByUser(int clientID)
        {
            foreach (Client client in clients)
            {
                if (client.clientID == clientID)
                {
                    return client;
                }
            }
            return null;
        }

        internal void drawLine(int clientID, byte[] messageBytes)
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

        internal void TellTurnOver(List<Player> players, string currentWord)
        {
            throw new NotImplementedException();
        }
    }
}
