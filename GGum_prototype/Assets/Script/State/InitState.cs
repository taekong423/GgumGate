using UnityEngine;
using System.Collections;

public class InitState : EnemyState {

    public InitState(Enemy enemy) : base(enemy)
    {

    }

    protected override IEnumerator Enter()
    {
        CurrentState = "Init";
        _enemy.isInvincible = false;
        _enemy.GetComponent<BoxCollider2D>().enabled = true;

        yield return null;
    }

    protected override IEnumerator Execute()
    {
        _enemy.SetStatePattern<IdleState<MoveState>>();

        yield return null;
    }

    protected override IEnumerator Exit()
    {
        _enemy._statePattern.StartState();

        yield return null;
    }

}
