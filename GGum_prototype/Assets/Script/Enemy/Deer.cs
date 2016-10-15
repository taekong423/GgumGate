using UnityEngine;
using System.Collections;

public partial class Deer : Enemy {

    int _attackCount = 0;

    public GameObject[] _lightningRods;

    public int AttackCount { get { return _attackCount; } set { _attackCount = value; } }

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
