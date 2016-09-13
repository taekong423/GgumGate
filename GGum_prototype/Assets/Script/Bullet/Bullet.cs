using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    protected HitData hitData;

    protected float destroyTime = 2.0f;

    [Header("Bullet Setting")]
    public float bulletSpeed;
    public GameObject effect;

    public HitData pHitData { get { return hitData; } set { hitData = value; } }


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
