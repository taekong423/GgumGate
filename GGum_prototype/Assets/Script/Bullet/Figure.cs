using UnityEngine;
using System.Collections;

public class Figure : Bullet {

    Collider2D _collider;

    // Use this for initialization
    void Start() {
        _collider = GetComponent<Collider2D>();
        SelfDestroy();
    }

    void FixedUpdate()
    {
        Move();
    }

    public override void DestroyBullet(float time)
    {
        //Destroy(gameObject, time);
    }

    void OnDestroy()
    {
        Debug.Log("Figure OnDestroy()");
        if (_collider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            Debug.Log("Figure Layer()");
            Instantiate(effect, transform.position, transform.rotation);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //DestroyBullet(0);
        }
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Figure OnTrigger()");
            Instantiate(effect, transform.position, transform.rotation);
        }
    }
}
