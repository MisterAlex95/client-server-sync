using System;

namespace Server
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IServer server = new ServerConnection();
            server.StartListener();
            Console.ReadKey();
        }
    }
}