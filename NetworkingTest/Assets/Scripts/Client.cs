using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    private byte reliableChannel;
    private int hostID;
    private byte error;

    //Max users using the server
    private const int MAX_USER = 8;
    private const int PORT = 38977;
    private const int WEB_PORT = 38979;
    private const string SERVER_IP = "127.0.0.1";

    private bool isStarted;

    // Everything that has to do with the monobehavior
    #region MonoBehaviour
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
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
        NetworkTransport.Connect(hostID, SERVER_IP, WEB_PORT, 0, out error);
        Debug.Log(string.Format("Connecting from webgl", SERVER_IP));
       
#else
        // Standalone Client
        NetworkTransport.Connect(hostID, SERVER_IP, PORT, 0, out error);
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
}
