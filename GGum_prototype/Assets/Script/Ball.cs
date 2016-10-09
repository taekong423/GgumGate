using UnityEngine;
using System.Collections;

public class Ball : Bullet {

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag == "DestroyWall")
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed, ForceMode2D.Force);
	}

}
