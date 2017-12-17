using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class MyClientBehaviour : MonoBehaviour {

    [SerializeField]
    private Text DebugText;

    private NetworkClient Client;


    public class MyMsgType
    {
        public static short POIUpdate = MsgType.Highest + 1;
    };

    public class POIUpdateMessage : MessageBase
    {
        public int ID;
    }

    public void SetClient(NetworkClient client)
    {
        Client = client;
        Client.RegisterHandler(MsgType.Connect, OnConnected);
        Client.RegisterHandler(MyMsgType.POIUpdate, OnPOIUpdate);
        Client.RegisterHandler(MsgType.Error, OnError);

        DebugText.text = "Was Set";

    }

    public void OnError(NetworkMessage msg)
    {
    }

    public void SendPOIUpdate(int id)
    {
        POIUpdateMessage msg = new POIUpdateMessage();
        msg.ID = id;
        NetworkServer.SendToAll(MyMsgType.POIUpdate, msg);
    }

    public void OnPOIUpdate(NetworkMessage netMsg)
    {
        POIUpdateMessage msg = netMsg.ReadMessage<POIUpdateMessage>();
        Debug.Log("ID " + msg.ID);
        DebugText.text = "ID " + msg.ID;
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void OnConnected(NetworkMessage msg)
    {
        Debug.Log("Connected to Server");
        DebugText.text = "Connected to Server";
    }

    private void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player connected to Server");
        
    }

    private void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.Log("Network Connection Error");
        DebugText.text = "Network Connection Error";
    }
}
