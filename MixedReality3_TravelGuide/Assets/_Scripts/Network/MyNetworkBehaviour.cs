using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(MyClientBehaviour))]
public class MyNetworkBehaviour : NetworkDiscovery {

    private NetworkClient currentHost;
    private NetworkClient currentClient;

    private MyClientBehaviour clientBehaviour;

    private void Start()
    {
        clientBehaviour = this.gameObject.GetComponent<MyClientBehaviour>();
        this.Initialize();
        this.StartAsClient();
    }

    public void OnServeAsHost()
    {
        if(null == currentHost)
        {
            currentHost = NetworkManager.singleton.StartHost();
            if(currentHost != null)
            {
                clientBehaviour.SetClient(currentHost);
            }
            this.isClient = false;
            this.StartAsServer();
        }
        
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        if(currentClient == null)
        {
            NetworkManager.singleton.networkAddress = fromAddress;

            currentClient = NetworkManager.singleton.StartClient();
            if(currentClient != null)
            {
                clientBehaviour.SetClient(currentClient);
                if (!this.isServer)
                {
                    this.enabled = false;
                }
            }
        }
    }
}
