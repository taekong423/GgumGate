using UnityEngine;
using System.Collections;
using System.Reflection;

public class TutoSquirrel : Squirrel {

    protected bool _isCinematic = false;

    public GameManager _gm;

    protected override IEnumerator InitState()
    {
        yield return null;

        while (!_gm.flags[_gm.flagKeys[0]])
        {
            yield return null;
        }

        animator.SetTrigger("TutoSturn");

        yield return new WaitForSeconds(2.0f);

        state = State.Idle;

        NextState();
        StartCoroutine(SearchUpdate());
    }

}
