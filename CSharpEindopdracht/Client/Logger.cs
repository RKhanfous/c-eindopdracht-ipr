using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server
{
    public class Logger : ILogger
    {
        private string logPath { get; set; }
        public StreamWriter outputFile { get; }

        public Logger(string logPathApp)
        {
            this.logPath = logPathApp;
            this.outputFile = new StreamWriter(Path.Combine(logPath, "SystemLog" + StringBuilderTime() + ".txt"));
        }

        public void logException(String exception)
        {
            this.outputFile.WriteLine("[" + DateTime.Now + "] this exception has occured: " + exception + "!");
        }

        public void logServer()
        {
            this.outputFile.WriteLine("[" + DateTime.Now + "] server started!");
        }

        public void logStartGame(Player player)
        {
            this.outputFile.WriteLine("[" + DateTime.Now + "] player:" + player.username + "with clientID:" + player.clientID + " started a game.");
        }

        public void logConnectClient(int clientID)
        {
            this.outputFile.WriteLine("[" + DateTime.Now + "]" + clientID + " connected to the server!");
            throw new NotImplementedException();
        }

        public void logDisconnectClient(Client client)
        {
            this.outputFile.WriteLine("[" + DateTime.Now + "]" + client.ClientId + " left the server!");
            throw new NotImplementedException();
        }

        private string StringBuilderTime()
        {
            return "[" + DateTime.Now.Hour + "H_" + DateTime.Now.Minute + "M_" + DateTime.Now.Second + "S_Day_" + DateTime.Now.Day + "_Month_" + DateTime.Now.Month + "_Year_" + DateTime.Now.Year + "]";
        }
    }
}
