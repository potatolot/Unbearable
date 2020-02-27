using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private byte reliableChannel;

    private int hostID;
    private int webHostID;

    //Max users using the server
    private const int MAX_USER = 8;
    private const int PORT = 38977;
    private const int WEB_PORT = 38979;
    private const int BYTE_SIZE = 1024;

    private bool isStarted;
    private byte error;

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

        // Server only code //

        hostID = NetworkTransport.AddHost(topo, PORT, null);
        webHostID = NetworkTransport.AddWebsocketHost(topo, WEB_PORT, null);


        Debug.Log(string.Format("Opening connection on port {0} and webport {1}", PORT, WEB_PORT));
        isStarted = true;

    }

    // Function to shutdown the server
    public void Shutdown()
    {
        isStarted = false;
        NetworkTransport.Shutdown();
    }

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
                Debug.Log(string.Format("User {0} has connected!", connectionID));
                break;

            case NetworkEventType.DisconnectEvent:
                Debug.Log(string.Format(" User {0} has disconnected :(", connectionID));
                break;

            case NetworkEventType.DataEvent:
                Debug.Log("Data");
                break;

            default:
            case NetworkEventType.BroadcastEvent:
                Debug.Log("Unexpected network event type");
                break;
        }

    }
}
