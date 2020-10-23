using SharedNetworking.Utils;
using SharedSkribbl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows;

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


        protected override async void HandleData(byte[] messageBytes)
        {
            //Debug.WriteLine("[wpf client] received packet " + Encoding.ASCII.GetString(messageBytes) + "with length " + messageBytes.Length);
            byte[] payload = messageBytes.Skip(5).ToArray();

            byte messageId = messageBytes[4];
            switch (messageId)
            {
                case 0x01:
                    this.clientCallback.AddLine(Line.GetLine(payload));
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
                        case DataParser.OWN_DATA:
                            (string, uint) playerData = DataParser.GetUsernameIdFromJsonMessage(payload);
                            clientCallback.SetMePlayer(playerData.Item1, playerData.Item2);
                            break;
                        case DataParser.START_GAME:
                            //null should be fine for now
                            this.clientCallback.GoToGameView(null);
                            break;
                        case DataParser.SET_DRAWER:
                            this.clientCallback.SetDrawer(DataParser.GetDrawerId(payload));
                            break;
                        case DataParser.WORD:
                            this.clientCallback.currentWord = DataParser.GetWordFromjsonMessage(payload);
                            Debug.WriteLine("word set to " + this.clientCallback.currentWord);

                            break;
                        case DataParser.CLEAR_LINES:
                            this.clientCallback.ClearLines();
                            break;
                        case DataParser.GUESS:
                            int score = DataParser.GetGuessScoreFromjsonMessage(payload);
                            this.clientCallback.GiveScore(score);
                            break;
                        default:
                            Console.WriteLine($"Received json with identifier {identifier}");
                            break;
                    }
                    break;

                case 0x03:
                    Player dataPlayer = DataParser.GetPlayerFromBytes(payload);

                    clientCallback.AddPlayer(dataPlayer);
                    break;
                default:
                    Debug.WriteLine($"received message with id {messageId}");
                    break;

            }
        }

        internal void AskToStart()
        {
            SendMessage(DataParser.GetStartMessage());
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
