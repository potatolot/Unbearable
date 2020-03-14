using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

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

	List<string> rooms;
	List<int> roomcount;

    // Everything that has to do with the monobehavior
    #region MonoBehaviour
    private void Start()
    {
		rooms = new List<string>();
		roomcount = new List<int>();
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

    // Function to communicate connection status and recieve data
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
                Debug.Log(string.Format("User {0} has connected through host {1}", connectionID, recHostID));
                break;

            case NetworkEventType.DisconnectEvent:
                Debug.Log(string.Format(" User {0} has disconnected :(", connectionID));
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

            case NetOP.JoinGame:
                JoinGame(connectionID, channelID, recHostID, (Net_JoinGame)msg);
                break;

            case NetOP.ReadyStatus:
                ReadyStatus(connectionID, channelID, recHostID, (Net_ReadyStatus)msg);
                break;
        }
    }

    private void JoinGame(int connectionID, int channelID, int recHostID, Net_JoinGame jg)
    {
        Debug.Log(string.Format("user: {0}, room: {1}", jg.Username, jg.Roomcode));

        Net_OnJoinGame ojg = new Net_OnJoinGame();
        ojg.Success = 0;
        ojg.Information = "Game Joined";
        ojg.Roomcode = jg.Roomcode;
        ojg.Token = "TOKEN";

		//rooms: 
		//1 2 1 2 2
		//loop 1 roomcount:
		//1
		//loop 2 roomcount:
		//0 1
		//loop 3 roomcount:
		//1 0 1
		//loop 4 roomcount:
		//0 1 0 1
		//loop 5 roomcount:

		rooms.Add(jg.Roomcode);

		if (roomcount.Count == 0)
		{
			roomcount.Add(1);
		}
		else
		{
			roomcount.Add(0);
		}

		for (int i = 0; i <= rooms.Count; i++)
		{
			if (rooms[i] == jg.Roomcode)
			{
				roomcount[i]++; //every item in roomcount now has the number of times it appears in rooms in the same order as rooms
			}
		}

		for (int i = 0; i <=roomcount.Count; i++)
		{ ojg.Playerslot += roomcount[i]; }

		SendClient(recHostID, connectionID, ojg);
    }

    private void ReadyStatus(int connectionID, int channelID, int recHostID, Net_ReadyStatus rs)
    {
        Net_OnReadyStatus ors = new Net_OnReadyStatus();
        ors.Username = rs.Username;
        ors.Status = rs.Status;

        SendClient(recHostID, connectionID, ors);
    }
    #endregion

    #region Send
    public void SendClient(int recHost, int connectionID, NetMsg msg)
    {
        //data holder
        byte[] buffer = new byte[BYTE_SIZE];

        //data crusher into byte array
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(buffer);
        formatter.Serialize(ms, msg);

        if (recHost == 0)
        {
            NetworkTransport.Send(hostID, connectionID, reliableChannel, buffer, BYTE_SIZE, out error);
        }
        else
        {
            NetworkTransport.Send(webHostID, connectionID, reliableChannel, buffer, BYTE_SIZE, out error);
        }
    }
    #endregion
}
