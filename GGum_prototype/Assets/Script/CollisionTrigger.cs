using UnityEngine;
using System.Collections;

public class CollisionTrigger : MonoBehaviour {

	private Collider2D playerCollider;

	[SerializeField]
	private Collider2D platformCollider;
	[SerializeField]
	private Collider2D platformTrigger;

	// Use this for initialization
	void Start () {
		playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider2D> ();
		Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player"))
		{
			Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.CompareTag("Player"))
		{
			Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
		}
	}
}
