using System;
using System.IO;
using PackageTypes;

namespace Server
{
    public static class PackageHandler
    {
        public static void ParsePackage(byte[] bytes)
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
                        case PackageTypes.PackageTypes.Ping:
                            var ping = buffer.DeserializeFromBytes<PackageTypes.Packages.Ping>();
                            Console.WriteLine(ping.Data);
                            break;
                        case PackageTypes.PackageTypes.HandCheck:
                            var handCheck = buffer.DeserializeFromBytes<PackageTypes.Packages.HandCheck>();
                            Console.WriteLine(handCheck.Data);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}