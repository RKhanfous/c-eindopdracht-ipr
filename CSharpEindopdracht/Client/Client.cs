
using SharedNetworking.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Client : SharedClient, IClient
    {
        private NetworkHandler networkHandler;
        private string username { get; set; }
        public uint ClientId { get; set; }

        public Client(TcpClient tcpClient, NetworkHandler network, uint clientID) : base(tcpClient)
        {
            this.ClientId = clientID;
            this.networkHandler = network;
        }

        public void Write(byte messageID, string packet)
        {
            Console.WriteLine("packet: " + packet);

            byte[] lengthPacket = BitConverter.GetBytes(packet.Length);
            lengthPacket = addByteToArray(lengthPacket, messageID);
            var packetMessageAsBytes = Encoding.ASCII.GetBytes(packet);

            byte[] lengthAndMessageID = lengthPacket.Concat(packetMessageAsBytes).ToArray();

            this.stream.BeginWrite(lengthAndMessageID, 0, lengthAndMessageID.Length, new AsyncCallback(onWrite), null);
        }

        private void onWrite(IAsyncResult ar)
        {
            this.stream.EndWrite(ar);
        }

        protected override async void HandleData(byte[] messageBytes)
        {

            byte[] payload = messageBytes.Skip(5).ToArray();

            string packet = Encoding.ASCII.GetString(payload);

            byte messageId = messageBytes[4];
            switch (messageId)
            {
                case 0x01:
                    networkHandler.DrewLine(ClientId, messageBytes);
                    break;

                case 0x02:
                    string identifier;
                    bool worked = DataParser.getJsonIdentifier(messageBytes, out identifier);
                    if (!worked)
                        throw new Exception("couldn't get identifier from json");
                    switch (identifier)
                    {
                        case DataParser.LOG_ON:
                            username = DataParser.GetUsernameFromLogOnjson(payload);
                            if (username == null)
                                throw new Exception("couldn't get username from json");
                            Console.WriteLine($"received username {username}");

                            (string, bool) roomData = networkHandler.Server.GetRoom(username, this.ClientId, DataParser.GetRoomCodeFromLogOnjson(payload));
                            if (roomData.Item1 == null)
                                throw new Exception("should never happen");

                            Player player = networkHandler.Server.GetPlayer(ClientId);
                            SendMessage(DataParser.GetOwnDataMessage(player.username, player.clientID));

                            SendMessage(DataParser.GetGoToRoomMessage(roomData.Item1, roomData.Item2));

                            networkHandler.SendDataToPlayer(this);
                            break;
                        case DataParser.START_GAME:
                            if (this.networkHandler.Server.GetPlayer(ClientId).playingInRoom.Start())
                                SendMessage(DataParser.GetStartMessage());
                            else
                                SendMessage(DataParser.GetGoToRoomMessage(this.networkHandler.Server.GetPlayer(ClientId).playingInRoom.roomCode, false));
                            break;
                        case DataParser.CLEAR_LINES:
                            this.networkHandler.DeleteLines(this.ClientId);

                            break;
                        default:
                            Console.WriteLine($"Received json with identifier {identifier}");
                            break;
                    }
                    break;

                default:
                    Debug.WriteLine($"received message with id {messageId}");
                    break;

            }

        }

        public byte[] addByteToArray(byte[] bArray, byte newByte)
        {
            byte[] newArray = new byte[bArray.Length + 1];
            bArray.CopyTo(newArray, 1);
            newArray[bArray.Length + 1] = newByte;
            return newArray;
        }
    }
}
