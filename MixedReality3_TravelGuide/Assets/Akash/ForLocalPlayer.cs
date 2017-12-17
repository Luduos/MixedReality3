using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}


	}
}