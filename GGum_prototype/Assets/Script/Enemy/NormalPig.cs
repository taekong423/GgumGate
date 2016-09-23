using UnityEngine;
using System.Collections;


public class NormalPig : Enemy {

    protected BossPig _boss;

    protected static int _deathNum;

    protected Transform _centerPivot;
        
    void OnEnable()
    {
        StartCoroutine(InitState());
    }

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

    void Sprinkle()
    {
        float power = Random.Range(20000, 30000);

        Vector3 centerDir = _centerPivot.position - transform.position;

        float max, min;

        if (centerDir.x < 0)
        {
            max = 0;
            min = -0.2f;
        }
        else
        {
            max = 0.2f;
            min = 0;
        }

        Vector2 dir = new Vector2(Random.Range(min, max), Random.Range(0.5f, 1.0f));

        m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        m_rigidbody.AddForce(dir * power, ForceMode2D.Force);
        

        //m_rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

    }

    public void Setting(BossPig boss, Transform[] waypoints, Transform center)
    {
        _boss = boss;
        _wayPoints = waypoints;
        _centerPivot = center;
    }

    protected override IEnumerator InitState()
    {
        state = State.Idle;

        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
        m_rigidbody.gravityScale = 50;
        Color col = GetComponentInChildren<SpriteRenderer>().color;
        col.a = 1;
        GetComponentInChildren<SpriteRenderer>().color = col;
        Physics2D.IgnoreCollision(_player.GetComponent<Collider2D>(), GetComponent<CircleCollider2D>(), true);

        yield return null;

        Sprinkle();

        

        yield return null;

        NextState();
    }

    protected override IEnumerator IdleState()
    {
        animator.SetTrigger("Idle");

        while (state == State.Idle)
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

            yield return null;
        }

        yield return null;

        NextState();
    }

    protected override IEnumerator MoveState()
    {

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
            yield return null;

            
        }
        NextState();
    }

    protected override IEnumerator HitState()
    {
        animator.SetTrigger("Hit");

        float hitDelay = 0;

        while (hitDelay <= 1.5f)
        {
            hitDelay += Time.deltaTime;

            if (hitDelay >= 1.0f && state == State.Hit)
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
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        m_rigidbody.gravityScale = 0;
        if (_boss != null)
        {
            _deathNum++;
            _boss.ChildPigNum--;


            if (_deathNum >= 3)
            {
                _deathNum = 0;

                int damage = (int)((float)_boss.MaxHP * 0.1f);

                damage = (damage <= 0) ? 1 : damage;
                HitData hitdata = new HitData(_player.gameObject, damage);

                _boss.CurrentState = State.Hit;
                _boss.ChildOnHit(hitdata);
                //보스 넉백 애니메이션 실행 1.5초 뒤 하이드 상태
            }

        }
        animator.SetTrigger("Hit");

        yield return new WaitForSeconds(0.5f);

        animator.SetTrigger("Dead");

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
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
