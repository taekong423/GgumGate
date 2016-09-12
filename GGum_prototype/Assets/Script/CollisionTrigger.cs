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
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			Debug.LogWarning(0);
			Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			Debug.LogWarning(0);
			Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
		}
	}
}
