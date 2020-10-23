using System;
using System.Net.Sockets;

namespace SharedNetworking.Utils
{
    public abstract class SharedClient
    {
        protected TcpClient client;
        protected NetworkStream stream;
        private byte[] buffer = new byte[1024];
        private byte[] totalBuffer = new byte[1024];
        private int totalBufferReceived = 0;

        public SharedClient(TcpClient client)
        {
            if (!client.Connected)
            {
                throw new ArgumentException("TcpClient is not connected!");
            }
            this.client = client;
            this.stream = client.GetStream();
            this.stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }

        private void OnRead(IAsyncResult ar)
        {
            if (ar == null || (!ar.IsCompleted) || (!this.stream.CanRead) || (!this.client.Client.Connected))
                return;
            int receivedBytes = this.stream.EndRead(ar);

            if (totalBufferReceived + receivedBytes > buffer.Length)
            {
                throw new OutOfMemoryException("buffer too small");
            }
            Array.Copy(buffer, 0, totalBuffer, totalBufferReceived, receivedBytes);
            totalBufferReceived += receivedBytes;

            int expectedMessageLength = BitConverter.ToInt32(totalBuffer, 0);
            while (totalBufferReceived >= expectedMessageLength)
            {
                //volledig packet binnen
                byte[] messageBytes = new byte[expectedMessageLength];
                Array.Copy(totalBuffer, 0, messageBytes, 0, expectedMessageLength);


                //byte[] payloadbytes = new byte[BitConverter.ToInt32(messageBytes, 0) - 5];

                //Array.Copy(messageBytes, 5, payloadbytes, 0, payloadbytes.Length);
                HandleData(messageBytes);


                totalBufferReceived -= expectedMessageLength;
                expectedMessageLength = BitConverter.ToInt32(totalBuffer, 0);
            }

            this.stream.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(OnRead), null);
        }

        protected void sendMessage(byte[] message)
        {
            this.stream.BeginWrite(message, 0, message.Length, new AsyncCallback(OnWrite), null);
        }
        private void OnWrite(IAsyncResult ar)
        {
            this.stream.EndWrite(ar);
        }

        protected abstract void HandleData(byte[] messageBytes);

    }
}
