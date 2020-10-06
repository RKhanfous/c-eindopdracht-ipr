using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    interface IClient
    {
        void SendBytes(byte[] bytes);
    }
}
