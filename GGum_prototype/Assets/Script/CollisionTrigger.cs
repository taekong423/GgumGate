using UnityEngine;
using System.Collections;

public class CollisionTrigger : MonoBehaviour {

	private CircleCollider2D playerCollider;
	[SerializeField]
	private BoxCollider2D platformCollider;
	[SerializeField]
	private BoxCollider2D platformTrigger;

	// Use this for initialization
	void Start () {

		playerCollider = GameObject.Find("Player").GetComponent< CircleCollider2D> ();
		Physics2D.IgnoreCollision(platformCollider,platformTrigger,true);

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.name == "Player")
		{
			Debug.LogWarning(0);
			Physics2D.IgnoreCollision(platformCollider,playerCollider,true);
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.name == "Player")
		{
			Debug.LogWarning(0);
			Physics2D.IgnoreCollision(platformCollider,playerCollider,false);
		}
	}
}
