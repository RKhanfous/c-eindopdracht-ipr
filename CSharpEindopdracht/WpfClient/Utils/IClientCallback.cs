using SharedNetworking.Utils;
using SharedSkribbl;
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
        void SetDrawer(uint id);
        public string currentWord { get; set; }

        void AddLine(Line line);
        void ClearLines();
    }
}
