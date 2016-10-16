using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

public class StatePattern {

    string _currentState = "Init";

    protected Character _character;
        
    public string CurrentState { get { return _currentState; } set { _currentState = value; } }

    T ParseEnum<T>(string value)
    {
        return (T) Enum.Parse(typeof(T), value, true);
    }

    public StatePattern(Character character)
    {
        _character = character;
    }

    public virtual void StartState()
    {
        Debug.Log(_character.gameObject.name + "의 StatePattern에 StartState가 제대로 구현 되어 있지 않습니다.");
    }

    public virtual void StateLog()
    {
        Debug.Log(_character.gameObject.name + " State : " + CurrentState);
    }

    public virtual void SetState(string value)
    {
        Debug.Log("StatePattern : " + value);
        CurrentState = value;
    }

    public virtual void HitFunc()
    {

    }

    public T GetState<T>(ref T stateEnum) where T : IConvertible
    {
        return stateEnum;
    }

    protected void SetState<T>(ref T stateEnum, string value) where T : IConvertible
    {
        stateEnum = ParseEnum<T>(value);
        CurrentState = value;
    }

    public void NextState(string stateName)
    {
        string methodName = stateName + "State";
        MethodInfo info = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        _character.StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    public virtual void Search()
    {
    }

}



