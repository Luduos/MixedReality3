using UnityEngine;
using UnityEngine.Events;

public class MapInfo : MonoBehaviour {
    [SerializeField]
    private GoogleMap MapScript;

    [SerializeField]
    private POIInfo[] PointsOfInterest;

    [SerializeField]
    private POI POIPrefab;

    [SerializeField]
    public UnityEvent OnFinishedInit;

    public static MapInfo instance = null;

    MapInfo()
    {
        if (instance != null)
        {
            Debug.Log("There can't be multiple MapInfo objects, destroying this object");
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        for(int i = 0; i < PointsOfInterest.Length; ++i)
        {
            POI poi = Instantiate(POIPrefab);
            poi.gameObject.name = PointsOfInterest[i].Name;
            poi.SetName(PointsOfInterest[i].Name);
            poi.SetGPSPosition(PointsOfInterest[i].GPSPosition);
            PointsOfInterest[i].UnityPosition = GetGPSAsUnityPosition(PointsOfInterest[i].GPSPosition);
            PointsOfInterest[i].ID = poi.ID;
        }
        if (null != OnFinishedInit)
            OnFinishedInit.Invoke();
    }

    public POIInfo[] GetPointsOfInterest()
    {
        return PointsOfInterest;
    }

    public Vector2 GetGPSMapCenter()
    {
        return new Vector2(MapScript.centerLocation.longitude, MapScript.centerLocation.latitude);
    }

    public void SetAndRefreshGPSMapCenter(Vector2 newMapCenter)
    {
        MapScript.centerLocation.longitude = newMapCenter.x;
        MapScript.centerLocation.latitude = newMapCenter.y;
        MapScript.Refresh();
    }

    public Vector2 GetUnityMapCenter()
    {
        Vector2 gpsMapCenter = GetGPSMapCenter();
        double x = MercatorProjection.lonToX(gpsMapCenter.x);
        double y = MercatorProjection.latToY(gpsMapCenter.y);
        return new Vector2((float)x, (float)y);
    }

    public float GetZoom()
    {
        return MapScript.zoom;
    }

    public Vector2 GetGPSAsUnityPosition(Vector2 gpsLocation)
    {
        double posX = MercatorProjection.lonToX(gpsLocation.x);
        double posY = MercatorProjection.latToY(gpsLocation.y);

        Vector2 mapCenter = GetUnityMapCenter();

        posX -= mapCenter.x;
        posX /= GetZoom();
        posY -= mapCenter.y;
        posY /= GetZoom();

        return new Vector2((float)posY, (float)posX);
    }

}

[System.Serializable]
public struct POIInfo
{
    public int ID;
    public string Name;
    public Vector2 GPSPosition;
    public Vector2 UnityPosition;
}