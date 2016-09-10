using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public class FuncList : UnityEvent { }

public class AnimationController : MonoBehaviour {

    Character _character;

    public FuncList _funcList;

    void Awake()
    {
        _character = GetComponentInParent<Character>();
    }

    public void SetState(State setState)
    {
        _character.CurrentState = setState;
    }

    public void FuncInvoke()
    {
        _funcList.Invoke();
    }
}
