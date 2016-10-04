using UnityEngine;
using System.Collections;

public class ActiveTrigger : Trigger {

    public bool _isActive;

    public GameObject _obj;

    void OnEnable()
    {
        DoAction();
    }

    public override void DoAction()
    {
        if (_usable)
        {
            _usable = false;
            _obj.SetActive(_isActive);
        }
    }
}
