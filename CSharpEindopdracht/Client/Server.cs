using SharedNetworking.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Server : IServer
    {
        private NetworkHandler networkHandler;
        private List<SkribblRoom> skribblRooms;
        private List<Player> players;

        public Server()
        {
            networkHandler = new NetworkHandler(this);
            skribblRooms = new List<SkribblRoom>();
            players = new List<Player>();
        }

        public string GetRoom(string username, int clientID, string roomCode)
        {
            Player player = getPlayer(clientID);

            if (player == null)
            {
                player = new Player(username, 0, clientID);
                players.Add(player);
            }

            SkribblRoom skribbleRoom = null;
            if (roomCode == "random" || roomCode == "")
            {
                string newRoomCode = AddToRandomSkribblRoom(player);
                if (newRoomCode != null)
                    return newRoomCode;
            }
            else
            {
                skribbleRoom = GetSkribbleRoom(roomCode);
                if (skribbleRoom != null)
                {
                    return skribbleRoom.roomCode;
                }
            }

            //create randomSkribbleRoom
            if (skribbleRoom == null)
            {
                skribbleRoom = new SkribblRoom(networkHandler, roomCode);
                skribbleRoom.AddPlayer(player);
                return skribbleRoom.roomCode;
            }
            return null;
        }



        #region helpers

        private Player getPlayer(int ClientId)
        {
            foreach (Player player in players)
            {
                if (player.clientID == ClientId)
                {
                    return player;
                }
            }
            return null;
        }

        private SkribblRoom GetSkribbleRoom(string roomCode)
        {
            foreach (SkribblRoom skribbleRoom in skribblRooms)
            {
                if (skribbleRoom.roomCode == roomCode)
                {
                    return skribbleRoom;
                }
            }
            return null;
        }

        private string AddToRandomSkribblRoom(Player player)
        {
            foreach (SkribblRoom skribbleRoom in skribblRooms)
            {
                if (skribbleRoom.TryAddPlayer(player))
                {
                    return skribbleRoom.roomCode;
                }
            }
            return null;
        }

        #endregion
    }
}
