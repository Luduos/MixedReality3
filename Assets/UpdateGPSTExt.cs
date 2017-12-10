using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGPSTExt : MonoBehaviour {
	public Text coordinates;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		coordinates.text = "Lat:" + GPS.Instance.lat.ToString () + "\n" + "Lon:" + GPS.Instance.lon.ToString ();
	}
}
