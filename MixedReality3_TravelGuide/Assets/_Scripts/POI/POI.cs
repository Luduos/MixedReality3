using UnityEngine;

public class POI : MonoBehaviour {

    public static int s_IDCounter;

    public int ID { get; set; }

    [SerializeField]
    private string Name;
    [SerializeField]
    private Vector2 GPSPosition;

    [SerializeField]
    private TextMesh NameText;

    private Camera MainCamera;

    POI()
    {
        ID = ++s_IDCounter;
    }

    private void Start()
    {
        MainCamera = Camera.main;
    }

    public void SetName(string name)
    {
        this.Name = name;
        NameText.text = name;
    }

    // This should be optimized - only updated f.e. if main camera is actually rotating
    private void Update()
    {
        Vector3 nameTextForward = NameText.transform.up;
        Vector3 mainCamForward = MainCamera.transform.up;
        float angleA = Mathf.Atan2(nameTextForward.x, nameTextForward.y) * Mathf.Rad2Deg;
        float angleB = Mathf.Atan2(mainCamForward.x, mainCamForward.y) * Mathf.Rad2Deg;

        // get the signed difference in these angles
        float angleDiff = Mathf.DeltaAngle(angleA, angleB);

        NameText.transform.RotateAround(this.transform.position, -Vector3.forward, angleDiff);
        this.transform.position = MapInfo.instance.GetGPSAsUnityPosition(GPSPosition);
    }

    public void SetGPSPosition(Vector2 gpsPosition)
    {
        GPSPosition = gpsPosition;
        this.transform.position = MapInfo.instance.GetGPSAsUnityPosition(gpsPosition);
    }
}
