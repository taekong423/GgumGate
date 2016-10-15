using UnityEngine;
using System.Collections;

public partial class Squirrel : Enemy {

    public bool isTuto = false;

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.tag == "Bullet")
    //    {
    //        if (isInvincible)
    //            return;

    //        HitData hitdata = other.GetComponent<Bullet>().pHitData;

    //        if (hitdata.attacker.tag == "Enemy")
    //            return;

    //        OnHit(hitdata);

    //        Destroy(other.gameObject);
    //    }
    //}


    protected override void InitCharacter()
    {
        base.InitCharacter();

        _statePatternList.Add(typeof(IdleState), new IdleState(this, new Search_Chase(this)));
        _statePatternList.Add(typeof(MoveState), new MoveState(this, new Search_Chase(this)));
        _statePatternList.Add(typeof(ChaseState), new ChaseState(this, new Search_Chase_Attack(this)));
        _statePatternList.Add(typeof(AttackState), new AttackState(this, new Search_Attack_Chase(this)));
        _statePatternList.Add(typeof(HitState), new HitState(this, 1.5f, 0.5f, new NoSearch()));
        _statePatternList.Add(typeof(DeadState), new DeadState(this, new NoSearch()));

        SetStatePattern<IdleState>();

        //_statePatternList.Add(typeof(Normal), new Normal(this));
        //_statePatternList.Add(typeof(Tutorial), new Tutorial(this));


        //SetStatePattern<Normal>();


    }

    //public override void SetStatePattern()
    //{
    //    if (isTuto)
    //        _statePattern = _statePatternList[typeof(Tutorial)] as StatePattern;
    //    else
    //        _statePattern = _statePatternList[typeof(Normal)] as StatePattern;
    //}

    //protected override void HitFunc()
    //{
    //    if (!_isHitEffectDelay)
    //    {
    //        _statePattern.SetState("Hit");
    //        _isHitEffectDelay = true;
    //    }
    //}
}
