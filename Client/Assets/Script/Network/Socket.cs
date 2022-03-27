using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

// Custom import
using PackageTypes;
using Ping = PackageTypes.Packages.Ping;

namespace Network
{
    public class Socket : MonoBehaviour
    {
        // Network
        public string hostIp;
        public int hostPort;

        private UdpClient client;
        private IPAddress serverIp;
        private IPEndPoint hostEndPoint;

        void Start()
        {
            // To keep it over scenes
            DontDestroyOnLoad(gameObject);

            serverIp = IPAddress.Parse(hostIp);
            hostEndPoint = new IPEndPoint(serverIp, hostPort);

            client = new UdpClient();
            client.Connect(hostEndPoint);
            client.Client.Blocking = false;

            SendDgram();
            client.BeginReceive(new AsyncCallback(ProcessDgram), client);
        }

        public void SendDgram()
        {
            var ping = new Ping("ping");
            var bytes = ping.PackMessage(PackageTypes.PackageTypes.Ping);
            client.Send(bytes, bytes.Length);
        }

        private void ProcessDgram(IAsyncResult res)
        {
            try
            {
                var received = client.EndReceive(res, ref hostEndPoint);
                var data = Encoding.UTF8.GetString(received);
                Debug.Log($"Received: {data}");
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }

            // Receive next Message
            client.BeginReceive(new AsyncCallback(ProcessDgram), client);
        }
    }
}