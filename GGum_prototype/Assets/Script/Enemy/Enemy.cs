using UnityEngine;
using System.Collections;
using System.Reflection;

public class Enemy : AICharacter {


	// Use this for initialization
	void Awake () {
        _player = GameObject.FindObjectOfType<PlayerCharacter>();
        state = State.Idle;
	}

    void OnEnable()
    {
        StartCoroutine(InitState());
    }

    protected virtual IEnumerator InitState()
    {

        yield return null;
    }

    protected virtual IEnumerator IdleState()
    {
        
        yield return null;
    }

    protected virtual IEnumerator MoveState()
    {

        yield return null;
    }

    protected virtual IEnumerator AttackState()
    {
        yield return null;
    }

    protected virtual IEnumerator HitState()
    {



        yield return null;
    }

    protected virtual IEnumerator DeadState()
    {



        yield return null;
    }

    protected virtual IEnumerator Search()
    {


        yield return null;
    }

    protected void NextState()
    {
        string methodName = state.ToString() + "State";
        MethodInfo info = GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

}
