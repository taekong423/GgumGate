using UnityEngine;
using System.Collections;

public class AttackState : EnemyState {

    float animTime;

    public AttackState(Enemy enemy, Searchable searchable) : base(enemy, searchable)
    {
    }

    protected override IEnumerator Enter()
    {
        _enemy.animator.SetTrigger("Attack");
        animTime = _enemy.animator.GetCurrentAnimatorStateInfo(0).length;

        yield return null;

    }

    protected override IEnumerator Execute()
    {

        float delay = 0.0f;

        while (_enemy._statePattern is AttackState)
        {

            delay += Time.deltaTime;

            if (delay >= animTime)
            {
                _enemy.animator.SetTrigger("Idle");
                break;
            }
            else if(delay >= animTime/2)
                _enemy.OnAttack();

            yield return null;
        }

        while (_enemy._statePattern is AttackState)
        {

            if (_enemy._currentAttackDelay >= _enemy._attackDelay)
            {
                break;
            }
            else
                _enemy._currentAttackDelay += Time.deltaTime;

           yield return null;
        }

        

        yield return null;
    }

    protected override IEnumerator Exit()
    {
        _enemy._currentAttackDelay = 0.0f;

        _enemy._statePattern.StartState();

        yield return null;
    }
}
