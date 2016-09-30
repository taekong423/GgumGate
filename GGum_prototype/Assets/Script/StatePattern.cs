using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

public abstract class StatePattern {

    protected Character _character;


    public string _currentState = "Init";

    T ParseEnum<T>(string value)
    {
        return (T) Enum.Parse(typeof(T), value, true);
    }

    public StatePattern(Character character)
    {
        _character = character;
    }

    public abstract void StartState();

    public void NextState(string stateName)
    {
        string methodName = stateName + "State";
        MethodInfo info = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        _character.StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    public virtual void StateLog()
    {

    }

    public T GetState<T>(ref T stateEnum) where T : IConvertible
    {
        return stateEnum;
    }

    public virtual void SetState(string value)
    {
        _currentState = value;
    }

    protected void SetState<T>(ref T stateEnum, string value) where T : IConvertible
    {
        stateEnum = ParseEnum<T>(value);
        _currentState = value;
    }

    public virtual void HitFunc()
    {

    }
        
}



