﻿using UnityEngine;
using System.Collections;
using System.Reflection;

public partial class Enemy : AICharacter {

    protected HitData _hitData;

    protected Collider2D _bodyCollider;

    [HideInInspector]
    public bool _isGround = false;
    //[HideInInspector]
    public bool _isHitEffectDelay = false;

    public HitData pHitData { get { return _hitData; } set { _hitData = value; } }

    

    public GameManager _gm;

    public float AttackDelay = 1.0f;

    // Use this for initialization
    void Awake () {
        InitCharacter();
	}

    void OnEnable()
    {
        SetStatePattern();
        //if(_statePattern != null)
            _statePattern.StartState();
    }
    
    protected override void InitCharacter()
    {
        base.InitCharacter();
        animator = GetComponentInChildren<Animator>();

        _hitData.attacker = gameObject;
        _hitData.damage = attackDamage;

        _bodyCollider = GetComponent<BoxCollider2D>();

    }

    public virtual void SetStatePattern()
    {
        
    }

    public void SetEnabled(bool enabled, bool setGravity = true)
    {
        _bodyCollider.enabled = enabled;

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
        Debug.Log("HITFUNC");
        _statePattern.HitFunc();
    }
}
