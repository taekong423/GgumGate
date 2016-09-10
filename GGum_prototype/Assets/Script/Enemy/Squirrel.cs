using UnityEngine;
using System.Collections;

public class Squirrel : Enemy {


    protected override IEnumerator InitState()
    {
        state = State.Idle;
        _hitinfo.name = "Squirrel";
        _hitinfo.damage = 1;

        NextState();
        StartCoroutine(Search());

        yield return null;
    }

    protected override IEnumerator IdleState()
    {
        _anim.SetTrigger("Idle");
        

        while (state == State.Idle)
        {
            if (!Search(_player.transform, _attackRange))
            {
                if (_currentMoveDleay <= 0.0f)
                {

                    _currentMoveDleay = _moveDelay;
                    if (_wayPoints.Length != 0)
                        _target = _wayPoints[_numWayPoint];

                    state = State.Move;
                }
                else
                {
                    _currentMoveDleay -= Time.fixedDeltaTime;
                }
            }
            yield return null;

        }

        NextState();

        yield return null;
    }

    protected override IEnumerator MoveState()
    {
        _anim.SetTrigger("Move");

        while (state == State.Move)
        {
            if (_target != null)
            {
                //도착.
                if (GoToTarget(_target.position))
                {
                    SetWayPointNum();
                    _target = _wayPoints[_numWayPoint];
                    state = State.Idle;
                }

            }

            yield return new WaitForFixedUpdate();
        }

        NextState();

        yield return null;
    }

    protected override IEnumerator AttackState()
    {
        _anim.SetTrigger("Attack");

        Attack(_hitinfo);

        yield return null;
    }

    protected override IEnumerator HitState()
    {



        yield return null;
    }

    protected override IEnumerator DeadState()
    {



        yield return null;
    }

    protected override void Attack(HitInfo hitInfo)
    {
        _player.OnHit(hitInfo);
    }

}
