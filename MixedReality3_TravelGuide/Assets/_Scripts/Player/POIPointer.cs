using UnityEngine;

public class POIPointer : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer NormalSprite;

    [SerializeField]
    private SpriteRenderer BigSprite;

    [SerializeField]
    private TextMesh VoteCounter;

    [SerializeField]
    private TextMesh NameMesh;

    [SerializeField]
    private float PointerDistanceToPlayer = 2.0f;

    [SerializeField]
    float DirectionMatchPercentage = 0.95f;

    private string poiName;
    public string POIName { get { return poiName; } set { poiName = value; NameMesh.text = value; } }
    public Vector2 UnityTarget { get; set; }
    public bool IsActive { get; set; }

    private int voteCount;
    public int VoteCount { get { return voteCount; } set { voteCount = value; VoteCounter.text = value.ToString(); } }
    public int ID { get; set; }

    public POI poiObject { get; set; }

    private void Start()
    {
        VoteCount = 0;
    }

    public void OnPlayerPositionChanged(Vector2 PlayerPosition)
    {
        UnityTarget = poiObject.transform.position;
        Vector3 dir = new Vector3(UnityTarget.x - PlayerPosition.x, UnityTarget.y - PlayerPosition.y, 0.0f);
        if (dir.magnitude > PointerDistanceToPlayer)
        {
            this.gameObject.SetActive(true);
            dir = PointerDistanceToPlayer * dir.normalized;
            this.transform.localPosition = dir;
            this.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        }
        else
        {
            this.gameObject.SetActive(false);
        }     
    }

    public void OnPlayerRotationChanged(Vector2 forward)
    {
        float dotForwardTarget = Vector2.Dot(this.transform.up, forward);
        if(dotForwardTarget > DirectionMatchPercentage)
        {
            BigSprite.gameObject.SetActive(true);
            NormalSprite.gameObject.SetActive(false);
            IsActive = true;
        }
        else
        {
            BigSprite.gameObject.SetActive(false);
            NormalSprite.gameObject.SetActive(true);
            IsActive = false;
        }
    }

    public void OnMakeTarget()
    {
        NormalSprite.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        NormalSprite.color = Color.green;
        BigSprite.color = Color.green;
        BigSprite.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        poiObject.GetComponent<Renderer>().material.color = Color.green;
    }

    public void OnMakeNormal()
    {
        NormalSprite.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        NormalSprite.color = new Color(0f, 0.462745f, 1f, 0.917648f);
        BigSprite.color = new Color(0f, 0.462745f, 1f, 0.917648f);
        BigSprite.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        poiObject.GetComponent<Renderer>().material.color = Color.red;
    }
        
}
