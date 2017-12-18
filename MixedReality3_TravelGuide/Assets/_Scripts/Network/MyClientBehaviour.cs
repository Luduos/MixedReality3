using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class MyClientBehaviour : MonoBehaviour {

    [SerializeField]
    private Text DebugText;

    [SerializeField]
    private Player PlayerComp;


    public bool IsHost { get; set; }

    public NetworkClient Client;


    public class MyMsgType
    {
        public static short POIUpdate = MsgType.Highest + 1;
        public static short POIVote = MsgType.Highest + 2;
        public static short StartVote = MsgType.Highest + 3;
        public static short StopVote = MsgType.Highest + 4;
    };

    public class POIVoteMessage : MessageBase
    {
        public int ID;
    }

    public class POIUpdateMessage : MessageBase
    {
        public int ID;
        public int Count;
    }

    public class StopVoteMessage : MessageBase
    {
        public int WinnerID;
    }

    public void SetClient(NetworkClient client, bool isHost)
    {
        this.IsHost = isHost;
        Client = client;
        Client.RegisterHandler(MsgType.Connect, OnConnected);
        Client.RegisterHandler(MsgType.Error, OnError);
        Client.RegisterHandler(MsgType.Disconnect, OnDisconnect);
        Client.RegisterHandler(MyMsgType.StartVote, OnStartVoting);
        Client.RegisterHandler(MyMsgType.StopVote, OnStopVoting);

        Client.RegisterHandler(MyMsgType.POIUpdate, OnPOIUpdate);
        if (IsHost)
        {
            NetworkServer.RegisterHandler(MyMsgType.POIVote, OnPOIVote);
            PlayerComp.ShowPOIVoteCounters(true);
        }
        else
        {
            PlayerComp.ShowPOIVoteCounters(false);
        }

        Debug.Log(Client.isConnected);
        DebugText.text = "Was Set";

    }

    /// <summary>
    /// Called by Button
    /// </summary>
    public void HostStartsVote()
    {
        if (IsHost)
        {
            NetworkServer.SendToAll(MyMsgType.StartVote, new EmptyMessage());
        }
    }

    /// <summary>
    /// Called by Button
    /// </summary>
    public void HostStopVote()
    {
        if (IsHost)
        {
            StopVoteMessage msg = new StopVoteMessage();
            msg.WinnerID = PlayerComp.FindWinnerID();
            if(msg.WinnerID > -1)
            {
                NetworkServer.SendToAll(MyMsgType.StopVote, msg);
            }
        }
    }

    private void OnStartVoting(NetworkMessage netMsg)
    {
        Debug.Log("It's voting time!");
        PlayerComp.StartVoting();
    }

    private void OnStopVoting(NetworkMessage netMsg)
    {
        Debug.Log("Voting time over");
        StopVoteMessage msg = netMsg.ReadMessage<StopVoteMessage>();

        // I know I could have written "PlayerComp.StopVoting(msg.WinnerID, IsHost);" but this version
        // is a lot better readable by a human
        if (IsHost)
        {
            PlayerComp.StopVoting(msg.WinnerID, true);
        }
        else
        {
            PlayerComp.StopVoting(msg.WinnerID, false);
        }

    }

    public void SendPOIVote(int ID)
    {
        POIVoteMessage msg = new POIVoteMessage();
        msg.ID = ID;
        if(Client != null)
        {
            Client.Send(MyMsgType.POIVote, msg);
        }
    }

    private void OnError(NetworkMessage msg)
    {
        Debug.Log("Error");
        DebugText.text = "Error";
        this.Client.Shutdown();
        this.Client = null;
    }

    /// <summary>
    /// Update for Host
    /// </summary>
    /// <param name="netMsg"></param>
    private void OnPOIUpdate(NetworkMessage netMsg)
    {

            POIUpdateMessage msg = netMsg.ReadMessage<POIUpdateMessage>();
            Debug.Log("ID " + msg.ID + " Count: " + msg.Count);
            PlayerComp.OnSetVoteCounter(msg.ID, msg.Count);

    }

    /// <summary>
    /// Only caught by Host!
    /// </summary>
    /// <param name="netMsg"></param>
    private void OnPOIVote(NetworkMessage netMsg)
    {
        POIVoteMessage msg = netMsg.ReadMessage<POIVoteMessage>();
        Debug.Log("Voted: " + msg.ID);
        int newCount = PlayerComp.OnIncreaseVoteCounter(msg.ID);

        // TODO: Send updated Count
        SendPOIUpdate(msg.ID, newCount);
    }

    private void SendPOIUpdate(int id, int newCount)
    {
        POIUpdateMessage msg = new POIUpdateMessage();
        msg.ID = id;
        msg.Count = newCount;
        NetworkServer.SendToAll(MyMsgType.POIUpdate, msg);
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
        this.Client.Shutdown();
        this.Client = null;
    }



    void OnDisconnect(NetworkMessage msg)
    {
        Debug.Log("Disconnect");
        DebugText.text = "Disconnect";
        this.Client.Shutdown();
        this.Client = null;
    }
}
