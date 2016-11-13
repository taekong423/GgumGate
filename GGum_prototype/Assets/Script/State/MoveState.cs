using UnityEngine;
using System.Collections;

public class MoveState : EnemyState {

    protected Transform _target;

    public MoveState(Enemy enemy, Searchable searchable) : base(enemy, searchable)
    {
        CurrentState = "Move";
    }

    protected override IEnumerator Enter()
    {
        _enemy.animator.SetTrigger("Move");

        _target = _enemy._wayPoints[_enemy._numWayPoint];

        yield return null;
    }

    protected override IEnumerator Execute()
    {
        while (_enemy._statePattern is MoveState)
        {
            if (_enemy.GoToTarget(_target.position))
            {
                _enemy.SetStatePattern<IdleState>();
                _enemy.SetWayPointNum();
            }

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

public class NewMoveState : State
{
    readonly NewEnemy _enemy;
    readonly string _transition;

    protected Transform _target;

    public NewMoveState(NewEnemy enemy, string id, string transition) : base(id)
    {
        _enemy = enemy;
        _transition = transition;
    }

    public override void Enter()
    {
        _target = _enemy._wayPoints[_enemy.PointIndex];
        _enemy.PlayAnimation(GetID);
    }

    public override void Excute()
    {        
        if (_enemy.GoToTarget(_target.position))
        {
            _enemy.SetState(_transition);
        }
    }

    public override void Exit()
    {
        _target = null;
        _enemy.SetPointIndex();
    }

}
