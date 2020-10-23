using System;

namespace SharedNetworking.Utils
{
    class DataParser
    {
        /// <summary>
        /// constructs a message with the payload and messageId
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="messageId"></param>
        /// <returns>the message ready for sending</returns>
        private static byte[] getMessage(byte[] payload, byte messageId)
        {
            byte[] res = new byte[payload.Length + 5];

            Array.Copy(BitConverter.GetBytes(payload.Length + 5), 0, res, 0, 4);
            res[4] = messageId;
            Array.Copy(payload, 0, res, 5, payload.Length);

            return res;
        }


        public static byte[] getLineMessage(byte[] line)
        {
            return getMessage(line, 0x01);
        }


    }
}
