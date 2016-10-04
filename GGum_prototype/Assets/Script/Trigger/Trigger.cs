using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

    [SerializeField]
    protected bool _usable;

    public bool Usable { get { return _usable; } set { _usable = value; } }

    public virtual void DoAction()
    {

    }

    public void Reset()
    {
        _usable = false;
    }
}
