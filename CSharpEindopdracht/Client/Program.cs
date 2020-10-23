using System;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
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
