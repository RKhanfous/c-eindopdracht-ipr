
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
    public class Client : SharedClient
    {
        private NetworkHandler networkHandler;
        private string username { get; set; }
        public int clientID { get; set; }

        public Client(TcpClient tcpClient, NetworkHandler network, int clientID) : base(tcpClient)
        {
            this.clientID = clientID;
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

        protected override void HandleData(byte[] messageBytes)
        {
            Console.WriteLine("Got a packet: " + messageBytes);

            byte[] payload = messageBytes.Skip(5).ToArray();

            string packet = Encoding.ASCII.GetString(payload);

            byte messageId = messageBytes[4];
            switch (messageId)
            {
                case 0x01:
                    networkHandler.drawLine(clientID, messageBytes);
                    break;

                case 0x02:
                    string identifier;
                    bool worked = DataParser.getJsonIdentifier(messageBytes, out identifier);
                    if (!worked)
                        throw new Exception("couldn't get identifier from json");
                    switch (identifier)
                    {
                        case DataParser.LOGON:
                            username = DataParser.getUsernameFromLogOnjson(payload);
                            if (username == null)
                                throw new Exception("couldn't get username from json");
                            Console.WriteLine($"received username {username}");
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

        public byte[] addByteToArray(byte[] bArray, byte newByte)
        {
            byte[] newArray = new byte[bArray.Length + 1];
            bArray.CopyTo(newArray, 1);
            newArray[bArray.Length + 1] = newByte;
            return newArray;
        }
    }
}
