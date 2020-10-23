using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    interface IClient
    {
        uint ClientId { get; set; }

        void SendMessage(byte[] bytes);
    }
}
