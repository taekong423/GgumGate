using UnityEngine;
using System.Collections;

public class IdleState : EnemyState {

    float _moveDelay;

    public IdleState(Enemy enemy, Searchable searchable) : base(enemy, searchable) 
    {
        CurrentState = "Idle";
    }

    protected override IEnumerator Enter()
    {
        _enemy.animator.SetTrigger("Idle");

        yield return null;
    }

    protected override IEnumerator Execute()
    {
        while (_enemy._statePattern is IdleState)
        {
            if (_moveDelay <= 0)
            {
                _enemy.SetStatePattern<MoveState>();
                _moveDelay = _enemy._moveDelay;
            }
            else
            {
                _moveDelay -= Time.deltaTime;
            }

            yield return null;
        }

        yield return null;
    }

    protected override IEnumerator Exit()
    {
        _enemy._statePattern.StartState();

        yield return null;
    }
}
