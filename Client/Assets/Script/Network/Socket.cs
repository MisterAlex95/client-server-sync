using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

// Custom import
using PackageTypes;
using PackageTypes.Packages;
using Server.HandleMessages;
using UnityEngine.UI;

namespace Network
{
    public class Socket : MonoBehaviour
    {
        // UI
        [SerializeField] private Text Ping;

        // Network
        public string hostIp;
        public int hostPort;

        private UdpClient _client;
        private IPAddress _serverIp;
        private IPEndPoint _hostEndPoint;

        private float _latency = -1;
        private bool _toUpdate = false;

        void Start()
        {
            // To keep it over scenes
            DontDestroyOnLoad(gameObject);

            _serverIp = IPAddress.Parse(hostIp);
            _hostEndPoint = new IPEndPoint(_serverIp, hostPort);

            _client = new UdpClient();
            _client.Connect(_hostEndPoint);
            _client.Client.Blocking = false;

            _client.BeginReceive(new AsyncCallback(ProcessDgram), _client);
            StartCoroutine(LatencyCoroutine());
        }

        public void CalculateLatency()
        {
            // To calculate the latency
            var message = new PingMessage("ping");
            this._latency = DateTime.Now.Millisecond;
            this._toUpdate = true;
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
                            this._latency -= pingHandler.Execute();
                            this._toUpdate = false;
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

        private IEnumerator LatencyCoroutine()
        {
            for (; ; )
            {
                // yield return new WaitForSeconds(1f);
                CalculateLatency();
            }
        }
        
        private void Update()
        {
            if (_toUpdate)
            {
                Ping.text = _latency.ToString();
            }
        }
    }
}