using UnityEngine;
using System.Collections;

public partial class Deer : Enemy {

    bool _isStanReady = false;

    int _attackCount = 0;

    public GameObject[] _lightningRods;

    public int AttackCount { get { return _attackCount; } set { _attackCount = value; } }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            if (isInvincible)
                return;

            HitData hitdata = other.GetComponent<Bullet>().pHitData;

            if (hitdata.attacker.tag == "Enemy")
                return;

            OnHit(hitdata);

            Destroy(other.gameObject);
        }
    }

    protected override void InitCharacter()
    {
        base.InitCharacter();

        _statePatternList.Add(typeof(Normal), new Normal(this));
    }

    public override void SetStatePattern()
    {
        _statePattern = _statePatternList[typeof(Normal)];
    }

    protected override IEnumerator InitState()
    {
        state = State.Idle;
        _hitData.attacker = gameObject;
        _hitData.damage = 1;

        currentHP = maxHP;

        _currentMoveDleay = _moveDelay;

        yield return null;

        NextState();
        StartCoroutine(SearchUpdate());
    }

    protected override IEnumerator IdleState()
    {
        if (_isStanReady)
        {
            animator.SetTrigger("StanReady");
        }
        else
        {
            animator.SetTrigger("Idle");
        }

        while (state == State.Idle)
        {
            if (!Search(_player.transform, _attackRange))
            {

                if (_currentMoveDleay <= 0.0f)
                {
                    state = State.Move;

                    if (!_isStanReady)
                    {
                        _currentMoveDleay = _moveDelay;
                        if (_wayPoints.Length != 0)
                            _target = _wayPoints[_numWayPoint];
                    }
                        
                    
                }
                else
                {
                    _currentMoveDleay -= Time.fixedDeltaTime;
                }
                
            }

            yield return null;
        }

        yield return null;

        NextState();
    }


    protected override IEnumerator MoveState()
    {
        _isStanReady = false;

        float stanReadyDelay = 3.0f;

        animator.SetTrigger("Move");

        if (_target == null)
            _target = _wayPoints[_numWayPoint];

        while (state == State.Move)
        {

            if (stanReadyDelay <= 0.0f)
            {
                _currentMoveDleay = 1.0f;
                _isStanReady = true;
                state = State.Idle;
            }
            else
            {
                stanReadyDelay -= Time.deltaTime;

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

        yield return null;

        NextState();
    }

    protected override IEnumerator AttackState()
    {

        animator.SetTrigger("Attack");

        Vector3 dir = _player.transform.position - transform.position;
        float dirX = dir.x / Mathf.Abs(dir.x);

        Flip(dirX);

        foreach (GameObject lighningRod in _lightningRods)
        {
            lighningRod.GetComponent<Bullet>().pHitData = pHitData;
            lighningRod.transform.position = new Vector3(lighningRod.transform.position.x, 200, lighningRod.transform.position.z);
            lighningRod.SetActive(true);
        }

        _isHitEffectDelay = true;

        while (state == State.Attack)
        {
            if (_attackCount >= 3)
            {
                state = State.Idle;
                _attackCount = 0;
                _currentMoveDleay = 2;
                _isHitEffectDelay = false;
                _isStanReady = true;
            }

            yield return null;
        }

        yield return null;

        NextState();
    }

    protected override IEnumerator HitState()
    {
        animator.SetTrigger("Hit");

        while (state == State.Hit)
        {
            yield return null;
        }

        _isHitEffectDelay = false;

        yield return null;

        NextState();
    }

    protected override IEnumerator DeadState()
    {
        animator.SetTrigger("Dead");

        GetComponent<BoxCollider2D>().enabled = false;
        //GetComponent<CircleCollider2D>().enabled = false;
        m_rigidbody.gravityScale = 0;

        while (state == State.Dead)
        {
            yield return null;
        }

        gameObject.SetActive(false);

        yield return null;
    }

    protected override void HitFunc()
    {
        if (!_isHitEffectDelay)
        {
            _statePattern.SetState("Hit");
            _isHitEffectDelay = true;
        }
    }

}
