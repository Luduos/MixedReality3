using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [SerializeField]
    private float RotationSpeed = 5.0f;
    [SerializeField]
    private float RotationThreshold = 0.5f;
    [SerializeField]
    private Text DebugText;
    [SerializeField]
    private Vector2 GPSPosition;

    [SerializeField]
    private float ZoomSpeed = 1.0f;

    public Vector2 GetGPSPosition { get { return GPSPosition; }  set{ GPSPosition = value; } }

    [SerializeField]
    private GameObject PlayerModel;

    [SerializeField]
    private SpriteRenderer PlayerSprite;

    [SerializeField]
    private POIPointer PointerPrefab;
    private List<POIPointer> pointers = new List<POIPointer>();


    [SerializeField]
    private MyClientBehaviour clientBehaviour;


    public bool IsVotingTime { get; set; }
    public bool IsZooming { get; set; }

    private Camera MainCamera;

    private void Start()
    {
        MainCamera = Camera.main;
        this.transform.position = MapInfo.instance.GetGPSAsUnityPosition(GPSPosition) ;
        this.transform.position += new Vector3(0.0f, 0.0f, -0.1f);

        Input.gyro.updateInterval = 0.03f;
        Input.gyro.enabled = true;

        
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if(horizontalInput != 0)
        {
            PlayerModel.transform.rotation = PlayerModel.transform.rotation * Quaternion.AngleAxis(Time.deltaTime * RotationSpeed * horizontalInput, -Vector3.forward);
            SetLookAt(PlayerModel.transform.up);
        }

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            PlayerModel.transform.rotation = new Quaternion(0.0f, 0.0f, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
            SetLookAt(PlayerModel.transform.up);

        }

        

        float verticalInput = Input.GetAxis("Vertical");
        if(verticalInput != 0)
        {
            GPSPosition += new Vector2(Time.deltaTime * 0.001f * verticalInput, 0.0f);
            SetGPSPosition(GPSPosition);
        }
        if (IsVotingTime && Input.GetKeyDown(KeyCode.Space) )
        {
            int selected = OnInputVote();
            if(selected != -1)
            {
                clientBehaviour.SendPOIVote(selected);
            }
            Debug.Log(selected);
        }
        else if(IsVotingTime && (Mathf.Abs(Input.gyro.userAcceleration.y) > 0.4f))
        {
            //DebugText.text = "Voted: " + Mathf.Abs(Input.gyro.userAcceleration.y);
            int selected = OnInputVote();
            if (selected != -1)
            {
                Handheld.Vibrate();
                clientBehaviour.SendPOIVote(selected);
            }
            Debug.Log(selected);
        }
        if (IsZooming)
        {
            MainCamera.orthographicSize = MainCamera.orthographicSize - Input.acceleration.y * Time.deltaTime * ZoomSpeed;
        }

        if(IsZooming && Input.GetKey(KeyCode.Q))
        {
            MainCamera.orthographicSize = MainCamera.orthographicSize - 1.0f * Time.deltaTime * ZoomSpeed;
        }
        if (IsZooming && Input.GetKey(KeyCode.E))
        {
            MainCamera.orthographicSize = MainCamera.orthographicSize + 1.0f * Time.deltaTime * ZoomSpeed;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            MapInfo.instance.RefreshMapCenter();
        }
    }

    public void StartVoting()
    {
        SetWinner(-1);
        IsVotingTime = true;
        Handheld.Vibrate();
        ResetVotes();
        PlayerSprite.color = Color.red;
    }

    private void ResetVotes()
    {
        foreach (POIPointer p in pointers)
        {
            p.VoteCount = 0;
            p.OnMakeNormal();
        }
    }

    public void StopVoting(int winnerID, bool shouldDisplayWinner)
    {
        IsVotingTime = false;
        Handheld.Vibrate();
        if (shouldDisplayWinner)
        {
            SetWinner(winnerID);
        }
        else
        {
            foreach (POIPointer p in pointers)
            {
                p.OnMakeNormal();
            }
        }
        PlayerSprite.color = new Color(0.0f, 0.5f, 1.0f, 1.0f);
    }

    /// <summary>
    /// Use negative values to resest everything
    /// </summary>
    /// <param name="winnerID"></param>
    private void SetWinner(int winnerID)
    {
        foreach (POIPointer p in pointers)
        {
            if (p.ID == winnerID)
            {
                p.OnMakeTarget();
            }
            else
            {
                p.OnMakeNormal();
            }
        }
    }

    public int FindWinnerID()
    {
        if(pointers.Count < 1)
        {
            return -1;
        }
        POIPointer max = pointers[0];
        foreach (POIPointer p in pointers)
        {
           if( max.VoteCount < p.VoteCount)
            {
                max = p;
            }
        }
        return max.ID;
    }

    public int OnInputVote()
    {
        if (!IsVotingTime)
        {
            return -1;
        }
        foreach (POIPointer p in pointers)
        {
            if (p.IsActive)
            {
                IsVotingTime = false;
                p.OnMarkSelected();
                return p.ID;
            }
        }
        return -1;
    }

    public void ShowPOIVoteCounters(bool shouldShow)
    {
        foreach (POIPointer p in pointers)
        {
            p.ShowVoteCounters(shouldShow);
        }
    }

    public int OnIncreaseVoteCounter(int ID)
    {
        int votes = 0;
       
        foreach (POIPointer p in pointers)
        {
            if (p.ID == ID)
            {
                p.VoteCount = p.VoteCount + 1;
                return p.VoteCount;
            }
        }
        return votes;
    }

    public void OnSetVoteCounter(int ID, int count)
    {
        foreach (POIPointer p in pointers)
        {
            if (p.ID == ID)
            {
                p.VoteCount = count;
                return;
            }
        }
    }


    public void SetGPSPosition(Vector2 gpsPosition)
    {
        GPSPosition = gpsPosition;
        Vector2 UnityPosition = MapInfo.instance.GetGPSAsUnityPosition(gpsPosition);
        this.transform.position = UnityPosition;
        this.transform.position += new Vector3(0.0f, 0.0f, -0.1f);
        foreach(POIPointer poi in pointers)
        {
            poi.OnPlayerPositionChanged(UnityPosition);
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

            pointer.POIName = poi.Name;
            pointer.UnityTarget = poi.UnityPosition;
            pointer.ID = poi.ID;
            pointer.poiObject = poi.poiObject;
            pointer.OnPlayerPositionChanged(CurrentPos);
            
            pointers.Add(pointer);

        }
        MapInfo.instance.RefreshMapCenter();
        ShowPOIVoteCounters(false);
    }

}
