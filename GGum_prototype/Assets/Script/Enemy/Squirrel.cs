using UnityEngine;
using System.Collections;

public partial class Squirrel : Enemy {

    public bool isTuto = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            if (isInvincible)
                return;

            HitData hitdata = other.GetComponent<Bullet>().pHitData;

            if (hitdata.attacker.tag == "Enemy")
                return;

            OnHit(hitdata);

            Destroy(other.gameObject);
        }
    }


    protected override void InitCharacter()
    {
        base.InitCharacter();

        _statePatternList.Add(typeof(Normal), new Normal(this));
        _statePatternList.Add(typeof(Tutorial), new Tutorial(this));

        

    }

    public override void SetStatePattern()
    {
        if (isTuto)
            _statePattern = _statePatternList[typeof(Tutorial)] as StatePattern;
        else
            _statePattern = _statePatternList[typeof(Normal)] as StatePattern;
    }

    protected override void HitFunc()
    {
        if (!_isHitEffectDelay)
        {
            _statePattern.SetState("Hit");
            _isHitEffectDelay = true;
        }
    }
}
