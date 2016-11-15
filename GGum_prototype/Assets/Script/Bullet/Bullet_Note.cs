using UnityEngine;
using System.Collections;

public class Bullet_Note : Bullet {

	// Use this for initialization
	void Start () {
        DestroyBullet(destroyTime);

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Move();
    }

    IEnumerator SelfActive(float time)
    {
        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
    }

    public override void DestroyBullet(float time)
    {
        //Destroy(gameObject, time);
        StartCoroutine(SelfActive(time));
    }

}
