using UnityEngine;
using System.Collections;

public class ThrowSquirrel : Enemy {

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

        _statePatternList.Add(typeof(ThrowPattern), new ThrowPattern(this));
    }


    protected override void HitFunc()
    {
        if (!_isHitEffectDelay)
        {
            _statePattern.SetState("Hit");
            _isHitEffectDelay = true;
        }
    }

    public class ThrowPattern : EnemyState
    {
        ThrowSquirrel _squirrel;

        public ThrowPattern(Enemy enemy) : base(enemy)
        {
            _squirrel = enemy as ThrowSquirrel;
        }

        public override void StartState()
        {
            NextState("Init");
        }

        IEnumerator InitState()
        {
            SetState("Idle");
            _enemy.GetComponent<BoxCollider2D>().enabled = true;

            yield return null;

            NextState(_currentState);

        }

        IEnumerator IdleState()
        {
            _enemy.animator.SetTrigger("Idle");

            yield return null;

            float delay = _squirrel._attackDelay;

            while (_currentState == "Idle")
            {
                if (delay <= 0.0f)
                {
                    SetState("Throw");
                }
                else
                {
                    delay -= Time.deltaTime;
                }

                yield return null;
            }

            NextState(_currentState);
        }


        IEnumerator ThrowState()
        {
            _enemy.animator.SetTrigger("Attack");

            GameObject obj = Instantiate(_squirrel.bullet, _squirrel.attackBox.position, _squirrel.attackBox.rotation) as GameObject;

            obj.GetComponent<Bullet>().pHitData = _squirrel.pHitData;

            yield return null;

            while (_currentState == "Throw")
            {
                yield return null;
            }

            NextState(_currentState);
        }

        IEnumerator HitState()
        {
            _enemy.animator.SetTrigger("Sturn");

            yield return null;

            float hitDelay = 0;

            while (hitDelay <= 1.5f)
            {
                hitDelay += Time.deltaTime;

                if (hitDelay >= 0.5f && _currentState == "Hit")
                {
                    SetState("Idle");
                    NextState(_currentState);
                }

                yield return null;
            }

            _enemy._isHitEffectDelay = false;

        }

        IEnumerator DeadState()
        {
            _enemy.animator.SetTrigger("Dead");

            _enemy.isInvincible = true;

            _enemy.GetComponent<BoxCollider2D>().enabled = false;

            while (_currentState == "Dead")
            {
                yield return null;
            }

            _enemy.gameObject.SetActive(false);

            yield return null;
        }

    }

}
