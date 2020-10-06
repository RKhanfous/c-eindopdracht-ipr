using Newtonsoft.Json;
using SharedSkribbl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Networking.Utils
{
    public static class DataParser
    {
        private static byte[] WrapMessageWithPrefix(byte[] payload, byte messageId)
        {
            byte[] result = new byte[payload.Length + 3];

            Array.Copy(BitConverter.GetBytes(payload.Length + 3), 0, result, 0, 2);
            result[4] = messageId;
            Array.Copy(payload, 0, result, 3, payload.Length);

            return result;
        }

        public static byte[] GetLineMessage(Line line)
        {
            return WrapMessageWithPrefix(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(line)), 0x01);
        }

        public static Line getLineFromPayload(byte[] payload)
        {
            return JsonConvert.DeserializeObject<Line>(Encoding.ASCII.GetString(payload));
        }
    }
}
