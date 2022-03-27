using System;

namespace Server
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var server = new ServerConnection();
            server.StartListener();
        }
    }
}