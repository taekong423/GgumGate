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

public class NewIdleState : State
{
    readonly NewEnemy _enemy;

    readonly float _stayTime;

    readonly string _transition;

    float _currentTime;

    public NewIdleState(NewEnemy enemy, string id, float stayTime, string transition) : base(id)
    {
        _enemy = enemy;
        _stayTime = stayTime;
        _transition = transition;
        _currentTime = 0;
    }

    public override void Enter()
    {
        if(_stayTime > 0)
            _enemy.PlayAnimation(GetID);
    }

    public override void Excute()
    {
        if (_enemy.Search(_enemy.GetPlayer.transform, _enemy._detectionRange))
        {
            _enemy.SetState("Chase");
            return;
        }

        if (_currentTime >= _stayTime)
        {
            _enemy.SetState(_transition);
        }
        else
        {
            _currentTime += Time.fixedDeltaTime;
        }
    }

    public override void Exit()
    {
        _currentTime = 0;
    }

}


public class AttackIdleState : State
{
    readonly NewEnemy _enemy;

    public AttackIdleState(NewEnemy enemy, string id) : base(id)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        _enemy.PlayAnimation(GetID);
    }

    public override void Excute()
    {
        if (!_enemy.Search(_enemy.GetPlayer.transform, _enemy._attackRange))
        {
            _enemy.SetState("Chase");
        }
        else if (_enemy.Attackable)
        {
            _enemy.SetState("Attack");
        }
    }

}