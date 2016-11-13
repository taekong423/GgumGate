using UnityEngine;
using System.Collections;

public class NewSquirrel : NewEnemy {

    void Start()
    {
        SetState("Idle");
    }

    protected override void SetStateList()
    {
        _stateList.Add("Idle", new NewIdleState(this, "Idle", _stayTime, "Move"));
        _stateList.Add("Move", new NewMoveState(this, "Move", "Idle"));
    }

}
