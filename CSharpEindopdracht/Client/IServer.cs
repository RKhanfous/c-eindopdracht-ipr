using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public interface IServer
    {
        string GetRoom(string username, int clientID, string roomCode);
    }
}
