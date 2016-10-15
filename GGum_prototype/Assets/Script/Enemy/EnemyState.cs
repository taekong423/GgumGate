using UnityEngine;
using System.Collections;
using System;

//public partial class Enemy {}


public class EnemyState : StatePattern
{
    Searchable _searchable;

    protected Enemy _enemy;

    public EnemyState(Enemy enemy, Searchable searchable = null) : base(enemy)
    {
        _enemy = enemy;
        _searchable = searchable;
    }

    public override void StartState()
    {
        _enemy.StartCoroutine(Action());
    }

    public override void SetState(string value)
    {
        CurrentState = value;
        switch (value)
        {
            case "Hit":
                _enemy.SetStatePattern<HitState>();
                break;

            case "Dead":
                _enemy.SetStatePattern<DeadState>();
                break;
        }
    }

    public virtual void SearchUpdate()
    {
        Debug.Log(_enemy.gameObject.name + "의 StatePattern에 SearchUpdate가 제대로 구현 되어 있지 않습니다.");
    }

    protected virtual IEnumerator SearchUpdate<T>() where T : IConvertible
    {
        float attackDelay = 0;

        while (CurrentState != "Dead")
        {
            if(CurrentState != "Attack")
                attackDelay -= Time.deltaTime;

            if (CurrentState == "Idle" || CurrentState == "Move")
            {
                if (_enemy.Search(_enemy._player.transform, _enemy._attackRange))
                {
                    if (attackDelay <= 0.0f)
                    {
                        SetState("Attack");
                        attackDelay = _enemy._attackDelay;
                    }   

                }
                else if (_enemy.Search(_enemy._player.transform, _enemy._detectionRange))
                {

                    SetState("Move");
                    attackDelay = _enemy._attackDelay;
                    _enemy._target = _enemy._player.transform;
                    _enemy._currentMoveDleay = _enemy._moveDelay;

                }
                else
                {
                    attackDelay = _enemy._attackDelay;

                    if (_enemy._target == _enemy._player.transform && _enemy._wayPoints.Length != 0)
                    {

                        _enemy._target = _enemy._wayPoints[_enemy._numWayPoint];
                    }

                }

            }

            yield return null;
        }

        yield return null;
    }

    protected virtual IEnumerator Enter()
    {
        yield return null;
    }

    protected virtual IEnumerator Execute()
    {
        yield return null;
    }

    protected virtual IEnumerator Exit()
    {
        yield return null;
    }

    protected virtual IEnumerator Action()
    {
        yield return Enter();

        yield return Execute();

        yield return Exit();
    }

    public override void Search()
    {
        if(_searchable != null)
            _searchable.Operate();
    }

}