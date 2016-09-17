using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossPig : Enemy {

    
    CameraController _camera;

    int _patternNum = 0;

    int _childPigNum = 0;


    int _patterCount = 0;

    float _baseMoveSpeed;

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

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            HitData hitdata = other.GetComponent<Bullet>().pHitData;

            OnHit(hitdata);

            Destroy(other.gameObject);
        }
    }

    protected override void HitFunc()
    {
        if (state != State.Hit && !_isHitEffectDelay)
        {
            state = State.Hit;
            _isHitEffectDelay = true;
        }
    }


    protected override IEnumerator InitState()
    {
        

        yield return new WaitForSeconds(1.0f);

        if (_camera == null)
            _camera = Global.shared<CameraController>();

        _camera.ShakeCamera(true);

        yield return new WaitForSeconds(1.0f);

        _camera.ShakeCamera(false);

        yield return null;

        //animator.SetTrigger("Hide");
        m_collider.enabled = false;
        Physics2D.IgnoreCollision(_player.GetComponent<Collider2D>(), GetComponent<CircleCollider2D>(), true);

        state = State.Idle;

        yield return null;

        NextState();
        StartCoroutine(PatternUpdate());

    }

    protected override IEnumerator IdleState()
    {
        m_collider.enabled = true;

        while (state == State.Idle)
        {
            switch (_patternNum)
            {
                case 0:
                case 1:

                    if (_currentMoveDleay <= 0.0f)
                    {
                        _currentMoveDleay = _moveDelay;
                        if (_wayPoints.Length != 0)
                            _target = _wayPoints[_numWayPoint];

                        state = State.Move;
                    }
                    else
                    {
                        _currentMoveDleay -= Time.fixedDeltaTime;
                    }

                    break;

                default:

                    Debug.Log("cc");

                    if (_currentMoveDleay <= 0.0f)
                    {
                        _currentMoveDleay = 3;
                        if (_wayPoints.Length != 0)
                            _target = _wayPoints[_numWayPoint];

                        state = State.Move;
                    }
                    else
                    {
                        _currentMoveDleay -= Time.fixedDeltaTime;
                    }

                    break;
            }

            yield return null;
        }

        yield return null;

        NextState();
    }

    protected override IEnumerator MoveState()
    {
        bool arrive = false;

        float pattern1Delay = 2;

        if (_patternNum != 2)
        {
            animator.SetTrigger("Hide");
            m_collider.enabled = false;
        }
        else
            animator.SetTrigger("Rush");


        while (state == State.Move)
        {
            if(!arrive)
                arrive = GoToTarget(_target.position);

            switch (_patternNum)
            {
                case 0:

                    if (arrive)
                    {
                        SetWayPointNum();
                        _target = _wayPoints[_numWayPoint];

                        if (_childPigNum <= 1)
                        {
                            state = State.Attack;
                        }
                        else
                        {
                            state = State.Idle;
                        }
                    }

                    break;


                case 1:


                    if (pattern1Delay <= 0)
                    {
                        state = State.Attack;
                    }
                    else
                    {
                        pattern1Delay -= Time.deltaTime;
                    }

                    break;

                default:

                    if (arrive)
                    {
                        state = State.Idle;
                        SetWayPointNum();
                        _target = _wayPoints[_numWayPoint];

                        Vector3 dir = _player.transform.position - transform.position;
                        float dirX = dir.x / Mathf.Abs(dir.x);

                        Flip(dirX);
                        
                    }

                    break;

            }

            yield return null;
        }

        yield return null;


        NextState();
    }

    protected override IEnumerator AttackState()
    {
        if (state == State.Attack)
        {
            switch (_patternNum)
            {
                case 0:

                    StartCoroutine(Pattern0());

                    break;

                case 1:

                    StartCoroutine(Pattern1());
                    
                    break;
            }
        }

        yield return null;
    }

    protected override IEnumerator HitState()
    {
        animator.SetTrigger("Hit");

        while (state == State.Hit)
        {
            yield return null;
        }

        _isHitEffectDelay = false;

        yield return null;

        NextState();
    }

    protected override IEnumerator DeadState()
    {
        while (state == State.Dead)
        {
            yield return null;
        }

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
            }
            else if (CurrentHP < MaxHP * 0.5f && _patternNum == 0)
            {
                _patternNum = 1;
            }

            yield return null;
        }

        yield return null;
    }

    IEnumerator Pattern0()
    {
        _camera.ShakeCamera(true);

        yield return new WaitForSeconds(1.0f);

        _camera.ShakeCamera(false);

        yield return null;

        Vector3 dir = _player.transform.position - transform.position;
        float dirX = dir.x / Mathf.Abs(dir.x);

        Flip(dirX);

        animator.SetTrigger("Attack");
        m_collider.enabled = true;
        //Physics2D.IgnoreCollision(_player.GetComponent<Collider2D>(), GetComponent<CircleCollider2D>(), false);
        SpawNormalPig();

        yield return new WaitForSeconds(2.0f);

        _camera.ShakeCamera(true);

        yield return new WaitForSeconds(1.0f);

        _camera.ShakeCamera(false);
        animator.SetTrigger("Hide");
        m_collider.enabled = false;
        state = State.Idle;

        yield return null;

        NextState();
    }

    IEnumerator Pattern1()
    {
        _camera.ShakeCamera(true);

        yield return new WaitForSeconds(0.5f);

        _camera.ShakeCamera(false);

        animator.SetTrigger("Hide2");

        yield return new WaitForSeconds(2.0f);


        transform.position = new Vector3(_player.transform.position.x, transform.position.y, transform.position.z);

        _camera.ShakeCamera(true);

        yield return new WaitForSeconds(0.5f);

        _camera.ShakeCamera(false);


        //idle
        animator.SetTrigger("Attack");
        m_collider.enabled = true;

        state = State.Idle;

        _patterCount++;

        SetWayPointNum();
        _target = _wayPoints[_numWayPoint];

        if (_patterCount >= 4)
        {
            _patterCount = 0;
            RandomSpawnPig();
        }

        yield return new WaitForSeconds(1.0f);

        NextState();
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
}
