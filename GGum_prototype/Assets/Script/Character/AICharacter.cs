using UnityEngine;
using System.Collections;

public class AICharacter : Character {

    int addNum = 1;

    protected HitData _hitData;

    [Header("AI Setting")]
    public float _detectionRange = 100.0f;

    public float _attackRange = 20.0f;

    public float _attackDelay = 1.0f;

    //[HideInInspector]
    public float _currentAttackDelay;

    [HideInInspector]
    public Player _player;
    [HideInInspector]
    public Transform _target;    
    [HideInInspector]
    public int _numWayPoint;
    [HideInInspector]
    public float _currentMoveDleay;


    public MoveType _moveType;
    public float _moveDelay;
    public Transform[] _wayPoints;

    public HitData pHitData { get { return _hitData; } set { _hitData = value; } }

    //public float DetectionRange { get { return _detectionRange; } set { _detectionRange = value; } }

    protected override void InitCharacter()
    {
        base.InitCharacter();
        _player = GameObject.FindObjectOfType<Player>();
    }

    public virtual void SetStatePattern<T>() where T : StatePattern
    {
        if(_statePatternList.ContainsKey(typeof(T)))
            _statePattern = _statePatternList[typeof(T)] as T;
    }

    public bool Search(Transform target, float detectionRange)
    {
        float dist = Vector3.Distance(target.position, transform.position);

        if (dist <= detectionRange)
            return true;
        else
            return false;
    }


    public bool GoToTarget(Vector3 targetPoint)
    {

        float dist = Vector3.Distance(targetPoint, transform.position);

        if (dist <= 15.0f)
            return true;

        Vector3 dir = targetPoint - transform.position;
        float dirX = dir.x / Mathf.Abs(dir.x);

        Move(Axis.Horizontal, dirX);
        Flip(dirX);

        return false;
    }

    

    public void SetWayPointNum()
    {
        switch (_moveType)
        {
            case MoveType.Once:
                break;

            case MoveType.PingPong:

                if (addNum > 0 && _numWayPoint >= _wayPoints.Length - 1)
                {
                    addNum = -1;
                }
                else if (addNum < 0 && _numWayPoint <= 0)
                {
                    addNum = 1;
                }

                _numWayPoint += addNum;

                break;
        }
    }

    public void OnAttack()
    {
        Attack(pHitData);
    }

}
