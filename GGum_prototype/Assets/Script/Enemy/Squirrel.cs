using UnityEngine;
using System.Collections;

public class Squirrel : Enemy {


    

    protected override IEnumerator InitState()
    {
        state = State.Idle;
        _hitinfo.attacker = gameObject;
        _hitinfo.damage = 1;

        CurrentHP = MaxHP;

        yield return null;

        NextState();
        StartCoroutine(Search());
    }

    protected override IEnumerator IdleState()
    {
        Debug.Log("Idle");
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
        Debug.Log("Move1");
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
        _anim.SetTrigger("Sturn");

        float hitDelay = 0;

        while (hitDelay <= 1.5f)
        {
            hitDelay += Time.deltaTime;

            if (hitDelay >= 0.5f && state == State.Hit)
            {
                state = State.Idle;
                NextState();
            }

            yield return null;
        }

        _isHitEffectDelay = false;

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

    protected override void Hit()
    {
        if (state != State.Hit && !_isHitEffectDelay)
        {
            state = State.Hit;
            _isHitEffectDelay = true;
        }
    }
}
