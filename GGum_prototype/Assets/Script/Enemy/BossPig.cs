using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class BossPig : Enemy {

    
    CameraController _camera;

    int _childPigNum = 0;

    float _baseMoveSpeed;

    public bool _isHide = false;

    Vector3 _baseSize;

    List<GameObject> _normalPigs;
    List<GameObject> _explosionPigs;

    public GameObject _normalPig;
    public GameObject _explosionPig;

    public Transform _centerPivot;
    
    public int ChildPigNum { get { return _childPigNum; } set { _childPigNum = value; } }

    public string tt;

    void Update()
    {
        tt = _statePattern._currentState;
    }

    protected override void InitCharacter()
    {
        base.InitCharacter();
        
        _player = GameObject.FindObjectOfType<Player>();
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

        currentHP = 5;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            if (isInvincible)
                return;

            HitData hitdata = other.GetComponent<Bullet>().pHitData;

            if (hitdata.attacker.tag == "Enemy")
                return;

            OnHit(hitdata);

            Destroy(other.gameObject);
        }
    }

    public override void SetStatePattern()
    {
        _statePattern = _statePatternList[typeof(Pattern0)] as Pattern0;
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
                pig.GetComponent<Enemy>()._statePattern.SetState("Dead");
        }

        foreach (GameObject pig in _explosionPigs)
        {
            if (pig.activeSelf == true)
                pig.GetComponent<Enemy>()._statePattern.SetState("Dead");
        }
    }

    public void ChildOnHit(HitData hitdata)
    {
        Debug.Log("Damage : " + hitdata.damage);

        isInvincible = false;

        if (_isHide)
        {
            OnHit(hitdata);
            if (_statePattern._currentState == "Idle" || _statePattern._currentState == "Move")
            {
                _statePattern.SetState("Hit");
            }
            
        }
        else
            OnHit(hitdata);
    }
}
