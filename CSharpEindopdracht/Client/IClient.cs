using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    interface IClient
    {
        int ClientId { get; set; }

        void SendMessage(byte[] bytes);
    }
}
