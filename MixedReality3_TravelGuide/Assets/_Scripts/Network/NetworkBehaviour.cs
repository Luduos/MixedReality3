using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkBehaviour : NetworkDiscovery {

    [SerializeField]
    public Text DebugText;

    public float test;

    private void Start()
    {
        this.showGUI = true;
        this.Initialize();
        this.StartAsServer();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        NetworkManager.singleton.networkAddress = fromAddress;
        NetworkManager.singleton.StartClient();
        Debug.Log("data");
        DebugText.text = data;
    }

    public void OnVoted(int POIID)
    {
        
    }
}
