using System;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class ServerConnection : IServer
    {
        private const int ListenPort = 11001;

        public void StartListener()
        {
            var socket = new UdpClient(ListenPort);

            try
            {
                Console.WriteLine("Waiting for a connexion...");
                Console.WriteLine("Press any key to close the server...");

                socket.BeginReceive(ReadUdpCallback, socket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void SendUdpCallback(IAsyncResult result)
        {
            
        }
        
        private static void ReadUdpCallback(IAsyncResult result)
        {
            var socket = (UdpClient) result.AsyncState;
            var source = new IPEndPoint(0, 0);
            var message = socket.EndReceive(result, ref source);

            Console.WriteLine("Got {0} from {1}", message.Length.ToString(), source);

            PackageHandler.ParsePackage(message);
            
            socket.BeginReceive(ReadUdpCallback, socket);
        }
    }
}