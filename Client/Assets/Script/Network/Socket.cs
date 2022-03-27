using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

// Custom import
using PackageTypes;
using PackageTypes.Packages;
using Server.HandleMessages;

namespace Network
{
    [RequireComponent(typeof(JobManager))]
    public class Socket : MonoBehaviour
    {
        // Network
        public string hostIp;
        public int hostPort;

        private UdpClient _client;
        private IPAddress _serverIp;
        private IPEndPoint _hostEndPoint;

        // Action linked to packages
        public static event Action<float> OnReceivedPing;
        private JobManager _jobManager;
        private object asyncLock = new object();

        void Start()
        {
            // To keep it over scenes
            DontDestroyOnLoad(gameObject);

            lock (asyncLock)
            {
                _jobManager = GetComponent<JobManager>();
            }

            _serverIp = IPAddress.Parse(hostIp);
            _hostEndPoint = new IPEndPoint(_serverIp, hostPort);

            _client = new UdpClient();
            _client.Connect(_hostEndPoint);
            _client.Client.Blocking = false;

            _client.BeginReceive(new AsyncCallback(ProcessDgram), _client);
        }

        public void CalculateLatency()
        {
            // To calculate the latency
            var message = new PingMessage("ping");
            SendDgram(message, PackageTypes.PackageTypes.PingMessage);
        }

        private void SendDgram(IPackage package, PackageTypes.PackageTypes type)
        {
            var bytes = package.PackMessage(type);
            _client.SendAsync(bytes, bytes.Length);
        }

        private void ProcessDgram(IAsyncResult res)
        {
            try
            {
                var received = _client.EndReceive(res, ref _hostEndPoint);
                this.ParsePackage(received);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            // Receive next Message
            _client.BeginReceive(new AsyncCallback(ProcessDgram), _client);
        }

        private void ParsePackage(byte[] bytes)
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
                        case PackageTypes.PackageTypes.HandCheckAnswer:
                            var handCheck = buffer.DeserializeFromBytes<PackageTypes.Packages.HandCheck>();
                            Console.WriteLine(handCheck.Timestamp);
                            break;

                        case PackageTypes.PackageTypes.PingAnswer:
                            var ping = buffer.DeserializeFromBytes<PackageTypes.Packages.PingAnswer>();
                            var pingHandler = new HandlePingMessage(ping);
                            var value = pingHandler.Execute();

                            lock (asyncLock)
                            {
                                _jobManager.AddAction(() => OnReceivedPing?.Invoke(value));
                            }

                            break;

                        case PackageTypes.PackageTypes.ActionAnswer:
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