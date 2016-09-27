using UnityEngine;
using System.Collections;

public partial class BossPig {

    public class BossPigState : EnemyState
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


        protected BossPig _bossPig;

        protected Stat _state;

        protected CameraController _camera;

        public BossPigState(Enemy enemy) : base(enemy)
        {
            _bossPig = enemy as BossPig;
        }
    }

    public class Appear : BossPigState
    {

        

        public Appear(Enemy enemy) : base(enemy)
        {
        }

        public override void StartState()
        {
            NextState("Appear");
        }

        IEnumerator AppearState()
        {
            _bossPig.isInvincible = true;

            yield return null;

            _camera = Global.shared<CameraController>();

            yield return new WaitForSeconds(1.0f);

            _camera.ShakeCamera(1.0f);

            _bossPig._isHide = true;

            yield return new WaitForSeconds(0.5f);

            _bossPig.animator.SetTrigger("Hide");

            yield return new WaitForSeconds(0.5f);

            EnemyState statePattern = _bossPig._statePatternList[typeof(Pattern0)] as Pattern0;

            _bossPig._statePattern = statePattern;
            statePattern.StartState();

        }
    }

    public class Pattern0 : BossPigState
    {
        public Pattern0(Enemy enemy) : base(enemy)
        {
        }

        public override void StartState()
        {
            NextState("Init");
        }

        public override void SetState(string value)
        {
            SetState<BossPigState.Stat>(ref _state, value);
        }

        IEnumerator InitState()
        {
            SetState("Idle");

            yield return null;

            NextState(_state.ToString());
        }

        IEnumerator IdleState()
        {

            if (_bossPig._isHide)
            {
                _camera.ShakeCamera(1.0f);
                _bossPig.isInvincible = true;
                _bossPig._isHide = true;

                yield return new WaitForSeconds(0.5f);

                _bossPig.animator.SetTrigger("Hide");

                yield return new WaitForSeconds(0.5f);
            }

            yield return null;

            while (_state == Stat.Idle)
            {
                if (_bossPig._currentMoveDleay <= 0)
                {
                    SetState("Move");
                    _bossPig._currentMoveDleay = _bossPig._moveDelay;
                    _bossPig._target = _bossPig._wayPoints[_bossPig._numWayPoint];
                }
                else
                {
                    _bossPig._currentMoveDleay -= Time.deltaTime;
                }

                yield return null;

                NextState(_state.ToString());
            }
        }

        IEnumerator MoveState()
        {
            if (_bossPig._isHide)
            {
                _camera.ShakeCamera(1.0f);
                _bossPig.isInvincible = true;
                _bossPig._isHide = true;

                yield return new WaitForSeconds(0.5f);

                _bossPig.animator.SetTrigger("Hide");

                yield return new WaitForSeconds(0.5f);
            }

            yield return null;

            bool arrive = false;

            while (_state == Stat.Move)
            {
                if (_bossPig._target != null && !arrive)
                    _bossPig.GoToTarget(_bossPig._target.position);

                if (arrive)
                {
                    _bossPig.SetWayPointNum();

                    if (_bossPig._childPigNum <= 0)
                    {
                        SetState("Attack");
                    }
                    else
                    {
                        SetState("Idle");
                    }

                }

                yield return null;
            }

            NextState(_state.ToString());

        }

        IEnumerator AttackState()
        {
            _camera.ShakeCamera(1.0f);

            yield return new WaitForSeconds(1.0f);

            _bossPig.isInvincible = false;
            _bossPig._isHide = false;

            _bossPig.LookTarget(_bossPig._player.transform);

            _bossPig.animator.SetTrigger("Attack");
        }

    }

    public class Pattern1 : EnemyState
    {
        public Pattern1(Enemy enemy) : base(enemy)
        {

        }
    }

    public class Pattern2 : EnemyState
    {
        public Pattern2(Enemy enemy) : base(enemy)
        {

        }
    }
}
