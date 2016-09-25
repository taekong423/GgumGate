using UnityEngine;
using System.Collections;

public class Squirrel : Enemy {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            if (isInvincible)
                return;

            HitData hitdata = other.GetComponent<Bullet>().pHitData;
            OnHit(hitdata);

            Destroy(other.gameObject);
        }
    }


    protected override IEnumerator InitState()
    {
        state = State.Idle;
        _hitData.attacker = gameObject;
        _hitData.damage = 1;

        currentHP = maxHP;

        yield return null;

        NextState();
        StartCoroutine(SearchUpdate());
    }

    protected override IEnumerator IdleState()
    {
        Debug.Log("Idle");
        animator.SetTrigger("Idle");
        

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
        Debug.Log("Move");
        animator.SetTrigger("Move");

        while (state == State.Move)
        {
            if (_target != null)
            {
                //도착.
                if (GoToTarget(_target.position))
                {
                    if (_target != _player.transform)
                    {
                        SetWayPointNum();
                        _target = _wayPoints[_numWayPoint];
                        state = State.Idle;
                    }
                }

            }

            yield return new WaitForFixedUpdate();
        }

        NextState();

        yield return null;
    }

    protected override IEnumerator AttackState()
    {
        Debug.Log("Attack");

        animator.SetTrigger("Attack");

        Attack(_hitData);

        yield return null;
    }

    protected override IEnumerator HitState()
    {
        Debug.Log("Hit");
        animator.SetTrigger("Sturn");

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
        Debug.Log("Dead");

        animator.SetTrigger("Dead");

        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        m_rigidbody.gravityScale = 0;

        while (state == State.Dead)
        {
            yield return null;
        }

        gameObject.SetActive(false);

        yield return null;
    }

    protected override void Attack(HitData hitInfo)
    {
        _player.OnHit(hitInfo);
    }

    protected override void HitFunc()
    {
        if (state != State.Hit && !_isHitEffectDelay)
        {
            state = State.Hit;
            _isHitEffectDelay = true;
        }
    }
}
