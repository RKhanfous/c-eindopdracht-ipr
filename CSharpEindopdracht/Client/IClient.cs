using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    interface IClient
    {
        private void SendBytes(byte[] bytes);
    }
}
