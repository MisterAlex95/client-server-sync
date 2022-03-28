using System;
using System.Collections.Generic;
using System.Net.Sockets;
using PackageTypes;
using PackageTypes.Packages;

namespace Server.HandleMessages
{
    public class HandleAction : IHandler
    {
        private ActionMessage Message;

        public HandleAction(ActionMessage data)
        {
            Message = data;
        }

        public void ExecuteOne(UdpClient socket, Connection client)
        {
            var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            var answer = new ActionAnswer((timestamp + 3));
            var data = answer.PackMessage<ActionAnswer>(PackageTypes.PackageTypes.ActionAnswer);
            Console.WriteLine($"Try to send a message to {0}:{1}", client.Hostname, client.Port.ToString());
            socket.Send (data, data.Length, client.Hostname, client.Port);
        }

        public void ExecuteAll(UdpClient socket, Dictionary<string, Connection> clients)
        {
            var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            var answer = new ActionAnswer((timestamp + 3));
            var data = answer.PackMessage<ActionAnswer>(PackageTypes.PackageTypes.ActionAnswer);

            foreach (var client in clients)
            {
                socket.Send(data, data.Length, client.Value.Hostname, client.Value.Port);
            }
        }
    }
}