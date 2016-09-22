using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private float xMargin = 1f;
    private float yMargin = 1f;
    private float xSmooth= 1f;
    private float ySmooth = 1f;

    private float minX = -400f;
    private float maxX = 400f;
    private float minY = -5;
    private float maxY = 1f;
    
    private float targetX;
    private float targetY;

    public bool CameraMoveOn;

    private bool shakeOn;
    private float shakeFreq = 0.05f;
    public float shakeRange = 5.0f;
    

    private Transform player;


    void Awake()
    {
        player = GameObject.Find("CameraPoint").transform;
    }

    void Start()
    {
        Global.shared<CameraController>(this);
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

        targetX = Mathf.Clamp(targetX, minX, maxX);
        targetY = Mathf.Clamp(targetY, minY, maxY);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    public void ShakeCamera(float time)
    {
        StartCoroutine(ShakeForSeconds(time));
        StartCoroutine(Shake());
    }

    public void ShakeCamera(bool shake)
    {
        if (shake)
        {
            shakeOn = true;
            StartCoroutine(Shake());
        }
        else
        {
            shakeOn = false;
        }
    }

    IEnumerator Shake()
    {
        while (shakeOn)
        {
            float x = transform.position.x + Random.Range(-shakeRange, shakeRange);
            float y = transform.position.y + Random.Range(-shakeRange, shakeRange);

            x = Mathf.Clamp(x, minX, maxX);
            y = Mathf.Clamp(y, minY, maxY);

            transform.position = new Vector3(x, y, transform.position.z);
            yield return new WaitForSeconds(shakeFreq);
        }
    }

    IEnumerator ShakeForSeconds(float time)
    {
        shakeOn = true;
        yield return new WaitForSeconds(time);
        shakeOn = false;
    }
}
