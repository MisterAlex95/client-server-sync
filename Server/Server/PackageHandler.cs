using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using PackageTypes;
using Server.HandleMessages;

namespace Server
{
    public static class PackageHandler
    {
        public static void ParsePackage(
            byte[] bytes,
            UdpClient socket,
            string connectionString,
            Dictionary<string, Connection> clients)
        {
            // this is server side
            using (var stream = new MemoryStream(bytes))
            {
                Int32 messageSize;

                // if we have a valid package do stuff
                // this loops until there isnt enough data for a package or empty
                while (stream.HasValidPackage(out messageSize))
                {
                    byte[] buffer;

                    switch (stream.UnPackMessage(messageSize, out buffer))
                    {
                        case PackageTypes.PackageTypes.HandCheckMessage:
                            var handCheck = buffer.DeserializeFromBytes<PackageTypes.Packages.HandCheck>();
                            Console.WriteLine(handCheck.Timestamp);
                            break;

                        case PackageTypes.PackageTypes.PingMessage:
                            var ping = buffer.DeserializeFromBytes<PackageTypes.Packages.PingMessage>();
                            var pingHandler = new HandlePing(ping);
                            pingHandler.ExecuteOne(socket, clients[connectionString]);
                            break;

                        case PackageTypes.PackageTypes.ActionMessage:
                            var action = buffer.DeserializeFromBytes<PackageTypes.Packages.Action>();
                            Console.WriteLine(action.Timestamp);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}