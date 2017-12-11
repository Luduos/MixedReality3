using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapScript : MonoBehaviour
{

    string url = ""; //will hold the completed  url request we make to the google maps api

    public float lat = 24.917828f; //Insert your desired latitude
    public float lon = 67.097096f; //Insert your desired longitude
    public int zoom = 14;
    public int mapWidth = 640;
    public int mapHeight = 640;
    public enum mapType { roadmap, satellite, hybrid, terrain }; //choose map type to display
    public mapType mapSelected;
    public int scale;

    private bool loadingMap = false; //keeps track of whether we're waiting for a texture to load in case we want to display some sort of loading message while user waits for the map to load

    private IEnumerator mapCoroutine;


    IEnumerator GetGoogleMap(float lat, float lon)
    {
        url = "https://maps.googleapis.com/maps/api/staticmap?center=" + lat + "," + lon +
            "&zoom=" + zoom + "&size=" + mapWidth + "x" + mapHeight + "&scale=" + scale
            + "&maptype=" + mapSelected +
            "&key=AIzaSyD2PxzFXlBjRFXh6kJaUq1UHgtmir6AiK0";
        loadingMap = true;
        WWW www = new WWW(url); //make the new request
        yield return www; //happens once we receive a response from the google maps api
        loadingMap = false;
        gameObject.GetComponent<RawImage>().texture = www.texture; //assign downloaded map texture to Canvas Image
        Debug.Log(www.error);
        StopCoroutine(mapCoroutine); //stop the coroutine once we have the texture

    }
    void Start()
    {
        mapCoroutine = GetGoogleMap(lat, lon);
        StartCoroutine(mapCoroutine);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.M))
        { //Example of how to update the map with a new set of coordinates
            lat = 40.6786806f;
            lon = -073.8644250f;
            mapCoroutine = GetGoogleMap(lat, lon); //redefine the coroutine with the new map coordinates (might be a better way to do this...let me know!)
            StartCoroutine(mapCoroutine); //restart the coroutine
        }
    }
}