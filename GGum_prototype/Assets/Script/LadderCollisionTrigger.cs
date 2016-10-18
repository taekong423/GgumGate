using UnityEngine;
using System.Collections;

public class LadderCollisionTrigger : MonoBehaviour {

    private Collider2D playerCollider;

    [SerializeField]
    private Collider2D platformCollider;
    private Collider2D ladderTrigger;

    // Use this for initialization
    void Start()
    {
        playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
        ladderTrigger = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(platformCollider, ladderTrigger, true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
        }
    }

    public void IgnorePlatform(bool ignore)
    {
        Physics2D.IgnoreCollision(platformCollider, playerCollider, ignore);
    }
}
