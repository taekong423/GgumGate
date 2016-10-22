using UnityEngine;
using System.Collections;

public class IdleState : EnemyState {

    protected float _delay;

    public IdleState(Enemy enemy, Searchable searchable) : base(enemy, searchable) 
    {
        CurrentState = "Idle";
    }

    protected override IEnumerator Enter()
    {
        Debug.Log("IdleEnter");
        _enemy.animator.SetTrigger("Idle");

        yield return null;
    }

    protected override IEnumerator Execute()
    {
        while (_enemy._statePattern is IdleState)
        {
            if (_delay <= 0)
            {
                _enemy.SetStatePattern<MoveState>();
                _delay = _enemy._moveDelay;
            }
            else
            {
                _delay -= Time.deltaTime;
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
