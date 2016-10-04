using UnityEngine;
using System.Collections;

public class EnableTrigger : Trigger {

    public bool _isEnabled;

    public MonoBehaviour _class;

    void OnEnable()
    {
        DoAction();
    }

    public override void DoAction()
    {
        if (_usable)
        {
            _usable = false;

            _class.enabled = _isEnabled;
        }
    }
}
