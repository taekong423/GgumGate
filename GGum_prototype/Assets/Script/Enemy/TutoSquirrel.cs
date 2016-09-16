using UnityEngine;
using System.Collections;
using System.Reflection;

public class TutoSquirrel : Squirrel {

    protected bool _isCinematic = false;

    protected override IEnumerator InitState()
    {
        while (!_isCinematic)
        {
            yield return new WaitForSeconds(2.0f);

            _isCinematic = true;

            yield return null;
        }

        animator.SetTrigger("TutoSturn");

        yield return new WaitForSeconds(1.0f);

        state = State.Idle;

        NextState();
        StartCoroutine(Search());
    }

    public void NextState()
    {
        string methodName = state.ToString() + "State";
        MethodInfo info = GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

}
