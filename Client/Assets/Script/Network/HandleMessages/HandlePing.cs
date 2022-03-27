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
        private PingAnswer Message;
        
        public HandlePingMessage(PingAnswer data)
        {
            Message = data;
            Debug.Log(data.Data);
        }

        public float Execute()
        {
            return DateTime.Now.Millisecond;
        }
    }
}