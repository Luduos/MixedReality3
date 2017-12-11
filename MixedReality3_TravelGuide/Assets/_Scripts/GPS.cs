using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour {

	public static GPS Instance{ set; get; }



	public float lat,lon;
	public bool isUnityRemote=true;

	void Start()
	{
		Instance = this;
		DontDestroyOnLoad (gameObject);
		StartCoroutine(StartLocationService());
	}

	private IEnumerator StartLocationService(){
		if (isUnityRemote)
		{
			yield return new WaitForSeconds(5);
		}
		if (!Input.location.isEnabledByUser) {
			Debug.Log ("Switch on GPS");	
			yield break;
		}

		Input.location.Start ();
		if (isUnityRemote)
		{
			yield return new WaitForSeconds(5);
		}
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds(1);
			Debug.Log ("Waiting");
			maxWait--;
		}
	

		if (maxWait <= 0) {
			Debug.Log ("TImed Out");
			yield break;
		}
	
		if (Input.location.status == LocationServiceStatus.Failed) {
			Debug.Log ("failed");
			yield break;}

		lat = Input.location.lastData.latitude;
		lon = Input.location.lastData.longitude;

		yield break;

	}

}
