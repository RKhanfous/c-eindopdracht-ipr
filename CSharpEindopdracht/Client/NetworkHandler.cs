using SharedNetworking.Utils;
using SharedSkribbl;
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
            foreach (Player player in players)
            {
                getClientByUser(player.clientID).SendMessage(DataParser.GetDrawerMessage(currentPlayer.clientID));
            }
            getClientByUser(currentPlayer.clientID).SendMessage(DataParser.GetWordMessage(currentWord));

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
            SkribblRoom skribblRoom = this.Server.GetPlayer(clientID).playingInRoom;
            skribblRoom.lines.Add(Line.GetLine(messageBytes.Skip(5).ToArray()));
            foreach (Player player in this.Server.GetPlayer(clientID).playingInRoom.GetPlayers())
            {
                if (player.clientID != clientID)
                    getClientByUser(player.clientID).SendMessage(messageBytes);
            }
        }
        internal void DeleteLine(uint clientID, byte[] messageBytes)
        {
            SkribblRoom skribblRoom = this.Server.GetPlayer(clientID).playingInRoom;
            Line deleteLine = Line.GetLine(messageBytes.Skip(5).ToArray());
            foreach (Line line in skribblRoom.lines)
            {
                if (line.Id == deleteLine.Id)
                {
                    skribblRoom.lines.Remove(line);
                    break;
                }
            }
            foreach (Player player in this.Server.GetPlayer(clientID).playingInRoom.GetPlayers())
            {
                if (player.clientID != clientID)
                    getClientByUser(player.clientID).SendMessage(messageBytes);
            }
        }
        internal void TellGameReset(List<Player> players)
        {
            SharedNetworking.Utils.Player[] dataPlayers = new SharedNetworking.Utils.Player[players.Count];
            int index = 0;
            foreach (Player p in players)
            {
                dataPlayers[index] = p.GetDataPlayer();
                index++;
            }
            foreach (Player player in players)
            {
                getClientByUser(player.clientID).SendMessage(DataParser.GetGameOverMessage(dataPlayers));
            }
        }

        internal void TellGameStart(List<Player> players)
        {
            foreach (Player player in players)
            {
                getClientByUser(player.clientID).SendMessage(DataParser.GetGoToRoomMessage(player.playingInRoom.roomCode, true));
            }
        }

        internal void Guess(uint clientId, string guess)
        {
            Player player = this.Server.GetPlayer(clientId);
            int score = player.playingInRoom.guess(player, guess);
            foreach (Player roomPlayers in player.playingInRoom.GetPlayers())
            {
                getClientByUser(roomPlayers.clientID).SendMessage(
                    DataParser.GetGuessScoreMessage(player.clientID, score));
            }
        }

        internal void ClearLines(uint clientId)
        {
            SkribblRoom skribblRoom = this.Server.GetPlayer(clientId).playingInRoom;
            skribblRoom.lines.Clear();
            foreach (Player player in skribblRoom.GetPlayers())
            {
                if (player.clientID != clientId)
                    getClientByUser(player.clientID).SendMessage(DataParser.GetClearLinesMessage());
            }
        }

        internal void TellGameOver(List<Player> players)
        {
            foreach (Player player in players)
            {
                getClientByUser(player.clientID).SendMessage(DataParser.GetGoToRoomMessage(player.playingInRoom.roomCode, false));
            }
        }

        internal void TellTurnOver(List<Player> players, string currentWord)
        {
            foreach (Player p in players)
            {
                getClientByUser(p.clientID).SendMessage(DataParser.GetTurnOverMessage(currentWord));
            }
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
