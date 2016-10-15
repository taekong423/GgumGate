using UnityEngine;
using System.Collections;

public class AttackState : EnemyState {

    float animTime;

    public AttackState(Enemy enemy, Searchable searchable) : base(enemy, searchable)
    {
    }

    protected override IEnumerator Enter()
    {
        Debug.Log("Attack : ");
        _enemy.animator.SetTrigger("Attack");
        _enemy.OnAttack();
        animTime = _enemy.animator.GetCurrentAnimatorStateInfo(0).length;

        yield return null;
            
    }

    protected override IEnumerator Execute()
    {

        float delay = 0.0f;

        while (_enemy._statePattern is AttackState)
        {
            if (delay >= animTime)
            {
                _enemy.animator.SetTrigger("Idle");
                break;
            }
            else
                delay += Time.deltaTime;


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
        Debug.Log("Exit");

        _enemy._currentAttackDelay = 0.0f;

        _enemy._statePattern.StartState();

        yield return null;
    }
}
