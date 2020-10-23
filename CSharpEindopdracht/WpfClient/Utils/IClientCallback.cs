using SharedNetworking.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace WpfClient.Utils
{
    interface IClientCallback
    {
        void GoToGameView(string item1);
        void GoToRoomView(string item1);
        void AddPlayer(Player dataPlayer);
        void SetMePlayer(string username, uint id);
    }
}
