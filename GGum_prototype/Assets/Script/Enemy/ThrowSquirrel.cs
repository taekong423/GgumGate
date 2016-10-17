using UnityEngine;
using System.Collections;

public class ThrowSquirrel : Enemy {


    protected override void InitCharacter()
    {
        base.InitCharacter();

        _statePatternList.Add(typeof(ThrowPattern), new ThrowPattern(this));

        SetStatePattern<ThrowPattern>();
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

        public override void SetState(string value)
        {
            CurrentState = value;
        }

        IEnumerator InitState()
        {
            Debug.Log("ThrowInit");
            SetState("Idle");
            _enemy.GetComponent<BoxCollider2D>().enabled = true;

            yield return null;

            NextState(CurrentState);

        }

        IEnumerator IdleState()
        {
            Debug.Log("ThrowIdle");

            _enemy.animator.SetTrigger("Idle");

            yield return null;

            float delay = _squirrel._attackDelay;

            while (CurrentState == "Idle")
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

            NextState(CurrentState);
        }


        IEnumerator ThrowState()
        {
            _enemy.animator.SetTrigger("Attack");

            GameObject obj = Instantiate(_squirrel.bullet, _squirrel.attackBox.position, _squirrel.attackBox.rotation) as GameObject;

            obj.GetComponent<Bullet>().pHitData = _squirrel.pHitData;

            yield return null;

            //while (CurrentState == "Throw")
            //{
            //    Debug.Log("ThrowLoop");
            //    yield return null;
            //}

            yield return new WaitForSeconds(0.5f);

            SetState("Idle");

            yield return null;

            Debug.Log("ThrowEnd");
            NextState(CurrentState);
        }

        IEnumerator HitState()
        {
            _enemy.animator.SetTrigger("Hit");
            _enemy.PlaySound("Hit");

            yield return null;

            float hitDelay = 0;

            while (hitDelay <= 1.5f)
            {
                hitDelay += Time.deltaTime;

                if (hitDelay >= 0.5f && CurrentState == "Hit")
                {
                    SetState("Idle");
                    NextState(CurrentState);
                }

                yield return null;
            }

            _enemy._isHitEffectDelay = false;

        }

        IEnumerator DeadState()
        {
            
            _enemy.isInvincible = true;
            _enemy.GetComponent<BoxCollider2D>().enabled = false;

            _enemy.Dead();

            yield return null;

            _enemy.animator.SetTrigger("Hit");

            yield return new WaitForSeconds(0.5f);

            SetState("Init");

            _enemy.currentHP = _enemy.maxHP;
            _enemy._isHitEffectDelay = false;

            _enemy.gameObject.SetActive(false);
        }

    }

}
