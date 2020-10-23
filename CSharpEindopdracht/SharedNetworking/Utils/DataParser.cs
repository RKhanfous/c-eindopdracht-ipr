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
        public const string OWN_DATA = "OWNDATA";
        public const string START_GAME = "START";
        public const string SET_DRAWER = "DRAWER";
        public const string WORD = "WORD";
        public const string CLEAR_LINES = "CLEARLINES";
        public const string GUESS = "GUESS";
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

        public static byte[] GetLineMessage(byte[] line)
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

        //==============================================================================================================================================
        public static byte[] GetOwnDataMessage(string mUsername, uint mId)
        {
            return getJsonMessage(OWN_DATA, new { username = mUsername, id = mId });
        }

        public static (string, uint) GetUsernameIdFromJsonMessage(byte[] payload)
        {
            dynamic json = JsonConvert.DeserializeObject(Encoding.ASCII.GetString(payload));
            if (json.identifier == OWN_DATA)
            {
                return (json.data.username, json.data.id);
            }
            return default;
        }

        //==============================================================================================================================================
        public static byte[] GetStartMessage()
        {
            return getJsonMessage(START_GAME, null);
        }

        //==============================================================================================================================================
        public static byte[] GetDrawerMessage(uint Id)
        {
            return getJsonMessage(SET_DRAWER, new { id = Id });
        }

        public static uint GetDrawerId(byte[] payload)
        {
            dynamic json = JsonConvert.DeserializeObject(Encoding.ASCII.GetString(payload));
            if (json.identifier == SET_DRAWER)
            {
                return json.data.id;
            }
            return default;
        }

        //==============================================================================================================================================
        public static byte[] GetWordMessage(string Word)
        {
            return getJsonMessage(WORD, new { word = Word });
        }

        public static string GetWordFromjsonMessage(byte[] payload)
        {
            dynamic json = JsonConvert.DeserializeObject(Encoding.ASCII.GetString(payload));
            if (json.identifier == WORD)
            {
                return json.data.word;
            }
            return default;
        }

        //==============================================================================================================================================
        public static byte[] GetClearLinesMessage()
        {
            return getJsonMessage(CLEAR_LINES, null);
        }

        //==============================================================================================================================================
        public static byte[] GetGuessMessage(string Guess)
        {
            return getJsonMessage(GUESS, new { guess = Guess });
        }

        public static string GetGuessFromjsonMessage(byte[] payload)
        {
            dynamic json = JsonConvert.DeserializeObject(Encoding.ASCII.GetString(payload));
            if (json.identifier == GUESS)
            {
                return json.data.guess;
            }
            return default;
        }

        public static byte[] GetGuessScoreMessage(int GuessScore)
        {
            return getJsonMessage(GUESS, new { guessScore = GuessScore });
        }

        public static int GetGuessScoreFromjsonMessage(byte[] payload)
        {
            dynamic json = JsonConvert.DeserializeObject(Encoding.ASCII.GetString(payload));
            if (json.identifier == GUESS)
            {
                return json.data.guessScore;
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
