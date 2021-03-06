﻿using UnityEngine;
using System.Collections;

public class AttackState : EnemyState {

    protected float animTime;

    public AttackState(Enemy enemy, Searchable searchable) : base(enemy, searchable)
    {
        CurrentState = "Attack";
    }

    protected override IEnumerator Enter()
    {
        while (_enemy._statePattern is AttackState)
        {
            if (_enemy._currentAttackDelay >= _enemy._attackDelay)
            {
                _enemy._currentAttackDelay = 0.0f;
                _enemy.animator.SetTrigger("Attack");
                animTime = _enemy.animator.GetCurrentAnimatorStateInfo(0).length;
                break;
            }
            else
            {
                _enemy._currentAttackDelay += Time.deltaTime;
            }

            yield return null;
        }

        yield return null;

    }

    protected override IEnumerator Execute()
    {

        float delay = 0.0f;

        bool onAttack = false;

        while (_enemy._statePattern is AttackState)
        {
            delay += Time.deltaTime;

            if (delay >= animTime)
            {
                _enemy.StartCoroutine(AttackDelay());
                _enemy.animator.SetTrigger("Idle");
                _enemy.SetStatePattern<IdleState>();
                break;
            }
            else if (delay >= animTime / 2 && !onAttack)
            {
                onAttack = true;
                _enemy.OnAttack();
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

    protected IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(_enemy._attackDelay);

        _enemy._currentAttackDelay = _enemy._attackDelay;
    }

}

public class NewAttackState : State
{
    readonly NewEnemy _enemy;

    protected float _animationLength;

    protected float _currentTime;

    public NewAttackState(NewEnemy enemy, string id) : base(id)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        if (_enemy.Attackable)
        {
            _enemy.PlayAnimation(GetID);
            _currentTime = 0;
            _animationLength = _enemy.GetAnimationLength();
        }
        else
        {
            _enemy.SetState("AttackIdle");
        }
    }

    public override void Excute()
    {
        _currentTime += Time.fixedDeltaTime;

        if (_currentTime >= _animationLength)
        {
            _enemy.SetState("AttackIdle");
        }
        else if (_enemy.Attackable &&_currentTime >= _animationLength * 0.5f)
        {
            _enemy.NoAttackForSecons();
            //_enemy.GetPlayer.OnHit(_enemy.pHitData);
        }
    }

    public override void Exit()
    {
        _currentTime = 0;
    }

}

