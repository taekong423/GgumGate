using UnityEngine;
using System.Collections;

public class dog : Enemy {


    protected override void InitCharacter()
    {
        base.InitCharacter();

        _statePatternList.Add(typeof(InitState), new InitState(this));
        _statePatternList.Add(typeof(IdleState), new IdleState(this, new Search_Chase_Attack(this)));
        _statePatternList.Add(typeof(MoveState), new MoveState(this, new Search_Chase_Attack(this)));
        _statePatternList.Add(typeof(ChaseState), new ChaseState(this, new Search_Move_Attack(this)));
        _statePatternList.Add(typeof(AttackState), new AttackState(this, new Search_Move_Chase(this)));
        _statePatternList.Add(typeof(HitState), new HitState(this, 1.5f, 0.5f, new NoSearch()));
        _statePatternList.Add(typeof(DeadState), new DeadState(this));

        SetStatePattern<InitState> ();
    }

    protected override void Attack(HitData hitInfo)
    {
        //_player.OnHit(hitInfo);
    }
}
