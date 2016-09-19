using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExplosionPig : NormalPig {

    bool _isExplosion = false;

    public Text _countText;
    public Collider2D _foot;

    protected override IEnumerator InitState()
    {
        state = State.Idle;

        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = false;
        m_rigidbody.gravityScale = 50;
        _foot.enabled = true;

        Color col = GetComponentInChildren<SpriteRenderer>().color;
        col.a = 1;
        GetComponentInChildren<SpriteRenderer>().color = col;

        Physics2D.IgnoreCollision(_player.GetComponent<Collider2D>(), _foot, true);

        yield return null;

        NextState();
        StartCoroutine(Explosion());
    }

    protected override IEnumerator DeadState()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        _foot.enabled = false;
        m_rigidbody.gravityScale = 0;
        
        if (!_isExplosion)
        {
            if (_boss != null)
            {
                _deathNum++;
                //_boss.ChildPigNum--;


                if (_deathNum >= 3)
                {
                    _deathNum = 0;
                    int damage = (int)((float)_boss.MaxHP * 0.1f);

                    damage = (damage <= 0) ? 1 : damage;
                    HitData hitdata = new HitData(_player.gameObject, damage);

                    _boss.CurrentState = State.Hit;
                    _boss.OnHit(hitdata);
                    //보스 넉백 애니메이션 실행 1.5초 뒤 하이드 상태
                }
            }

            animator.SetTrigger("Hit");

            yield return new WaitForSeconds(0.5f);

            animator.SetTrigger("Dead");

            yield return null;
        }
        else
        {
            GetComponent<CircleCollider2D>().enabled = true;
            animator.SetTrigger("Dead");
        }

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }

    IEnumerator Explosion()
    {

        float delay = 4;

        while (state != State.Dead)
        {
            Debug.Log("ex");
            _countText.text = string.Format("{0:F0}", delay);
            if (delay <= 0)
            {
                state = State.Dead;
                _isExplosion = true;
            }
            else
            {
                delay -= Time.deltaTime;
            }

            yield return null;
        }

        

        yield return null;

    }
}
