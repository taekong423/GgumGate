using UnityEngine;
using System.Collections;

public class EnemyPattern {


    public virtual IEnumerator InitState()
    {
        yield return null;
    }

    public virtual IEnumerator IdleState()
    {
        yield return null;
    }

    public virtual IEnumerator MoveState()
    {
        yield return null;
    }

    public virtual IEnumerator AttackState()
    {
        yield return null;
    }

    public virtual IEnumerator HitState()
    {
        yield return null;
    }

    public virtual IEnumerator DeadState()
    {
        yield return null;
    }

}
