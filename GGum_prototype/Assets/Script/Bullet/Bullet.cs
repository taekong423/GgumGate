using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    protected HitInfo hitInfo;

    protected float destroyTime = 2.0f;

    [Header("Bullet Setting")]
    public float bulletSpeed;
    public GameObject effect;

    public HitInfo HitInfo { get { return hitInfo; } set { hitInfo = value; } }


    protected void Move()
    {
        transform.Translate(Vector2.right * bulletSpeed * Time.fixedDeltaTime);
    }

    protected void OnEffect()
    {
        effect.SetActive(true);
        effect.transform.parent = null;
    }

    protected void SelfDestroy()
    {
        Destroy(gameObject, destroyTime);
    }
}
