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

        private string fileName { get; set; }

        public Logger(string logPathApp)
        {
            this.logPath = logPathApp;
            this.logName = "SystemLog" + StringBuilderTime() + ".txt";
            this.fileName = logPath + @"\" + logName;
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
        }

        public string logException(string exception)
        {
            writeTextToFile("[" + DateTime.Now + "] this exception has occured: " + exception + "!");
            return exception;
        }

        public void logServer()
        {
            writeTextToFile("[" + DateTime.Now + "] server started!");
        }

        public void logStartGame(Player player)
        {
            writeTextToFile($"[{DateTime.Now}] roomCode: {player.playingInRoom.roomCode} player: {player.username} with clientID: {player.clientID} started game");
        }

        public void logConnectClient(int clientID)
        {
            writeTextToFile("[" + DateTime.Now + "]" + clientID + " connected to the server!");
        }

        public void logDisconnectClient(Client client)
        {
            writeTextToFile("[" + DateTime.Now + "]" + client.ClientId + " left the server!");
        }
        public void LogGameOver(Player player)
        {
            writeTextToFile($"[{DateTime.Now}] roomcode: {player.playingInRoom.roomCode}  player: {player.username} with clientID: {player.clientID} game over");
        }

        public void logNewPlayer(Player newPlayer)
        {
            writeTextToFile($"[{DateTime.Now}] roomcode: {newPlayer.playingInRoom.roomCode}  newPlayer: {newPlayer.username} with clientID: {newPlayer.clientID} joined room");
        }

        private string StringBuilderTime()
        {
            return "[" + DateTime.Now.Hour + "H-" + DateTime.Now.Minute + "M-" + DateTime.Now.Second + "S-Day" + DateTime.Now.Day + "-Month" + DateTime.Now.Month + "-Year" + DateTime.Now.Year + "]";
        }


        public void writeTextToFile(string data)
        {
            using (StreamWriter sw = File.AppendText(this.fileName))
            {
                sw.WriteLine(data);
            }
        }

    }
}
