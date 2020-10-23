using System;
using System.Collections.Generic;
using System.Text;

namespace WpfClient.Utils
{
    interface IClientCallback
    {
        void GoToGameView(string item1);
        void GoToRoomView(string item1);
    }
}
