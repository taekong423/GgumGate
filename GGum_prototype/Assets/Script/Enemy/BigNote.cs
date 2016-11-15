using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BigNote : NewEnemy {

    BumJoong _owner;

    float _selfDamage = 0.05f;

    bool _isAllDead = false;

    public Image _hpBar;

    public float _fallSpeed;

    public bool IsAllDead { get { return _isAllDead; } set { _isAllDead = value; } }

    void OnEnable()
    {
        SetState("Create");
    }

    public override int CurrentHP
    {
        get
        {
            return base.CurrentHP;
        }

        set
        {
            base.CurrentHP = value;

            _hpBar.fillAmount = (float)CurrentHP / (float)MaxHP;

        }
    }

    protected override void Init()
    {
        base.Init();
    }

    protected override void SetStateList()
    {
        _stateList.Add("Create", new CreateState(this, "Create"));
        _stateList.Add("Fall", new FallState(this, "Fall"));
        _stateList.Add("Idle", new BigNote_IdleState(this, "Idle"));
        _stateList.Add("Dead", new BigNote_DaedState(this, "Dead"));
    }

    protected override void Reset()
    {
        base.Reset();
        _hpBar.fillAmount = (float)CurrentHP / (float)MaxHP;
    }

    public void Setting(BumJoong owner, float fallSpeed = 0, float selfDamage = -1)
    {
        _owner = owner;
        if (fallSpeed != 0)
            _fallSpeed = fallSpeed;

        if (selfDamage != -1)
            _selfDamage = selfDamage;

    }


    class CreateState : State
    {
        readonly BigNote _note;

        float _animTime;

        float _delay = 0;

        public CreateState(BigNote note, string id) : base(id)
        {
            _note = note;
        }

        public override void Enter()
        {
            _note.IsInvincible = true;
            _note._collider.enabled = false;

            _note._rigidbody.gravityScale = 0;
            _animTime = _note._animator.GetCurrentAnimatorStateInfo(0).length;
        }

        public override void Excute()
        {

            if (_delay >= _animTime)
            {
                _delay = 0;
                _note.SetState("Fall");
            }
            else
            {
                _delay += Time.fixedDeltaTime;
            }

        }

        public override void Exit()
        {
            _note.IsInvincible = false;
            _note._collider.enabled = true;
        }

    }

    class FallState : State
    {
        readonly BigNote _note;

        RaycastHit2D _hit;

        float _dir;

        public FallState(BigNote note, string id) : base(id)
        {
            _note = note;
        }

        public override void Enter()
        {
            _note._rigidbody.gravityScale = _note._fallSpeed;

            _dir = (Random.Range(0, 2) == 0) ? 1 : -1;

        }

        public override void Excute()
        {
            _hit = Physics2D.Raycast(_note._transform.position, Vector2.down, 8.0f, LayerMask.GetMask("Ground"));
            if (_hit.collider != null)
            {
                _note.SetState("Idle");
            }
            else
            {
                _note._transform.Translate(_dir * Vector2.right * _note._moveSpeed * Time.fixedDeltaTime);
                if(_note._owner != null)
                    _note._transform.position = new Vector2(Mathf.Clamp(_note._transform.position.x, _note._owner._minTrans.position.x, _note._owner._maxTrans.position.x), _note.transform.position.y);
            }
        }

    }

    class BigNote_IdleState : State
    {
        readonly BigNote _note;

        float _seta = 0;

        Vector2 _basePos;

        public BigNote_IdleState(BigNote note, string id) : base(id)
        {
            _note = note;
        }

        public override void Enter()
        {
            _note._rigidbody.velocity = Vector2.zero;
            _note._rigidbody.gravityScale = 0;
            _basePos = _note._transform.position;
        }

        public override void Excute()
        {
            _note._transform.position = _basePos + new Vector2(0, Mathf.Sin(_seta) * 3);

            _seta += Time.fixedDeltaTime * 3;

            if (_seta >= 180.0f)
                _seta = 0;

        }

        public override void Exit()
        {
            _seta = 0;
        }

    }

    class BigNote_DaedState : State
    {
        readonly BigNote _note;

        float _delay = 0;

        float _animTime;

        public BigNote_DaedState(BigNote note, string id) : base(id)
        {
            _note = note;
        }

        public override void Enter()
        {
            _note.IsInvincible = true;
            _note._collider.enabled = false;

            _note.PlayAnimation("Dead");
            _animTime = _note._animator.GetCurrentAnimatorStateInfo(0).length;

        }

        public override void Excute()
        {
            if (_delay >= _animTime)
            {
                _delay = 0;
                if (_note._owner != null && !_note.IsAllDead)
                    _note._owner.BigNoteDead();

                _note.Reset();

                _note.Active(false);
            }
            else
            {
                _delay += Time.fixedDeltaTime;
            }
        }

        public override void Exit()
        {
            if (_note.IsAllDead)
                _note.IsAllDead = false;
        }

    }

}
