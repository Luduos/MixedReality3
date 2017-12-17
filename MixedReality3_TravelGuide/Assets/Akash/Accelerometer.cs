using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using UnityEngine.Networking;

public class Accelerometer : MonoBehaviour {
	public static bool zoomPressed;
	private float x,y,z;
	public bool bounds;
	public float maxZoomIn;
	public float maxZoomOut;


	// Use this for initialization
	void Start () {
		zoomPressed = false;
	}

	// Update is called once per frame
	void Update () {
		
		if (bounds) { 

			transform.position = new Vector3 (transform.position.x, Mathf.Clamp (transform.position.y, maxZoomIn,maxZoomOut), transform.position.z);
		
		}


		if (zoomPressed == false) {
			Navigate (0, 0, 0);
		} else
		{
			Navigate(0, -Input.acceleration.y, 0);
		}


	}

public void Zoom(){
		if(zoomPressed== false){
			zoomPressed = true;}
		else if(zoomPressed == true){
			zoomPressed =false;
		}
		
		Debug.Log ("Zooming");
		Debug.Log (zoomPressed);


}

	public void Navigate(float x,float y,float z){
		transform.Translate (x,y,z);
	}



}
