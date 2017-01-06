using UnityEngine;
using System.Collections;

public partial class Deer {

    public class Normal : EnemyState
    {
        public enum State
        {
            Init,
            Idle,
            Rest,
            Move,
            Hit,
            Attack,
            Dead,
        }

        Deer _deer;

        float _restDelay = 1.0f;

        protected State _state;

        public Normal(Enemy enemy) : base(enemy)
        {
            _deer = enemy as Deer;
        }

        public override void StartState()
        {
            NextState("Init");
        }

        public override void SetState(string value)
        {
            SetState<State>(ref _state, value);
        }

        public override void SearchUpdate()
        {
            _deer.StartCoroutine(SearchUpdate<State>());
        }

        IEnumerator InitState()
        {
            SetState("Idle");

            yield return null;

            NextState(_state.ToString());
            SearchUpdate();
        }

        IEnumerator IdleState()
        {
            if (_deer._isHitEffectDelay)
                _deer._isHitEffectDelay = false;

            _deer.animator.SetTrigger("Idle");
                        
            yield return null;

            while (_state == State.Idle)
            {

                if (_deer._currentMoveDleay <= 0.0f)
                {
                    SetState("Move");
                    _deer._currentMoveDleay = _deer._moveDelay;

                    if (_deer._wayPoints.Length != 0)
                        _deer._target = _deer._wayPoints[_deer._numWayPoint];
                }
                else
                {
                    _deer._currentMoveDleay -= Time.deltaTime;
                }

                yield return null;
            }

            NextState(_state.ToString());

        }

        IEnumerator RestState()
        {
            _deer.animator.SetTrigger("StandReady");

            yield return null;

            while (_state == State.Rest)
            {

                if (_restDelay <= 0.0f)
                {
                    SetState("Move");
                    _restDelay = 1.0f;
                }
                else
                {
                    _restDelay -= Time.deltaTime;
                }

                yield return null;
            }

            NextState(_state.ToString());

        }

        IEnumerator MoveState()
        {
            if (_deer._isHitEffectDelay)
                _deer._isHitEffectDelay = false;

            _deer.animator.SetTrigger("Move");

            yield return null;

            float restDelay = 1.5f;

            while (_state == State.Move)
            {
                if (restDelay <= 0.0f)
                {
                    SetState("Rest");
                }
                else
                {
                    restDelay -= Time.fixedDeltaTime;

                    if (_deer.GoToTarget(_deer._target.position))
                    {
                        if (_deer._target != _deer._player.transform)
                        {
                            SetState("Idle");
                            _deer.SetWayPointNum();
                            _deer._target = _deer._wayPoints[_deer._numWayPoint];
                        }
                    }
                }

                yield return new WaitForFixedUpdate();
            }

            NextState(_state.ToString());

        }

        IEnumerator AttackState()
        {
            _deer._isHitEffectDelay = true;

            _deer.animator.SetTrigger("Attack");

            _deer.LookTarget(_deer._player.transform);

            foreach (GameObject lighningRod in _deer._lightningRods)
            {
                //lighningRod.GetComponent<Bullet>().pHitData = _deer.pHitData;
                lighningRod.transform.position = new Vector3(lighningRod.transform.position.x, 200, lighningRod.transform.position.z);
                lighningRod.SetActive(true);
            }

            yield return null;

            while (_state == State.Attack)
            {

                if (_deer.AttackCount >= 3)
                {
                    SetState("Rest");
                    _restDelay = 2.0f;
                    _deer.AttackCount = 0;
                }

                yield return null;
            }

            _deer._isHitEffectDelay = true;

            NextState(_state.ToString());

        }

        IEnumerator HitState()
        {
            _deer.animator.SetTrigger("Hit");

            yield return null;

            while (_state == State.Hit)
            {
                yield return null;
            }

            _deer._isHitEffectDelay = false;

            NextState(_state.ToString());
        }


        IEnumerator DeadState()
        {
            _deer.GetComponent<BoxCollider2D>().enabled = false;

            yield return new WaitForSeconds(0.5f);

            SetState("Init");

            _deer.gameObject.SetActive(false);
        }

    }
}
