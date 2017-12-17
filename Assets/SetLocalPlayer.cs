using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SetLocalPlayer : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
		if (isLocalPlayer) {
			GetComponent<Gyroscope>().enabled = true;
			//GetComponentInChildren<Camera>().enabled = true;
			GetComponent<Accelerometer>().enabled = true;
			GetComponent<GPS>().enabled = true;
			//GetComponentInChildren<Canvas>().enabled = true;



		}
		
	}

}
