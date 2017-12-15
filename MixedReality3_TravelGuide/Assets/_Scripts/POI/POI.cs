using UnityEngine;

public class POI : MonoBehaviour {

    [SerializeField]
    private string Name;
    [SerializeField]
    private Vector2 GPSPosition;
    
    public int Votes { get; set; }

    public void SetName(string name)
    {
        this.Name = name;
    }

    public void SetGPSPosition(Vector2 gpsPosition)
    {
        GPSPosition = gpsPosition;
        this.transform.position = MapInfo.instance.GetGPSAsUnityPosition(gpsPosition);
    }
}
