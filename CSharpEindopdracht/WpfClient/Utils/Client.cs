using SharedNetworking.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace WpfClient.Utils
{
    class Client : SharedClient
    {
        private string username { get; set; }
        private IClientCallback clientCallback;


        public Client(TcpClient tcpClient, IClientCallback clientCallback) : base(tcpClient)
        {
            this.clientCallback = clientCallback;
        }


        protected override void HandleData(byte[] messageBytes)
        {
            byte[] payload = messageBytes.Skip(5).ToArray();

            byte messageId = messageBytes[4];
            switch (messageId)
            {
                case 0x01:
                    throw new NotImplementedException();
                    break;

                case 0x02:
                    string identifier;
                    bool worked = DataParser.getJsonIdentifier(messageBytes, out identifier);
                    if (!worked)
                        throw new Exception("couldn't get identifier from json");

                    switch (identifier)
                    {
                        case DataParser.GO_TO_ROOM:
                            (string, bool) roomData = DataParser.GetRoomDataFromGoToRoomjson(payload);
                            if (roomData.Item1 == null)
                                throw new Exception("somehow didn't find a room");

                            if (roomData.Item2)
                                this.clientCallback.GoToGameView(roomData.Item1);
                            else
                                this.clientCallback.GoToRoomView(roomData.Item1);
                            break;

                        default:
                            Console.WriteLine($"Received json with identifier {identifier}");
                            break;
                    }
                    break;

                default:
                    break;

            }
        }



        public bool Connected()
        {
            return this.client.Client.Connected;
        }

        internal void LogOn(string username, string roomCode)
        {
            this.username = username;
            SendMessage(DataParser.GetLogOnJsonMessage(username, roomCode));
        }
    }
}
