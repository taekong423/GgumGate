using UnityEngine;
using System.Collections;

public class ThroughUp : Through {

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            New.Character _character = collision.GetComponent<New.Character>();

            if (_character != null)
            {
                _character.IgnoreCollision(_platformColliders);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            New.Character _character = collision.GetComponent<New.Character>();

            if (_character != null)
            {
                _character.NoIgnoreCollision(_platformColliders);
            }
        }
    }
}
