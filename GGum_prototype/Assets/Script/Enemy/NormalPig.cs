using UnityEngine;
using System.Collections;


public partial class NormalPig : Enemy {

    [HideInInspector]
    public BossPig _boss;

    protected static int _deathNum;

    protected Transform _centerPivot;

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

    public void Sprinkle()
    {
        float power = Random.Range(20000, 25000);

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

    protected override void InitCharacter()
    {
        base.InitCharacter();

        _statePatternList.Add(typeof(NormalState), new NormalState(this));
    }

    public void Setting(BossPig boss, Transform[] waypoints, Transform center)
    {
        _boss = boss;
        _wayPoints = waypoints;
        _centerPivot = center;
    }

    public override void SetStatePattern()
    {
        _statePattern = _statePatternList[typeof(NormalState)];
    }


    public void Dead()
    {
        if (_boss != null)
        {
            _deathNum++;
            _boss.ChildPigNum--;


            if (_deathNum >= 4)
            {
                Debug.Log("Child OnHIt");
                _deathNum = 0;

                int damage = (int)((float)_boss.maxHP * 0.1f);

                damage = (damage <= 0) ? 1 : damage;
                HitData hitdata = new HitData(_player.gameObject, damage);

                _boss.state = State.Hit;
                _boss.ChildOnHit(hitdata);
                //보스 넉백 애니메이션 실행 1.5초 뒤 하이드 상태
            }

        }
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
