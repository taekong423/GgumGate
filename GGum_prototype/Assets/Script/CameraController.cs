using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private float xMargin = 1f;
    private float yMargin = 1f;
    private float xSmooth = 2f;
    private float ySmooth = 2f;

    private float minX = -400f;
    private float maxX = 400f;
    private float minY = -5.0f;
    private float maxY = 5.0f;
    
    private float targetX;
    private float targetY;
    private float orthSize;

    public bool CameraMoveOn;

    private bool shakeOn;
    private float shakeFreq = 0.05f;
    public float shakeRange = 5.0f;

    private bool isZooming = true;
    public float targetSize;

    private Transform currTarget;
    public Transform CurrTarget { get { return currTarget; } set { currTarget = value; } }

    void Awake()
    {
        currTarget = GameObject.Find("CameraPoint").transform;
        targetSize = 100f;
    }

    void Start()
    {
        Global.shared<CameraController>(this);
    }

    void OnDestroy()
    {
        Global.remove_shared<CameraController>();
    }

    void FixedUpdate()
    {
        if(CameraMoveOn)
        {
            TrackTarget();
        }

        if(isZooming)
        {
            Zooming();
        }
    }

   bool CheckXmargin()
    {
        return Mathf.Abs(transform.position.x - currTarget.position.x) > xMargin;
    }
   bool CheckYmargin()
    {
        return Mathf.Abs(transform.position.y - currTarget.position.y) > yMargin;
    }

    void TrackTarget()
    {
        targetX = transform.position.x;
        targetY = transform.position.y;

        if (CheckXmargin())
        {
            targetX = Mathf.Lerp(transform.position.x, currTarget.position.x, xSmooth * Time.deltaTime);
        }
        if (CheckYmargin())
        {
            targetY = Mathf.Lerp(transform.position.y, currTarget.position.y, ySmooth * Time.deltaTime);
        }

        targetX = Mathf.Clamp(targetX, minX, maxX);
        targetY = Mathf.Clamp(targetY, minY, maxY);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    void Zooming()
    {
        orthSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, 5 * Time.deltaTime);

        orthSize = Mathf.Clamp(orthSize, 50, 100);

        Camera.main.orthographicSize = orthSize;
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

    public void ZoomIn(Transform target)
    {
        currTarget = target;
        targetSize = 50.0f;
        minY = -50.0f;
        maxY = 50.0f;
    }

    public void ZoomOut()
    {
        currTarget = GameObject.Find("CameraPoint").transform;
        targetSize = 100.0f;
        minY = -4f;
        maxY = 4f;
    }
}
