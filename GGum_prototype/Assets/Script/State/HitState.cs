using UnityEngine;
using System.Collections;

public class HitState : EnemyState {

    float _keepTime;
    float _delay;

    public HitState(Enemy enemy, float keepTime, float delay, Searchable searchable) : base(enemy, searchable)
    {
        _keepTime = keepTime;
        _delay = delay;
    }

    protected override IEnumerator Enter()
    {
        Debug.Log("Hit");
        _enemy.animator.SetTrigger("Hit");
        _enemy._isHitEffectDelay = true;
        _enemy.StartCoroutine(Keep());

        yield return null;
    }

    protected override IEnumerator Execute()
    {
        float delay = 0;
        while (_enemy._statePattern is HitState)
        {
            delay += Time.deltaTime;

            if (delay >= _delay)
            {
                _enemy.SetStatePattern<IdleState>();
            }

            yield return null;
        }
    }

    protected override IEnumerator Exit()
    {
        _enemy._statePattern.StartState();

        yield return null;
    }

    IEnumerator Keep()
    {
        yield return new WaitForSeconds(_keepTime);

        _enemy._isHitEffectDelay = false;
    }
}
