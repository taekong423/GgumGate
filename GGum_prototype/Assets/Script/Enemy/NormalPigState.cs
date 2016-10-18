using UnityEngine;
using System.Collections;

public partial class NormalPig {


    public class NormalState : EnemyState
    {
        public enum Stat
        {
            Init,
            Idle,
            Move,
            Attack,
            Hit,
            Dead,
        }

        NormalPig _pig;

        protected Stat _state;

        protected Color _baseColor;

        public NormalState(Enemy enemy) : base(enemy)
        {
            _pig = enemy as NormalPig;
        }

        public override void StartState()
        {
            NextState("Init");
        }

        public override void SetState(string value)
        {
            SetState<Stat>(ref _state, value);
        }

        protected virtual IEnumerator InitState()
        {
            _pig.isInvincible = false;
            _pig.GetComponent<BoxCollider2D>().enabled = true;

            yield return null;

            SetState("Idle");
            //_pig.Sprinkle();

            yield return null;

            NextState(CurrentState);
        }

        protected virtual IEnumerator IdleState()
        {

            _pig.animator.SetTrigger("Idle");

            yield return null;

            while (_state == Stat.Idle)
            {

                if (_pig._currentMoveDleay <= 0)
                {
                    SetState("Move");
                    _pig._currentMoveDleay = _pig._moveDelay;
                    _pig._target = _pig._wayPoints[_pig._numWayPoint];
                }
                else
                {
                    _pig._currentMoveDleay -= Time.deltaTime;
                }

                yield return null;
            }

            NextState(CurrentState);
        }

        protected virtual IEnumerator MoveState()
        {
            while (_state == Stat.Move)
            {
                if (_pig._target != null)
                {
                    //도착.
                    if (_pig.GoToTarget(_pig._target.position))
                    {
                        if (_pig._target != _pig._player.transform)
                        {
                            SetState("Idle");
                            _pig.SetWayPointNum();
                            _pig._target = _pig._wayPoints[_pig._numWayPoint];
                        }
                    }

                }

                yield return new WaitForFixedUpdate();
            }

            NextState(CurrentState);
        }

        protected virtual IEnumerator HitState()
        {
            _pig.animator.SetTrigger("Hit");

            yield return null;

            float hitDelay = 0;

            while (_state == Stat.Hit && hitDelay <= 1.5f)
            {
                hitDelay += Time.deltaTime;

                if (hitDelay >= 0.5f)
                {
                    SetState("Idle");
                }
                yield return null;
            }
            NextState(_state.ToString());

            if (hitDelay > 1.5f)
                hitDelay = 1.5f;

            yield return new WaitForSeconds(1.5f - hitDelay);

            _pig._isHitEffectDelay = false;

        }

        protected virtual IEnumerator DeadState()
        {
            _pig.isInvincible = true;
            _pig.GetComponent<BoxCollider2D>().enabled = false;

            _pig.Dead();

            yield return null;

            _pig.animator.SetTrigger("Hit");

            yield return new WaitForSeconds(0.5f);

            SetState("Init");

            _enemy.currentHP = _enemy.maxHP;
            _enemy._isHitEffectDelay = false;

            _pig.gameObject.SetActive(false);
        }
    }

}
