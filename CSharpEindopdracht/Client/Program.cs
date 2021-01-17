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

            //Server server = new Server();

            /*while (true)
            {
                Thread.Yield();
            }*/

            string filepath = @"Words\GameWords.txt";
            HashSet<String> words = new HashSet<string>();
            foreach (string word in File.ReadAllLines(filepath))
            {
                words.Add(word);
            }

            foreach(string word in words)
            {
                Console.WriteLine(word);
            }
        }
    }
}
