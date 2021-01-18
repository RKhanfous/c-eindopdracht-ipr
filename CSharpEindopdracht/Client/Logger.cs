using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server
{
    public class Logger : ILogger
    {
        private string logPath { get; set; }

        private string logName { get; set; }

        public Logger(string logPathApp)
        {
            this.logPath = logPathApp;
            this.logName = "SystemLog" + StringBuilderTime() + ".txt";
        }

        public void logException(String exception)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(logPath, logName))) {
                this.outputFile.WriteLine("[" + DateTime.Now + "] this exception has occured: " + exception + "!");
            }
        }

        public void logServer()
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(logPath, logName)))
            {
                this.outputFile.WriteLine("[" + DateTime.Now + "] server started!");
            }
        }

        public void logStartGame(Player player)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(logPath, logName)))
            {
                this.outputFile.WriteLine("[" + DateTime.Now + "] player:" + player.username + "with clientID:" + player.clientID + " started a game.");
            }
        }

        public void logConnectClient(int clientID)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(logPath, logName)))
            {
                this.outputFile.WriteLine("[" + DateTime.Now + "]" + clientID + " connected to the server!");
            }
        }

        public void logDisconnectClient(Client client)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(logPath, logName)))
            {
                this.outputFile.WriteLine("[" + DateTime.Now + "]" + client.ClientId + " left the server!");
            }
        }

        private string StringBuilderTime()
        {
            return "[" + DateTime.Now.Hour + "H_" + DateTime.Now.Minute + "M_" + DateTime.Now.Second + "S_Day_" + DateTime.Now.Day + "_Month_" + DateTime.Now.Month + "_Year_" + DateTime.Now.Year + "]";
        }
    }
}
