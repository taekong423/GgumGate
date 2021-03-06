﻿using UnityEngine;
using System.Collections;

public partial class Deer : Enemy, IEnemy {

    int _attackCount = 0;

    public GameObject[] _lightningRods;

    public int AttackCount { get { return _attackCount; } set { _attackCount = value; } }

    public int CurrentHP { get; set; }
    public int MaxHP { get; set; }

    public string GetID { get { return id; } }
        
    public void Active(bool active)
    {
        gameObject.SetActive(active);
    }

    protected override void InitCharacter()
    {
        base.InitCharacter();

        _statePatternList.Add(typeof(InitState), new InitState(this));
        _statePatternList.Add(typeof(IdleState), new IdleState(this, new Search_Chase_Attack(this)));
        _statePatternList.Add(typeof(MoveState), new DeerMoveState(this, new Search_Chase_Attack(this)));
        _statePatternList.Add(typeof(RestState), new RestState(this, new Search_Chase_Attack(this)));
        _statePatternList.Add(typeof(ChaseState), new ChaseState(this, new Search_Move_Attack(this)));
        _statePatternList.Add(typeof(AttackState), new DeerAttackState(this, new Search_Move_Chase(this)));
        _statePatternList.Add(typeof(HitState), new HitState(this, 1.5f, 0.5f, new NoSearch()));
        _statePatternList.Add(typeof(DeadState), new DeadState(this));

        SetStatePattern<InitState>();

        //_statePatternList.Add(typeof(Normal), new Normal(this));

        //SetStatePattern<Normal>();
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
            float restDelay = 2.0f;

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
                        _enemy.SetStatePattern<IdleState>();
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
            CurrentState = "Rest";
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
                    _enemy.SetStatePattern<MoveState>();
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

    class DeerAttackState : AttackState
    {
        Deer _deer;

        bool _onAttack = false;

        public DeerAttackState(Enemy enemy, Searchable searchable) : base(enemy, searchable)
        {
            _deer = enemy as Deer;
        }

        protected override IEnumerator Enter()
        {
            _enemy._isHitEffectDelay = true;

            while (_enemy._currentAttackDelay <= _enemy._attackDelay && !(_enemy._statePattern is DeadState))
            {
                Debug.Log("EnterLoop");
                _enemy._currentAttackDelay += Time.deltaTime;

                yield return null;
            }

            if (_enemy._statePattern is AttackState)
            {
                _enemy.animator.SetTrigger("Attack");
                _enemy._currentAttackDelay = 0.0f;
                _enemy.LookTarget(_enemy._player.transform);
                _onAttack = true;

                foreach (GameObject lighningRod in _deer._lightningRods)
                {
                    //lighningRod.GetComponent<Bullet>().pHitData = _deer.pHitData;
                    lighningRod.transform.localPosition = new Vector3(lighningRod.transform.localPosition.x, 75, lighningRod.transform.localPosition.z);
                    lighningRod.SetActive(true);
                }

                animTime = _enemy.animator.GetCurrentAnimatorStateInfo(0).length;
            }

            yield return null;

        }

        protected override IEnumerator Execute()
        {
            while (_onAttack && _deer.AttackCount <= 3 && !(_enemy._statePattern is DeadState))
            {

                if (_deer.AttackCount >= 3)
                {
                    _onAttack = false;
                    _deer.AttackCount = 0;
                    _enemy.animator.SetTrigger("StandReady");
                    _enemy.StartCoroutine(AttackDelay());
                    _enemy.SetStatePattern<RestState>();
                    break;
                }

                yield return null;
            }

            yield return null;

            _enemy._isHitEffectDelay = false;
        }
    }

}
