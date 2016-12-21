using UnityEngine;
using System.Collections;
using System;


public partial class Squirrel{

    public class Normal : EnemyState
    {

        public enum State
        {
            Init,
            Idle,
            Move,
            Attack,
            Hit,
            Dead,
        }

        
        public State _state;

        Player _player;

        public Normal(Enemy enemy) : base(enemy)
        {
        }

        public override void StartState()
        {
            NextState("Init");
        }

        public override void StateLog()
        {
        }

        public override void SetState(string value)
        {
            SetState<State>(ref _state, value);
        }

        public override void SearchUpdate()
        {
            _enemy.StartCoroutine(SearchUpdate<State>());
        }

        IEnumerator InitState()
        {
            SetState("Idle");

            yield return null;

            _enemy.isInvincible = false;

            NextState(_state.ToString());
            SearchUpdate();
        }

        IEnumerator IdleState()
        {
            StateLog();
            _enemy.animator.SetTrigger("Idle");

            yield return null;

            while (_state == State.Idle)
            {
                if (!_enemy.Search(_enemy._player.transform, _enemy._attackRange))
                {
                    if (_enemy._currentMoveDleay <= 0.0f)
                    {
                        _enemy._currentMoveDleay = _enemy._moveDelay;
                        if (_enemy._wayPoints.Length != 0)
                            _enemy._target = _enemy._wayPoints[_enemy._numWayPoint];

                        SetState("Move");
                    }
                    else
                    {
                        _enemy._currentMoveDleay -= Time.fixedDeltaTime;
                    }
                }

                yield return null;
            }

            yield return null;

            NextState(_state.ToString());
        }

        IEnumerator MoveState()
        {
            StateLog();
            _enemy.animator.SetTrigger("Move");

            yield return null;

            while (_state == State.Move)
            {
                if (_enemy._target != null)
                {
                    //도착.
                    if (_enemy.GoToTarget(_enemy._target.position))
                    {
                        if (_enemy._target != _enemy._player.transform)
                        {
                            _enemy.SetWayPointNum();
                            _enemy._target = _enemy._wayPoints[_enemy._numWayPoint];
                            _state = State.Idle;
                        }
                    }

                }

                yield return new WaitForFixedUpdate();
            }

            yield return null;

            NextState(_state.ToString());
        }

        IEnumerator AttackState()
        {
            StateLog();

            _enemy.animator.SetTrigger("Attack");
            //_enemy._player.OnHit(_enemy.pHitData);

            yield return null;

            while (_state == State.Attack)
            {
                yield return null;
            }

            yield return null;

            NextState(_state.ToString());
        }

        IEnumerator HitState()
        {
            _enemy.animator.SetTrigger("Sturn");

            yield return null;

            float hitDelay = 0;

            while (_state == State.Hit && hitDelay <= 1.5f)
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

            _enemy._isHitEffectDelay = false;

        }

        IEnumerator DeadState()
        {
            _enemy.isInvincible = true;
            _enemy.GetComponent<BoxCollider2D>().enabled = false;

            yield return null;

            _enemy.animator.SetTrigger("Sturn");

            yield return new WaitForSeconds(0.5f);

            SetState("Init");

            _enemy.gameObject.SetActive(false);
        }

    }

    public class Tutorial : EnemyState
    {
        //Enemy _enemy;

        public Tutorial(Enemy enemy) : base(enemy)
        {
            _enemy = enemy;
        }

        public override void StartState()
        {
            NextState("Cinematic");
        }

        IEnumerator CinematicState()
        {
            _enemy.isInvincible = true;

            yield return null;

            while (!_enemy._gm.flags[_enemy._gm.flagKeys[0]])
            {
                yield return null;
            }

            _enemy.animator.SetTrigger("TutoSturn");

            yield return new WaitForSeconds(2.0f);

            EnemyState statePattern = _character._statePatternList[typeof(Normal)] as Normal;

            _character._statePattern = statePattern;

            statePattern.SetState("Idle");
            _enemy.isInvincible = false;

            yield return null;

            statePattern.NextState("Idle");
            statePattern.SearchUpdate();
        }


    }
	
}

