using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SharedNetworking.Utils
{
    class SharedClient
    {
        private TcpClient client;
        private NetworkStream stream;
        private byte[] buffer = new byte[1024];
        private byte[] totalBuffer = new byte[1024];
        private int totalBufferReceived = 0;
        private DataHandler dataHandler;

        public SharedClient(TcpClient client, DataHandler dataHandler)
        {
            if (!client.Connected)
            {
                throw new ArgumentException("TcpClient is not connected!");
            }
            this.client = client;
            this.stream = client.GetStream();
            this.stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
            this.dataHandler = dataHandler;
        }

        private void OnRead(IAsyncResult ar)
        {
            int receivedBytes = this.stream.EndRead(ar);

            if (totalBufferReceived + receivedBytes > buffer.Length)
            {
                throw new OutOfMemoryException("buffer too small");
            }
            Array.Copy(buffer, 0, totalBuffer, totalBufferReceived, receivedBytes);
            totalBufferReceived += receivedBytes;

            ushort expectedMessageLength = BitConverter.ToUInt16(totalBuffer, 0);
            while (totalBufferReceived >= expectedMessageLength)
            {
                //volledig packet binnen
                byte[] messageBytes = new byte[expectedMessageLength];
                Array.Copy(totalBuffer, 0, messageBytes, 0, expectedMessageLength);


                //byte[] payloadbytes = new byte[BitConverter.ToInt32(messageBytes, 0) - 5];

                //Array.Copy(messageBytes, 5, payloadbytes, 0, payloadbytes.Length);
                dataHandler.Invoke(messageBytes);
            }

            this.stream.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(OnRead), null);
        }

    }

    public delegate void DataHandler(byte[] bytes);
}
