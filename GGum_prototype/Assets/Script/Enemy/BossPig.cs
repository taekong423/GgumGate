using UnityEngine;
using UnityEngine.UI;
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

    GameObject _currentOrora;

    public GameObject _normalPig;
    public GameObject _explosionPig;

    public Transform _centerPivot;
        
    public GameObject[] _ororas;
    public Image _hpBar;

    public GameObject _WarringImg;

    public int ChildPigNum { get { return _childPigNum; } set { _childPigNum = value; } }

    protected override void InitCharacter()
    {
        base.InitCharacter();

        isInvincible = true;
        _baseMoveSpeed = moveSpeed;
        _baseSize = transform.localScale;

        _normalPigs = new List<GameObject>();
        _explosionPigs = new List<GameObject>();
        m_collider = GetComponent<BoxCollider2D>();

        _statePatternList.Add(typeof(Pattern0), new Pattern0(this));
        _statePatternList.Add(typeof(Pattern1), new Pattern1(this));
        _statePatternList.Add(typeof(Pattern2), new Pattern2(this));

        SetStatePattern<Pattern0>();
    }

    //void Update()
    //{
    //    if (_hpBar != null)
    //    {
    //        if (_hpBar.transform.parent.gameObject.activeSelf)
    //        {
    //            _hpBar.fillAmount = (float)currentHP / (float)maxHP;
    //        }
    //    }
    //}

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

    public void SetOrora(int num)
    {
        _currentOrora = _ororas[num];
    }

    public void OroraActive(bool active)
    {
        _currentOrora.SetActive(active);
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
                obj.SetActive(true);
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
                        obj.SetActive(true);
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
                        obj.SetActive(true);
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
        isInvincible = false;

        if (_isHide)
        {
            OnHit(hitdata);
            if (_statePattern.CurrentState == "Idle" || _statePattern.CurrentState == "Move")
            {
                _statePattern.SetState("Hit");
            }
            
        }
        else
            OnHit(hitdata);
    }

    protected override void HitFunc()
    {
        _statePattern.HitFunc();
    }
}
