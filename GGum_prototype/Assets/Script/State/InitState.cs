﻿using UnityEngine;
using System.Collections;

public class InitState : EnemyState {

    public InitState(Enemy enemy) : base(enemy)
    {
        CurrentState = "Init";
    }

    protected override IEnumerator Enter()
    {
        _enemy.isInvincible = false;
        _enemy.GetComponent<BoxCollider2D>().enabled = true;

        yield return null;
    }

    protected override IEnumerator Execute()
    {
        _enemy.SetStatePattern<IdleState>();

        yield return null;
    }

    protected override IEnumerator Exit()
    {
        _enemy._statePattern.StartState();

        yield return null;
    }

}
