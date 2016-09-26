using UnityEngine;
using System.Collections;
using System.Reflection;

public partial class Enemy : AICharacter {

    protected HitData _hitData;

    [HideInInspector]
    public bool _isHitEffectDelay = false;

    public HitData pHitData { get { return _hitData; } set { _hitData = value; } }

    public GameManager _gm;

    public float AttackDelay = 1.0f;

    // Use this for initialization
    void Awake () {
        Debug.Log("Awake");
        InitCharacter();
	}

    void OnEnable()
    {
        Debug.Log("Enable");
        _statePattern.StartState();
        //NextState();
        //StartCoroutine(InitState());
    }
    
    protected override void InitCharacter()
    {
        base.InitCharacter();
        animator = GetComponentInChildren<Animator>();

        _hitData.attacker = gameObject;
        _hitData.damage = attackDamage;
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

    protected override IEnumerator InitState()
    {
        return base.InitState();
    }

    protected override IEnumerator IdleState()
    {
        return base.IdleState();
    }

    protected override IEnumerator MoveState()
    {
        return base.MoveState();
    }



}
