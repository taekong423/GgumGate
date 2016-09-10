using UnityEngine;
using System.Collections;

public class AICharacter : Character {

    int addNum = 1;

    [Header("AI Setting")]
    [SerializeField]
    protected float _detectionRange = 100.0f;

    protected float _attackRange = 20.0f;

    protected PlayerCharacter _player;
    protected Transform _target;

    protected int _numWayPoint;
    protected float _currentMoveDleay;


    public MoveType _moveType;
    public float _moveDelay;
    public Transform[] _wayPoints;

    public float DetectionRange { get { return _detectionRange; } set { _detectionRange = value; } }

    protected bool Search(Transform target, float detectionRange)
    {
        float dist = Vector3.Distance(target.position, transform.position);

        if (dist <= detectionRange)
            return true;
        else
            return false;
    }


    protected bool GoToTarget(Vector3 targetPoint)
    {

        float dist = Vector3.Distance(targetPoint, transform.position);

        if (dist <= 15.0f)
            return true;

        Debug.Log("Move");

        Vector3 dir = targetPoint - transform.position;
        float dirX = dir.x / Mathf.Abs(dir.x);

        Move(Axis.Horizontal, dirX);
        Flip(dirX);

        return false;
    }

    

    protected void SetWayPointNum()
    {
        Debug.Log("SetPoint");
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

}
