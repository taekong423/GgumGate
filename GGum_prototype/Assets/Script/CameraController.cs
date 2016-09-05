using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private float xMargin = 1f;
    private float yMargin = 1f;
    private float xSmooth= 1f;
    private float ySmooth = 1f;
    private float maxY = 1f;
    private float miny = -5;

    private float targetX;
    private float targetY;

    public bool CameraMoveOn;

    private Transform player;
    // Use this for initialization
    void Start () {
	
	}

    void Awake()
    {
        player = GameObject.Find("CameraPoint").transform;
    }
	// Update is called once per frame
	void Update () {
	
	}
    void FixedUpdate()
    {
        if(CameraMoveOn)
        {
            Traclkplayer();
        }
        
    }

   public bool CheckXmargin()
    {
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }
   public bool CheckYmargin()
    {
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }


    void Traclkplayer()
    {
        targetX = transform.position.x;
        targetY = transform.position.y;

        if (CheckXmargin())
        {
            targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);
        }
        if (CheckXmargin())
        {
            targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);
        }

        targetY = Mathf.Clamp(targetY, miny, maxY);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    
}
