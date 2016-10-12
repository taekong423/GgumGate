using UnityEngine;
using System.Collections;

public class Figure : Bullet {

	// Use this for initialization
	void Start () {
        SelfDestroy();
	}

    void FixedUpdate ()
    {
        Move();
    }

    public override void DestroyBullet(float time)
    {
        Destroy(gameObject, time);
    }
}
