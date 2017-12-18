using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(MyClientBehaviour))]
public class MyNetworkBehaviour : NetworkDiscovery {

    private MyClientBehaviour clientBehaviour;

    private void Start()
    {
        clientBehaviour = this.gameObject.GetComponent<MyClientBehaviour>();
        this.Initialize();
        this.StartAsClient();

    }

    public void OnServeAsHost()
    {
        if (null == clientBehaviour.Client)
        {
            clientBehaviour.Client = NetworkManager.singleton.StartHost();
            if (clientBehaviour.Client != null)
            {
                clientBehaviour.SetClient(clientBehaviour.Client, true);

            }
            this.StopBroadcast();
            this.Initialize();
            this.StartAsServer();
        }
        else
        {
            if (!clientBehaviour.Client.isConnected)
            {
                NetworkManager.singleton.StartServer();
                clientBehaviour.IsHost = true;
                this.StopBroadcast();
                this.Initialize();
                this.StartAsServer();
            }
            else if (clientBehaviour.IsHost)
            {
                NetworkServer.Shutdown();
                clientBehaviour.IsHost = false;
                clientBehaviour.Client.Disconnect();
                this.StopBroadcast();
                this.Initialize();
                this.StartAsClient();
            }
        }
        
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        if(clientBehaviour.Client == null)
        {
            NetworkManager.singleton.networkAddress = fromAddress;
            clientBehaviour.Client = NetworkManager.singleton.StartClient();
            if (clientBehaviour.Client != null)
            {
                clientBehaviour.SetClient(clientBehaviour.Client, false);
            }
        }
        else
        {
            if (!clientBehaviour.Client.isConnected)
            {
                clientBehaviour.Client.ReconnectToNewHost(fromAddress, NetworkManager.singleton.networkPort);
            }
        }
    }
}
