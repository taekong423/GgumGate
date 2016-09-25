using UnityEngine;
using System.Collections;
using System.Reflection;

public class StatePattern : MonoBehaviour {

    protected Character _character;

    public void NextState(string stateName)
    {
        string methodName = stateName + "State";
        MethodInfo info = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        _character.StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    public virtual void StartState()
    {
        
    }

}



