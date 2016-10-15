using UnityEngine;
using System.Collections;

public partial class Deer : Enemy {

    int _attackCount = 0;

    public GameObject[] _lightningRods;

    public int AttackCount { get { return _attackCount; } set { _attackCount = value; } }

    protected override void InitCharacter()
    {
        base.InitCharacter();

        //_statePatternList.Add(typeof(IdleState<DeerMoveState>), new IdleState<DeerMoveState>(this, new NoSearch()));
        //_statePatternList.Add(typeof(DeerMoveState), new DeerMoveState(this, new NoSearch()));
        //_statePatternList.Add(typeof(RestState), new RestState(this, new NoSearch()));


        //SetStatePattern<IdleState<DeerMoveState>>();

        _statePatternList.Add(typeof(Normal), new Normal(this));

        SetStatePattern<Normal>();
    }

    protected override void HitFunc()
    {
        if (!_isHitEffectDelay)
        {
            _statePattern.SetState("Hit");
            _isHitEffectDelay = true;
        }
    }

    class DeerMoveState : MoveState
    {
        public DeerMoveState(Enemy enemy, Searchable searchable) : base(enemy, searchable)
        {

        }

        protected override IEnumerator Execute()
        {
            float restDelay = 1.0f;

            while (_enemy._statePattern is DeerMoveState)
            {
                if (restDelay <= 0.0f)
                {
                    _enemy.SetStatePattern<RestState>();
                }
                else
                {
                    restDelay -= Time.fixedDeltaTime;

                    if (_enemy.GoToTarget(_target.position))
                    {
                        _enemy.SetStatePattern<IdleState<DeerMoveState>>();
                        _enemy.SetWayPointNum();
                    }
                }

                yield return new WaitForFixedUpdate();
            }
        }

    }

    class RestState : EnemyState
    {
        public RestState(Enemy enemy, Searchable searchable) : base(enemy, searchable)
        {
        }

        protected override IEnumerator Enter()
        {
            _enemy.animator.SetTrigger("StandReady");

            yield return null;
        }

        protected override IEnumerator Execute()
        {
            float restDelay = 1.0f;

            while (_enemy._statePattern is RestState)
            {
                if (restDelay <= 0.0f)
                {
                    _enemy.SetStatePattern<DeerMoveState>();
                }
                else
                {
                    restDelay -= Time.deltaTime;
                }

                yield return null;
            }

            yield return null;
        }

        protected override IEnumerator Exit()
        {
            _enemy._statePattern.StartState();

            yield return null;
        }

    }

}
