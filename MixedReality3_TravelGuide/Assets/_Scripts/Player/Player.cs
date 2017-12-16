using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float RotationSpeed = 5.0f;
    [SerializeField]
    private Vector2 GPSPosition;

    [SerializeField]
    private GameObject PlayerModel;

    [SerializeField]
    private POIPointer PointerPrefab;
    private List<POIPointer> pointers = new List<POIPointer>();

    private void Start()
    {
        this.transform.position = MapInfo.instance.GetGPSAsUnityPosition(GPSPosition) ;
        this.transform.position += new Vector3(0.0f, 0.0f, -0.1f);
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if(horizontalInput != 0)
        {
            PlayerModel.transform.rotation = PlayerModel.transform.rotation * Quaternion.AngleAxis(Time.deltaTime * RotationSpeed * horizontalInput, -Vector3.forward);
            SetLookAt(PlayerModel.transform.up);
        }
    }


    public void SetGPSPosition(Vector2 gpsPosition)
    {
        GPSPosition = gpsPosition;
        this.transform.position = MapInfo.instance.GetGPSAsUnityPosition(gpsPosition);
        this.transform.position += new Vector3(0.0f, 0.0f, -0.1f);
        foreach(POIPointer poi in pointers)
        {
            poi.OnPlayerPositionChanged(gpsPosition);
        }
    }

    public void SetLookAt(Vector2 lookat)
    {
        foreach (POIPointer poi in pointers)
        {
            poi.OnPlayerRotationChanged(lookat);
        }
    }

    public void OnMapInfoLoaded()
    {
        Vector3 CurrentPos = MapInfo.instance.GetGPSAsUnityPosition(GPSPosition);
        foreach (POIInfo poi in MapInfo.instance.GetPointsOfInterest())
        {
            POIPointer pointer = Instantiate(PointerPrefab);
            pointer.transform.SetParent(this.transform, false);

            pointer.UnityTarget = poi.UnityPosition;
            pointer.OnPlayerPositionChanged(CurrentPos);
            pointers.Add(pointer);
        }
    }

}
