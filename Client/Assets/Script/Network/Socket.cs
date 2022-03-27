using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

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

        client.BeginReceive(new AsyncCallback(ProcessDgram), client);
    }

    public void SendDgram(string msg)
    {
        var dgram = Encoding.UTF8.GetBytes(msg);

        Debug.Log($"Send new : {msg}");

        client.Send(dgram, dgram.Length);
    }

    public void ProcessDgram(IAsyncResult res)
    {
        try
        {
            var recieved = client.EndReceive(res, ref hostEndPoint);
            var data = Encoding.UTF8.GetString(recieved);
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