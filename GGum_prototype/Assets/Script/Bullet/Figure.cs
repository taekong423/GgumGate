using UnityEngine;
using System.Collections;

public class Figure : Bullet {

	// Use this for initialization
	void Start () {
        SelfDestroy();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    void OnTriggerEnter2D(Collider2D other)
    {

    }
}
