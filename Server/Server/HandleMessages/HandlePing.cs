using System;
using System.Collections.Generic;
using System.Net.Sockets;
using PackageTypes;
using PackageTypes.Packages;

namespace Server.HandleMessages
{
    public class HandlePing : IHandler
    {
        private PingMessage Message;

        public HandlePing(PingMessage data)
        {
            Message = data;
        }

        public void ExecuteOne(UdpClient socket, Connection client)
        {
            var answer = new PingAnswer("pong");
            var data = answer.PackMessage<PingAnswer>(PackageTypes.PackageTypes.PingAnswer);
            Console.WriteLine($"Try to send a message to {0}:{1}", client.Hostname, client.Port.ToString());
            socket.Send (data, data.Length, client.Hostname, client.Port);
        }

        public void ExecuteAll(UdpClient socket, Dictionary<string, Connection> clients)
        {
            var answer = new PingAnswer("pong");
            var data = answer.PackMessage<PingAnswer>(PackageTypes.PackageTypes.PingAnswer);
            socket.Send(data, data.Length);
        }
    }
}