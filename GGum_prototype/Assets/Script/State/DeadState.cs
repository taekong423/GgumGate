using UnityEngine;
using System.Collections;

public class DeadState : EnemyState {

    public DeadState(Enemy enemy, Searchable searchable) : base(enemy, searchable)
    {

    }

    protected override IEnumerator Enter()
    {
        _enemy.isInvincible = true;
        _enemy.GetComponent<BoxCollider2D>().enabled = false;

        _enemy.Dead();

        yield return null;
    }

    protected override IEnumerator Execute()
    {
        _enemy.animator.SetTrigger("Hit");

        yield return new WaitForSeconds(0.5f);
    }

    protected override IEnumerator Exit()
    {
        _enemy.SetStatePattern<InitState>();
        _enemy.gameObject.SetActive(false);

        _enemy.currentHP = _enemy.maxHP;

        yield return null;
    }
}
