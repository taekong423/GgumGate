using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewEnemy : MonoBehaviour, IEnemy {

    public string CurrentState;

    [Header("Chracter Setting")]
    [SerializeField]
    string _enemyID;

    bool _isInvincible;
    bool _isSuperArmour;
    bool _isExhaustion;
    bool _attackable = true;


    [SerializeField]
    int _maxHP;
    int _currentHP;

    int _addIndex = 1;
    int _pointIndex = 0;

    HitData _hitData;

    IState _state;
    protected Dictionary<string, IState> _stateList;

    protected Transform _transform;
    protected Rigidbody2D _rigidbody;
    protected Collider2D _collider;

    protected Animator _animator;
    protected SoundPlayer _soundPlayer;

    protected Player _player;
    protected Transform _target;


    protected float _currentAttackDelay;

    public int _attackDamage;

    public float _stayTime;
    public float _moveSpeed;
    public float _attackDelay;

    [Header("Chracter Object")]
    public Transform _container;

    [Header("AI Setting")]
    public float _detectionRange = 100.0f;
    public float _attackRange = 20.0f;

    public MoveType _moveType;
    public Transform[] _wayPoints;

    public string GetID { get { return _enemyID; } }
    public virtual bool IsInvincible { get { return _isInvincible; } set { _isInvincible = value; } }
    public virtual bool IsSuperArmour { get { return _isSuperArmour; } set { _isSuperArmour = value; } }
    public virtual bool IsExhaustion { get { return _isExhaustion; } set { _isExhaustion = value; } }
    public bool Attackable { get { return _attackable; } set { _attackable = value; } }

    public int MaxHP { get { return _maxHP; } protected set { _maxHP = value; } }

    public virtual int CurrentHP
    {
        get { return _currentHP; }
        set
        {
            if (IsInvincible)
                return;

            Mathf.Clamp(value, 0, MaxHP);
            _currentHP = value;

            if(_currentHP > 0)
                HitEvent();

            if (_currentHP <= 0)
                SetState("Dead");
        }

    }

    public int PointIndex { get { return _pointIndex; } }
    public HitData pHitData { get { return _hitData; } protected set { _hitData = value; } }

    public string State { get { return _state.GetID; } }

    public Player GetPlayer { get { return _player; } }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Bullet"))
        {
            if (IsInvincible)
                return;

            HitData hitdata = coll.GetComponent<Bullet>().pHitData;

            if (hitdata.attacker.CompareTag("Enemy"))
                return;

            OnHit(hitdata);
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        Init();
        SetStateList();
    }

    void FixedUpdate()
    {
        _state.Excute();
    }
        
    protected virtual void Init()
    {
        _stateList = new Dictionary<string, IState>();

        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _animator = GetComponentInChildren<Animator>();
        _soundPlayer = GetComponent<SoundPlayer>();
        _container = _transform.FindChild("Container");

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        _hitData.attacker = gameObject;
        _hitData.damage = _attackDamage;

        CurrentHP = MaxHP;
        _currentAttackDelay = 0;
    }

    protected virtual void HitEvent()
    {

    }

    protected virtual void SetStateList()
    {
        
    }

    public virtual void SetState(string key)
    {
        if (_stateList.ContainsKey(key))
        {
            if (_state != null)
                _state.Exit();

            _state = _stateList[key];
            _state.Enter();
            CurrentState = State;
        }
    }

    protected void Move(Axis axis, float keyValue)
    {
        if (axis == Axis.Horizontal)
        {
            _transform.Translate(Vector2.right * keyValue * _moveSpeed * Time.fixedDeltaTime);
        }
        else if (axis == Axis.Vertical)
        {
            _transform.Translate(Vector2.up * keyValue * _moveSpeed * Time.fixedDeltaTime);
        }
    }

    protected void Flip(float dir)
    {
        if (dir > 0)
            _container.rotation = Quaternion.Euler(0, 0, 0);
        else if (dir < 0)
            _container.rotation = Quaternion.Euler(0, 180, 0);
    }

    public bool Search(Transform target, float range)
    {
        float dist = Vector2.Distance(target.position, _transform.position);

        return (dist <= range) ? true : false;
    }

    public bool GoToTarget(Vector3 targetPoint)
    {
        float dist = Vector2.Distance(targetPoint, _transform.position);

        if (dist <= 15.0f)
        {
            return true;
        }
        else
        {
            Vector2 dir = targetPoint - _transform.position;
            float dirX = dir.x / Mathf.Abs(dir.x);

            Move(Axis.Horizontal, dirX);
            Flip(dirX);

            return false;
        }

    }

    public void SetPointIndex()
    {
        switch (_moveType)
        {
            case MoveType.Once:
                break;

            case MoveType.PingPong:

                if (_addIndex > 0 && _pointIndex >= _wayPoints.Length - 1)
                {
                    _addIndex = -1;
                }
                else if (_addIndex < 0 && _pointIndex <= 0)
                {
                    _addIndex = 1;
                }

                _pointIndex += _addIndex;

                break;
        }
    }

    public void LookTarget(Transform target)
    {
        Vector3 dir = target.transform.position - _transform.position;
        float dirX = dir.x / Mathf.Abs(dir.x);

        Flip(dirX);
    }

    public void Active(bool active)
    {
        gameObject.SetActive(active);
    }

    public void OnHit(HitData hitdata)
    {
        CurrentHP -= hitdata.damage;
    }

    public void PlayAnimation(string name)
    {
        _animator.SetTrigger(name);
    }

    public float GetAnimationLength()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).length;
    }

    IEnumerator NoAttackForSecons(float delay)
    {
        Attackable = false;

        yield return new WaitForSeconds(delay);

        Attackable = true;
    }

    public void NoAttackForSecons()
    {
        StartCoroutine(NoAttackForSecons(_attackDelay));
    }

}
