using UnityEngine;
using System.Collections;

public class TutoSquirrel : TutoEnemy {

    protected override IEnumerator InitState()
    {
        return base.InitState();
    }

    protected override IEnumerator IdleState()
    {
        return base.IdleState();
    }

    protected override IEnumerator MoveState()
    {
        return base.MoveState();
    }

    protected override IEnumerator AttackState()
    {
        return base.AttackState();
    }

    protected override IEnumerator HitState()
    {
        return base.HitState();
    }

    protected override IEnumerator DeadState()
    {
        return base.DeadState();
    }
}
