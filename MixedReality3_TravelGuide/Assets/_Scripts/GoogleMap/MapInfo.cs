using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class MapInfo : MonoBehaviour {
    [SerializeField]
    private GoogleMap MapScript;

    [SerializeField]
    private POIInfo[] PointsOfInterest;

    [SerializeField]
    private POI POIPrefab;

    [SerializeField]
    public UnityEvent OnFinishedInit;

    [SerializeField]
    public Text DebugText;

    public static MapInfo instance = null;

    private Player PlayerObject;

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
        PlayerObject = FindObjectOfType<Player>();
        for (int i = 0; i < PointsOfInterest.Length; ++i)
        {
            POI poi = Instantiate(POIPrefab);
            poi.gameObject.name = PointsOfInterest[i].Name;
            poi.SetName(PointsOfInterest[i].Name);
            poi.SetGPSPosition(PointsOfInterest[i].GPSPosition);
            PointsOfInterest[i].UnityPosition = GetGPSAsUnityPosition(PointsOfInterest[i].GPSPosition);
            PointsOfInterest[i].ID = poi.ID;
            PointsOfInterest[i].poiObject = poi;
        }
        if (null != OnFinishedInit)
            OnFinishedInit.Invoke();

        RefreshMapCenter();
        GPS.Instance.OnInitialized += RefreshMapCenter;

    }

    private void Update()
    {
        UpdatePositions();
    }

    public POIInfo[] GetPointsOfInterest()
    {
        return PointsOfInterest;
    }

    public Vector2 GetGPSMapCenter()
    {
        return new Vector2(MapScript.centerLocation.longitude, MapScript.centerLocation.latitude);
    }

    public void RefreshMapCenter()
    {
        MapScript.centerLocation.longitude = PlayerObject.GetGPSPosition.x;
        MapScript.centerLocation.latitude = PlayerObject.GetGPSPosition.y;
        MapScript.Refresh();  
        UpdatePositions();
    }

    public Vector2 GetUnityMapCenter()
    {
        Vector2 gpsMapCenter = GetGPSMapCenter();
        double x = MercatorProjection.lonToX(gpsMapCenter.x);
        double y = MercatorProjection.latToY(gpsMapCenter.y);
        return new Vector2((float)x, (float)y);
    }

    public void IncreaseZoom()
    {
        MapScript.zoom = MapScript.zoom + 1;
        UpdatePositions();
    }

    public void UpdatePositions()
    {
        for (int i = 0; i < PointsOfInterest.Length; ++i)
        {
            PointsOfInterest[i].UnityPosition = GetGPSAsUnityPosition(PointsOfInterest[i].GPSPosition);
        }
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer && LocationServiceStatus.Running == Input.location.status)
        {
            PlayerObject.SetGPSPosition(new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude));
            
            DebugText.text = "lon: " + GPS.Instance.lon + "lat: " + GPS.Instance.lat;
        }
        else
        {
            // doing this for some updating.
            PlayerObject.SetGPSPosition(PlayerObject.GetGPSPosition);
        }
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

    public POI poiObject;
}