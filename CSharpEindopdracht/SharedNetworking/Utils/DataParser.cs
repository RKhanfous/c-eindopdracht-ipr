using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace SharedNetworking.Utils
{
    class DataParser
    {

        #region consts

        public const string LOG_ON = "LOGON";
        public const string GO_TO_ROOM = "GOTOROOM";
        #endregion


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


        #region lines

        public static byte[] getLineMessage(byte[] line)
        {
            return getMessage(line, 0x01);
        }

        #endregion

        #region json with identifier

        /// <summary>
        /// get the identifier from json
        /// </summary>
        /// <param name="bytes">json in ASCII</param>
        /// <param name="identifier">gets the identifier</param>
        /// <returns>if it sucseeded</returns>
        public static bool getJsonIdentifier(byte[] bytes, out string identifier)
        {
            if (bytes.Length <= 5)
            {
                throw new ArgumentException("bytes to short");
            }
            byte messageId = bytes[4];

            if (messageId == 0x02)
            {
                dynamic json = JsonConvert.DeserializeObject(Encoding.ASCII.GetString(bytes.Skip(5).ToArray()));
                identifier = json.identifier;
                return true;
            }
            else
            {
                identifier = "";
                return false;
            }
        }

        private static byte[] getJsonMessage(string mIdentifier, dynamic data)
        {
            dynamic json = new
            {
                identifier = mIdentifier,
                data
            };
            return getMessage(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(json)), 0x02);
        }

        //==============================================================================================================================================
        public static byte[] GetLogOnJsonMessage(string mUsername, string mRoomCode)
        {
            return getJsonMessage(LOG_ON, new { username = mUsername, roomCode = mRoomCode });
        }

        public static string GetUsernameFromLogOnjson(byte[] payload)
        {
            dynamic json = JsonConvert.DeserializeObject(Encoding.ASCII.GetString(payload));
            if (json.identifier == LOG_ON)
            {
                return json.data.username;
            }
            return null;
        }

        public static string GetRoomCodeFromLogOnjson(byte[] payload)
        {
            dynamic json = JsonConvert.DeserializeObject(Encoding.ASCII.GetString(payload));
            if (json.identifier == LOG_ON)
            {
                return json.data.roomCode;
            }
            return null;
        }

        //==============================================================================================================================================

        public static byte[] GetGoToRoomMessage(string mRoomCode, bool mRunning)
        {
            return getJsonMessage(GO_TO_ROOM, new { roomCode = mRoomCode, running = mRunning });
        }

        public static (string, bool) GetRoomDataFromGoToRoomjson(byte[] payload)
        {
            dynamic json = JsonConvert.DeserializeObject(Encoding.ASCII.GetString(payload));
            if (json.identifier == GO_TO_ROOM)
            {
                return (json.data.roomCode, json.data.running);
            }
            return default;
        }

        #endregion

        #region players

        public static byte[] GetPlayerMessage(Player player)
        {
            return getMessage(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(player)), 0x03);
        }

        public static Player GetPlayerFromBytes(byte[] payload)
        {
            return JsonConvert.DeserializeObject<Player>(Encoding.ASCII.GetString(payload));
        }

        #endregion
    }
}
