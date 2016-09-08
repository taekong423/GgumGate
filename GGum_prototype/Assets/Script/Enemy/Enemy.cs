using UnityEngine;
using System.Collections;
using System.Reflection;

public class Enemy : AICharacter {



	// Use this for initialization
	void Awake () {
        _player = GameObject.FindObjectOfType<PlayerCharacter>();
        state = State.Idle;
	}

    void OnEnable()
    {
        StartCoroutine(InitState());
    }

    protected virtual IEnumerator InitState()
    {
        state = State.Idle;

        NextState();
        StartCoroutine(Search());

        yield return null;
    }

    protected virtual IEnumerator IdleState()
    {
        while (state == State.Idle)
        {

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

            yield return null;

        }

        NextState();

        yield return null;
    }

    protected virtual IEnumerator MoveState()
    {

        while (state == State.Move)
        {
            if (_target != null)
            {
                //도착.
                if (GoToTarget(_target.position))
                {
                    SetWayPointNum();
                    _target = _wayPoints[_numWayPoint];
                    state = State.Idle;
                }

            }

            yield return new WaitForFixedUpdate();
        }

        NextState();

        yield return null;
    }

    protected virtual IEnumerator HitState()
    {



        yield return null;
    }

    protected virtual IEnumerator DeadState()
    {



        yield return null;
    }

    protected virtual IEnumerator Search()
    {
        while (state != State.Dead)
        {
            if (state != State.Attack && state != State.Hit)
            {

                if (Search(_player.transform, _detectionRange))
                {
                    _currentMoveDleay = _moveDelay;
                    _target = _player.transform;
                    state = State.Move;
                }
                else
                {
                    if (_target == _player.transform)
                    {
                        if (_wayPoints.Length != 0)
                            _target = _wayPoints[_numWayPoint];
                    }
                }

            }

            yield return null;
        }


        yield return null;
    }

    protected void NextState()
    {
        string methodName = state.ToString() + "State";
        MethodInfo info = GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    /*void FixedUpdate()
    {

        if (Search(_player.transform, _detectionRange))
        {
            Debug.Log("AA");
            _currentMoveDleay = _moveDelay;
            _target = _player.transform;
            state = State.Move;
        }
        else
        {
            if (_target == _player.transform)
            {
                if (_wayPoints.Length != 0)
                    _target = _wayPoints[_numWayPoint];
            }
        }

        switch (state)
        {
            case State.Idle:

                if (_currentMoveDleay <= 0.0f)
                {
                    _currentMoveDleay = _moveDelay;
                    if(_wayPoints.Length != 0)
                        _target = _wayPoints[_numWayPoint];
                    state = State.Move;
                }
                else
                {
                    _currentMoveDleay -= Time.fixedDeltaTime;
                }
                
                break;

            case State.Move:

                if (_target != null)
                {
                    //도착.
                    
                    if(GoToTarget(_target.position))
                    {
                        SetWayPointNum();
                        _target = _wayPoints[_numWayPoint];
                        state = State.Idle;
                    }
                        
                }

                break;

            default:

                break;
        }
    }*/


}
