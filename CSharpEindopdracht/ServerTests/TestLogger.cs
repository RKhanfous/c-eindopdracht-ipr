using Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerTests
{
    class TestLogger : ILogger
    {
        public void logConnectClient(int clientID)
        {
            Console.WriteLine($"logConnectClient {clientID}");
        }

        public void logDisconnectClient(Client client)
        {
            Console.WriteLine($"logDisconnectClient {client}");
        }

        public string logException(string exception)
        {
            Console.WriteLine($"logException {exception}");
            return exception;
        }

        public void logServer()
        {
            Console.WriteLine($"logServer");
        }

        public void logStartGame(Player player)
        {
            Console.WriteLine($"logStartGame {player}");
        }
    }
}
