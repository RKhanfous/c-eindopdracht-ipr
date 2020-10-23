using SharedNetworking.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server
{
    public class NetworkHandler
    {
        private TcpListener listener;
        public List<Client> clients { get; set; }
        public IServer Server { get; private set; }

        public NetworkHandler(IServer server)
        {
            this.Server = server;
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
                if (clients[i].ClientId == randomClientID)
                {
                    randomClientID = r.Next();
                    i = 0;
                }
            }
            var tcpClient = listener.EndAcceptTcpClient(ar);
            Console.WriteLine($"Client connected from {tcpClient.Client.RemoteEndPoint}");
            clients.Add(new Client(tcpClient, this, (uint)randomClientID));


            listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }
        internal void TellNewTurn(Player currentPlayer, string currentWord, List<Player> players)
        {
            //clients = new List<Client>();
            //listener = new TcpListener(IPAddress.Any, 15243);
            //listener.Start();
            //listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }

        internal void Disconnect(Client client)
        {
            clients.Remove(client);
            Console.WriteLine("Client disconnected");
        }

        internal void SendToUser(int clientID, string packet)
        {
            foreach (var client in clients.Where(c => c.ClientId == clientID))
            {
                client.Write(0x02, packet);
            }
        }

        public bool checkClientsForUser(int clientID)
        {
            foreach (Client client in clients)
            {
                if (client.ClientId == clientID)
                {
                    return true;
                }
            }
            return false;
        }

        public Client getClientByUser(uint clientID)
        {
            foreach (Client client in clients)
            {
                if (client.ClientId == clientID)
                {
                    return client;
                }
            }
            return null;
        }

        internal void DrewLine(uint clientID, byte[] messageBytes)
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

        internal void SendDataToPlayer(IClient client)
        {
            Player player = Server.GetPlayer(client.ClientId);
            SkribblRoom skribblRoom = player.playingInRoom;
            foreach (Player participant in skribblRoom.GetPlayers())
            {
                Console.WriteLine($"sending {participant.GetDataPlayer()}");
                client.SendMessage(DataParser.GetPlayerMessage(participant.GetDataPlayer()));
            }
        }

        internal void TellAboutNewPlayer(List<Player> players, Player newPlayer)
        {
            foreach (Player player in players)
            {
                getClientByUser(player.clientID).SendMessage(DataParser.GetPlayerMessage(newPlayer.GetDataPlayer()));
            }
        }
    }
}
