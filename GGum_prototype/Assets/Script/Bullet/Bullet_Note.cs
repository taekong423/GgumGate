using UnityEngine;
using System.Collections;

public class Bullet_Note : Bullet {

	// Use this for initialization
	void Start () {
        SelfDestroy();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Move();
    }

    public override void DestroyBullet(float time)
    {
        Destroy(gameObject, time);
    }

}
