using UnityEngine;
using System.Collections;

public class Enemy : AICharacter {



	// Use this for initialization
	void Awake () {
        _player = GameObject.FindObjectOfType<PlayerCharacter>();
	}

    void FixedUpdate()
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
    }


}
