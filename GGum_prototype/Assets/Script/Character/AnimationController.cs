using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public class FuncList : UnityEvent { }

public class AnimationController : MonoBehaviour {

    Character _character;

    bool _condition;

    public FuncList _funcList;

    void Awake()
    {
        _character = GetComponentInParent<Character>();
    }

    public void SetState(string setState)
    {
        _character._statePattern.SetState(setState);
    }

    public void FuncInvoke()
    {
        _funcList.Invoke();
    }

    public void ConditionCheck(string condition)
    {
        _condition = _character._statePattern.CurrentState == condition; 
    }

    public void ConditionSetState(string setState)
    {
        if (_condition)
            SetState(setState);
    }
}
