using UnityEngine;
using System.Collections;

public class State : IState {

    readonly string _ID;

    public State(string id)
    {
        _ID = id;
    }

	public string GetID
    {
        get { return _ID; }
    }

    public virtual void Enter()
    {

    }

    public virtual void Excute()
    {

    }

    public virtual void Exit()
    {

    }

}
