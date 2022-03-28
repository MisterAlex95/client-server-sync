using System;
using System.Collections.Generic;
using System.Net.Sockets;
using PackageTypes;
using PackageTypes.Packages;
using UnityEngine;

namespace Server.HandleMessages
{
    public class HandleActionMessage
    {
        private ActionAnswer _message;
        
        public HandleActionMessage(ActionAnswer data)
        {
            _message = data;
            Debug.Log(data.Timestamp.ToString());
        }

        public long Execute()
        {
            return _message.Timestamp;
        }
    }
}