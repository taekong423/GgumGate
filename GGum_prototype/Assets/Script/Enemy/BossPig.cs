using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class BossPig : Enemy {

    
    CameraController _camera;

    int _patternNum = 0;

    int _childPigNum = 0;


    int _patterCount = 0;

    float _baseMoveSpeed;

    public bool _isHide = false;

    Vector3 _baseSize;

    List<GameObject> _normalPigs;
    List<GameObject> _explosionPigs;

    public GameObject _normalPig;
    public GameObject _explosionPig;

    public Transform _centerPivot;
    
    public int ChildPigNum { get { return _childPigNum; } set { _childPigNum = value; } }

    protected override void InitCharacter()
    {
        base.InitCharacter();
        
        _player = GameObject.FindObjectOfType<PlayerCharacter>();
        _normalPigs = new List<GameObject>();
        _explosionPigs = new List<GameObject>();
        m_collider = GetComponent<BoxCollider2D>();

        _baseMoveSpeed = moveSpeed;

        isInvincible = true;
        Physics2D.IgnoreCollision(_player.GetComponent<Collider2D>(), GetComponent<CircleCollider2D>(), true);

        _baseSize = transform.localScale;

        _statePatternList.Add(typeof(Pattern0), new Pattern0(this));
        _statePatternList.Add(typeof(Pattern1), new Pattern1(this));
        _statePatternList.Add(typeof(Pattern2), new Pattern2(this));

        currentHP = 3;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            if (isInvincible)
                return;

            HitData hitdata = other.GetComponent<Bullet>().pHitData;
            OnHit(hitdata);

            Destroy(other.gameObject);
        }
    }

    public override void SetStatePattern()
    {
        _statePattern = _statePatternList[typeof(Pattern0)] as Pattern0;
    }

    protected override IEnumerator InitState()
    {
        yield return null;
        
        state = State.Idle;

        if(_camera == null)
            _camera = Global.shared<CameraController>();

        yield return null;

        NextState();
        StartCoroutine(PatternUpdate());
    }

    protected override IEnumerator IdleState()
    {
        switch (_patternNum)
        {
            case 0:
            case 1:

                if (!_isHide)
                {
                    _camera.ShakeCamera(1.0f);

                    yield return new WaitForSeconds(1.0f);

                    Hide(true, 0);
                }

                break;

            case 2:

                if (_isHide)
                {
                    _camera.ShakeCamera(1.0f);

                    yield return new WaitForSeconds(1.0f);

                    Hide(false);
                }

                _currentMoveDleay = 3;
                animator.SetTrigger("Rush");

                break;

            default:

                break;
        }

        while (state == State.Idle)
        {
            if (_currentMoveDleay <= 0)
            {

                switch (_patternNum)
                {
                    case 0:
                    case 1:
                    case 2:

                        state = State.Move;
                        _currentMoveDleay = _moveDelay;
                        _target = _wayPoints[_numWayPoint];

                        break;

                    default:

                        break;
                }

            }
            else
            {
                _currentMoveDleay -= Time.deltaTime;
            }

            yield return null;
        }

        yield return null;

        NextState();

    }

    protected override IEnumerator MoveState()
    {

        switch (_patternNum)
        {
            case 0:
            case 1:

                if (!_isHide)
                {
                    _camera.ShakeCamera(1.0f);

                    yield return new WaitForSeconds(1.0f);

                    Hide(true, 0);
                    
                }

                break;

            case 2:

                if (_isHide)
                    Hide(false);

                animator.SetTrigger("Rush");

                break;

            default:

                break;
        }

        bool arrive = false;

        float patterDelay = 2;

        while (state == State.Move)
        {
            if(_target != null && !arrive)
                arrive = GoToTarget(_target.position);

            switch (_patternNum)
            {
                case 0:

                    if (arrive)
                    {
                        SetWayPointNum();

                        if (_childPigNum <= 1)
                            state = State.Attack;
                        else
                            state = State.Idle;

                    }

                    break;

                case 1:

                    if (patterDelay <= 0)
                    {
                        state = State.Attack;
                        patterDelay = 2;
                    }
                    else
                        patterDelay -= Time.fixedDeltaTime;

                    break;

                case 2:

                    if (arrive)
                    {
                        state = State.Idle;
                        SetWayPointNum();

                        Vector3 dir = _player.transform.position - transform.position;
                        float dirX = dir.x / Mathf.Abs(dir.x);

                        Flip(dirX);
                    }

                    break;

            }

            yield return new WaitForFixedUpdate();
        }

        yield return null;

        NextState();

    }

    protected override void HitFunc()
    {
        _statePattern.HitFunc();
    }

    protected override IEnumerator AttackState()
    {
        return base.AttackState();
    }

    void Update()
    {
        if (_statePattern is Pattern0)
        {
            Debug.Log("Pattern0");
        }

        if (_statePattern is Pattern1)
        {
            Debug.Log("Pattern1");
        }

        if (_statePattern is Pattern2)
        {
            Debug.Log("Pattern2");
        }
    }

    //protected override IEnumerator AttackState()
    //{
    //    Debug.Log("Attack");

    //    float stayTime = 0.0f;

    //    switch (_patternNum)
    //    {
    //        case 0:

    //            stayTime = 5.0f;
    //            StartCoroutine(Pattern10());

    //            break;

    //        case 1:

    //            stayTime = 4.0f;
    //            StartCoroutine(Pattern11());

    //            break;

    //        case 2:

    //            break;
    //    }

    //    while (state == State.Attack)
    //    {
    //        if (stayTime <= 0)
    //        {
    //            state = State.Idle;
    //        }
    //        else
    //            stayTime -= Time.deltaTime;

    //        yield return null;
    //    }

    //    yield return null;

    //    NextState();
    //}

    protected override IEnumerator HitState()
    {

        animator.SetTrigger("Hit");

        Hide(false);

        yield return new WaitForSeconds(0.5f);

        _camera.ShakeCamera(1.0f);

        yield return new WaitForSeconds(1.0f);

        Hide(true);

        yield return new WaitForSeconds(0.5f);

        state = State.Idle;

        yield return null;

        NextState();
    }

    protected override IEnumerator DeadState()
    {
        ChildAllDead();

        animator.SetTrigger("Hit");

        yield return new WaitForSeconds(0.5f);

        Hide(true);

        yield return new WaitForSeconds(1.0f);

        gameObject.SetActive(false);

        yield return null;
    }

    IEnumerator PatternUpdate()
    {
        while (state != State.Dead)
        {
            if (currentHP <= maxHP * 0.15f && _patternNum == 1)
            {
                _patternNum = 2;
                moveSpeed *= 3;
                transform.localScale -= _baseSize * 0.15f;
            }
            else if (currentHP < maxHP * 0.5f && _patternNum == 0)
            {
                _patternNum = 1;
                transform.localScale -= _baseSize * 0.15f;
            }

            yield return null;
        }
    }


    IEnumerator Pattern10()
    {
        _camera.ShakeCamera(1.0f);

        yield return new WaitForSeconds(1.0f);

        Hide(false);

        yield return new WaitForSeconds(1.0f);

        Vector3 dir = _player.transform.position - transform.position;
        float dirX = dir.x / Mathf.Abs(dir.x);

        Flip(dirX);

        animator.SetTrigger("Attack");

        SpawNormalPig();

        yield return null;
    }

    IEnumerator Pattern11()
    {
        Hide(true, 1);

        yield return new WaitForSeconds(2.0f);

        _camera.ShakeCamera(1.0f);

        yield return new WaitForSeconds(0.5f);

        transform.position = _player.transform.position;

        yield return new WaitForSeconds(0.5f);

        Hide(false);

        animator.SetTrigger("Attack");

        _patterCount++;

        SetWayPointNum();
        _target = _wayPoints[_numWayPoint];

        if (_patterCount >= 4)
        {
            _patterCount = 0;
            RandomSpawnPig();
        }

        yield return null;
    }

    bool HasPigs(string pigName)
    {
        switch (pigName)
        {
            case "NormalPig":

                if (_normalPig !=null && _normalPigs.Count > 0)
                    return true;
                break;

            case "ExplosionPig":

                if (_normalPig != null && _explosionPigs.Count > 0)
                    return true;
                break;
        }

        return false;
    }

    GameObject GetPigs(string pigName)
    {
        switch (pigName)
        {
            case "NormalPig":

                if (HasPigs("NormalPig"))
                    return _normalPigs.Find(x => x.activeSelf == false);
                break;

            case "ExplosionPig":

                if (HasPigs("ExplosionPig"))
                    return _explosionPigs.Find(x => x.activeSelf == false);
                break;
        }

        return null;
    }

    public void SpawNormalPig()
    {
        for (int i = 0; i < 4; i++)
        {

            GameObject pig = GetPigs("NormalPig");

            if (pig == null)
            {
                GameObject obj = Instantiate(_normalPig, transform.position, Quaternion.identity) as GameObject;
                obj.GetComponent<NormalPig>().Setting(this, _wayPoints, _centerPivot);
                _normalPigs.Add(obj);
            }
            else
            {
                pig.transform.position = transform.position;
                pig.SetActive(true);
            }

            _childPigNum++;
        }
    }

    public void RandomSpawnPig()
    {
        int pigkind;

        for (int i = 0; i < 4; i++)
        {
            pigkind = Random.Range(0, 2);

            GameObject pig;

            switch (pigkind)
            {
                case 0:

                    pig = GetPigs("NormalPig");

                    if (pig == null)
                    {
                        GameObject obj = Instantiate(_normalPig, transform.position, Quaternion.identity) as GameObject;
                        obj.GetComponent<NormalPig>().Setting(this, _wayPoints, _centerPivot);
                        _normalPigs.Add(obj);
                    }
                    else
                    {
                        pig.transform.position = transform.position;
                        pig.SetActive(true);
                    }

                    break;

                case 1:

                    pig = GetPigs("ExplosionPig");

                    if (pig == null)
                    {
                        GameObject obj = Instantiate(_explosionPig, transform.position, Quaternion.identity) as GameObject;
                        obj.GetComponent<NormalPig>().Setting(this, _wayPoints, _centerPivot);
                        _explosionPigs.Add(obj);
                    }
                    else
                    {
                        pig.SetActive(true);
                    }

                    break;

            }

            _childPigNum++;
        }
        
    }

    public void Hide(bool isHide, int kind = 0)
    {
        isInvincible = isHide;
        m_collider.enabled = !isHide;
        _isHide = isHide;

        if (isHide)
        {
            if (kind == 0)
                animator.SetTrigger("Hide");
            else
                animator.SetTrigger("Hide2");
            
        }

    }

    public void ChildAllDead()
    {
        foreach (GameObject pig in _normalPigs)
        {
            if (pig.activeSelf == true)
                pig.GetComponent<Enemy>().state = State.Dead;
        }

        foreach (GameObject pig in _explosionPigs)
        {
            if (pig.activeSelf == true)
                pig.GetComponent<Enemy>().state = State.Dead;
        }
    }

    public void ChildOnHit(HitData hitdata)
    {
        if (_isHide)
        {
            Debug.Log("BossOnHIt");
            OnHit(hitdata);
            if (_statePattern._currentState == "Idle" || _statePattern._currentState == "Move")
            {
                Debug.Log("BossHit");
                _statePattern.SetState("Hit");
            }
            
        }
        else
            OnHit(hitdata);
    }
}
