using UnityEngine;
using System.Collections;

public class ChaseState : EnemyState {

    Transform _target;

    public ChaseState(Enemy enemy, Searchable searchable) : base(enemy, searchable)
    {
        CurrentState = "Chase";
    }

    protected override IEnumerator Enter()
    {
        _enemy.animator.SetTrigger("Move");

        _target = _enemy._player.transform;

        yield return null;
    }

    protected override IEnumerator Execute()
    {
        while (_enemy._statePattern is ChaseState)
        {
            _enemy.GoToTarget(_target.position);

            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }

    protected override IEnumerator Exit()
    {
        _enemy._statePattern.StartState();

        yield return null;
    }

}


public class NewChaseState : State
{
    readonly NewEnemy _enemy;

    protected Transform _target;

    public NewChaseState(NewEnemy enemy, string id) : base(id)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        _target = _enemy.GetPlayer.transform;
        _enemy.PlayAnimation("Move");
    }

    public override void Excute()
    {
        _enemy.GoToTarget(_target.position);

        if (_enemy.Search(_target, _enemy._attackRange))
        {
            _enemy.SetState("Attack");
        }
        else if(!_enemy.Search(_target, _enemy._detectionRange))
        {
            _enemy.SetState("Idle");
        }

    }

}