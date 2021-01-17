using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Server
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("server started");

            Server server = new Server();

            while (true)
            {
                Thread.Yield();
            }
        }
    }
}
