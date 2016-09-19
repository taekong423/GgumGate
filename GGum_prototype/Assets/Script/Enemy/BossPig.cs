using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossPig : Enemy {

    
    CameraController _camera;

    int _patternNum = 0;

    int _childPigNum = 0;


    int _patterCount = 0;

    float _baseMoveSpeed;

    bool _isHide = false;

    Vector3 _baseSize;

    List<GameObject> _normalPigs;
    List<GameObject> _explosionPigs;

    public GameObject _normalPig;
    public GameObject _explosionPig;

    
    public int ChildPigNum { get { return _childPigNum; } set { _childPigNum = value; } }

    protected override void InitCharacter()
    {
        base.InitCharacter();
        if(Global.has_shared< CameraController>())
            _camera = Global.shared<CameraController>();
        _player = GameObject.FindObjectOfType<PlayerCharacter>();
        _normalPigs = new List<GameObject>();
        _explosionPigs = new List<GameObject>();
        m_collider = GetComponent<BoxCollider2D>();

        _baseMoveSpeed = MoveSpeed;

        isInvincible = true;
        Physics2D.IgnoreCollision(_player.GetComponent<Collider2D>(), GetComponent<CircleCollider2D>(), true);

        _baseSize = transform.localScale;
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

    protected override IEnumerator InitState()
    {
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
                    Hide(true, 0, 1.0f);
                    yield return new WaitForSeconds(1.0f);
                }

                break;

            case 2:

                if (_isHide)
                    Hide(false);

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
                    Hide(true, 0, 1.0f);
                    yield return new WaitForSeconds(1.0f);
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

    protected override IEnumerator AttackState()
    {
        Debug.Log("Attack");

        float stayTime = 0.0f;

        switch (_patternNum)
        {
            case 0:

                stayTime = 4.0f;
                StartCoroutine(Pattern0());
                
                break;

            case 1:

                stayTime = 3;
                StartCoroutine(Pattern1());

                break;

            case 2:

                break;
        }

        while (state == State.Attack)
        {
            if (stayTime <= 0)
            {
                state = State.Idle;
            }
            else
                stayTime -= Time.deltaTime;

            yield return null;
        }

        yield return null;

        NextState();
    }

    protected override IEnumerator HitState()
    {
        animator.SetTrigger("Hit");

        yield return new WaitForSeconds(1.5f);

        Hide(true);

        yield return new WaitForSeconds(1.0f);

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
            if (CurrentHP <= MaxHP * 0.15f && _patternNum == 1)
            {
                _patternNum = 2;
                MoveSpeed *= 3;
                transform.localScale -= _baseSize * 0.15f;
            }
            else if (CurrentHP < MaxHP * 0.5f && _patternNum == 0)
            {
                _patternNum = 1;
                transform.localScale -= _baseSize * 0.15f;
            }

            yield return null;
        }
    }


    IEnumerator Pattern0()
    {
        Hide(false);

        yield return new WaitForSeconds(1.0f);

        Vector3 dir = _player.transform.position - transform.position;
        float dirX = dir.x / Mathf.Abs(dir.x);

        Flip(dirX);

        animator.SetTrigger("Attack");

        SpawNormalPig();

        yield return null;
    }

    IEnumerator Pattern1()
    {
        Hide(true, 1);

        yield return new WaitForSeconds(1.5f);

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

    void SpawNormalPig()
    {
        for (int i = 0; i < 4; i++)
        {

            GameObject pig = GetPigs("NormalPig");

            if (pig == null)
            {
                GameObject obj = Instantiate(_normalPig, transform.position, Quaternion.identity) as GameObject;
                obj.GetComponent<NormalPig>().Setting(this, _wayPoints);
                _normalPigs.Add(obj);
            }
            else
            {
                pig.SetActive(true);
            }

            _childPigNum++;
        }
    }

    void RandomSpawnPig()
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
                        obj.GetComponent<NormalPig>().Setting(this, _wayPoints);
                        _normalPigs.Add(obj);
                    }
                    else
                    {
                        pig.SetActive(true);
                    }

                    break;

                case 1:

                    pig = GetPigs("ExplosionPig");

                    if (pig == null)
                    {
                        GameObject obj = Instantiate(_explosionPig, transform.position, Quaternion.identity) as GameObject;
                        obj.GetComponent<NormalPig>().Setting(this, _wayPoints);
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

    void Hide(bool isHide, int kind = 0, float shackeTime = 1.0f)
    {
        _camera.ShakeCamera(shackeTime);

        _isHide = isHide;

        if (isHide)
        {
            isInvincible = true;
            m_collider.enabled = false;

            if (kind == 0)
                animator.SetTrigger("Hide");
            else
                animator.SetTrigger("Hide2");
            
        }
        else
        {
            m_collider.enabled = true;
            isInvincible = false;
        }

    }

    void ChildAllDead()
    {
        foreach (GameObject pig in _normalPigs)
        {
            if (pig.activeSelf == true)
                pig.GetComponent<Enemy>().CurrentState = State.Dead;
        }

        foreach (GameObject pig in _explosionPigs)
        {
            if (pig.activeSelf == true)
                pig.GetComponent<Enemy>().CurrentState = State.Dead;
        }
    }

    public void ChildOnHit(HitData hitdata)
    {
        if (_isHide)
        {
            isInvincible = false;
            OnHit(hitdata);
            isInvincible = true;
        }
        else
            OnHit(hitdata);
    }
}
