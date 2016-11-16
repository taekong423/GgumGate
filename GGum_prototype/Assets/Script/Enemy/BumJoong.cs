using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BumJoong : NewEnemy {

    Dictionary<int, List<GameObject>> _noteList;

    [SerializeField]
    int _PageNum = 1;

    int _bigNoteNum = 0;
    int _bigNoteDeadCount = 0;
    int _pattern2Num = 2;

    float _centerPos;

    bool _isCenter = false;
    bool _isDialogue = false;

    DialogueManager _dm;

    [Header("BumJoong Setting")]
    public string _dialogueID;
    public float _teleportDelay = 0.5f;

    public Transform _minTrans;
    public Transform _maxTrans;

    public GameObject _teleportEffect;

    public bool IsDialogue { get { return _isDialogue; } set { _isDialogue = value; } }

    [Header("Pattern1")]
    public float _exhaustionTime;
    public float _shootDelay;
    public int _numShoot;

    public Transform _muzzle;
    public GameObject[] _notePrefabs;

    [Header("Pattern2")]
    public int _spawnNum;
    public float _spawnDelay;
    public float _spawnHeight = 100.0f;
    [Range(0.0f, 1.0f)]
    public float _bigNoteSelftDamage = 0.05f;

    public GameObject _bigNotePrefab;

    [Header("Pattern3")]
    public float _height = 100.0f;
    public float _speed = 10.0f;
    public float _fallSpeed;
    
    public Animator _tornadoAnim;
    public GameObject _tornadoBox;

    [Header("SpeechBubble")]
    public SpeechBubble2 _sp;

    void OnEnable()
    {
        SetState("Init");
        //SetState("Appear");
    }

    protected override void Init()
    {
        base.Init();

        _noteList = new Dictionary<int, List<GameObject>>();
        _centerPos = (_minTrans.position.x + _maxTrans.position.x) * 0.5f;
        _dm = GameObject.FindObjectOfType<DialogueManager>();

    }

    protected override void SetStateList()
    {
        _stateList.Add("Init", new BumJoongInit(this, "Init"));
        _stateList.Add("Dialogue", new DialogueState(this, "Dialogue"));
        _stateList.Add("Appear", new AppearState(this, "Appear"));
        _stateList.Add("Pattern_1", new Pattern_1State(this, "Pattern_1"));
        _stateList.Add("Exhaustion", new ExhaustionState(this, "Exhaustion"));
        _stateList.Add("Pattern_2", new Pattern_2State(this, "Pattern_2"));
        _stateList.Add("Idle", new Pattern_2IdleState(this, "Idle"));
        _stateList.Add("OnPattern_3", new OnPattern_3State(this, "OnPattern_3"));
        _stateList.Add("Pattern_3", new Pattern_3State(this, "Pattern_3"));
        _stateList.Add("Dead", new BumJoong_DeadState(this, "Dead"));
    }

    void Teleport(Vector2 point)
    {
        //point.x = Mathf.Clamp(point.x, _minX, _maxX);
        point.x = Mathf.Clamp(point.x, _minTrans.position.x, _maxTrans.position.x);

        if (point.x == _centerPos)
            _isCenter = true;
        else
            _isCenter = false;

        _transform.position = point;

    }

    void SpawnNote(int kind)
    {
        if (!_noteList.ContainsKey(kind))
        {
            _noteList.Add(kind, new List<GameObject>());
        }

        GameObject obj = null;

        if (_noteList[kind].Count > 0)
        {
            obj = _noteList[kind].Find(x => x.activeSelf == false);
        }

        if (obj == null)
        {
            obj = Instantiate(_notePrefabs[kind], _muzzle.position, _muzzle.rotation) as GameObject;
            obj.GetComponent<Bullet>().pHitData = pHitData;
            _noteList[kind].Add(obj);
        }
        else
        {
            obj.transform.position = _muzzle.transform.position;
            obj.transform.rotation = _muzzle.transform.rotation;
            obj.SetActive(true);
        }
    }

    void SpawnRandomNote()
    {
        SpawnNote(Random.Range(0, 2));
    }

    void SpawnBigNote(float fallSpeed = 0)
    {
        _bigNoteNum++;

        if (!_noteList.ContainsKey(2))
        {
            _noteList.Add(2, new List<GameObject>());
        }

        GameObject obj = null;

        if (_noteList[2].Count > 0)
        {
            obj = _noteList[2].Find(x => x.activeSelf == false);
        }

        Vector2 pos = new Vector2(Random.Range(_minTrans.position.x, _maxTrans.position.x), _spawnHeight);

        if (obj == null)
        {
            obj = Instantiate(_bigNotePrefab, pos, Quaternion.identity) as GameObject;
            //TODO : 오너 셋팅, 낙하스피드 셋팅.
            obj.GetComponent<BigNote>().Setting(this, fallSpeed, _bigNoteSelftDamage);

            _noteList[2].Add(obj);
        }
        else
        {
            obj.transform.position = pos;
            obj.SetActive(true);
        }

    }

    IEnumerator HitAnimation()
    {
        _soundPlayer.Play("Hit");
        IsSuperArmour = true;

        PlayAnimation("Hit");

        yield return new WaitForSeconds(1.0f);

        IsSuperArmour = false;

        PlayAnimation(State);
    }

    protected void StopHitAnimation()
    {
        StopAllCoroutines();
        IsSuperArmour = false;
    }

    protected override void HitEvent()
    {
        if(!IsSuperArmour && !IsInvincible)
            StartCoroutine(HitAnimation());

        if (CurrentHP < MaxHP * 0.4f && _PageNum == 2)
        {
            _PageNum = 3;
        }
        else if (CurrentHP < MaxHP * 0.7f && _PageNum == 1)
        {
            _PageNum = 2;
        }

    }

    public void BigNoteDead()
    {
        _bigNoteNum--;
        _bigNoteDeadCount++;

        int damage = (int)((float)MaxHP * _bigNoteSelftDamage);

        damage = Mathf.Clamp(damage, 1, MaxHP);

        CurrentHP -= damage;
    }

    public void NoteAllDead()
    {

        if ((!_noteList.ContainsKey(0) && !_noteList.ContainsKey(1)))
            return;


        foreach (GameObject note in _noteList[0])
        {
            if (note.activeSelf == true)
            {
                note.GetComponent<Bullet_Note>().gameObject.SetActive(false);
            }
        }
    }

    public void BigNoteAllDead()
    {

        if (!_noteList.ContainsKey(2) || _noteList[2] == null)
            return;

        _bigNoteNum = 0;
        _bigNoteDeadCount = 0;
        foreach (GameObject note in _noteList[2])
        {
            if (note.activeSelf == true)
            {
                note.GetComponent<BigNote>().IsAllDead = true;
                note.GetComponent<BigNote>().SetState("Dead");
            }
        }
    }

    class BumJoongInit : State
    {
        readonly BumJoong _boss;

        public BumJoongInit(BumJoong boss, string id) : base(id)
        {
            _boss = boss;
        }

        public override void Enter()
        {
            _boss.IsInvincible = true;
            _boss._collider.enabled = false;
        }

        public override void Excute()
        {
            if (_boss.IsDialogue)
            {
                _boss.SetState("Dialogue");
                return;
            }
        }

    }

    class DialogueState : State
    {
        readonly BumJoong _boss;

        public DialogueState(BumJoong boss, string id) : base(id)
        {
            _boss = boss;
        }

        public override void Enter()
        {
            _boss._dm.DisplayDialogue(_boss._dialogueID);
        }

        public override void Excute()
        {

            if (_boss._dm.GetIsEnd(_boss._dialogueID))
            {
                _boss.SetState("Appear");
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!_boss._dm.Displaying)
                    {
                        _boss._dm.NextContent();
                        return;
                    }
                    if (_boss._dm.Displaying && _boss._dm._useDelay)
                    {
                        _boss._dm.Displaying = false;
                    }
                }
            }
        }

    }

    class AppearState : State
    {
        readonly BumJoong _boss;

        float _delay = 0;

        public AppearState(BumJoong boss, string id) : base(id)
        {
            _boss = boss;
        }

        public override void Enter()
        {
            _boss.IsInvincible = true;
            _boss._collider.enabled = false;
            _boss.PlayAnimation("Attack1");
            _boss._sp.AppearDisplay(1.9f);
            Global.shared<SoundManager>().ChangeBGM("BossMode");
        }

        public override void Excute()
        {
            if (_delay >= 2.0f)
            {
                _delay = 0;
                _boss.SetState("Pattern_1");
            }
            else
            {
                _delay += Time.fixedDeltaTime;
            }
        }

        public override void Exit()
        {
            _boss.IsInvincible = false;
            _boss._collider.enabled = true;
            BossHPBar.Display(_boss);
        }

    }

    class Pattern_1State : State
    {
        readonly BumJoong _boss;

        int _num = 2;
        int _CurrentShootNum = 0;

        float _delay = -1.0f;

        bool _isTeleporting = false;

        RaycastHit2D _hit;

        public Pattern_1State(BumJoong boss, string id) : base(id)
        {
            _boss = boss;
        }

        public override void Enter()
        {
            _boss.IsSuperArmour = true;
            Buffer.SuperArmourAcitve(true);
            _boss.PlayAnimation("Attack0");
            _boss.LookTarget(_boss._player.transform);
            _boss._sp.Pattern1Display(1.0f);
        }

        public override void Excute()
        {
            if (_num > 0)
            {
                if (_CurrentShootNum < _boss._numShoot)
                {
                    if (_isTeleporting)
                    {
                        _isTeleporting = false;
                        _boss._animator.gameObject.SetActive(true);
                        Enter();
                    }

                    if (_delay >= _boss._shootDelay)
                    {
                        _delay = 0;
                        _CurrentShootNum++;
                        _boss.SpawnNote(0);
                    }
                    else
                    {
                        _delay += Time.fixedDeltaTime;
                    }
                }
                else
                {
                    if (!_isTeleporting)
                    {
                        _isTeleporting = true;
                        _boss.PlayAnimation("SuperMove");
                        _boss._teleportEffect.SetActive(true);
                        _num--;
                    }

                    if (_delay >= _boss._teleportDelay)
                    {
                        _hit = Physics2D.Raycast(_boss._player.transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

                        Vector3 pos = _hit.point;
                        float dir = (_boss._player.container.rotation.y == 0) ? -1 : 1;
                        pos.x += dir * 20;

                        _boss._animator.gameObject.SetActive(false);
                        _boss._teleportEffect.SetActive(false);
                        _boss.Teleport(pos);

                        if (_num <= 0)
                            _delay = 0;
                        else
                            _delay = -1.0f;
                        _CurrentShootNum = 0;
                    }
                    else
                    {
                        _delay += Time.fixedDeltaTime;
                    }
                }
            }
            else
            {
                if (!_isTeleporting)
                {
                    _isTeleporting = true;
                    _boss.PlayAnimation("SuperMove");
                    _boss._teleportEffect.SetActive(true);
                }

                if (_delay >= _boss._teleportDelay && _isTeleporting)
                {
                    Vector2 pos = _boss.transform.position;
                    pos.x = _boss._centerPos;
                    _boss.Teleport(pos);

                    _isTeleporting = false;
                    _boss._animator.gameObject.SetActive(true);
                    _boss._teleportEffect.SetActive(false);

                    _boss.SetState("Exhaustion");
                }
                else
                {
                    _delay += Time.fixedDeltaTime;
                }
                
            }
            
                                    
        }

        public override void Exit()
        {
            _num = 2;
            _CurrentShootNum = 0;
            _delay = -1.0f;
            _isTeleporting = false;
            _boss._animator.gameObject.SetActive(true);
            Buffer.SuperArmourAcitve(false);
            _boss.IsSuperArmour = false;
        }

    }

    class ExhaustionState : State
    {
        readonly BumJoong _boss;

        float _delay = 0;

        public ExhaustionState(BumJoong boss, string id) : base(id)
        {
            _boss = boss;
        }

        public override void Enter()
        {
            _boss.IsSuperArmour = false;

            _boss.PlayAnimation(GetID);
            Buffer.Exhaustion(true);
            _boss._sp.ExhaustionDisplay(1.0f);
        }

        public override void Excute()
        {
            if (_delay >= _boss._exhaustionTime)
            {
                switch (_boss._PageNum)
                {
                    case 1:
                        _boss.SetState("Pattern_1");
                        break;

                    case 2:
                        _boss.SetState("Pattern_2");
                        break;

                    case 3:
                        _boss.SetState("OnPattern_3");
                        break;
                }
            }
            else
            {
                _delay += Time.fixedDeltaTime;
            }

        }

        public override void Exit()
        {
            _delay = 0;
            Buffer.Exhaustion(false);
            _boss.StopHitAnimation();
        }

    }

    class Pattern_2IdleState : State
    {
        readonly BumJoong _boss;

        float _delay = 0;

        public Pattern_2IdleState(BumJoong boss, string id) : base(id)
        {
            _boss = boss;
        }

        public override void Enter()
        {
            _boss._pattern2Num--;
            _boss.PlayAnimation(GetID);
        }

        public override void Excute()
        {
            if (_delay >= 1.0f)
            {
                _delay = 0;
                if (_boss._pattern2Num > 0)
                {
                    if (_boss._PageNum == 3)
                        _boss.SetState("OnPattern_3");
                    else
                        _boss.SetState("Pattern_2");
                }
                else
                {
                    _boss._pattern2Num = 2;
                    if (_boss._PageNum == 3)
                        _boss.SetState("OnPattern_3");
                    else
                        _boss.SetState("Pattern_1");
                }
            }
            else
            {
                _delay += Time.fixedDeltaTime;
            }
        }

    }

    class Pattern_2State : State
    {
        readonly BumJoong _boss;

        float _delay = 0;

        bool _isHit = false;

        public Pattern_2State(BumJoong boss, string id) : base(id)
        {
            _boss = boss;
        }

        public override void Enter()
        {
            _boss.IsInvincible = true;
            _boss._collider.enabled = false;
            Buffer.InvincibleActive(true);
            Buffer.NoteActive(true);
            _boss.PlayAnimation("Attack1");
            _boss._sp.Pattern2Display(1.0f);
        }

        public override void Excute()
        {
            if (_boss._bigNoteNum < _boss._spawnNum)
            {

                if (_boss._bigNoteDeadCount < 3 && !_isHit)
                {
                    if (_delay >= _boss._spawnDelay)
                    {
                        _delay = 0;
                        _boss.SpawnBigNote();
                    }
                    else
                    {
                        _delay += Time.fixedDeltaTime;
                    }
                }
                else
                {
                    if (!_isHit)
                    {
                        _isHit = true;
                        _delay = 0;
                        _boss.BigNoteAllDead();
                        _boss.PlayAnimation("Hit");
                    }

                    if (_delay >= 1.0f)
                    {
                        _boss.SetState("Idle");
                    }
                    else
                    {
                        _delay += Time.fixedDeltaTime;
                    }

                }

            }
        }

        public override void Exit()
        {
            _isHit = false;
            _boss.IsInvincible = false;
            _boss._collider.enabled = true;
            Buffer.InvincibleActive(false);
            Buffer.NoteActive(false);
        }

    }

    class OnPattern_3State : State
    {
        readonly BumJoong _boss;

        Transform _moveObj;
        Vector3 _pos;

        float _delay = 0;

        bool _isTeleporting = false;

        public OnPattern_3State(BumJoong boss, string id) : base(id)
        {
            _boss = boss;
        }

        public override void Enter()
        {
            _boss._sp.OnPattern3Display(1.0f);
            _boss.IsInvincible = true;
            _boss._collider.enabled = false;
            Buffer.InvincibleActive(true);
            _moveObj = _boss._animator.transform;
            _pos = new Vector3(_moveObj.localPosition.x, _boss._height, _moveObj.localPosition.z);

            if (_boss._isCenter)
            {
                _boss.PlayAnimation("Attack_Tornado");
                _boss._tornadoAnim.gameObject.SetActive(true);
            }
            else
            {
                _isTeleporting = true;
                _boss.PlayAnimation("SuperMove");
                _boss._teleportEffect.SetActive(true);
            }
        }

        public override void Excute()
        {
            if (_boss._isCenter)
            {
                if (_isTeleporting)
                {
                    _isTeleporting = false;
                    _boss.PlayAnimation("Attack_Tornado");
                    _boss._tornadoAnim.gameObject.SetActive(false);
                }

                if (_moveObj.localPosition == _pos)
                {
                    _boss.SetState("Pattern_3");
                }
                else
                {
                    _moveObj.localPosition = Vector3.MoveTowards(_moveObj.localPosition, _pos, _boss._speed * Time.fixedDeltaTime);
                }
            }
            else
            {
                if (_delay >= _boss._teleportDelay)
                {
                    _delay = 0;
                    Vector2 pos = _boss.transform.position;
                    pos.x = _boss._centerPos;
                    _boss.Teleport(pos);
                }
                else
                {
                    _delay += Time.fixedDeltaTime;
                }
            }

        }

    }

    class Pattern_3State : State
    {
        readonly BumJoong _boss;

        float _delay = 0;

        public Pattern_3State(BumJoong boss, string id) : base(id)
        {
            _boss = boss;
        }

        public override void Enter()
        {
            _boss._tornadoBox.SetActive(true);
            _boss._sp.Pattern3Display(1.0f);
        }

        public override void Excute()
        {

            if (_boss._bigNoteNum < _boss._spawnNum)
            {
                if (_delay >= _boss._spawnDelay)
                {
                    _delay = 0;
                    _boss.SpawnBigNote(_boss._fallSpeed);
                }
                else
                {
                    _delay += Time.fixedDeltaTime;
                }
            }

        }

    }

    class BumJoong_DeadState : State
    {
        readonly BumJoong _boss;

        bool _isTornadoEnd = false;

        float _delay = 0;

        Vector3 _pos;
        Transform _moveObj;

        int _prevIndex = 0;

        public BumJoong_DeadState(BumJoong boss, string id) : base(id)
        {
            _boss = boss;
        }

        public override void Enter()
        {
            _pos = new Vector3(0, 9, 0);
            _moveObj = _boss._animator.transform;
            _boss.BigNoteAllDead();
            _boss.NoteAllDead();
            _boss._tornadoAnim.SetTrigger("End");
            _boss._tornadoBox.SetActive(false);
            _boss.IsInvincible = false;
            _boss._collider.enabled = false;
            Buffer.InvincibleActive(false);
            Buffer.SuperArmourAcitve(false);
            BossHPBar.Conceal();
            Global.shared<SoundManager>().ChangeBGM("Stage-001");

            _boss._dm.DisplayDialogue("End");

        }

        public override void Excute()
        {

            if (!_boss._dm.GetIsEnd("End"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!_boss._dm.Displaying)
                    {
                        _boss._dm.NextContent();
                        return;
                    }
                    if (_boss._dm.Displaying && _boss._dm._useDelay)
                    {
                        _boss._dm.Displaying = false;
                    }
                }
            }

            if (_isTornadoEnd)
            {

                if (_delay >= 2.0f)
                {
                    _delay = 0;

                    int animIndex;

                    do
                    {
                        animIndex = Random.Range(0, 4);
                    }
                    while (_prevIndex == animIndex);

                    _boss._sp.DeadDisplay(1.0f);

                    switch (animIndex)
                    {
                        case 0:
                            _boss.PlayAnimation("Idle");
                            break;

                        case 1:
                            _boss.PlayAnimation("Attack0");
                            break;

                        case 2:
                            _boss.PlayAnimation("Attack1");
                            break;

                        case 3:
                            _boss.PlayAnimation("Attack_Tornado");
                            break;
                    }

                }
                else
                {
                    _delay += Time.fixedDeltaTime;
                }

            }
            else
            {

                if (_moveObj.localPosition == _pos)
                {
                    _isTornadoEnd = true;
                    _boss._tornadoAnim.gameObject.SetActive(false);
                    _boss.PlayAnimation("Idle");
                }
                else
                {
                    _moveObj.localPosition = Vector3.MoveTowards(_moveObj.localPosition, _pos, _boss._speed * Time.fixedDeltaTime);
                }

            }
            
        }

    }

}
