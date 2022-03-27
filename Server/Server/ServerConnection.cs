using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class Connection
    {
        public int Port;
        public string Hostname;
    }

    public class ServerConnection : IServer
    {
        private const int ListenPort = 11001;
        private Dictionary<string, Connection> clients = new Dictionary<string, Connection>();

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

        private void SendUdpCallback(IAsyncResult result)
        {
        }

        private void ReadUdpCallback(IAsyncResult result)
        {
            var socket = (UdpClient) result.AsyncState;
            var source = new IPEndPoint(0, 0);
            var message = socket.EndReceive(result, ref source);

            Console.WriteLine("Got {0} from {1}", message.Length.ToString(), source);
            
            if (!clients.ContainsKey(source.ToString()))
            {
                var connectionData = source.ToString().Split(':');
                clients[source.ToString()] = new Connection
                {
                    Hostname = connectionData[0],
                    Port = int.Parse(connectionData[1]) 
                };
                Console.WriteLine("Register new client : {0}", source);
            }

            PackageHandler.ParsePackage(message, socket, source.ToString(), clients);

            socket.BeginReceive(ReadUdpCallback, socket);
        }
    }
}