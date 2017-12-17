using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;

public class Gyroscope : MonoBehaviour
{
	
	private Gyroscope gyro;
	public GameObject arrProp;

	public ArrowInfo arrPropInfo;

	[SerializeField]
	private GameObject arrPropNotSelected;
	[SerializeField]
	private ArrowInfo arrPropInfoNotSelected;
	[SerializeField]
	private bool selected;
	//private Accelerometer accelerometer;
	private int timesYouCanVote;




	private void Start()
	{
		timesYouCanVote = 1;
		Input.gyro.updateInterval = 0.03f;
		Input.gyro.enabled = true;
	}


	private void Update ()
	{
		/*if (!isLocalPlayer) {
			return;
		}*/
		RaycastHit hit;

		Vector3 groundSelectionVectorPosition = new Vector3 (this.transform.position.x, 0, this.transform.position.z); 
		Ray selectionRay = new Ray (groundSelectionVectorPosition, transform.forward);
		Debug.DrawRay (groundSelectionVectorPosition, transform.forward * 5, Color.green);

		this.transform.Rotate (0, -Input.gyro.rotationRateUnbiased.z, 0);
		//Debug.Log ("Gravity is "+Input.gyro.gravity);
		//Debug.Log (-Input.gyro.rotationRateUnbiased.z);

			 



		if (Physics.Raycast (selectionRay, out hit, Mathf.Infinity) && (timesYouCanVote != 0) && (Input.gyro.userAcceleration.y > 0.4f) ){
			

					timesYouCanVote--;
					
					//selected = true;

					arrProp = hit.transform.gameObject;
			arrPropInfo = arrProp.GetComponent<ArrowInfo>();
			arrPropInfo.arrowsInfo.count++;
			Debug.Log ("Selected" + arrPropInfo.arrowsInfo.POI);
				
		}
		arrPropNotSelected = hit.transform.gameObject;
		arrPropInfoNotSelected = arrPropNotSelected.GetComponent<ArrowInfo>();
				Debug.Log ("Not selected is" + arrPropInfoNotSelected.arrowsInfo.POI);
		

			

		}


	}






