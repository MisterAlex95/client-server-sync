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
        private float _timestamp;

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
                this._timestamp = DateTime.Now.Millisecond;
                socket.CalculateLatency();
            }
        }

        private void SocketOnReceivedPing(float timestamp)
        {
            // Calculate the difference between the send and the receive
            this._timestamp = timestamp - this._timestamp;
            Ping.text = this._timestamp.ToString();
        }
    }
}