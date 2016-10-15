using UnityEngine;
using System.Collections;

public class IdleState<T> : EnemyState where T : StatePattern{

    float _moveDelay;

    public IdleState(Enemy enemy, Searchable searchable) : base(enemy, searchable) 
    {

    }

    protected override IEnumerator Enter()
    {
        _enemy.animator.SetTrigger("Idle");

        yield return null;
    }

    protected override IEnumerator Execute()
    {
        while (_enemy._statePattern is IdleState<T>)
        {
            if (_moveDelay <= 0)
            {
                _enemy.SetStatePattern<T>();
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
