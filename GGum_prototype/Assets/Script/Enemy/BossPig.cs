using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public partial class BossPig : Enemy, IEnemy {

    CameraController _camera;

    int _numChild = 0;
    int _numPattern = 0;


    float _baseMoveSpeed;

    Vector3 _baseSize;

    bool _isHide = false;

    GameObject _currentOrora;

    List<GameObject> _normalPigList;
    List<GameObject> _explosionPigList;

    [Header("BossPig Object Setting")]
    public Transform _centerPivot;

    public GameObject _normalPig;
    public GameObject _explosionPig;

    public GameObject[] _ororas;

    public GameObject _warringImg;

    public int NumChild { get { return _numChild; } set { _numChild = value; } }

    public int CurrentHP { get { return currentHP; } set { currentHP = value; } }
    public int MaxHP { get { return maxHP; } set { maxHP = value; } }

    public string GetID { get { return id; } }

    public void Active(bool active)
    {
        gameObject.SetActive(active);
    }

    protected override void InitCharacter()
    {
        base.InitCharacter();

        isInvincible = true;
        _baseMoveSpeed = moveSpeed;
        _baseSize = _transform.localScale;

        _normalPigList = new List<GameObject>();
        _explosionPigList = new List<GameObject>();
        m_collider = GetComponent<BoxCollider2D>();

        _camera = Camera.main.GetComponent<CameraController>();

        _statePatternList.Add(typeof(InitState), new BossPig_InitState(this));
        _statePatternList.Add(typeof(IdleState), new BossPig_IdleState(this));
        _statePatternList.Add(typeof(MoveState), new BossPig_MoveState(this));
        _statePatternList.Add(typeof(HitState), new BossPig_HitState(this, 0.5f));
        _statePatternList.Add(typeof(DeadState), new BossPig_DeadState(this));
        _statePatternList.Add(typeof(AttackState), new BossPig_Attack0State(this));
        _statePatternList.Add(typeof(BossPig_Attack1State), new BossPig_Attack1State(this));

        SetStatePattern<InitState>();

    }


    IEnumerator Hide()
    {
        _camera.ShakeCamera(1.0f);

        yield return new WaitForSeconds(0.5f);

        _isHide = true;
        isInvincible = true;
        m_collider.enabled = false;
        OroraActive(false);
        animator.SetTrigger("Hide");

        yield return new WaitForSeconds(0.5f);
    }

    bool HasPigs(string pigName)
    {
        switch (pigName)
        {
            case "NormalPig":

                if (_normalPig != null && _normalPigList.Count > 0)
                    return true;
                break;

            case "ExplosionPig":

                if (_normalPig != null && _explosionPigList.Count > 0)
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
                    return _normalPigList.Find(x => x.activeSelf == false);
                break;

            case "ExplosionPig":

                if (HasPigs("ExplosionPig"))
                    return _explosionPigList.Find(x => x.activeSelf == false);
                break;
        }

        return null;
    }

    void SetOrora(int num)
    {
        _currentOrora = _ororas[num];
    }

    void OroraActive(bool active)
    {
        _currentOrora.SetActive(active);
    }

    void SpawNormalPig()
    {
        for (int i = 0; i < 4; i++)
        {

            GameObject pig = GetPigs("NormalPig");

            if (pig == null)
            {
                GameObject obj = Instantiate(_normalPig, transform.position, Quaternion.identity) as GameObject;
                obj.GetComponent<NormalPig>().Setting(this, _wayPoints, _centerPivot);
                _normalPigList.Add(obj);
                obj.SetActive(true);
            }
            else
            {
                pig.transform.position = transform.position;
                pig.SetActive(true);
            }

            NumChild++;
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
                        obj.GetComponent<NormalPig>().Setting(this, _wayPoints, _centerPivot);
                        _normalPigList.Add(obj);
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
                        _explosionPigList.Add(obj);
                        obj.SetActive(true);
                    }
                    else
                    {
                        pig.SetActive(true);
                    }

                    break;

            }

            NumChild++;
        }

    }

    void ChildAllDead()
    {
        foreach (GameObject pig in _normalPigList)
        {
            if (pig.activeSelf == true)
                pig.GetComponent<Enemy>()._statePattern.SetState("Dead");
        }

        foreach (GameObject pig in _explosionPigList)
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
                SetStatePattern<HitState>();
            }

        }
        else
            OnHit(hitdata);
    }

    protected override void HitFunc()
    {
        if (_numPattern == 0 && currentHP <= maxHP * 0.5f)
        {
            OroraActive(false);
            _numPattern++;
            moveSpeed *= 6;
            _moveDelay = 3;
            SetOrora(_numPattern);
            _transform.localScale -= _baseSize * 0.3f;
            StopAllCoroutines();
            SetStatePattern<BossPig_Attack1State>();
            _statePattern.StartState();
        }

    }
}
