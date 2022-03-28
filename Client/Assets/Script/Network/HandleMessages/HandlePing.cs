using System;
using System.Collections.Generic;
using System.Net.Sockets;
using PackageTypes;
using PackageTypes.Packages;
using UnityEngine;

namespace Server.HandleMessages
{
    public class HandlePingMessage
    {
        private PingAnswer _message;
        
        public HandlePingMessage(PingAnswer data)
        {
            _message = data;
            Debug.Log(data.Data);
        }

        public long Execute()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }
    }
}