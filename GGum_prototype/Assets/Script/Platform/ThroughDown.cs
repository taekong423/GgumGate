using UnityEngine;
using System.Collections;

public class ThroughDown : Through {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            New.Character _character = collision.GetComponent<New.Character>();

            if (_character != null)
            {
                _character.SetIgnoreColliders(_platformColliders);
                _character.CanThrough = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            New.Character _character = collision.GetComponent<New.Character>();

            if (_character != null)
            {
                _character.SetIgnoreColliders(null);
                _character.CanThrough = false;
                _character.NoIgnoreCollision(_platformColliders);
            }
        }
    }

}
