using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public interface IServer
    {
        (string, bool) GetRoom(string username, uint clientID, string roomCode);
        Player GetPlayer(uint clientId);
    }
}
