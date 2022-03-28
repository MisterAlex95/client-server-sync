using System;
using Network;
using UnityEngine.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.UI
{
    public class ActionUIHandler : MonoBehaviour
    {
        // UI
        [SerializeField] private Image BackgroundImage;
        private PingUIHandler pingHandler = null;
        private bool receivedAction = false;

        // Internal data
        private long _timestamp;

        private void Start()
        {
            Socket.OnReceivedAction += SocketOnReceivedAction;
            pingHandler = GameObject.FindObjectOfType<PingUIHandler>();
        }

        private void OnDestroy()
        {
            Socket.OnReceivedAction -= SocketOnReceivedAction;
        }

        private void SocketOnReceivedAction(long timestamp)
        {
            // Calculate the difference between the send and the receive
            this._timestamp = timestamp - pingHandler.Latency;
            this.receivedAction = true;
            Debug.Log($"Action at {this._timestamp} received at {new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}");
        }

        private void Update()
        {
            if (this.receivedAction && this._timestamp <= new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds())
            {
                BackgroundImage.material.color = Random.ColorHSV();
                this.receivedAction = false;
            }
        }
    }
}