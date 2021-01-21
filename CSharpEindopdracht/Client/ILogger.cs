using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server
{
    public interface ILogger
    {
        string logException(string exception);
        void logServer();
        void logConnectClient(int clientID);
        void logDisconnectClient(Client client);
        void logStartGame(Player player);
    }
}
