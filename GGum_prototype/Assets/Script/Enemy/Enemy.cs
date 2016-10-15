using UnityEngine;
using System.Collections;
using System.Reflection;

public partial class Enemy : AICharacter {

    [HideInInspector]
    public bool _isHitEffectDelay = false;

    

    public GameManager _gm;

    public float _attackDelay = 1.0f;

    [HideInInspector]
    public float _currentAttackDelay;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            if (isInvincible)
                return;

            HitData hitdata = other.GetComponent<Bullet>().pHitData;

            if (hitdata.attacker.tag == "Enemy")
                return;
            Debug.Log("aaaaaaaaaaaaaaaaaaa");
            OnHit(hitdata);

            Destroy(other.gameObject);
        }
    }

    // Use this for initialization
    void Awake () {
        Debug.Log("asdfasdf");
        InitCharacter();
	}

    void OnEnable()
    {
        if (_statePattern != null)
        {
            _statePattern.StartState();
        }
    }

    public string aaaa;

    void Update()
    {
        _statePattern.Search();

        aaaa = _statePattern.CurrentState;
    }

    protected override void InitCharacter()
    {
        base.InitCharacter();
        animator = GetComponentInChildren<Animator>();

        _hitData.attacker = gameObject;
        _hitData.damage = attackDamage;
    }

    public void SetEnabled(bool enabled, bool setGravity = true)
    {
        m_collider.enabled = enabled;

        if(setGravity)
            m_rigidbody.gravityScale = (enabled) ? 50 : 0;
    }

    public void LookTarget(Transform target)
    {
        Vector3 dir = target.transform.position - transform.position;
        float dirX = dir.x / Mathf.Abs(dir.x);

        Flip(dirX);
    }

    protected override void HitFunc()
    {
        if(!_isHitEffectDelay)
            SetStatePattern<HitState>();
    }

    public virtual void Dead()
    {

    }

}
