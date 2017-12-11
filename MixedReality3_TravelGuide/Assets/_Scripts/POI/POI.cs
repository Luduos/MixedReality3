using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POI : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer QRCode;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMouseDown()
    {
        QRCode.gameObject.SetActive(!QRCode.gameObject.activeSelf);
    }


}
