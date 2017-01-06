using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    protected New.HitData hitData;

    

    [Header("Bullet Setting")]
    public float bulletSpeed;
    public float destroyTime = 2.0f;

    public GameObject effect;

    public New.HitData pHitData { get { return hitData; } set { hitData = value; } }


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

    public virtual void DestroyBullet(float time) { }
}
