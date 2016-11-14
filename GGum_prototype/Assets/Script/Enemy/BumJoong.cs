using UnityEngine;
using System.Collections.Generic;

public class BumJoong : NewEnemy {

    Dictionary<int, List<GameObject>> _noteList;

    [Header("BumJoong Setting")]
    public float _minX;
    public float _maxX;
    public float _teleportDelay = 0.5f;

    public GameObject _teleportEffect;

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

    void OnEnable()
    {
        SetState("Pattern_1");
    }

    protected override void Init()
    {
        base.Init();

        _noteList = new Dictionary<int, List<GameObject>>();

    }

    protected override void SetStateList()
    {
        _stateList.Add("Pattern_1", new Pattern_1State(this, "Pattern_1"));
        _stateList.Add("Exhaustion", new ExhaustionState(this, "Exhaustion"));
        _stateList.Add("Pattern_2", new Pattern_2State(this, "Pattern_2"));
    }

    void Teleport(Vector2 point)
    {
        point.x = Mathf.Clamp(point.x, _minX, _maxX);

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
            obj.SetActive(true);
        }
    }

    void SpawnRandomNote()
    {
        SpawnNote(Random.Range(0, 2));
    }

    void SpawnBigNote()
    {
        if (_noteList.ContainsKey(2))
        {
            _noteList.Add(2, new List<GameObject>());
        }

        GameObject obj = null;

        if (_noteList[2].Count > 0)
        {
            obj = _noteList[2].Find(x => x.activeSelf == false);
        }

        if (obj == null)
        {
            Vector2 pos = new Vector2(Random.Range(_minX, _maxX), _spawnHeight);
            obj = Instantiate(_bigNotePrefab, pos, Quaternion.identity) as GameObject;
            //TODO : 오너 셋팅, 낙하스피드 셋팅.

            _noteList[2].Add(obj);
        }
        else
        {
            obj.SetActive(true);
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
            _boss.PlayAnimation("Attack0");
            _boss.LookTarget(_boss._player.transform);
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
                    float center = (_boss._minX + _boss._maxX) * 0.5f;
                    Vector2 pos = _boss.transform.position;
                    pos.x = center;
                    _boss.Teleport(pos);

                    _isTeleporting = false;
                    _boss._animator.gameObject.SetActive(true);

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
            _boss.PlayAnimation("Idle");
            //Buffer.Exhaustion(true);
        }

        public override void Excute()
        {
            if (_delay >= _boss._exhaustionTime)
            {
                _boss.SetState("Pattern_2");
            }
            else
            {
                _delay += Time.fixedDeltaTime;
            }

        }

        public override void Exit()
        {
            _delay = 0;
            //Buffer.Exhaustion(false);
        }

    }

    class Pattern_2State : State
    {
        readonly BumJoong _boss;

        public Pattern_2State(BumJoong boss, string id) : base(id)
        {
            _boss = boss;
        }

        public override void Enter()
        {
            _boss.PlayAnimation("Attack1");
        }

    }

}
