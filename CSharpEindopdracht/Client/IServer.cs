using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    interface IServer
    {
        void GiveBytes(byte[] bytes);
    }
}
