﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    private byte reliableChannel;
    private int connectionID;
    private int hostID;
    private byte error;

    //Max users using the server
    private const int MAX_USER = 8;
    private const int PORT = 38977;
    private const int WEB_PORT = 38979;
    private const int BYTE_SIZE = 1024;
    private const string SERVER_IP = "127.0.0.1";

    private bool isStarted;

    // Everything that has to do with the monobehavior
    #region MonoBehaviour
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }
    private void Update()
    {
        UpdateMessagePump();
    }
    #endregion

    // Everything that is server related
    public void Init()
    {
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();

        // Reliable will make sure the data will get delivered
        reliableChannel = cc.AddChannel(QosType.Reliable);

        // Blueprint of the server
        HostTopology topo = new HostTopology(cc, MAX_USER);

        // Client only code //

        hostID = NetworkTransport.AddHost(topo, 0);

#if UNITY_WEBGL && !UNITY_EDITOR
        // Web Client
        connectionID = NetworkTransport.Connect(hostID, SERVER_IP, WEB_PORT, 0, out error);
        Debug.Log(string.Format("Connecting from webgl", SERVER_IP));
       
#else
        // Standalone Client
        connectionID = NetworkTransport.Connect(hostID, SERVER_IP, PORT, 0, out error);
        Debug.Log(string.Format("Connecting from standalone", SERVER_IP));

#endif

        Debug.Log(string.Format("Attempting to connect on {0}...", SERVER_IP));
        
        isStarted = true;
    }

    // Function to shutdown the server
    public void Shutdown()
    {
        isStarted = false;
        NetworkTransport.Shutdown();
    }

    // Function to communicate connection status
    public void UpdateMessagePump()
    {
        if (!isStarted)
            return;

        int recHostID;      // Is this from the web or standalone
        int connectionID;   // Which user is sending me this?
        int channelID;      // Which lane is he sending that message from

        byte[] recBuffer = new byte[BYTE_SIZE]; // Received information
        int dataSize;

        NetworkEventType type = NetworkTransport.Receive(out recHostID, out connectionID, out channelID, recBuffer, BYTE_SIZE, out dataSize, out error);
        switch (type)
        {
            case NetworkEventType.Nothing:
                break;

            case NetworkEventType.ConnectEvent:
                Debug.Log("Connected to the server has been established");
                break;

            case NetworkEventType.DisconnectEvent:
                Debug.Log("Connection to the server has been broken (by server)");
                break;

            case NetworkEventType.DataEvent:
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(recBuffer);
                NetMsg msg = (NetMsg)formatter.Deserialize(ms);

                OnData(connectionID, channelID, recHostID, msg);
                break;

            default:
            case NetworkEventType.BroadcastEvent:
                Debug.Log("Unexpected network event type");
                break;
        }
    }

    #region OnData
    private void OnData(int connectionID, int channelID, int recHostID, NetMsg msg)
    {
        switch (msg.OperationCode)
        {
            case NetOP.None:
                Debug.Log("Unexpected NETOP");
                break;
        }
    }
    #endregion

    #region Send
    public void SendServer(NetMsg msg)
    {
        //data holder
        byte[] buffer = new byte[BYTE_SIZE];

        //data crusher into byte array
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(buffer);
        formatter.Serialize(ms, msg);

        NetworkTransport.Send(hostID, connectionID, reliableChannel, buffer, BYTE_SIZE, out error);
    }
	#endregion

    public void TESTFUNCTIONCREATEACCOUNT()
    {
        Net_CreateAccount ca = new Net_CreateAccount();

        ca.Username = "Swag";
        ca.Password = "Yolo";
        ca.Email = "Bruh";

        SendServer(ca);
    }
}
