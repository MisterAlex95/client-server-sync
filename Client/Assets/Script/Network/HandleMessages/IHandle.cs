using System.Collections.Generic;
using System.Net.Sockets;

namespace Server.HandleMessages
{
    public interface IHandler
    {
        public void ExecuteOne(UdpClient socket);
    }
}