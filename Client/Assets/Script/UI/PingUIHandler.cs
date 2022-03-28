using System;
using System.Collections;
using Network;
using UnityEngine.UI;
using UnityEngine;

namespace Script.UI
{
    public class PingUIHandler : MonoBehaviour
    {
        // Network Data
        [SerializeField] private Socket socket;

        // UI
        [SerializeField] private Text Ping;

        // Internal data
        private long _timestamp;
        public long Latency { get; private set; }

        private void Start()
        {
            Socket.OnReceivedPing += SocketOnReceivedPing;

            StartCoroutine(LatencyCoroutine());
        }

        private void OnDestroy()
        {
            Socket.OnReceivedPing -= SocketOnReceivedPing;
        }

        private IEnumerator LatencyCoroutine()
        {
            for (;;)
            {
                yield return new WaitForSeconds(1f);

                // Reset timestamp
                this._timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                Debug.Log($"Send ping at {this._timestamp}");
                socket.CalculateLatency();
            }
        }

        private void SocketOnReceivedPing(long newTimestamp)
        {
            // Calculate the difference between the send and the receive
            this.Latency = newTimestamp - this._timestamp;
            Debug.Log($"Receive ping at {newTimestamp}");
            Debug.Log($"Latency at {this.Latency}");
            Ping.text = this.Latency.ToString();
        }
    }
}