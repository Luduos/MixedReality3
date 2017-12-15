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
    private float PointerDistanceToPlayer = 2.0f;
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
        PlayerModel.transform.rotation = PlayerModel.transform.rotation * Quaternion.AngleAxis(Time.deltaTime * RotationSpeed * horizontalInput, -Vector3.forward);
    }


    public void SetGPSPosition(Vector2 gpsPosition)
    {
        GPSPosition = gpsPosition;
        this.transform.position = MapInfo.instance.GetGPSAsUnityPosition(gpsPosition);
        this.transform.position += new Vector3(0.0f, 0.0f, -0.1f);
    }

    public void OnMapInfoLoaded()
    {
        Vector3 CurrentPos = MapInfo.instance.GetGPSAsUnityPosition(GPSPosition);
        foreach (POIInfo poi in MapInfo.instance.GetPointsOfInterest())
        {
            POIPointer pointer = Instantiate(PointerPrefab);
            pointer.transform.SetParent(this.transform, false);
           
            Vector3 dir = PointerDistanceToPlayer * new Vector3(poi.UnityPosition.x - CurrentPos.x, poi.UnityPosition.y - CurrentPos.y, 0.0f).normalized;
            pointer.transform.Translate(dir.x, dir.y, -0.1f);
            pointer.transform.LookAt(CurrentPos + dir);
            pointers.Add(pointer);
        }
    }

}
