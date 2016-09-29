using UnityEngine;
using System.Collections;
using System.Reflection;

public partial class Enemy : AICharacter {

    protected HitData _hitData;

    protected Collider2D _bodyCollider;
    protected Collider2D _footCollider;

    [HideInInspector]
    public bool _isGround = false;
    [HideInInspector]
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
        _footCollider = container.GetComponent<CircleCollider2D>();

    }

    public virtual void SetStatePattern()
    {
        
    }

    public void SetEnabled(bool enabled, bool setGravity = true)
    {
        _bodyCollider.enabled = enabled;
        _footCollider.enabled = enabled;

        if(setGravity)
            m_rigidbody.gravityScale = (enabled) ? 50 : 0;
    }

    public void LookTarget(Transform target)
    {
        Vector3 dir = target.transform.position - transform.position;
        float dirX = dir.x / Mathf.Abs(dir.x);

        Flip(dirX);
    }

    public virtual IEnumerator SearchUpdate()
    {
        float attackDelay = 0;

        while (state != State.Dead)
        {
            if (state == State.Idle || state == State.Move)
            {

                if (Search(_player.transform, _attackRange))
                {
                    if (attackDelay <= 0.0f)
                    {
                        state = State.Attack;
                        attackDelay = AttackDelay;
                    }
                    else
                        attackDelay -= Time.deltaTime;

                }
                else if (Search(_player.transform, _detectionRange))
                {

                    state = State.Move;
                    attackDelay = AttackDelay;
                    _target = _player.transform;
                    _currentMoveDleay = _moveDelay;

                }
                else
                {
                    attackDelay = AttackDelay;

                    if (_target == _player.transform && _wayPoints.Length != 0)
                    {

                        _target = _wayPoints[_numWayPoint];
                    }

                }

            }

            yield return null;
        }

        yield return null;
    }

}
